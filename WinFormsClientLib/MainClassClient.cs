using Client.Forms;

using CRMLibs;

using Network;

using Server.Class.HDDClass;

using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Windows.Forms;

using WindowsFormsClientLibrary.Class;

namespace Client
{

    public static class ActiveValue 
    {
        private static Partner activePartner;
        private static int[] saleValue;
        public static Partner ActivePartner { get => activePartner; set { activePartner = value; ChangedPartner?.Invoke(activePartner); } }

        public static int[] SaleValue { get => saleValue; set { saleValue = value; ChangeSale?.Invoke(saleValue[0], saleValue[1]); } }

        public static event Action<Partner> ChangedPartner;
        public static event Action<int, int> ChangeSale;






    }

    public class WrapNetClient : INetClient
    {
        string IP = Main.Server_IP;
        int Port = Main.Server_Port;
        public TCPMessage Messaging(TCPMessage Data)
        {
            using ClientTCP Client = new ClientTCP(IP, Port, Main.Token);
            
            return Client.Messaging(Data);
        }

    }
    public class Main
    {
        public static Form CommonWindow;
        public static string Token;
        public static string Server_IP;
        public static int Server_Port;

        [STAThread]
        public void MainClient()

        {
            ClientConfig.ConfigValue = new Deserializer<List<KeyValuePair<string, string>>>("ClientConfig.bin").Doit();
            Token = null;
            if (ClientConfig.ConfigValue == null)
            {
                ClientConfig.ConfigValue = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("Server_IP", "127.0.0.1"),
                    new KeyValuePair<string, string>("Port", "12001")
                };
            }

            Server_IP = ClientConfig.GetValue("Server_IP");
            Server_Port = Convert.ToInt32(ClientConfig.GetValue("Port"));


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            CommonWindow = new АuthorizationForm();
            Application.Run(CommonWindow);

        }
    }
}
