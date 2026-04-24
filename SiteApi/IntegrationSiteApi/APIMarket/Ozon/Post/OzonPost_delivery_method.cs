using Newtonsoft.Json;
using StructLibCore.Marketplace;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;
namespace SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post
{
    public class OzonPost_delivery_method : OzonPost
    {
        public OzonPost_delivery_method(APISetting aPISetting) : base(aPISetting)
        {
        }
        public List<OzonPost_delivery_method_Result> Get()
        {
            var httpWebRequest = GetRequest(@"v1/delivery-method/list");


            OzonPost_delivery_method_Request Request = new();
            Request.Filter = null;
            Request.Limit = 100;
            Request.Offset = 0;

            Response response = null;
            try
            {
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    var root = JsonConvert.SerializeObject(Request);
                    streamWriter.Write(root);
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }
                
                

                response = JsonConvert.DeserializeObject<Response>(result);

            }
            catch (System.Exception e)
            { 
            }

            if (response != null)
            {
                return response.Result;
            }

            return new List<OzonPost_delivery_method_Result>();
        }


        public class OzonPost_delivery_method_Result
        {
            [JsonProperty("id")]
            [JsonPropertyName("id")]
            public double Id { get; set; }

            [JsonProperty("company_id")]
            [JsonPropertyName("company_id")]
            public double CompanyId { get; set; }

            [JsonProperty("name")]
            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonProperty("status")]
            [JsonPropertyName("status")]
            public string Status { get; set; }

            [JsonProperty("cutoff")]
            [JsonPropertyName("cutoff")]
            public string Cutoff { get; set; }

            [JsonProperty("provider_id")]
            [JsonPropertyName("provider_id")]
            public double ProviderId { get; set; }

            [JsonProperty("template_id")]
            [JsonPropertyName("template_id")]
            public double TemplateId { get; set; }

            [JsonProperty("warehouse_id")]
            [JsonPropertyName("warehouse_id")]
            public double WarehouseId { get; set; }

            [JsonProperty("sla_cut_in")]
            [JsonPropertyName("sla_cut_in")]
            public double SlaCutIn { get; set; }

            [JsonProperty("created_at")]
            [JsonPropertyName("created_at")]
            public DateTime CreatedAt { get; set; }

            [JsonProperty("updated_at")]
            [JsonPropertyName("updated_at")]
            public DateTime UpdatedAt { get; set; }
        }

        public class Response
        {
            [JsonProperty("result")]
            public List<OzonPost_delivery_method_Result> Result { get; set; }

            [JsonProperty("has_next")]
            public bool HasNext { get; set; }
        }


        public class Filter
        {
            [JsonProperty("provider_id")]
            public double ProviderId { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("warehouse_id")]
            public double WarehouseId { get; set; }
        }

        public class OzonPost_delivery_method_Request
        {
            [JsonProperty("filter")]
            public Filter Filter { get; set; }

            [JsonProperty("limit")]
            public double Limit { get; set; }

            [JsonProperty("offset")]
            public double Offset { get; set; }
        }

    }
}
