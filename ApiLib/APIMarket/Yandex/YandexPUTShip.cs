using Newtonsoft.Json;

using StructLibCore.Marketplace;

using System.Net;

namespace SiteApi.IntegrationSiteApi.APIMarket.Yandex
{
    public class YandexPUTShip : SiteApi.IntegrationSiteApi.APIMarket.Yandex.YandexApiClass
    {
        public YandexPUTShip(APISetting APISetting) : base(APISetting)
        {
        }

        public bool Get(object Order)
        {

            Server.Class.IntegrationSiteApi.Market.Yandex.YandexGetItemOrders.YandexGetItemOrders.Order X = (Server.Class.IntegrationSiteApi.Market.Yandex.YandexGetItemOrders.YandexGetItemOrders.Order)Order;

            HttpWebRequest httpWebRequest = GetRequest(@"/campaigns/" + ClientID + "/orders/" + X.Id + "/status.json", "PUT");

            Root itemsRoot = new Root
            {
                Order = new Order() { Status = "PROCESSING", Substatus = "READY_TO_SHIP" }
            };

            using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string Root = JsonConvert.SerializeObject(itemsRoot);
                streamWriter.Write(Root);
            }

            _ = (HttpWebResponse)httpWebRequest.GetResponse();
            return true;
        }
        public class Order
        {
            [JsonProperty("status")]
            public string? Status;

            [JsonProperty("substatus")]
            public string? Substatus;
        }

        public class Root
        {
            [JsonProperty("order")]
            public Order? Order;

        }

    }
}
