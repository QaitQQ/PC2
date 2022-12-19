using HtmlAgilityPack;
using NPOI.POIFS.Crypt.Dsig;
using Object_Description;
using Server;
using Server.Class.PriceProcessing;
using StructLibs;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
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
    public class RemovePriceStorege : NetQwerry
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            var activeprice = (PriceStorage)Attach;
            var PriceStorageList = ((CashClass)Obj).PriceStorageList;
            if (PriceStorageList.Exists(x => x.Name == activeprice.Name))
            {
                if (activeprice.FilePath!= "" && File.Exists(activeprice.FilePath))
                {
                    File.Delete(activeprice.FilePath);
                }             
                ((CashClass)Obj).PriceStorageList.Remove(((CashClass)Obj).PriceStorageList.First(x => x.Name == activeprice.Name && x.FilePath == activeprice.FilePath));
                ((CashClass)Obj).PriceStorageList = ((CashClass)Obj).PriceStorageList;
            }
           Message.Obj =true;
           return Message;
        }
    }
    [System.Serializable]
    public class DownloadPriceStorege : NetQwerry
    {
        public override  TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            var activeprice = (PriceStorage)Attach;
            var PriceStorageList = ((CashClass)Obj).PriceStorageList;
            string Path = @"price_storage\\";
            if (activeprice.Link.ToLower().Contains("Zip".ToLower()))
            {
                Path = Path + activeprice.Name + ".xlsx";
                Task.Factory.StartNew(() => DownloadPriceZip(activeprice, Path));

            }
            else
            {
                if (activeprice.FilePath == "")
                {
                    Path = Path + activeprice.Name + ".xlsx";
                }
                else
                {
                    Path = activeprice.FilePath;
                }
                Task.Factory.StartNew(() => DownloadPriceXls(activeprice, Path));
            }
            activeprice.FilePath = Path;
            activeprice.ReceivingData = DateTime.Now;
            if (PriceStorageList.Exists(x => x.Name == activeprice.Name))
            {
                for (int i = 0; i < PriceStorageList.Count; i++)
                {
                    if (PriceStorageList[i].Name == activeprice.Name)
                    {
                        PriceStorageList[i] = activeprice;
                    }
                }
            }
            else
            {
                PriceStorageList.Add(activeprice);
            }
             ((CashClass)Obj).PriceStorageList = PriceStorageList;
            Message.Obj = true;
            return Message;
        }
        private static void DownloadPriceXls(PriceStorage activeprice, string Path)
        {
           WebClient client = new WebClient();
           client.DownloadFileAsync(new Uri(activeprice.Link), activeprice.FilePath);         
        }
        private static void DownloadPriceZip(PriceStorage activeprice, string Path)
        {
            WebClient client = new WebClient();
            byte[] Mass = client.DownloadData(new Uri(activeprice.Link));
            Stream stream = new MemoryStream(Mass);
            ZipArchive archive = new ZipArchive(stream);
            foreach (ZipArchiveEntry entry in archive.Entries)
            {
                if (entry.FullName.ToLower().Contains(".xls"))
                {
                    entry.ExtractToFile(Path);
                    
                    break;
                }
            }
        }
    }
    [System.Serializable]
    public class SavePriceStorege : NetQwerry
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            var PriceStorage = (PriceStorage)Attach;
            var PriceStorageList = ((CashClass)Obj).PriceStorageList;
            if (PriceStorageList.Exists(x=>x.Name == PriceStorage.Name))
            {
                for (int i = 0; i < PriceStorageList.Count; i++)
                {
                    if (PriceStorageList[i].Name == PriceStorage.Name)
                    {
                        PriceStorageList[i] = PriceStorage;
                    }
                }
            }
            else 
            {
                PriceStorageList.Add(PriceStorage);
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
            var StoregeInfo = (PriceStorage)Attach;
            if (StoregeInfo.FilePath != null && File.Exists(StoregeInfo.FilePath))
            {
                try
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
                catch (Exception e)
                {
                    Message.Obj = e.Message;
                }
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
    [System.Serializable]
    public class SetTarget : NetQwerry
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            var PriceStorage = (PriceStorage)Attach;
            var PriceStorageList = ((CashClass)Obj).PriceStorageList;
            if (PriceStorageList.Exists(x => x.Name == PriceStorage.Name))
            {
                for (int i = 0; i < PriceStorageList.Count; i++)
                {
                    if (PriceStorageList[i].Name == PriceStorage.Name)
                    {
                        PriceStorageList[i] = PriceStorage;
                    }
                }
            }
            else
            {
                PriceStorageList.Add(PriceStorage);
            }
            ((CashClass)Obj).PriceStorageList = PriceStorageList;
            Message.Obj = true;
            return Message;
        }
    }
    [System.Serializable]
    public class GetDic : NetQwerry
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            var activeprice = (PriceStorage)Attach;
            var Dictionaries = ((CashClass)Obj).Dictionaries.GetDictionaryRelate(Object_Description.DictionaryRelate.Price);
            for (int i = 0; i < Dictionaries.Count(); i++)
            {
                if (Dictionaries.ToList()[i].Values.Contains(activeprice.Name))
                {
                    Message.Obj = Dictionaries.ToList()[i];
                    break;
                }
                if (i == Dictionaries.Count()-1)
                {
                    Message.Obj = new DictionaryPrice("None", DictionaryRelate.Storage);
                }
            }
            return Message;
        }
    }
}
