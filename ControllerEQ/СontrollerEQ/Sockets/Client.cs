using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ControllerEQ.Model.Data;

public class Client
{
    public static async Task SendMessageAsync(string message)
    {
        try
        {
            var scoreboards = DataWorker.GetOfficeScoreboards();
            if (scoreboards.Any())
            {
                scoreboards.ToList().ForEach(async x =>
                 {
                     // Подключение к серверу Табло
                     try
                     {
                         using TcpClient client = new();
                         await client.ConnectAsync(IPAddress.Parse(x.ScoreboardIp), 1235);

                         using NetworkStream stream = client.GetStream();
                         byte[] buffer = Encoding.UTF8.GetBytes(message);
                         await stream.WriteAsync(buffer, 0, buffer.Length);
                     }
                     catch { }
                 });
            }

            if (!string.IsNullOrEmpty(DataWorker.Window.ElectronicScoreboardIp))
            {
                // Подключение к серверу Табло над оператором
                try
                {
                    using TcpClient client = new();
                    await client.ConnectAsync(IPAddress.Parse(DataWorker.Window.ElectronicScoreboardIp), 1236);

                    using NetworkStream stream = client.GetStream();
                    byte[] buffer = Encoding.UTF8.GetBytes(message);
                    await stream.WriteAsync(buffer, 0, buffer.Length);
                }
                catch { }
            }
        }
        catch (Exception ex)
        {
        }
    }

    public static async Task SendMessageAsyncNewTicket(string message)
    {
        var windows = DataWorker.GetOfficeWindows();
        if (windows.Any())
        {
            windows.ToList().ForEach(async x =>
            {
                // Подключение к серверу Терминала и к Окнам
                try
                {
                    using TcpClient client = new();
                    await client.ConnectAsync(IPAddress.Parse(x.WindowIp), 1234);

                    using NetworkStream stream = client.GetStream();
                    byte[] buffer = Encoding.UTF8.GetBytes(message);
                    await stream.WriteAsync(buffer, 0, buffer.Length);
                }
                catch { }
            });
        }
    }
}
