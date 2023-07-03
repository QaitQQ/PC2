
using Object_Description;

using Server;
using Server.Class.IntegrationSiteApi;

using Spire.Pdf.Exporting.XPS.Schema;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Network.Dictionary
{
    [Serializable]
    public class NetDictionares : NetQwerry { }
    [Serializable]
    public class GetDictionares : NetDictionares
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            CashClass T = ((CashClass)Obj);
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
    [Serializable]
    public class SiteSync : NetDictionares
    {

        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            CashClass cash = ((CashClass)Obj);

            System.Collections.Generic.IEnumerable<IDictionaryPC> Manuf =   cash.Dictionaries.GetDictionaryRelate(DictionaryRelate.Manufactor);
            //Производители

            StructSite.Manufactur[] X = new StructSite(cash.ApiSiteSettings[0]).ManufactorsId();
            System.Collections.Generic.IEnumerable<IDictionaryPC> result = Manuf.Where(n => !X.Any(t => Convert.ToInt32(t.Id) == n.Id));
            System.Collections.Generic.IEnumerable<StructSite.Manufactur> result2 = X.Where(n => !Manuf.Any(t => t.Id == Convert.ToInt32(n.Id)));
            if (result2.Count()>0)
            {
                foreach (StructSite.Manufactur item in result2)
                {
                    cash.Dictionaries.Add(new DictionaryBase(item.Name, DictionaryRelate.Manufactor) { Id = Convert.ToInt32(item.Id) });
                }
            }

            //Атрибуты
            System.Collections.Generic.IEnumerable<IDictionaryPC> Attribute = cash.Dictionaries.GetDictionaryRelate(DictionaryRelate.Attribute);
            StructSite.attribute_description[] attribute_s = new Server.Class.IntegrationSiteApi.StructSite("https://salessab.su" + "/index.php?route=api").GetTable<Server.Class.IntegrationSiteApi.StructSite.attribute_description>("attribute_description");
            System.Collections.Generic.IEnumerable<IDictionaryPC> result3 = Attribute.Where(n => !attribute_s.Any(t => Convert.ToInt32(t.attribute_id) == n.Id));
            System.Collections.Generic.IEnumerable<StructSite.attribute_description> result4 = attribute_s.Where(n => !Attribute.Any(t => t.Id == Convert.ToInt32(n.attribute_id)));
            if (result4.Count() > 0)
            {
                foreach (StructSite.attribute_description item in result4)
                {
                    cash.Dictionaries.Add(new DictionaryBase(item.name, DictionaryRelate.Attribute) { Id = Convert.ToInt32(item.attribute_id) });
                }
            }

            // Категории



            System.Collections.Generic.IEnumerable<IDictionaryPC> Categorys = cash.Dictionaries.GetDictionaryRelate(DictionaryRelate.Category);

            var Catg = new List<IDictionaryPC>();
            
            foreach (var item in Categorys)
            {
                Catg.Add(item);
            }

            //foreach (var item in Catg)
            //{
            //    cash.Dictionaries.Del(item);
            //}




            Catg = new List<IDictionaryPC>();
            Categorys = cash.Dictionaries.GetDictionaryRelate(DictionaryRelate.Category);


            foreach (var item in Categorys)
            {
                Catg.Add(item);
            }

            StructSite.Cat[] Categorys_s = new Server.Class.IntegrationSiteApi.StructSite("https://salessab.su" + "/index.php?route=api").GetTable<Server.Class.IntegrationSiteApi.StructSite.Cat>("Categorys");
            System.Collections.Generic.IEnumerable<IDictionaryPC> result5 = Categorys.Where(n => !Categorys_s.Any(t => Convert.ToInt32(t.Id) == n.Id));

            List<StructSite.Cat> Result = new List<StructSite.Cat>();

            foreach (var item in Categorys_s)
            {
                if (!Categorys.Contains(Categorys.FirstOrDefault(x=>x.Id == Convert.ToInt32(item.Id))))
                {
                    cash.Dictionaries.Add(new DictionarySiteCategory(item.Name, DictionaryRelate.Category) { Id = Convert.ToInt32(item.Id), Parent_id = Convert.ToInt32(item.Parent_id) });
                }
            }

            Catg = new List<IDictionaryPC>();
            Categorys = cash.Dictionaries.GetDictionaryRelate(DictionaryRelate.Category);


            foreach (var item in Categorys)
            {
                Catg.Add(item);
            }



            //System.Collections.Generic.IEnumerable<StructSite.Cat> result6 = Categorys_s.Where(n => !Categorys.Any(t => t.Id == Convert.ToInt32(n.Id)));
            //if (result6.Count() > 0)
            //{
            //    foreach (StructSite.Cat item in result6)
            //    {
            //        cash.Dictionaries.Add(new DictionarySiteCategory(item.Name, DictionaryRelate.Category) { Id = Convert.ToInt32(item.Id), Parent_id = Convert.ToInt32(item.Parent_id) });
            //    }
            //}

            cash.Dictionaries = cash.Dictionaries;
            Message.Obj = true;
            return Message;



        }
    }
    [Serializable]
    public class GetDictionaresByRelate : NetDictionares
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            CashClass T = ((CashClass)Obj);

            DictionaryRelate Relate = (DictionaryRelate)this.Attach;




           var X = T.Dictionaries.GetDictionaryRelate(Relate);


             Dictionaries _dictionaries = new Dictionaries();

            foreach (var item in X)
            {
                _dictionaries.Add(item);
            }

            Message.Obj = _dictionaries;

            return Message;
        }
    }
}

