using Newtonsoft.Json;

using StructLibCore.Marketplace;

using System.Net;
namespace Server.Class.IntegrationSiteApi.Market.Yandex.YandexGetName
{
    public class YandexGetItemList : SiteApi.IntegrationSiteApi.APIMarket.Yandex.YandexApiClass
    {
        public YandexGetItemList(APISetting APISetting) : base(APISetting) { }
        public List<IMarketItem> Get()
        {
            HttpWebRequest httpWebRequest = GetRequest(@"/campaigns/" + ClientID + "/offer-mapping-entries.json?limit=200", "GET");
            HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }
            List<IMarketItem> Lst = new List<IMarketItem>();
            if (result != null)
            {
                Root root = JsonConvert.DeserializeObject<Root>(result)!;
                List<object> Prices = new YandexGetPrice.YandexGetPrice(aPISetting!).Get();
                List<object> Stocks = new YandexGetPrice.YandexPostStocks(aPISetting!).Get(Prices);
                Mapped(root, Lst, Prices, Stocks);
                if (root.result?.paging?.nextPageToken != null)
                {
                    httpWebRequest = GetRequest(@"/campaigns/" + ClientID + "/offer-mapping-entries.json?page_token=" + root.result.paging.nextPageToken, "GET");
                    httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }
                    if (result != null)
                    {
                        root = JsonConvert.DeserializeObject<Root>(result)!;
                    }
                    Mapped(root, Lst, Prices, Stocks);
                }
            }
            return Lst;
        }
        private static void Mapped(Root root, List<IMarketItem> Lst, List<object> Prises, List<object> Stocks)
        {
            foreach (OfferMappingEntry item in root?.result?.offerMappingEntries!)
            {
                IEnumerable<object> z = from prise in Prises where ((IMarketItem)prise).SKU == item.offer?.SKU select prise;
                if (z.Count() > 0)
                {
                    if (item.offer != null)
                    {
                        item.offer.Price = ((IMarketItem)z.First()).Price;
                        item.offer.Yprice = ((YandexGetPrice.YandexItem)z.First()).price;
                    }

                }
                IEnumerable<object> o = from Stock in Stocks where ((KeyValuePair<string, string>)Stock).Key == item.offer?.SKU select Stock;
                if (o.Count() > 0)
                {
                    if (item.offer != null)
                    {
                        item.offer.Stocks = ((KeyValuePair<string, string>)o.First()).Value;
                    }

                }
                Lst.Add(item.offer!);
            }
        }
        [Serializable]
        public class Paging
        {
            public string? nextPageToken { get; set; }
        }
        [Serializable]
        public class WeightDimensions
        {
            public double length { get; set; }
            public double width { get; set; }
            public double height { get; set; }
            public double weight { get; set; }
        }
        [Serializable]
        public class ProcessingState
        {
            public string? status { get; set; }
        }
        [Serializable]
        public class GuaranteePeriod
        {
            public int timePeriod { get; set; }
            public string? timeUnit { get; set; }
        }
        [Serializable]
        public class ItemYandex : IMarketItem
        {
            public ItemYandex()
            {
                APISetting = new APISetting();
                APISettingSource = new APISetting();
            }

            public string? name { get; set; }
            public string? shopSku { get; set; }
            public string? category { get; set; }
            public string? vendor { get; set; }
            public string? description { get; set; }
            public List<string>? barcodes { get; set; }
            public List<string>? pictures { get; set; }
            public WeightDimensions? weightDimensions { get; set; }
            public List<string>? supplyScheduleDays { get; set; }
            public ProcessingState? processingState { get; set; }
            public string? availability { get; set; }
            public List<string>? urls { get; set; }
            public string? manufacturer { get; set; }
            public List<string>? manufacturerCountries { get; set; }
            public int? guaranteePeriodDays { get; set; }
            public GuaranteePeriod? guaranteePeriod { get; set; }
            public string? Name { get => name; set => name = value; }
            public string? Price { get; set; }
            public string? SKU { get => shopSku; set => shopSku = value; }
            public string? Stocks { get; set; }
            public APISetting APISetting { get; set; }
            public Server.Class.IntegrationSiteApi.Market.Yandex.YandexGetPrice.Price? Yprice { get; set; }
            public string? MinPrice { get; set; }
            public APISetting APISettingSource { get; set; }
            public List<string> Pic { get { return pictures!; } set { pictures = value; } }
            public List<string> Barcodes { get { return barcodes != null ? barcodes : new List<string>(); } set => barcodes = value; }
        }
        [Serializable]
        public class Mapping
        {
            public object? marketSku { get; set; }
            public int categoryId { get; set; }
        }
        [Serializable]
        public class AwaitingModerationMapping
        {
            public string? marketSku { get; set; }
            public string? categoryId { get; set; }
        }
        [Serializable]
        public class OfferMappingEntry
        {
            public ItemYandex? offer { get; set; }
            public Mapping? mapping { get; set; }
            public AwaitingModerationMapping? awaitingModerationMapping { get; set; }
        }
        [Serializable]
        public class Result
        {
            public Paging? paging { get; set; }
            public List<OfferMappingEntry>? offerMappingEntries { get; set; }
        }
        [Serializable]
        public class Root
        {
            public string? status { get; set; }
            public Result? result { get; set; }
        }
    }
}
