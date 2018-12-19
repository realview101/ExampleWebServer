using System;
using System.IO;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ExampleWebServer
{
  static   class Router
    {
        public static string pathToPage(string whichPage)
        {
            string[] segments = System.Environment.CurrentDirectory.Split('\\');
            StringBuilder rstr = new StringBuilder();
            foreach (string seg in segments)
            {
                if (seg == "bin") break;
                if (!seg.Contains(":")) rstr.Append("\\");

                rstr.Append(seg);
            }
            rstr.Append("\\Pages\\");
            rstr.Append( whichPage);
            return rstr.ToString();
        }
        public static string ABC()
        {
            string page = pathToPage("ABC\\Index.html");
            string text = System.IO.File.ReadAllText(page);
            return text;
        }
        public static string DEF()
        {
            string page = pathToPage("DEF\\Index.html");
            string text = System.IO.File.ReadAllText(page);
            return text;
        }
        public static string Home()
        {
            string page = pathToPage("Home\\Index.html");
            string text = System.IO.File.ReadAllText(page);
            return text;
        }
        public static string Error()
        {
            string page = pathToPage("Error\\Index.html");
            string text = System.IO.File.ReadAllText(page);
            return text;
        }
    }
    class WebServiceRequest : IWebService
    {
        public string processRequestCallBack(string request)
        {
            String body = "";
            string[] urlArray = request.Split(' ');
            if (urlArray.Length < 1 || urlArray[0] != "GET" || urlArray[1] == "/" || urlArray[1].ToUpper() == "/HOME")
            {
                body = Router.Home();
            }
            else if (urlArray[1].ToUpper() == "/ABC" ) {
                body = Router.ABC();
            }
            else if (urlArray[1].ToUpper() == "/DEF")
            {
                body = Router.DEF();
            }
            else
            {
                body = Router.Error();
            }
            Console.WriteLine("Request was: {0}", request);

            String response =
@"HTTP/1.1 200 OK
Server: Example
Accept-Ranges: bytes
Content-Length: " + body.Length.ToString() + @"
Content-Type: text/html

" + body;
            return response;
        }
    }

    class program {
        static  void   Main(string[] args)
        {
            WebServer Server = null;
            try
            {
                //string connetionString = "Data Source=54.213.195.209;Initial Catalog=Example;User ID=example;Password=example";
                //SqlConnection cnn = new SqlConnection(connetionString);
                //try
                //{
                //    cnn.Open();                    
                //    cnn.Close();
                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine("Can not open data connection");
                //}
                Server = new WebServer("127.0.0.1", 8081);
 
                Byte[] bytes = new Byte[256];
                Console.WriteLine("Entering wait loop...");
                while (true)
                {
                    Server.processRequest(new WebServiceRequest());                 }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                Server.Stop();
            }

            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }
    }
}
