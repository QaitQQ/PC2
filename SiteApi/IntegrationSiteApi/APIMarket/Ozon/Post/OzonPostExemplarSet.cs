using Newtonsoft.Json;

using StructLibCore.Marketplace;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
namespace SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post
{
    public class OzonPostExemplarSet : OzonPost
    {
        public OzonPostExemplarSet(APISetting aPISetting) : base(aPISetting)
        {
        }
        public bool Get(object Order, List<OzonPostExemplarStatus.Product> products)
        {
            HttpWebRequest httpWebRequest = GetRequest(@"/v6/fbs/posting/product/exemplar/set");
            Server.Class.IntegrationSiteApi.Market.Ozon.OzonPortOrderList.Order Or = (Server.Class.IntegrationSiteApi.Market.Ozon.OzonPortOrderList.Order)Order;

            var Products = new List<Product>();
            foreach (var item in products)
            {
                var Product = new Product() { ProductId = (long)item.ProductId };
                Product.Exemplars = new List<Exemplar>();
                foreach (var ex in item.Exemplars)
                {
                    var Exemplar = new Exemplar();
                    Exemplar.Gtd = "";
                    Exemplar.ExemplarId = (long)ex.ExemplarId;
                    Exemplar.IsGtdAbsent = true;
                    Exemplar.IsRnptAbsent = true;
                    Product.Exemplars.Add(Exemplar);
                }

                Products.Add(Product);
            }


            Request root = new() { MultiBoxQty = 1, PostingNumber = Or.PostingNumber, Products = Products };
            using (StreamWriter streamWriter = new(httpWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(root); streamWriter.Write(json);
            }
            try
            {
                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }

            }
            catch (Exception e)
            {
                return false;
            }
            return false;
        }


        public class Exemplar
        {
            [JsonProperty("exemplar_id")]
            public long ExemplarId { get; set; }

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
            public string mark { get; set; }

            [JsonProperty("mark_type")]
            public string MarkType { get; set; }
        }

        public class Product
        {
            [JsonProperty("exemplars")]
            public List<Exemplar> Exemplars { get; set; }

            [JsonProperty("product_id")]
            public long ProductId { get; set; }
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
