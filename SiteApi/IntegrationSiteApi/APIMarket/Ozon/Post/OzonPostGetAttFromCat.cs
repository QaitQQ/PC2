using Newtonsoft.Json;
using StructLibCore.Marketplace;
using System.Collections.Generic;
using System.IO;
using System.Net;
namespace SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post
{
    public class OzonPostGetAttFromCat : OzonPost
    {
        public OzonPostGetAttFromCat(APISetting aPISetting) : base(aPISetting){}
        public Response Get(List<string> CatIds)
        {
            HttpWebRequest httpWebRequest = GetRequest(@"v3/category/attribute");
            Request root = new() { AttributeType = "REQUIRED", Language = "DEFAULT", CategoryId = CatIds };
            using (StreamWriter streamWriter = new(httpWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(root); streamWriter.Write(json);
            }
            try
            {
                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }
                Response Response = JsonConvert.DeserializeObject<Response>(result);
                return Response;
            }
            catch
            {
                return null;
            }
        }
        public class Request
        {
            public Request() { CategoryId = new List<string>(); }
            [JsonProperty("attribute_type")]
            public string AttributeType { get; set; }
            [JsonProperty("category_id")]
            public List<string> CategoryId { get; set; }
            [JsonProperty("language")]
            public string Language { get; set; }
        }
        public class Attribute
        {
            [JsonProperty("id")]
            public int Id { get; set; }
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("description")]
            public string Description { get; set; }
            [JsonProperty("type")]
            public string Type { get; set; }
            [JsonProperty("is_collection")]
            public bool IsCollection { get; set; }
            [JsonProperty("is_required")]
            public bool IsRequired { get; set; }
            [JsonProperty("group_id")]
            public int GroupId { get; set; }
            [JsonProperty("group_name")]
            public string GroupName { get; set; }
            [JsonProperty("dictionary_id")]
            public int DictionaryId { get; set; }
            [JsonProperty("is_aspect")]
            public bool IsAspect { get; set; }
            [JsonProperty("category_dependent")]
            public bool CategoryDependent { get; set; }
        }
        public class Result
        {
            [JsonProperty("category_id")]
            public int CategoryId { get; set; }
            [JsonProperty("attributes")]
            public List<Attribute> Attributes { get; set; }
        }
        public class Response
        {
            [JsonProperty("result")]
            public List<Result> Result { get; set; }
        }
    }
}
