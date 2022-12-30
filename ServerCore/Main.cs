using ICQ.Bot;
using ICQ.Bot.Args;
using Server.Class.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Server
{
    public delegate string Meseger(string Message);
    internal static class Program
    {
        public static CashClass Cash;
        //  private static TelegramBotClient Bot;
        private static Task Server;
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
        public static event Action<string> Log;
        public static void Loging(string STR) { Log(STR); }
        private static async Task Main(string[] arg)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            Log += new Action<string>(CW);
            Cash = new CashClass();
            Cash.LoadCash();
            ShowWindow(GetConsoleWindow(), 0);
            if (arg.Contains("-Server"))
            {
                Log("Server Start " + DateTime.Now);
                Cash.ReloadNameCash();
                GC.SuppressFinalize(Cash);
                StartTargeHandler();
                if (arg.Contains("-Bot"))
                {
                    try { await Task.Factory.StartNew(() => StartBot()); }
                    catch { }
                }
                await StartServers(Convert.ToInt32(arg.First(x => x.Contains("Port")).ToString().Split(":")[1]));
                if (!arg.Contains("-Client"))
                {
                    await Task.Factory.StartNew(() => new Ico());
                    Application.Run();
                }
            }
            if (arg.Contains("-Client"))
            {
                try
                {
                    Log("Client Start " + DateTime.Now);
                    StartClient();
                }
                catch (Exception e)
                {
                    Log(e.ToString() + DateTime.Now);
                }
            }
            GC.Collect();
            if (Server != null)
            {
                Log("Server Wait " + DateTime.Now);
                Server.Wait();
            }
        }
        private static async Task StartServers(int Port)
        {
            Server = new Network.ServerTCP(Port, new CommonSwitch().Result).AsyncUp();
            await Task.Factory.StartNew(() => Server);
            //  Class.Net.Imap_Checker ImapServer = new Class.Net.Imap_Checker();
            //  await Task.Factory.StartNew(() => ImapServer.Start_Check());
            TargetDictionary.Dictionarys.Add("Imap_Checker", new Action(() => new Imap_Checker().CheckAndSave()));
            TargetDictionary.Dictionarys.Add("planedPriceWork", new Action(() => Cash.PlanedPriceWork(Cash)));
            TargetDictionary.Dictionarys.Add("UploadStoregeToSite", new Action(() => Cash.UploadStoregeToSite(Cash)));
            //Cash.Targets.Add(new Target("UploadStoregeToSite", Target.Regularity.after_time, PeriodTime: 300));
            // Cash.Targets.Add(new Target("planedPriceWork", Target.Regularity.after_time, PeriodTime: 300));
            //  Cash.Targets.Add(new Target("Imap_Checker", Target.Regularity.after_time, PeriodTime: 300));
            //  Cash.Targets = Cash.Targets;
            //   Cash.Targets.Add(new Target(new Action(() => MessageBox.Show("Вот задание")), Target.Regularity.after_time, PeriodTime: 30));
        }
        private static void StartClient()
        {
            Thread Client = new Thread(() => new Client.Main().MainClient());
            Client.SetApartmentState(ApartmentState.STA);
            Client.Start();
        }
        private static void StartTargeHandler()
        {
            Thread TargeHandler = new Thread(() => _startTargeHandler());
            TargeHandler.Start();
        }
        private static void CW(string STR) { System.Console.WriteLine(STR); }
        private static void StartBot()
        {
            //Bot = new TelegramBotClient("1694098904:AAHJh5Xrj1723wAMWLjRyoeZFYlu7xR9LPI");
            //Bot. += Bot_OnMessage;
            //Bot.OnMakingApiRequest += Bot_OnMessage;
            //Bot.StartReceiving();
            //void Bot_OnMessage(object sender, Telegram.Bot.Args.ApiRequestEventArgs e)
            //{
            //    if (e.Message.Text != null && e.Request.ToString().Contains("!"))
            //    {
            //        if (e.Message.Text != "" && !e.Message.Text.Contains("|"))
            //        {
            //            System.Collections.IEnumerable Ot = null;
            //            using (ApplicationContext DB = new ApplicationContext())
            //            {
            //                Network.Item.ItemSearch QW = new Network.Item.ItemSearch
            //                {
            //                    Attach = new object[] { e.Message.Text, 3 }
            //                };
            //                Ot = (System.Collections.IEnumerable)(QW.Post(DB, Program.Cash).Obj);
            //            }
            //            ReplyKeyboardMarkup keyboard;
            //            List<List<KeyboardButton>> KeyLst = new List<List<KeyboardButton>>();
            //            if (Ot != null)
            //            {
            //                foreach (object item in Ot)
            //                {
            //                    string Otv = ((StructLibs.СomparisonNameID)item).Id + "|" + ((StructLibs.СomparisonNameID)item).Name;
            //                    KeyLst.Add(new List<KeyboardButton> { new KeyboardButton { Text = Otv } });
            //                }
            //            }
            //            keyboard = new ReplyKeyboardMarkup { Keyboard = KeyLst };
            //            var X = Bot.SendTextMessageAsync(e.Message.Chat.Id, "Search...", replyMarkup: keyboard).Result.MessageId;
            //        }
            //        else if (e.Message.Text.Contains("|"))
            //        {
            //            StructLibs.ItemPlusImageAndStorege Qt;
            //            using (ApplicationContext DB = new ApplicationContext())
            //            {
            //                Network.Item.GetItemFromId QW = new Network.Item.GetItemFromId
            //                {
            //                    Attach = e.Message.Text.Split('|')[0]
            //                };
            //                Qt = (StructLibs.ItemPlusImageAndStorege)(QW.Post(DB, Program.Cash).Obj);
            //            }
            //            string Storege = null;
            //            if (Qt.Storages != null)
            //            {
            //                foreach (StructLibs.Storage item in Qt.Storages)
            //                {
            //                    Storege = Storege + item.Warehouse.Name + "  " + item.Count + "  " + item.SourceName + "  " + item.DateСhange + "\n";
            //                }
            //            }
            //            Bot.SendTextMessageAsync(e.Message.Chat.Id, Qt.Item.Name + " " + Qt.Item.PriceRC + "\n" + Storege);
            //        }
            //        {
            //            Bot.DeleteMessageAsync(e.Message.Chat.Id, e.Message.ForwardFromMessageId);
            //        }
            //    }
            //}
            IICQBotClient bot = new ICQBotClient("001.3891216990.3031562668:758574936");
            bot.OnMessage += BotOnMessageReceived;
            bot.OnCallbackQuery += BotOnCallbackQuery;
            try
            {
                ICQ.Bot.Types.User me = bot.GetMeAsync().Result;
                bot.StartReceiving();
            }
            catch
            {
            }
            void BotOnMessageReceived(object sender, MessageEventArgs e)
            {
                ICQ.Bot.Types.Message message = e.Message;
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
                    foreach (object item in Ot)
                    {
                        Lst.Add(new List<ICQ.Bot.Types.ReplyMarkups.InlineKeyboardButton> { new ICQ.Bot.Types.ReplyMarkups.InlineKeyboardButton { Text = ((StructLibs.СomparisonNameID)item).Name, CallbackData = ((StructLibs.СomparisonNameID)item).Id.ToString() } });
                    }
                    ICQ.Bot.Types.ReplyMarkups.InlineKeyboardMarkup markup = new ICQ.Bot.Types.ReplyMarkups.InlineKeyboardMarkup(Lst);
                    if (markup.InlineKeyboard.Count() < 10)
                    {
                        bot.SendTextMessageAsync(message.From.UserId, message.Text, replyMarkup: markup).Wait();
                    }
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
        private static void _startTargeHandler()
        {
            while (true)
            {
                Thread.Sleep(20000);
                for (int i = 0; i < Cash.Targets.Count; i++)
                {
                    Target target = Cash.Targets[i];
                    target.TargetCheck();
                    if (target.Done && (target.Regularity == Target.RegularityType.once || target.Regularity == Target.RegularityType.by_time_once))
                    { Cash.Targets.Remove(target); }
                }
            }
        }
    }
}
