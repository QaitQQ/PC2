using Newtonsoft.Json;
using StructLibCore.Marketplace;
using System.IO;
using System.Net;
namespace SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post
{
    internal class OzonPostDesc : OzonPost
    {
        public OzonPostDesc(APISetting aPISetting) : base(aPISetting)
        {
        }
        public string Get(IMarketItem item)
        {
            HttpWebRequest httpWebRequest = GetRequest(@"v1/product/info/description");
            using (StreamWriter streamWriter = new(httpWebRequest.GetRequestStream())) { string root = JsonConvert.SerializeObject(new Root() { OfferId = item.SKU, ProductId = ((OzonItemDesc)item).id }); streamWriter.Write(root); }
            HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (StreamReader streamReader = new(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }
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
