using StructLibCore.Marketplace;

using System.Net;

namespace SiteApi.IntegrationSiteApi.APIMarket.Yandex
{
    public class YandexGetOrder : SiteApi.IntegrationSiteApi.APIMarket.Yandex.YandexApiClass
    {
        public YandexGetOrder(APISetting APISetting) : base(APISetting)
        {
        }
        public List<object> Get(string Id)
        {
            HttpWebRequest httpWebRequest = GetRequest(@"/campaigns/" + ClientID + "/orders/" + Id + ".json", "GET");
            try
            {
                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }

                List<object> Lst = new List<object>();
                return Lst;
            }
            catch
            {

                return null!;
            }

        }
    }
}
