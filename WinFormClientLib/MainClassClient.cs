using Client.Forms;
using Server.Class.HDDClass;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Client
{
    public class Main
    {

        public static АuthorizationForm АuthorizationWindow;

        public static List<KeyValuePair<string, string>> Config;

        [STAThread]
        public void MainClient ( )

        {
            Client.Main.Config = new Deserializer<List<KeyValuePair<string, string>>>("ClientConfig.bin").Doit();
            if (Config == null)
            {
                Config = new List<KeyValuePair<string, string>>();
                Config.Add(new KeyValuePair<string, string>("Server_IP", "127.0.0.1"));
                Config.Add(new KeyValuePair<string, string>("Port", "12001"));
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            АuthorizationWindow = new АuthorizationForm();
            Application.Run(АuthorizationWindow);
        }
    }
}
