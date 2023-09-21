using StructLibCore.Marketplace;

using System.Net;
namespace SiteApi.IntegrationSiteApi.APIMarket.Yandex
{
    public class YandexGETAct : SiteApi.IntegrationSiteApi.APIMarket.Yandex.YandexApiClass
    {
        public YandexGETAct(APISetting APISetting) : base(APISetting)
        {
        }
        public string Get()
        {
            HttpWebRequest httpWebRequest = GetRequest(@"/campaigns/" + ClientID + "/shipments/reception-transfer-act.json", "GET");
            HttpWebResponse httpResponse;
            try
            {
                httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            }
            catch (Exception e)
            {
                return "none" + e.Message;
            }
            try
            {
                using FileStream str = File.Create("temp.pdf");
                httpResponse.GetResponseStream().CopyTo(str);

                return "temp.pdf";
            }
            catch
            {
                using FileStream str = File.Create("temp2.pdf");
                httpResponse.GetResponseStream().CopyTo(str);

                return "temp2.pdf";
            }
        }
    }
}
