using Newtonsoft.Json;

using System.Net;

namespace SiteApi.IntegrationSiteApi.APIMarket.Yandex
{
    public class Deserialize<T>
    {
        private readonly HttpWebRequest httpWebRequest;

        public Deserialize(HttpWebRequest httpWebRequest)
        {
            this.httpWebRequest = httpWebRequest;
        }
        public T Get(string? result)
        {
            HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }
            T root = JsonConvert.DeserializeObject<T>(result)!;
            return root!;
        }

    }
}
