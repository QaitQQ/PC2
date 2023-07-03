using Server.Class.Base;
using Server.Class.PriceProcessing;
using StructLibCore;
using StructLibs;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using System.Xml.Serialization;
namespace Server
{
    public class PricePlanedWork
    {
        public void Download(CashClass cash)
        {
            foreach (PriceStorage item in cash.PriceStorageList)
            {
                string key = item.Name + "_TargetDictionary";
                if (item.PlanedRead)
                {
                    if (!TargetDictionary.Dictionarys.ContainsKey(key))
                    {
                        TargetDictionary.Dictionarys.Add(key, () => Download(item));
                    }
                    if (cash.Targets.FindAll(x => x.KeyTask == key).Count == 0)
                    {
                        cash.Targets.Add(new Target(key, Target.RegularityType.once, item.PlanedTime));
                    }
                }
                else
                {
                    if (TargetDictionary.Dictionarys.ContainsKey(key))
                    {
                        TargetDictionary.Dictionarys.Remove(key);
                        cash.Targets.Remove(cash.Targets.Find(x => x.KeyTask == key));
                    }
                }
            }
            void Download(PriceStorage activeprice)
            {
                string Path = null;
                if (activeprice.FilePath == null || activeprice.FilePath == "")   {  Path = @"price_storage\\" + activeprice.Name + ".xlsx";                 }
                else{Path = activeprice.FilePath;}
                if (activeprice.Link.ToLower().Contains("zip")){DownloadPriceZip(activeprice, Path);}
                else{DownloadPriceXls(activeprice, Path);}
                Read(activeprice);
                foreach (PriceStorage item in cash.PriceStorageList)
                {
                    if (item.Name == activeprice.Name){item.ReceivingData = DateTime.Now;}
                }
                cash.PriceStorageList = cash.PriceStorageList;
            }
            List<ItemDBStruct> DB_list;
            void Read(PriceStorage activeprice)
            {
                try
                {
                    FileStream fs = File.OpenRead(activeprice.FilePath);
                    string Attr = string.Join(",", activeprice.Attributes);
                    using ApplicationContext DB = new ApplicationContext();
                    DB_list = DB.Item.ToList();
                    PriceProcessingRules lst = new PriceProcessingRules(fs, activeprice.Name, Attr, cash);
                    lst.СhangeResult += Comparer;
                    lst.Apply_rules();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
            void Comparer(object Lst)
            {
                Сompare_NewPrice_with_DB CompareNewPriceWithDB = new Сompare_NewPrice_with_DB((List<ItemPlusImageAndStorege>)Lst, DB_list, cash);
                CompareNewPriceWithDB.StartCompare();
            }
            void DownloadPriceXls(PriceStorage activeprice, string Path)
            {
                WebClient client = new WebClient();
                client.DownloadFile(new Uri(activeprice.Link), activeprice.FilePath);
            }
            void DownloadPriceZip(PriceStorage activeprice, string Path)
            {
                WebClient client = new WebClient();
                byte[] Mass = client.DownloadData(new Uri(activeprice.Link));
                Stream stream = new MemoryStream(Mass);
                ZipArchive archive = new ZipArchive(stream);
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    if (entry.FullName.ToLower().Contains(".xls"))
                    {
                        if (File.Exists(activeprice.FilePath)){File.Delete(activeprice.FilePath);}
                        entry.ExtractToFile(activeprice.FilePath);
                        break;
                    }
                }
            }
        }
        public void UploadStorageToSite()
        {
            FTP FTP = new FTP(Settings.FtpSettingsStorege);
            using ApplicationContext db = new ApplicationContext();
            var IC = from St in db.Storage select new { ID = St.ItemID, SID = St.WarehouseID, C = St.Count, D = St.DateСhange };
            var W = from Wt in db.Warehouse select new { ID = Wt.Id, WN = Wt.Name };
            SiteStoregeStruct _struct = new SiteStoregeStruct();
            foreach (var item in IC)
            {
                if (item.D.Day == DateTime.Now.Day && item.C > 0)
                {
                    _struct.ItmsCount.Add(new IC() { Id = item.ID, WID = item.SID, C = item.C });
                }
            }
            foreach (var item in W)
            {
                _struct.Warehouses.Add(new WS() { Id = item.ID, N = item.WN });
            }
            XmlSerializer xmlSerializer = new(typeof(SiteStoregeStruct));
            string FN = "STList.xml";
            if (File.Exists(FN))
            {
                File.Delete(FN);
            }
            using FileStream fs = new(FN, FileMode.OpenOrCreate);
            xmlSerializer.Serialize(fs, _struct);
            fs.Close();
            FTP.FTPUploadFile(FN);
        }
    }
}
