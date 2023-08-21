using static SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post.OzonPostGetAttr;
using System.Collections.Generic;

namespace SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post
{
    public interface IAttribute
    {
        public int ID { get; set; }
        public int ComplexId { get; set; }
        public List<OzValue> Values { get; set; }
    }
}