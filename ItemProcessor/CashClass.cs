using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Object_Description;
using Server.Class.Base;
using Server.Class.HDDClass;
using StructLibCore.Marketplace;
using StructLibs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private List<Target> targets;
        public string[] ApiSiteSettings;
        public string[] FtpSiteSettings;
        public static string[] FtpSiteSettingsRoot { get {return Settings.FtpSettingsStorege;} }
        public bool MailCheckFlag { get; set; }
        private List<ItemPlusImageAndStorege> newItem;
        private List<ItemChanges> changedItems;
        private Dictionaries dictionaries;
        private MarketPlaceCash marketplace;
        private event Action<string, object> ChangeList;
        public List<ItemDBStruct> SiteList
        {
            get => siteList;
            set { siteList = value; ChangeList?.Invoke(Settings.SiteList, siteList); }
        }
        public List<Target> Targets
        {
            get => targets;
            set { targets = value; ChangeList?.Invoke(Settings.TargetsList, targets); }
        }
        public List<PriceStorage> PriceStorageList
        {
            get => priceStorageList;
            set { priceStorageList = value; ChangeList?.Invoke(Settings.PriceStoragePath, PriceStorageList); }
        }
        public MarketPlaceCash Marketplace
        {
            get { return marketplace; }
            set { if (value != null) { marketplace = value; ChangeList?.Invoke(Settings.Marketplace, marketplace); } }
        }
        public List<ItemPlusImageAndStorege> NewItem
        {
            get => newItem;
            set { if (value != null) { newItem = value; ChangeList?.Invoke(Settings.NewItem, newItem); } }
        }
        public List<ItemChanges> ChangedItems
        {
            get => changedItems;
            set { if (value != null) { changedItems = value; ChangeList?.Invoke(Settings.СhangedItems, changedItems); } }
        }
        public Dictionaries Dictionaries { get => dictionaries; set { if (value != null) { dictionaries = value; ChangeList?.Invoke(Settings.Dictionaries, dictionaries); } } }
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
            ChangeList += Serializer.Doit;
            ApiSiteSettings = Settings.ApiSettngs;
            FtpSiteSettings = Settings.FtpSettingsImage;
        }
        public void ReloadNameCash()
        {
            using ApplicationContext db = new ApplicationContext();
            ItemName = (from Item in db.Item select new СomparisonNameID() { Name = Item.Name, СomparisonName = Item.СomparisonName[0], Id = Item.Id }).ToList();
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
        private static void LoadFromFile<T>(ref T Object, string Path)
        {
            T Obj = Task.Run(() => new Deserializer<T>(Path).Doit()).Result;
            if (Obj != null) { Object = Obj; }
        }
        public static void PlanedPriceWork(CashClass cash)
        {
            var N = cash.Targets.FirstOrDefault(x => x.KeyTask == "planedPriceWork");
            if (N == null)
            {
                var TR = new Target("planedPriceWork", Target.RegularityType.after_time);
                TR.Period = 86400;
                cash.Targets.Add(TR);
            }
            else
            {
                if (N.Regularity == Target.RegularityType.once)
                {
                    cash.Targets.Remove(N);
                    var TR = new Target("planedPriceWork", Target.RegularityType.after_time);
                    TR.Period = 86400;
                    cash.Targets.Add(TR);
                }
            }
            new PricePlanedWork().Download(cash);
        }
        public static void UploadStorageToSite(CashClass cash)
        {
            var N = cash.Targets.FirstOrDefault(x => x.KeyTask == "UploadStorageToSite");
            if (N == null) 
            {
                var TR = new Target("UploadStorageToSite", Target.RegularityType.after_time);
                TR.Period = 1200;
                cash.Targets.Add(TR);
            }
            else
            {
                if (N.Regularity == Target.RegularityType.once)     
                {
                    cash.Targets.Remove(N);
                    var TR = new Target("UploadStorageToSite", Target.RegularityType.after_time);
                    TR.Period = 1200;
                    cash.Targets.Add(TR);
                }
            }
            new PricePlanedWork().UploadStorageToSite();
        }
    }
}
