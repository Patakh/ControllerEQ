using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace QE.Sockets
{
    public class Client
    {
        public static void SendMessage(List<string> windowIp, string message)
        {
            windowIp.ForEach(async x =>
            {
                try
                {
                    using (TcpClient client = new TcpClient())
                    {
                        await client.ConnectAsync(IPAddress.Parse(x), 1234);
                        using (NetworkStream stream = client.GetStream())
                        {
                            byte[] buffer = Encoding.UTF8.GetBytes(message);
                            await stream.WriteAsync(buffer, 0, buffer.Length);
                        }
                    }
                }
                catch { };
            });
        }
    }
}
