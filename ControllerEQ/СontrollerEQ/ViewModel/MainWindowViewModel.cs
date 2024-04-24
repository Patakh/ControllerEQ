using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using СontrollerEQ.Model;
using СontrollerEQ.Views;
using СontrollerEQ.Model.Data;
using СontrollerEQ.Views.Modal;
using System.Windows.Threading;
using СontrollerEQ.Command;

namespace СontrollerEQ.ViewModel;

public class MainWindowViewModel : INotifyPropertyChanged
{
    private MainWindowModel _model;

    public event EventHandler StartServisingEvent;
    public event EventHandler BreakServisingEvent;
    public event EventHandler StopServisingEvent;
    public event EventHandler TransverServisingEvent;
    public event EventHandler DeferServisingEvent;

    public string TimeServising
    {
        get => _model.TimeServising;
        set
        {
            _model.TimeServising = value;
            NotifyPropertyChanged("TimeServising");
        }
    }
    public MainWindowViewModel()
    {
        _model = DataWorker.GetMainWindowData();
        _model.NewClients = new ClientListWindowModel(Status.New);
        _model.DeferClients = new ClientListWindowModel(Status.Defer);
        _model.TransverClients = new ClientListWindowModel(Status.Transferred);

        StartListeningAsync().GetAwaiter();
    }
    public RelayCommand Call
    {
        get
        {
            return _model.Call ?? new RelayCommand(async odj =>
            {
                ClientModel clientModel = new ClientModel { TicketName = _model.Cliente, Id = _model.ClienteId };
                CallClient(clientModel);
            },
            _ => _model.WindowId != 0 && _model.Cliente != "---"
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
    public RelayCommand ShowListTransverClientsCommand
    {
        get
        {
            return _model.TransverClients.ShowWindowCommand ?? new RelayCommand(odj =>
            {
                ShowListClients(Status.Transferred, ref _model.TransverClients.Window, ref _model.TransverClients.IsVisible);
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
                StartServisingEvent.Invoke(odj, EventArgs.Empty);
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
                BreakServisingEvent.Invoke(odj, EventArgs.Empty);
            }, _ => _model.WindowId != 0
            );
        }
    }
    public RelayCommand StopServisingCommand
    {
        get
        {
            return _model.StopServisingCommand ?? new RelayCommand(odj =>
            {
                StopServisingEvent.Invoke(odj, EventArgs.Empty);
            }, _ => _model.WindowId != 0
            );
        }
    }
    public RelayCommand TransverServisingCommand
    {
        get
        {
            return _model.TransverServisingCommand ?? new RelayCommand(odj =>
            {
                TransverServisingEvent.Invoke(odj, EventArgs.Empty);
            }, _ => _model.WindowId != 0
            );
        }
    }
    public RelayCommand DeferServisingCommand
    {
        get
        {
            return _model.DeferServisingCommand ?? new RelayCommand(odj =>
            {
                DeferServisingEvent.Invoke(odj, EventArgs.Empty);
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
                HideListClients(_model.TransverClients.Window, ref _model.TransverClients.IsVisible);
                new PreRegistraationWindow().ShowDialog();
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
            HideListClients(_model.TransverClients.Window, ref _model.TransverClients.IsVisible);
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
        HideListClients(_model.TransverClients.Window, ref _model.TransverClients.IsVisible);

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
        HideListClients(_model.TransverClients.Window, ref _model.TransverClients.IsVisible);

        ButtonServingVisibility = Visibility.Visible;
        ButtonMainVisibility = Visibility.Collapsed;
        ButtonServingFinishVisibility = Visibility.Collapsed;
        Cliente = clientModel.TicketName;

        await clientModel.Call(); 

        BreakServisingEvent = null;
        StartServisingEvent = null;
        DeferServisingEvent = null;
        TransverServisingEvent = null;
        StopServisingEvent = null;

        //не явился
        BreakServisingEvent += async (s, e) =>
        {
            await clientModel.BreakServising();
            ButtonMainShow();
            Refresh();
        };

        //начать обслуживание
        StartServisingEvent += async (s, e) =>
        {
            ButtonServingVisibility = Visibility.Collapsed;
            ButtonServingFinishVisibility = Visibility.Visible;
            ButtonMainVisibility = Visibility.Collapsed;
            await clientModel.StartServising();
            TimerStart();


            //отложить
            DeferServisingEvent += async (s, e) =>
            {
                ConfirmationDialogWindow confirmationDialog = new();
                if (confirmationDialog.ShowDialog() == true)
                {
                    await clientModel.DeferServising();
                    ButtonMainShow();
                    Refresh();
                    DeferServisingEvent = null;
                }
                confirmationDialog.Close();
            };

            //передать
            TransverServisingEvent += async (s, e) =>
            {
                TransverClientWindow transverClientWindow = new(clientModel);
                if (transverClientWindow.ShowDialog() == true)
                {
                    ButtonMainShow();
                    Refresh();
                    TransverServisingEvent = null;
                }
            };

            //завершить
            StopServisingEvent += async (s, e) =>
            {
                await clientModel.StopServising();
                ButtonMainShow();
                Refresh();
                StopServisingEvent = null;
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
        catch (Exception ex)
        {

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
                Refresh();
            }
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

        CountClientes = modelMain.CountClientes;
        QueueClienteCount = modelMain.QueueClienteCount;
        TransferClienteCount = modelMain.TransferClienteCount;
        DeferClienteCount = modelMain.DeferClienteCount;
        ClienteId = modelMain.ClienteId;

        if (ButtonServingFinishVisibility != Visibility.Visible && ButtonServingVisibility != Visibility.Visible)
        {
            Cliente = modelMain.Cliente;
        }
    }
    private void TimerStart()
    {
        _model.TimerServising = new DateTime();
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
        if (_model.Timer != null) _model.Timer.Stop();
        TimeServising = "00:00";
    }
    private void UpdateDateTime()
    {
        _model.TimerServising = _model.TimerServising.AddSeconds(1);
        TimeServising = _model.TimerServising.ToString("mm:ss");
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
        get => _model.TransverClients.Window;
        set
        {
            _model.TransverClients.Window = value;
        }
    }
    public string? IPAdress
    {
        get => _model.IpAdress;
        set
        {
            _model.IpAdress = value;
            NotifyPropertyChanged("IPAdress");
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
            NotifyPropertyChanged("WindowId");
        }
    }
    public string? WindowName
    {
        get => _model.WindowsName;
        set
        {
            _model.WindowsName = value;
            NotifyPropertyChanged("WindowsName");
        }
    }
    public string? Cliente
    {
        get => _model.Cliente;
        set
        {
            _model.Cliente = value;
            NotifyPropertyChanged("Cliente");
        }
    }
    public long ClienteId
    {
        get => _model.ClienteId;
        set
        {
            _model.ClienteId = value;
            NotifyPropertyChanged("ClienteId");
        }
    }
    public string? ErrorMessage
    {
        get => _model.ErrorMessage;
        set
        {
            _model.ErrorMessage = value;
            NotifyPropertyChanged("ErrorMessage");
        }
    }
    public string? CountClientes
    {
        get => _model.CountClientes;
        set
        {
            _model.CountClientes = value;
            NotifyPropertyChanged("CountClientes");
        }
    }
    public string? QueueClienteCount
    {
        get => _model.QueueClienteCount;
        set
        {
            _model.QueueClienteCount = value;
            NotifyPropertyChanged("QueueClienteCount");
        }
    }
    public string? TransferClienteCount
    {
        get => _model.TransferClienteCount;
        set
        {
            _model.TransferClienteCount = value;
            NotifyPropertyChanged("TransferClienteCount");
        }
    }
    public string? DeferClienteCount
    {
        get => _model.DeferClienteCount;
        set
        {
            _model.DeferClienteCount = value;
            NotifyPropertyChanged("DeferClienteCount");
        }
    }
}