using System;
using System.Windows;
using ControllerEQ.Model;
using ControllerEQ.ViewModel;

namespace ControllerEQ.Views; 
public partial class TransverClientWindow : Window
{ 
    public TransverClientWindow(ClientModel client)
    {
        InitializeComponent();
        DataContext = new TransverClientWindowViewModel(client, this); 
    } 
}