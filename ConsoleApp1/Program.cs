using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConsoleApp1
{
    class Program
    {
        static readonly HttpClient client = new HttpClient(); // client default port = 80
        static HttpListener _httpListener = new HttpListener(); // server

        static string address = "http://63b014fa.ngrok.io"; // adres
        static void createServer()
        {
            Console.WriteLine("Starting server...");
            _httpListener.Prefixes.Add(address + "/start/");
            _httpListener.Prefixes.Add(address + "/time/");
            _httpListener.Start(); // start server (Run application as Administrator!)
            Console.WriteLine("Server started. Press any key...");
            Console.ReadKey();
        }
        static void sendVal(string strResultJson)
        {

            Console.WriteLine("Sending values ... : " + strResultJson);
            client.PutAsync("http://9baa3a2f.ngrok.io/to", new StringContent(strResultJson)); // 5 ok
            Console.WriteLine("Sent to 5");
            client.PutAsync("http://0bac3af1.ngrok.io/dostawca", new StringContent(strResultJson, Encoding.UTF8, "application/json")); // 4    ok 
            Console.WriteLine("Sent to 4");
            client.PutAsync("http://97517dbf.ngrok.io/to", new StringContent(strResultJson)); // 6.1    ok 
            Console.WriteLine("Sent to 6.1");
            client.PutAsync("http://90fb7f89.ngrok.io/to", new StringContent(strResultJson)); // 6.2
            Console.WriteLine("Sent to 6.2");
            client.PutAsync("http://e1674b72.ngrok.io/to", new StringContent(strResultJson, Encoding.UTF8, "application/json")); // 6.3   ok
            Console.WriteLine("Sent to 6.3");
            client.PostAsync("http://4bcc0e78.ngrok.io/3", new StringContent(strResultJson, Encoding.UTF8, "application/json")); // 7
            Console.WriteLine("Sent to 7");
        }
        static string receive()
        {
            HttpListenerContext context = _httpListener.GetContext();
            var request = context.Request;
            string text;
            using (var reader = new StreamReader(request.InputStream,
                                                 request.ContentEncoding))
            {
                text = reader.ReadToEnd();
            }
            return text;
        }

        static bool checkBreakdown(bool breakdown)
        {
            if ((Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.A))
            {
                if (!breakdown)
                {
                    Console.WriteLine("CAUTION ! BREAKDOWN !");
                    return true;
                }
                else
                {
                    Console.WriteLine(" BREAKDOWN STOPPED ");
                    return false;
                }
            }
            return breakdown;
        }

        static async Task<int> Main(string[] args)
        {
            createServer();
            string responseBody = receive();
            Console.WriteLine("Odebrane dane : " + responseBody);
            
            DataOut info = new DataOut();
            Function func = new Function();
            DataIn _speed = new DataIn();

            string strResultJson;
            int secondsWaiting = 0;
            int hours = 0;
            bool breakdown = false;
            
            _speed = JsonConvert.DeserializeObject<DataIn>(responseBody);

            Console.WriteLine("Press \"a\" to simulate breakdown");

            while (true)
            {
                info = func.createDataOut(breakdown, _speed);

                if (hours++ > 8)
                {
                    func.weatherTendencyChange();
                    hours = 0;
                }
                strResultJson = JsonConvert.SerializeObject(info);
                sendVal(strResultJson);


                while (secondsWaiting < 3600)
                {
                    breakdown = checkBreakdown(breakdown);
                    responseBody = receive();
                    _speed = JsonConvert.DeserializeObject<DataIn>(responseBody);
                    Console.WriteLine(_speed.speed);
                    secondsWaiting += 30*_speed.speed;
                }
                secondsWaiting = 0;

            }   
        }
    }
}
