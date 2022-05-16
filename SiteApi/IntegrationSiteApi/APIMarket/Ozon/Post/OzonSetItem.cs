using Newtonsoft.Json;

using Server.Class.IntegrationSiteApi.Market.Ozon.OzonPost;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace Server.Class.IntegrationSiteApi.Market.Ozon
{
    public class OzonSetItem : Server.Class.IntegrationSiteApi.Market.Ozon.OzonPost.OzonPost
    {
        public OzonSetItem(StructLibCore.Marketplace.APISetting aPISetting) : base(aPISetting)
        {
        }

        private StructLibCore.Marketplace.IMarketItem[] Lst { get; set; }
        public object Get(StructLibCore.Marketplace.IMarketItem[] Lst)
        {
            this.Lst = Lst;
            List<IGrouping<StructLibCore.Marketplace.APISetting, StructLibCore.Marketplace.IMarketItem>> LST = ConvertListApi();
            foreach (IGrouping<StructLibCore.Marketplace.APISetting, StructLibCore.Marketplace.IMarketItem> item in LST)
            {
                ClientID = item.Key.ApiString[0];
                apiKey = item.Key.ApiString[1];

                HttpWebRequest httpWebRequest = GetRequest(@"v2/product/import");

                Root items = new Root() { items = ConverItems(item.ToList()) };

                using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string root = JsonConvert.SerializeObject(items);
                    streamWriter.Write(root);
                }
                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream());
                result = streamReader.ReadToEnd();

                // ResultRoot = JsonConvert.DeserializeObject<ResultRoot>(result);

            }
            return result;
        }
        private List<IGrouping<StructLibCore.Marketplace.APISetting, StructLibCore.Marketplace.IMarketItem>> ConvertListApi()
        {
            IEnumerable<IGrouping<StructLibCore.Marketplace.APISetting, StructLibCore.Marketplace.IMarketItem>> X = Lst.GroupBy(x => x.APISetting);
            List<IGrouping<StructLibCore.Marketplace.APISetting, StructLibCore.Marketplace.IMarketItem>> A = X.ToList();
            return A;
        }
        private List<SetOzonItem> ConverItems(List<StructLibCore.Marketplace.IMarketItem> lst)
        {
            List<SetOzonItem> Nlst = new List<SetOzonItem>();

            foreach (StructLibCore.Marketplace.IMarketItem item in lst)
            {
                Nlst.Add(new SetOzonItem((OzonItemDesc)item));
            }

            return Nlst;
        }
        public class SetOzonItem
        {
            public List<object> attributes { get; set; }
            public string barcode { get; set; }
            public int category_id { get; set; }
            public string color_image { get; set; }
            public List<object> complex_attributes { get; set; }
            public int depth { get; set; }
            //   public string dimension_unit { get; set; }
            public int height { get; set; }
            public List<string> images { get; set; }
            public List<object> images360 { get; set; }
            public string name { get; set; }
            public string offer_id { get; set; }
            public string old_price { get; set; }
            public List<object> pdf_list { get; set; }
            public string premium_price { get; set; }
            public string min_price { get; set; }
            public string price { get; set; }
            public string primary_image { get; set; }
            public string vat { get; set; }
            public int weight { get; set; }
            //   public string weight_unit { get; set; }
            public int width { get; set; }


            public SetOzonItem(OzonItemDesc itemDesc)
            {
                attributes = new List<object>();
                complex_attributes = new List<object>();
                pdf_list = new List<object>();
                barcode = itemDesc.barcode;
                category_id = itemDesc.category_id;
                color_image = itemDesc.color_image;
                images = itemDesc.images;
                images360 = itemDesc.images360;
                name = itemDesc.name;
                offer_id = itemDesc.offer_id;
                old_price = itemDesc.old_price;
                premium_price = itemDesc.premium_price;
                price = itemDesc.price;
                primary_image = itemDesc.primary_image;
                vat = itemDesc.vat;
                min_price = itemDesc.min_price;

            }

        }

        public class Root
        {
            public List<SetOzonItem> items { get; set; }
        }

        public class Result
        {
            [JsonProperty("task_id")]
            public string TaskId { get; set; }
        }

        public class ResultRoot
        {
            [JsonProperty("result")]
            public Result Result { get; set; }
        }

    }
}


