using System.Windows;
using TVQE.HandleKey;
using TVQE.ViewModel.Error;
namespace TVQE.Views.Error;

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

