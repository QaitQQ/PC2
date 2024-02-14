using Newtonsoft.Json;

using StructLibCore.Marketplace;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
namespace SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post
{
    public class OzonPostShip : OzonPost
    {
        public OzonPostShip(APISetting aPISetting) : base(aPISetting)
        {
        }
        public bool Get(object Order)
        {
            HttpWebRequest httpWebRequest = GetRequest(@"v4/posting/fbs/ship");
            Server.Class.IntegrationSiteApi.Market.Ozon.OzonPortOrderList.Order Or = (Server.Class.IntegrationSiteApi.Market.Ozon.OzonPortOrderList.Order)Order;
            List<Product> Products = new();


  

            foreach (Server.Class.IntegrationSiteApi.Market.Ozon.OzonPortOrderList.Product item in Or.Products)
            {
                Products.Add(new Product() { ProductId = item.Sku, Quantity = item.Quantity});
            }
            Request root = new() { PostingNumber = Or.PostingNumber, Packages = new List<Package>() { new Package() { Products = Products } }, With = new With() { AdditionalData = true } };
            using (StreamWriter streamWriter = new(httpWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(root); streamWriter.Write(json);
            }
            try
            {
                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }
                if (result.Contains(Or.PostingNumber))
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
    }
    public class Package
    {
        [JsonProperty("products")]
        public List<Product> Products { get; set; }
    }

    public class Product
    {
        [JsonProperty("product_id")]
        public int ProductId { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }

    public class Request
    {
        [JsonProperty("packages")]
        public List<Package> Packages { get; set; }

        [JsonProperty("posting_number")]
        public string PostingNumber { get; set; }

        [JsonProperty("with")]
        public With With { get; set; }
    }
    public class With
    {
        [JsonProperty("additional_data")]
        public bool AdditionalData { get; set; }
    }
}
