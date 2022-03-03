using Object_Description;

using Server.Class.Base;
using Server.Class.HDDClass;
using Server.Class.ItemProcessor;
using Server.Class.PriceProcessing;

using StructLibCore;

using StructLibs;

using System;
using System.Collections.Generic;
using System.IO;
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
        public List<ItemDBStruct> SiteList
        {
            get => siteList;
            set { siteList = value; СhangeList?.Invoke(Settings.SiteList, siteList); }
        }
        private List<Target> targets;
        public List<Target> Targets
        {
            get => targets;
            set
            {
                targets = value; СhangeList?.Invoke(Settings.TargetsList, targets);
            }
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
 
        private List<MarketItem> marketItems;
        public List<MarketItem> MarketItems
        {
            get { return marketItems; }
            set { if (value != null) { marketItems = value; СhangeList?.Invoke(Settings.MarketItems, marketItems); } }
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
            using (ApplicationContext db = new ApplicationContext())
            {
                ItemName = (from Item in db.Item select new СomparisonNameID() { Name = Item.Name, СomparisonName = Item.СomparisonName[0], Id = Item.Id }).ToList();
            }

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
            marketItems = new List<MarketItem>();
            Serializer<object> Serializer = new Serializer<object>();
            MailCheckFlag = false;
            ObjBuffer = new List<QueueOfObj>();
            СhangeList += Serializer.Doit;
            ApiSiteSettngs = Settings.ApiSettngs;
            FtpSiteSettngs = Settings.FtpSettings;

        }
        public void LoadCash()
        {
            LoadFromFile(ref newItem, Settings.NewItem);
            LoadFromFile(ref changedItems, Settings.СhangedItems);
            LoadFromFile(ref dictionaries, Settings.Dictionaries);
            LoadFromFile(ref priceStorageList, Settings.PriceStoragePath);
            LoadFromFile(ref siteList, Settings.SiteList);
            LoadFromFile(ref targets, Settings.TargetsList);
            LoadFromFile(ref marketItems, Settings.MarketItems);
            ReloadNameCash();
            // Gen_Dic();
        }
        private void Gen_Dic()
        {
            Dictionaries = new Dictionaries();

            Dictionaries.Add(new DictionaryPrice("DSSL_Price", new List<string> { "ДССЛ" }, DictionaryRelate.Price));
            Dictionaries.Add(new DictionaryPrice("Hik_Price", new List<string> { "Хик" }, DictionaryRelate.Price));
            Dictionaries.Add(new DictionaryPrice("GeoVision_Price", new List<string> { "GeoVision" }, DictionaryRelate.Price));
            Dictionaries.Add(new DictionaryPrice("HiWatch_Price", new List<string> { "Хайвотч" }, DictionaryRelate.Price));
            Dictionaries.Add(new DictionaryPrice("HIQ", new List<string> { "HIQ" }, DictionaryRelate.Price));
            Dictionaries.Add(new DictionaryPrice("Dahua", new List<string> { "Dahua" }, DictionaryRelate.Price));

            Dictionaries.Add(new DictionaryPrice("DSSL_Storage", new List<string> { "Складская справка email" }, DictionaryRelate.Storage));
            Dictionaries.Add(new DictionaryBase("xls", new List<string> { "xls", "xlsx" }, Relate: DictionaryRelate.Extension));
            Dictionaries.Add(new DictionaryBase("NameEdit", new List<string> { "!", "(", ")", "новинка", "готовится к релизу", "снимается с продаж", "снимается с производства", "замена", "\n", "Доступен к заказу" }, DictionaryRelate.Extension));
            Dictionaries.Add(new DictionaryBase("New", new List<string> { "новинка", "готовитсякрелизу" }, DictionaryRelate.Tags));
            Dictionaries.Add(new DictionaryBase("Old", new List<string> { "снимаетсяспродаж", "снимаетсяспроизводства", "замена" }, DictionaryRelate.Tags));
            Dictionaries.Add(new DictionaryBase("OnRequest", new List<string> { "подзаказ2месяца", "подзаказ2недели" }, DictionaryRelate.Tags));



            DictionaryPrice DSSL_PriceDic = (DictionaryPrice)Dictionaries.Get("DSSL_Price");

            DSSL_PriceDic.Set_Filling_method_string(FillDefinitionPrice.Name, "Модель");
            DSSL_PriceDic.Set_Filling_method_string(FillDefinitionPrice.PriceRC, "розни");
            DSSL_PriceDic.Set_Filling_method_string(FillDefinitionPrice.PriceRC, "ользователь");
            DSSL_PriceDic.Set_Filling_method_string(FillDefinitionPrice.PriceRC, "EU");
            DSSL_PriceDic.Set_Filling_method_string(FillDefinitionPrice.Description, "писание");
            DSSL_PriceDic.Set_Filling_method_string(FillDefinitionPrice.PriceDC, "Дистрибьютор");
            DSSL_PriceDic.Set_Filling_method_string(FillDefinitionPrice.MaxRow, "250");


            DictionaryPrice Hik_PriceDic = (DictionaryPrice)Dictionaries.Get("Hik_Price");

            Hik_PriceDic.Set_Filling_method_string(FillDefinitionPrice.Name, "Модель");
            Hik_PriceDic.Set_Filling_method_string(FillDefinitionPrice.PriceRC, "Новая РРЦ");
            Hik_PriceDic.Set_Filling_method_string(FillDefinitionPrice.Description, "Описание");
            Hik_PriceDic.Set_Filling_method_string(FillDefinitionPrice.MaxRow, "250");

            DictionaryPrice GeoVision_Price_PriceDic = (DictionaryPrice)Dictionaries.Get("GeoVision_Price");

            GeoVision_Price_PriceDic.Set_Filling_method_string(FillDefinitionPrice.Name, @"Наименование");
            GeoVision_Price_PriceDic.Set_Filling_method_string(FillDefinitionPrice.PriceRC, "Розн");
            GeoVision_Price_PriceDic.Set_Filling_method_string(FillDefinitionPrice.PriceDC, "Дилер");
            GeoVision_Price_PriceDic.Set_Filling_method_string(FillDefinitionPrice.Description, "Наименование");
            GeoVision_Price_PriceDic.Set_Filling_method_string(FillDefinitionPrice.MaxRow, "250");
            GeoVision_Price_PriceDic.Set_Filling_method_string(FillDefinitionPrice.NamePattern, @"^.*\s");

            DictionaryPrice HiWatch_PriceDic = (DictionaryPrice)Dictionaries.Get("HiWatch_Price");

            HiWatch_PriceDic.Set_Filling_method_string(FillDefinitionPrice.Name, "Модель");
            HiWatch_PriceDic.Set_Filling_method_string(FillDefinitionPrice.PriceRC, "Розница 10.03");
            HiWatch_PriceDic.Set_Filling_method_string(FillDefinitionPrice.Description, "Описание");
            HiWatch_PriceDic.Set_Filling_method_string(FillDefinitionPrice.MaxRow, "250");

            DictionaryPrice HIQ_PriceDic = (DictionaryPrice)Dictionaries.Get("HIQ");

            HIQ_PriceDic.Set_Filling_method_string(FillDefinitionPrice.Name, "Наименование");
            HIQ_PriceDic.Set_Filling_method_string(FillDefinitionPrice.PriceRC, "Розничная");
            HIQ_PriceDic.Set_Filling_method_string(FillDefinitionPrice.Description, "Краткие характеристики");
            HIQ_PriceDic.Set_Filling_method_string(FillDefinitionPrice.NamePattern, @"HIQ.*?\, ");
            HIQ_PriceDic.Set_Filling_method_string(FillDefinitionPrice.MaxRow, "250");

            DictionaryPrice Dahua_PriceDic = (DictionaryPrice)Dictionaries.Get("Dahua");
            Dahua_PriceDic.Set_Filling_method_string(FillDefinitionPrice.Name, "Наименование");
            Dahua_PriceDic.Set_Filling_method_string(FillDefinitionPrice.PriceRC, "Розничная");
            Dahua_PriceDic.Set_Filling_method_string(FillDefinitionPrice.Description, "Описание");
            Dahua_PriceDic.Set_Filling_method_string(FillDefinitionPrice.MaxRow, "250");
            Dahua_PriceDic.Set_Filling_method_string(FillDefinitionPrice.PriceRC, "РРЦ");

        }
        private void LoadFromFile<T>(ref T Object, string Path)
        {
            T Obj = Task.Run(() => new Deserializer<T>(Path).Doit()).Result;
            if (Obj != null)
            {
                Object = Obj;
            }

        }
        public void planedPriceWork(CashClass cash)
        {
            foreach (var item in PriceStorageList)
            {
                var key = item.Name + "_TargetDictionary";

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
                System.Net.WebClient client = new System.Net.WebClient();
                client.DownloadFileAsync(new Uri(activeprice.Link), activeprice.FilePath);

                client.DownloadFileCompleted += (a, e) => { Read(activeprice); };

                foreach (var item in cash.PriceStorageList)
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
                var fs = File.OpenRead(activeprice.FilePath);
                string Attb = string.Join(",", activeprice.Attributes);
                using (ApplicationContext DB = new ApplicationContext())
                {
                    DB_list = DB.Item.ToList();
                    var lst = new PriceProcessingRules(fs, activeprice.Name, Attb, cash);
                    lst.СhangeResult += Comparer;
                    lst.Apply_rules();
                }

            }
            void Comparer(object Lst)
            {
                var cmpr = new Сompare_NewPrice_with_DB((List<ItemPlusImageAndStorege>)Lst, DB_list, cash);
                cmpr.StartCompare();
            }

        }
    }
}

