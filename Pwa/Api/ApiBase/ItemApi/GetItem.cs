using Newtonsoft.Json;
using SiteApi.IntegrationSiteApi.ApiBase.ItemApi;
namespace ApiLib.ApiBase.ItemApi
{
    internal class GetItem : AbstractItemApi
    {
        public GetItem(string Token, string SiteUrl) : base(Token, SiteUrl)
        {
            Method = Method_Type.GET;
        }
        public IAbstractItem Get(string item_id)
        {
            MethodUri = @"/api/pwa/v1/item/get_item?item_id=" + item_id;
            return Go<Response>().Item[0];
        }
        public class Item:IAbstractItem
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("description")]
            public Description DescriptionItem { get; set; }

            [JsonProperty("itemComparisonName")]
            public List<ItemComparisonName> ItemComparisonName { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("sku")]
            public object Sku { get; set; }

            [JsonProperty("price")]
            public double ItemPrice { get; set; }

            [JsonProperty("changes")]
            public object Changes { get; set; }

            [JsonProperty("categories")]
            public object Categories { get; set; }

            [JsonProperty("manufactor")]
            public object Manufactor { get; set; }

            public string Description { get { return DescriptionItem.DescriptionItem; } }

            public string Price { get { return ItemPrice.ToString(); } }
        }

        public class Response
        {
            [JsonProperty("Item")]
            public List<Item> Item { get; set; }
        }


    }
}
