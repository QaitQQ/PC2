using System.Net;
namespace SiteApi.IntegrationSiteApi.ApiBase
{
    public abstract class BaseV1Api
    {
        internal string Token;
        internal string SiteUrl;
        internal string Method;
        internal string result;
        internal BaseV1Api(string Token, string SiteUrl)
        {
            this.Token = Token;
            this.SiteUrl = SiteUrl;
        }
        internal HttpWebRequest GetRequest(string Url)
        {
            string url = @SiteUrl + Url;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = Method!;

            httpWebRequest.Headers.Add("Authorization:" + "Token " + Token);
            return httpWebRequest;
        }
    }
}
