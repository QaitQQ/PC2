using Newtonsoft.Json;

using SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post;

using StructLibCore.Marketplace;

using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Server.Class.IntegrationSiteApi.Market.Ozon
{
    public class OzonPostItemList : SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post.OzonPost
    {
        public OzonPostItemList(APISetting aPISetting) : base(aPISetting)
        {
        }

        public List<Item> Get()
        {
            var httpWebRequest = GetRequest(@"v3/product/list");

            Filter filter = new() { Visibility = "ALL" };
            Request request = new() {Limit = 1000, Filter=filter, LastId = "" };

            Response response = null;
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {

                string json = JsonConvert.SerializeObject(request); streamWriter.Write(json);
                streamWriter.Write(json);
            }
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }
                response = JsonConvert.DeserializeObject<Response>(result);
            }
            catch (System.Exception e)
            {


            }



            if (response != null)
            {
                return response.Result.Items;
            }
            else {  return null; }
        }

        public class Filter
        {
            [JsonProperty("offer_id")]
            public List<string> OfferId { get; set; }

            [JsonProperty("product_id")]
            public List<string> ProductId { get; set; }

            [JsonProperty("visibility")]
            public string Visibility { get; set; }
        }

        public class Request
        {
            [JsonProperty("filter")]
            public Filter Filter { get; set; }

            [JsonProperty("last_id")]
            public string LastId { get; set; }

            [JsonProperty("limit")]
            public int Limit { get; set; }
        }
        public class Item
        {
            [JsonProperty("archived")]
            public bool Archived { get; set; }

            [JsonProperty("has_fbo_stocks")]
            public bool HasFboStocks { get; set; }

            [JsonProperty("has_fbs_stocks")]
            public bool HasFbsStocks { get; set; }

            [JsonProperty("is_discounted")]
            public bool IsDiscounted { get; set; }

            [JsonProperty("offer_id")]
            public string OfferId { get; set; }

            [JsonProperty("product_id")]
            public long ProductId { get; set; }

            [JsonProperty("quants")]
            public List<Quant> Quants { get; set; }
        }

        public class Quant
        {
            [JsonProperty("quant_code")]
            public string QuantCode { get; set; }

            [JsonProperty("quant_size")]
            public int QuantSize { get; set; }
        }

        public class Result
        {
            [JsonProperty("items")]
            public List<Item> Items { get; set; }

            [JsonProperty("total")]
            public int Total { get; set; }

            [JsonProperty("last_id")]
            public string LastId { get; set; }
        }

        public class Response
        {
            [JsonProperty("result")]
            public Result Result { get; set; }
        }



    }

}


