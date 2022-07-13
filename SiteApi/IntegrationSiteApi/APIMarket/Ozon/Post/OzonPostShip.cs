using Newtonsoft.Json;
using StructLibCore.Marketplace;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post
{
     public class OzonPostShip : Server.Class.IntegrationSiteApi.Market.Ozon.OzonPost.OzonPost

    {
        public OzonPostShip(APISetting aPISetting) : base(aPISetting)
        {
        }


        public bool Get(object Order)
        {
            var httpWebRequest = GetRequest(@"v3/posting/fbs/ship");

            var Or = (Server.Class.IntegrationSiteApi.Market.Ozon.OzonPortOrderList.Order)Order;

            var Products = new List<Product>();

            foreach (var item in Or.Products)
            {
                Products.Add(new Product() { ProductId = item.Sku, Quantity = item.Quantity, ExemplarInfo = new List<ExemplarInfo>() { new ExemplarInfo() { IsGtdAbsent = true } } });
            }
            var root = new Root() { PostingNumber = Or.PostingNumber, Packages = new List<Package>() { new Package() {Products = Products } } };
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
           {
               var json = JsonConvert.SerializeObject(root); streamWriter.Write(json);
            }
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }

                if (result.Contains(Or.PostingNumber))
                {
                    return true;
                }
            }
            catch 

            {

                return false;
            }
            return false;

        }




    }

    public class ExemplarInfo
    {
        [JsonProperty("gtd")]
        public string Gtd { get; set; }

        [JsonProperty("is_gtd_absent")]
        public bool IsGtdAbsent { get; set; }

        [JsonProperty("mandatory_mark")]
        public string MandatoryMark { get; set; }
    }

    public class Product
    {
        [JsonProperty("exemplar_info")]
        public List<ExemplarInfo> ExemplarInfo { get; set; }

        [JsonProperty("product_id")]
        public int ProductId { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }

    public class Package
    {
        [JsonProperty("products")]
        public List<Product> Products { get; set; }
    }

    public class Root
    {
        [JsonProperty("packages")]
        public List<Package> Packages { get; set; }

        [JsonProperty("posting_number")]
        public string PostingNumber { get; set; }
    }
}
