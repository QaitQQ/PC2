using Newtonsoft.Json;
using SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post;
using StructLibCore.Marketplace;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
namespace Server.Class.IntegrationSiteApi.Market.Ozon
{
    public class OzonPostItemCopyObject : SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post.OzonPost
    {
        public OzonPostItemCopyObject(APISetting aPISetting) : base(aPISetting)
        {
        }
        public Response Get(List<IMarketItem> list)
        {
            var httpWebRequest = GetRequest(@"v2/product/import");
            var Request = new Request();
            Response Response = null;
            List<SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post.Item> TestList = new OzonPostItemList(aPISetting).Get();
            List<IMarketItem> TestListArchive = new OzonPostItemARCHIVEDList(aPISetting).Get();
            List<string> Ids = new List<string>();
            foreach (var item in TestList)
            {
                Ids.Add(item.offer_id);
            }
            foreach (var item in TestListArchive)
            {
                Ids.Add(item.SKU);
            }         
            List<IMarketItem> EndList = new List<IMarketItem>();
            foreach (var item in list)
            {
                if (!Ids.Contains(item.SKU))
                {
                    bool test = true;
                    if (!item.Name.ToLower().Contains("AltСam".ToLower()))
                    {
                        foreach (var itemX in ((SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post.OzonPostGetAttr.Result)item.attributes[0]).Attributes)
                        {
                            if (itemX.Values.Count > 0)
                            {
                                if (((itemX.Values[0].Value.ToLower().Contains("http:") || (itemX.Values[0].Value.ToLower().Contains("https:")))) || itemX.Values[0].Value.ToLower().Contains("altcam"))
                                {
                                    test = false; break;
                                }
                            }
                        }
                        if (test)
                        {
                            EndList.Add(item);
                        }
                    }
                }
            }
            if (EndList.Count > 99)
            {
                int cycle = EndList.Count / 99;
                int LC = EndList.Count % cycle;
                for (int i = 4; i < cycle + 1; i++)
                {
                    Request.Items = new List<Item>();
                    if (i == cycle)
                    {
                        for (int n = 99 * i; n < 99 * (i) + LC; n++)
                        {
                            Request.Items.Add(new Item(EndList[n]));
                        }
                        Response = Send(httpWebRequest, Request);
                    }
                    else
                    {
                        int F = (99 * (1 + i));
                        for (int n = 99 * i; n < F; n++)
                        {
                            Request.Items.Add(new Item(EndList[n]));
                        }
                        Response = Send(httpWebRequest, Request);
                    }
                }
            }
            else
            {
                for (int n = 0; n < EndList.Count; n++)
                {
                    Request.Items.Add(new Item(EndList[n]));
                }
                if (Request.Items.Count > 0)
                {
                    Response = Send(httpWebRequest, Request);
                }
               
            }
            return Response;
        }
        private Response Send(HttpWebRequest httpWebRequest, Request Request)
        {
            try
            {
                Response Response;
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = JsonConvert.SerializeObject(Request);
                    streamWriter.Write(json);
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }
                Response = JsonConvert.DeserializeObject<Response>(result);
                Thread.Sleep(100000);
                return Response;
            }
            catch (System.Exception e)
            {
                return null;
            }
        }
        public class Attribute
        {
            [JsonProperty("complex_id")]
            public int ComplexId { get; set; }
            [JsonProperty("id")]
            public int Id { get; set; }
            [JsonProperty("values")]
            public List<Value> Values { get; set; }
        }
        public class Item
        {
            public Item(IMarketItem item)
            {
                var ozItem = (SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post.ItemDesc)item;
                var ozDesc = (SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post.OzonPostGetAttr.Result)(ozItem.attributes[0]);
                var attr = (ozDesc).Attributes;
                Attributes = new List<Attribute>();
                foreach (var X in attr)
                {
                    var values = new List<Value>();
                    foreach (var Y in X.Values)
                    {
                        values.Add(new Value() { DictionaryValueId = Y.DictionaryValueId, _Value = Y.Value });
                    }
                    Attributes.Add(new Attribute() { ComplexId = X.ComplexId, Id = X.ID, Values = values });
                }
                string bar = null;
                if (ozItem.Barcodes.Count > 0)
                {
                    bar = ozItem.Barcodes?[0];
                }
                if (bar != null)
                {
                    Barcode = bar;
                }
                CategoryId = (int)ozItem.DescriptionCategoryId;
                ColorImage = ozItem.ColorImage.ToString();
                //ComplexAttributes = complexAttributes;
                CurrencyCode = "RUB";
                Depth = ozDesc.Depth;
                DimensionUnit = ozDesc.DimensionUnit;
                Height = ozDesc.Height;
                Images = new List<string>();
                foreach (var X in ozDesc.Images)
                { Images.Add(X.FileName); };
                Name = ozItem.Name;
                OfferId = ozItem.SKU;
                OldPrice = ozItem.OldPrice;

                Price = ozItem.Price;
                Vat = ozItem.Vat;
                Weight = ozDesc.Weight;
                WeightUnit = ozDesc.WeightUnit;
                Width = ozDesc.Width;
            }
            [JsonProperty("attributes")]
            public List<Attribute> Attributes { get; set; }
            [JsonProperty("barcode")]
            public string Barcode { get; set; }
            [JsonProperty("category_id")]
            public int CategoryId { get; set; }
            [JsonProperty("color_image")]
            public string ColorImage { get; set; }
            [JsonProperty("complex_attributes")]
            public List<object> ComplexAttributes { get; set; }
            [JsonProperty("currency_code")]
            public string CurrencyCode { get; set; }
            [JsonProperty("depth")]
            public int Depth { get; set; }
            [JsonProperty("dimension_unit")]
            public string DimensionUnit { get; set; }
            [JsonProperty("height")]
            public int Height { get; set; }
            [JsonProperty("images")]
            public List<string> Images { get; set; }
            [JsonProperty("images360")]
            public List<object> Images360 { get; set; }
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("offer_id")]
            public string OfferId { get; set; }
            [JsonProperty("old_price")]
            public string OldPrice { get; set; }
            [JsonProperty("pdf_list")]
            public List<object> PdfList { get; set; }
            [JsonProperty("premium_price")]
            public string PremiumPrice { get; set; }
            [JsonProperty("price")]
            public string Price { get; set; }
            [JsonProperty("primary_image")]
            public string PrimaryImage { get; set; }
            [JsonProperty("vat")]
            public string Vat { get; set; }
            [JsonProperty("weight")]
            public int Weight { get; set; }
            [JsonProperty("weight_unit")]
            public string WeightUnit { get; set; }
            [JsonProperty("width")]
            public int Width { get; set; }
        }
        public class Request
        {
            public Request()
            {
                Items = new List<Item>();
            }
            [JsonProperty("items")]
            public List<Item> Items { get; set; }
        }
        public class Value
        {
            [JsonProperty("dictionary_value_id")]
            public int DictionaryValueId { get; set; }
            [JsonProperty("value")]
            public string _Value { get; set; }
        }
        public class Result
        {
            [JsonProperty("task_id")]
            public int TaskId { get; set; }
            [JsonProperty("unmatched_sku_list")]
            public List<object> UnmatchedSkuList { get; set; }
        }
        public class Response
        {
            [JsonProperty("result")]
            public Result Result { get; set; }
        }
    }
}
