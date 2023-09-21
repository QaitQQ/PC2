using Newtonsoft.Json;

using SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post;

using StructLibCore.Marketplace;

using System.Net;

using static SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post.OzonPostGetAttr;

namespace Server.Class.IntegrationSiteApi.Market.Ozon
{
    public class OzonPostSetItem : SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post.OzonPost
    {
        public OzonPostSetItem(APISetting aPISetting) : base(aPISetting)
        {
        }
        private IMarketItem[]? Lst { get; set; }
        public object? Get(IMarketItem[] Lst)
        {
            this.Lst = Lst;

            List<string> LstCat = new List<string>();
            List<string> LstIDs = new List<string>();


            foreach (IMarketItem item in Lst)
            {
                OzonItemDesc Oitm = (OzonItemDesc)item;

                LstCat.Add(Oitm.category_id.ToString());
                LstIDs.Add(Oitm.id.ToString());
            }
            OzonPostGetAttFromCat.Response CatAttr = new OzonPostGetAttFromCat(aPISetting!).Get(LstCat);

            OzonPostGetAttr.Response? ItmAttr = new OzonPostGetAttr(aPISetting!).Get(LstIDs);




            List<IGrouping<APISetting, IMarketItem>> LST = ConvertListApi();
            foreach (IGrouping<APISetting, IMarketItem> item in LST)
            {
                ClientID = item.Key?.ApiString?[0]!;
                apiKey = item.Key?.ApiString?[1]!;
                HttpWebRequest httpWebRequest = GetRequest(@"v2/product/import");




                Root items = new Root() { items = ConverItems(item.ToList(), ItmAttr?.Result!, CatAttr.Result!) };
                using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string root = JsonConvert.SerializeObject(items);
                    streamWriter.Write(root);
                }
                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream());
                result = streamReader.ReadToEnd();

            }
            return result;
        }
        private List<IGrouping<APISetting, IMarketItem>> ConvertListApi()
        {
            IEnumerable<IGrouping<APISetting, IMarketItem>> X = Lst?.GroupBy(x => x.APISetting)!;
            List<IGrouping<APISetting, IMarketItem>> A = X.ToList();
            return A;
        }
        private List<SetOzonItem> ConverItems(List<IMarketItem> lst, List<OzonPostGetAttr.Result> ItmAttr, List<OzonPostGetAttFromCat.Result> CatAttr)
        {
            List<SetOzonItem> Nlst = new List<SetOzonItem>();
            foreach (IMarketItem item in lst)
            {
                OzonItemDesc Oitm = (OzonItemDesc)item;

                OzonPostGetAttr.Result? Attribute = ItmAttr.First(x => x.Id == Oitm.id);
                List<Attribute> LSTAttr = new List<Attribute>();

                OzonPostGetAttFromCat.Result CatArrt = CatAttr.First(x => x.CategoryId == Oitm.category_id);

                foreach (OzonPostGetAttFromCat.Attribute X in CatArrt?.Attributes!)
                {
                    if (Attribute?.Attributes!.FirstOrDefault(x => x.ID == X.Id) == null)
                    {
                        LSTAttr.Add(new Attribute() { AttributeId = X.Id, Values = new List<OzonPostGetAttr.OzValue>() { new OzonPostGetAttr.OzValue() { DictionaryValueId = X.DictionaryId, Value = "" } } });
                    }
                }



                foreach (OzonPostGetAttr.Attribute X in Attribute?.Attributes!)
                {
                    LSTAttr.Add(new Attribute(X));
                }
                Nlst.Add(new SetOzonItem((OzonItemDesc)item) { attributes = LSTAttr, weight = Attribute.Weight, depth = Attribute.Depth, width = Attribute.Width, height = Attribute.Height });
            }
            return Nlst;
        }
        public class Attribute : IAttribute
        {
            public Attribute()
            {
                Values = new List<OzValue>();
            }

            public Attribute(IAttribute attribute)
            {
                AttributeId = attribute.ID;
                ComplexId = attribute.ComplexId;
                Values = attribute.Values;

            }

            [JsonProperty("id")]
            public int AttributeId { get; set; }
            [JsonProperty("complex_id")]
            public int ComplexId { get; set; }
            [JsonProperty("values")]
            public List<OzValue> Values { get; set; }
            public int ID { get { return AttributeId; } set { AttributeId = value; } }
        }
        public class SetOzonItem
        {
            //weight_unit
            //dimension_unit
            //attributes
            public List<Attribute> attributes { get; set; }
            public string? barcode { get; set; }
            //  public List<string> barcodes { get; set; }
            public int category_id { get; set; }
            public string? color_image { get; set; }
            public List<object> complex_attributes { get; set; }
            public int depth { get; set; }
            public string? dimension_unit { get; set; }
            public int height { get; set; }
            public List<string> images { get; set; }
            public List<object> images360 { get; set; }
            public string? name { get; set; }
            public string? offer_id { get; set; }
            public string? old_price { get; set; }
            public List<object> pdf_list { get; set; }
            public string? premium_price { get; set; }
            public string? min_price { get; set; }
            public string? price { get; set; }
            public string? primary_image { get; set; }
            public string? vat { get; set; }
            public int weight { get; set; }
            public string? weight_unit { get; set; }
            public int width { get; set; }
            public SetOzonItem(OzonItemDesc itemDesc)
            {
                attributes = new List<Attribute>();
                complex_attributes = new List<object>();
                pdf_list = new List<object>();
                barcode = itemDesc.barcodes?[1];
                //  barcodes = itemDesc.barcodes;
                category_id = itemDesc.category_id;
                color_image = itemDesc.color_image;
                images = itemDesc.images!;
                images360 = itemDesc.images360!;
                name = itemDesc.name;
                offer_id = itemDesc.offer_id;
                old_price = itemDesc.old_price;
                premium_price = itemDesc.premium_price;
                price = itemDesc.price;
                primary_image = itemDesc.primary_image;
                vat = itemDesc.vat;
                min_price = itemDesc.min_price;
                weight_unit = "g";
                dimension_unit = "mm";

            }
        }
        public class Root
        {
            public List<SetOzonItem>? items { get; set; }
        }
        public class Result
        {
            [JsonProperty("task_id")]
            public string? TaskId { get; set; }
        }
        public class ResultRoot
        {
            [JsonProperty("result")]
            public Result? Result { get; set; }
        }
    }
}
