using Newtonsoft.Json;

using SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post;

using StructLibCore.Marketplace;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;

namespace Server.Class.IntegrationSiteApi.Market.Ozon
{
    public class OzonPostSetStoks : SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post.OzonPost
    {
        public OzonPostSetStoks(APISetting aPISetting) : base(aPISetting)
        {
        }
        public object Get(IMarketItem[] List)
        {
            Root itemsRoot = new Root();
            foreach (IMarketItem item in List)
            {
                itemsRoot.Prices.Add(new PriceItem(item));
            }
                var httpWebRequest = GetRequest(@"v1/product/import/prices");
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    var root = JsonConvert.SerializeObject(itemsRoot);
                    streamWriter.Write(root);
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using var streamReader = new StreamReader(httpResponse.GetResponseStream()); result = streamReader.ReadToEnd();

                var Rst = JsonConvert.DeserializeObject<ResultRoot>(result);

            return Rst.Result;
        }
        public enum AutoActionEnabled { UNKNOWN, ENABLED, DISABLED }


        public class PriceItem
        {
            [JsonProperty("auto_action_enabled")]
            public AutoActionEnabled AutoActionEnabled;

            [JsonProperty("min_price")]
            public string MinPrice;

            [JsonProperty("offer_id")]
            public string OfferId;

            [JsonProperty("old_price")]
            public string OldPrice;

            [JsonProperty("price")]
            public string Price;

            [JsonProperty("product_id")]
            public string ProductId;

            public PriceItem(IMarketItem ozonItemDesc, AutoActionEnabled autoActionEnabled = AutoActionEnabled.UNKNOWN)
            {
                MinPrice = ozonItemDesc.MinPrice.Replace(',', '.');
                OfferId = ozonItemDesc.SKU;

                //if (Convert.ToDouble(ozonItemDesc.OldPrice.Replace('.', ',')) < Convert.ToDouble(ozonItemDesc.Price.Replace('.', ',')))
                //{
                //    double oldPrice = Convert.ToDouble(ozonItemDesc.Price.Replace('.', ',')) * 1.1;

                //    OldPrice = oldPrice.ToString().Replace(',', '.');
                //}

                
                Price = ozonItemDesc.Price.Replace(',','.');
                ProductId = ozonItemDesc.MarketID;
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
            public string ProductId;

            [JsonProperty("reserved")]
            public int Reserved;

            [JsonProperty("warehouse_id")]
            public int WarehouseId;

            [JsonProperty("warehouse_name")]
            public string WarehouseName;
        }

        public class ResultRoot
        {
            [JsonProperty("result")]
            public List<Result> Result;
        }




    }
}


