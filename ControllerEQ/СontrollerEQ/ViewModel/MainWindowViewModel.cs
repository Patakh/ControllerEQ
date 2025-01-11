using ControllerEQ.Command;
using ControllerEQ.Model;
using ControllerEQ.Model.Data;
using ControllerEQ.Views;
using ControllerEQ.Views.Modal;
using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ControllerEQ.ViewModel;

public class MainWindowViewModel : INotifyPropertyChanged
{
    private MainWindowModel _model;

    public event EventHandler StartServicingEvent;
    public event EventHandler BreakServicingEvent;
    public event EventHandler StopServicingEvent;
    public event EventHandler TransversServicingEvent;
    public event EventHandler DeferServicingEvent;

    public string TimeServicing
    {
        get => _model.TimeServicing;
        set
        {
            _model.TimeServicing = value;
            NotifyPropertyChanged(nameof(TimeServicing));
        }
    }
    public MainWindowViewModel()
    {
        _model = DataWorker.GetMainWindowData();
        _model.NewClients = new ClientListWindowModel(Status.New);
        _model.DeferClients = new ClientListWindowModel(Status.Defer);
        _model.TransversClients = new ClientListWindowModel(Status.Transferred);

        StartListeningAsync().GetAwaiter();
    }
    public RelayCommand Call
    {
        get
        {
            return _model.Call ?? new RelayCommand(async odj =>
            {
                ClientModel clientModel = new ClientModel { TicketName = _model.Client, Id = _model.ClientId };
                CallClient(clientModel);
            },
            _ => _model.WindowId != 0 && _model.Client != "---"
            );
        }
    }
    public Visibility ButtonMainVisibility
    {
        get => _model.ButtonMainVisibility;
        set
        {
            _model.ButtonMainVisibility = value;
            NotifyPropertyChanged("ButtonMainVisibility");
        }
    }
    public Visibility ButtonServingVisibility
    {
        get => _model.ButtonServingVisibility;
        set
        {
            _model.ButtonServingVisibility = value;
            NotifyPropertyChanged("ButtonServingVisibility");
        }
    }
    public Visibility ButtonServingFinishVisibility
    {
        get => _model.ButtonServingFinishVisibility;
        set
        {
            _model.ButtonServingFinishVisibility = value;
            NotifyPropertyChanged("ButtonServingFinishVisibility");
        }
    }
    public Visibility BodyVisibility
    {
        get => _model.BodyVisibility;
        set
        {
            _model.BodyVisibility = value;
            NotifyPropertyChanged("BodyVisibility");
        }
    }
    public MainWindowModel MainWindowData
    {
        get => _model;
        set
        {
            _model = value;
            NotifyPropertyChanged("MainWindowData");
        }
    }
    public event PropertyChangedEventHandler PropertyChanged;
    public RelayCommand ShowListClientsCommand
    {
        get
        {
            return _model.NewClients.ShowWindowCommand ?? new RelayCommand(odj =>
            {
                ShowListClients(Status.New, ref _model.NewClients.Window, ref _model.NewClients.IsVisible);
            }, _ => _model.WindowId != 0
            );
        }
    }
    public RelayCommand ShowListDeferClientsCommand
    {
        get
        {
            return _model.DeferClients.ShowWindowCommand ?? new RelayCommand(odj =>
            {
                ShowListClients(Status.Defer, ref _model.DeferClients.Window, ref _model.DeferClients.IsVisible);
            }, _ => _model.WindowId != 0
            );
        }
    }
    public RelayCommand ShowListTransversClientsCommand
    {
        get
        {
            return _model.TransversClients.ShowWindowCommand ?? new RelayCommand(odj =>
            {
                ShowListClients(Status.Transferred, ref _model.TransversClients.Window, ref _model.TransversClients.IsVisible);
            }, _ => _model.WindowId != 0
            );
        }
    }
    public RelayCommand ShowMainWindowCommand
    {
        get
        {
            return _model.ShowMainWindowCommand ?? new RelayCommand(odj =>
            {
                ShowMainWindow();
            }, _ => true
            );
        }
    }
    public RelayCommand StartServicingCommand
    {
        get
        {
            return _model.StartServicingCommand ?? new RelayCommand(odj =>
            {
                StartServicingEvent.Invoke(odj, EventArgs.Empty);
            }, _ => _model.WindowId != 0
            );
        }
    }
    public RelayCommand BreakServisingCommand
    {
        get
        {
            return _model.BreakServisingCommand ?? new RelayCommand(odj =>
            {
                BreakServicingEvent.Invoke(odj, EventArgs.Empty);
            }, _ => _model.WindowId != 0
            );
        }
    }
    public RelayCommand StopServicingCommand
    {
        get
        {
            return _model.StopServisingCommand ?? new RelayCommand(odj =>
            {
                StopServicingEvent.Invoke(odj, EventArgs.Empty);
            }, _ => _model.WindowId != 0
            );
        }
    }
    public RelayCommand TransversServicingCommand
    {
        get
        {
            return _model.TransverServisingCommand ?? new RelayCommand(odj =>
            {
                TransversServicingEvent.Invoke(odj, EventArgs.Empty);
            }, _ => _model.WindowId != 0
            );
        }
    }
    public RelayCommand DeferServicingCommand
    {
        get
        {
            return _model.DeferServisingCommand ?? new RelayCommand(odj =>
            {
                DeferServicingEvent.Invoke(odj, EventArgs.Empty);
            }, _ => _model.WindowId != 0
            );
        }
    }
    public RelayCommand PreRegistrationCommand
    {
        get
        {
            return _model.PreRegistrationCommand ?? new RelayCommand(odj =>
            {
                HideListClients(_model.NewClients.Window, ref _model.NewClients.IsVisible);
                HideListClients(_model.DeferClients.Window, ref _model.DeferClients.IsVisible);
                HideListClients(_model.TransversClients.Window, ref _model.TransversClients.IsVisible);
                new PreRegistrationWindow().ShowDialog();
            }, _ => _model.WindowId != 0
            );
        }
    }
    private void ShowMainWindow()
    {
        if (_model.IsShow)
        {
            BodyVisibility = Visibility.Hidden;
            _model.IsShow = false;

            HideListClients(_model.NewClients.Window, ref _model.NewClients.IsVisible);
            HideListClients(_model.DeferClients.Window, ref _model.DeferClients.IsVisible);
            HideListClients(_model.TransversClients.Window, ref _model.TransversClients.IsVisible);
        }
        else
        {
            BodyVisibility = Visibility.Visible;
            _model.IsShow = true;
        }
    }
    private void HideListClients(Window window, ref bool isShow)
    {
        window.Hide();
        isShow = false;
    }
    private void ShowListClients(Status clientType, ref ClientListWindow window, ref bool isShow)
    {
        bool _isShow = isShow;

        HideListClients(_model.NewClients.Window, ref _model.NewClients.IsVisible);
        HideListClients(_model.DeferClients.Window, ref _model.DeferClients.IsVisible);
        HideListClients(_model.TransversClients.Window, ref _model.TransversClients.IsVisible);

        if (_isShow)
        {
            window.Hide();
            isShow = false;
        }
        else
        {
            window.Show();
            isShow = true;
        }
    }
    private void NotifyPropertyChanged(string propertyName)
    {
        if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }
    public async void CallClient(ClientModel clientModel)
    {
        HideListClients(_model.NewClients.Window, ref _model.NewClients.IsVisible);
        HideListClients(_model.DeferClients.Window, ref _model.DeferClients.IsVisible);
        HideListClients(_model.TransversClients.Window, ref _model.TransversClients.IsVisible);

        ButtonServingVisibility = Visibility.Visible;
        ButtonMainVisibility = Visibility.Collapsed;
        ButtonServingFinishVisibility = Visibility.Collapsed;
        Client = clientModel.TicketName;

        await clientModel.Call();

        BreakServicingEvent = null;
        StartServicingEvent = null;
        DeferServicingEvent = null;
        TransversServicingEvent = null;
        StopServicingEvent = null;

        //не явился
        BreakServicingEvent += async (s, e) =>
        {
            await clientModel.BreakServicing();
            ButtonMainShow();
            Refresh();
        };

        //начать обслуживание
        StartServicingEvent += async (s, e) =>
        {
            ButtonServingVisibility = Visibility.Collapsed;
            ButtonServingFinishVisibility = Visibility.Visible;
            ButtonMainVisibility = Visibility.Collapsed;
            await clientModel.StartServicing();
            TimerStart();


            //отложить
            DeferServicingEvent += async (s, e) =>
            {
                ConfirmationDialogWindow confirmationDialog = new();
                if (confirmationDialog.ShowDialog() == true)
                {
                    await clientModel.DeferServicing();
                    ButtonMainShow();
                    Refresh();
                    DeferServicingEvent = null;
                }
                confirmationDialog.Close();
            };

            //передать
            TransversServicingEvent += async (s, e) =>
            {
                TransverClientWindow transverClientWindow = new(clientModel);
                if (transverClientWindow.ShowDialog() == true)
                {
                    ButtonMainShow();
                    Refresh();
                    TransversServicingEvent = null;
                }
            };

            //завершить
            StopServicingEvent += async (s, e) =>
            {
                await clientModel.StopServicing();
                ButtonMainShow();
                Refresh();
                StopServicingEvent = null;
            };
        };
    }
    public async Task StartListeningAsync()
    {
        try
        {
            TcpListener listener = new TcpListener(IPAddress.Parse(DataWorker.Window.WindowIp), 1234);
            listener.Start();
            Console.WriteLine("Сервер запущен. Ожидание подключений...");

            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                _ = HandleClientAsync(client);
            }
        }
        catch { }
    }
    private async Task HandleClientAsync(TcpClient client)
    {
        byte[] buffer = new byte[1024];
        StringBuilder messageBuilder = new StringBuilder();

        using NetworkStream stream = client.GetStream();

        int bytesRead;

        while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
        {
            string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            messageBuilder.Append(receivedMessage);
        }

        string message = messageBuilder.ToString();
        if (message == "new Ticket")
        {
            Refresh();
        }
    }
    private void ButtonMainShow()
    {
        ButtonServingVisibility = Visibility.Collapsed;
        ButtonServingFinishVisibility = Visibility.Collapsed;
        ButtonMainVisibility = Visibility.Visible;
        TimerStop();
    }
    private void Refresh()
    {
        ClientListViewModel clientNewList = (ClientListViewModel)ClientNewListWindow.DataContext;
        ClientListViewModel clientDeferList = (ClientListViewModel)ClientDeferListWindow.DataContext;
        ClientListViewModel clientTransverList = (ClientListViewModel)ClientTransverListWindow.DataContext;

        clientNewList.Clients = DataWorker.GetClientNewListData();
        clientDeferList.Clients = DataWorker.GetClientDeferListData();
        clientTransverList.Clients = DataWorker.GetClientTransferListData();

        MainWindowModel modelMain = DataWorker.GetMainWindowData();

        CountClients = modelMain.CountClients;
        QueueClientsCount = modelMain.QueueClientsCount;
        TransferClientsCount = modelMain.TransferClientsCount;
        DeferClientsCount = modelMain.DeferClientsCount;
        ClientId = modelMain.ClientId;

        if (ButtonServingFinishVisibility != Visibility.Visible && ButtonServingVisibility != Visibility.Visible)
        {
            Client = modelMain.Client;
        }
    }
    private void TimerStart()
    {
        _model.TimerServicing = new DateTime();
        _model.Timer = new DispatcherTimer();
        _model.Timer.Interval = TimeSpan.FromSeconds(1);
        _model.Timer.Tick += (s, e) =>
        {
            UpdateDateTime();
        };

        _model.Timer.Start();

        UpdateDateTime();
    }
    private void TimerStop()
    {
        _model.Timer!.Stop();
        TimeServicing = "00:00";
    }
    private void UpdateDateTime()
    {
        _model.TimerServicing = _model.TimerServicing.AddSeconds(1);
        TimeServicing = _model.TimerServicing.ToString("mm:ss");
    }
    public ClientListWindow ClientNewListWindow
    {
        get => _model.NewClients.Window;
        set
        {
            _model.NewClients.Window = value;
        }
    }
    public ClientListWindow ClientDeferListWindow
    {
        get => _model.DeferClients.Window;
        set
        {
            _model.DeferClients.Window = value;
        }
    }
    public ClientListWindow ClientTransverListWindow
    {
        get => _model.TransversClients.Window;
        set
        {
            _model.TransversClients.Window = value;
        }
    }
    public string? IpAddress
    {
        get => _model.IpAddress;
        set
        {
            _model.IpAddress = value;
            NotifyPropertyChanged(nameof(IpAddress));
        }
    }
    public long? WindowId
    {
        get
        {
            return _model.WindowId;
        }
        set
        {
            _model.WindowId = value;
            NotifyPropertyChanged(nameof(WindowId));
        }
    }
    public string? WindowName
    {
        get => _model.WindowsName;
        set
        {
            _model.WindowsName = value;
            NotifyPropertyChanged(nameof(WindowName));
        }
    }
    public string? Client
    {
        get => _model.Client;
        set
        {
            _model.Client = value;
            NotifyPropertyChanged(nameof(Client));
        }
    }
    public long ClientId
    {
        get => _model.ClientId;
        set
        {
            _model.ClientId = value;
            NotifyPropertyChanged(nameof(ClientId));
        }
    }
    public string? ErrorMessage
    {
        get => _model.ErrorMessage;
        set
        {
            _model.ErrorMessage = value;
            NotifyPropertyChanged(nameof(ErrorMessage));
        }
    }
    public string? CountClients
    {
        get => _model.CountClients;
        set
        {
            _model.CountClients = value;
            NotifyPropertyChanged(nameof(CountClients));
        }
    }
    public string? QueueClientsCount
    {
        get => _model.QueueClientsCount;
        set
        {
            _model.QueueClientsCount = value;
            NotifyPropertyChanged(nameof(QueueClientsCount));
        }
    }
    public string? TransferClientsCount
    {
        get => _model.TransferClientsCount;
        set
        {
            _model.TransferClientsCount = value;
            NotifyPropertyChanged(nameof(TransferClientsCount));
        }
    }
    public string? DeferClientsCount
    {
        get => _model.DeferClientsCount;
        set
        {
            _model.DeferClientsCount = value;
            NotifyPropertyChanged(nameof(DeferClientsCount));
        }
    }
}