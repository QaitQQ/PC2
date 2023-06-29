using Newtonsoft.Json;

using SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post.OzonGetDesc;

using StructLibCore.Marketplace;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;

namespace SiteApi.IntegrationSiteApi.APIMarket.Yandex.YandexPostImport
{
    public class YandexPostImport : SiteApi.IntegrationSiteApi.APIMarket.Yandex.YandexApiClass
    {
        public YandexPostImport(APISetting APISetting) : base(APISetting)  {}
        public List<object> Get(StructLibCore.Marketplace.IMarketItem[] List)
        {
            var httpWebRequest = GetRequest(@"/campaigns/" + ClientID + "/offer-mapping-entries/updates.json", "POST");
            Root itemsRoot = new Root();

            foreach (IMarketItem item in List)
            {
                itemsRoot.OfferMappingEntries.Add(new OfferMappingEntry() {Offer = new Offer(item) });
            }
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                var Root = JsonConvert.SerializeObject(itemsRoot);
                streamWriter.Write(Root);
            }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            string result;

            using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }

            return new List<object>();
        }


    }
    internal class WeightDimensions
    {
        [JsonProperty("length")]
        public double Length { get; set; }

        [JsonProperty("width")]
        public double Width { get; set; }

        [JsonProperty("height")]
        public double Height { get; set; }

        [JsonProperty("weight")]
        public double Weight { get; set; }
    }

    internal class ShelfLife
    {
        [JsonProperty("timePeriod")]
        public int TimePeriod { get; set; }

        [JsonProperty("timeUnit")]
        public string TimeUnit { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }
    }

    internal class LifeTime
    {
        [JsonProperty("timePeriod")]
        public int TimePeriod { get; set; }

        [JsonProperty("timeUnit")]
        public string TimeUnit { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }
    }

    internal class GuaranteePeriod
    {
        [JsonProperty("timePeriod")]
        public int TimePeriod { get; set; }

        [JsonProperty("timeUnit")]
        public string TimeUnit { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }
    }

    internal class Offer
    {
        public Offer(IMarketItem item)
        {
            if (item.APISettingSource != null && item.APISettingSource.Type == MarketName.Ozon)
            {
                try
                {
                    Description = new OzonPostDesc(item.APISettingSource).Get(item);
                }
                catch (Exception e)
                {

                    Description = e.Message;
                }
               
            }
            if (item.APISettingSource != null && item.APISettingSource.Type == MarketName.Yandex)
            {
               var X = (Server.Class.IntegrationSiteApi.Market.Yandex.YandexGetName.YandexGetItemList.ItemYandex)item;

                Description = X.description;
                Manufacturer = X.manufacturer; 
            }

            ShopSku = item.SKU;
            Name = item.Name;
            Pictures = item.Pic.ToList();
            Barcodes = new List<string> { item.Barcode };

            Category = "IP камера";
           // Manufacturer = "AltCam";
            ManufacturerCountries = new List<string>() { "Китай" };
            Urls = item.Pic.ToList();
            //   CustomsCommodityCodes = new List<string>();
            //   SupplyScheduleDays = new List<string>();
        }

        [JsonProperty("shopSku")]
        public string ShopSku { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("manufacturer")]
        public string Manufacturer { get; set; }

        [JsonProperty("manufacturerCountries")]
        public List<string> ManufacturerCountries { get; set; }

        //[JsonProperty("weightDimensions")]
        //public WeightDimensions WeightDimensions { get; set; }

        [JsonProperty("urls")]
        public List<string> Urls { get; set; }

        [JsonProperty("pictures")]
        public List<string> Pictures { get; set; }

        //[JsonProperty("vendor")]
        //public string Vendor { get; set; }

        //[JsonProperty("vendorCode")]
        //public string VendorCode { get; set; }

        [JsonProperty("barcodes")]
        public List<string> Barcodes { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        //[JsonProperty("shelfLife")]
        //public ShelfLife ShelfLife { get; set; }

        //[JsonProperty("lifeTime")]
        //public LifeTime LifeTime { get; set; }

        //[JsonProperty("guaranteePeriod")]
        //public GuaranteePeriod GuaranteePeriod { get; set; }

        //[JsonProperty("customsCommodityCodes")]
        //public List<string> CustomsCommodityCodes { get; set; }

        //[JsonProperty("certificate")]
        //public string Certificate { get; set; }

        //[JsonProperty("availability")]
        //public string Availability { get; set; }

        //[JsonProperty("transportUnitSize")]
        //public int TransportUnitSize { get; set; }

        //[JsonProperty("minShipment")]
        //public int MinShipment { get; set; }

        //[JsonProperty("quantumOfSupply")]
        //public int QuantumOfSupply { get; set; }

        //[JsonProperty("supplyScheduleDays")]
        //public List<string> SupplyScheduleDays { get; set; }

        //[JsonProperty("deliveryDurationDays")]
        //public int DeliveryDurationDays { get; set; }

        //[JsonProperty("boxCount")]
        //public int BoxCount { get; set; }

        //[JsonProperty("shelfLifeDays")]
        //public int ShelfLifeDays { get; set; }

        //[JsonProperty("lifeTimeDays")]
        //public int LifeTimeDays { get; set; }

        //[JsonProperty("guaranteePeriodDays")]
        //public int GuaranteePeriodDays { get; set; }
    }

    internal class Mapping
    {
        [JsonProperty("marketSku")]
        public int MarketSku { get; set; }
    }

    internal class OfferMappingEntry
    {
        [JsonProperty("offer")]
        public Offer Offer { get; set; }

        //[JsonProperty("mapping")]
        //public Mapping Mapping { get; set; }
    }

    internal class Root
    {
        public Root()
        {
            OfferMappingEntries = new List<OfferMappingEntry>();
        }

        [JsonProperty("offerMappingEntries")]
        public List<OfferMappingEntry> OfferMappingEntries { get; set; }
    }


}
