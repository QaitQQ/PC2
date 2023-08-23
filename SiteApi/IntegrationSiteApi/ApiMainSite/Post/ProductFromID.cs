using Newtonsoft.Json;

using System.Collections.Generic;

namespace SiteApi.IntegrationSiteApi.ApiMainSite.Post
{
    internal partial class ProductFromID : MainSitePostAbstract
    {
        public ProductFromID(string Token, string SiteUrl) : base(Token, SiteUrl)
        {
        }

        public List<Product> Post(List<string> IDs) 
        {
            pPost("myapi.getProduct", JsonConvert.SerializeObject(new Request() { ProductId = Formatter(IDs), Token = Token }));
            return JsonConvert.DeserializeObject<Resposne>(result).Products;
        }


        public class Request : MainRequest 
        {
            [JsonProperty("product_id")]
            public string ProductId { get; set; }
        }

        private string Formatter(List<string> str)
        {
            string formstr = null;
            int i = 0;
            while (i < str.Count - 1)
            {
                formstr = formstr + str[i] + ";";
                i++;
            }
            formstr = formstr + str[str.Count - 1];
            return formstr;
        }
        public class Resposne
        {
            [JsonProperty("success")]
            public List<Product> Products { get; set; }
        }

        public class Product
        {
            [JsonProperty("product_id")]
            public string ProductId { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("base_price")]
            public string BasePrice { get; set; }
        }




    }
}
