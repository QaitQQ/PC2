using StructLibCore.Marketplace;

using System.Net;
namespace SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post
{
    public class OzonPostPromo : OzonPost
    {
        public OzonPostPromo(APISetting aPISetting) : base(aPISetting) { }
        public bool Get(object PromoList)
        {
            HttpWebRequest httpWebRequest = GetRequest(@"v1/actions/products");
            try
            {
                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using StreamReader streamReader = new(httpResponse.GetResponseStream());
                result = streamReader.ReadToEnd();
            }
            catch
            {
                return false;
            }
            return false;
        }


    }
}
