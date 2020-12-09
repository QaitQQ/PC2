using CRMLibs;

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
    public class PartnersSearch : NetCRM
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            Message.Obj = Db.Partner.Where(x => x.Name.Contains((string)Attach)).ToList() ;
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
    [System.Serializable]
    public class AddEvent : NetCRM
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            Db.Add((Event)Attach);
            Db.SaveChanges();
            Message.Obj = true;

            return Message;
        }
    }
    [System.Serializable]
    public class DelEvent : NetCRM
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            Db.Remove((Event)Attach);
            Db.SaveChanges();
            Message.Obj = true;
            return Message;
        }
    }
    [System.Serializable]
    public class AddPartner : NetCRM
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            Db.Add((Partner)Attach);
            Db.SaveChanges();
            Message.Obj = true;
            return Message;
        }
    }
    [System.Serializable]
    public class DelPartner : NetCRM
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {

            Partner partner = (Partner)Attach;

            Db.Remove(partner);

            System.Collections.Generic.List<Event> events = Db.Events.Where(x => x.PartnerID == partner.Id).ToList();

            foreach (var item in events)
            {
                Db.Remove(item);
            }

            Db.SaveChanges();
            Message.Obj = true;
            return Message;

        }
    }
}




