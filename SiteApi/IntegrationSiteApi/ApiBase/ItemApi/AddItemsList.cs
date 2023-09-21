using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
namespace SiteApi.IntegrationSiteApi.ApiBase.ItemApi
{
    public class AddItemsList : AbstractItemApi
    {
        public AddItemsList(string Token, string SiteUrl) : base(Token, SiteUrl)
        {
            Method = "POST";
        }
        public List<string> Post(List<Item> items)
        {
            pPost(@"api/pwa/v1/item/create_list/", JsonConvert.SerializeObject(new Request(items)));
            if (!result.ToLower().Contains("error"))
            {
                return JsonConvert.DeserializeObject<Response>(result).IDs;
            }
            return null;
        }
        private void pPost(string PostUrl, string JsonPostMessage)
        {
            HttpWebRequest httpWebRequest = GetRequest(PostUrl);
            HttpWebResponse httpResponse = null;
            //попытка отправить
            try
            {
                using (StreamWriter streamWriter = new(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(JsonPostMessage);
                }
                httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            }
            catch (Exception e)
            {
                result = e.Message;
            }
            //попытка принять
            try
            {
                using StreamReader streamReader = new(httpResponse.GetResponseStream());
                result = streamReader.ReadToEnd();
            }
            catch (Exception e)
            {
                result = e.Message;
            }
        }
        public class Description
        {
            [JsonProperty("description")]
            public string DescriptionItem { get; set; }
            [JsonProperty("descriptionSeparator")]
            public string DescriptionSeparator { get; set; }
        }
        public class Item
        {
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("sku")]
            public string Sku { get; set; }
            [JsonProperty("images")]
            public Image Images { get; set; }
            [JsonProperty("price")]
            public double Price { get; set; }
            [JsonProperty("description")]
            public Description Description { get; set; }
            [JsonProperty("changes")]
            public Change Changes { get; set; }
            [JsonProperty("categories")]
            public Category Categories { get; set; }
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
            public List<string> IDs { get; set; }
        }
    }
    public class Category
    {
        [JsonProperty("parent")]
        public string Parent { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
    public class Change
    {
        [JsonProperty("fieldСhange")]
        public string FieldСhange { get; set; }
        [JsonProperty("oldValue")]
        public string OldValue { get; set; }
        [JsonProperty("newValue")]
        public string NewValue { get; set; }
        [JsonProperty("source")]
        public string Source { get; set; }
        [JsonProperty("dateСhange")]
        public string DateСhange { get; set; }
        [JsonProperty("user")]
        public string User { get; set; }
    }
    public class Image
    {
        [JsonProperty("uri")]
        public string Uri { get; set; }
    }
}
