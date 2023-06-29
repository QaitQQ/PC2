using Newtonsoft.Json;

using StructLibCore.Marketplace;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
namespace SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post.OzonGetDesc
{
    class OzonPostDesc : OzonPost
    {
        public OzonPostDesc(APISetting aPISetting) : base(aPISetting)
        {
        }
        public string Get(IMarketItem item)
        {
            var httpWebRequest = GetRequest(@"v1/product/info/description");
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream())) { var root = JsonConvert.SerializeObject(new Root() { OfferId = item.SKU, ProductId = ((OzonItemDesc)item).id }); streamWriter.Write(root); }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }
            RootResult End = JsonConvert.DeserializeObject<RootResult>(result);
            return End.result.Description;
        }
        internal class Root
        {
            [JsonProperty("offer_id")]
            public string OfferId { get; set; }
            [JsonProperty("product_id")]
            public int ProductId { get; set; }
        }
        internal class Result
        {
            [JsonProperty("id")]
            public int Id { get; set; }
            [JsonProperty("offer_id")]
            public string Offer_id { get; set; }
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("description")]
            public string Description { get; set; }
        }
        internal class RootResult
        {
            public Result result { get; set; }
        }
    }
}
