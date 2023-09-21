using StructLibCore.Marketplace;

using System.Net;

namespace SiteApi.IntegrationSiteApi.APIMarket.Yandex
{
    public class YandexGetLabel : SiteApi.IntegrationSiteApi.APIMarket.Yandex.YandexApiClass
    {
        public YandexGetLabel(APISetting APISetting) : base(APISetting)
        {
        }

        public string Get(object Order)
        {

            Server.Class.IntegrationSiteApi.Market.Yandex.YandexGetItemOrders.YandexGetItemOrders.Order X = (Server.Class.IntegrationSiteApi.Market.Yandex.YandexGetItemOrders.YandexGetItemOrders.Order)Order;

            HttpWebRequest httpWebRequest = GetRequest(@"/campaigns/" + ClientID + "/orders/" + X.Id + "/delivery/labels.json", "GET");

            HttpWebResponse httpResponse;
            try
            {
                httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            }
            catch (Exception e)
            {

                return "none" + e.Message;
            }

            try
            {
                using FileStream str = File.Create("temp.pdf");
                httpResponse.GetResponseStream().CopyTo(str);
                return "temp.pdf";
            }
            catch
            {
                using FileStream str = File.Create("temp2.pdf");
                httpResponse.GetResponseStream().CopyTo(str);
                return "temp2.pdf";
            }
        }
    }
}
