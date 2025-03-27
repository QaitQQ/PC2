using StructLibCore;
using StructLibCore.Marketplace;

using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json.Serialization;
using System.Xml.Linq;
namespace SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post
{
    public abstract class OzonPost : IMarketApi
    {
        internal string ClientID;
        internal string apiKey;
        internal string result;
        internal APISetting aPISetting;
        public OzonPost(APISetting aPISetting)
        {
            this.aPISetting = aPISetting;
            ClientID = aPISetting.ApiString[0];
            apiKey = aPISetting.ApiString[1];
        }
        internal HttpWebRequest GetRequest(string Url)
        {
            string url = @"https://api-seller.ozon.ru/" + Url;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.Headers.Add("Host: api-seller.ozon.ru");
            httpWebRequest.Headers.Add("Client-Id:" + ClientID);
            httpWebRequest.Headers.Add("Api-Key:" + apiKey);
            return httpWebRequest;
        }
    }
    #region query desc
    [Serializable]
    public class Item
    {
        public int product_id { get; set; }
        public string offer_id { get; set; }
    }
    [Serializable]
    public class Result
    {
        public List<Item> items { get; set; }
        public int total { get; set; }
    }
    [Serializable]
    public class Root
    {
        public Result result { get; set; }
    }
    [Serializable]
    public class ItemQ
    {
        public ItemQ() { offer_id = new List<string>(); product_id = new List<object>(); sku = new List<object>(); }
        public List<string> offer_id { get; set; }
        public List<object> product_id { get; set; }
        public List<object> sku { get; set; }
    }
    [Serializable]
    public class Source
    {
        public bool is_enabled { get; set; }
        public int sku { get; set; }
        public string source { get; set; }
    }
    [Serializable]
    public class Stocks
    {
        public string coming { get; set; }
        public string present { get; set; }
        public string reserved { get; set; }
    }
    [Serializable]
    public class Reasons
    {
    }
    [Serializable]
    public class VisibilityDetails
    {
        public bool has_price { get; set; }
        public bool has_stock { get; set; }
        public bool active_product { get; set; }
        public Reasons reasons { get; set; }
    }
    [Serializable]
    public class Status
    {
        public string state { get; set; }
        public string state_failed { get; set; }
        public string moderate_status { get; set; }
        //  public List<object> decline_reasons { get; set; }
        public string validation_state { get; set; }
        public string state_name { get; set; }
        public string state_description { get; set; }
        public bool is_failed { get; set; }
        public bool is_created { get; set; }
        public string state_tooltip { get; set; }
        //   public List<object> item_errors { get; set; }
        public DateTime state_updated_at { get; set; }
    }
    [Serializable]
    public class Commission
    {
        [JsonPropertyName("delivery_amount")]
        public double? DeliveryAmount;

        [JsonPropertyName("percent")]
        public int? Percent;

        [JsonPropertyName("return_amount")]
        public int? ReturnAmount;

        [JsonPropertyName("sale_schema")]
        public string SaleSchema;

        [JsonPropertyName("value")]
        public double? Value;
    }
    [Serializable]
    public class ExternalIndexData
    {
        [JsonPropertyName("minimal_price")]
        public string MinimalPrice;

        [JsonPropertyName("minimal_price_currency")]
        public string MinimalPriceCurrency;

        [JsonPropertyName("price_index_value")]
        public int? PriceIndexValue;
    }
    [Serializable]
    public class ItemDesc : IMarketItem
    {

        public ItemDesc() { Barcodes = new List<string>(); }

        [JsonPropertyName("id")]
        public int? Id;

        [JsonPropertyName("name")]
        public string Name;

        [JsonPropertyName("offer_id")]
        public string OfferId;

        [JsonPropertyName("is_archived")]
        public bool? IsArchived;

        [JsonPropertyName("is_autoarchived")]
        public bool? IsAutoarchived;

        [JsonPropertyName("barcodes")]
        public List<string> Barcodes;

        [JsonPropertyName("description_category_id")]
        public int? DescriptionCategoryId;

        [JsonPropertyName("type_id")]
        public int? TypeId;

        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt;

        [JsonPropertyName("images")]
        public List<string> Images;

        [JsonPropertyName("currency_code")]
        public string CurrencyCode;

        [JsonPropertyName("marketing_price")]
        public string MarketingPrice;

        [JsonPropertyName("min_price")]
        public string MinPrice;

        [JsonPropertyName("old_price")]
        public string OldPrice;

        [JsonPropertyName("price")]
        public string Price;

        [JsonPropertyName("sources")]
        public List<Source> Sources;

        [JsonPropertyName("model_info")]
        public ModelInfo ModelInfo;

        [JsonPropertyName("commissions")]
        public List<Commission> Commissions;

        [JsonPropertyName("is_prepayment_allowed")]
        public bool? IsPrepaymentAllowed;

        [JsonPropertyName("volume_weight")]
        public double? VolumeWeight;

        [JsonPropertyName("has_discounted_fbo_item")]
        public bool? HasDiscountedFboItem;

        [JsonPropertyName("is_discounted")]
        public bool? IsDiscounted;

        [JsonPropertyName("discounted_fbo_stocks")]
        public int? DiscountedFboStocks;

        [JsonPropertyName("stocks")]
        public Stocks Stocks;

        [JsonPropertyName("errors")]
        public List<object> Errors;

        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt;

        [JsonPropertyName("vat")]
        public string Vat;

        [JsonPropertyName("visibility_details")]
        public VisibilityDetails_D VisibilityDetails;

        [JsonPropertyName("price_indexes")]
        public PriceIndexes PriceIndexes;

        [JsonPropertyName("images360")]
        public List<object> Images360;

        [JsonPropertyName("is_kgt")]
        public bool? IsKgt;

        [JsonPropertyName("color_image")]
        public List<object> ColorImage;

       
        public List<string> primary_image;

        [JsonPropertyName("statuses")]
        public Statuses Statuses;

        [JsonPropertyName("is_super")]
        public bool? IsSuper;
        public object Priceinfo { get; set; }

        public static explicit operator ItemDesc(List<IMarketItem> v)
        {
            throw new NotImplementedException();
        }

        public string SKU => OfferId;

        string IMarketItem.Stocks { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public APISetting APISetting { get; set; }

        public APISetting APISettingSource { get; set; }
        public List<string> Pic
        {
            get
            {
                var nlist = new List<string>();

                if (primary_image != null )
                {
                    nlist.Add(primary_image[0]);
                }


                    foreach (var item in Images)
                {
                    nlist.Add(item);
                }
                return nlist;
            }

        }
        public List<object> attributes { get; set; }
        string IMarketItem.Name  { get { return Name; } set { Name = value; } }

        string IMarketItem.Price { get { return Price; } set { Price = value; } }
        string IMarketItem.MinPrice { get { return MinPrice; } set { MinPrice = value; } }
        List<string> IMarketItem.Barcodes { get { return Barcodes; } set { Barcodes = value; } }
    }
    [Serializable]
    public class ModelInfo
    {
        [JsonPropertyName("model_id")]
        public int? ModelId;

        [JsonPropertyName("count")]
        public int? Count;
    }
    [Serializable]
    public class OzonIndexData
    {
        [JsonPropertyName("minimal_price")]
        public string MinimalPrice;

        [JsonPropertyName("minimal_price_currency")]
        public string MinimalPriceCurrency;

        [JsonPropertyName("price_index_value")]
        public int? PriceIndexValue;
    }
    [Serializable]
    public class PriceIndexes
    {
        [JsonPropertyName("color_index")]
        public string ColorIndex;

        [JsonPropertyName("external_index_data")]
        public ExternalIndexData ExternalIndexData;

        [JsonPropertyName("ozon_index_data")]
        public OzonIndexData OzonIndexData;

        [JsonPropertyName("self_marketplaces_index_data")]
        public SelfMarketplacesIndexData SelfMarketplacesIndexData;
    }
    [Serializable]
    public class Root_D
    {
        [JsonPropertyName("items")]
        public List<ItemDesc> Items;
    }
    [Serializable]
    public class SelfMarketplacesIndexData
    {
        [JsonPropertyName("minimal_price")]
        public string MinimalPrice;

        [JsonPropertyName("minimal_price_currency")]
        public string MinimalPriceCurrency;

        [JsonPropertyName("price_index_value")]
        public int? PriceIndexValue;
    }
    [Serializable]
    public class Source_D
    {
        [JsonPropertyName("sku")]
        public int? Sku;

        [JsonPropertyName("source")]
        public string Source;

        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt;

        [JsonPropertyName("shipment_type")]
        public string ShipmentType;

        [JsonPropertyName("quant_code")]
        public string QuantCode;
    }
    [Serializable]
    public class Statuses
    {
        [JsonPropertyName("status")]
        public string Status;

        [JsonPropertyName("status_failed")]
        public string StatusFailed;

        [JsonPropertyName("moderate_status")]
        public string ModerateStatus;

        [JsonPropertyName("validation_status")]
        public string ValidationStatus;

        [JsonPropertyName("status_name")]
        public string StatusName;

        [JsonPropertyName("status_description")]
        public string StatusDescription;

        [JsonPropertyName("is_created")]
        public bool? IsCreated;

        [JsonPropertyName("status_tooltip")]
        public string StatusTooltip;

        [JsonPropertyName("status_updated_at")]
        public DateTime? StatusUpdatedAt;
    }
    [Serializable]
    public class Stock
    {
        [JsonPropertyName("present")]
        public int? Present;

        [JsonPropertyName("reserved")]
        public int? Reserved;

        [JsonPropertyName("sku")]
        public int? Sku;

        [JsonPropertyName("source")]
        public string Source;

        [JsonPropertyName("has_stock")]
        public bool? HasStock;

        [JsonPropertyName("stocks")]
        public List<Stock> Stocks;
    }
    [Serializable]
    public class VisibilityDetails_D
    {
        [JsonPropertyName("has_price")]
        public bool? HasPrice;

        [JsonPropertyName("has_stock")]
        public bool? HasStock;
    }











    #endregion
}
