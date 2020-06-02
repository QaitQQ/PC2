using Server;

using System.Linq;

namespace Network.CRM
{
    [System.Serializable]
    public class NetCRM : NetQwerry { }


    [System.Serializable]
    public class GetAllPartners : NetCRM
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            Message.Obj = Db.Partner.ToList();
            return Message;
        }
    }
    [System.Serializable]
    public class GetEventsFromPartnerID : NetCRM
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            int ID = (int)Attach;
            Message.Obj = Db.Events.Where(item => item.PartnerID == ID).ToList();
            return Message;
        }
    }
}
