using Server;
using Server.Class.PriceProcessing;

using StructLibs;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Network.PriceService
{
    [System.Serializable]
    public class PriceService : NetQwerry { }
    [System.Serializable]
    public class GetPriceStorege : NetQwerry
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            Message.Obj = ((CashClass)Obj).PriceStorageList;
            return Message;
        }
    }

    [System.Serializable]
    public class SavePriceStorege : NetQwerry
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            var PriceStorage = (PriceStorage)Attach;

            var PriceStorageList = ((CashClass)Obj).PriceStorageList;

            for (int i = 0; i < PriceStorageList.Count; i++)
            {
                if (PriceStorageList[i].Name == PriceStorage.Name)
                {
                    PriceStorageList[i] = PriceStorage;
                }
            }
            ((CashClass)Obj).PriceStorageList = PriceStorageList;


            Message.Obj = true;
            return Message;
        }
    }
    [System.Serializable]
    public class ReadPrice : NetQwerry
    {   [NonSerialized]
        List<ItemDBStruct> DB_list;
        [NonSerialized]
        CashClass Cash;
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            var StoregeInfo = ((PriceStorage)Attach);
            if (StoregeInfo.FilePath != null && File.Exists(StoregeInfo.FilePath))
            {
                var fs = File.OpenRead(StoregeInfo.FilePath);
                string Attb = string.Join(",", StoregeInfo.Attributes);
                DB_list = Db.Item.ToList();
                Cash = ((CashClass)Obj);

                var lst = new PriceProcessingRules(fs, StoregeInfo.Name, Attb, Cash);
                lst.СhangeResult += Comparer;
                lst.Apply_rules();
                Message.Obj = "Чтение";
            }
            else
            {
                Message.Obj = "Файла не существует";
            }


            return Message;
        }
        private void Comparer(object Lst)
        {
            var cmpr = new Сompare_NewPrice_with_DB((List<ItemPlusImageAndStorege>)Lst, DB_list, Cash);
            cmpr.StartCompare();
        }

    }
}
