using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using Server.Class.IntegrationSiteApi.Market.Ozon.OzonPortOrderList;

namespace SiteApi.IntegrationSiteApi.ApiBase.Delete
{
    public class DeleteOrder : AbstractDeleteBase
    {
        public DeleteOrder(string Token, string SiteUrl) : base(Token, SiteUrl)
        {
        }

        public string Get(SiteApi.IntegrationSiteApi.ApiBase.ObjDesc.Order order)
        {
            pDELETE(@"api/v1/order/Detail/"+ order.Id);
            return result;
        }

    }
}