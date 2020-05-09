using Microsoft.Extensions.Logging;

using Server.Class.Net.NetServer;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public delegate string Meseger(string Message);
    internal static class Program
    {
        public static CashClass Cash;
        private static Task Server;

        public static  event Action<string> Log;

        // правила обработки
        // несинхронный цикл проверки почты
        // правила сравнения
        private static async Task Main(string[] arg)
        {
            Log += new Action<string>(CW);
            Cash = new CashClass();
            Cash.LoadCash();
            if (arg.Contains("-Server"))
            {
                Log("Server Start"+ DateTime.Now);
                GC.SuppressFinalize(Cash);
                await StartServers();
            }
            if (arg.Contains("-Client"))
            {
                Log("Client Start" + DateTime.Now);
                StartClient();
            }
            GC.Collect();
            if (Server != null)
            {
                Log("Server Wait" + DateTime.Now);
                Server.Wait();
            }
        }

        private static async Task StartServers()
        {
            Server = new TCPServer().AsyncUp();
            await Task.Factory.StartNew(() => Server);
            Class.Net.Imap_Checker ImapServer = new Class.Net.Imap_Checker();
            await Task.Factory.StartNew(() => ImapServer.Start_Check());
        }

        private static void StartClient()
        {
            Thread Client = new Thread(() => new Client.Main().MainClient());
            Client.SetApartmentState(ApartmentState.STA);
            Client.Start();
        }


        private static void CW(string STR) { System.Console.WriteLine(STR); }
    }
}
