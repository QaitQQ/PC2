using Newtonsoft.Json;

using Server.Class.IntegrationSiteApi.Market.Ozon.OzonPost;

using StructLibCore.Marketplace;

using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Server.Class.IntegrationSiteApi.Market.Ozon
{
    public class OzonGetItemList : Server.Class.IntegrationSiteApi.Market.Ozon.OzonPost.OzonPost
    {
        public OzonGetItemList(APISetting aPISetting) : base(aPISetting)
        {
        }

        public List<Item> Get()
        {
            var httpWebRequest = GetRequest(@"v1/product/list");

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"page\": \"1\",\"page_size\": \"1000\"}";
                streamWriter.Write(json);
            }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }
            Root root = JsonConvert.DeserializeObject<Root>(result);

            return root.result.items;
        }
    }

}


