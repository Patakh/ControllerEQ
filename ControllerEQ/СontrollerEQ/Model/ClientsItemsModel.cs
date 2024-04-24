using Microsoft.Windows.Themes;
using System;
using СontrollerEQ.Command;
using СontrollerEQ.Views;
namespace СontrollerEQ.Model;

public class ClientListWindowModel
{
    public ClientListWindowModel(Status status)
    {
        Window = new ClientListWindow(status);
    }

    public ClientListWindow Window;
    public bool IsVisible = false;
    public RelayCommand ShowWindowCommand;
    public RelayCommand ServisingCommand;
    public event EventHandler ServisingEvent; 
}
