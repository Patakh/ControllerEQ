using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using Function;
using Microsoft.EntityFrameworkCore;
using TVQE.Model.Data.Context;

namespace TVQE.Model.Data;

public class DataWorker
{
    static public EqContext _context;
    static public SOfficeScoreboard? OfficeScoreboards;
    static DataWorker()
    {
        _context = new EqContext();
        List<string> computerIpAdreses = Dns.GetHostAddresses(Dns.GetHostName()).Where(w => w.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            .Select(s => s.ToString())
            .ToList();

        OfficeScoreboards = _context.SOfficeScoreboards
            .AsNoTracking()
            .FirstOrDefault(s => computerIpAdreses.Contains(s.ScoreboardIp)) ?? new();
    }

    static public string GetOfficeName() =>
      _context.SOffices
        .First(l => OfficeScoreboards.Id != null && OfficeScoreboards.Id != 0 && l.Id == OfficeScoreboards.SOfficeId).OfficeName;

    static public string GetRunningText() =>
        _context.SOfficeScoreboardTexts
        .AsNoTracking()
        .FirstOrDefault(l => l.IsActive && l.SOfficeScoreboardId == OfficeScoreboards.Id)?.TextMonitor
        .ToString() ?? "";

    static public ObservableCollection<Ticket> GetTickets() =>
            Tickets.SelectTicketServed(OfficeScoreboards.Id);

    static public DTicket? GetTicket(long id) =>
        _context.DTickets
        .AsNoTracking()
        .First(t => t.Id == id);

    static public string GetWindowName(long? windowId) =>
        _context.SOfficeWindows
        .AsNoTracking()
        .First(w => w.Id == windowId).WindowName;

    static public ColorsTV GetColorsTV() =>
       new ColorsTV()
       {
           TicketsForeground = _context.SColors.AsNoTracking().First(s => s.Id == 9).ColorValue,
           TitleForeground = _context.SColors.AsNoTracking().First(s => s.Id == 8).ColorValue,
           RunningTextForeground = _context.SColors.AsNoTracking().First(s => s.Id == 10).ColorValue,
           RunningTextBackground = _context.SColors.AsNoTracking().First(s => s.Id == 11).ColorValue
       };

    static public List<SVoice> GetVoices() =>
        _context.SVoices
        .AsNoTracking()
        .GroupBy(s => s.VoiceName)
        .Select(e => e.First())
        .ToList();

    static public List<string> GetBanners() =>
     _context.SOfficeScoreboardMultimedia
        .AsNoTracking()
        .Where(w => w.IsActive && w.SOfficeScoreboardId == OfficeScoreboards.Id)
        .Select(s => s.MultimediaPath)
        .ToList();
}

