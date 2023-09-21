using Newtonsoft.Json;

using System.Net;
using System.Text;
using System.Web;
namespace SiteApi.IntegrationSiteApi.ApiMainSite.Post
{
    public class MainSiteAutorization : MainSiteV1Abstract
    {
        public MainSiteAutorization(string Token, string SiteUrl) : base(Token, SiteUrl)
        {
        }
        public string Autorization()
        {
            HttpWebRequest httpWebRequest = GetRequest();
            HttpWebResponse httpResponse = null!;
            StringBuilder LST = new StringBuilder();
            AppendUrlEncoded(LST, "username", "Default");
            AppendUrlEncoded(LST, "key", "3EjSWpLrm8ZEZB8IQ6fQWniJ0vD6Lwn7SB7aZWm78FSlAVZ1Hqtg8MydBzOt6OSXKc2gUQiYVNRyXpLkY7lptKhj7mU1FNA6UOcXPgWQnEhCtctx4fjuhpX8QhUbtXVyy4zAbc8T0QRQMxZPhwtpwhFOkuFFLdnIwYbl8Eogfu5835IK7CzZskA6jd0MqQZNttGc64t2CToXzatu2uRAzclwEbCgJuSnsu5UC7k8BwCBxGWwpgJRqNeOHtam6xPb");
            try
            {
                using (StreamWriter streamWriter = new(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(LST);
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
            Response? End = JsonConvert.DeserializeObject<Response>(result);

            if (End?.ApiToken != null)
            {
                return End.ApiToken;
            }
            else
            {
                return null!;
            }
        }
        public static void AppendUrlEncoded(StringBuilder sb, string name, string value)
        {
            if (sb.Length != 0)
                _ = sb.Append("&");
            _ = sb.Append(HttpUtility.UrlEncode(name));
            _ = sb.Append("=");
            _ = sb.Append(HttpUtility.UrlEncode(value));
        }
        private HttpWebRequest GetRequest()
        {
            string url = @SiteUrl + @"account/login";
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.Method = "POST";
            return httpWebRequest;
        }
        public class Response
        {
            [JsonProperty("success")]
            public string? Success { get; set; }
            [JsonProperty("api_token")]
            public string? ApiToken { get; set; }
        }
    }
}
