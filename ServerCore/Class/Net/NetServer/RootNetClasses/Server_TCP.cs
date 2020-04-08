using Server.Class.Base;

using StructLibs;

using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace Server.Class.Net.NetServer
{
    internal class TCPServer
    {
        private readonly TcpListener Listner = new TcpListener(new IPEndPoint(IPAddress.Any, Settings.NetPort));

        private TcpClient Client;
        public TCPServer() => Listner.Start();
        public async Task AsyncUp()
        {
            while (true)
            {
                Client = await Task.Run(() => Listner.AcceptTcpClientAsync());
                await Task.Factory.StartNew(() => Messaging());
            }
        }

        private void Messaging()
        {
            byte[] buffer = new byte[1048576];
            // клиент при входящем соединении
            Client.Client.Receive(buffer, SocketFlags.None);  // засовываем в буфер входящее сообщение
            TCP_CS_Obj Data = Bin_To_TCP_CS(buffer);
            Data = new Server_Switch(Data).DoSwitch();
            buffer = new byte[1048576];
            buffer = TCP_CS_To_Bin(Data);
            Client.Client.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, null, null);
        }
        private TCP_CS_Obj Bin_To_TCP_CS(byte[] buffer)
        {
            TCP_CS_Obj Data = null;
            using MemoryStream ms = new MemoryStream(buffer, false); Data = new BinaryFormatter().Deserialize(ms) as TCP_CS_Obj;
            return Data;
        }
        private byte[] TCP_CS_To_Bin(TCP_CS_Obj Message)
        {
            using MemoryStream ms = new MemoryStream();
            new BinaryFormatter().Serialize(ms, Message);
            return ms.ToArray();
        }
    }
}
