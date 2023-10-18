using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using СontrollerEQ.Context;
using Function;
using static Function.TicketCall;
using System.Runtime.ConstrainedExecution;

namespace СontrollerEQ
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                //подключение к базе
                EqContext eqContext = new EqContext();
                IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
                string IpOffise = "";

                foreach (IPAddress address in localIPs)
                {
                    if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        IpOffise = address.ToString();
                    }
                }
                var windows = eqContext.SOfficeWindows.Where(s => s.WindowIp == IpOffise);
                if (windows.Any())
                {
                    WindowName.Text = windows.First().WindowName;

                }
            }
            catch (Exception ex)
            {

            }
        }

        //Клик "Вызвать"
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CallOperation(2);
            Call.Visibility = Visibility.Collapsed;
            StartServicing.Visibility = Visibility.Visible;
            DidntUp.Visibility = Visibility.Visible;
        }

        //Клик "Начать обслуживание"
        private void StartServicing_Click(object sender, RoutedEventArgs e)
        {
            CallOperation(3);

            StartServicing.Visibility = Visibility.Collapsed;
            DidntUp.Visibility = Visibility.Collapsed;
            Call.Visibility = Visibility.Visible;
        }

        //Клик "Не явился"
        private void DidntUp_Click(object sender, RoutedEventArgs e)
        {
            CallOperation(8);
            StartServicing.Visibility = Visibility.Collapsed;
            DidntUp.Visibility = Visibility.Collapsed;
            Call.Visibility = Visibility.Visible;
        }

        // Передать
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            StartServicing.Visibility = Visibility.Collapsed;
            DidntUp.Visibility = Visibility.Collapsed;
            Call.Visibility = Visibility.Visible;

            //подключение к базе
            EqContext eqContext = new EqContext();
            IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());

            string IpOffise = ""; 
            foreach (IPAddress address in localIPs)
            {
                if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    IpOffise = address.ToString();
                }
            }

            var windows = eqContext.SOfficeWindows.Where(s => s.WindowIp == IpOffise);
            if (windows.Any())
            {
                SOfficeWindow sOfficeWindow = windows.First();
                var ticketCall = TicketCall.GetNextTicket(sOfficeWindow.Id);
                if (ticketCall.Id != null)
                { 
                    List<SelectWindowResult> windowResult = WindowResult(ticketCall.Id);
                    if (windowResult.Any())
                    {
                        // Создание и отображение нового окна
                        Window newWindow = new Window();
                        newWindow.Title = "Окна передачи";
                        newWindow.Width = 150;
                        newWindow.Height = 250;
                        newWindow.Owner = this;
                        WrapPanel wrapPanelButtons = new WrapPanel();
                        wrapPanelButtons.HorizontalAlignment = HorizontalAlignment.Center;
                        wrapPanelButtons.VerticalAlignment = VerticalAlignment.Center;

                        WrapPanel wrapPanelButtonOk = new WrapPanel();
                        wrapPanelButtonOk.HorizontalAlignment = HorizontalAlignment.Center;
                        wrapPanelButtonOk.VerticalAlignment = VerticalAlignment.Bottom;

                        Button btnOk = new Button();
                        btnOk.Content = "Передать";
                        btnOk.HorizontalAlignment = HorizontalAlignment.Center;
                        btnOk.VerticalAlignment = VerticalAlignment.Center;
                        btnOk.Height = 40;
                        btnOk.Width = 100;
                        btnOk.Visibility = Visibility.Hidden;
                        btnOk.Margin = new Thickness(10);
                        btnOk.Background = new SolidColorBrush(Colors.Green);
                        btnOk.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 250, 255));
                        btnOk.FontFamily = new FontFamily("Area");
                        btnOk.FontSize = 16;
                        btnOk.Foreground = new SolidColorBrush(Color.FromRgb(252, 252, 240));

                        windowResult.ForEach(s =>
                        {
                            Button btnWindow = new Button();
                            btnWindow.Content = s.WindowName;
                            btnWindow.HorizontalAlignment = HorizontalAlignment.Center;
                            btnWindow.VerticalAlignment = VerticalAlignment.Center;
                            btnWindow.Height = 50;
                            btnWindow.Width = 125;
                            btnWindow.Visibility = Visibility.Visible;
                            btnWindow.Margin = new Thickness(10);
                            btnWindow.Background = new SolidColorBrush(Color.FromRgb(81, 96, 151));
                            btnWindow.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 250, 255));
                            btnWindow.FontFamily = new FontFamily("Area");
                            btnWindow.FontSize = 18;
                            btnWindow.Foreground = new SolidColorBrush(Color.FromRgb(252, 252, 240));
                            btnWindow.Click += (s, e) =>
                            {
                                btnWindow.Background = new SolidColorBrush(Color.FromRgb(101, 116, 171));
                                btnOk.Visibility = Visibility.Visible;
                                btnOk.Click += (s, e) =>
                                {
                                    CallOperation(4);
                                    newWindow.Close();
                                };
                            };
                            wrapPanelButtons.Children.Add(btnWindow);
                            newWindow.Width += 150;
                        });

                        double left = Left + (Width - newWindow.Width) / 2;
                        double top = Top + (Height - newWindow.Height) / 2;
                        newWindow.Left = left;
                        newWindow.Top = top;

                        wrapPanelButtonOk.Children.Add(btnOk);
                        WrapPanel wrapPanel = new WrapPanel();
                        wrapPanel.Orientation = Orientation.Vertical;
                        wrapPanel.HorizontalAlignment = HorizontalAlignment.Center;
                        wrapPanel.VerticalAlignment = VerticalAlignment.Center;
                        wrapPanel.Margin = new Thickness(40);

                        wrapPanel.Children.Add(wrapPanelButtons);
                        wrapPanel.Children.Add(wrapPanelButtonOk);

                        newWindow.Content = wrapPanel;

                        newWindow.ShowDialog();

                    }
                }
            } 
        }

        // Отложить
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            StartServicing.Visibility = Visibility.Collapsed;
            DidntUp.Visibility = Visibility.Collapsed;
            Call.Visibility = Visibility.Visible;

            // Создаем и отображаем модальное окно подтверждения
            ConfirmationDialog confirmationDialog = new ConfirmationDialog("Вы уверены, что хотите выполнить это действие?");
            confirmationDialog.Owner = this; // Устанавливаем главное окно владельцем дочернего окна

            // Расчет координат для центрирования окна
            double left = Left + (Width - confirmationDialog.Width) / 2;
            double top = Top + (Height - confirmationDialog.Height) / 2;

            confirmationDialog.Left = left;
            confirmationDialog.Top = top;

            bool? result = confirmationDialog.ShowDialog();

            // Обработка результата модального окна
            if (result == true)
            {
                // Действие подтверждено
                CallOperation(5);
            }
            else
            {
                // Действие отменено или окно закрыто
                // Выполните необходимые действия здесь
            } 
        }

        // Завершение талона
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            StartServicing.Visibility = Visibility.Collapsed;
            DidntUp.Visibility = Visibility.Collapsed;
            Call.Visibility = Visibility.Visible;

            CallOperation(6); 
        }

        // дабавление статуса
        private void CallOperation(int statusId)
        {
            try
            {
                //подключение к базе
                EqContext eqContext = new EqContext();
                IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
                string IpOffise = "";

                foreach (IPAddress address in localIPs)
                {
                    if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        IpOffise = address.ToString();
                    }
                }

                var windows = eqContext.SOfficeWindows.Where(s => s.WindowIp == IpOffise);
                if (windows.Any())
                {
                    SOfficeWindow sOfficeWindow = windows.First();
                    var ticketCall = TicketCall.GetNextTicket(sOfficeWindow.Id);
                    if (ticketCall.Id != 0)
                    {
                        DTicketStatus dTicketStatus = new DTicketStatus
                        {
                            SStatusId = statusId,
                            DTicketId = ticketCall.Id,
                            SOfficeWindowId = sOfficeWindow.Id,
                            SEmployeeId = 1,
                            EmployeeNameAdd = "",
                        };

                        eqContext.DTicketStatuses.Add(dTicketStatus);
                        eqContext.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {

            }
             
        }

    }
}

// модальное окно
public class ConfirmationDialog : Window
{
    public ConfirmationDialog(string message)
    {
        Title = "Подтверждение";
        HorizontalAlignment = HorizontalAlignment.Center;
        VerticalAlignment = VerticalAlignment.Center;
        Width = 255;
        Height = 130;
        AllowDrop = false;
        AllowsTransparency = false;
        WindowStyle = WindowStyle.None;
        WrapPanel mainPanel = new WrapPanel();
        
        // Создание текстового блока с сообщением
        TextBlock textBlock = new TextBlock();
        textBlock.Text = message;
        textBlock.FontSize = 16;
        textBlock.Foreground = new SolidColorBrush(Colors.Brown);
        textBlock.TextWrapping = TextWrapping.Wrap;
        textBlock.Margin= new Thickness(10);
        mainPanel.Children.Add(textBlock);

        // Создание панели с кнопками для подтверждения или отмены действия
        WrapPanel buttonPanel = new WrapPanel();
        buttonPanel.Orientation = Orientation.Horizontal;

        Button confirmButton = new Button(); 
        confirmButton.HorizontalAlignment = HorizontalAlignment.Center;
        confirmButton.VerticalAlignment = VerticalAlignment.Center;
        confirmButton.Height = 30;
        confirmButton.Width = 100;
        confirmButton.Margin = new Thickness(32, 18, 0, 0);
        confirmButton.Background = new SolidColorBrush(Colors.Green);
        confirmButton.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 250, 255));
        confirmButton.FontFamily = new FontFamily("Area");
        confirmButton.FontSize = 15;
        confirmButton.Foreground = new SolidColorBrush(Color.FromRgb(252, 252, 240)); 
        confirmButton.Content = "Подтвердить";
        confirmButton.Margin = new Thickness(10);
        confirmButton.Click += (sender, e) => { DialogResult = true; };

        Button cancelButton = new Button();
        cancelButton.HorizontalAlignment = HorizontalAlignment.Center;
        cancelButton.VerticalAlignment = VerticalAlignment.Center;
        cancelButton.Height = 30;
        cancelButton.Width = 100;
        cancelButton.Margin = new Thickness(32, 18, 0, 0);
        cancelButton.Background = new SolidColorBrush(Colors.Red);
        cancelButton.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 250, 255));
        cancelButton.FontFamily = new FontFamily("Area");
        cancelButton.FontSize = 15;
        cancelButton.Foreground = new SolidColorBrush(Color.FromRgb(252, 252, 240));
        cancelButton.Margin = new Thickness(10); 
        cancelButton.Content = "Отмена"; 
        cancelButton.Click += (sender, e) => { DialogResult = false; };

        buttonPanel.Children.Add(confirmButton);
        buttonPanel.Children.Add(cancelButton);

        mainPanel.Children.Add(buttonPanel);

        Content = mainPanel;
    }
}