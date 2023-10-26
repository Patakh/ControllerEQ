using СontrollerEQ.Context;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

public class Client
{
    public static async Task SendMessageAsync(string message, string IpWindow)
    {
        EqContext eqContext = new EqContext();

        var scoreboards = eqContext.SOfficeScoreboards.Where(x => x.SOfficeId == eqContext.SOfficeWindows.First(g => g.WindowIp == IpWindow).SOfficeId);

        if (scoreboards.Any())
        {
            scoreboards.ToList().ForEach(async x =>
             { 
                 // Подключение к серверу
                 try
                 {
                     using (TcpClient client = new TcpClient())
                     {
                         await client.ConnectAsync(IPAddress.Parse(x.ScoreboardIp), 1234);

                         using (NetworkStream stream = client.GetStream())
                         {
                             byte[] buffer = Encoding.UTF8.GetBytes(message);
                             await stream.WriteAsync(buffer, 0, buffer.Length);
                         }
                     }
                 }
                 catch (Exception ex)
                 {

                 }

             });
        }

    }
}
