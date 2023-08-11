using Newtonsoft.Json;
using SiteApi.IntegrationSiteApi.ApiBase.ObjDesc;
using System;
using System.Collections.Generic;
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
            if (!result.ToLower().Contains("error"))
            {
                var T = JsonConvert.DeserializeObject<List<Order>>(result);
                foreach (var item in T)
                {
                    item.BoxingDate = (TimeZoneInfo.ConvertTimeFromUtc(item.BoxingDate, TimeZoneInfo.Local));
                    item.OrderDate = (TimeZoneInfo.ConvertTimeFromUtc(item.OrderDate, TimeZoneInfo.Local));
                }
                return T;
            }
            return null;
        }
    }
}
