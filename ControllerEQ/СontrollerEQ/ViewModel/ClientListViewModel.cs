using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using ControllerEQ.Command;
using ControllerEQ.Model;
using ControllerEQ.Model.Data;
using ControllerEQ.Views;

namespace ControllerEQ.ViewModel;
class ClientListViewModel : INotifyPropertyChanged
{
    private ClientListWindow _window;
    private ObservableCollection<ClientModel> _clients;
    public ClientListViewModel(ClientListWindow window, Status Status)
    {
        _window = window;
        Statuss = Status;
        Clients = Status switch
        {
            Status.New => DataWorker.GetClientNewListData(),
            Status.Defer => DataWorker.GetClientDeferListData(),
            Status.Transferred => DataWorker.GetClientTransferListData(),
            _ => new ObservableCollection<ClientModel>()
        };

        Title = Status switch
        {
            Status.New => "На очереди",
            Status.Defer => "Отложенные",
            Status.Transferred => "Переданные",
            _ => "Окно"
        };

        foreach (var client in _clients)
        {
            client.CallClientEvent += (s, e) =>
            {
                Clients = DataWorker.GetClientNewListData();
            };
        }
    } 
    public ObservableCollection<ClientModel> Clients
    {
        get => _clients;
        set
        {
            foreach (var client in value)
            {
                client.CallClientEvent += async (s, e) =>
                { 
                    if(Application.Current.MainWindow.DataContext is MainWindowViewModel main)
                         main.CallClient(client);
                };
            }

            _clients = value;
            NotifyPropertyChanged("Clients");
        }
    } 
    public Status Statuss { get; set; } 
    private string _title;
    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            NotifyPropertyChanged("Title");
        }
    } 
    public event PropertyChangedEventHandler PropertyChanged; 
    public RelayCommand ClouseCommand
    {
        get
        {
            return new RelayCommand(odj =>
            {
                _window.Hide(); 
            }, _ => true
            );
        }
    } 
    private void NotifyPropertyChanged(string propertyName)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}