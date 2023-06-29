using StructLibCore.Marketplace;

using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SiteApi.IntegrationSiteApi.APIMarket.Ozon.Get
{
    abstract public class OzonGet:IMarketApi
    {
        internal string ClientID;
        internal string apiKey;
        internal string result;
        internal APISetting aPISetting;

        public OzonGet(APISetting aPISetting)
        {
            this.aPISetting = aPISetting;
            this.ClientID = aPISetting.ApiString[0];
            this.apiKey = aPISetting.ApiString[1];
        }
        internal HttpWebRequest GetRequest(string Url)
        {
            string url = @"https://api-seller.ozon.ru/" + Url;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";
            httpWebRequest.Headers.Add("Host: api-seller.ozon.ru");
            httpWebRequest.Headers.Add("Client-Id:" + ClientID);
            httpWebRequest.Headers.Add("Api-Key:" + apiKey);
            return httpWebRequest;
        }

    }
}
