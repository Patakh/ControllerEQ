using Microsoft.EntityFrameworkCore;
using QE.Context;
using QE.FunctionContext;
using QE.Models.DTO;
using QE.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
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
        public async Task PreRecordPages(Grid panel)
        {
            panel.Children.Clear();

            var list = await _context.GetPreRecordData(_terminalDto.Office.Id);
            if (list == null || list.Count == 0)
            {
                panel.Children.Add(new PanelEmpty());
            }
            else
            {
                GraficaPreRecordButtonDate(panel, list);
            }
        }

        private void GraficaPreRecordButtonDate(Grid panel, List<PreRecord> request)
        {
            WrapPanel wrapPanelPreRegistrationMain = new WrapPanel();

            var data = request.GroupBy(g => new { g.Date, g.DayName }).Select(s => new { s.Key.Date, s.Key.DayName, Time = s.ToList() }).ToList();

            foreach (var f in data)
            {
                ButtonPreRecordDate btnDate = new ButtonPreRecordDate(f.Date.ToString("d") + "\n" + f.DayName);
                btnDate.Click += (s, e) =>
                {
                    GraficaPreRecordButtonTime(wrapPanelPreRegistrationMain, f.Time);
                };
                wrapPanelPreRegistrationMain.Children.Add(btnDate);
            }

            panel.Children.Add(wrapPanelPreRegistrationMain);
        }

        private void GraficaPreRecordButtonTime(WrapPanel wrapPanelPreRegistrationMain, List<PreRecord> data)
        {

            wrapPanelPreRegistrationMain.Children.Clear();

            foreach (var f in data)
            {
                ButtonPreRecordTime btnTime = new ButtonPreRecordTime(f.StartTimePrerecord.ToString("hh\\:mm") + " - " + f.StopTimePrerecord.ToString("hh\\:mm"));
                btnTime.Click += (s, e) =>
                {
                    GraficaPreRecordForm(wrapPanelPreRegistrationMain, f);
                };
                wrapPanelPreRegistrationMain.Children.Add(btnTime);
            }
        }

        private void GraficaPreRecordForm(WrapPanel wrapPanelPreRegistrationMain, PreRecord data)
        {
            wrapPanelPreRegistrationMain.Children.Clear();
            wrapPanelPreRegistrationMain.HorizontalAlignment = HorizontalAlignment.Center;

            WrapPanel stackPanelForm = new WrapPanel();
            stackPanelForm.Orientation = Orientation.Vertical;

            StackPanel stackPanelData = new StackPanel();
            stackPanelData.Orientation = Orientation.Vertical;
            stackPanelData.Width = 600;

            Label labelFio = new Label();
            labelFio.FontFamily = new FontFamily("Area");
            labelFio.FontSize = 20;
            labelFio.Content = "ФИО: ";

            TextBox textBoxFio = new TextBox();
            textBoxFio.FontFamily = new FontFamily("Area");
            textBoxFio.Padding = new Thickness(5, 8, 5, 8);
            textBoxFio.FontSize = 25;
            textBoxFio.Height = 45;

            textBoxFio.Focus();

            Label labelPhone = new Label();
            labelPhone.FontFamily = new FontFamily("Area");
            labelPhone.FontSize = 20;
            labelPhone.Content = "Телефон: ";

            TextBox textBoxPhone = new TextBox();
            textBoxPhone.FontFamily = new FontFamily("Area");
            textBoxPhone.Padding = new Thickness(5, 8, 5, 8);
            textBoxPhone.FontSize = 25;
            textBoxPhone.Height = 45;
            textBoxPhone.Text = "+7(";

            stackPanelData.Children.Add(labelFio);
            stackPanelData.Children.Add(textBoxFio);

            stackPanelData.Children.Add(labelPhone);
            stackPanelData.Children.Add(textBoxPhone);

            stackPanelForm.Children.Add(stackPanelData);

            StackPanelKeyboard stackPanelKeyboard = new StackPanelKeyboard();
            stackPanelKeyboard.Margin = new Thickness(0, 20, 0, 0);
            stackPanelForm.Children.Add(stackPanelKeyboard);

            StackPanelKeyboardNumbers stackPanelKeyboardNumbers = new StackPanelKeyboardNumbers();
            stackPanelKeyboardNumbers.Margin = new Thickness(0, 20, 0, 0);
            stackPanelKeyboardNumbers.Visibility = Visibility.Collapsed;
            stackPanelForm.Children.Add(stackPanelKeyboardNumbers);

            StackPanel stackPanekBtnNext = new StackPanel();
            Button buttonNext = new()
            {
                Background = new SolidColorBrush(Colors.LightGray),
                Content = "Записаться",
                Visibility = Visibility.Collapsed,
                Width = 600,
                Height = 50,
                Foreground = new SolidColorBrush(Colors.White),
                Margin = new Thickness(8),
                FontSize = 18,
            };
            stackPanekBtnNext.Children.Add(buttonNext);
            stackPanelForm.Children.Add(stackPanekBtnNext);

            bool upperCase = true;
            foreach (WrapPanel item in stackPanelKeyboard.Children)
            {
                foreach (ButtonKeyboard buttonKeyboard in item.Children)
                {

                    buttonKeyboard.Click += (s, e) =>
                    {
                        textBoxFio.BorderBrush = new SolidColorBrush(Colors.Black);
                        labelFio.Foreground = new SolidColorBrush(Colors.Black);

                        Button buttonClick = (Button)s;
                        switch (buttonKeyboard.Content.ToString())
                        {

                            case "Удалить":
                                textBoxFio.Text = textBoxFio.Text.Length == 0 ? "" : textBoxFio.Text.Substring(0, textBoxFio.Text.Length - 1);
                                textBoxFio.CaretIndex = textBoxFio.Text.Length;
                                textBoxFio.Focus();
                                if (textBoxFio.Text.Length == 0) buttonNext.Visibility = Visibility.Collapsed;
                                break;
                            case "Пробел":
                                textBoxFio.Text += " ";
                                textBoxFio.CaretIndex = textBoxFio.Text.Length;
                                textBoxFio.Focus();
                                break;
                            case "Очистить":
                                textBoxFio.Text = "";
                                textBoxFio.CaretIndex = textBoxFio.Text.Length;
                                textBoxFio.Focus();
                                buttonNext.Visibility = Visibility.Collapsed;
                                break;
                            case "Далее":
                                if (textBoxFio.Text.Length == 0)
                                {
                                    textBoxFio.BorderBrush = new SolidColorBrush(Colors.Red);
                                    labelFio.Foreground = new SolidColorBrush(Colors.Red);
                                    labelFio.Content = "ФИО: не заполнено !";
                                }
                                else
                                {
                                    stackPanelKeyboardNumbers.Visibility = Visibility.Visible;
                                    stackPanelKeyboard.Visibility = Visibility.Collapsed;
                                    textBoxPhone.CaretIndex = textBoxPhone.Text.Length;
                                    textBoxPhone.Focus();
                                }
                                break;
                            case "Регистр":
                                upperCase = !upperCase;
                                if (!upperCase)
                                {
                                    buttonKeyboard.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                                    buttonKeyboard.Foreground = new SolidColorBrush(Colors.Brown);
                                }
                                else
                                {
                                    buttonKeyboard.Background = new SolidColorBrush(Colors.Brown);
                                    buttonKeyboard.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                                }
                                break;
                            default:
                                labelFio.Content = "ФИО: ";
                                textBoxFio.Text += upperCase ? buttonKeyboard.Content.ToString().ToLower() : buttonKeyboard.Content.ToString().ToUpper();
                                textBoxFio.CaretIndex = textBoxFio.Text.Length;
                                textBoxFio.Focus();
                                if (textBoxPhone.Text.Length == 16) buttonNext.Visibility = Visibility.Visible;
                                break;
                        }
                    };
                }

            }

            foreach (WrapPanel item in stackPanelKeyboardNumbers.Children)
            {
                foreach (Button buttonKeyboard in item.Children)
                {

                    buttonKeyboard.Click += (s, e) =>
                    {
                        Button buttonClick = (Button)s;

                        switch (buttonKeyboard.Content.ToString())
                        {
                            case "Удалить":

                                textBoxPhone.Text = textBoxPhone.Text.Length == 0 ? "" : textBoxPhone.Text.Length > 3 ? textBoxPhone.Text.Substring(0, textBoxPhone.Text.Length - 1) : textBoxPhone.Text;

                                break;
                            default:
                                switch (textBoxPhone.Text.Length)
                                {
                                    case 3:
                                        textBoxPhone.Text += buttonKeyboard.Content;
                                        break;
                                    case 4:
                                        textBoxPhone.Text += buttonKeyboard.Content;
                                        break;
                                    case 5:
                                        textBoxPhone.Text += buttonKeyboard.Content + ")";
                                        break;
                                    case 7:
                                        textBoxPhone.Text += buttonKeyboard.Content;
                                        break;
                                    case 8:
                                        textBoxPhone.Text += buttonKeyboard.Content;
                                        break;
                                    case 9:
                                        textBoxPhone.Text += buttonKeyboard.Content + "-";
                                        break;
                                    case 11:
                                        textBoxPhone.Text += buttonKeyboard.Content;
                                        break;
                                    case 12:
                                        textBoxPhone.Text += buttonKeyboard.Content + "-";
                                        break;
                                    case 14:
                                        textBoxPhone.Text += buttonKeyboard.Content;
                                        break;
                                    case 15:
                                        textBoxPhone.Text += buttonKeyboard.Content;
                                        if (textBoxFio.Text.Length == 0)
                                        {
                                            textBoxFio.BorderBrush = new SolidColorBrush(Colors.Red);
                                            labelFio.Foreground = new SolidColorBrush(Colors.Red);
                                            labelFio.Content = "ФИО: не заполнено !";
                                        }
                                        else
                                        {
                                            buttonNext.Visibility = Visibility.Visible;
                                        }
                                        break;
                                }
                                break;
                        }
                        if (textBoxPhone.Text.Length != 16) buttonNext.Visibility = Visibility.Collapsed;
                        textBoxPhone.CaretIndex = textBoxPhone.Text.Length;
                        textBoxPhone.Focus();
                    };
                }
            }

            textBoxFio.PreviewMouseDown += (s, e) =>
            {
                stackPanelKeyboardNumbers.Visibility = Visibility.Collapsed;
                stackPanelKeyboard.Visibility = Visibility.Visible;
                textBoxFio.Focus();
            };

            textBoxPhone.PreviewMouseDown += (s, e) =>
            {
                stackPanelKeyboardNumbers.Visibility = Visibility.Visible;
                stackPanelKeyboard.Visibility = Visibility.Collapsed;
                textBoxPhone.Focus();
            };

            buttonNext.Click += async (s, e) =>
            {
                var response = await _context.PreRecordSaveAsync(_terminalDto.Office.Id,new PreRecordSaveRequestDto { Fio = textBoxFio.Text, PhoneNumber = textBoxPhone.Text, DatePreRecord = data.Date, StartTimePrerecord = data.StartTimePrerecord, StopTimePrerecord = data.StopTimePrerecord });
                if (response != null)
                {
                    GraficaPreRecordFinal(wrapPanelPreRegistrationMain, response);
                }
                else
                {
                    MessageBox.Show("Ошибка при сохранение", "Configuration", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            };

            wrapPanelPreRegistrationMain.Children.Add(stackPanelForm);
        }


        private void GraficaPreRecordFinal(WrapPanel wrapPanelPreRegistrationMain, PreRecordSaveResponseDto data)
        {
            wrapPanelPreRegistrationMain.Children.Clear();
            wrapPanelPreRegistrationMain.HorizontalAlignment = HorizontalAlignment.Stretch;

            WrapPanel panel = new WrapPanel();
            panel.Orientation = Orientation.Vertical;

            WrapPanel panelText = new WrapPanel();

            TextBlock ResultPreRegistrationCode = new TextBlock();
            ResultPreRegistrationCode.Text = "Ваш код: " + data.Number.ToString("D4");
            ResultPreRegistrationCode.HorizontalAlignment = HorizontalAlignment.Left;
            ResultPreRegistrationCode.VerticalAlignment = VerticalAlignment.Center;
            ResultPreRegistrationCode.FontSize = 100;
            ResultPreRegistrationCode.TextWrapping = TextWrapping.Wrap;
            panelText.Children.Add(ResultPreRegistrationCode);

            TextBlock ResultPreRegistrationText = new TextBlock();
            ResultPreRegistrationText.Text = "Вы должны явиться в " + CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat.GetDayName(data.DatePreRecord.DayOfWeek) + " " + data.DatePreRecord.ToShortDateString() + "\nс " + data.StartTimePrerecord.ToString("hh\\:mm") + " по " + data.StopTimePrerecord.ToString("hh\\:mm");
            ResultPreRegistrationText.HorizontalAlignment = HorizontalAlignment.Left;
            ResultPreRegistrationCode.VerticalAlignment = VerticalAlignment.Center;
            ResultPreRegistrationText.FontSize = 40;
            ResultPreRegistrationText.Margin = new Thickness(30, 15, 0, 0);
            ResultPreRegistrationText.Foreground = new SolidColorBrush(Colors.Green);
            ResultPreRegistrationText.TextWrapping = TextWrapping.Wrap;
            panelText.Children.Add(ResultPreRegistrationText);


            //печать кода
            Button buttonPrintResultPreRegistration = new Button();
            buttonPrintResultPreRegistration.Content = "Печать";
            buttonPrintResultPreRegistration.HorizontalAlignment = HorizontalAlignment.Left;
            buttonPrintResultPreRegistration.VerticalAlignment = VerticalAlignment.Bottom;
            buttonPrintResultPreRegistration.Height = 50;
            buttonPrintResultPreRegistration.Width = 250;
            buttonPrintResultPreRegistration.Margin = new Thickness(0, 50, 0, 0);
            buttonPrintResultPreRegistration.Background = new SolidColorBrush(Colors.DarkGreen);
            buttonPrintResultPreRegistration.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 20));
            buttonPrintResultPreRegistration.FontFamily = new FontFamily("Area");
            buttonPrintResultPreRegistration.FontSize = 20;
            buttonPrintResultPreRegistration.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));

            buttonPrintResultPreRegistration.Click += (s, e) =>
            {
                try
                {
                    PreRecordPrint(data);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при печати", "Configuration", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            };

            panel.Children.Add(panelText);
            panel.Children.Add(buttonPrintResultPreRegistration);
            wrapPanelPreRegistrationMain.Children.Add(panel);
        }

        private async Task<PreRecordSaveResponseDto?> PreRecordSave(PreRecordSaveRequestDto request)
        {
            DbCommand? cmd = null;
            DbDataReader? reader = null;
            try
            {
                cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = "SELECT * FROM public.insert_prerecord(@in_s_office_id, @in_s_source_prerecord_id, @in_s_employee_id, " +
                    "@in_customer_full_name, @in_customer_phone_number, @in_customer_e_mail, @in_customer_snils," +
                    "@in_date_prerecord, @in_start_time_prerecord, @in_stop_time_prerecord)";
                List<DbParameter> parameters = new List<DbParameter>();

                var parameterOfficeId = cmd.CreateParameter();
                parameterOfficeId.ParameterName = "in_s_office_id";
                parameterOfficeId.Value = _terminalDto.Office.Id;
                parameterOfficeId.DbType = DbType.Int64;
                parameters.Add(parameterOfficeId);

                var parameterSourceCode = cmd.CreateParameter();
                parameterSourceCode.ParameterName = "in_s_source_prerecord_id";
                parameterSourceCode.Value = 2;
                parameterSourceCode.DbType = DbType.Int64;
                parameters.Add(parameterSourceCode);

                var parameterEmployeeId = cmd.CreateParameter();
                parameterEmployeeId.ParameterName = "in_s_employee_id";
                parameterEmployeeId.Value = DBNull.Value;
                parameterEmployeeId.DbType = DbType.Int64;
                parameters.Add(parameterEmployeeId);

                var parameterCustomerFio = cmd.CreateParameter();
                parameterCustomerFio.ParameterName = "in_customer_full_name";
                parameterCustomerFio.Value = request.Fio;
                parameterCustomerFio.DbType = DbType.String;
                parameters.Add(parameterCustomerFio);

                var parameterCustomerPhoneNumber = cmd.CreateParameter();
                parameterCustomerPhoneNumber.ParameterName = "in_customer_phone_number";
                parameterCustomerPhoneNumber.Value = request.PhoneNumber;
                parameterCustomerPhoneNumber.DbType = DbType.String;
                parameters.Add(parameterCustomerPhoneNumber);

                var parameterCustomerEmail = cmd.CreateParameter();
                parameterCustomerEmail.ParameterName = "in_customer_e_mail";
                parameterCustomerEmail.Value = DBNull.Value;
                parameters.Add(parameterCustomerEmail);

                var parametrCustomerSnils = cmd.CreateParameter();
                parametrCustomerSnils.ParameterName = "in_customer_snils";
                parametrCustomerSnils.Value = DBNull.Value;
                parameters.Add(parametrCustomerSnils);

                var parameterStartTime = cmd.CreateParameter();
                parameterStartTime.ParameterName = "in_start_time_prerecord";
                parameterStartTime.Value = TimeOnly.FromTimeSpan(request.StartTimePrerecord);
                parameters.Add(parameterStartTime);

                var parameterStopTime = cmd.CreateParameter();
                parameterStopTime.ParameterName = "in_stop_time_prerecord";
                parameterStopTime.Value = TimeOnly.FromTimeSpan(request.StopTimePrerecord);
                parameters.Add(parameterStopTime);

                var parameterDate = cmd.CreateParameter();
                parameterDate.ParameterName = "in_date_prerecord";
                parameterDate.Value = request.DatePreRecord.Date;
                parameterDate.DbType = DbType.Date;
                parameters.Add(parameterDate);

                cmd.Parameters.AddRange(parameters.ToArray());

                if (cmd.Connection != null)
                {
                    await cmd.Connection.OpenAsync();
                }
                reader = await cmd.ExecuteReaderAsync();

                PreRecordSaveResponseDto responseData = new PreRecordSaveResponseDto
                {
                    DatePreRecord = request.DatePreRecord,
                    StartTimePrerecord = request.StartTimePrerecord,
                    StopTimePrerecord = request.StopTimePrerecord
                };

                while (await reader.ReadAsync())
                {
                    responseData.Number = (long)reader["out_code_prerecord"];
                }

                return responseData;
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
                }
            }
        }


        private async Task<PreRecordSaveResponseDto?> PreRecordSaveV2(PreRecordSaveRequestDto request)
        {
            try
            {
                DTicketPrerecord dTicketPrerecord = new DTicketPrerecord();
                dTicketPrerecord.SOfficeId = _terminalDto.Office.Id;
                dTicketPrerecord.SSourсePrerecordId = 2;
                dTicketPrerecord.CustomerFullName = request.Fio;
                dTicketPrerecord.CustomerPhoneNumber = request.PhoneNumber;
                dTicketPrerecord.DatePrerecord = request.DatePreRecord;
                dTicketPrerecord.StartTimePrerecord = request.StartTimePrerecord;
                dTicketPrerecord.StopTimePrerecord = request.StopTimePrerecord;
                dTicketPrerecord.IsConfirmation = false;
                dTicketPrerecord.CodePrerecord = 5555;
                await _context.DTicketPrerecords.AddAsync(dTicketPrerecord);
                await _context.SaveChangesAsync();
                return new PreRecordSaveResponseDto
                {
                    Number = dTicketPrerecord.CodePrerecord,
                    DatePreRecord = request.DatePreRecord,
                    StartTimePrerecord = request.StartTimePrerecord,
                    StopTimePrerecord = request.StopTimePrerecord
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private void PreRecordPrint(PreRecordSaveResponseDto data)
        {
            FastReport.Report report = new FastReport.Report();

            report.Load(new MemoryStream(Properties.Resources.PreRegistration));
            report.SetParameterValue("Code", data.Number.ToString());
            report.SetParameterValue("DayWeek", CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat.GetDayName(data.DatePreRecord.DayOfWeek));
            report.SetParameterValue("DateReg", data.DatePreRecord.ToShortDateString());
            report.SetParameterValue("StartTime", data.StartTimePrerecord.ToString("hh\\:mm"));
            report.SetParameterValue("StopTime", data.StopTimePrerecord.ToString("hh\\:mm"));
            report.SetParameterValue("MFC", _terminalDto.Office.Name);

            report.Prepare();
            report.PrintSettings.ShowDialog = false;
            report.PrintSettings.PrintOnSheetRawPaperSize = 0;

            report.Print();
        }


        public async Task<List<PreRecordDto>?> GetPreRecordData(long officeId)
        {
            DbCommand? cmd = null;
            DbDataReader? reader = null;
            try
            {

                cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = "SELECT * FROM public.select_prerecord(@in_s_office_id, @in_date)";
                List<DbParameter> parameters = new List<DbParameter>();
                var parametrOfficeId = cmd.CreateParameter();
                parametrOfficeId.ParameterName = "in_s_office_id";
                parametrOfficeId.Value = officeId;
                parametrOfficeId.DbType = DbType.Int64;
                parameters.Add(parametrOfficeId);
                var parametrDate = cmd.CreateParameter();
                parametrDate.ParameterName = "in_date";
                parametrDate.Value = DateTime.Now.Date;
                parametrDate.DbType = DbType.Date;
                parameters.Add(parametrDate);
                cmd.Parameters.AddRange(parameters.ToArray());

                if (cmd.Connection != null)
                {
                    await cmd.Connection.OpenAsync();
                }
                reader = await cmd.ExecuteReaderAsync();

                List<PreRecordDto> responseData = new List<PreRecordDto>();
                while (await reader.ReadAsync())
                {
                    PreRecordDto prerecord = new PreRecordDto();
                    prerecord.SDayWeekId = (long)reader["out_s_day_week_id"];
                    prerecord.DayName = (string)reader["out_day_name"];
                    prerecord.Date = (DateTime)reader["out_date"];
                    prerecord.StartTimePrerecord = (TimeSpan)reader["out_start_time_prerecord"];
                    prerecord.StopTimePrerecord = (TimeSpan)reader["out_stop_time_prerecord"];
                    responseData.Add(prerecord);
                }

                return responseData;
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
                }
            }
        }
    }
}
