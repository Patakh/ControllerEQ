using Function;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using СontrollerEQ.Command;
using СontrollerEQ.Model;
using СontrollerEQ.Model.Data;
using СontrollerEQ.Model.Data.Context;

namespace СontrollerEQ.ViewModel;
class PreRegistraationViewModel : INotifyPropertyChanged
{
    private PreRegistraationModel _preRegModel = new();
    public PreRegistraationViewModel(Window window)
    {
        _preRegModel.DateList = DataWorker.GetDatePreRegistration();
        _preRegModel.OfficeId = DataWorker.GetOfficeId();

        VisibleDate();

        _preRegModel.Window = window;
        foreach (var item in _preRegModel.DateList)
        {
            item.ClickEvent += (s, e) =>
            {
                Date = item.Date;
                foreach (var _item in _preRegModel.DateList)
                {
                    _item.ButtonBackground = new SolidColorBrush(Colors.Blue);
                    _item.ButtonForground = new SolidColorBrush(Colors.White);
                }
                item.ButtonBackground = new SolidColorBrush(Colors.White);
                item.ButtonForground = new SolidColorBrush(Colors.Blue);

                NotifyPropertyChanged("ButtonBackground");
                NotifyPropertyChanged("ButtonForground");

                VisibleTime();

                TimeList = item.PreRegistraationTimes;
            };
        }
    }

    public string Fio
    {
        get => _preRegModel.Fio;
        set
        {
            _preRegModel.Fio = value;
            ErrorText = "";
            NotifyPropertyChanged("Fio");
        }
    }
    public string Phone
    {
        get => _preRegModel.Phone;
        set
        {
            _preRegModel.Phone = value;
            ErrorText = "";
            NotifyPropertyChanged("Phone");
        }
    }
    public string ErrorText
    {
        get => _preRegModel.ErrorText;
        set
        {
            _preRegModel.ErrorText = value;
            NotifyPropertyChanged("ErrorText");
        }
    }
    public long Code
    {
        get => _preRegModel.Code;
        set
        {
            _preRegModel.Code = value;
            NotifyPropertyChanged("Code");
        }
    }
    public DateTime Date
    {
        get => _preRegModel.Date;
        set
        {
            _preRegModel.Date = value;
            NotifyPropertyChanged("Date");
        }
    }
    public TimeSpan TimeStart
    {
        get => _preRegModel.TimeStart;
        set
        {
            _preRegModel.TimeStart = value;
            NotifyPropertyChanged("TimeStart");
        }
    }
    public TimeSpan TimeStop
    {
        get => _preRegModel.TimeStop;
        set
        {
            _preRegModel.TimeStop = value;
            NotifyPropertyChanged("TimeStop");
        }
    }
    public Visibility VisibilityDate
    {
        get => _preRegModel.VisibilityDate;
        set
        {
            _preRegModel.VisibilityDate = value;
            NotifyPropertyChanged("VisibilityDate");
        }
    }
    public Visibility VisibilityTime
    {
        get => _preRegModel.VisibilityTime;
        set
        {
            _preRegModel.VisibilityTime = value;
            NotifyPropertyChanged("VisibilityTime");
        }
    }
    public Visibility VisibilityBack
    {
        get => _preRegModel.VisibilityBack;
        set
        {
            _preRegModel.VisibilityBack = value;
            NotifyPropertyChanged("VisibilityBack");
        }
    }
    public Visibility VisibilityForm
    {
        get => _preRegModel.VisibilityForm;
        set
        {
            _preRegModel.VisibilityForm = value;
            NotifyPropertyChanged("VisibilityForm");
        }
    }
    public Visibility VisibilityCode
    {
        get => _preRegModel.VisibilityCode;
        set
        {
            _preRegModel.VisibilityCode = value;
            NotifyPropertyChanged("VisibilityCode");
        }
    }

    public ObservableCollection<PreRegistraationDate> DateList
    {
        get => _preRegModel.DateList;
        set
        {
            _preRegModel.DateList = value;

            NotifyPropertyChanged("DateList");
        }
    }
    public ObservableCollection<PreRegistraationTime> TimeList
    {
        get => _preRegModel.TimeList;
        set
        {
            _preRegModel.TimeList = value;

            foreach (var item in value)
            {
                item.ClickEvent += (s, e) =>
                {
                    TimeStart = item.StartTimePrerecord;
                    TimeStop = item.StopTimePrerecord;
                    VisibleForm();

                    foreach (var _item in _preRegModel.TimeList)
                    {
                        _item.ButtonBackground = new SolidColorBrush(Colors.Blue);
                        _item.ButtonForground = new SolidColorBrush(Colors.White);
                    }

                    item.ButtonBackground = new SolidColorBrush(Colors.White);
                    item.ButtonForground = new SolidColorBrush(Colors.Blue);

                    BackClickEvent += (s, e) =>
                    {
                        VisibleTime();
                    };
                };
            }

            NotifyPropertyChanged("TimeList");
        }
    }
    public event EventHandler BackClickEvent;
    public RelayCommand Back
    {
        get
        {
            return new RelayCommand(async odj =>
            {
                BackClickEvent.Invoke(odj, EventArgs.Empty);
            },
             _ => true
            );
        }
    }
    public RelayCommand Clouse
    {
        get
        {
            return new RelayCommand(async odj =>
            {
                _preRegModel.Window.Close();
            },
             _ => true
            );
        }
    }
    public RelayCommand SendCommand
    {
        get
        {
            return new RelayCommand(async odj =>
            {
                ErrorText = "";

                if (string.IsNullOrEmpty(Fio))
                {
                    ErrorText = "ФИО не заполнено!\n";
                    return;
                }
                if (Phone.Length < 18)
                {
                    ErrorText += "Телефон не корректен!";
                    return;
                }


                DTicketPrerecord dTicketPrerecord = new();
                dTicketPrerecord.CustomerFullName = _preRegModel.Fio;
                dTicketPrerecord.CustomerPhoneNumber = Phone;
                dTicketPrerecord.DatePrerecord = DateOnly.Parse(_preRegModel.Date.ToString("d"));
                dTicketPrerecord.StartTimePrerecord = TimeOnly.Parse(_preRegModel.TimeStart.ToString("hh\\:mm"));
                dTicketPrerecord.StopTimePrerecord = TimeOnly.Parse(_preRegModel.TimeStop.ToString("hh\\:mm"));
                dTicketPrerecord.IsConfirmation = false;
                dTicketPrerecord.SSourcePrerecordId = 2;

                dTicketPrerecord = await DataWorker.PreRecordSaveAsync(_preRegModel.OfficeId, dTicketPrerecord);

                Code = dTicketPrerecord.CodePrerecord;
                VisibleCode();
            },
             _ => true
            );
        }
    }
    public RelayCommand ClouseCommand
    {
        get
        {
            return new RelayCommand(odj =>
            {
                _preRegModel.Window.Close();
            }, _ => true
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
    void VisibleDate()
    {
        VisibilityDate = Visibility.Visible;
        VisibilityTime = Visibility.Collapsed;
        VisibilityBack = Visibility.Collapsed;
        VisibilityForm = Visibility.Collapsed;
        VisibilityCode = Visibility.Collapsed;
    }
    void VisibleTime()
    {
        VisibilityBack = Visibility.Visible;
        VisibilityTime = Visibility.Visible;
        VisibilityDate = Visibility.Collapsed;
        VisibilityForm = Visibility.Collapsed;
        VisibilityCode = Visibility.Collapsed;
        BackClickEvent += (s, e) => VisibleDate();
    }
    void VisibleForm()
    {
        VisibilityForm = Visibility.Visible;
        VisibilityBack = Visibility.Visible;
        VisibilityDate = Visibility.Collapsed;
        VisibilityTime = Visibility.Collapsed;
        VisibilityCode = Visibility.Collapsed;
    }
    void VisibleCode()
    {
        VisibilityForm = Visibility.Collapsed;
        VisibilityBack = Visibility.Collapsed;
        VisibilityDate = Visibility.Collapsed;
        VisibilityTime = Visibility.Collapsed;
        VisibilityCode = Visibility.Visible;
    }
}
