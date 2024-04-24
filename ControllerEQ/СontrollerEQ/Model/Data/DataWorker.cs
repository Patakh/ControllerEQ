using Function;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using СontrollerEQ.Model.Data.Context;

namespace СontrollerEQ.Model.Data;

static class DataWorker
{
    private static EqContext _context;
    public static SOfficeWindow? Window;
    static DataWorker()
    {
        _context = new EqContext();

        List<string> computerIpAdreses = Dns.GetHostAddresses(Dns.GetHostName())
            .Where(w => w.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            .Select(s => s.ToString())
            .ToList();

        Window = _context.SOfficeWindows.AsNoTracking().FirstOrDefault(s => computerIpAdreses.Contains(s.WindowIp)) ?? new();
    }
    public static List<SOfficeScoreboard> GetOfficeScoreboards() =>
        _context.SOfficeScoreboards
        .Where(x => x.SOfficeId == _context.SOfficeWindows.AsNoTracking().First(g => g.WindowIp == Window.WindowIp).SOfficeId)
        .ToList();
    public static List<SOfficeWindow> GetOfficeWindows() =>
        _context.SOfficeWindows
        .Where(x => x.SOfficeId == _context.SOfficeWindows.AsNoTracking().First(g => g.WindowIp == Window.WindowIp).SOfficeId && x.WindowIp != Window.WindowIp)
        .ToList();
    public static MainWindowModel GetMainWindowData()
    {
        MainWindowModel mainWindowModel = new();
        var ticket = TicketCall.GetNextTicketAsync(Window.Id).Result;
        var ticketsTransferCount = TicketTransferred.SelectTicketTransferredtAsync(Window.Id).Result;
        var ticketsPostponed = TicketPostponed.SelectTicketPostponedAsync(Window.Id).Result;

        mainWindowModel.IpAdress = Window.WindowIp;
        mainWindowModel.WindowId = Window.Id;
        mainWindowModel.WindowsName = Window.WindowName;
        mainWindowModel.CountClientes = _context.DTickets.AsNoTracking().Where(s =>
        s.SOfficeId == Window.SOfficeId
        && s.SStatusId != (int)Status.NeverShowed
        && s.SStatusId != (int)Status.Completed
        && s.DateRegistration == DateOnly.FromDateTime(DateTime.Now))
            .Count()
            .ToString();

        mainWindowModel.QueueClienteCount = GetClientListCount();
        mainWindowModel.ClienteId = ticket.Id;
        mainWindowModel.Cliente = ticket.TicketNumberFull == null ? "---" : ticket.TicketNumberFull;
        mainWindowModel.TransferClienteCount = ticketsTransferCount.Count.ToString() == "0" ? "-" : ticketsTransferCount.Count.ToString();
        mainWindowModel.DeferClienteCount = ticketsPostponed.Count.ToString() == "0" ? "-" : ticketsPostponed.Count.ToString();

        return mainWindowModel;
    }
    public static ObservableCollection<ClientModel> GetClientTransferListData()
    {
        var clientList = new ObservableCollection<ClientModel>();
        var tickets = TicketTransferred.SelectTicketTransferredtAsync(Window.Id).Result;
        if (tickets.Any())
        {
            tickets.ForEach(cl =>
            {
                clientList.Add(new ClientModel
                {
                    Id = cl.Id,
                    TicketName = cl.TicketNumberFull,
                });
            });
        }

        return clientList;
    }
    public static ObservableCollection<ClientModel> GetClientDeferListData()
    {
        var clientList = new ObservableCollection<ClientModel>();
        var tickets = TicketPostponed.SelectTicketPostponedAsync(Window.Id).Result;
        if (tickets.Any())
        {
            tickets.ForEach(cl =>
            {
                clientList.Add(new ClientModel
                {
                    Id = cl.Id,
                    TicketName = cl.TicketNumberFull,
                });
            });
        }
        return clientList;
    }
    public static ObservableCollection<ClientModel> GetClientNewListData()
    {
        var tickets = _context.DTickets.AsNoTracking().Where(s =>
        s.SOfficeId == Window.SOfficeId
        && s.SOfficeWindowId == null
        && s.SStatusId == 1
        && s.DateRegistration == DateOnly.FromDateTime(DateTime.Now)).OrderBy(r => r.TicketNumber).ToList();

        var clientList = new ObservableCollection<ClientModel>();
        if (tickets.Any())
        {
            tickets.ForEach(cl =>
            {
                clientList.Add(new ClientModel
                {
                    Id = cl.Id,
                    TicketName = cl.TicketNumberFull,
                });
            });
        }
        return clientList;
    }
    public static string? GetClientListCount()
    {
        var count = _context.DTickets.AsNoTracking().Where(s =>
       s.SOfficeId == Window.SOfficeId
       && s.SStatusId == 1
       && s.DateRegistration == DateOnly.FromDateTime(DateTime.Now)).OrderBy(r => r.TicketNumber).Count();
        return count.ToString();
    }
    public static async Task CallOperation(long ticketId, Status status, long? idWindowTransfer = null)
    {
        try
        {
            if (Window != null && ticketId != 0)
            {
                var changeTicket = _context.DTickets.First(x => x.Id == ticketId);
                changeTicket.SStatusId = (int)status;
                changeTicket.SOfficeWindowId = Window.Id;
                _context.DTicketStatuses.Add(new DTicketStatus
                {
                    DTicketId = ticketId,
                    SStatusId = (int)status,
                    SOfficeWindowId = Window.Id,
                    SOfficeWindowIdTransferred = idWindowTransfer,
                });
                _context.SaveChanges();
                await Client.SendMessageAsyncNewTicket("new Ticket");
            }
        }
        catch (Exception ex)
        {

        }
    }
    public static async Task TransferClient(long idCliente, long idWindow)
    {
        var changeStatus = _context.DTicketStatuses.First(x => x.DTicketId == idCliente);
        changeStatus.SOfficeWindowIdTransferred = idWindow;
        await _context.SaveChangesAsync();
    } 
    public static ObservableCollection<PreRegistraationDate> GetDatePreRegistration()
    {
        var data = Prerecord.GetDateList(Window.SOfficeId, DateOnly.FromDateTime(DateTime.Now))
            .GroupBy(g => new { g.Date, g.DayName })
            .Select(s => new { s.Key.Date, s.Key.DayName, Time = s.ToList() })
            .ToList();

        ObservableCollection<PreRegistraationDate> result = new();

        foreach (var item in data)
        {
            var preRegistraationModel = new PreRegistraationDate
            {
                Date = item.Date,
                DayName = item.DayName,
                PreRegistraationTimes = new ObservableCollection<PreRegistraationTime>()
            };

            item.Time.ForEach(x =>
            {
                var preRegistrTime = new PreRegistraationTime
                {
                    StartTimePrerecord = x.StartTimePrerecord,
                    StopTimePrerecord = x.StopTimePrerecord,
                };
                preRegistraationModel.PreRegistraationTimes.Add(preRegistrTime);
            });

            result.Add(preRegistraationModel);
        }

        return result;
    }
    public static long GetOfficeId() => Window.SOfficeId;
    public static async Task<DTicketPrerecord> PreRecordSaveAsync(long officeId, DTicketPrerecord request) =>
        await _context.PreRecordSaveAsync(officeId, request);
}