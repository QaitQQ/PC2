using Newtonsoft.Json;

using SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post;

using System.Collections.Generic;
using System.IO;
using System.Net;

using static SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post.OzonPostWarehouseInfo;
namespace Server.Class.IntegrationSiteApi.Market.Ozon
{
    public class OzonPostSetPrice : SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post.OzonPost
    {
        public OzonPostSetPrice(StructLibCore.Marketplace.APISetting aPISetting) : base(aPISetting)
        {
        }
        public object Get(StructLibCore.Marketplace.IMarketItem[] List)
        {
            List<WarehouseResult> X = (List<WarehouseResult>)new OzonPostWarehouseInfo(aPISetting).Get();
            ProductsStocksRequestStockRoot itemsRoot = new ProductsStocksRequestStockRoot();
            foreach (OzonItemDesc item in List)
            {
                itemsRoot.Stocks.Add(new Stock(item, X[0].WarehouseId));
            }
            HttpWebRequest httpWebRequest = GetRequest(@"v2/products/stocks");
            using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string root = JsonConvert.SerializeObject(itemsRoot);
                streamWriter.Write(root);
            }
            try
            {
                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()); result = streamReader.ReadToEnd();
                return result;
            }
            catch
            {
                return false;
            }
        }
        public class ProductsStocksRequestStockRoot
        {
            public ProductsStocksRequestStockRoot()
            {
                Stocks = new List<Stock>();
            }
            [JsonProperty("stocks")]
            public List<Stock> Stocks { get; set; }
        }
        public class Stock
        {
            public Stock(OzonItemDesc itemDesc, string WarehouseId)
            {
                OfferId = itemDesc.offer_id;
                ProductId = itemDesc.id.ToString();
                StockCount = itemDesc.Stocks;
                this.WarehouseId = WarehouseId;
            }
            [JsonProperty("offer_id")]
            public string OfferId { get; set; }
            [JsonProperty("product_id")]
            public string ProductId { get; set; }
            [JsonProperty("stock")]
            public string StockCount { get; set; }
            [JsonProperty("warehouse_id")]
            public string WarehouseId { get; set; }
        }
    }
}
