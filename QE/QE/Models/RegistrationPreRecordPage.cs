using Microsoft.EntityFrameworkCore;
using QE.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace QE.Models
{
    public partial class Main
    {
        public void RegistrationPreRecordPages(Grid panel)
        {
           GraficaRegistrationPreRecord(panel);
        }

        private void GraficaRegistrationPreRecord(Grid gridPanelPreRegistrationMain)
        {
            gridPanelPreRegistrationMain.Children.Clear();

            WrapPanel stackPanelForm = new WrapPanel();
            stackPanelForm.Orientation = Orientation.Vertical;
            stackPanelForm.HorizontalAlignment = HorizontalAlignment.Center;

            StackPanel stackPanelData = new StackPanel();
            stackPanelData.Orientation = Orientation.Vertical;

            TextBlock textBlockRegistrationAppointment = new TextBlock();
            textBlockRegistrationAppointment.FontFamily = new FontFamily("Area");
            textBlockRegistrationAppointment.FontSize = 40;
            textBlockRegistrationAppointment.TextWrapping = TextWrapping.Wrap;
            textBlockRegistrationAppointment.HorizontalAlignment = HorizontalAlignment.Center;
            textBlockRegistrationAppointment.Foreground = new SolidColorBrush(Colors.Black);
            textBlockRegistrationAppointment.Text = "Введите код";

            //ошибки при вводе
            TextBlock textBlockRegistrationAppointmentError = new TextBlock();
            textBlockRegistrationAppointmentError.FontFamily = new FontFamily("Area");
            textBlockRegistrationAppointmentError.FontSize = 20;
            textBlockRegistrationAppointmentError.TextWrapping = TextWrapping.Wrap;
            textBlockRegistrationAppointmentError.HorizontalAlignment = HorizontalAlignment.Center;
            textBlockRegistrationAppointmentError.Foreground = new SolidColorBrush(Colors.Red);

            //поле для ввода
            TextBox textBoxRegistrationAppointment = new TextBox();
            textBoxRegistrationAppointment.FontSize = 40;
            textBoxRegistrationAppointment.Foreground = new SolidColorBrush(Colors.Black);
            textBoxRegistrationAppointment.Margin = new Thickness(0, 10, 0, 10);
            textBoxRegistrationAppointment.Width = 310;
            textBoxRegistrationAppointment.TextWrapping = TextWrapping.Wrap;
            textBoxRegistrationAppointment.MaxLength = 4;
            textBoxRegistrationAppointment.Focus();

            stackPanelData.Children.Add(textBlockRegistrationAppointment);
            stackPanelData.Children.Add(textBlockRegistrationAppointmentError);
            stackPanelData.Children.Add(textBoxRegistrationAppointment);

            stackPanelForm.Children.Add(stackPanelData);

            StackPanelKeyboardRegistrationPreRecord stackPanelKeyboardRegistrationAppointment = new StackPanelKeyboardRegistrationPreRecord();
            stackPanelKeyboardRegistrationAppointment.Margin = new Thickness(0, 10, 0, 0);
            stackPanelForm.Children.Add(stackPanelKeyboardRegistrationAppointment);

            foreach (WrapPanel item in stackPanelKeyboardRegistrationAppointment.Children)
            {
                foreach (ButtonKeyboardRegistrationPreRecord button in item.Children)
                {
                    button.Click += async (s, e) =>
                    {
                        var buttonClick = (Button)s;
                        textBlockRegistrationAppointmentError.Text = "";
                        textBoxRegistrationAppointment.BorderBrush = new SolidColorBrush(Colors.Black);
                        textBoxRegistrationAppointment.Foreground = new SolidColorBrush(Colors.Black);
                        switch (buttonClick.Content.ToString())
                        {
                            case "Удалить":
                                textBoxRegistrationAppointment.Text = textBoxRegistrationAppointment.Text.Length == 0 ? "" : textBoxRegistrationAppointment.Text.Substring(0, textBoxRegistrationAppointment.Text.Length - 1);
                                break;
                            case "Ввод":
                                if (textBoxRegistrationAppointment.Text.Length != 4)
                                {
                                    textBlockRegistrationAppointmentError.Text = "Код не коректен !";
                                    textBoxRegistrationAppointment.BorderBrush = new SolidColorBrush(Colors.Red);
                                    textBoxRegistrationAppointment.Foreground = new SolidColorBrush(Colors.Red);
                                }
                                else
                                {
                                    var prerecord = await _context.DTicketPrerecords.AsNoTracking().Where(d => d.CodePrerecord == Convert.ToInt64(textBoxRegistrationAppointment.Text) && d.SOfficeId == _terminalDto.Office.Id).FirstOrDefaultAsync();
                                    var date = DateTime.Now;
                                    if (prerecord == null)
                                    {
                                        textBlockRegistrationAppointmentError.Text = "Неверный код !";
                                        textBoxRegistrationAppointment.BorderBrush = new SolidColorBrush(Colors.Red);
                                        textBoxRegistrationAppointment.Foreground = new SolidColorBrush(Colors.Red);
                                    }
                                    else
                                    if (prerecord.DatePrerecord.Add(prerecord.StartTimePrerecord) > date)
                                    {
                                        textBlockRegistrationAppointmentError.Text = "Время предварительной записи не вышло. Вы должны явиться " + prerecord.DatePrerecord.ToShortDateString() + " с " + prerecord.StartTimePrerecord.ToString("hh\\:mm") + " по " + prerecord.StopTimePrerecord.ToString("hh\\:mm");
                                    }
                                    else
                                    if (prerecord.DatePrerecord.Add(prerecord.StopTimePrerecord) < date)
                                    {
                                        textBlockRegistrationAppointmentError.Text = "Время предварительной записи вышло. Вы должны были явиться " + prerecord.DatePrerecord.ToShortDateString() + " с " + prerecord.StartTimePrerecord.ToString("hh\\:mm") + " по " + prerecord.StopTimePrerecord.ToString("hh\\:mm");
                                    }
                                    else
                                    {
                                        await AnimatiomRegistrationPreRecord(gridPanelPreRegistrationMain);
                                        await InitButtons(gridPanelPreRegistrationMain, headers: new List<string> { "Пред. запись" }, preRecordId: prerecord.Id);
                                    }
                                }
                                break;
                            default:
                                if (textBoxRegistrationAppointment.Text.Length <= 3)
                                    textBoxRegistrationAppointment.Text += button.Content.ToString().ToUpper();
                                break;
                        }
                        textBoxRegistrationAppointment.CaretIndex = textBoxRegistrationAppointment.Text.Length;
                        textBoxRegistrationAppointment.Focus();
                    };
                }
            }

            gridPanelPreRegistrationMain.Children.Add(stackPanelForm);
        }

        private async Task AnimatiomRegistrationPreRecord(Grid gridPanelPreRegistrationMain)
        {
            gridPanelPreRegistrationMain.Children.Clear();

            WrapPanel stackPanelForm = new WrapPanel();
            stackPanelForm.Orientation = Orientation.Vertical;
            stackPanelForm.HorizontalAlignment = HorizontalAlignment.Center;
            stackPanelForm.VerticalAlignment = VerticalAlignment.Center;

            stackPanelForm.Children.Add(new TextAnimationRegistrationPreRecord());

            gridPanelPreRegistrationMain.Children.Add(stackPanelForm);

            await Task.Delay(1000);
        }

    }


    //    private async Task GraficaRegistrationPreRecordV2(Grid panel)
    //    {
    //        if (wrapPanelRegistrationAppointment.Children.Count > 0) wrapPanelRegistrationAppointment.Children.Clear();
    //        TextBlock textBlockRegistrationAppointment = new TextBlock();
    //        textBlockRegistrationAppointment.FontFamily = new FontFamily("Area");
    //        textBlockRegistrationAppointment.FontSize = 60;
    //        textBlockRegistrationAppointment.TextWrapping = TextWrapping.Wrap;
    //        textBlockRegistrationAppointment.HorizontalAlignment = HorizontalAlignment.Center;
    //        textBlockRegistrationAppointment.Foreground = new SolidColorBrush(Color.FromRgb(25, 51, 10));
    //        textBlockRegistrationAppointment.Text = "Введите код";

    //        //ошибки при вводе
    //        TextBlock textBlockRegistrationAppointmentError = new TextBlock();
    //        textBlockRegistrationAppointmentError.FontFamily = new FontFamily("Area");
    //        textBlockRegistrationAppointmentError.FontSize = 30;
    //        textBlockRegistrationAppointmentError.TextWrapping = TextWrapping.Wrap;
    //        textBlockRegistrationAppointmentError.HorizontalAlignment = HorizontalAlignment.Center;
    //        textBlockRegistrationAppointmentError.Foreground = new SolidColorBrush(Colors.Red);

    //        //поле для ввода
    //        TextBox textBoxRegistrationAppointment = new TextBox();
    //        textBoxRegistrationAppointment.FontSize = 50;
    //        textBoxRegistrationAppointment.Foreground = new SolidColorBrush(Color.FromRgb(25, 51, 100));
    //        textBoxRegistrationAppointment.Margin = new Thickness(0, 20, 0, 20);
    //        textBoxRegistrationAppointment.Width = 310;
    //        textBoxRegistrationAppointment.TextWrapping = TextWrapping.Wrap;
    //        textBoxRegistrationAppointment.Focus();

    //        StackPanel stackPanelHeadRegistrationAppointment = new StackPanel();
    //        stackPanelHeadRegistrationAppointment.Orientation = Orientation.Vertical;
    //        stackPanelHeadRegistrationAppointment.VerticalAlignment = VerticalAlignment.Top;
    //        stackPanelHeadRegistrationAppointment.Children.Add(textBlockRegistrationAppointment);
    //        stackPanelHeadRegistrationAppointment.Children.Add(textBlockRegistrationAppointmentError);
    //        stackPanelHeadRegistrationAppointment.Children.Add(textBoxRegistrationAppointment);

    //        wrapPanelRegistrationAppointment.Children.Add(stackPanelHeadRegistrationAppointment);

    //        //клавиатура
    //        StackPanel stackPanelKeyboardRegistrationAppointment = new StackPanel();
    //        stackPanelKeyboardRegistrationAppointment.Children.Add((StackPanel)this.FindResource("KeyboardNumberRegistrationAppointment"));
    //        bool upperCase = false;
    //        foreach (StackPanel item in stackPanelKeyboardRegistrationAppointment.Children)
    //        {
    //            foreach (StackPanel stackPanel in item.Children)
    //            {
    //                foreach (Button button in stackPanel.Children)
    //                {
    //                    button.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
    //                    button.Foreground = new SolidColorBrush(Colors.Brown);
    //                    button.BorderBrush = button.Foreground;
    //                    button.FontWeight = FontWeights.Black;
    //                    button.FontSize = 20;
    //                    button.Margin = new Thickness(5);
    //                    button.Click += async (s, e) =>
    //                    {
    //                        Button buttonClick = (Button)s;
    //                        textBlockRegistrationAppointmentError.Text = "";
    //                        textBoxRegistrationAppointment.BorderBrush = new SolidColorBrush(Colors.Black);
    //                        textBoxRegistrationAppointment.Foreground = new SolidColorBrush(Colors.Black);
    //                        switch (buttonClick.Content.ToString())
    //                        {
    //                            case "Удалить":
    //                                textBoxRegistrationAppointment.Text = textBoxRegistrationAppointment.Text.Length == 0 ? "" : textBoxRegistrationAppointment.Text.Substring(0, textBoxRegistrationAppointment.Text.Length - 1);
    //                                break;
    //                            case "Ввод":
    //                                if (textBoxRegistrationAppointment.Text.Length != 4)
    //                                {
    //                                    textBlockRegistrationAppointmentError.Text = "Код не коректен !";
    //                                    textBoxRegistrationAppointment.BorderBrush = new SolidColorBrush(Colors.Red);
    //                                    textBoxRegistrationAppointment.Foreground = new SolidColorBrush(Colors.Red);
    //                                }
    //                                else
    //                                {
    //                                    var prerecord = eqContext.DTicketPrerecords.Where(d => d.CodePrerecord == Convert.ToInt64(textBoxRegistrationAppointment.Text) && d.SOfficeId == officeId).FirstOrDefault();
    //                                    //if (prerecord == null)
    //                                    //{
    //                                    //    textBlockRegistrationAppointmentError.Text = "Неверный код !";
    //                                    //    textBoxRegistrationAppointment.BorderBrush = new SolidColorBrush(Colors.Red);
    //                                    //    textBoxRegistrationAppointment.Foreground = new SolidColorBrush(Colors.Red);
    //                                    //}
    //                                    //else
    //                                    //if (prerecord.DatePrerecord > DateOnly.FromDateTime(DateTime.Now) || prerecord.StartTimePrerecord > TimeOnly.FromDateTime(DateTime.Now))
    //                                    //{
    //                                    //    textBlockRegistrationAppointmentError.Text = "Время предварительной записи не вышло.Вы должны явиться " + prerecord.DatePrerecord + " с " + prerecord.StartTimePrerecord + " по " + prerecord.StopTimePrerecord;
    //                                    //}
    //                                    //else
    //                                    //if (prerecord.DatePrerecord < DateOnly.FromDateTime(DateTime.Now) || prerecord.StopTimePrerecord < TimeOnly.FromDateTime(DateTime.Now))
    //                                    //{
    //                                    //    textBlockRegistrationAppointmentError.Text = "Время предварительной записи вышло.Вы должны были явиться " + prerecord.DatePrerecord + " с " + prerecord.StartTimePrerecord + " по " + prerecord.StopTimePrerecord;
    //                                    //}
    //                                    //else
    //                                    //{
    //                                    //    TextBlock textBlockRegistrationAppointmentBtn = new TextBlock();
    //                                    //    textBlockRegistrationAppointmentBtn.FontFamily = new FontFamily("Area");
    //                                    //    textBlockRegistrationAppointmentBtn.FontSize = 36;
    //                                    //    textBlockRegistrationAppointmentBtn.TextWrapping = TextWrapping.Wrap;
    //                                    //    textBlockRegistrationAppointmentBtn.HorizontalAlignment = HorizontalAlignment.Center;
    //                                    //    textBlockRegistrationAppointmentBtn.Foreground = new SolidColorBrush(Color.FromRgb(25, 51, 10));
    //                                    //    textBlockRegistrationAppointmentBtn.Text = "Теперь выберите услугу";

    //                                    //    WrapPanel wrapPanelRegistrationAppointmentButtons = new WrapPanel();
    //                                    //    wrapPanelRegistrationAppointmentButtons.Name = "wrapPanelRegistrationAppointmentButtons";
    //                                    //    eqContext.SOfficeTerminalButtons.Where(s => s.SOfficeTerminal.IpAddress == Ip && !s.ParentId.HasValue).OrderBy(o => o.ButtonType).ToList().ForEach(b =>
    //                                    //    {
    //                                    //        Test(b, wrapPanelRegistrationAppointmentButtons, prerecord.Id);
    //                                    //    });
    //                                    //    wrapPanelRegistrationAppointment.Children.Clear();
    //                                    //    wrapPanelRegistrationAppointment.Children.Add(textBlockRegistrationAppointmentBtn);
    //                                    //    wrapPanelRegistrationAppointment.Children.Add(wrapPanelRegistrationAppointmentButtons);
    //                                    //}
    //                                }
    //                                break;
    //                            default:
    //                                textBoxRegistrationAppointment.Text += textBoxRegistrationAppointment.Text.Length == 4 ? "" : upperCase ? button.Content.ToString().ToLower() : button.Content.ToString().ToUpper();
    //                                break;
    //                        }
    //                        textBoxRegistrationAppointment.CaretIndex = textBoxRegistrationAppointment.Text.Length;
    //                        textBoxRegistrationAppointment.Focus();
    //                    };
    //                }
    //            }
    //        }

    //        wrapPanelRegistrationAppointment.Children.Add(stackPanelKeyboardRegistrationAppointment);

    //        //foreach (WrapPanel obj in BodyWindow.Children)
    //        //{
    //        //    obj.Visibility = Visibility.Collapsed;
    //        //    if (obj.Name == "RegistrationAppointment")
    //        //    {
    //        //        obj.Visibility = Visibility.Visible;
    //        //    }
    //        //}
    //        //StackClose.Visibility = Visibility.Visible;

    //        foreach (StackPanel stackPanel in Buttons_Service.Children)
    //        {
    //            Button button = (Button)stackPanel.Children[0];
    //            button.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
    //        }
    //        Button_Click_RegistrationAppointment.Background = new SolidColorBrush(Color.FromRgb(240, 250, 220));
    //    };

    //    void Test(SOfficeTerminalButton terminalBtn, WrapPanel panel, long preRecordId)
    //    {
    //        Button btn;
    //        if (terminalBtn.ButtonType == 1)
    //        {
    //            //btn = new ButtonMenu(terminalBtn, settingsColor);
    //            //btn.Click += (s, e) =>
    //            //{
    //            //    var SOfficeTerminalButton = eqContext.SOfficeTerminalButtons.Where(q => q.SOfficeTerminalId == terminalBtn.SOfficeTerminalId && q.ParentId == terminalBtn.Id);
    //            //    panel.Children.Clear();
    //            //    SOfficeTerminalButton.ToList().ForEach(button =>
    //            //    {
    //            //        Test(button, panel,preRecordId);
    //            //    });

    //            //};
    //        }
    //        else
    //        {
    //            SService sServices = eqContext.SServices.First(f => f.Id == terminalBtn.SServiceId);
    //            //btn = new ButtonAction(settingsColor, terminalBtn.ButtonName,sServices, Ip, preRecordId: preRecordId);
    //            //btn.Click += (s, e) =>
    //            //{
    //            //  Home(s, e);
    //            //};

    //        }
    //        //panel.Children.Add(btn);
    //    }

    //    //BodyWindow.Children.Add(wrapPanelRegistrationAppointment);
    //}

}
