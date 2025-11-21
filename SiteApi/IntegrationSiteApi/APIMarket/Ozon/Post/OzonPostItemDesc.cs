using Newtonsoft.Json;

using SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post;

using StructLibCore.Marketplace;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace Server.Class.IntegrationSiteApi.Market.Ozon
{
    public class OzonPostItemDesc : SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post.OzonPost
    {
        public OzonPostItemDesc(APISetting aPISetting) : base(aPISetting)
        {
        }

        public List<IMarketItem> Get(List<string> Ids = null)
        {
            var httpWebRequest = GetRequest(@"v3/product/info/list");
            ItemQ itemQ = new ItemQ();
            if (Ids == null)
            {
                var Lst = new OzonPostItemList(this.aPISetting).Get();               
                foreach (var item in Lst) { itemQ.offer_id.Add(item.OfferId); }
            }
            else
            {
                foreach (string item in Ids) { itemQ.offer_id.Add(item); }
            }
            List<IMarketItem> NLST = new List<IMarketItem>();
            try
            {
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream())) { 
                    var root = JsonConvert.SerializeObject(itemQ);
                    streamWriter.Write(root); }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }
                Response response = JsonConvert.DeserializeObject<Response>(result);


                List<string> IDS = new List<string>();

                foreach (var item in response.Items) { IDS.Add(item.Id.ToString()); }

                List<OzonPostPriceInfo.Item> PriceInfoList = new OzonPostPriceInfo(aPISetting).Get(IDS, OzonPostPriceInfo.PriceInfoType.product);

                foreach (var item in response.Items) { item.Priceinfo = PriceInfoList?.FirstOrDefault(x => x.ProductId == item.Id); NLST.Add(item); }

            }
            catch (System.Exception e)
            {
                return NLST;

            }


            return NLST;
        }

        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Commission
        {
            [JsonProperty("delivery_amount")]
            public double DeliveryAmount { get; set; }

            [JsonProperty("percent")]
            public double Percent { get; set; }

            [JsonProperty("return_amount")]
            public double ReturnAmount { get; set; }

            [JsonProperty("sale_schema")]
            public string SaleSchema { get; set; }

            [JsonProperty("value")]
            public double Value { get; set; }
        }

        public class ExternalIndexData
        {
            [JsonProperty("minimal_price")]
            public string MinimalPrice { get; set; }

            [JsonProperty("minimal_price_currency")]
            public string MinimalPriceCurrency { get; set; }

            [JsonProperty("price_index_value")]
            public double PriceIndexValue { get; set; }
        }

        public class Item:IMarketItem
        {
            public Item()
            {
                this.stocks = new Stock_Desc();
                this.ModelInfo = new ModelInfo();
                this.PriceIndexes = new PriceIndexes();
                this.Statuses = new Statuses();
            }

            public object Priceinfo { get; set; }
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("offer_id")]
            public string OfferId { get; set; }

            [JsonProperty("is_archived")]
            public bool IsArchived { get; set; }

            [JsonProperty("is_autoarchived")]
            public bool IsAutoarchived { get; set; }

            [JsonProperty("barcodes")]
            public List<string> Barcodes { get; set; }

            [JsonProperty("description_category_id")]
            public string DescriptionCategoryId { get; set; }

            [JsonProperty("type_id")]
            public string TypeId { get; set; }

            [JsonProperty("created_at")]
            public DateTime CreatedAt { get; set; }

            [JsonProperty("images")]
            public List<string> Images { get; set; }

            [JsonProperty("currency_code")]
            public string CurrencyCode { get; set; }

            [JsonProperty("marketing_price")]
            public string MarketingPrice { get; set; }

            [JsonProperty("min_price")]
            public string MinPrice { get; set; }

            [JsonProperty("old_price")]
            public string OldPrice { get; set; }

            [JsonProperty("price")]
            public string Price { get; set; }

            [JsonProperty("sources")]
            public List<Source> Sources { get; set; }

            [JsonProperty("model_info")]
            public ModelInfo ModelInfo { get; set; }

            [JsonProperty("commissions")]
            public List<Commission> Commissions { get; set; }

            [JsonProperty("is_prepayment_allowed")]
            public bool IsPrepaymentAllowed { get; set; }

            [JsonProperty("volume_weight")]
            public double VolumeWeight { get; set; }

            [JsonProperty("has_discounted_fbo_item")]
            public bool HasDiscountedFboItem { get; set; }

            [JsonProperty("is_discounted")]
            public bool IsDiscounted { get; set; }

            [JsonProperty("discounted_fbo_stocks")]
            public double DiscountedFboStocks { get; set; }

            [JsonProperty("stocks")]
            public Stock_Desc stocks { get; set; }

            [JsonProperty("errors")]
            public List<object> Errors { get; set; }

            [JsonProperty("updated_at")]
            public DateTime UpdatedAt { get; set; }

            [JsonProperty("vat")]
            public string Vat { get; set; }

            [JsonProperty("visibility_details")]
            public VisibilityDetails VisibilityDetails { get; set; }

            [JsonProperty("price_indexes")]
            public PriceIndexes PriceIndexes { get; set; }

            [JsonProperty("images360")]
            public List<object> Images360 { get; set; }

            [JsonProperty("is_kgt")]
            public bool IsKgt { get; set; }

            [JsonProperty("color_image")]
            public List<object> ColorImage { get; set; }

            [JsonProperty("primary_image")]
            public List<string> PrimaryImage { get; set; }

            [JsonProperty("statuses")]
            public Statuses Statuses { get; set; }

            [JsonProperty("is_super")]
            public bool IsSuper { get; set; }

            [JsonProperty("is_seasonal")]
            public bool IsSeasonal { get; set; }

            public string SKU => OfferId;

            public string Stocks { get {

                    if (stocks.Stocks != null && stocks.Stocks.Count > 0)
                    {
                        var x = stocks.Stocks.FirstOrDefault(x => x.Source == "fbs");
                        var z = x.Present;
                        return z.ToString();
                    }
                    return "0";
                } set {

                    var x = stocks.Stocks.FirstOrDefault(x => x.Source == "fbs");
                    x.Present = int.Parse(value);
                    } }
            public APISetting APISetting { get; set; }
            public APISetting APISettingSource { get; set; }
            public List<string> Pic {
                get {
                
                var lst = new List<string>();
                    foreach (var item in Images)
                    {
                        lst.Add(item);
                    }
                    foreach (var item in PrimaryImage)
                    {
                        lst.Add(item);
                    }
                    return lst;
                }  }
            public List<object> attributes { get; set; }

            public string MarketID { get { return Id; } }
        }

        public class ModelInfo
        {
            [JsonProperty("model_id")]
            public int ModelId { get; set; }

            [JsonProperty("count")]
            public double Count { get; set; }
        }

        public class OzonIndexData
        {
            [JsonProperty("minimal_price")]
            public string MinimalPrice { get; set; }

            [JsonProperty("minimal_price_currency")]
            public string MinimalPriceCurrency { get; set; }

            [JsonProperty("price_index_value")]
            public double PriceIndexValue { get; set; }
        }

        public class PriceIndexes
        {
            [JsonProperty("color_index")]
            public string ColorIndex { get; set; }

            [JsonProperty("external_index_data")]
            public ExternalIndexData ExternalIndexData { get; set; }

            [JsonProperty("ozon_index_data")]
            public OzonIndexData OzonIndexData { get; set; }

            [JsonProperty("self_marketplaces_index_data")]
            public SelfMarketplacesIndexData SelfMarketplacesIndexData { get; set; }
        }

        public class Response
        {
            [JsonProperty("items")]
            public List<Item> Items { get; set; }
        }

        public class SelfMarketplacesIndexData
        {
            [JsonProperty("minimal_price")]
            public string MinimalPrice { get; set; }

            [JsonProperty("minimal_price_currency")]
            public string MinimalPriceCurrency { get; set; }

            [JsonProperty("price_index_value")]
            public int PriceIndexValue { get; set; }
        }

        public class Source
        {
            [JsonProperty("sku")]
            public long Sku { get; set; }

            [JsonProperty("source")]
            public string source { get; set; }

            [JsonProperty("created_at")]
            public DateTime CreatedAt { get; set; }

            [JsonProperty("shipment_type")]
            public string ShipmentType { get; set; }

            [JsonProperty("quant_code")]
            public string QuantCode { get; set; }
        }

        public class Statuses
        {
            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("status_failed")]
            public string StatusFailed { get; set; }

            [JsonProperty("moderate_status")]
            public string ModerateStatus { get; set; }

            [JsonProperty("validation_status")]
            public string ValidationStatus { get; set; }

            [JsonProperty("status_name")]
            public string StatusName { get; set; }

            [JsonProperty("status_description")]
            public string StatusDescription { get; set; }

            [JsonProperty("is_created")]
            public bool IsCreated { get; set; }

            [JsonProperty("status_tooltip")]
            public string StatusTooltip { get; set; }

            [JsonProperty("status_updated_at")]
            public DateTime StatusUpdatedAt { get; set; }
        }

        public class Stock_Desc
        {
            [JsonProperty("present")]
            public double Present { get; set; }

            [JsonProperty("reserved")]
            public double Reserved { get; set; }

            [JsonProperty("sku")]
            public long Sku { get; set; }

            [JsonProperty("source")]
            public string Source { get; set; }

            [JsonProperty("has_stock")]
            public bool HasStock { get; set; }

            [JsonProperty("stocks")]
            public List<Stock> Stocks { get; set; }
        }

        public class VisibilityDetails
        {
            [JsonProperty("has_price")]
            public bool HasPrice { get; set; }

            [JsonProperty("has_stock")]
            public bool HasStock { get; set; }
        }





    }
}


