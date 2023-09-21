using System.Net;

namespace SiteApi.IntegrationSiteApi.ApiBase.Delete
{
    public abstract class AbstractDeleteBase : BaseV1Api
    {
        public AbstractDeleteBase(string Token, string SiteUrl) : base(Token, SiteUrl)
        {
            Method = "DELETE";
        }

        internal void pDELETE(string DELETEUrl)
        {
            HttpWebRequest httpWebRequest = GetRequest(DELETEUrl);
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