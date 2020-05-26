using Client;

using Network;

namespace Server
{
    public class CommonSwitch
    {
        public TCPMessage Result(TCPMessage Message)
        {

            using ApplicationContext DB = new ApplicationContext();

            if (((INetQwerry)Message.Obj) is Network.Аuthorization.АuthorizationNet)
            {
                var C = Message.Obj.GetType().Name;
                if (Message.Obj.GetType().Name.Contains("SetToken"))
                {
                    Message = ((INetQwerry)Message.Obj).Post(DB);
                    Program.Cash.Tokens.Add((string)Message.Obj);
                }
                else
                {
                    Message = ((INetQwerry)Message.Obj).Post(DB);
                }            
            }
            else if (Program.Cash.Tokens.Contains(Message.Token))
            {
                if (Message.Obj.GetType().Name.Contains("ItemSearch"))
                {
                    Message = ((INetQwerry)Message.Obj).Post(DB,Program.Cash.ItemName);
                }
                else
                {
                    Message = ((INetQwerry)Message.Obj).Post(DB);
                }

             
            }
            return Message;
        }
    }


}





