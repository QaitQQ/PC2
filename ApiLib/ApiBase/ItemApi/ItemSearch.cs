using Newtonsoft.Json;

using SiteApi.IntegrationSiteApi.ApiBase.ItemApi;
using System.Text.Json.Serialization;
namespace ApiLib.ApiBase.ItemApi
{
    public class ItemSearch : AbstractItemApi
    {
        public ItemSearch(string Token, string SiteUrl) : base(Token, SiteUrl)
        {
        }
        public object Post(string Name)
        {
            MethodUri = @"/api/pwa/v1/item/item_search/";
            Message = new Request(Name);
            Response Result = Messaging<Response>();
            if (Result != null)
            {
                return Result.Items!;
            }
            return null!;
        }
        public class Request
        {
            public Request(string? name)
            {
                Name = name;
            }
            [JsonProperty("cname")]
            public string? Name { get; set; }
        }
        public class FItem
        {
            [JsonProperty("item")]
            public Item? ItemN { get; set; }
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
