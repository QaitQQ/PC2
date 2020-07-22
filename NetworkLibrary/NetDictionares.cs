using Microsoft.EntityFrameworkCore.Infrastructure;

using Object_Description;

using Server;

using System;

namespace Network.Dictionary
{[Serializable]
    public class NetDictionares : NetQwerry { }
    [Serializable]
    public class GetDictionares : NetDictionares
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            var T = ((CashClass)Obj);
            Message.Obj = T.Dictionaries;
            return Message;
        }
    }
    [Serializable]
    public class SetDictionares : NetDictionares
    {

        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            ((CashClass)Obj).Dictionaries = (Dictionaries)Attach;
            Message.Obj = true;


            return Message;
        }
    }

}

