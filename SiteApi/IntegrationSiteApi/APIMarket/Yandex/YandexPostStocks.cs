using Newtonsoft.Json;

using SiteApi.IntegrationSiteApi.APIMarket.Yandex;

using StructLibCore.Marketplace;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Server.Class.IntegrationSiteApi.Market.Yandex.YandexGetPrice
{
    internal class YandexPostStocks : SiteApi.IntegrationSiteApi.APIMarket.Yandex.YandexApiClass
    {
        public YandexPostStocks(APISetting APISetting) : base(APISetting)
        {
        }

        public List<object> Get(List<object> ObjectsList)
        {
            var Lst = new List<object>();
            var httpWebRequest = GetRequest(@"/campaigns/" + ClientID + "/stats/skus.json", "POST");
            List<string> SKULst = new List<string>();
            foreach (IMarketItem item in ObjectsList){SKULst.Add(item.SKU);}
            POSTStocks QW = new POSTStocks();
            QW.ShopSkus = SKULst;
            using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(JsonConvert.SerializeObject(QW));
            }
            ResultStocks root = new Deserialize<ResultStocks>(httpWebRequest).Get(result);
            foreach (var item in root.Result.ShopSkus)
            {
                Lst.Add(new KeyValuePair<string, string>(item.Sku, item.Warehouses?[0].Stocks.FindAll(x => x.Type == "FIT")[0].Count.ToString()));
            }
            return Lst;
        }
        public class POSTStocks
        {
            [JsonProperty("shopSkus")]
            public List<string> ShopSkus;
        }
        public class Hiding
        {
            [JsonProperty("comment")]
            public string Comment { get; set; }
        }

        public class Result
        {
            [JsonProperty("shopSkus")]
            public List<ShopSku> ShopSkus { get; set; }
        }

        public class ResultStocks
        {
            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("result")]
            public Result Result { get; set; }
        }

        public class ShopSku
        {
            [JsonProperty("shopSku")]
            public string Sku { get; set; }

            [JsonProperty("marketSku")]
            public object MarketSku { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("price")]
            public double Price { get; set; }

            [JsonProperty("categoryId")]
            public int CategoryId { get; set; }

            [JsonProperty("categoryName")]
            public string CategoryName { get; set; }

            [JsonProperty("weightDimensions")]
            public WeightDimensions WeightDimensions { get; set; }

            [JsonProperty("hidings")]
            public List<Hiding> Hidings { get; set; }

            [JsonProperty("tariffs")]
            public List<Tariff> Tariffs { get; set; }

            [JsonProperty("pictures")]
            public List<string> Pictures { get; set; }

            [JsonProperty("warehouses")]
            public List<Warehouse> Warehouses { get; set; }
        }

        public class Stock
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("count")]
            public int Count { get; set; }
        }

        public class Tariff
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("percent")]
            public double Percent { get; set; }

            [JsonProperty("amount")]
            public double Amount { get; set; }
        }

        public class Warehouse
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("stocks")]
            public List<Stock> Stocks { get; set; }
        }

        public class WeightDimensions
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



    }
}