using Newtonsoft.Json;
using System.Net;
namespace SiteApi.IntegrationSiteApi.ApiBase.ItemApi
{
    public class AddItemsList : AbstractItemApi
    {
        public AddItemsList(string Token, string SiteUrl) : base(Token, SiteUrl)
        {
            Method =  Method_Type.POST;
        }
        public List<string> Post(List<Item> items)
        {
            MethodUri = @"/api/pwa/v1/item/create_list/";
            Message = new Request(items);
            var Result = Messaging<Response>();
            if (Result != null)
            {
                return Result!.IDs!;
            }
            return null!;
        }

        public class Item
        {
            [JsonProperty("name")]
            public string? Name { get; set; }
            [JsonProperty("sku")]
            public string? Sku { get; set; }
            [JsonProperty("images")]
            public Image? Images { get; set; }
            [JsonProperty("price")]
            public double Price { get; set; }
            [JsonProperty("description")]
            public Description? Description { get; set; }
            [JsonProperty("changes")]
            public Change? Changes { get; set; }
            [JsonProperty("categories")]
            public Category? Categories { get; set; }
        }
        public class Request
        {
            public Request(List<Item> items)
            {
                Items = items;
            }
            [JsonProperty("Items")]
            public List<Item> Items { get; set; }
        }
        public class Response
        {
            [JsonProperty("IDs")]
            public List<string>? IDs { get; set; }
        }
    }

}
