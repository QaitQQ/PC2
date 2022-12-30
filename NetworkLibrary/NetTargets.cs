using Network.Other;
using Server;
using StructLibs;
using System;
using System.Linq;

namespace Network.Target
{
    [Serializable]
    public class NetTargets : NetQwerry { }

    [Serializable]
    public class GetTarget : NetOther
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            var Targets = ((CashClass)Obj).Targets;
            Message.Obj = Targets;
            return Message;
        }
    }

}
