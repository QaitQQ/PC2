using Newtonsoft.Json;
using StructLibCore.Marketplace;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
namespace SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post
{
    public class OzonPostGetAttr : OzonPost
    {
        public OzonPostGetAttr(APISetting aPISetting) : base(aPISetting) { }
        public Response Get(List<string> ItemId)
        {
            HttpWebRequest httpWebRequest = GetRequest(@"v3/products/info/attributes");
            Request root = new() { Filter = new Filter() { ProductId = ItemId } };
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
            catch (Exception e)
            {
                return null;
            }
        }
        public class Filter
        {
            public Filter()
            {
                Visibility = "ALL";
            }
            [JsonProperty("product_id")]
            public List<string> ProductId { get; set; }
            [JsonProperty("visibility")]
            public string Visibility { get; set; }
        }
        public class Request
        {
            public Request()
            {
                Limit = 1000;
                SortDir = "ASC";
            }
            [JsonProperty("filter")]
            public Filter Filter { get; set; }
            [JsonProperty("limit")]
            public int Limit { get; set; }
            [JsonProperty("last_id")]
            public string LastId { get; set; }
            [JsonProperty("sort_dir")]
            public string SortDir { get; set; }
        }
        public class Attribute: IAttribute
        {
            [JsonProperty("attribute_id")]
            public int AttributeId { get; set; }
            [JsonProperty("complex_id")]
            public int ComplexId { get; set; }
            [JsonProperty("values")]
            public List<OzValue> Values { get; set; }
            public int ID { get { return AttributeId; } set { AttributeId = value; } }
        }
        public class Image
        {
            [JsonProperty("file_name")]
            public string FileName { get; set; }
            [JsonProperty("default")]
            public bool Default { get; set; }
            [JsonProperty("index")]
            public int Index { get; set; }
        }
        public class Result
        {
            [JsonProperty("id")]
            public int Id { get; set; }
            [JsonProperty("barcode")]
            public string Barcode { get; set; }
            [JsonProperty("category_id")]
            public int CategoryId { get; set; }
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("offer_id")]
            public string OfferId { get; set; }
            [JsonProperty("height")]
            public int Height { get; set; }
            [JsonProperty("depth")]
            public int Depth { get; set; }
            [JsonProperty("width")]
            public int Width { get; set; }
            [JsonProperty("dimension_unit")]
            public string DimensionUnit { get; set; }
            [JsonProperty("weight")]
            public int Weight { get; set; }
            [JsonProperty("weight_unit")]
            public string WeightUnit { get; set; }
            [JsonProperty("images")]
            public List<Image> Images { get; set; }
            [JsonProperty("image_group_id")]
            public string ImageGroupId { get; set; }
            [JsonProperty("images360")]
            public List<object> Images360 { get; set; }
            [JsonProperty("pdf_list")]
            public List<object> PdfList { get; set; }
            [JsonProperty("attributes")]
            public List<Attribute> Attributes { get; set; }
            [JsonProperty("complex_attributes")]
            public List<object> ComplexAttributes { get; set; }
            [JsonProperty("color_image")]
            public string ColorImage { get; set; }
            [JsonProperty("last_id")]
            public string LastId { get; set; }
        }
        public class Response
        {
            [JsonProperty("result")]
            public List<Result> Result { get; set; }
            [JsonProperty("total")]
            public int Total { get; set; }
            [JsonProperty("last_id")]
            public string LastId { get; set; }
        }
        public class OzValue
        {
            [JsonProperty("dictionary_value_id")]
            public int DictionaryValueId { get; set; }
            [JsonProperty("value")]
            public string Value { get; set; }
        }
    }
}
