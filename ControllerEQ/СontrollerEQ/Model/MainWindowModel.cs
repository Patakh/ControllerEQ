using System;
using System.Windows;
using System.Windows.Threading;
using СontrollerEQ.Command;

namespace СontrollerEQ.Model;

public class MainWindowModel
{
    public string? IpAdress;
    public long? WindowId;
    public string? WindowsName;
    public string? Cliente;
    public long ClienteId;
    public string? ErrorMessage;
    public string? CountClientes;
    public string? QueueClienteCount;
    public string? TransferClienteCount { get; set; }
    public string? DeferClienteCount;
    public bool IsShow = true;

    public string TimeServising = "00:00";
    public DateTime TimerServising;

    public DispatcherTimer Timer;
    public ClientListWindowModel? NewClients;
    public ClientListWindowModel? DeferClients;
    public ClientListWindowModel? TransverClients;
      
    public Visibility ButtonServingVisibility = Visibility.Collapsed;
    public Visibility ButtonServingFinishVisibility = Visibility.Collapsed;
    public Visibility ButtonMainVisibility = Visibility.Visible;
    public Visibility BodyVisibility = Visibility.Visible;

    public RelayCommand Call;
    public RelayCommand ShowMainWindowCommand;
    public RelayCommand StartServicingCommand;
    public RelayCommand BreakServisingCommand;
    public RelayCommand StopServisingCommand;
    public RelayCommand TransverServisingCommand;
    public RelayCommand DeferServisingCommand;
    public RelayCommand PreRegistrationCommand;    
}