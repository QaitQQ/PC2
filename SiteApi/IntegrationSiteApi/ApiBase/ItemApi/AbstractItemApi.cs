using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SiteApi.IntegrationSiteApi.ApiBase.ItemApi
{
    public abstract class AbstractItemApi
    {
            internal string Token;
            internal string SiteUrl;
            internal string Method;
            internal string result;
            internal AbstractItemApi(string Token, string SiteUrl)
            {
                this.Token = Token;
                this.SiteUrl = SiteUrl;
            }
            internal HttpWebRequest GetRequest(string Url)
            {
                string url = @SiteUrl + Url;
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = Method;

                httpWebRequest.Headers.Add("Authorization:" + "Token " + Token);
                return httpWebRequest;
            }
        }
    
}
