using Newtonsoft.Json;
using StructLibCore.Marketplace;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
namespace SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post
{
    public class OzonPostEmplarSet : OzonPost
    {
        public OzonPostEmplarSet(APISetting aPISetting) : base(aPISetting)
        {
        }
        public bool Get(object Order)
        {
            HttpWebRequest httpWebRequest = GetRequest(@"/v6/fbs/posting/product/exemplar/set");
            Server.Class.IntegrationSiteApi.Market.Ozon.OzonPortOrderList.Order Or = (Server.Class.IntegrationSiteApi.Market.Ozon.OzonPortOrderList.Order)Order;


            foreach (var item in Or.p)
            {

            }


            Request root = new() { PostingNumber = Or.PostingNumber, Products = new List<Product>};
            using (StreamWriter streamWriter = new(httpWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(root); streamWriter.Write(json);
            }
            try
            {
                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }

                Response OrderList = JsonConvert.DeserializeObject<Response>(result);


                if (OrderList.Status == "ship_not_available")
                {
                    return true;
                }
                
            }
            catch(Exception e)
            {
                return false;
            }
            return false;
        }


        public class Exemplar
        {
            [JsonProperty("exemplar_id")]
            public int ExemplarId { get; set; }

            [JsonProperty("gtd")]
            public string Gtd { get; set; }

            [JsonProperty("is_gtd_absent")]
            public bool IsGtdAbsent { get; set; }

            [JsonProperty("is_rnpt_absent")]
            public bool IsRnptAbsent { get; set; }

            [JsonProperty("marks")]
            public List<Mark> Marks { get; set; }

            [JsonProperty("rnpt")]
            public string Rnpt { get; set; }
        }

        public class Mark
        {
            [JsonProperty("mark")]
            public string Mark { get; set; }

            [JsonProperty("mark_type")]
            public string MarkType { get; set; }
        }

        public class Product
        {
            [JsonProperty("exemplars")]
            public List<Exemplar> Exemplars { get; set; }

            [JsonProperty("product_id")]
            public int ProductId { get; set; }
        }

        public class Request
        {
            [JsonProperty("multi_box_qty")]
            public int MultiBoxQty { get; set; }

            [JsonProperty("posting_number")]
            public string PostingNumber { get; set; }

            [JsonProperty("products")]
            public List<Product> Products { get; set; }
        }


    }
}
