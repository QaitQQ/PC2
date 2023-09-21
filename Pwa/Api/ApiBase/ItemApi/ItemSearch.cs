using Newtonsoft.Json;

using SiteApi.IntegrationSiteApi.ApiBase.ItemApi;
namespace ApiLib.ApiBase.ItemApi
{
    public class ItemSearch : AbstractItemApi
    {
        public ItemSearch(string Token, string SiteUrl) : base(Token, SiteUrl)
        {
            Method = Method_Type.POST;
        }
        public object Post(string Name)
        {
            MethodUri = @"/api/pwa/v1/item/item_search/";
            Message = new Request(Name);
            Response Result = Go<ApiLib.ApiBase.ItemApi.ItemSearch.Response>();
            if (Result != null)
            {
                return Result.Items!;
            }
            return null!;
        }
        public class Request
        {
            public Request(string name)
            {
                Name = name;
            }
            [JsonProperty("cname")]
            public string? Name { get; set; }
        }
        public class FItem: IAbstractItem
        {
            [JsonProperty("item")]
            public Item? ItemN { get; set; }
            public string Name { get { return ItemN.Name; } }
            public int Id { get { return ItemN.Id; } }
            public string? Description { get; }

            public string Price { get; }
        }

        public class Item
        {
            [JsonProperty("name")]
            public string? Name { get; set; }

            [JsonProperty("id")]
            public int Id { get; set; }
        }

        public class Response
        {
            [JsonProperty("Items")]
            public List<FItem>? Items { get; set; }
        }


    }
}
