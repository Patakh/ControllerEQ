using System.Windows; 
using ControllerEQ.HandleKey;
using ControllerEQ.ViewModel.Error;
namespace ControllerEQ.Views.Error;

/// <summary>
/// Логика взаимодействия для ConfirmationDialogWindow.xaml
/// </summary>
public partial class ErrorWindow : Window
{
    public ErrorWindow(string errorText)
    {
        DataContext = new ErrorWindowViewModel(errorText, this);
        PreviewKeyDown += (s, e) =>
        {
            KeysDown.Press(e);
        };

        InitializeComponent();
    }
}

