using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using TVQE.Model;
using TVQE.Model.Data;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Linq;
using System.IO;
using System.Windows.Media.Media3D;

namespace TVQE.ViewModel;
class MainWindowViewModel : INotifyPropertyChanged
{
    private MainWindowModel _model;
    public MainWindowViewModel(StackPanel stackBanner, Canvas canvas, TextBlock textBlock, AxisAngleRotation3D rotationClientList, AxisAngleRotation3D rotationClient)
    {
        _model = new MainWindowModel();
        _model.Canvas = canvas;
        _model.RunningText = textBlock;
        _model.MediaElementsBanner = stackBanner;
        _model.RunningText.Text = DataWorker.GetRunningText();

        _model.Audio.MediaEnded += MediaEndedAudio;
        _model.Audio.MediaFailed += Audio_MediaFailed;
        _model.Banner.MediaEnded += MediaEndedBanner;
        _model.Banner.MediaFailed += Banner_MediaFailed;

        _model.RotationClientList = rotationClientList;
        _model.RotationClient = rotationClient;
        
        PlayBanner();

        DateTimeView();
        StartRunningTextAnimation();
        StartListeningAsync().GetAwaiter();
    }
    public string Window
    {
        get => _model.Window;
        set
        {
            _model.Window = value;
            NotifyPropertyChanged("Window");
        }
    }
    public string Ticket
    {
        get => _model.Ticket;
        set
        {
            _model.Ticket = value;
            NotifyPropertyChanged("Ticket");
        }
    }
    public Visibility TicketCallVisibility
    {
        get => _model.TicketCallVisibility;
        set
        {
            _model.TicketCallVisibility = value;
            NotifyPropertyChanged("TicketCallVisibility");
        }
    }
    public Visibility BannerVisibility
    {
        get => _model.BannerVisibility;
        set
        {
            _model.BannerVisibility = value;
            NotifyPropertyChanged("BannerVisibility");
        }
    }
    public string Title
    {
        get => _model.Title;
        set
        {
            _model.Title = value;
            NotifyPropertyChanged("Title");
        }
    }
    public string Date
    {
        get => _model.Date;
        set
        {
            _model.Date = value;
            NotifyPropertyChanged("Date");
        }
    }
    public string Time
    {
        get => _model.Time;
        set
        {
            _model.Time = value;
            NotifyPropertyChanged("Time");
        }
    }
    public string DayName
    {
        get => _model.DayName;
        set
        {
            _model.DayName = value;
            NotifyPropertyChanged("DayName");
        }
    }
    public ObservableCollection<Ticket> Tickets
    {
        get => _model.Tickets;
        set
        {
            _model.Tickets = value;
            NotifyPropertyChanged("Tickets");
        }
    }
    public event PropertyChangedEventHandler PropertyChanged;
    private void NotifyPropertyChanged(string property)
    {
        if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property));
    }
    private void DateTimeView()
    {
        _model.Timer = new DispatcherTimer();
        _model.Timer.Interval = TimeSpan.FromSeconds(1);
        _model.Timer.Tick += (s, e) =>
        {
            UpdateDateTime();
        };
        _model.Timer.Start();
        UpdateDateTime();
    }
    private void UpdateDateTime()
    {
        DateTime now = DateTime.Now;
        Date = now.ToString("D");
        Time = now.ToString("HH:mm:ss");
        Time = now.ToString("HH:mm:ss");
        DayName = now.ToString("dddd");
    }
    public async Task StartListeningAsync()
    {
        TcpListener listener = new TcpListener(IPAddress.Parse(DataWorker.OfficeScoreboards.ScoreboardIp), 1235);
        listener.Start();
        Console.WriteLine("Сервер запущен. Ожидание подключений...");

        while (true)
        {
            TcpClient client = await listener.AcceptTcpClientAsync();
            _ = HandleClientAsync(client);
        }
    }
    private void StartRunningTextAnimation()
    {
        double canvasWidth = _model.Canvas.ActualWidth;
        double textWidth = _model.RunningText.Text.Length * 6.5;

        DoubleAnimation animation = new();
        animation.From = 0;
        animation.To = canvasWidth - textWidth;
        animation.EasingFunction = new QuadraticEase();
        animation.RepeatBehavior = RepeatBehavior.Forever;
        animation.Duration = new Duration(TimeSpan.FromSeconds(10));

        _model.RunningText.BeginAnimation(Canvas.LeftProperty, animation);
    }
    private async Task HandleClientAsync(TcpClient client)
    {
        using (client)
        {
            byte[] buffer = new byte[1024];
            StringBuilder messageBuilder = new();

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
            if (message == "Pocess")
            {
                Tickets = DataWorker.GetTickets();
            }
            else
            {
                string[] ident = message.Split(':');
                string type = ident[0];
                string value = ident[1];

                switch (type)
                {
                    case "Call":
                        try
                        {
                            var ticketCall = DataWorker.GetTicket(Convert.ToInt64(value));
                            var windowName = DataWorker.GetWindowName(ticketCall.SOfficeWindowId);
                            string prefix = ticketCall.ServicePrefix;
                            string number = ticketCall.TicketNumber.ToString();

                            if (_model.CallTickets.Count == 0) RotateClient();
                            _model.CallTickets.Add(new CallTickets(prefix, number, windowName));

                            if (_model.CallTickets.Count == 1)
                            {
                                CallTicket();
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                        break;
                }
            }
        }
    }
    private void CallTicket()
    {
        try
        {
            _model.CurrentIndexAudio = 0;
            _model.AudioFiles = _model.CallTickets[0].AudioFiles;
            Ticket = _model.CallTickets[0].Prefix + _model.CallTickets[0].Number;
            Window = _model.CallTickets[0].WindowName;
            PlayAudio();
        }
        catch (Exception ex)
        {

        }
    }
    private void PlayAudio()
    {
        _model.Audio.Source = new Uri($"{Path.GetFullPath(Environment.CurrentDirectory)}\\Voice\\{_model.AudioFiles[_model.CurrentIndexAudio]}.mp3");
        _model.Audio.LoadedBehavior = MediaState.Manual;
        _model.Audio.UnloadedBehavior = MediaState.Manual;
        _model.Audio.Play();
    }
    private void MediaEndedAudio(object sender, RoutedEventArgs e)
    {
        if (_model.AudioFiles.Count == _model.CurrentIndexAudio + 1)
        {
            _model.CallTickets.Remove(_model.CallTickets[0]);
            if (_model.CallTickets.Count == 0)
            {
                Tickets = DataWorker.GetTickets();
                RotateClientList();
            }
            else
                CallTicket();
        }
        else
        {
            _model.CurrentIndexAudio++;
            PlayAudio();
        }
    }

    private void Audio_MediaFailed(object? sender, ExceptionRoutedEventArgs e)
    {
        if (_model.BannerFiles.Count > _model.CurrentIndexAudio++)
            PlayAudio();
        else
            MediaEndedAudio(sender, e);
    }

    private void PlayBanner()
    {
        try
        {
            if (_model.BannerFiles.Count > 0)
            {
                _model.MediaElementsBanner.Children.Clear();
                _model.Banner.Volume = 0;
                _model.MediaElementsBanner.Children.Add(_model.Banner);
                _model.Banner.VerticalAlignment = VerticalAlignment.Top;
                _model.Banner.Source = new Uri(_model.BannerFiles[_model.CurrentIndexBanner]);
                _model.Banner.LoadedBehavior = MediaState.Manual;
                _model.Banner.UnloadedBehavior = MediaState.Play;
                _model.Banner.Play();
            }
        }
        catch (Exception ex)
        {
            _model.CurrentIndexBanner++;
            PlayBanner();
        }
    }

    private void Banner_MediaFailed(object? sender, ExceptionRoutedEventArgs e)
    {
        _model.BannerFiles.Remove(_model.BannerFiles[_model.CurrentIndexBanner]);
        _model.CurrentIndexBanner--;
        MediaEndedBanner(sender, e);
    }
    private void MediaEndedBanner(object sender, RoutedEventArgs e)
    {
        _model.CurrentIndexBanner++;
        if (_model.CurrentIndexBanner == _model.BannerFiles.Count()) _model.CurrentIndexBanner = 0;
        PlayBanner();
    }

    private void RotateClient()
    {
        DoubleAnimation animationClient = new();
        animationClient.From = 90;
        animationClient.To = 0;
        animationClient.Duration = TimeSpan.FromSeconds(1);

        DoubleAnimation animationClientList = new();
        animationClientList.From = 0;
        animationClientList.To = -90;
        animationClientList.Duration = animationClient.Duration;

        AxisAngleRotation3D rotationClient = _model.RotationClient;
        rotationClient.BeginAnimation(AxisAngleRotation3D.AngleProperty, animationClient);

        AxisAngleRotation3D rotationClientList = _model.RotationClientList;
        rotationClientList.BeginAnimation(AxisAngleRotation3D.AngleProperty, animationClientList);
    }

    private void RotateClientList()
    {
        DoubleAnimation animationClient = new();
        animationClient.From = 0;
        animationClient.To = 90;
        animationClient.Duration = TimeSpan.FromSeconds(1);

        DoubleAnimation animationClientList = new();
        animationClientList.From = -90;
        animationClientList.To = 0;
        animationClientList.Duration = animationClient.Duration;

        AxisAngleRotation3D rotationClient = _model.RotationClient;
        rotationClient.BeginAnimation(AxisAngleRotation3D.AngleProperty, animationClient);

        AxisAngleRotation3D rotationClientList = _model.RotationClientList;
        rotationClientList.BeginAnimation(AxisAngleRotation3D.AngleProperty, animationClientList);
    }
}
