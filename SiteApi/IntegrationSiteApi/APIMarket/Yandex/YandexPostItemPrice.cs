using Newtonsoft.Json;

using StructLibCore;
using StructLibCore.Marketplace;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Server.Class.IntegrationSiteApi.Market.Yandex.YandexPostItemPrice
{
   public class YandexPostItemPrice: SiteApi.IntegrationSiteApi.APIMarket.Yandex.YandexApiClass
    {
        public YandexPostItemPrice(APISetting APISetting) : base(APISetting) {}

        public List<object> Get(StructLibCore.Marketplace.IMarketItem[] List)
        {
            var httpWebRequest = GetRequest(@"/campaigns/" + ClientID + "/offer-prices/updates.json", "POST");
            Root itemsRoot = new Root();
            foreach (Server.Class.IntegrationSiteApi.Market.Yandex.YandexGetName.YandexGetItemList.ItemYandex item in List)
            {
                itemsRoot.offers.Add(new ItemPostPrice(item));
            }
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                var Root = JsonConvert.SerializeObject(itemsRoot);
                streamWriter.Write(Root);
            }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            string result;

            using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }

            return new List<object>();
        }
    }
    [Serializable]
    public class ItemPostPrice
    {
        public ItemPostPrice(Server.Class.IntegrationSiteApi.Market.Yandex.YandexGetName.YandexGetItemList.ItemYandex Item)
        {
            id = Item.SKU;
            price = Item.Yprice;
            price.value = Item.Price;
        }
        public String id { get; set; }
        public Server.Class.IntegrationSiteApi.Market.Yandex.YandexGetPrice.Price price { get; set; }
    }
    [Serializable]
    public class Root
    {
        public Root() { offers = new List<ItemPostPrice>(); }
        public List<ItemPostPrice> offers { get; set; }
    }

}
