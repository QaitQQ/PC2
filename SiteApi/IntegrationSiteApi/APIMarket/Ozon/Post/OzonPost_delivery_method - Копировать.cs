using Newtonsoft.Json;
using StructLibCore.Marketplace;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
namespace SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post
{
    public class OzonPost_carriage_approve : OzonPost
    {
        public OzonPost_carriage_approve(APISetting aPISetting) : base(aPISetting)
        {
        }
        public void Get(long id)
        {
            var httpWebRequest = GetRequest(@"v1/carriage/approve");

            Request Request = new();
            Request.CarriageId = id;

            try
            {
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    var root = JsonConvert.SerializeObject(Request);
                    streamWriter.Write(root);
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }
                //response = JsonConvert.DeserializeObject<Response>(result);

            }
            catch (System.Exception e)
            { 
            }
      
        }
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Request
        {
            [JsonProperty("carriage_id")]
            public long CarriageId { get; set; }

        }


    }
}
