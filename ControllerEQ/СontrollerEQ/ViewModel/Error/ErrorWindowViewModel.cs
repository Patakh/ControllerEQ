using System.ComponentModel;
using System.Windows;
using СontrollerEQ.Command;

namespace СontrollerEQ.ViewModel.Error;

public class ErrorWindowViewModel
{
    private Window _window;
    private string _messageError;

    public ErrorWindowViewModel(string message, Window window)
    {
        _messageError = message;
        _window = window;   
    }

    private RelayCommand _okCommand;

    public RelayCommand OkCommand
    {
        get
        {
            return _okCommand ?? new RelayCommand(odj =>
            {
                _window.DialogResult = true;
            }, _ => true
            );
        }
    }

    public string MessageError
    {
        get => _messageError;
        set
        {
            _messageError = value;
            NotifyPropertyChanged("MessageError");
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

}
