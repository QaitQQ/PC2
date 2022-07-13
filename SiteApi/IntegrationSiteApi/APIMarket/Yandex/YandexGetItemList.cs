﻿using Newtonsoft.Json;
using StructLibCore.Marketplace;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Linq;

namespace Server.Class.IntegrationSiteApi.Market.Yandex.YandexGetName
{
    public class YandexGetItemList : SiteApi.IntegrationSiteApi.APIMarket.Yandex.YandexApiClass
    {
        public YandexGetItemList(APISetting APISetting) : base(APISetting){}
        public List<object> Get()
        {
            var httpWebRequest = GetRequest(@"/campaigns/" + ClientID + "/offer-mapping-entries.json?limit=200", "GET");
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }
            Root root = JsonConvert.DeserializeObject<Root>(result);

            var Lst = new List<object>();
            var Prises = new YandexGetPrice.YandexGetPrice(aPISetting).Get();

            Mapped(root, Lst, Prises);

            if (root.result.paging.nextPageToken != null)
            {
                httpWebRequest = GetRequest(@"/campaigns/" + ClientID + "/offer-mapping-entries.json?page_token=" + root.result.paging.nextPageToken, "GET");
                httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }
                root = JsonConvert.DeserializeObject<Root>(result);
                Mapped(root, Lst, Prises);
            }

            return Lst;
        }

        private static void Mapped(Root root, List<object> Lst, List<object> Prises)
        {
            foreach (var item in root.result.offerMappingEntries)
            {

                IEnumerable<object> z = from prise in Prises where ((IMarketItem)prise).SKU == item.offer.SKU select prise;

                if (z.Count() > 0)
                {
                    item.offer.Price = ((IMarketItem)z.First()).Price;
                    item.offer.Yprice = ((Server.Class.IntegrationSiteApi.Market.Yandex.YandexGetPrice.YandexItem)z.First()).price;
                }
                Lst.Add(item.offer);
            }
        }

        [Serializable]
        public class Paging
        {
            public string nextPageToken { get; set; }
        }
        [Serializable]
        public class WeightDimensions
        {
            public double length { get; set; }
            public double width { get; set; }
            public double height { get; set; }
            public double weight { get; set; }
        }
        [Serializable]
        public class ProcessingState
        {
            public string status { get; set; }
        }
        [Serializable]
        public class GuaranteePeriod
        {
            public int timePeriod { get; set; }
            public string timeUnit { get; set; }
        }
        [Serializable]
        public class ItemYandex : IMarketItem
        {
            public string name { get; set; }
            public string shopSku { get; set; }
            public string category { get; set; }
            public string vendor { get; set; }
            public string description { get; set; }
            public List<string> barcodes { get; set; }
            public List<string> pictures { get; set; }
            public WeightDimensions weightDimensions { get; set; }
            public List<string> supplyScheduleDays { get; set; }
            public ProcessingState processingState { get; set; }
            public string availability { get; set; }
            public List<string> urls { get; set; }
            public string manufacturer { get; set; }
            public List<string> manufacturerCountries { get; set; }
            public int? guaranteePeriodDays { get; set; }
            public GuaranteePeriod guaranteePeriod { get; set; }
            public string Name { get { return name; } set { name = value; } }
            public string Price { get; set; }
            public string SKU { get { return shopSku; } set { shopSku = value; } }
            public string Stocks { get; set; }
            public StructLibCore.Marketplace.APISetting APISetting { get; set; }
            public Server.Class.IntegrationSiteApi.Market.Yandex.YandexGetPrice.Price Yprice { get; set; }

            public string MinPrice { get; set; }
            public APISetting APISettingSource { get; set; }
            public string Pic { get { return pictures[0]; } set { pictures[0] = value; } }
            public string Barcode { get { return barcodes[0]; } set { barcodes[0] = value; } }
        }
        [Serializable]
        public class Mapping
        {
            public object marketSku { get; set; }
            public int categoryId { get; set; }
        }
        [Serializable]
        public class AwaitingModerationMapping
        {
            public string marketSku { get; set; }
            public string categoryId { get; set; }
        }
        [Serializable]
        public class OfferMappingEntry
        {
            public ItemYandex offer { get; set; }
            public Mapping mapping { get; set; }
            public AwaitingModerationMapping awaitingModerationMapping { get; set; }
        }
        [Serializable]
        public class Result
        {
            public Paging paging { get; set; }
            public List<OfferMappingEntry> offerMappingEntries { get; set; }
        }
        [Serializable]
        public class Root
        {
            public string status { get; set; }
            public Result result { get; set; }
        }
    }
}
