﻿using ControllerEQ.Command;
using ControllerEQ.Views;
using System;
namespace ControllerEQ.Model;

public class ClientListWindowModel
{
    public ClientListWindowModel(Status status) =>  Window = new ClientListWindow(status);
    public ClientListWindow Window;
    public bool IsVisible = false;
    public RelayCommand ShowWindowCommand;
    public RelayCommand ServicingCommand;
    public event EventHandler ServicingEvent; 
}
