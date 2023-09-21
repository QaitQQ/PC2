using Newtonsoft.Json;

using StructLibCore.Marketplace;

using System.Net;

namespace SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post
{
    public class OzonPOSTShowStatusImport : OzonPost
    {
        public OzonPOSTShowStatusImport(APISetting aPISetting) : base(aPISetting)
        {
        }
        public object? Get(string task_id)
        {
            HttpWebRequest httpWebRequest = GetRequest(@"v1/product/import/info");
            Request root = new() { TaskId = task_id };
            using (StreamWriter streamWriter = new(httpWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(root); streamWriter.Write(json);
            }
            try
            {
                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }
                return result;
            }
            catch
            {
                return null;
            }
        }
        public class Request
        {
            [JsonProperty("task_id")]
            public string? TaskId { get; set; }
        }
    }
}
