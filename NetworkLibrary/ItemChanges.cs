using Server;

using StructLibs;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Network.Item.Changes
{
    [Serializable]
    public class GetChanges : NetItem
    {      
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            Message.Obj = ((CashClass)Obj).ChangedItems;

            return Message;
        }
    }
    [Serializable]
    public class GetNewList : NetItem
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            var List = ((CashClass)Obj).NewItem;
            List<StructLibs.ItemChanges> Result = new List<StructLibs.ItemChanges>();
            foreach (var item in List)
            {
                Result.Add(new StructLibs.ItemChanges()
                {
                    ItemName = item.Item.Name,
                    NewValue = item.Item.PriceRC,
                    Source = item.Item.SourceName
                }
                );
            }
            Message.Obj = Result;
            return Message;
        }
    }
    /// <summary>
    /// принять все изменения из накопителя
    /// вернуть список(List<string>) состоящий из имен позиций и ОКев
    /// </summary>
    [Serializable]
    public class AllowAllChanges : NetItem
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            var Changes = ((CashClass)Obj).ChangedItems;

            var Prop = ItemDBStruct.GetProperties();

            var Items = Db.Item.ToList();

            var Result = new List<string>();


            foreach (var Change in Changes)
            {

                var Item = Items.First(x => x.Id == Change.ItemID);

                foreach (System.Reflection.PropertyInfo Pp in Prop)
                {
                    if (Pp.Name == Change.FieldName)
                    {
                        Pp.SetValue(Item, Change.NewValue);
                    }
                }

                Result.Add(Item.Name + " OK");
                Db.Update(Item);
            }

            Db.SaveChanges();


            Message.Obj = Result;
            return Message;
        }
    }
    [Serializable]
    public class DelFromСhangedList : NetItem
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {

            List<StructLibs.ItemChanges> СhangedList = ((CashClass)Obj).ChangedItems;

            СhangedList.RemoveAll(x=> x.ItemID == ((StructLibs.ItemChanges)Attach).ItemID && x.FieldName == ((StructLibs.ItemChanges)Attach).FieldName);

            Message.Obj = true;

            return Message;
        }
    }

}


