using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ExampleWebServer
{
    public interface IWebService
    {
        string processRequestCallBack(string request);
    }

    public class WebServer
    {
        private TcpListener server = null;

        public WebServer(string ipAddress, int port)
        {
            IPAddress localAddr = IPAddress.Parse(ipAddress);
            server = new TcpListener(localAddr, port);
            server.Start();

        }
        public void Stop()
        {
            server.Stop();
      }

        async public Task<string >processRequest(IWebService callback)
        {
            TcpClient client = await server.AcceptTcpClientAsync();
            NetworkStream stream = client.GetStream();
            string response = callback.processRequestCallBack(GetRequest(stream));
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(response);
            stream.Write(msg, 0, msg.Length);
            client.Close();
            return response;

        }
        private  async Task<String>  GetRequestAsync(NetworkStream stream)
        {

 
            Byte[] bytes = new Byte[256];
            String data = null;
            StringBuilder totalString = new StringBuilder();
            int i;

            while (stream.DataAvailable && (i = await stream.ReadAsync(bytes, 0, bytes.Length)) != 0)
            {
                data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                totalString.Append(data);
            }
            return totalString.ToString();
        }
        public  String GetRequest(NetworkStream stream)
        {
            return GetRequestAsync(stream).Result.ToString();
        }


    }
}
