using Newtonsoft.Json;

using StructLibCore.Marketplace;

using System.Net;
namespace SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post
{
    public class OzonPostLabel : OzonPost
    {
        public OzonPostLabel(APISetting aPISetting) : base(aPISetting)
        {
        }
        public string? Get(object Order)
        {
            HttpWebRequest httpWebRequest = GetRequest(@"v2/posting/fbs/package-label");
            httpWebRequest.Accept = "text/html,application/xhtml+xml,application/xml,application/pdf";
            Server.Class.IntegrationSiteApi.Market.Ozon.OzonPortOrderList.Order Or = (Server.Class.IntegrationSiteApi.Market.Ozon.OzonPortOrderList.Order)Order;
            Root root = new() { PostingNumber = new List<string>() { Or?.PostingNumber! } };
            using (StreamWriter streamWriter = new(httpWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(root); streamWriter.Write(json);
            }
            HttpWebResponse httpResponse;
            try
            {
                httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            }
            catch (Exception e)
            {
                return "none " + e.Message;
            }
            try
            {
                using FileStream str = File.Create("temp.pdf");
                httpResponse.GetResponseStream().CopyTo(str);
                return "temp.pdf";
            }
            catch
            {
                using FileStream str = File.Create("temp2.pdf");
                httpResponse.GetResponseStream().CopyTo(str);
                return "temp2.pdf";
            }
        }
        public class Root
        {
            [JsonProperty("posting_number")]
            public List<string>? PostingNumber { get; set; }
        }
        public class RootResult
        {
            [JsonProperty("content")]
            public string? Content { get; set; }
        }
    }
}
