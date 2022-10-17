using CRMLibs;
using Server;
using StructLibs;

using System;
using System.Collections.Generic;
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
    [System.Serializable]
    public class GetStorage : NetCRM
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            Message.Obj = Db.Warehouse.ToList();
            return Message;
        }
    }
    [System.Serializable]
    public class RemoveStorage : NetCRM
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            Warehouse war = (Warehouse)Attach;
            var str = Db.Storage.Where(x => x.WarehouseID == war.Id);
            foreach (var item in str)
            {
                Db.Storage.Remove(item);
            }
           var TWAR = Db.Warehouse.First(X => X.Id == war.Id);
            Db.Warehouse.Remove(TWAR);
            Db.SaveChanges();
            Message.Obj =true;
            return Message;
        }
    }
    [System.Serializable]
    public class GetEventFromDateОccurred : NetCRM
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            var att = (KeyValuePair<int, DateTime>)Attach;
            Message.Obj = Db.Events.Where(item => item.DateОccurred.Year == att.Value.Year && item.DateОccurred.Month == att.Value.Month && item.DateОccurred.Day == att.Value.Day && item.UserID == att.Key).ToList();
            return Message;

        }
    }
    [System.Serializable]
    public class GetEventFromDateDatePlanned : NetCRM
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            var att = (KeyValuePair<int, DateTime>)Attach;
            Message.Obj = Db.Events.Where(item => item.DatePlanned.Year == att.Value.Year && item.DatePlanned.Month == att.Value.Month && item.DatePlanned.Day == att.Value.Day && item.UserID == att.Key).ToList();
            return Message;

        }
    }
}
