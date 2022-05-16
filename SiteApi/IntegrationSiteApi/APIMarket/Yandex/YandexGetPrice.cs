using Newtonsoft.Json;

using SiteApi.IntegrationSiteApi;

using StructLibCore.Marketplace;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Server.Class.IntegrationSiteApi.Market.Yandex.YandexGetPrice
{

    public class YandexGetPrice : SiteApi.IntegrationSiteApi.APIMarket.Yandex.YandexApiClass
    {
        public YandexGetPrice(APISetting APISetting) : base(APISetting)
        {
        }

        public List<object> Get()
        {
            var httpWebRequest = GetRequest(@"/campaigns/" + ClientID + "/offer-prices.json", "GET");

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }
            Root root = JsonConvert.DeserializeObject<Root>(result);

           var Lst = new List<object>();

            foreach (var item in root.result.offers)
            {
                Lst.Add(item);
            }

            return Lst;
        }
    }
    [Serializable]
    public class Paging
    {
        public string nextPageToken { get; set; }
    }
    [Serializable]
    public class Price
    {
        public string value { get; set; }
        public string discountBase { get; set; }
        public string currencyId { get; set; }
        public int vat { get; set; }
    }
    [Serializable]
    public class YandexItem : IMarketItem
    {
        public string id { get; set; }
        public Price price { get; set; }
        public DateTime updatedAt { get; set; }
        public string marketSku { get; set; }
        public string Name { get; set; }
        public string Price { get { return price.value; } set { } }
        public string SKU { get { return id; } set { marketSku = value; } }
        public string Stocks { get; set; }
        public APISetting APISetting { get; set; }
        public string MinPrice { get { return price.discountBase; } set { price.discountBase = value; } }
        public APISetting APISettingSource { get; set; }
        public string Pic { get; set; }
        public string Barcode { get; set; }
    }
    [Serializable]
    public class Result
    {
        public int total { get; set; }
        public Paging paging { get; set; }
        public List<YandexItem> offers { get; set; }
    }
    [Serializable]
    public class Root
    {
        public string status { get; set; }
        public Result result { get; set; }
    }
}
