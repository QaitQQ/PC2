using Newtonsoft.Json;

using StructLibCore.Marketplace;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post
{
    public class OzonPostAct : OzonPost
    {
        public OzonPostAct(APISetting aPISetting) : base(aPISetting)
        {
        }

        public string Get(List<IOrder> orders)
        {
            var httpWebRequest = GetRequest(@"v2/posting/fbs/act/create");
            httpWebRequest.Accept = "text/html,application/xhtml+xml,application/xml,application/pdf";

            List<string> Ids = new List<string>();


            foreach (var item in orders)
            {
                if (!Ids.Contains(((Server.Class.IntegrationSiteApi.Market.Ozon.OzonPortOrderList.Order)item).DeliveryMethod.Id.ToString()))
                {
                    Ids.Add(((Server.Class.IntegrationSiteApi.Market.Ozon.OzonPortOrderList.Order)item).DeliveryMethod.Id.ToString());
                }
            }

            var root = new Root() 
            {
                DeliveryMethodId = Ids[0]
            };

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
               var json = JsonConvert.SerializeObject(root); streamWriter.Write(json);
            }
            HttpWebResponse httpResponse;
            try
            {
                httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }
                ResultRoot rslt = JsonConvert.DeserializeObject<ResultRoot>(result);
                httpWebRequest = GetRequest(@"v2/posting/fbs/act/get-pdf");
                Result XR = new Result() { Id = rslt.Result.Id };
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    var json = JsonConvert.SerializeObject(XR); streamWriter.Write(json);
                }
                httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            }
            catch (Exception e)
            {
                return e.Message;
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
            //[JsonProperty("containers_count")]
            //public int ContainersCount;

            [JsonProperty("delivery_method_id")]
            public string DeliveryMethodId;

            //[JsonProperty("departure_date")]
            //public DateTime DepartureDate;
        }


        public class Result
        {
            [JsonProperty("id")]
            public int Id;
        }

        public class ResultRoot
        {
            [JsonProperty("result")]
            public Result Result;
        }

    }

    public class FileRoot
    {
        [JsonProperty("content")]
        public string Content;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("type")]
        public string Type;
    }


}
