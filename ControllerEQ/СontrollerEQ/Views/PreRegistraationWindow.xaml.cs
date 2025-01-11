using ControllerEQ.ViewModel;
using System.Windows;

namespace ControllerEQ.Views;

/// <summary>
/// Логика взаимодействия для PreRegistrationWindow.xaml
/// </summary>
public partial class PreRegistrationWindow : Window
{
    public PreRegistrationWindow()
    {
        InitializeComponent();
        DataContext = new PreRegistraationViewModel(this);
    }
}
