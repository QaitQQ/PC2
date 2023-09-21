using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
namespace SiteApi.IntegrationSiteApi.ApiBase.ItemApi
{
    public abstract class AbstractItemApi
    {
        public interface IAbstractItem
        {
            public string Name { get; }
            public int Id { get; }
            public string? Description { get; }
            public string? Price { get; }


        }


        internal enum Method_Type {POST,GET,DELETE,PUT }
        internal string Token;
        internal string SiteUrl;
        internal Method_Type Method;
        internal string MethodUri;
        internal object? Message;

        internal AbstractItemApi(string Token, string SiteUrl)
        {
            this.Token = Token;
            this.SiteUrl = SiteUrl;
        }
        internal T Go<T>()
        {
            string url = @SiteUrl + MethodUri;
            string jsonValue = JsonConvert.SerializeObject(Message);
            using HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(@SiteUrl);
            client.DefaultRequestHeaders.Accept.Add(new  MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.AcceptEncoding.Add( new StringWithQualityHeaderValue("UTF8"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", Token);

            StringContent content = new StringContent(jsonValue, Encoding.UTF8, "application/json");
            string message = null;

            HttpResponseMessage response=null;
            try
            {
                switch (Method)
                {
                    case Method_Type.POST:
                        response = client.PostAsync(url, content).Result;
                        break;
                    case Method_Type.GET:
                        response = client.SendAsync(new HttpRequestMessage(HttpMethod.Get, url)).Result;

                        break;
                    case Method_Type.DELETE:
                        break;
                    case Method_Type.PUT:
                        break;
                }
            }
            catch 
            {

            }

            message = response?.Content?.ReadAsStringAsync().Result;
                    
            if ((message!=null && response!=null && response.IsSuccessStatusCode))
            {
                var result = JsonConvert.DeserializeObject<T>(message);
                return result;
            }
            return default;
        }


        public class Description
        {
            [JsonProperty("description")]
            public string? DescriptionItem { get; set; }
            [JsonProperty("descriptionSeparator")]
            public string? DescriptionSeparator { get; set; }
        }
        public class Category
        {
            [JsonProperty("parent")]
            public string? Parent { get; set; }
            [JsonProperty("name")]
            public string? Name { get; set; }
        }
        public class Change
        {
            [JsonProperty("fieldСhange")]
            public string? FieldСhange { get; set; }
            [JsonProperty("oldValue")]
            public string? OldValue { get; set; }
            [JsonProperty("newValue")]
            public string? NewValue { get; set; }
            [JsonProperty("source")]
            public string? Source { get; set; }
            [JsonProperty("dateСhange")]
            public string? DateСhange { get; set; }
            [JsonProperty("user")]
            public string? User { get; set; }
        }
        public class Image
        {
            [JsonProperty("uri")]
            public string? Uri { get; set; }
        }
        public class ItemComparisonName
        {
            [JsonProperty("cname")]
            public string? Cname { get; set; }
        }



    }
}
