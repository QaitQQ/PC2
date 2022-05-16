using Newtonsoft.Json;

using StructLibCore.Marketplace;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace SiteApi.IntegrationSiteApi.APIMarket.Yandex
{
    public  class YandexGetLabel : SiteApi.IntegrationSiteApi.APIMarket.Yandex.YandexApiClass
    {
        public YandexGetLabel(APISetting APISetting) : base(APISetting)
        {
        }

        public string Get(object Order)
        {

            Server.Class.IntegrationSiteApi.Market.Yandex.YandexGetItemOrders.YandexGetItemOrders.Order X = (Server.Class.IntegrationSiteApi.Market.Yandex.YandexGetItemOrders.YandexGetItemOrders.Order)Order;

            var httpWebRequest = GetRequest(@"/campaigns/" + ClientID + "/orders/"+ X.Id +"/delivery/labels.json", "GET");

            HttpWebResponse httpResponse;
            try
            {
                httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            }
            catch (Exception e )
            {

                return "none" + e.Message;
            }

            try
            {
                using (var str = File.Create("temp.pdf"))
                    httpResponse.GetResponseStream().CopyTo(str);
                return "temp.pdf";
            }
            catch
            {
                using (var str = File.Create("temp2.pdf"))
                    httpResponse.GetResponseStream().CopyTo(str);
                return "temp2.pdf";
            }
        }
    }
}
