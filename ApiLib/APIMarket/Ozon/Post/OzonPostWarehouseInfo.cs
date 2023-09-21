using Newtonsoft.Json;

using StructLibCore.Marketplace;

using System.Net;

namespace SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post
{
    class OzonPostWarehouseInfo : OzonPost
    {
        public OzonPostWarehouseInfo(APISetting aPISetting) : base(aPISetting) { }
        public object? Get()
        {
            HttpWebRequest httpWebRequest = GetRequest(@"v1/warehouse/list");
            try
            {
                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }
                return (JsonConvert.DeserializeObject<OzonPostWarehouseInfoRoot>(result!)!).Result;
            }
            catch { return null; }
        }

        public class WarehouseResult
        {
            [JsonProperty("warehouse_id")]
            public string? WarehouseId { get; set; }

            [JsonProperty("name")]
            public string? Name { get; set; }

            [JsonProperty("is_rfbs")]
            public bool? IsRfbs { get; set; }
        }

        public class OzonPostWarehouseInfoRoot
        {
            [JsonProperty("result")]
            public List<WarehouseResult>? Result { get; set; }
        }

    }
}
