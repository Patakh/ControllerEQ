using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows; 
using TVQE.Model.Data;
using System.Windows.Media.Media3D;

namespace TVQE.Model;
public class MainWindowModel
{
    public string Title;
    public string Date;
    public string Time;
    public string DayName;
    public string Ticket;
    public string Window;
    public int CurrentIndexBanner;
    public int CurrentIndexAudio;
    public Visibility TicketCallVisibility;
    public Visibility BannerVisibility;
    public AxisAngleRotation3D RotationClient;
    public AxisAngleRotation3D RotationClientList;
    public MainWindowModel()
    {
        Title = DataWorker.GetOfficeName();
        Tickets = DataWorker.GetTickets();
        BannerFiles = DataWorker.GetBanners();
        BannerVisibility = Visibility.Visible;
        TicketCallVisibility = Visibility.Collapsed;
    }

    public DispatcherTimer Timer;
    public ObservableCollection<Ticket> Tickets;
    public List<string> AudioFiles = new();
    public List<string> BannerFiles;
    public List<CallTickets> CallTickets = new();
    public StackPanel MediaElementsBanner;
    public MediaElement Audio = new();
    public MediaElement Banner = new();
    public TextBlock RunningText;
    public Canvas Canvas;
}