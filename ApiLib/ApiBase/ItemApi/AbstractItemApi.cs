using Newtonsoft.Json;
using System.Net;
namespace SiteApi.IntegrationSiteApi.ApiBase.ItemApi
{
    public abstract class AbstractItemApi
    {
        internal enum Method_Type {POST,GET,DELETE,PUT }
        internal string? Token;
        internal string? SiteUrl;
        internal Method_Type Method;
        internal string? MethodUri;
        internal string? result;
        internal object? Message;

        internal AbstractItemApi(string Token, string SiteUrl)
        {
            this.Token = Token;
            this.SiteUrl = SiteUrl;
        }
        internal HttpWebRequest GetRequest(string Url)
        {
            string url = @SiteUrl + Url;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = Method!.ToString()!;
            httpWebRequest.Headers.Add("Authorization:" + "Token " + Token);
            return httpWebRequest;
        }
        internal void pPost()
        {
            HttpWebRequest httpWebRequest = GetRequest(MethodUri!);
            HttpWebResponse? httpResponse = null;
            //попытка отправить
            try
            {
                using (StreamWriter streamWriter = new(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(JsonConvert.SerializeObject(Message));
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
                using StreamReader streamReader = new(httpResponse?.GetResponseStream()!);
                result = streamReader.ReadToEnd();
            }
            catch (Exception e)
            {
                result = e.Message;
            }
        }
        internal void pGet()
        {
            HttpWebRequest httpWebRequest = GetRequest(MethodUri!);
            HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
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
        internal T Messaging<T>() 
        {
            switch (Method)
            {
                case Method_Type.POST:
                    pPost();
                    break;
                case Method_Type.GET:
                    pGet();
                    break;
                case Method_Type.DELETE:
                    break;
                case Method_Type.PUT:
                    break;
                default:
                    break;
            }
            if (result != null && !result!.ToLower().Contains("error"))
            {
                return JsonConvert.DeserializeObject<T>(result!)!;
            }
            return default!;
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
