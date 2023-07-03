using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SiteApi.IntegrationSiteApi.APIMarket.Yandex
{
    public class YandexApiClass
    {
        internal string ClientID;
        internal string oauth_token;
        internal string add_ip;
        internal string warehouseId;
        internal StructLibCore.Marketplace.APISetting aPISetting;
        internal string result;

        public YandexApiClass(StructLibCore.Marketplace.APISetting APISetting)
        {
            aPISetting = APISetting;
            warehouseId = APISetting.ApiString[3];
            ClientID = APISetting.ApiString[2];
            oauth_token = APISetting.ApiString[1];
            add_ip = APISetting.ApiString[0];
        }

        internal HttpWebRequest GetRequest(string Url, string Method)
        {
            string url = @"https://api.partner.market.yandex.ru/" + Url;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            httpWebRequest.Method = Method;
            httpWebRequest.ContentType = "application/json ";
            httpWebRequest.Headers.Add("Authorization", "OAuth oauth_token = \"" + oauth_token + "\", oauth_client_id = \"" + add_ip + "\"");
            return httpWebRequest;
        }

    }
}
