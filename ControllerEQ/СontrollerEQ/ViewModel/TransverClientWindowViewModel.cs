using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using ControllerEQ.Command;
using ControllerEQ.Model;
using ControllerEQ.Model.Data;
using ControllerEQ.Views;

namespace ControllerEQ.ViewModel;
public class TransverClientWindowViewModel : INotifyPropertyChanged
{
    private long _sendWindowId;
    private bool _isSend = false;
    private ObservableCollection<TransferClientWindowModel> _buttonTransver;
    private Window _window;
    private ClientModel _client;
    private Visibility _visibilitySend;

    public TransverClientWindowViewModel(ClientModel client, Window window)
    {
        _client = client;
        _window = window;
        _buttonTransver = Function.TicketCall.WindowResult(_client.Id);
        _window.Owner = Application.Current.MainWindow;
        _visibilitySend = Visibility.Hidden;

        foreach (var item in _buttonTransver)
        {
            item.WindowBackground = new SolidColorBrush(Colors.Blue);
            item.WindowForground = new SolidColorBrush(Colors.White);

            item.ClickWindowEvent += (s, e) =>
            {
                SendVisibility = Visibility.Visible;
                foreach (var _item in _buttonTransver)
                {
                    _item.WindowBackground = new SolidColorBrush(Colors.Blue);
                    _item.WindowForground = new SolidColorBrush(Colors.White);
                }

                item.WindowBackground = new SolidColorBrush(Colors.White);
                item.WindowForground = new SolidColorBrush(Colors.Blue);

                _sendWindowId = item.WindowId;
                _isSend = true;
            };
        }
    }

    public ObservableCollection<TransferClientWindowModel> ButtonTransver
    {
        get => _buttonTransver;
        set
        {
            _buttonTransver = value;
            NotifyPropertyChanged("ButtonTransver");
        }
    }

    public Visibility SendVisibility
    {
        get => _visibilitySend;
        set
        {
            _visibilitySend = value;
            NotifyPropertyChanged("SendVisibility");
        }
    }

    private RelayCommand _sendCommand;
    public RelayCommand SendCommand
    {
        get
        {
            return _sendCommand ?? new RelayCommand(async odj =>
            {
                await _client.TransferClient(_sendWindowId);
                _window.DialogResult = true;
                _window.Close();
            }, _ => _isSend
          );
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    private void NotifyPropertyChanged(string propertyName)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public RelayCommand ClouseCommand
    {
        get
        {
            return new RelayCommand(async odj =>
            {
                _window.DialogResult = false;
                _window.Close();
            }, _ => true
            );
        }
    }
}