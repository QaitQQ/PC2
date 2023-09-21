using Newtonsoft.Json;

using SiteApi.IntegrationSiteApi.ApiBase.ObjDesc;
namespace SiteApi.IntegrationSiteApi.ApiBase.Get
{
    public class GetAllOrders : AbstractGetBase
    {
        public GetAllOrders(string Token, string Uri) : base(Token, Uri)
        {
        }
        public List<Order> Get()
        {
            pGet(@"api/v1/order/all/");
            if (result != null && !result.ToLower().Contains("error"))
            {
                List<Order>? T = JsonConvert.DeserializeObject<List<Order>>(result);
                foreach (Order item in T!)
                {
                    item.BoxingDate = (TimeZoneInfo.ConvertTimeFromUtc(item.BoxingDate, TimeZoneInfo.Local));
                    item.OrderDate = (TimeZoneInfo.ConvertTimeFromUtc(item.OrderDate, TimeZoneInfo.Local));
                }
                return T;
            }
            return null!;
        }
    }
}
