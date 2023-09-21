using Newtonsoft.Json;

namespace SiteApi.IntegrationSiteApi.ApiMainSite.Post
{
    public class ProductList : MainSitePostAbstract
    {
        public ProductList(string Token, string SiteUrl) : base(Token, SiteUrl)
        {
        }
        public List<Product> Post()
        {

            pPost("myapi.getAllProduct", JsonConvert.SerializeObject(new SiteApi.IntegrationSiteApi.ApiMainSite.Post.MainRequest() { Token = Token }));
            return JsonConvert.DeserializeObject<Resposne>(result!)?.Products!;
        }
        public class Resposne
        {
            [JsonProperty("success")]
            public List<Product>? Products { get; set; }
        }

        public class Product
        {
            [JsonProperty("product_id")]
            public string? ProductId { get; set; }

            [JsonProperty("name")]
            public string? Name { get; set; }

            [JsonProperty("price")]
            public string? Price { get; set; }
        }

    }
}
