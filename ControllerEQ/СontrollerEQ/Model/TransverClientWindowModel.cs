﻿using System;
using System.ComponentModel;
using System.Windows.Media;
using ControllerEQ.Command;
namespace ControllerEQ.Model;
public class TransferClientWindowModel : INotifyPropertyChanged
{
    public long WindowId { get; set; }
    public string WindowName { get; set; }

    private Brush _windowBackground;
    public Brush _windowForground;

    public Brush WindowBackground
    {
        get => _windowBackground;
        set
        {
            _windowBackground = value;
            NotifyPropertyChanged("WindowBackground");
        }
    }

    public Brush WindowForground
    {
        get => _windowForground;
        set
        {
            _windowForground = value;
            NotifyPropertyChanged("WindowForeground");
        }
    }  
     

    public event EventHandler ClickWindowEvent;
    public RelayCommand CallClientCommand
    {
        get
        {
            return new RelayCommand(async odj =>
            {
                ClickWindowEvent.Invoke(this, EventArgs.Empty);
            },
             _ => true
            );
        }
    }
    public event PropertyChangedEventHandler PropertyChanged;
    private void NotifyPropertyChanged(string propertyName)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
