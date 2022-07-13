using Newtonsoft.Json;

using Server.Class.IntegrationSiteApi.Market.Ozon.OzonPost;

using StructLibCore.Marketplace;

using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Server.Class.IntegrationSiteApi.Market.Ozon
{
    public class OzonGetItemDesc : Server.Class.IntegrationSiteApi.Market.Ozon.OzonPost.OzonPost
    {
        public OzonGetItemDesc(APISetting aPISetting) : base(aPISetting)
        {
        }

        public List<object> Get()
        {
            var httpWebRequest = GetRequest(@"v2/product/info/list");
            var Lst = new OzonGetItemList(this.aPISetting).Get();
            ItemQ itemQ = new ItemQ();
            foreach (var item in Lst) { itemQ.offer_id.Add(item.offer_id);}
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream())) { var root = JsonConvert.SerializeObject(itemQ); streamWriter.Write(root); }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }
            Root_D End = JsonConvert.DeserializeObject<Root_D>(result);
            List<object> NLST = new List<object>();
            foreach (var item in End.result.items) { NLST.Add(item); }
            return NLST;
        }
    }
}


