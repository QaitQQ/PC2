using Newtonsoft.Json;

using StructLibCore.Marketplace;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post
{
    public class OzonPostLabel : OzonPost
    {
        public OzonPostLabel(APISetting aPISetting) : base(aPISetting)
        {
        }

        public string Get(object Order)
        {
            var httpWebRequest = GetRequest(@"v2/posting/fbs/package-label");
            httpWebRequest.Accept = "text/html,application/xhtml+xml,application/xml,application/pdf";

            var Or = (Server.Class.IntegrationSiteApi.Market.Ozon.OzonPortOrderList.Order)Order;

            var root = new Root() { PostingNumber = new List<string>() { Or.PostingNumber } };

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                var json = JsonConvert.SerializeObject(root); streamWriter.Write(json);
            }
            HttpWebResponse httpResponse = null;
            try
            {
                 httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            }
            catch (Exception e)
            {
                return "none " + e.Message;
            }
            try
            {
                using (var str = File.Create("temp.pdf"))
                httpResponse.GetResponseStream().CopyTo(str);
                return "temp.pdf";
            }
            catch 
            {
                using (var str = File.Create("temp2.pdf"))
                    httpResponse.GetResponseStream().CopyTo(str);
                return "temp2.pdf";
            }
        }

        public class Root
        {
            [JsonProperty("posting_number")]
            public List<string> PostingNumber { get; set; }
        }

        public class RootResult
        {
            [JsonProperty("content")]
            public string Content { get; set; }
        }
    }
}
