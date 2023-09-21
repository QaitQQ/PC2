using Newtonsoft.Json;

using SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post;

using StructLibCore.Marketplace;

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
            HttpWebRequest httpWebRequest = GetRequest(@"v2/product/list");

            using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"page\": \"1\",\"page_size\": \"1000\"}";
                streamWriter.Write(json);
            }
            HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }

            if (result != null)
            {

                Root root = JsonConvert.DeserializeObject<Root>(result)!;
                return root?.result?.items!;
            }
            return null!;



        }
    }

}


