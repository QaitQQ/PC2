using Client.Forms;

using Server.Class.HDDClass;

using System;
using System.Collections.Generic;
using System.Windows.Forms;

using WindowsFormsClientLibrary.Class;

namespace Client
{




    public class Main
    {

        public static АuthorizationForm АuthorizationWindow;



        [STAThread]
        public void MainClient()

        {
            ClientConfig.IP_and_Port_Server = new Deserializer<List<KeyValuePair<string, string>>>("ClientConfig.bin").Doit();

            if (ClientConfig.IP_and_Port_Server == null)
            {
                ClientConfig.IP_and_Port_Server = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("Server_IP", "127.0.0.1"),
                    new KeyValuePair<string, string>("Port", "12001")
                };
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            АuthorizationWindow = new АuthorizationForm();
            Application.Run(АuthorizationWindow);
        }
    }
}
