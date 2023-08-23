using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SiteApi.IntegrationSiteApi.ApiMainSite
{
    public abstract class MainSiteV1Abstract
    {

        internal string Token;
        internal string SiteUrl;
        internal string Method;
        internal string result;
        internal MainSiteV1Abstract(string Token, string SiteUrl)
        {
            this.Token = Token;
            this.SiteUrl = SiteUrl;
        }
        internal  HttpWebRequest GetRequest(string Url)
        {
            string url = @SiteUrl + Url;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = Method;

           // httpWebRequest.Headers.Add("api_token:" + Token);
            return httpWebRequest;
        }
    }
}
