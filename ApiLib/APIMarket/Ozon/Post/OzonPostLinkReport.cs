using Newtonsoft.Json;

using StructLibCore.Marketplace;

using System.Net;
namespace SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post
{
    public class OzonPostLinkReport : OzonPost
    {
        public OzonPostLinkReport(APISetting aPISetting) : base(aPISetting)
        {
        }
        public string? Get(string code)
        {
            HttpWebRequest httpWebRequest = GetRequest(@"v1/report/info");

            using (StreamWriter streamWriter = new(httpWebRequest.GetRequestStream())) { string root = JsonConvert.SerializeObject(new Request(code)); streamWriter.Write(root); }
            HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (StreamReader streamReader = new(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }
            Response End = JsonConvert.DeserializeObject<Response>(result!)!;

            if (End.Result?.Status == "success")
            {
                return End.Result.File;
            }
            return null;

        }
        public class Request
        {
            public Request(string code)
            {
                Code = code;
            }

            [JsonProperty("code")]
            public string? Code { get; set; }
        }

        public class Params
        {
        }

        public class Result
        {
            [JsonProperty("code")]
            public string? Code { get; set; }

            [JsonProperty("status")]
            public string? Status { get; set; }

            [JsonProperty("error")]
            public string? Error { get; set; }

            [JsonProperty("file")]
            public string? File { get; set; }

            [JsonProperty("report_type")]
            public string? ReportType { get; set; }

            [JsonProperty("params")]
            public Params? Params { get; set; }

            [JsonProperty("created_at")]
            public DateTime CreatedAt { get; set; }
        }

        public class Response
        {
            [JsonProperty("result")]
            public Result? Result { get; set; }
        }

    }
}
