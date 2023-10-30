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
using System.Net.Sockets;
using СontrollerEQ.Modal;
using System.Windows.Media.Effects;
using System.Runtime.Intrinsics.X86;
using System.Windows.Controls.Primitives;
using System.Security.Cryptography.Xml;
using System.Diagnostics.Metrics;

namespace СontrollerEQ
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// ip adress компитера
        /// </summary>
        public static string Ip { get; set; }

        /// <summary>
        /// текущий талон
        /// </summary>
        static Ticket TicketLive { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            GetIp();
            Main();
            Closing += MainWindow_Closing;
        }

        #region закритие пиложения
        private async void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Здесь можно разместить код для обработки закрытия приложения 
            CallOperation(6, TicketLive.Id);
            await Client.SendMessageAsync("Pocess", Ip); 
        }
        #endregion

        #region Server
        public async Task StartListeningAsync(Window window, string ip)
        {
            TcpListener listener = new TcpListener(IPAddress.Parse(ip), 1234);
            listener.Start();
            Console.WriteLine("Сервер запущен. Ожидание подключений...");

            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                _ = HandleClientAsync(client);
            }
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            using (client)
            {
                byte[] buffer = new byte[1024];
                StringBuilder messageBuilder = new StringBuilder();

                using (NetworkStream stream = client.GetStream())
                {
                    int bytesRead;

                    while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        messageBuilder.Append(receivedMessage);
                    }
                }

                string message = messageBuilder.ToString();
                if (message == "new Ticket")
                {
                    Main();
                }
            }
        }

        #endregion

        #region Main
        async void Main()
        {
            try
            {
                //подключение к базе
                EqContext eqContext = new EqContext();
                var windows = eqContext.SOfficeWindows.Where(s => s.WindowIp == Ip);

                if (windows.Any())
                {
                    var window = windows.First();
                    WindowName.Text = window.WindowName;
                    var ticket = GetNextTicket(window.Id);
                    TicketLive = ticket;
                    TicketName.Text = TicketLive.TicketNumberFull == null ? "---" : TicketLive.TicketNumberFull;
                    CallTextBlock.Text = TicketLive.TicketNumberFull == null ? "Вызвать ..." : "Вызвать " + TicketLive.TicketNumberFull;

                    #region Все талоны
                    var tickets = eqContext.DTickets.Where(s => s.SOfficeId == window.SOfficeId && s.SStatusId == 1 && s.DateRegistration == DateOnly.FromDateTime(DateTime.Now)).OrderBy(r => r.TicketNumber);
                    ComboBoxTicketItems.Items.Clear();
                    if (tickets.Any())
                    {
                        tickets.ToList().ForEach(ticket =>
                        {
                            ComboBoxItem comboBoxItem = new ComboBoxItem();
                            comboBoxItem.Focusable = false;

                            WrapPanel wrapPanelComboBoxItem = new WrapPanel();
                            wrapPanelComboBoxItem.Orientation = Orientation.Horizontal;

                            TextBlock textBlock = new TextBlock();
                            textBlock.Text = ticket.TicketNumberFull;
                            textBlock.FontFamily = new FontFamily("Arial");
                             
                            textBlock.HorizontalAlignment = HorizontalAlignment.Left;
                            textBlock.TextAlignment = TextAlignment.Center;
                            textBlock.VerticalAlignment = VerticalAlignment.Center;
                            textBlock.Foreground = new SolidColorBrush(Colors.White);
                            textBlock.Background = new SolidColorBrush(Colors.Black);
                            textBlock.FontSize = 16;

                            Button button = new Button();
                            button.Content = "Вызвать 🔊";
                            button.FontSize = 14;
                            button.HorizontalAlignment = HorizontalAlignment.Right;
                            button.Foreground = new SolidColorBrush(Colors.White);
                            button.FontFamily = new FontFamily("Arial");
                            button.Background = new SolidColorBrush(Colors.ForestGreen);

                            button.Click += (s, e) =>
                            {
                                CallFromList(s, e, ticket);
                            };

                            wrapPanelComboBoxItem.Children.Add(textBlock);
                            wrapPanelComboBoxItem.Children.Add(button);

                            comboBoxItem.Content = wrapPanelComboBoxItem;
                            ComboBoxTicketItems.Items.Add(comboBoxItem);
                        });
                    }
                    CountClient.Text = tickets.Count().ToString();
                    #endregion

                    #region Переданные талоны

                    TransferComboBox.Items.Clear();
                    var tticketsTransfer = TicketTransferred.SelectTicketTransferred(window.Id);
                    TransferCount.Text = tticketsTransfer.Count().ToString();
                    if (tticketsTransfer.Any())
                    {
                        tticketsTransfer.ToList().ForEach(ticketTransfer =>
                        {
                            ComboBoxItem comboBoxItem = new ComboBoxItem();
                            comboBoxItem.Focusable = false;

                            WrapPanel wrapPanelComboBoxItem = new WrapPanel();
                            wrapPanelComboBoxItem.Orientation = Orientation.Horizontal;

                            TextBlock textBlock = new TextBlock();
                            textBlock.Text = ticketTransfer.TicketNumberFull;
                            textBlock.FontFamily = new FontFamily("Arial"); 
                            textBlock.HorizontalAlignment = HorizontalAlignment.Left;
                            textBlock.TextAlignment = TextAlignment.Center;
                            textBlock.VerticalAlignment = VerticalAlignment.Center;
                            textBlock.Foreground = new SolidColorBrush(Colors.White);
                            textBlock.Background = new SolidColorBrush(Colors.Black);
                            textBlock.FontSize = 16;

                            Button button = new Button();
                            button.Content = "Вызвать 🔊";
                            button.FontSize = 14;
                            button.TabIndex = 1000;
                            button.HorizontalAlignment = HorizontalAlignment.Right;
                            button.Foreground = new SolidColorBrush(Colors.White);
                            button.FontFamily = new FontFamily("Arial");
                            button.Background = new SolidColorBrush(Colors.ForestGreen);

                            button.Click += (s, e) =>
                            {
                                CallFromList(s, e, new DTicket { Id = ticketTransfer.Id, TicketNumberFull = ticketTransfer.TicketNumberFull });
                            };


                            wrapPanelComboBoxItem.Children.Add(textBlock);
                            wrapPanelComboBoxItem.Children.Add(button);

                            comboBoxItem.Content = wrapPanelComboBoxItem;
                            TransferComboBox.Items.Add(comboBoxItem);
                        });
                    }
                    #endregion

                    #region Отложенные талоны
                    DeferComboBox.Items.Clear();
                    var ticketsPostponed = TicketPostponed.SelectTicketPostponed(window.Id);
                    DeferCount.Text = ticketsPostponed.Count().ToString();
                    if (ticketsPostponed.Any())
                    {
                        ticketsPostponed.ToList().ForEach(ticketPostponed =>
                        {
                            ComboBoxItem comboBoxItem = new ComboBoxItem();
                            comboBoxItem.Focusable = false;

                            WrapPanel wrapPanelComboBoxItem = new WrapPanel();
                            wrapPanelComboBoxItem.Orientation = Orientation.Horizontal;

                            TextBlock textBlock = new TextBlock();
                            textBlock.Text = ticketPostponed.TicketNumberFull;
                            textBlock.FontFamily = new FontFamily("Arial"); 
                            textBlock.HorizontalAlignment = HorizontalAlignment.Left;
                            textBlock.TextAlignment = TextAlignment.Center;
                            textBlock.VerticalAlignment = VerticalAlignment.Center;
                            textBlock.Foreground = new SolidColorBrush(Colors.White);
                            textBlock.Background = new SolidColorBrush(Colors.Black);
                            textBlock.FontSize = 16;

                            Button button = new Button();
                            button.Content = "Вызвать 🔊";
                            button.FontSize = 14;
                            button.TabIndex = 1000;
                            button.HorizontalAlignment = HorizontalAlignment.Right;
                            button.Foreground = new SolidColorBrush(Colors.White);
                            button.FontFamily = new FontFamily("Arial");
                            button.Background = new SolidColorBrush(Colors.ForestGreen);

                            button.Click += (s, e) =>
                            {
                                CallFromList(s, e, new DTicket { Id = ticketPostponed.Id, TicketNumberFull = ticketPostponed.TicketNumberFull });
                            };
                            wrapPanelComboBoxItem.Children.Add(textBlock);
                            wrapPanelComboBoxItem.Children.Add(button);

                            comboBoxItem.Content = wrapPanelComboBoxItem;
                            DeferComboBox.Items.Add(comboBoxItem);
                        });
                    }

                    CountClient.Text = (tticketsTransfer.Count() + tickets.Count() + ticketsPostponed.Count()).ToString();
                    #endregion


                    await StartListeningAsync(this, Ip);
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region Вызвать
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (TicketLive.TicketNumberFull != null)
            {
                CallOperation(2, TicketLive.Id);
                TicketName.Text = TicketLive.TicketNumberFull + " Ожидание";
                StartServicing.Visibility = Visibility.Visible;

                DidntUp.Visibility = Visibility.Visible;

                Call.Visibility = Visibility.Collapsed;
                PreRegistration.Visibility = Visibility.Collapsed;
                Transfer.Visibility = Visibility.Collapsed;
                Defer.Visibility = Visibility.Collapsed;
                Finish.Visibility = Visibility.Collapsed;
                TransferBlockList.Visibility = Visibility.Collapsed;
                DeferBlockList.Visibility = Visibility.Collapsed;
                await Client.SendMessageAsync("Call:" + TicketLive.Id, Ip);
            }
        }
        #endregion

        #region Вызов из списка
        private async void CallFromList(object sender, RoutedEventArgs e, DTicket ticket)
        {
            TicketLive.TicketNumberFull = ticket.TicketNumberFull;
            TicketLive.Id = ticket.Id;
            if (TicketLive.TicketNumberFull != null)
            {
                CallOperation(2, TicketLive.Id);
                TicketName.Text = TicketLive.TicketNumberFull + " Ожидание";
                StartServicing.Visibility = Visibility.Visible;

                DidntUp.Visibility = Visibility.Visible;

                Call.Visibility = Visibility.Collapsed;
                PreRegistration.Visibility = Visibility.Collapsed;
                Transfer.Visibility = Visibility.Collapsed;
                Defer.Visibility = Visibility.Collapsed;
                Finish.Visibility = Visibility.Collapsed;
                TransferBlockList.Visibility = Visibility.Collapsed;
                DeferBlockList.Visibility = Visibility.Collapsed;
                await Client.SendMessageAsync("Call:" + TicketLive.Id, Ip);
            }
        }
        #endregion

        #region Начать обслуживание
        private async void StartServicing_Click(object sender, RoutedEventArgs e)
        {
            TicketName.Text = TicketLive.TicketNumberFull + " Обслуживание";
            CallOperation(3, TicketLive.Id);
            Finish.Visibility = Visibility.Visible;
            Transfer.Visibility = Visibility.Visible;
            Defer.Visibility = Visibility.Visible;

            DidntUp.Visibility = Visibility.Collapsed;
            StartServicing.Visibility = Visibility.Collapsed;
            await Client.SendMessageAsync("Pocess", Ip);
        }
        #endregion

        #region Не явился
        private void DidntUp_Click(object sender, RoutedEventArgs e)
        {
            TicketName.Text = TicketLive.TicketNumberFull;
            CallOperation(8, TicketLive.Id);
            Call.Visibility = Visibility.Visible;
            PreRegistration.Visibility = Visibility.Visible;
            TransferBlockList.Visibility = Visibility.Visible;
            DeferBlockList.Visibility = Visibility.Visible;
            DidntUp.Visibility = Visibility.Collapsed;
            StartServicing.Visibility = Visibility.Collapsed;
            Main();
        }
        #endregion

        #region Передать
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            StartServicing.Visibility = Visibility.Collapsed;
            DidntUp.Visibility = Visibility.Collapsed;

            //подключение к базе
            EqContext eqContext = new EqContext();

            try
            {
                var windows = eqContext.SOfficeWindows.Where(s => s.WindowIp == Ip);
                if (windows.Any())
                {
                    SOfficeWindow sOfficeWindow = windows.First();
                    List<SelectWindowResult> windowResult = WindowResult(TicketLive.Id);
                    if (windowResult.Any())
                    {
                        // Создание и отображение нового окна
                        Window newWindow = new Window();
                        newWindow.Title = "Окна передачи";
                        newWindow.Width = 150;
                        newWindow.Background = new SolidColorBrush(Colors.Black);
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

                        windowResult.ForEach(windowItem =>
                            {
                                Button btnWindow = new Button();
                                btnWindow.Content = windowItem.WindowName;
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
                                    btnOk.Click += async (s, e) =>
                                    {
                                        CallOperation(4, TicketLive.Id, windowItem.SOfficeWindowId);
                                        await Client.SendMessageAsync("Pocess", Ip);
                                        Main();
                                        newWindow.Close();
                                        StartServicing.Visibility = Visibility.Collapsed;
                                        DidntUp.Visibility = Visibility.Collapsed;
                                        Finish.Visibility = Visibility.Collapsed;
                                        Defer.Visibility = Visibility.Collapsed;
                                        Transfer.Visibility = Visibility.Collapsed;

                                        TransferBlockList.Visibility = Visibility.Visible;
                                        DeferBlockList.Visibility = Visibility.Visible;
                                        Call.Visibility = Visibility.Visible;
                                        PreRegistration.Visibility = Visibility.Visible;
                                    };
                                };
                                wrapPanelButtons.Children.Add(btnWindow);
                                newWindow.Width += 175;
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
            catch (Exception ex)
            {


            }
        }
        #endregion

        #region Отложить
        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {

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
                CallOperation(5, TicketLive.Id);
                Call.Visibility = Visibility.Visible;
                PreRegistration.Visibility = Visibility.Visible;
                TransferBlockList.Visibility = Visibility.Visible;
                DeferBlockList.Visibility = Visibility.Visible;

                Finish.Visibility = Visibility.Collapsed;
                Transfer.Visibility = Visibility.Collapsed;
                Defer.Visibility = Visibility.Collapsed;
                await Client.SendMessageAsync("Pocess", Ip);
                Main();
            }
            else
            {
                // Действие отменено или окно закрыто
                // Выполните необходимые действия здесь
            }
        }
        #endregion

        #region Завершение талона
        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {
            StartServicing.Visibility = Visibility.Collapsed;
            DidntUp.Visibility = Visibility.Collapsed;
            Finish.Visibility = Visibility.Collapsed;
            Defer.Visibility = Visibility.Collapsed;
            Transfer.Visibility = Visibility.Collapsed;

            Call.Visibility = Visibility.Visible;
            PreRegistration.Visibility = Visibility.Visible;
            TransferBlockList.Visibility = Visibility.Visible;
            DeferBlockList.Visibility = Visibility.Visible;

            CallOperation(6, TicketLive.Id);
            await Client.SendMessageAsync("Pocess", Ip);
            Main();
        }
        #endregion

        #region Предварительная запись
        //Первый предок

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            WrapPanel wrapPanelPreRegistrationMain = new WrapPanel();
            wrapPanelPreRegistrationMain.Name = "PreRegistration";
            wrapPanelPreRegistrationMain.Orientation = Orientation.Vertical;
            wrapPanelPreRegistrationMain.Visibility = Visibility.Collapsed;

            if (wrapPanelPreRegistrationMain.Children.Count > 0) wrapPanelPreRegistrationMain.Children.Clear();
            WrapPanel foteWrapPanel = new WrapPanel();
            foteWrapPanel.Orientation = Orientation.Horizontal;

            //Блок 1 этапа
            WrapPanel wrapPanelPreRegistrationStage1 = new WrapPanel();
            wrapPanelPreRegistrationStage1.HorizontalAlignment = HorizontalAlignment.Center;
            wrapPanelPreRegistrationStage1.Name = "PreRegistrationStage1";

            WrapPanel wrapPanelStage1Menu = new WrapPanel();
            wrapPanelStage1Menu.Name = "PreRegistrationStage1Menu";

            WrapPanel wrapPanelStage1Buttons = new WrapPanel();
            wrapPanelStage1Buttons.Name = "PreRegistrationStage1Buttons";

            //Блок 2 этапа
            WrapPanel wrapPanelPreRegistrationStage2 = new WrapPanel();
            wrapPanelPreRegistrationStage2.HorizontalAlignment = HorizontalAlignment.Center;
            wrapPanelPreRegistrationStage2.Name = "PreRegistrationStage2";

            //Блок 3 этапа
            WrapPanel wrapPanelPreRegistrationStage3 = new WrapPanel();
            wrapPanelPreRegistrationStage3.HorizontalAlignment = HorizontalAlignment.Center;
            wrapPanelPreRegistrationStage3.Name = "PreRegistrationStage3";

            //Блок 4 этапа
            WrapPanel wrapPanelPreRegistrationStage4 = new WrapPanel();
            wrapPanelPreRegistrationStage4.HorizontalAlignment = HorizontalAlignment.Center;
            wrapPanelPreRegistrationStage4.VerticalAlignment = VerticalAlignment.Bottom;
            wrapPanelPreRegistrationStage4.Name = "PreRegistrationStage4";

            wrapPanelPreRegistrationStage1.Visibility = Visibility.Visible;
            wrapPanelStage1Menu.Visibility = Visibility.Visible;
            wrapPanelStage1Buttons.Visibility = Visibility.Collapsed;

            #region Кнопка далее и назад 
            Button btnBack = new Button();
            btnBack.Name = "Back";
            btnBack.Content = "Назад";
            btnBack.HorizontalAlignment = HorizontalAlignment.Left;
            btnBack.VerticalAlignment = VerticalAlignment.Bottom;
            btnBack.Height = 50;
            btnBack.Width = 100;
            btnBack.Background = new SolidColorBrush(Colors.FloralWhite);
            btnBack.FontFamily = new FontFamily("Area");
            btnBack.FontSize = 25;
            btnBack.Foreground = new SolidColorBrush(Colors.Red);
            btnBack.Visibility = Visibility.Hidden;
            btnBack.Margin = new Thickness(20);
            ControlTemplate myControlTemplateBack = new ControlTemplate(typeof(Button));
            FrameworkElementFactory borderBack = new FrameworkElementFactory(typeof(Border));
            borderBack.Name = "border";
            borderBack.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(Border.BackgroundProperty));
            borderBack.SetValue(Border.BorderBrushProperty, new TemplateBindingExtension(Border.BorderBrushProperty));
            borderBack.SetValue(Border.BorderThicknessProperty, new TemplateBindingExtension(Border.BorderThicknessProperty));
            borderBack.SetValue(Border.CornerRadiusProperty, new CornerRadius(10));
            FrameworkElementFactory contentPresenterBack = new FrameworkElementFactory(typeof(ContentPresenter));
            contentPresenterBack.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            contentPresenterBack.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
            borderBack.AppendChild(contentPresenterBack);
            myControlTemplateBack.VisualTree = borderBack;
            btnBack.Template = myControlTemplateBack;

            Button btnNextStage = new Button();
            btnNextStage.Name = "NextStage";
            btnNextStage.Content = "Далее";
            btnBack.HorizontalAlignment = HorizontalAlignment.Right;
            btnNextStage.VerticalAlignment = VerticalAlignment.Bottom;
            btnNextStage.Height = 50;
            btnNextStage.Width = 100;
            btnNextStage.Background = new SolidColorBrush(Colors.DarkGreen);
            btnNextStage.FontFamily = new FontFamily("Area");
            btnNextStage.FontSize = 25;
            btnNextStage.Margin = new Thickness(20);
            btnNextStage.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            btnNextStage.TabIndex = 999;
            btnNextStage.Visibility = Visibility.Hidden;
            ControlTemplate myControlTemplateNextStage = new ControlTemplate(typeof(Button));
            FrameworkElementFactory borderNextStage = new FrameworkElementFactory(typeof(Border));
            borderNextStage.Name = "border";
            borderNextStage.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(Border.BackgroundProperty));
            borderNextStage.SetValue(Border.BorderBrushProperty, new TemplateBindingExtension(Border.BorderBrushProperty));
            borderNextStage.SetValue(Border.BorderThicknessProperty, new TemplateBindingExtension(Border.BorderThicknessProperty));
            borderNextStage.SetValue(Border.CornerRadiusProperty, new CornerRadius(10));
            FrameworkElementFactory contentPresenterNextStage = new FrameworkElementFactory(typeof(ContentPresenter));
            contentPresenterNextStage.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            contentPresenterNextStage.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
            borderNextStage.AppendChild(contentPresenterNextStage);
            myControlTemplateNextStage.VisualTree = borderNextStage;
            btnNextStage.Template = myControlTemplateNextStage;

            #endregion

            TextBlock textBlockPreRegistration = new TextBlock();
            textBlockPreRegistration.Text = "Предварительная запись";
            textBlockPreRegistration.HorizontalAlignment = HorizontalAlignment.Center;
            textBlockPreRegistration.FontFamily = new FontFamily("Area");
            textBlockPreRegistration.FontSize = 25;
            textBlockPreRegistration.Margin = new Thickness(0, 30, 0, 0);
            textBlockPreRegistration.Foreground = new SolidColorBrush(Colors.White);
            wrapPanelPreRegistrationMain.Children.Add(textBlockPreRegistration);

            //подключение к базе
            EqContext eqContext = new EqContext();

            // меню и кнопки  Предварительная запись
            eqContext.SOfficeTerminalButtons.Where(s => s.SServiceId == eqContext.SOfficeWindows.First(f => f.WindowIp == Ip).SOfficeId).OrderBy(o => o.ButtonType).ToList().ForEach(b =>
            {
                if (b.ButtonType == 1) // 1 - Меню. 2 - Кнопка
                {
                    #region создаем кнопку перехода на меню 
                    Button btnMenu = new Button();
                    DropShadowEffect shadowEffectMenu = new DropShadowEffect();
                    shadowEffectMenu.Color = Colors.White;
                    shadowEffectMenu.ShadowDepth = 3;
                    btnMenu.Effect = shadowEffectMenu;
                    btnMenu.Name = "menu";
                    btnMenu.Content = b.ButtonName;
                    btnMenu.HorizontalAlignment = HorizontalAlignment.Center;
                    btnMenu.VerticalAlignment = VerticalAlignment.Top;
                    btnMenu.Height = 75;
                    btnMenu.Width = 200;
                    btnMenu.Margin = new Thickness(0, 18, 32, 0);
                    btnMenu.Background = new SolidColorBrush(Colors.DarkRed);
                    btnMenu.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 20));
                    btnMenu.FontFamily = new FontFamily("Area");
                    btnMenu.FontSize = 25;
                    btnMenu.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    btnMenu.TabIndex = 999;
                    ControlTemplate myControlTemplateMenu = new ControlTemplate(typeof(Button));
                    FrameworkElementFactory borderMenu = new FrameworkElementFactory(typeof(Border));
                    borderMenu.Name = "border";
                    borderMenu.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(Border.BackgroundProperty));
                    borderMenu.SetValue(Border.BorderBrushProperty, new TemplateBindingExtension(Border.BorderBrushProperty));
                    borderMenu.SetValue(Border.BorderThicknessProperty, new TemplateBindingExtension(Border.BorderThicknessProperty));
                    borderMenu.SetValue(Border.CornerRadiusProperty, new CornerRadius(10));
                    FrameworkElementFactory contentPresenterMenu = new FrameworkElementFactory(typeof(ContentPresenter));
                    contentPresenterMenu.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                    contentPresenterMenu.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
                    borderMenu.AppendChild(contentPresenterMenu);
                    myControlTemplateMenu.VisualTree = borderMenu;
                    btnMenu.Template = myControlTemplateMenu;
                    #endregion

                    //все кнопки этого меню
                    var SOfficeTerminalButton = eqContext.SOfficeTerminalButtons.Where(q => q.SOfficeTerminalId == b.SOfficeTerminalId && q.ParentId == b.ParentId && q.ButtonType != 1);

                    //создаем кнопки меню
                    List<SService> sServices = new List<SService>();
                    WrapPanel wrapPanelButtons = new WrapPanel();
                    wrapPanelButtons.Orientation = Orientation.Horizontal;
                    wrapPanelButtons.HorizontalAlignment = HorizontalAlignment.Center;
                    wrapPanelButtons.Visibility = Visibility.Collapsed;
                    wrapPanelButtons.MaxWidth = 800;


                    SOfficeTerminalButton.ToList().ForEach(button =>
                    {
                        Button btnStage1 = new Button();
                        btnStage1.Name = "button";
                        btnStage1.Content = button.ButtonName;
                        btnStage1.HorizontalAlignment = HorizontalAlignment.Center;
                        btnStage1.VerticalAlignment = VerticalAlignment.Top;
                        btnStage1.Height = 75;
                        btnStage1.Width = 200;
                        btnStage1.Margin = new Thickness(0, 18, 32, 0);
                        btnStage1.Background = new SolidColorBrush(Color.FromRgb(255, 250, 255));
                        btnStage1.BorderBrush = new SolidColorBrush(Color.FromRgb(55, 55, 55));
                        btnStage1.FontFamily = new FontFamily("Area");
                        btnStage1.FontSize = 25;
                        btnStage1.Foreground = new SolidColorBrush(Color.FromRgb(135, 98, 27));
                        DropShadowEffect btnShadowEffectStage1 = new DropShadowEffect();
                        btnShadowEffectStage1.Color = Color.FromRgb(22, 22, 22);
                        btnShadowEffectStage1.Direction = 50;
                        btnShadowEffectStage1.ShadowDepth = 2;
                        btnStage1.Effect = btnShadowEffectStage1;
                        ControlTemplate myControlTemplateStage1 = new ControlTemplate(typeof(Button));
                        FrameworkElementFactory borderStage1 = new FrameworkElementFactory(typeof(Border));
                        borderStage1.Name = "border";
                        borderStage1.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(Border.BackgroundProperty));
                        borderStage1.SetValue(Border.BorderBrushProperty, new TemplateBindingExtension(Border.BorderBrushProperty));
                        borderStage1.SetValue(Border.BorderThicknessProperty, new TemplateBindingExtension(Border.BorderThicknessProperty));
                        borderStage1.SetValue(Border.CornerRadiusProperty, new CornerRadius(10));
                        FrameworkElementFactory contentPresenterStage1 = new FrameworkElementFactory(typeof(ContentPresenter));
                        contentPresenterStage1.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                        contentPresenterStage1.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
                        borderStage1.AppendChild(contentPresenterStage1);
                        myControlTemplateStage1.VisualTree = borderStage1;
                        btnStage1.Template = myControlTemplateStage1;
                        wrapPanelButtons.Children.Add(btnStage1);
                        btnStage1.Click += (s, e) =>
                        {
                            ClickBtnPreReg(s, e,
                                button: button,
                                wrapPanelStage1Menu: wrapPanelStage1Menu,
                                wrapPanelButtons: wrapPanelButtons,
                                btnNextStage: btnNextStage,
                                btnBack: btnBack,
                                wrapPanelPreRegistrationStage1: wrapPanelPreRegistrationStage1,
                                wrapPanelPreRegistrationStage2: wrapPanelPreRegistrationStage2,
                                wrapPanelPreRegistrationStage3: wrapPanelPreRegistrationStage3,
                                wrapPanelPreRegistrationStage4: wrapPanelPreRegistrationStage4,
                                wrapPanelPreRegistrationMain: wrapPanelPreRegistrationMain,
                                eqContext: eqContext
                                );
                        };

                        btnBack.Click += (s, e) =>
                        {
                            wrapPanelButtons.Visibility = Visibility.Collapsed;
                            wrapPanelStage1Buttons.Visibility = Visibility.Collapsed;
                            wrapPanelStage1Menu.Visibility = Visibility.Visible;
                            btnBack.Visibility = Visibility.Hidden;
                            btnNextStage.Visibility = Visibility.Hidden;

                            foreach (Button button in wrapPanelButtons.Children)
                            {
                                button.Background = new SolidColorBrush(Color.FromRgb(255, 250, 255));
                            }
                        };
                    });

                    wrapPanelStage1Buttons.Children.Add(wrapPanelButtons);

                    btnMenu.Click += (s, e) =>
                    {
                        wrapPanelButtons.Visibility = Visibility.Visible;
                        wrapPanelStage1Buttons.Visibility = Visibility.Visible;
                        wrapPanelStage1Menu.Visibility = Visibility.Collapsed;
                        btnBack.Visibility = Visibility.Visible;
                    };


                    wrapPanelStage1Menu.Children.Add(btnMenu);
                }
                else
                if (b.ParentId == 0)
                {
                    SService sServices = eqContext.SServices.First(f => f.Id == b.SServiceId);
                    Button btnStage1 = new Button();
                    btnStage1.Name = "button";
                    btnStage1.Content = b.ButtonName;
                    btnStage1.HorizontalAlignment = HorizontalAlignment.Center;
                    btnStage1.VerticalAlignment = VerticalAlignment.Top;
                    btnStage1.Height = 75;
                    btnStage1.Width = 200;
                    btnStage1.Margin = new Thickness(0, 18, 32, 0);
                    btnStage1.Background = new SolidColorBrush(Color.FromRgb(255, 250, 255));
                    btnStage1.BorderBrush = new SolidColorBrush(Color.FromRgb(55, 55, 55));
                    btnStage1.FontFamily = new FontFamily("Area");
                    btnStage1.FontSize = 25;
                    btnStage1.Foreground = new SolidColorBrush(Color.FromRgb(135, 98, 27));
                    DropShadowEffect btnShadowEffectStage1 = new DropShadowEffect();
                    btnShadowEffectStage1.Color = Color.FromRgb(22, 22, 22);
                    btnShadowEffectStage1.Direction = 50;
                    btnShadowEffectStage1.ShadowDepth = 2;
                    btnStage1.Effect = btnShadowEffectStage1;
                    ControlTemplate myControlTemplateStage1 = new ControlTemplate(typeof(Button));
                    FrameworkElementFactory borderStage1 = new FrameworkElementFactory(typeof(Border));
                    borderStage1.Name = "border";
                    borderStage1.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(Border.BackgroundProperty));
                    borderStage1.SetValue(Border.BorderBrushProperty, new TemplateBindingExtension(Border.BorderBrushProperty));
                    borderStage1.SetValue(Border.BorderThicknessProperty, new TemplateBindingExtension(Border.BorderThicknessProperty));
                    borderStage1.SetValue(Border.CornerRadiusProperty, new CornerRadius(10));
                    FrameworkElementFactory contentPresenterStage1 = new FrameworkElementFactory(typeof(ContentPresenter));
                    contentPresenterStage1.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                    contentPresenterStage1.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
                    borderStage1.AppendChild(contentPresenterStage1);
                    myControlTemplateStage1.VisualTree = borderStage1;
                    btnStage1.Template = myControlTemplateStage1;
                    btnStage1.Click += (s, e) =>
                    {
                        ClickBtnPreReg(s, e,
                            button: b,
                            wrapPanelStage1Menu: wrapPanelStage1Menu,
                            wrapPanelButtons: wrapPanelStage1Menu,
                            btnNextStage: btnNextStage,
                            btnBack: btnBack,
                            wrapPanelPreRegistrationStage1: wrapPanelPreRegistrationStage1,
                            wrapPanelPreRegistrationStage2: wrapPanelPreRegistrationStage2,
                            wrapPanelPreRegistrationStage3: wrapPanelPreRegistrationStage3,
                            wrapPanelPreRegistrationStage4: wrapPanelPreRegistrationStage4,
                            wrapPanelPreRegistrationMain: wrapPanelPreRegistrationMain,
                            eqContext: eqContext
                            );
                    };
                    wrapPanelStage1Menu.Children.Add(btnStage1);
                }
            });


            wrapPanelPreRegistrationMain.Visibility = Visibility.Visible;

            foteWrapPanel.Children.Add(btnBack);
            foteWrapPanel.Children.Add(btnNextStage);

            wrapPanelPreRegistrationStage1.Children.Add(wrapPanelStage1Menu);
            wrapPanelPreRegistrationStage1.Children.Add(wrapPanelStage1Buttons);

            wrapPanelPreRegistrationMain.Children.Add(wrapPanelPreRegistrationStage1);
            wrapPanelPreRegistrationMain.Children.Add(wrapPanelPreRegistrationStage2);
            wrapPanelPreRegistrationMain.Children.Add(wrapPanelPreRegistrationStage3);
            wrapPanelPreRegistrationMain.Children.Add(wrapPanelPreRegistrationStage4);

            wrapPanelPreRegistrationMain.Children.Add(foteWrapPanel);

            // Создание и отображение нового окна
            Window newWindow = new Window();
            newWindow.Title = "Предварительная запись";
            newWindow.Width = 800;
            newWindow.Background = new SolidColorBrush(Colors.Black);
            newWindow.Height = 800;
            newWindow.Owner = this;

            double left = Left + (Width - newWindow.Width) / 2;
            double top = Top + (Height - newWindow.Height) / 2;
            newWindow.Left = left;
            newWindow.Top = top;

            newWindow.Content = wrapPanelPreRegistrationMain;

            newWindow.ShowDialog();
        }
        #endregion

        #region дабавление статуса
        public static void CallOperation(int statusId, long ticketId = 0, long idTransferred = 0)
        {
            try
            {
                //подключение к базе
                EqContext eqContext = new EqContext();

                var windows = eqContext.SOfficeWindows.Where(s => s.WindowIp == Ip);

                if (windows.Any())
                {
                    SOfficeWindow sOfficeWindow = windows.First();

                    if (ticketId == 0)
                    {
                        var ticketCall = GetNextTicket(sOfficeWindow.Id);
                        if (ticketCall.Id != 0)
                        {
                            var changeStatus = eqContext.DTicketStatuses.First(x => x.DTicketId == ticketCall.Id);
                            changeStatus.SStatusId = statusId;
                            changeStatus.SOfficeWindowId = sOfficeWindow.Id;

                            var changeTicket = eqContext.DTickets.First(x => x.Id == ticketCall.Id);
                            changeTicket.SStatusId = statusId;
                            changeTicket.SOfficeWindowId = sOfficeWindow.Id;

                        }
                    }
                    else
                    {
                        var changeStatus = eqContext.DTicketStatuses.First(x => x.DTicketId == ticketId);
                        changeStatus.SStatusId = statusId;
                        changeStatus.SOfficeWindowId = sOfficeWindow.Id;
                        if (idTransferred != 0) changeStatus.SOfficeWindowIdTransferred = idTransferred;
                        var changeTicket = eqContext.DTickets.First(x => x.Id == ticketId);
                        changeTicket.SStatusId = statusId;
                        changeTicket.SOfficeWindowId = sOfficeWindow.Id;
                    }
                    eqContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

            }

        }
        #endregion

        #region получение IP
        private void GetIp()
        {
            string IpOffise = "";
            IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress address in localIPs)
            {
                if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    IpOffise = address.ToString();
                }
            }
            Ip = IpOffise;
        }
        #endregion

        #region Генерация уникального кода
        static long GenerateUniqueCode(List<long> existingNumbers)
        {
            Random random = new Random();

            while (true)
            {
                long code = random.Next(1000, 10000);
                long[] codeDigits = code.ToString().ToCharArray().Select(c => long.Parse(c.ToString())).ToArray();

                bool isUnique = true;
                foreach (long digit in codeDigits)
                {
                    if (existingNumbers.Contains(digit))
                    {
                        isUnique = false;
                        break;
                    }
                }

                if (isUnique)
                {
                    return code;
                }
            }
        }
        #endregion

        #region пререгистрация
        private void ClickBtnPreReg(
            object sender,
            EventArgs e,
            WrapPanel wrapPanelButtons,
            WrapPanel wrapPanelStage1Menu,
            Button btnNextStage,
            Button btnBack,
            SOfficeTerminalButton button,
            WrapPanel wrapPanelPreRegistrationStage1,
            WrapPanel wrapPanelPreRegistrationStage2,
            WrapPanel wrapPanelPreRegistrationStage3,
            WrapPanel wrapPanelPreRegistrationStage4,
            WrapPanel wrapPanelPreRegistrationMain,
            EqContext eqContext
        )
        {
            Button btnStage1 = sender as Button;

            foreach (Button buttonMenu in wrapPanelButtons.Children)
            {
                if (buttonMenu.Name != "menu")
                {
                    buttonMenu.Background = new SolidColorBrush(Color.FromRgb(255, 250, 255));
                }
            };

            btnNextStage.Visibility = Visibility.Visible;
            btnStage1.Background = new SolidColorBrush(Color.FromRgb(100, 250, 255));

            btnNextStage.Click += (s, e) =>
            {
                btnBack.Visibility = Visibility.Collapsed;
                wrapPanelPreRegistrationStage2.Children.Clear();
                btnNextStage.Visibility = Visibility.Hidden;
                wrapPanelPreRegistrationStage1.Visibility = Visibility.Collapsed;
                wrapPanelPreRegistrationStage2.Visibility = Visibility.Visible;

                // Кнопки с датами записи 
                foreach (var ter in Prerecord.GetPrerecordData(button.SServiceId.Value, DateOnly.FromDateTime(DateTime.Now)).DistinctBy(x => x.Date).ToList())
                {
                    Button btnDate = new Button();
                    btnDate.Content = ter.Date.ToString("d") + "\n" + ter.DayName;
                    btnDate.HorizontalAlignment = HorizontalAlignment.Center;
                    btnDate.VerticalAlignment = VerticalAlignment.Center;
                    btnDate.Height = 75;
                    btnDate.Width = 200;
                    btnDate.Margin = new Thickness(32, 18, 0, 0);
                    btnDate.Background = new SolidColorBrush(Color.FromRgb(81, 96, 151));
                    btnDate.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 250, 255));
                    btnDate.FontFamily = new FontFamily("Area");
                    btnDate.FontSize = 20;
                    btnDate.Foreground = new SolidColorBrush(Color.FromRgb(252, 252, 240));

                    btnDate.Click += (s, e) =>
                    {
                        //горение выбраанной кнопки
                        foreach (Button button in wrapPanelPreRegistrationStage2.Children) button.Background = new SolidColorBrush(Color.FromRgb(81, 96, 151));
                        btnDate.Background = new SolidColorBrush(Color.FromRgb(100, 250, 255));
                        btnNextStage.Visibility = Visibility.Visible;

                        //переход на 3 этап
                        btnNextStage.Click += (s, e) =>
                        {
                            btnBack.Visibility = Visibility.Collapsed;
                            wrapPanelPreRegistrationStage2.Visibility = Visibility.Collapsed;
                            wrapPanelPreRegistrationStage3.Children.Clear();
                            wrapPanelPreRegistrationStage3.Visibility = Visibility.Visible;
                            // Кнопки с временем записи
                            foreach (var ter in Prerecord.GetPrerecordData(button.SServiceId.Value, DateOnly.FromDateTime(DateTime.Now)).DistinctBy(x => x.StopTimePrerecord).ToList())
                            {
                                Button btnTime = new Button();
                                btnTime.Content = ter.StartTimePrerecord.ToString("hh\\:mm") + " - " + ter.StopTimePrerecord.ToString("hh\\:mm");
                                btnTime.HorizontalAlignment = HorizontalAlignment.Center;
                                btnTime.VerticalAlignment = VerticalAlignment.Center;
                                btnTime.Height = 75;
                                btnTime.Width = 200;
                                btnTime.Margin = new Thickness(32, 18, 0, 0);
                                btnTime.Background = new SolidColorBrush(Color.FromRgb(81, 96, 151));
                                btnTime.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 250, 255));
                                btnTime.FontFamily = new FontFamily("Area");
                                btnTime.FontSize = 20;
                                btnTime.Foreground = new SolidColorBrush(Color.FromRgb(252, 252, 240));
                                //переход на 4 этап
                                btnTime.Click += (s, e) =>
                                {
                                    foreach (Button button in wrapPanelPreRegistrationStage3.Children)
                                    {
                                        button.Background = new SolidColorBrush(Color.FromRgb(81, 96, 151));
                                    };
                                    btnTime.Background = new SolidColorBrush(Color.FromRgb(100, 250, 255));
                                    btnNextStage.Visibility = Visibility.Visible;

                                    //переход на 4 этап
                                    btnNextStage.Click += (s, e) =>
                                    {
                                        btnBack.Visibility = Visibility.Collapsed;

                                        wrapPanelPreRegistrationStage4.Orientation = Orientation.Vertical;
                                        wrapPanelPreRegistrationStage3.Visibility = Visibility.Collapsed;

                                        //поля фио и телефон
                                        StackPanel stackPanelForm = new StackPanel();
                                        stackPanelForm.HorizontalAlignment = HorizontalAlignment.Center;

                                        TextBox textBoxFio = new TextBox();
                                        textBoxFio.FontSize = 25;
                                        textBoxFio.FontFamily = new FontFamily("Area");
                                        textBoxFio.Padding = new Thickness(5, 8, 5, 8);
                                        textBoxFio.Height = 45;
                                        textBoxFio.Width = 600;
                                        textBoxFio.Foreground = new SolidColorBrush(Colors.Black);
                                        textBoxFio.Focusable = true;

                                        Label labelFio = new Label();
                                        labelFio.FontFamily = new FontFamily("Area");
                                        labelFio.FontSize = 20;
                                        labelFio.Foreground = new SolidColorBrush(Colors.White);
                                        labelFio.Content = "ФИО: ";


                                        TextBox textBoxPhone = new TextBox();
                                        textBoxPhone.FontFamily = new FontFamily("Area");
                                        textBoxPhone.Padding = new Thickness(5, 8, 5, 8);
                                        textBoxPhone.Foreground = new SolidColorBrush(Colors.Black);
                                        textBoxPhone.FontSize = 25;
                                        textBoxPhone.Width = 600;
                                        textBoxPhone.Height = 45;
                                        textBoxPhone.Text = "+7(";
                                        bool isUpdating = false;

                                        textBoxPhone.TextChanged += (s, e) =>
                                        {
                                            if (isUpdating)
                                                return;

                                            string phoneNumber = textBoxPhone.Text;

                                            // Удаление всех нецифровых символов из введенного номера телефона
                                            string digitsOnly = new string(phoneNumber.Where(char.IsDigit).ToArray());

                                            // Применение маски (например, формат "+X (XXX) XXX-XXXX")
                                            StringBuilder maskedNumber = new StringBuilder();

                                            int index = 0;
                                            foreach (char c in "+X (XXX) XXX-XX-XX")
                                            {
                                                if (c == 'X') // Заменяем "X" на символ из введенного номера телефона или пустое значение для неполного номера
                                                {
                                                    if (index < digitsOnly.Length)
                                                        maskedNumber.Append(digitsOnly[index]);
                                                    else
                                                        break; // Номер телефона имеет меньше цифр, чем маска
                                                    index++;
                                                }
                                                else
                                                {
                                                    maskedNumber.Append(c);
                                                }
                                            }

                                            isUpdating = true;
                                            textBoxPhone.Text = maskedNumber.ToString();
                                            textBoxPhone.CaretIndex = maskedNumber.Length; // Перемещаем курсор в конец строки
                                            isUpdating = false;
                                        };

                                        Label labelPhone = new Label();
                                        labelPhone.FontFamily = new FontFamily("Area");
                                        labelPhone.Margin = new Thickness(0, 15, 0, 0);
                                        labelPhone.FontSize = 20;
                                        labelPhone.Foreground = new SolidColorBrush(Colors.White);
                                        labelPhone.Content = "Телефон: ";

                                        stackPanelForm.Children.Add(labelFio);
                                        stackPanelForm.Children.Add(textBoxFio);

                                        stackPanelForm.Children.Add(labelPhone);
                                        stackPanelForm.Children.Add(textBoxPhone);

                                        wrapPanelPreRegistrationStage4.Children.Add(stackPanelForm);
                                        btnNextStage.Content = "Записать";


                                        //финальная кнопка
                                        Button btnPreRegistrationFinal = new Button();
                                        DropShadowEffect shadowPreRegistrationFinal = new DropShadowEffect();
                                        shadowPreRegistrationFinal.Color = Colors.White;
                                        shadowPreRegistrationFinal.ShadowDepth = 3;
                                        btnPreRegistrationFinal.Effect = shadowPreRegistrationFinal;
                                        btnPreRegistrationFinal.Name = "btnPreRegistrationFinal";
                                        btnPreRegistrationFinal.Content = "Записаться";
                                        btnPreRegistrationFinal.HorizontalAlignment = HorizontalAlignment.Center;
                                        btnPreRegistrationFinal.VerticalAlignment = VerticalAlignment.Bottom;
                                        btnPreRegistrationFinal.Height = 50;
                                        btnPreRegistrationFinal.Width = 150;
                                        btnPreRegistrationFinal.Margin = new Thickness(0, 10, 0, 0);
                                        btnPreRegistrationFinal.Background = new SolidColorBrush(Colors.DarkGreen);
                                        btnPreRegistrationFinal.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 20));
                                        btnPreRegistrationFinal.FontFamily = new FontFamily("Area");
                                        btnPreRegistrationFinal.FontSize = 20;
                                        btnPreRegistrationFinal.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                                        btnPreRegistrationFinal.TabIndex = 999;

                                        btnPreRegistrationFinal.Click += (s, e) =>
                                        {
                                            if (textBoxFio.Text.Length == 0)
                                            {
                                                textBoxFio.BorderBrush = new SolidColorBrush(Colors.Red);
                                                labelFio.Foreground = new SolidColorBrush(Colors.Red);
                                                labelFio.Content = "ФИО: не заполнено !";
                                            }
                                            else
                                            {

                                                var codePrerecord = GenerateUniqueCode(eqContext.DTicketPrerecords.Where(w => w.DatePrerecord == DateOnly.Parse(ter.Date.ToString("d"))).Select(s => s.CodePrerecord).ToList());
                                                //записиваю в базу
                                                DTicketPrerecord dTicketPrerecord = new DTicketPrerecord();
                                                dTicketPrerecord.SServiceId = button.SServiceId.Value;
                                                dTicketPrerecord.SOfficeId = eqContext.SOfficeWindows.First(d => d.WindowIp == Ip).SOfficeId;
                                                dTicketPrerecord.SSourсePrerecordId = 2;
                                                dTicketPrerecord.CustomerFullName = textBoxFio.Text;
                                                dTicketPrerecord.CustomerPhoneNumber = textBoxPhone.Text;
                                                dTicketPrerecord.DatePrerecord = DateOnly.Parse(ter.Date.ToString("d"));
                                                dTicketPrerecord.StartTimePrerecord = TimeOnly.Parse(ter.StartTimePrerecord.ToString("hh\\:mm"));
                                                dTicketPrerecord.StopTimePrerecord = TimeOnly.Parse(ter.StopTimePrerecord.ToString("hh\\:mm"));
                                                dTicketPrerecord.IsConfirmation = false;
                                                dTicketPrerecord.CodePrerecord = codePrerecord;
                                                eqContext.DTicketPrerecords.Add(dTicketPrerecord);
                                                eqContext.SaveChanges();

                                                wrapPanelPreRegistrationStage4.Visibility = Visibility.Collapsed;

                                                //показываю код
                                                WrapPanel wrapPanelResultPreRegistration = new WrapPanel();
                                                TextBlock ResultPreRegistrationCode = new TextBlock();
                                                ResultPreRegistrationCode.Text = "Ваш код: " + codePrerecord.ToString();
                                                ResultPreRegistrationCode.HorizontalAlignment = HorizontalAlignment.Center;
                                                ResultPreRegistrationCode.Foreground = new SolidColorBrush(Colors.White);
                                                ResultPreRegistrationCode.FontSize = 100;
                                                ResultPreRegistrationCode.TextWrapping = TextWrapping.Wrap;

                                                TextBlock ResultPreRegistrationText = new TextBlock();
                                                ResultPreRegistrationText.Text = "Вы должны явиться в " + ter.DayName + " " + dTicketPrerecord.DatePrerecord + "\nс " + dTicketPrerecord.StartTimePrerecord + " по " + dTicketPrerecord.StartTimePrerecord;
                                                ResultPreRegistrationText.HorizontalAlignment = HorizontalAlignment.Center;
                                                ResultPreRegistrationText.FontSize = 40;
                                                ResultPreRegistrationText.Margin = new Thickness(0, 15, 0, 0);
                                                ResultPreRegistrationText.Foreground = new SolidColorBrush(Colors.Green);
                                                ResultPreRegistrationText.TextWrapping = TextWrapping.Wrap;
                                                wrapPanelResultPreRegistration.Children.Add(ResultPreRegistrationCode);
                                                wrapPanelResultPreRegistration.Children.Add(ResultPreRegistrationText);

                                                wrapPanelPreRegistrationMain.Children.Add(wrapPanelResultPreRegistration);
                                            }
                                        };

                                        wrapPanelPreRegistrationStage4.Children.Add(btnPreRegistrationFinal);
                                    };
                                };
                                wrapPanelPreRegistrationStage3.Children.Add(btnTime);
                            }
                        };
                    };

                    wrapPanelPreRegistrationStage2.Children.Add(btnDate);
                }

            };
        }
        #endregion 
    }
}