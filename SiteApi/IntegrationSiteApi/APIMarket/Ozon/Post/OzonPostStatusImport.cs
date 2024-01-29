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
    public class OzonPostStatusImport : SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post.OzonPost
    {
        public OzonPostStatusImport(APISetting aPISetting) : base(aPISetting)
        {
        }
        public Response Get(string TaskId)
        {
            var httpWebRequest = GetRequest(@"v1/product/import/info");

            var Request = new Request();
            Request.TaskId = TaskId;
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
                return null;
            }
            return Response;
        }

        public class Request
        {
            [JsonProperty("task_id")]
            public string TaskId { get; set; }
        }

        public class Item
        {
            [JsonProperty("offer_id")]
            public string OfferId { get; set; }

            [JsonProperty("product_id")]
            public int ProductId { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("errors")]
            public List<object> Errors { get; set; }
        }

        public class Response
        {
            [JsonProperty("items")]
            public List<Item> Items { get; set; }

            [JsonProperty("total")]
            public int Total { get; set; }
        }

        public class Root
        {
            [JsonProperty("result")]
            public Result Result { get; set; }
        }
    }

}


