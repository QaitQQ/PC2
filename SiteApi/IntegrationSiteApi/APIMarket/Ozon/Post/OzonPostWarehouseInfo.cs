using Newtonsoft.Json;

using StructLibCore.Marketplace;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post
{
    class OzonPostWarehouseInfo : Server.Class.IntegrationSiteApi.Market.Ozon.OzonPost.OzonPost
    {
        public OzonPostWarehouseInfo(APISetting aPISetting) : base(aPISetting) {  }
        public object Get()
        {
            var httpWebRequest = GetRequest(@"v1/warehouse/list");
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }
                return (JsonConvert.DeserializeObject<OzonPostWarehouseInfoRoot>(result)).Result;
            }
            catch  { return null; }
        }

        public class WarehouseResult
        {
            [JsonProperty("warehouse_id")]
            public string WarehouseId { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("is_rfbs")]
            public bool IsRfbs { get; set; }
        }

        public class OzonPostWarehouseInfoRoot
        {
            [JsonProperty("result")]
            public List<WarehouseResult> Result { get; set; }
        }

    }
}
