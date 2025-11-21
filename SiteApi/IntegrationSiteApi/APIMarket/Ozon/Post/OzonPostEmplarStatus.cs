using Newtonsoft.Json;

using StructLibCore.Marketplace;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
namespace SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post
{
    public class OzonPostExemplarStatus : OzonPost
    {
        public OzonPostExemplarStatus(APISetting aPISetting) : base(aPISetting)
        {
        }
        public List<Product> Get(object Order)
        {
            HttpWebRequest httpWebRequest = GetRequest(@"v5/fbs/posting/product/exemplar/status");
            Server.Class.IntegrationSiteApi.Market.Ozon.OzonPortOrderList.Order Or = (Server.Class.IntegrationSiteApi.Market.Ozon.OzonPortOrderList.Order)Order;





            OzonPostEmplarStatus_Request root = new() { PostingNumber = Or.PostingNumber};
            using (StreamWriter streamWriter = new(httpWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(root); streamWriter.Write(json);
            }
            try
            {
                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }

                Response Response = JsonConvert.DeserializeObject<Response>(result);


                if (Response.Status == "ship_not_available")
                {
                    return Response.Products;
                }
                
            }
            catch(Exception e)
            {
                return null;
            }
            return null;
        }
        public class Exemplar
        {
            [JsonProperty("exemplar_id")]
            public object ExemplarId { get; set; }

            [JsonProperty("gtd")]
            public string Gtd { get; set; }

            [JsonProperty("gtd_check_status")]
            public string GtdCheckStatus { get; set; }

            [JsonProperty("gtd_error_codes")]
            public List<object> GtdErrorCodes { get; set; }

            [JsonProperty("is_gtd_absent")]
            public bool IsGtdAbsent { get; set; }

            [JsonProperty("is_rnpt_absent")]
            public bool IsRnptAbsent { get; set; }

            [JsonProperty("marks")]
            public List<object> Marks { get; set; }

            [JsonProperty("rnpt")]
            public string Rnpt { get; set; }

            [JsonProperty("rnpt_check_status")]
            public string RnptCheckStatus { get; set; }

            [JsonProperty("rnpt_error_codes")]
            public List<object> RnptErrorCodes { get; set; }
        }

        public class Product
        {
            [JsonProperty("exemplars")]
            public List<Exemplar> Exemplars { get; set; }

            [JsonProperty("product_id")]
            public int ProductId { get; set; }
        }

        public class Response
        {
            [JsonProperty("posting_number")]
            public string PostingNumber { get; set; }

            [JsonProperty("products")]
            public List<Product> Products { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }
        }





    }
    public class OzonPostEmplarStatus_Request
    {
        [JsonProperty("posting_number")]
        public string PostingNumber { get; set; }
    }

}
