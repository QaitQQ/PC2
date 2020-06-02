using Object_Description;

using Pricecona;
using Server.Class.Base;
using Server.Class.HDDClass;

using StructLibs;

using System.Linq;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server
{
    public class CashClass
    {
        public List<СomparisonNameID> ItemName;
        public List<string> UserName;
        public List<string> Tokens;
        public bool MailCheckFlag { get; set; }
        private List<ItemPlusImage> _NewItem;
        private List<ItemChanges> _СhangedItems;
        private List<KeyValuePair<PriceStruct, ItemDBStruct>> _SiteItemsСhanged;
        private Dictionaries dictionaries;
        private event Action<string, object> СhangeList;
        public List<ItemPlusImage> NewItem
        {
            get => _NewItem;
            set { if (value != null) { _NewItem = value; СhangeList?.Invoke(Settings.NewItem, _NewItem); } }
        }

        public List<ItemChanges> СhangedItems
        {
            get => _СhangedItems;
            set { if (value != null) { _СhangedItems = value; СhangeList?.Invoke(Settings.СhangedItems, _СhangedItems); } }
        }

        public List<KeyValuePair<PriceStruct, ItemDBStruct>> SiteItemsСhanged
        {
            get => _SiteItemsСhanged;
            set { if (value != null) { _SiteItemsСhanged = value; СhangeList?.Invoke(Settings.SiteListСhanged, _SiteItemsСhanged); } }
        }

        public Dictionaries Dictionaries { get => dictionaries; set { if (value != null) { dictionaries = value; СhangeList?.Invoke(Settings.Dictionaries, dictionaries); } } }
        public void ReloadNameCash() 
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                ItemName = (from Item in db.Item select new СomparisonNameID() { Name = Item.Name, СomparisonName = Item.СomparisonName, Id = Item.Id }).ToList();
            }
      
        }

        public CashClass()
        {
            NewItem = new List<ItemPlusImage>();
            СhangedItems = new List<ItemChanges>();
            Dictionaries = new Dictionaries();
            Tokens = new List<string>();
            Serializer<object> Serializer = new Serializer<object>();
            MailCheckFlag = false;

            СhangeList += Serializer.Doit;
        }

        public void LoadCash()
        {

             NewItem = Task.Run(() => new Deserializer<List<ItemPlusImage>>(Settings.NewItem).Doit()).Result;
            //  SiteItemsСhanged = Task.Run(() => new Deserializer<List<KeyValuePair<PriceStruct, ItemDBStruct>>>(Settings.SiteListСhanged).Doit()).Result;
            СhangedItems = Task.Run(() => new Deserializer<List<ItemChanges>>(Settings.СhangedItems).Doit()).Result;
            Dictionaries = Task.Run(() => new Deserializer<Dictionaries>(Settings.Dictionaries).Doit()).Result;
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
    }
}
