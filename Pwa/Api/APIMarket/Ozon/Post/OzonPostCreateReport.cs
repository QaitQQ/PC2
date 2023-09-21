using Newtonsoft.Json;

using StructLibCore.Marketplace;

using System.Net;
namespace SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post
{
    public class OzonPostCreateReport : OzonPost
    {
        public OzonPostCreateReport(APISetting aPISetting) : base(aPISetting)
        {
        }
        public string Get()
        {
            HttpWebRequest httpWebRequest = GetRequest(@"v1/report/postings/create");
            DateTime today = DateTime.Today;
            DateTime month = new DateTime(today.Year, today.Month, 1);
            DateTime first = month.AddMonths(-1);
            DateTime last = month.AddDays(-1);
            using (StreamWriter streamWriter = new(httpWebRequest.GetRequestStream())) { string root = JsonConvert.SerializeObject(new Request(first, last)); streamWriter.Write(root); }
            HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (StreamReader streamReader = new(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }
            Response End = JsonConvert.DeserializeObject<Response>(result)!;
            return End?.Result?.Code!;
        }
        public class Filter
        {
            public Filter(DateTime processedAtFrom, DateTime processedAtTo)
            {
                ProcessedAtFrom = processedAtFrom.ToString("u").Replace(" ", "T");
                ProcessedAtTo = processedAtTo.ToString("u").Replace(" ", "T");
                DeliverySchema = new List<string> { "fbs" };
            }
            [JsonProperty("processed_at_from")]
            public string? ProcessedAtFrom { get; set; }
            [JsonProperty("processed_at_to")]
            public string? ProcessedAtTo { get; set; }
            [JsonProperty("delivery_schema")]
            public List<string>? DeliverySchema { get; set; }
            [JsonProperty("sku")]
            public List<object>? Sku { get; set; }
            [JsonProperty("cancel_reason_id")]
            public List<object>? CancelReasonId { get; set; }
            [JsonProperty("offer_id")]
            public string? OfferId { get; set; }
            [JsonProperty("status_alias")]
            public List<object>? StatusAlias { get; set; }
            [JsonProperty("statuses")]
            public List<object>? Statuses { get; set; }
            [JsonProperty("title")]
            public string? Title { get; set; }
        }
        public class Request
        {
            public Request(DateTime processedAtFrom, DateTime processedAtTo)
            {
                Filter = new Filter(processedAtFrom, processedAtTo);
                Language = "DEFAULT";
            }
            [JsonProperty("filter")]
            public Filter Filter { get; set; }
            [JsonProperty("language")]
            public string Language { get; set; }
        }
        public class Result
        {
            [JsonProperty("code")]
            public string? Code { get; set; }
        }
        public class Response
        {
            [JsonProperty("result")]
            public Result? Result { get; set; }
        }
    }
}
