using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace Network
{
    [System.Serializable]
    public class TCPMessage
    {
        public object[] Code { get; set; }
        public object Obj { get; set; }
        public string Token { get; set; }
    }

    public interface INetClient { public TCPMessage Messaging(TCPMessage Data); }

    public abstract class TCPClass : IDisposable
    {
        protected bool ConnectedStatus;
        protected byte[] Buffer;
        protected TCPMessage Data;
        protected string Token;
        protected int Port;
        protected TcpClient Client;
        protected Func<TCPMessage, TCPMessage> Method;
        private void BinToObj(byte[] buffer)
        {
            Data = null;
            using MemoryStream ms = new MemoryStream(buffer, false);
            Data = new BinaryFormatter().Deserialize(ms) as TCPMessage;
        }
        private byte[] ObjToBin(object Message)
        {
            using MemoryStream ms = new MemoryStream();
            new BinaryFormatter().Serialize(ms, Message);
            return ms.ToArray();
        }
        protected void Receive()
        {
            Buffer = new byte[1048576];
            Client.Client.Receive(Buffer, Buffer.Length, SocketFlags.None);
            BinToObj(Buffer);
        }
        protected void Send()
        {
            Buffer = new byte[1048576];
            Buffer = ObjToBin(Data);
            Client.Client.BeginSend(Buffer, 0, Buffer.Length, SocketFlags.None, null, null);
        }

        public void Dispose()
        {
            Client.Close();
            Method = null;
            Data = null;
            this.Buffer = null;
            GC.SuppressFinalize(this);
        }
    }
    public class ServerTCP : TCPClass
    {
        private readonly TcpListener Listner;
        public ServerTCP(int Port, Func<TCPMessage, TCPMessage> Method)
        {
            Client = new TcpClient();
            this.Method = Method;
            this.Port = Port;
            Listner = new TcpListener(new IPEndPoint(IPAddress.Any, Port));
            Listner.Start();
        }
        public async Task AsyncUp()
        {
            while (true)
            {
                this.Client = await Task.Run(() => Listner.AcceptTcpClientAsync());
                await Task.Factory.StartNew(() => Messaging());
            }
        }
        private void Messaging()
        {
            Receive();
            Data = Method.Invoke(Data);
            Send();
        }
    }
    public class ClientTCP : TCPClass, INetClient
    {
        public ClientTCP(string IP, int Port, string Token)
        {
            Client = new TcpClient();
            this.Port = Port;
            var IPEndPoint = new IPEndPoint(IPAddress.Parse(IP), Port);
            this.Token = Token;
            Client.Connect(IPEndPoint);
        }
        public TCPMessage Messaging(TCPMessage Data)
        {
            this.Data = Data;


            if (this.Token != null)
            {
                Data.Token = Token;
            }
            Send();
            Receive();

            Client.Close();
            return this.Data;
        }
    }
}
