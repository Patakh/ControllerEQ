using System.Windows;
using СontrollerEQ.ViewModel.Modal;
namespace СontrollerEQ.Views.Modal;

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

