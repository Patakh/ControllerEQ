using System;
using System.Threading.Tasks;
using СontrollerEQ.Command;
using СontrollerEQ.Model.Data;

namespace СontrollerEQ.Model;

public class ClientModel
{
    public long Id { get; set; }

    public string? TicketName { get; set; }

    public event EventHandler CallClientEvent;

    public RelayCommand CallClientClick
    {
        get
        {
            return new RelayCommand(async odj =>
            {
                CallClientEvent.Invoke(this, EventArgs.Empty);
            },
             _ => true
            );
        }
    }

    public async Task Call()
    {
        await DataWorker.CallOperation(Id, Status.Call);
        await Client.SendMessageAsync("Call:" + Id);
        await Client.SendMessageAsyncNewTicket("new Ticket");
    }

    public async Task StartServising()
    {
        await DataWorker.CallOperation(Id, Status.Serviced);
        await Client.SendMessageAsync("Pocess");
    }

    public async Task BreakServising()
    {
        await DataWorker.CallOperation(Id, Status.NeverShowed);
        await RefreshMessage();
    }

    public async Task DeferServising()
    {
        await DataWorker.CallOperation(Id, Status.Defer);
        await RefreshMessage();
    }

    public async Task TransferClient(long sendWindowId)
    {
        await DataWorker.CallOperation(Id, Status.Transferred,sendWindowId);
        await RefreshMessage();
    }

    public async Task StopServising()
    {
        await DataWorker.CallOperation(Id, Status.Completed);
        await RefreshMessage();
    }

    public async Task RefreshMessage()
    {
        await Client.SendMessageAsync("Pocess");
        await Client.SendMessageAsyncNewTicket("new Ticket");
    }
}
