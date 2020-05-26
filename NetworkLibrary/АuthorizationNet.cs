
using Server;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Network.Аuthorization
{

    [Serializable]
    public abstract class АuthorizationNet : NetQwerry  {}

    [Serializable]
    public class SetToken : АuthorizationNet
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            string Token = null;

            object[] NamePass = (object[])this.Attach;

            string Name = NamePass[0].ToString();
            string Pass = NamePass[1].ToString();
            string QweryResult = null;

            List<string> Qwery = (from User in Db.User where User.Name == Name select User.Pass).ToList();
            QweryResult = Qwery[0];


            if (QweryResult == Pass)
            {
                Random rnd = new Random();
                Token = null;

                for (int i = 0; i < 12; i++)
                {
                    int value = rnd.Next(50, 114);
                    Token += (char)value;
                }
            }
            Message.Obj = Token;

            return Message;
        }
    }
    [Serializable]
    public class GetUserList : АuthorizationNet
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            Obj = (from User in Db.User select User.Name).ToList();
            Message.Obj = Obj;
            Message.Code = new Object[] { 1 };
            return Message;
        }
    }

}





