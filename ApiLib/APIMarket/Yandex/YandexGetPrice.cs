using Newtonsoft.Json;

using StructLibCore.Marketplace;

using System.Net;

namespace Server.Class.IntegrationSiteApi.Market.Yandex.YandexGetPrice
{

    public class YandexGetPrice : SiteApi.IntegrationSiteApi.APIMarket.Yandex.YandexApiClass
    {
        public YandexGetPrice(APISetting APISetting) : base(APISetting)
        {
        }

        public List<object> Get()
        {
            HttpWebRequest httpWebRequest = GetRequest(@"/campaigns/" + ClientID + "/offer-prices.json", "GET");
            HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }
            Root root = JsonConvert.DeserializeObject<Root>(result!)!;
            List<object> Lst = new List<object>();
            foreach (YandexItem item in root?.result?.offers!)
            {
                Lst.Add(item);
            }
            return Lst;
        }
    }
    [Serializable]
    public class Paging
    {
        public string? nextPageToken { get; set; }
    }
    [Serializable]
    public class Price
    {
        public string? value { get; set; }
        public string? discountBase { get; set; }
        public string? currencyId { get; set; }
        public int vat { get; set; }
    }
    [Serializable]
    public class YandexItem : IMarketItem
    {
        public YandexItem()
        {
            APISetting = new APISetting();
            APISettingSource = new APISetting();
            Pic = new List<string>();
            Barcodes = new List<string>();
        }

        public string? id { get; set; }
        public Price? price { get; set; }
        public DateTime updatedAt { get; set; }
        public string? marketSku { get; set; }
        public string? Name { get; set; }
        public string? Price { get { return price?.value; } set { } }
        public string? SKU { get { return id; } set { marketSku = value; } }
        public string? Stocks { get; set; }
        public APISetting APISetting { get; set; }
        public string? MinPrice
        {
            get { return price?.discountBase; }
            set
            {
                if (price != null)
                {
                    price.discountBase = value;
                }
            }
        }
        public APISetting APISettingSource { get; set; }
        public List<string> Pic { get; set; }
        public List<string> Barcodes { get; set; }
    }
    [Serializable]
    public class Result
    {
        public int total { get; set; }
        public Paging? paging { get; set; }
        public List<YandexItem>? offers { get; set; }
    }
    [Serializable]
    public class Root
    {
        public string? status { get; set; }
        public Result? result { get; set; }
    }
}
