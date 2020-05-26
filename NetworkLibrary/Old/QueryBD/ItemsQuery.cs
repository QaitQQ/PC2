using Microsoft.EntityFrameworkCore;

using Pricecona;

using Server.Class.ItemProcessor;

using StructLibs;

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Server.Class.Query
{

    public class ItemsQuery : DB_Abstract<ItemDBStruct>
    {

        public void AddPriceStruct(PriceStruct Item)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                db.Add(new ItemDBStruct(Item));
                db.SaveChanges();
            }
        }
        public void AddListPriceStruct(List<PriceStruct> Item)
        {

            using ApplicationContext db = new ApplicationContext();

            foreach (var item in Item)
            {
                db.Add(new ItemDBStruct(item));
            }

            db.SaveChanges();
        }

        public List<ItemDBStruct> FindWithParameters(string Str, PropertyInfo Field)
        {
            List<ItemDBStruct> QweryResult = null;

            if (Field.PropertyType.Name.Contains("List"))
            {
                using ApplicationContext db = new ApplicationContext();
                QweryResult = db.Item.ToList();               
                QweryResult = QweryResult.FindAll(item => (Field.GetValue(item) as List<string>) != null && (Field.GetValue(item) as List<string>).Contains(Str));
            }
            else
            {
                using ApplicationContext db = new ApplicationContext();
                QweryResult = db.Item.ToList();
                QweryResult = QweryResult.FindAll(item => Field.GetValue(item).ToString().ToUpper().Contains(Str.ToUpper()));
            }


            return QweryResult;
        }
        public override List<ItemDBStruct> Find(string Name)
        {
            List<ItemDBStruct> QweryResult = null;

            using (ApplicationContext db = new ApplicationContext())
            {
                QweryResult = db.Item.Where(item => item.Name == Name).ToList();
            }
            return QweryResult;
        }
        public List<ItemDBStruct> GetAll()
        {
            List<ItemDBStruct> QweryResult = null;

            using (ApplicationContext db = new ApplicationContext())
            {
                QweryResult = db.Item.ToList();
            }
            return QweryResult;
        }
        public List<ItemDBStruct> FindByСomparisonName(string СomparisonName)
        {
            List<ItemDBStruct> QweryResult = null;


            using (ApplicationContext db = new ApplicationContext())
            {
                QweryResult = db.Item.Where(item => item.СomparisonName.Contains(СomparisonName)).ToList();
            }
            return QweryResult;
        }
        public override List<ItemDBStruct> FindID(int ID)
        {
            List<ItemDBStruct> QweryResult = null;
            using (ApplicationContext db = new ApplicationContext())
            {
                QweryResult = db.Item.Where(item => item.Id == ID).ToList();
            }
            return QweryResult;
        }
        //public void FixName()
        //{
        //    using ApplicationContext db = new ApplicationContext();
        //    DbSet<ItemDBStruct> QweryResult = db.Item;

        //    foreach (ItemDBStruct item in QweryResult)
        //    {
        //        ItemDBStruct editItem = item;
        //        Object_Description.IDictionaryPC Dic = Program.Cash.Dictionaries.Get("NameEdit");
        //        editItem.Name = item.Name.Trim().ToUpper();
        //        foreach (string X in Dic.Values) { editItem.Name = item.Name.Replace(X.ToUpper(), ""); }
        //        editItem.СomparisonName = СomparisonNameGenerator.Get(editItem.Name);
        //        db.Update(editItem);
        //    }

        //    db.SaveChanges();

        //}
        public void DoubleDelete()
        {
            using ApplicationContext db = new ApplicationContext();
            List<ItemDBStruct> QweryResult = db.Item.ToList();

            foreach (ItemDBStruct item in QweryResult)
            {
                var Result = QweryResult.FindAll(x => x.СomparisonName == item.СomparisonName);
                if (Result.Count > 1)
                {
                    db.Item.Remove(item);
                }
            }

            db.SaveChanges();

        }
    }

}
