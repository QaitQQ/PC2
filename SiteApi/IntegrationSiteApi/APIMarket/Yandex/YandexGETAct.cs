using StructLibCore.Marketplace;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace SiteApi.IntegrationSiteApi.APIMarket.Yandex
{
    public class YandexGETAct : SiteApi.IntegrationSiteApi.APIMarket.Yandex.YandexApiClass
    {
        public YandexGETAct(APISetting APISetting) : base(APISetting)
        {
        }

        public string Get()
        {

          
            var httpWebRequest = GetRequest(@"/campaigns/" + ClientID + "/shipments/reception-transfer-act.json", "GET");

            HttpWebResponse httpResponse;
            try
            {
                httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            }
            catch(Exception e)
            {
                return "none"+e.Message;
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
