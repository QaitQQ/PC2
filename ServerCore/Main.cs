
using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    public class Ico
    {
        private void Close(object sender, MouseEventArgs e) { System.Environment.Exit(0); }
        public Ico()
        {
            NotifyIcon icon = new NotifyIcon();
            icon.MouseDown += new MouseEventHandler(Close);
            icon.Icon = new Icon("134.ico");
            icon.Visible = true;


        }


    }

    public delegate string Meseger(string Message);
    internal static class Program
    {
        public static CashClass Cash;
        private static Task Server;

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        public static event Action<string> Log;

        // правила обработки
        // несинхронный цикл проверки почты
        // правила сравнения
        private static async Task Main(string[] arg)
        {
            Log += new Action<string>(CW);
            Cash = new CashClass();
            Cash.LoadCash();

            ShowWindow(GetConsoleWindow(), 0);


            if (arg.Contains("-Server"))
            {
                Log("Server Start " + DateTime.Now);
                GC.SuppressFinalize(Cash);
               

                if (!arg.Contains("-Client"))
                {
                    await System.Threading.Tasks.Task.Factory.StartNew(() => new Ico());
                    Application.Run();
                }


                
                await StartServers();
              


            }
            if (arg.Contains("-Client"))
            {
                Log("Client Start " + DateTime.Now);
                StartClient();
            }
            GC.Collect();

            if (Server != null)
            {
                Log("Server Wait " + DateTime.Now);
                Server.Wait();
            }


        }

        private static async Task StartServers()
        {
            Server = new Network.ServerTCP(12001, new CommonSwitch().Result).AsyncUp();
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
