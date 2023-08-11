using Newtonsoft.Json;

using StructLibCore;
using StructLibCore.Marketplace;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace SiteApi.IntegrationSiteApi.APIMarket.Yandex
{
    public class YandexGetOrder : SiteApi.IntegrationSiteApi.APIMarket.Yandex.YandexApiClass
    {
        public YandexGetOrder(APISetting APISetting) : base(APISetting)
        {
        }
        public List<object> Get(string Id)
        {
            var httpWebRequest = GetRequest(@"/campaigns/" + ClientID + "/orders/"+ Id + ".json", "GET");
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }
                // Root root = JsonConvert.DeserializeObject<Root>(result);
                var Lst = new List<object>();
                return Lst;
            }
            catch
            {

                return null;
            }

        }
    }
}
