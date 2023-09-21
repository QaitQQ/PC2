using Newtonsoft.Json;

using StructLibCore.Marketplace;

using System.Net;
namespace SiteApi.IntegrationSiteApi.APIMarket.Yandex.YandexPUTStocks
{
    public class YandexPUTStocks : SiteApi.IntegrationSiteApi.APIMarket.Yandex.YandexApiClass
    {
        public YandexPUTStocks(APISetting APISetting) : base(APISetting)
        {
        }
        public bool Get(IMarketItem[] itemList)
        {
            HttpWebRequest httpWebRequest = GetRequest(@"/campaigns/" + ClientID + "/offers/stocks.json", "PUT");
            Root itemsRoot = new Root();
            if (aPISetting != null)
            {
                foreach (IMarketItem item in itemList)
                {
                    itemsRoot.skus?.Add(new Sku() { sku = item.SKU, warehouseId = aPISetting.ApiString?[3]!, items = new List<Item> { new Item() { count = item.Stocks, type = "FIT", updatedAt = DateTime.Now } } });
                }
            }

            using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string Root = JsonConvert.SerializeObject(itemsRoot);
                streamWriter.Write(Root);
            }
            try
            {
                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream());
                result = streamReader.ReadToEnd(); return true;
            }
            catch
            {
                return false;
            }
        }
        public class Item
        {
            public string? count { get; set; }
            public string? type { get; set; }
            public DateTime updatedAt { get; set; }
        }
        public class Root
        {
            public Root()
            {
                this.skus = new List<Sku>();
            }
            public List<Sku>? skus { get; set; }
        }
        public class Sku
        {
            public string? sku { get; set; }
            public string? warehouseId { get; set; }
            public List<Item>? items { get; set; }
        }
    }
}
