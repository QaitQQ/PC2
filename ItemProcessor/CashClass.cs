using Object_Description;

using Server.Class.Base;
using Server.Class.HDDClass;
using Server.Class.PriceProcessing;

using StructLibCore;
using StructLibCore.Marketplace;

using StructLibs;

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
namespace Server
{
    public class CashClass
    {
        public List<СomparisonNameID> ItemName;
        public List<string> UserName;
        public List<string> Tokens;
        private List<PriceStorage> priceStorageList;
        private List<ItemDBStruct> siteList;
        public List<QueueOfObj> ObjBuffer;
        public List<ItemDBStruct> SiteList
        {
            get => siteList;
            set { siteList = value; СhangeList?.Invoke(Settings.SiteList, siteList); }
        }
        private List<Target> targets;
        public List<Target> Targets
        {
            get => targets;
            set { targets = value; СhangeList?.Invoke(Settings.TargetsList, targets); }
        }
        public string[] ApiSiteSettngs;
        public string[] FtpSiteSettngs;
        public List<PriceStorage> PriceStorageList
        {
            get => priceStorageList;
            set { priceStorageList = value; СhangeList?.Invoke(Settings.PriceStoragePath, PriceStorageList); }
        }
        public bool MailCheckFlag { get; set; }
        private List<ItemPlusImageAndStorege> newItem;
        private List<ItemChanges> changedItems;
        private Dictionaries dictionaries;
        private MarketPlaceCash marketplace;
        public MarketPlaceCash Marketplace
        {
            get { return marketplace; }
            set { if (value != null) { marketplace = value; СhangeList?.Invoke(Settings.Marketplace, marketplace); } }
        }
        private event Action<string, object> СhangeList;
        public List<ItemPlusImageAndStorege> NewItem
        {
            get => newItem;
            set { if (value != null) { newItem = value; СhangeList?.Invoke(Settings.NewItem, newItem); } }
        }
        public List<ItemChanges> СhangedItems
        {
            get => changedItems;
            set { if (value != null) { changedItems = value; СhangeList?.Invoke(Settings.СhangedItems, changedItems); } }
        }
        public Dictionaries Dictionaries { get => dictionaries; set { if (value != null) { dictionaries = value; СhangeList?.Invoke(Settings.Dictionaries, dictionaries); } } }
        public void ReloadNameCash()
        {
            using ApplicationContext db = new ApplicationContext();
            ItemName = (from Item in db.Item select new СomparisonNameID() { Name = Item.Name, СomparisonName = Item.СomparisonName[0], Id = Item.Id }).ToList();
        }
        public CashClass()
        {
            targets = new List<Target>();
            newItem = new List<ItemPlusImageAndStorege>();
            priceStorageList = new List<PriceStorage>();
            changedItems = new List<ItemChanges>();
            dictionaries = new Dictionaries();
            siteList = new List<ItemDBStruct>();
            Tokens = new List<string>();
            Marketplace = new MarketPlaceCash();
            Serializer<object> Serializer = new Serializer<object>();
            MailCheckFlag = false;
            ObjBuffer = new List<QueueOfObj>();
            СhangeList += Serializer.Doit;
            ApiSiteSettngs = Settings.ApiSettngs;
            FtpSiteSettngs = Settings.FtpSettingsImage;
            UploadStoregeToSite(this);
        }
        public void LoadCash()
        {
            LoadFromFile(ref newItem, Settings.NewItem);
            LoadFromFile(ref changedItems, Settings.СhangedItems);
            LoadFromFile(ref dictionaries, Settings.Dictionaries);
            LoadFromFile(ref priceStorageList, Settings.PriceStoragePath);
            LoadFromFile(ref siteList, Settings.SiteList);
            LoadFromFile(ref targets, Settings.TargetsList);
            LoadFromFile(ref marketplace, Settings.Marketplace);
        }
        private void LoadFromFile<T>(ref T Object, string Path)
        {
            T Obj = Task.Run(() => new Deserializer<T>(Path).Doit()).Result;
            if (Obj != null) { Object = Obj; }
        }
        public void PlanedPriceWork(CashClass cash)
        {
            foreach (PriceStorage item in PriceStorageList)
            {
                string key = item.Name + "_TargetDictionary";
                if (item.PlanedRead)
                {
                    if (!TargetDictionary.Dictionarys.ContainsKey(key))
                    {
                        TargetDictionary.Dictionarys.Add(key, () => Download(item));
                    }
                    if (targets.FindAll(x => x.KeyTask == key).Count == 0)
                    {
                        targets.Add(new Target(key, Target.Regularity.by_time, item.PlanedTime));
                    }
                }
                else
                {
                    if (TargetDictionary.Dictionarys.ContainsKey(key))
                    {
                        TargetDictionary.Dictionarys.Remove(key);
                        targets.Remove(targets.Find(x => x.KeyTask == key));
                    }
                }
            }
            void Download(PriceStorage activeprice)
            {
                string Path = null;

                if (activeprice.FilePath == null || activeprice.FilePath == "")
                {
                    Path = @"price_storage\\" + activeprice.Name + ".xlsx"; ;
                }
                else
                {
                    Path = activeprice.FilePath;
                }


                if (activeprice.Link.ToLower().Contains("Zip".ToLower()))
                {
                    DownloadPriceZip(activeprice, Path);
                }
                else
                {
                    DownloadPriceXls(activeprice, Path);
                }

                Thread.Sleep(10000);

                Read(activeprice);
                foreach (PriceStorage item in cash.PriceStorageList)
                {
                    if (item.Name == activeprice.Name)
                    {
                        item.ReceivingData = DateTime.Now;
                    }
                }
                cash.PriceStorageList = cash.PriceStorageList;
            }
            List<ItemDBStruct> DB_list;
            void Read(PriceStorage activeprice)
            {
                try
                {
                    FileStream fs = File.OpenRead(activeprice.FilePath);
                    string Attb = string.Join(",", activeprice.Attributes);
                    using ApplicationContext DB = new ApplicationContext();
                    DB_list = DB.Item.ToList();
                    PriceProcessingRules lst = new PriceProcessingRules(fs, activeprice.Name, Attb, cash);
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
                Сompare_NewPrice_with_DB cmpr = new Сompare_NewPrice_with_DB((List<ItemPlusImageAndStorege>)Lst, DB_list, cash);
                cmpr.StartCompare();
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
                        entry.ExtractToFile(Path);
                        break;
                    }
                }
            }
        }
        public void UploadStoregeToSite(CashClass cash)
        {
            var FTP = new FTP(Settings.FtpSettingsStorege);
            using ApplicationContext db = new ApplicationContext();
            var IC = from St in db.Storage select new { ID = St.ItemID, SID = St.WarehouseID, C = St.Count, D = St.DateСhange };
            var W = from Wt in db.Warehouse select new { ID = Wt.Id, WN = Wt.Name };
            var i = 0;
            SiteStoregeStruct _struct = new SiteStoregeStruct();
            foreach (var item in IC)
            {
                if (item.D.Day == DateTime.Now.Day && item.C>0)
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
