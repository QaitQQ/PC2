using Newtonsoft.Json;
using SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post;
using StructLibCore.Marketplace;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
namespace Server.Class.IntegrationSiteApi.Market.Ozon
{
    public class OzonPostItemCopyIDList : SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post.OzonPost
    {
        public OzonPostItemCopyIDList(APISetting aPISetting) : base(aPISetting)
        {
        }
        public TaskIdName Get(List<string> Ids)
        {
            var httpWebRequest = GetRequest(@"v1/product/import-by-sku");
            var Request = new Request();
            foreach (var item in Ids) { Request.Items.Add(new Item(item)); }
            var names = new List<string>();
            foreach (var item in Request.Items)
            {
                names.Add(item.Name);
            }
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(Request);
                streamWriter.Write(json);
            }
            Response Response = null;
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }
                Response = JsonConvert.DeserializeObject<Response>(result);
            }
            catch (System.Exception e)
            {
                return null;
            }
            return new TaskIdName() { TaskId = Response.Result.TaskId.ToString(), OzonIds = Ids, Names = names };
        }
        public class TaskIdName
        {
            public TaskIdName()
            {
                OzonIds = new List<string>();
                Names = new List<string>();
            }
            public string TaskId { get; set; }
            public List<string> OzonIds { get; set; }
            public List<string> Names { get; set; }
        }
        public class Item
        {
            public Item(IMarketItem item)
            {
                var ozItem = (SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post.OzonItemDesc)item;
                Sku = ozItem.id.ToString();
                Name = ozItem.Name;
                OfferId = ozItem.SKU;
                CurrencyCode = "RUB";
                OldPrice = ozItem.old_price;
                Price = ozItem.price;
                PremiumPrice = ozItem.premium_price;
                Vat = "0";
            }
            public Item(string Id)
            {
                string name = Environment.TickCount.ToString();
                Sku = Id;
                Name = name;
                OfferId = name;
                CurrencyCode = "RUB";
                OldPrice = "0";
                Price = "0";
                PremiumPrice = "0";
                Vat = "0";
            }
            [JsonProperty("sku")]
            public string Sku { get; set; }
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("offer_id")]
            public string OfferId { get; set; }
            [JsonProperty("currency_code")]
            public string CurrencyCode { get; set; }
            [JsonProperty("old_price")]
            public string OldPrice { get; set; }
            [JsonProperty("price")]
            public string Price { get; set; }
            [JsonProperty("premium_price")]
            public string PremiumPrice { get; set; }
            [JsonProperty("vat")]
            public string Vat { get; set; }
        }
        public class Request
        {
            public Request()
            {
                Items = new List<Item>();
            }
            [JsonProperty("items")]
            public List<Item> Items { get; set; }
        }
        public class Result
        {
            [JsonProperty("task_id")]
            public int TaskId { get; set; }
            [JsonProperty("unmatched_sku_list")]
            public List<object> UnmatchedSkuList { get; set; }
        }
        public class Response
        {
            [JsonProperty("result")]
            public Result Result { get; set; }
        }
    }
}
