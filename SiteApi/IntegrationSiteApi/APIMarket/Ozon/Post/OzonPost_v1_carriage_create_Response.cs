using Newtonsoft.Json;
using StructLibCore.Marketplace;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
namespace SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post
{
    public class OzonPost_v1_carriage_create : OzonPost
    {
        public OzonPost_v1_carriage_create(APISetting aPISetting) : base(aPISetting)
        {
        }
        public string Get(List<IOrder> orders)
        {
            var httpWebRequest = GetRequest(@"v1/carriage/create");

            OzonPost_v1_carriage_create_Request Request = new ();

            var parsedDate = DateTime.Parse(orders[0].ShipmentDate);

            var Date = DateTime.Today;

            string formData = $"{Date:O}";
            formData = formData.Replace("0000000", "44Z");
            formData = formData.Replace("+03:00", "");
            Request.DepartureDate = formData;
            Request.AllBlrTraceable = false;


            var Methods = new OzonPost_delivery_method(aPISetting).Get();


            foreach (var item in Methods)
            {
                if (item.Status == "ACTIVE" )
                {
                    Request.DeliveryMethodId = item.Id;
                    break;
                }
            }



            try
            {
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    var root = JsonConvert.SerializeObject(Request);
                    streamWriter.Write(root);
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }
                Response_CarriageId response = JsonConvert.DeserializeObject<Response_CarriageId>(result);

                 new OzonPost_carriage_approve(aPISetting).Get(response.CarriageId);



            }
            catch (System.Exception e)
            {
             

            }


            return result;




        }

        public class Response_CarriageId
        {
            [JsonProperty("carriage_id")]
            public long CarriageId { get; set; }
        }

        public class OzonPost_v1_carriage_create_Request
        {
            [JsonProperty("all_blr_traceable")]
            public bool AllBlrTraceable { get; set; }

            [JsonProperty("delivery_method_id")]
            public long DeliveryMethodId { get; set; }

            [JsonProperty("departure_date")]
            public string DepartureDate { get; set; }
        }

    }
}
