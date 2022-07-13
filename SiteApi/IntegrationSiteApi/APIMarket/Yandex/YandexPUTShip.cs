using Newtonsoft.Json;

using StructLibCore.Marketplace;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

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

            var httpWebRequest = GetRequest(@"/campaigns/" + ClientID + "/orders/" + X.Id + "/status.json", "PUT");

            Root itemsRoot = new Root();


                itemsRoot.Order = new Order() { Status = "PROCESSING", Substatus = "READY_TO_SHIP" };
            
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                var Root = JsonConvert.SerializeObject(itemsRoot);
                streamWriter.Write(Root);
            }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            return true;
        }
        public class Order
        {
            [JsonProperty("status")]
            public string Status;

            [JsonProperty("substatus")]
            public string Substatus;
        }

        public class Root
        {
            [JsonProperty("order")]
            public Order Order;

        }

    }
}
