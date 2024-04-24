using Microsoft.EntityFrameworkCore;
using QE.Context;
using QE.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace QE.Models
{
    public partial class Main
    {
        public async Task InitTerminal()
        {
            Page.Start(_window.ContentWrapper, "Запуск...");
            try
            {
                var options = new DbContextOptionsBuilder<EqContext>()
                        .UseNpgsql(Properties.Settings.Default.connection)
                        .Options;

                _context = new EqContext(options);
            }
            catch (Exception e)
            {
                Page.Error(_window.ContentWrapper, "Инициализация бд");
                return;
            }
            try
            {
                _terminalDto = await InitTerminalDtoAsync();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
                Page.Error(_window.ContentWrapper, "Терминал не найден");
                return;
            }
            try
            {
                _schedulesDto = await InitSchedulesAsync(_terminalDto.Office.Id);
                if (_schedulesDto == null || _schedulesDto.Count == 0) throw new Exception();
            }
            catch
            {
                Page.Error(_window.ContentWrapper, "Расписание не найдено");
                return;
            }
            try
            {
                _colorDto = await InitColorsAsync();
            }
            catch
            {
                Page.Error(_window.ContentWrapper, "Цветовая схема не настроена");
                return;
            }
            try
            {
                _settingDto = await InitSettingAsync();
            }
            catch
            {
                Page.Error(_window.ContentWrapper, "Настройки не найдены");
                return;
            }

            _window.Title = GetTerminalName();
            HeaderOfficesPages(_window.HeaderTextBlockOfice);
            HeaderDatePages(_window.HeaderTextBlockDate);

            var timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += async (s, e) => { await UpdateDateTime(_window.HeaderTextBlockDate); };
            timer.Start();
        }

        public static string GetIp()
        {
            return Dns.GetHostAddresses(Dns.GetHostName()).First(f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();
        }
        public static List<string> GetListIp()
        {
            return Dns.GetHostAddresses(Dns.GetHostName()).Where(f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).Select(s=>s.ToString()).ToList();
        }

        public string GetTerminalName() => _terminalDto.Name;
        public ColorDto GetColor() => _colorDto;
        public SettingDto GetSetting() => _settingDto;

        private async Task<TerminalDto> InitTerminalDtoAsync()
        {
            return await _context.SOfficeTerminals
                .AsNoTracking()
                .Where(w =>w.IpAddress!=null&& GetListIp().Contains(w.IpAddress))
                .Select(s => new TerminalDto
                {
                    Id = s.Id,
                    Name = s.TerminalName,
                    Ip = s.IpAddress,
                    Office = new OfficeDto
                    {
                        Id = s.SOffice.Id,
                        Name = s.SOffice.OfficeName
                    }
                })
                .FirstOrDefaultAsync() ?? throw new Exception($"терминал ({String.Join(" ", GetListIp())}) не найден");
        }

        private async Task<ColorDto> InitColorsAsync()
        {
            var color = new ColorDto();
            var dictonary = await _context.SColors.AsNoTracking().ToDictionaryAsync(k => k.Id, v => v.ColorValue);

            color.ColorBtnMenu = dictonary[1] != "0" ? (Color)ColorConverter.ConvertFromString(dictonary[1]) : Colors.Black;
            color.ColorBtnAction = dictonary[2] != "0" ? (Color)ColorConverter.ConvertFromString(dictonary[2]) : Colors.Black; ;
            color.ColorBtnTextMenu = dictonary[3] != "0" ? (Color)ColorConverter.ConvertFromString(dictonary[3]) : Colors.Black; ;
            color.ColorBtnTextAction = dictonary[4] != "0" ? (Color)ColorConverter.ConvertFromString(dictonary[4]) : Colors.Black; ;
            color.ColorTextHeader = dictonary[5] != "0" ? (Color)ColorConverter.ConvertFromString(dictonary[5]) : Colors.Black; ;
            color.ColorTextFooter = dictonary[6] != "0" ? (Color)ColorConverter.ConvertFromString(dictonary[6]) : Colors.Black; ;
            color.ColorTextSheldue = dictonary[7] != "0" ? (Color)ColorConverter.ConvertFromString(dictonary[7]) : Colors.Black; ;

            return color;
        }

        private async Task<SettingDto> InitSettingAsync()
        {
            var setting = new SettingDto();
            var dictonary = await _context.SSettings.AsNoTracking().ToDictionaryAsync(k => k.Id, v => v.ParamValue);

            setting.isActiveButtonPriority = bool.Parse(dictonary[1]);
            setting.isActiveButtonPreRecord = bool.Parse(dictonary[2]);

            return setting;
        }

        private async Task<List<SchedulesDto>> InitSchedulesAsync(long officeId)
        {
            return await _context.SOfficeSchedules
                .AsNoTracking()
                .Where(w => w.SOfficeId == officeId)
                .OrderBy(w => w.SDayWeekId)
                .Select(s => new SchedulesDto
                {
                    Id = s.Id,
                    SDayWeekId = s.SDayWeekId,
                    SDayWeekName = s.SDayWeek.DayName,
                    StartTime = s.StartTime,
                    StopTime = s.StopTime,
                }).ToListAsync();
        }

    }
}
