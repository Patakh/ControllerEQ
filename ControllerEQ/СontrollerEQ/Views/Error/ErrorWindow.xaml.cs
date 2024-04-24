using System.Windows; 
using СontrollerEQ.HandleKey;
using СontrollerEQ.ViewModel.Error;
namespace СontrollerEQ.Views.Error;

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

