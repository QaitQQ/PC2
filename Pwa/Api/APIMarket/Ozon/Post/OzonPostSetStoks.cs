using Newtonsoft.Json;

using SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post;

using StructLibCore.Marketplace;

using System.Net;

namespace Server.Class.IntegrationSiteApi.Market.Ozon
{
    public class OzonPostSetStoks : SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post.OzonPost
    {
        public OzonPostSetStoks(APISetting aPISetting) : base(aPISetting)
        {
        }
        public object? Get(IMarketItem[] List)
        {
            Root itemsRoot = new Root();
            foreach (OzonItemDesc item in List)
            {
                itemsRoot.Prices.Add(new PriceItem(item));
            }
            HttpWebRequest httpWebRequest = GetRequest(@"v1/product/import/prices");
            using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string root = JsonConvert.SerializeObject(itemsRoot);
                streamWriter.Write(root);
            }
            HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()); result = streamReader.ReadToEnd();

            ResultRoot? Rst = JsonConvert.DeserializeObject<ResultRoot>(result);

            return Rst?.Result;
        }
        public enum AutoActionEnabled { UNKNOWN, ENABLED, DISABLED }


        public class PriceItem
        {
            [JsonProperty("auto_action_enabled")]
            public AutoActionEnabled AutoActionEnabled;

            [JsonProperty("min_price")]
            public string? MinPrice;

            [JsonProperty("offer_id")]
            public string? OfferId;

            [JsonProperty("old_price")]
            public string? OldPrice;

            [JsonProperty("price")]
            public string? Price;

            [JsonProperty("product_id")]
            public int ProductId;

            public PriceItem(OzonItemDesc ozonItemDesc, AutoActionEnabled autoActionEnabled = AutoActionEnabled.UNKNOWN)
            {
                if (ozonItemDesc != null)
                {
                    MinPrice = ozonItemDesc.min_price!.Replace(',', '.');
                    OfferId = ozonItemDesc.offer_id!;
                    OldPrice = ozonItemDesc.old_price!;
                    Price = ozonItemDesc.price!.Replace(',', '.');
                    ProductId = ozonItemDesc.id;
                }

            }
        }
        public class Root
        {
            [JsonProperty("prices")]
            public List<PriceItem> Prices;

            public Root()
            {
                Prices = new List<PriceItem>();
            }
        }

        public class Result
        {
            [JsonProperty("fbs_sku")]
            public int FbsSku;

            [JsonProperty("present")]
            public int Present;

            [JsonProperty("product_id")]
            public int ProductId;

            [JsonProperty("reserved")]
            public int Reserved;

            [JsonProperty("warehouse_id")]
            public int WarehouseId;

            [JsonProperty("warehouse_name")]
            public string? WarehouseName;
        }

        public class ResultRoot
        {
            [JsonProperty("result")]
            public List<Result>? Result;
        }




    }
}


