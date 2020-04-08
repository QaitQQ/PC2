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

        // правила обработки
        // несинхронный цикл проверки почты
        // правила сравнения
        private static async Task Main(string[] arg)
        {
            Cash = new CashClass();
            Cash.LoadCash();

            if (arg.Contains("-Server"))
            {
                GC.SuppressFinalize(Cash);
                await StartServers();
            }
            if (arg.Contains("-Client"))
            {
                StartClient();
            }
            GC.Collect();
            if (Server != null)
            {
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
    }
}
