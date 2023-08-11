using System;
using System.IO;
using System.Net;
namespace SiteApi.IntegrationSiteApi.ApiBase.Get
{
    public class AbstractGetBase : BaseV1Api
    {
        internal AbstractGetBase(string Token, string Url) :base(Token, Url)
        { base.Method = "Get"; }
        internal void pGet(string GetUrl)
        {
            HttpWebRequest httpWebRequest = GetRequest(GetUrl);
            HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            try
            {
                using StreamReader streamReader = new(httpResponse.GetResponseStream());
                result = streamReader.ReadToEnd();
            }
            catch (Exception e)
            {
                result = e.Message;
            }
        }
    }
}
