using Newtonsoft.Json;
using SiteApi.IntegrationSiteApi.ApiBase.ItemApi;
namespace ApiLib.ApiBase.ItemApi
{
    internal class GetItem : AbstractItemApi
    {
        public GetItem(string Token, string SiteUrl) : base(Token, SiteUrl)
        {
            Method = Method_Type.POST;
        }
        public Item Get(string item_id)
        {
            MethodUri = @"/api/pwa/v1/item/get_item?item_id=" + item_id;
            return Messaging<Item>();
        }
        public class Item
        {
            [JsonProperty("id")]
            public int? Id { get; set; }
            [JsonProperty("description")]
            public Description? Description { get; set; }
            [JsonProperty("itemComparisonName")]
            public List<ItemComparisonName>? ItemComparisonName { get; set; }
            [JsonProperty("name")]
            public string? Name { get; set; }
            [JsonProperty("sku")]
            public string? Sku { get; set; }
            [JsonProperty("price")]
            public double? Price { get; set; }
            [JsonProperty("changes")]
            public Change? Change { get; set; }
            [JsonProperty("categories")]
            public Category? Categories { get; set; }
            [JsonProperty("manufactor")]
            public object? Manufactor { get; set; }
            [JsonProperty("images")]
            public Image? Images { get; set; }
        }
    }
}
