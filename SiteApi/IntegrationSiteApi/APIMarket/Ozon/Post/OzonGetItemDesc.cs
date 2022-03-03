using Newtonsoft.Json;

using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Server.Class.IntegrationSiteApi.Market.Ozon
{
    public class OzonGetItemDesc : OzonPost
    {
        public OzonGetItemDesc(string ClientID, string apiKey) : base(ClientID, apiKey) {}
        public List<object> Get()
        {
            var httpWebRequest = GetRequest(@"v2/product/info/list");
            var Lst = new OzonGetItemList(ClientID, apiKey).Get();
            ItemQ itemQ = new ItemQ();
            foreach (var item in Lst) { itemQ.offer_id.Add(item.offer_id); }
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


