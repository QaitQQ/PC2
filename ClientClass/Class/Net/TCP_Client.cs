using StructLibs;

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

using WindowsFormsClientLibrary.Class;
//Main.Config.Find(x=>x.Key == "Server_IP").Value
//"127.0.0.1"
namespace Client
{
    internal class TCP_Client
    {

        private bool ConnectedStatus;
        public bool GetConnectedStatus() { Send(new object[] { 1, 0 }); return ConnectedStatus; }
        public TCP_CS_Obj Data { get; set; }

        private readonly TcpClient client = new TcpClient();
        private byte[] buffer = new byte[1048576];
        private readonly IPEndPoint IPEndPoint;
        private readonly string Token;
        public TCP_Client(object[] Code, string Token = null, object SendObj = null)
        {
            string addres = ClientConfig.IP_and_Port_Server.Find(x => x.Key == "Server_IP").Value;
            string port = ClientConfig.IP_and_Port_Server.Find(x => x.Key == "Port").Value;
            if (addres == null)
            {
                addres = "127.0.0.1";
                port = "12001";
            }

            IPEndPoint = new IPEndPoint(IPAddress.Parse(addres), Convert.ToInt32(port));
            Data = new TCP_CS_Obj();
            this.Token = Token;
            client.Connect(IPEndPoint);
            Send(Code, SendObj);
            Accept();
            Down();
        }

        private void Send(object[] Code, object SendObj = null)
        {
            Data.Code = Code;
            Data.Obj = SendObj;
            Data.Token = Token;
            TCP_CS_To_Bin(Data);
            client.Client.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, null, null);

        }
        private void Accept()
        {
            buffer = new byte[1048576];
            client.Client.Receive(buffer, buffer.Length, SocketFlags.None);
            Bin_To_TCP_CS();
            ConnectedStatus = true;
        }

        private void Down()
        {
            client.Close();
            ConnectedStatus = false;
        }

        private void Bin_To_TCP_CS()
        {
            using MemoryStream ms = new MemoryStream(buffer, false);
            ms.Position = 0; Data = new BinaryFormatter().Deserialize(ms) as TCP_CS_Obj;
        }
        private void TCP_CS_To_Bin(TCP_CS_Obj Message)
        {

            using (MemoryStream ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, Message);
                buffer = ms.ToArray();
            }
        }
    }
}
