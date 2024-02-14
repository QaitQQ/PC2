using Newtonsoft.Json;
using SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post;
using StructLibCore.Marketplace;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
namespace Server.Class.IntegrationSiteApi.Market.Ozon
{
    public class OzonPostItemToArchive : SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post.OzonPost
    {
        public OzonPostItemToArchive(APISetting aPISetting) : base(aPISetting){}
        public bool Get(List<string> Ids)
        {
            var httpWebRequest = GetRequest(@"v1/product/archive");
            var Request = new Request();
            foreach (var item in Ids) { Request.ProductId.Add(item); }
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(Request);
                streamWriter.Write(json);
            }
            Response Response = null;
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }
                Response = JsonConvert.DeserializeObject<Response>(result);
            }
            catch (System.Exception e)
            {
                return false;
            }
            return true;
        }

        public class Request
        {
            [JsonProperty("product_id")]
            public List<string> ProductId;

            public Request()
            {
                ProductId = new List<string>();
            }
        }
        public class Response
        {
            [JsonProperty("result")]
            public bool? Result;
        }
    }
}
