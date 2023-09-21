using Newtonsoft.Json;

using StructLibCore.Marketplace;

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
            HttpWebRequest httpWebRequest = GetRequest(@"v3/posting/fbs/ship");
            Server.Class.IntegrationSiteApi.Market.Ozon.OzonPortOrderList.Order Or = (Server.Class.IntegrationSiteApi.Market.Ozon.OzonPortOrderList.Order)Order;
            List<Product> Products = new();




            foreach (Server.Class.IntegrationSiteApi.Market.Ozon.OzonPortOrderList.Product item in Or.Products!)
            {
                List<ExemplarInfo> exemplarInfoList = new List<ExemplarInfo>();

                for (int i = 0; i < item.Quantity; i++)
                {
                    exemplarInfoList.Add(new ExemplarInfo() { IsGtdAbsent = true, IsRnptAbsent = true });
                }


                Products.Add(new Product() { ProductId = item.Sku, Quantity = item.Quantity, ExemplarInfo = exemplarInfoList });
            }
            RootT root = new() { PostingNumber = Or.PostingNumber, Packages = new List<Package>() { new Package() { Products = Products } } };
            using (StreamWriter streamWriter = new(httpWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(root); streamWriter.Write(json);
            }
            try
            {
                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }
                if (result.Contains(Or.PostingNumber!))
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
        public string? Gtd { get; set; }
        [JsonProperty("is_gtd_absent")]
        public bool IsGtdAbsent { get; set; }

        [JsonProperty("is_rnpt_absent")]
        public bool IsRnptAbsent { get; set; }

        [JsonProperty("mandatory_mark")]
        public string? MandatoryMark { get; set; }
    }
    public class Product
    {
        [JsonProperty("exemplar_info")]
        public List<ExemplarInfo>? ExemplarInfo { get; set; }
        [JsonProperty("product_id")]
        public int ProductId { get; set; }
        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }
    public class Package
    {
        [JsonProperty("products")]
        public List<Product>? Products { get; set; }
    }
    public class RootT
    {
        [JsonProperty("packages")]
        public List<Package>? Packages { get; set; }
        [JsonProperty("posting_number")]
        public string? PostingNumber { get; set; }
    }
}
