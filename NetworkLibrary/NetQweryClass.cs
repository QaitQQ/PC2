using Server;

using System;

namespace Network
{
    public interface INetQwerry
    {
        public TCPMessage Message { get; set; }
        public T Get<T>(INetClient Client, object Obj = null);
        public TCPMessage Post(ApplicationContext Db, object Obj = null);
        public object Attach { get; set; }
    }
    [Serializable]
    public abstract class NetQwerry : INetQwerry, IDisposable
    {
        public TCPMessage Message { get; set; }
        public object Attach { get; set; }
        public NetQwerry() { Message = new TCPMessage(); }
        public virtual T Get<T>(INetClient Client, object Obj = null)
        {
            Attach = Obj;
            Message.Obj = this;
            Message.Code = new object[] { 2 };
            return (T)Client.Messaging(Message).Obj;
        }
        public virtual TCPMessage Post(ApplicationContext Db, object Obj = null) { throw new NotImplementedException(); }

        public void Dispose()
        {
            this.Attach = null;
            this.Message = null;
        }
    }


}




