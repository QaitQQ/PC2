using Server;
using StructLibs;
using System;
using System.Linq;

namespace Network.Other
{
    [Serializable]
    public class NetOther : NetQwerry { }


    [Serializable]
    public class GetManufactors : NetOther
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {

            Message.Obj = Db.Manufactor.ToList();

            return Message;
        }
    }
    [Serializable]
    public class ManufGenerator : NetOther
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            System.Collections.Generic.IEnumerable<Object_Description.IDictionaryPC> dic = ((CashClass)Obj).Dictionaries.GetDictionaryRelate(Object_Description.DictionaryRelate.Manufactor);
            foreach (Object_Description.IDictionaryPC item in dic) { Db.Manufactor.Add(new StructLibs.Manufactor() { Id = item.Id, Name = item.Name }); }
            Db.SaveChanges();
            Message.Obj = true;
            return Message;
        }
    }
    [Serializable]
    public class GetManufSite : NetOther
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            if (Attach != null)
            {
                ManufactorSite x = Db.ManufactorSite.First(x => x.ManufactorId == (int)Attach);
                Message.Obj = x;
            }
            else
            {
                Message.Obj = Db.ManufactorSite.ToList();
            }

            return Message;
        }
    }
    [Serializable]
    public class NewManufSite : NetOther
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            Db.Add(((ManufactorSite)Attach));

            Db.SaveChanges();
            Message.Obj = true;
            return Message;
        }
    }

    [Serializable]
    public class FindManuf : NetOther
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {

            var Item = (ItemDBStruct)Attach;
            var Dic = ((CashClass)Obj).Dictionaries.RetunUnderRelate(Object_Description.DictionaryRelate.Manufactor);

            string SerchSTR = Item.Name;

            foreach (var item in Dic)
            {
                if (item.Values.Count > 0)
                {
                    for (int i = 0; i < item.Values.Count; i++)
                    {
                        if (SerchSTR.Contains(item.Values[i]))
                        {
                            Item.ManufactorID = item.Id;
                        }
                    }
                }
            }

            Db.Update(Item);
            Db.SaveChanges();
            Message.Obj = true;
            return Message;
        }
    }


}


