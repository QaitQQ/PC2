using StructLibCore.Marketplace;
using Newtonsoft.Json;
using SiteApi.IntegrationSiteApi.ApiBase.ObjDesc;
namespace SiteApi.IntegrationSiteApi.ApiBase.Post
{
    public class PostOrderBase : AbstractPOSTBase
    {
        public PostOrderBase(string Token, string SiteUrl) : base(Token, SiteUrl){}
        public bool Post(IOrder order) 
        {
            pPost(@"api/v1/order/create/", JsonConvert.SerializeObject(new Order(order)));
            if (!result.ToLower().Contains("error"))
            {
                return true;
            }
                return false;
        }
    }
}
