using System.Windows;
using ControllerEQ.ViewModel.Modal;
namespace ControllerEQ.Views.Modal;

/// <summary>
/// Логика взаимодействия для ConfirmationDialogWindow.xaml
/// </summary>
public partial class ConfirmationDialogWindow : Window
{
    public ConfirmationDialogWindow()
    {
        InitializeComponent();
        DataContext = new ConfirmationDialogWindowViewModel(this);
    }
}

