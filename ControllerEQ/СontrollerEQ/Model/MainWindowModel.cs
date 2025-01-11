using System;
using System.Windows;
using System.Windows.Threading;
using ControllerEQ.Command;

namespace ControllerEQ.Model;

public class MainWindowModel
{
    public string? IpAddress;
    public long? WindowId;
    public string? WindowsName;
    public string? Client;
    public long ClientId;
    public string? ErrorMessage;
    public string? CountClients;
    public string? QueueClientsCount;
    public string? TransferClientsCount { get; set; }
    public string? DeferClientsCount;
    public bool IsShow = true;

    public string TimeServicing = "00:00";
    public DateTime TimerServicing;

    public DispatcherTimer Timer;
    public ClientListWindowModel? NewClients;
    public ClientListWindowModel? DeferClients;
    public ClientListWindowModel? TransversClients;
      
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