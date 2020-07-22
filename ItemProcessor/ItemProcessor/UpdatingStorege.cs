using StructLibs;

using System.Collections.Generic;
using System.Linq;

namespace Server.Class.ItemProcessor
{
    public class UpdatingStorege
    {
      public UpdatingStorege(List<ItemPlusImageAndStorege> storegeUpdateList)
        {

            using ApplicationContext DB = new ApplicationContext();

            var storeges = DB.Storage.ToList();
            var Items = DB.Item.ToList();
            var Warehouses = DB.Warehouse.ToList();

            foreach (var item in storegeUpdateList)
            {
                foreach (var Stor in item.Storages)
                {
                    var Strorege = storeges.FindAll(X => X.ItemID == item.Item.Id && X.Warehouse.Name == Stor.Warehouse.Name);
                    if (Strorege.Count == 1)
                    {
                        Strorege[0].Count = Stor.Count;
                        Strorege[0].DateСhange = Stor.DateСhange;
                        Strorege[0].SourceName = Stor.SourceName;

                        DB.Update(Strorege[0]);
                    }
                    else if (Strorege.Count == 0)
                    {
                        Stor.ItemID = Items.First(x => x.Id == item.Item.Id).Id;

                        var wh = Warehouses.FindAll(X => X.Name == Stor.Warehouse.Name);

                        if (wh.Count== 1)
                        {                           
                            Stor.WarehouseID = wh[0].Id;
                            Stor.Warehouse = null;
                            DB.Storage.Add(Stor);
                        }
                        else if (wh.Count == 0)
                        {
                            DB.Storage.Add(Stor);
                            DB.SaveChanges();
                            Warehouses = DB.Warehouse.ToList();
                        }

                       
                    }
                }

               
            }

            DB.SaveChanges();

            storeges = DB.Storage.ToList();

            foreach (var item in storeges)
            {
                var Item = Items.FirstOrDefault(X => X.Id == item.ItemID);
                if (Item != null)
                {
                    if (Item.StorageID == null)
                    {
                        Item.StorageID = new List<int>();
                    }
                    Item.StorageID.Add(item.ID);
                    DB.Update(Item);
                }
                else
                {
                    DB.Remove(item);
                }
            }
            DB.SaveChanges();

        }
   



    }
}



