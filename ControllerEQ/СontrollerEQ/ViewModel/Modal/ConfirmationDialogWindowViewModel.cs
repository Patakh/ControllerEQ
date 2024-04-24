using System.Windows;
using СontrollerEQ.Command;

namespace СontrollerEQ.ViewModel.Modal;

public class ConfirmationDialogWindowViewModel
{
    private Window _window;
    public ConfirmationDialogWindowViewModel(Window window)
    { 
        _window = window;
        Window mainWindow = Application.Current.MainWindow;
        _window.Owner = mainWindow;
        double left = mainWindow.Left + (mainWindow.Width - _window.Width) / 2;
        double top = mainWindow.Top + (mainWindow.Height - _window.Height) / 2;

        _window.Left = left;
        _window.Top = top; 
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

    private RelayCommand _cancelCommand;
    public RelayCommand CancelCommand
    {
        get
        {
            return _cancelCommand ?? new RelayCommand(odj =>
            {
                _window.DialogResult = false;
            }, _ => true
            );
        }
    }
}
