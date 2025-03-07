﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using ControllerEQ.Command;

namespace ControllerEQ.Model;
public class PreRegistraationModel
{
    public string Fio;
    public string Phone = "+7";
    public string ErrorText;
    public long OfficeId;
    public long Code;
    public DateTime Date;
    public TimeSpan TimeStart;
    public TimeSpan TimeStop;
    public Window Window;
    public ObservableCollection<PreRegistraationDate> DateList;
    public ObservableCollection<PreRegistrationTime> TimeList;
    public Visibility VisibilityDate;
    public Visibility VisibilityTime;
    public Visibility VisibilityBack;
    public Visibility VisibilityForm;
    public Visibility VisibilityCode;
    public event EventHandler ClickEvent;
    public RelayCommand ButtonClickCommand
    {
        get
        {
            return new RelayCommand(async odj =>
            {
                ClickEvent.Invoke(this, EventArgs.Empty);
            },
             _ => true
            );
        }
    }
}

public class PreRegistraationDate : INotifyPropertyChanged
{
    public long SDayWeekId { get; set; }
    public string DayName { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan StartTimePrerecord { get; set; }
    public TimeSpan StopTimePrerecord { get; set; }
    public Brush _buttonBackground  = new SolidColorBrush(Colors.Blue);
    public Brush _buttonForeground = new SolidColorBrush(Colors.White);
    public Brush ButtonBackground
    {
        get => _buttonBackground;
        set
        {
            _buttonBackground = value;
            NotifyPropertyChanged("ButtonBackground");
        }
    }
    public Brush ButtonForground
    {
        get => _buttonForeground;
        set
        {
            _buttonForeground = value;
            NotifyPropertyChanged("ButtonForeground");
        }
    }
    public ObservableCollection<PreRegistrationTime> PreRegistraationTimes { get; set; }
    public event EventHandler ClickEvent;
    public RelayCommand ButtonClickCommand
    {
        get
        {
            return new RelayCommand(async odj =>
            {
                ClickEvent.Invoke(this, EventArgs.Empty);
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

public class PreRegistrationTime:INotifyPropertyChanged
{
    public TimeSpan StartTimePrerecord { get; set; }
    public TimeSpan StopTimePrerecord { get; set; }
    public Brush _buttonBackground = new SolidColorBrush(Colors.Blue);
    public Brush _buttonForeground = new SolidColorBrush(Colors.White);
    public Brush ButtonBackground
    {
        get => _buttonBackground;
        set
        {
            _buttonBackground = value;
            NotifyPropertyChanged("ButtonBackground");
        }
    }
    public Brush ButtonForeground
    {
        get => _buttonForeground;
        set
        {
            _buttonForeground = value;
            NotifyPropertyChanged("ButtonForeground");
        }
    }

    public event EventHandler ClickEvent;
    public RelayCommand ButtonClickCommand
    {
        get
        {
            return new RelayCommand(async odj =>
            {
                ClickEvent.Invoke(this, EventArgs.Empty); 
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