using ICQ.Bot;
using ICQ.Bot.Args;
using ICQ.Bot.Types.ReplyMarkups;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Server
{
    public delegate string Meseger(string Message);
    internal static class Program
    {
        public static CashClass Cash;
        private static TelegramBotClient Bot;
        private static Task Server;
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
        public static event Action<string> Log;
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
                if (arg.Contains("-Bot"))
                {
                    await System.Threading.Tasks.Task.Factory.StartNew(() => StartBot());
                }
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
        private static void StartBot()
        {
            Bot = new TelegramBotClient("1694098904:AAHJh5Xrj1723wAMWLjRyoeZFYlu7xR9LPI");
            Bot.OnMessage += Bot_OnMessage;
            Bot.OnMessageEdited += Bot_OnMessage;
            Bot.StartReceiving();

            void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
            {

                if (e.Message.Text != "" && !e.Message.Text.Contains("|"))
                {
                    System.Collections.IEnumerable Ot = null;
                    using (ApplicationContext DB = new ApplicationContext())
                    {
                        Network.Item.ItemSearch QW = new Network.Item.ItemSearch
                        {
                            Attach = new object[] { e.Message.Text, 3 }
                        };
                        Ot = (System.Collections.IEnumerable)(QW.Post(DB, Program.Cash).Obj);
                    }

                    ReplyKeyboardMarkup keyboard;

                    List<List<KeyboardButton>> KeyLst = new List<List<KeyboardButton>>();
                    if (Ot != null)
                    {
                        foreach (object item in Ot)
                        {
                            string Otv = ((StructLibs.СomparisonNameID)item).Id + "|" + ((StructLibs.СomparisonNameID)item).Name;

                            KeyLst.Add(new List<KeyboardButton> { new KeyboardButton { Text = Otv } });

                        }
                    }

                    keyboard = new ReplyKeyboardMarkup { Keyboard = KeyLst };
                    Bot.SendTextMessageAsync(e.Message.Chat.Id, "Hi", replyMarkup: keyboard);
                }
                else if (e.Message.Text.Contains("|"))
                {
                    StructLibs.ItemPlusImageAndStorege Qt;
                    using (ApplicationContext DB = new ApplicationContext())
                    {
                        Network.Item.GetItemFromId QW = new Network.Item.GetItemFromId
                        {
                            Attach = e.Message.Text.Split('|')[0]
                        };
                        Qt = (StructLibs.ItemPlusImageAndStorege)(QW.Post(DB, Program.Cash).Obj);
                    }

                    string Storege = null;

                    if (Qt.Storages != null)
                    {
                        foreach (StructLibs.Storage item in Qt.Storages)
                        {
                            Storege = Storege + item.Warehouse.Name + "  " + item.Count + "  " + item.SourceName + "  " + item.DateСhange + "\n";
                        }
                    }

                    Bot.SendTextMessageAsync(e.Message.Chat.Id, Qt.Item.Name + " " + Qt.Item.PriceRC + "\n" + Storege);
                }

            }

            IICQBotClient bot = new ICQBotClient("001.3891216990.3031562668:758574936");
            bot.OnMessage += BotOnMessageReceived;
            bot.OnCallbackQuery += BotOnCallbackQuery;


            var me = bot.GetMeAsync().Result;
            bot.StartReceiving();



             void BotOnMessageReceived(object sender, MessageEventArgs e)
            {
                var message = e.Message;

                if (e.Message.Text != "" && !e.Message.Text.Contains("|"))
                {
                    System.Collections.IEnumerable Ot = null;
                    using (ApplicationContext DB = new ApplicationContext())
                    {
                        Network.Item.ItemSearch QW = new Network.Item.ItemSearch
                        {
                            Attach = new object[] { e.Message.Text, 3 }
                        };
                        Ot = (System.Collections.IEnumerable)(QW.Post(DB, Program.Cash).Obj);
                    }
                    List<List<ICQ.Bot.Types.ReplyMarkups.InlineKeyboardButton>> Lst = new List<List<ICQ.Bot.Types.ReplyMarkups.InlineKeyboardButton>>();

                    foreach (var item in Ot)
                    {
                        Lst.Add(new List<ICQ.Bot.Types.ReplyMarkups.InlineKeyboardButton> { new ICQ.Bot.Types.ReplyMarkups.InlineKeyboardButton { Text = ((StructLibs.СomparisonNameID)item).Name, CallbackData = ((StructLibs.СomparisonNameID)item).Id.ToString() } });
                    }

                    ICQ.Bot.Types.ReplyMarkups.InlineKeyboardMarkup markup = new ICQ.Bot.Types.ReplyMarkups.InlineKeyboardMarkup(Lst);



                    bot.SendTextMessageAsync(message.From.UserId, message.Text, replyMarkup: markup).Wait();


                }
                else if (e.Message.Text.Contains("|"))
                {
                }
                
            }


            void BotOnCallbackQuery(object sender, CallbackQueryEventArgs e)
            {
                StructLibs.ItemPlusImageAndStorege Qt;
                using (ApplicationContext DB = new ApplicationContext())
                {
                    Network.Item.GetItemFromId QW = new Network.Item.GetItemFromId
                    {
                        Attach = e.CallbackData
                    };
                    Qt = (StructLibs.ItemPlusImageAndStorege)(QW.Post(DB, Program.Cash).Obj);
                }

                string Storege = null;

                if (Qt.Storages != null)
                {
                    foreach (StructLibs.Storage item in Qt.Storages)
                    {
                        Storege = Storege + item.Warehouse.Name + "  " + item.Count + "  " + item.SourceName + "  " + item.DateСhange + "\n";
                    }
                }
                bot.SendTextMessageAsync(e.From.UserId, Qt.Item.Name + " " + Qt.Item.PriceRC + "\n" + Storege).Wait();
            }
        }
    }
}
