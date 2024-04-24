using Microsoft.EntityFrameworkCore;
using QE.Context;
using QE.Models.DTO;
using QE.Sockets;
using QE.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace QE.Models
{
    public partial class Main
    {
        private EqContext _context;
        private MainWindow _window;
        private TerminalDto _terminalDto;
        private ColorDto _colorDto;
        private SettingDto _settingDto;
        private List<SchedulesDto> _schedulesDto;
        private bool _prevState = false;
        private bool _nextState = false;
        private bool _isActiveWorkTime = false;
        private bool _isActiveStart = true;
        public Main(MainWindow window)
        {
           _context = new EqContext();
           _window = window;
           _terminalDto = new TerminalDto();
           _colorDto = new ColorDto();
           _settingDto = new SettingDto();
           _schedulesDto  = new List<SchedulesDto>();
        }
        private async Task InitButtons(Grid panel, List<string>? headers = null, long? preRecordId = null, long? priorityId = null)
        {
            try
            {
                List<ButtonDto> buttons = new List<ButtonDto>();

                var btnList = await _context.SOfficeTerminalButtons
                    .AsNoTracking()
                    .Where(s => s.SOfficeTerminal.IpAddress == _terminalDto.Ip&&s.IsActive)
                    .Select(s => new TerminalButtonsDto
                    {
                        Id = s.Id,
                        ParentId = s.ParentId,
                        ButtonType = s.ButtonType,
                        Name = s.ButtonName,
                        ServiceId = s.SServiceId,
                        ServiceName = s.SServiceId.HasValue ? s.SService.ServiceName : string.Empty,
                        SortId = s.SortId,
                    })
                    .ToListAsync();

                var isHeadersAdd = headers is not null and { Count: > 0 };

                    btnList.Where(w => !w.ParentId.HasValue).ToList().ForEach(f =>
                    {
                        btnList.Remove(f);
                        var btn = new ButtonDto();
                        btn.Id = f.Id;
                        btn.ParetId = f.ParentId;
                        btn.Name = f.Name;
                        btn.Type = f.ButtonType;
                        btn.ServiceId = f.ServiceId;
                        btn.ServiceName = f.ServiceName;
                        btn.SortId = f.SortId;
                        if (isHeadersAdd) btn.Destcriptions.AddRange(headers);
                        btn.Destcriptions.Add(f.Name);
                        SearchButtonsChild(btn, btnList);
                        buttons.Add(btn);
                    });

                    await GraficaMain(panel, buttons, menuHeaders: headers, preRecordId: preRecordId, priorityId: priorityId);
               
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при инициализации", "Configuration", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private static void SearchButtonsChild(ButtonDto buttons, List<TerminalButtonsDto> sprButtons)
        {
            var buttonsChilds = sprButtons.Where(w => w.ParentId == buttons.Id).ToList();

            if (!buttonsChilds.Any() || !sprButtons.Any()) return;

            sprButtons.RemoveAll(r => buttonsChilds.Contains(r));

            buttonsChilds.ForEach(f =>
            {
                var btn = new ButtonDto();
                btn.Id = f.Id;
                btn.Name = f.Name;
                btn.ParetId = f.ParentId;
                btn.Type = f.ButtonType;
                btn.ServiceId = f.ServiceId;
                btn.ServiceName = f.ServiceName;
                btn.Destcriptions.AddRange(buttons.Destcriptions);
                btn.Destcriptions.Add(f.Name);
                SearchButtonsChild(btn, sprButtons);
                buttons.ChildButtons.Add(btn);
            });
        }

        private async Task GraficaMain(Grid panel, List<ButtonDto> buttons, List<string>? menuHeaders = null, long? preRecordId = null, long? priorityId = null)
        {
            panel.Children.Clear();
            var grid = new Grid();

            var settingButtons = await GetTerminalButtonSettings();
            var idxRow = 0;
            var idxCol = 0;

            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(30) });
            var menuHeadersTextBlock = new TextBlock();
            menuHeadersTextBlock.FontSize = 20;
            menuHeadersTextBlock.Foreground = new SolidColorBrush(Colors.Black);
            Grid.SetRow(menuHeadersTextBlock, idxRow++);
            grid.Children.Add(menuHeadersTextBlock);

            if (menuHeaders != null)
            {
                menuHeadersTextBlock.Text = String.Join(" -> ", menuHeaders);
            }

            if (buttons.Count > 0)
            {
                for (int i = 0; i < settingButtons.RowCount; i++)
                {
                    grid.RowDefinitions.Add(new RowDefinition { MaxHeight = 300 });
                }

                for (int i = 0; i < settingButtons.ColCount; i++)
                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition { MaxWidth = 600 });
                }
                buttons.OrderBy(o=>o.SortId).ToList().ForEach(f =>
                {
                    if (idxCol == settingButtons.ColCount)
                    {
                        idxCol = 0;
                        idxRow++;
                    }
                    switch (f.Type)
                    {
                        case 1:
                            ButtonMenu btnMenu = new ButtonMenu(f, _colorDto);
                            //btnMenu.MaxWidth = 600;
                            //btnMenu.MaxHeight = 400;
                            btnMenu.Click += async (s, e) =>
                            {
                                await GraficaMain(panel, f.ChildButtons, f.Destcriptions, preRecordId: preRecordId, priorityId: priorityId);
                            };
                            Grid.SetRow(btnMenu, idxRow);
                            Grid.SetColumn(btnMenu, idxCol);
                            grid.Children.Add(btnMenu);
                            break;
                        case 2:
                            ButtonAction btnAction = new ButtonAction(_colorDto, f.Name);
                            //btnAction.MaxWidth = 600;
                            //btnAction.MaxHeight = 400;
                            btnAction.Click += async (s, e) =>
                            {
                                if (f.ServiceId.HasValue)
                                {
                                    var save = await _context.TiketSaveAsync(new TiketSaveRequestDto { OfficeId = _terminalDto.Office.Id, OfficeTerminalId = _terminalDto.Id, ServiceId = f.ServiceId.Value });
                                    if (save != null)
                                    {
                                        var windowIp = await _context.SOfficeWindows
                                        .AsNoTracking()
                                        .Where(w=>w.WindowIp!=null&&w.SOfficeId==_terminalDto.Office.Id && 
                                        w.SOfficeWindowServices.Any(a=>a.SServiceId==f.ServiceId&&a.IsActive))
                                        .Select(s=>s.WindowIp)
                                        .ToListAsync();
                                        if (windowIp != null && windowIp.Count > 0)
                                        {
                                            Client.SendMessage(windowIp,"new Ticket");
                                        }
                                        try
                                        {
                                            PrintTalon(new PrintTiketDto
                                            {
                                                TicketNumberFull = save.TicketNumberFull,
                                                Count = save.Count,
                                                serviceName = f.ServiceName ?? string.Empty,
                                                OfficeName = _terminalDto.Office.Name
                                            });
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show("Ошибка при печати", "Configuration", MessageBoxButton.OK, MessageBoxImage.Warning);
                                        }
                                        if(f.ParetId!=null) await MainPages(_window.ContentWrapper);
                                    }
                                    else MessageBox.Show("Ошибка при сохранение", "Configuration", MessageBoxButton.OK, MessageBoxImage.Warning);

                                }
                            };
                            Grid.SetRow(btnAction, idxRow);
                            Grid.SetColumn(btnAction, idxCol);
                            grid.Children.Add(btnAction);
                            break;
                        default: break;
                    };
                    idxCol++;
                });
            }
            else
            {
                grid.RowDefinitions.Add(new RowDefinition { });
                var childButtonsEmpty = new TextBlock();
                childButtonsEmpty.FontSize = 40;
                childButtonsEmpty.Foreground = new SolidColorBrush(Colors.LightGray);
                childButtonsEmpty.Text = "Нет данных";
                childButtonsEmpty.HorizontalAlignment = HorizontalAlignment.Center;
                childButtonsEmpty.VerticalAlignment = VerticalAlignment.Center;
                Grid.SetRow(childButtonsEmpty, idxRow);
                Grid.SetColumn(childButtonsEmpty, idxCol);
                Grid.SetColumnSpan(childButtonsEmpty, (int)settingButtons.ColCount);
                grid.Children.Add(childButtonsEmpty);

            }

            Grid.SetRow(grid, 0);
            Grid.SetColumn(grid, 0);
            panel.Children.Add(grid);
        }

        private async Task<TiketSaveResponseDto?> SaveTalon(TiketSaveRequestDto requestDto)
        {
            DbCommand? cmd = null;
            DbDataReader? reader = null;
            try
            {
                cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = "SELECT * FROM public.insert_ticket(@in_s_office_id, @in_s_office_terminal_id, @in_s_service_id, @in_s_priority_id, @in_d_ticket_prerecord_id)";

                var parameters = GetParametersRequest(cmd, requestDto);

                cmd.Parameters.AddRange(parameters);

                if (cmd.Connection != null)
                {
                    await cmd.Connection.OpenAsync();
                }
                reader = await cmd.ExecuteReaderAsync();
                var response = await GetParametersResponse(reader);

                return response;

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null)
                {
                    await reader.CloseAsync();
                    await reader.DisposeAsync();
                }
                if (cmd != null && cmd.Connection != null)
                {
                    await cmd.Connection.CloseAsync();
                    await cmd.Connection.DisposeAsync();
                }
            }
        }

        private static Array GetParametersRequest(DbCommand dbcommand, TiketSaveRequestDto requestModel)
        {
            List<DbParameter> response = new List<DbParameter>();
            var parametrOfficeId = dbcommand.CreateParameter();
            parametrOfficeId.ParameterName = "in_s_office_id";
            parametrOfficeId.Value = requestModel.OfficeId;
            parametrOfficeId.DbType = DbType.Int64;
            response.Add(parametrOfficeId);

            var parametrTerminalId = dbcommand.CreateParameter();
            parametrTerminalId.ParameterName = "in_s_office_terminal_id";
            parametrTerminalId.Value = requestModel.OfficeTerminalId;
            parametrTerminalId.DbType = DbType.Int64;
            response.Add(parametrTerminalId);

            var parametrServiceId = dbcommand.CreateParameter();
            parametrServiceId.ParameterName = "in_s_service_id";
            parametrServiceId.Value = requestModel.ServiceId;
            parametrServiceId.DbType = DbType.Int64;
            response.Add(parametrServiceId);

            var parametrPriorityId = dbcommand.CreateParameter();
            parametrPriorityId.ParameterName = "in_s_priority_id";
            parametrPriorityId.Value = requestModel.PriorityId.HasValue ? requestModel.PriorityId.Value : DBNull.Value;
            parametrPriorityId.DbType = DbType.Int64;
            response.Add(parametrPriorityId);


            var parametrPrerecordId = dbcommand.CreateParameter();
            parametrPrerecordId.ParameterName = "in_d_ticket_prerecord_id";
            parametrPrerecordId.Value = requestModel.PrerecordId.HasValue ? requestModel.PrerecordId.Value : DBNull.Value;
            parametrPrerecordId.DbType = DbType.Int64;
            response.Add(parametrPrerecordId);

            return response.ToArray();
        }

        private static async Task<TiketSaveResponseDto?> GetParametersResponse(DbDataReader reader)
        {

            try
            {
                var response = new TiketSaveResponseDto();
                while (await reader.ReadAsync())
                {
                    response.Count = (long)reader["out_count"];
                    response.TicketNumberFull = (string)reader["out_ticket_number_full"];
                }
                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static void PrintTalon(PrintTiketDto tiketDto)
        {
            FastReport.Report report = new FastReport.Report();

            report.Load(new MemoryStream(Properties.Resources.Talon));

            report.SetParameterValue("Operation", tiketDto.serviceName);
            report.SetParameterValue("Number", tiketDto.TicketNumberFull);
            report.SetParameterValue("Time", TimeOnly.FromDateTime(DateTime.Now));
            report.SetParameterValue("TotalQueue", tiketDto.Count);
            report.SetParameterValue("BeforeCount", tiketDto.Count);
            report.SetParameterValue("MFC", tiketDto.OfficeName);
            report.Prepare();
            report.PrintSettings.ShowDialog = false;
            report.PrintSettings.PrintOnSheetRawPaperSize = 0;

            report.Print();
        }

        private async Task<TerminalButtonSettings> GetTerminalButtonSettings()
        {
            try
            {
                return await _context.SOfficeTerminals
                    .AsNoTracking()
                    .Where(w => w.Id == _terminalDto.Id)
                    .Select(s => new TerminalButtonSettings
                    {
                        RowCount = s.NumberLines ?? 1,
                        ColCount = s.NumberColumns ?? 1,
                    }).FirstOrDefaultAsync() ?? new TerminalButtonSettings(1, 1);
            }
            catch
            {
                return new TerminalButtonSettings(1, 1);
            }

        }
    }
}
