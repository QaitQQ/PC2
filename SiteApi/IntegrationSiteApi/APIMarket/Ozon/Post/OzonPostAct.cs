using Newtonsoft.Json;
using StructLibCore.Marketplace;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
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
            var parsedDate = DateTime.Parse(orders[0].ShipmentDate);
            var formData = $"{parsedDate:O}";
            formData = formData.Replace("0000000", "444Z");
            var root = new Root()
            {
                DeliveryMethodId = Ids[0],
             //   ContainersCount = 1,
                DepartureDate = formData,
            };
            using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(root); streamWriter.Write(json);
            }
            HttpWebResponse httpResponse;
            try
            {
                httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }
                ResultRoot rslt = JsonConvert.DeserializeObject<ResultRoot>(result);
                httpWebRequest = GetRequest(@"v2/posting/fbs/act/check-status");
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
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }
            var RS =  JsonConvert.DeserializeObject<Response>(result);
            if (RS.Result.Status == "ready")
            {
                return "true";
            }
            return "false";
        }
        public class Root
        {
         //   [JsonProperty("containers_count")]
        //    public int ContainersCount { get; set; }
            [JsonProperty("delivery_method_id")]
            public string DeliveryMethodId { get; set; }
            [JsonProperty("departure_date")]
            public string DepartureDate { get; set; }
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
    public class ResponseD
    {
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("added_to_act")]
        public List<string> AddedToAct { get; set; }
        [JsonProperty("removed_from_act")]
        public List<object> RemovedFromAct { get; set; }
        [JsonProperty("act_type")]
        public string ActType { get; set; }
        [JsonProperty("is_partial")]
        public bool IsPartial { get; set; }
        [JsonProperty("has_postings_for_next_carriage")]
        public bool HasPostingsForNextCarriage { get; set; }
        [JsonProperty("partial_num")]
        public int PartialNum { get; set; }
    }
    public class Response
    {
        [JsonProperty("result")]
        public ResponseD Result { get; set; }
    }
}
