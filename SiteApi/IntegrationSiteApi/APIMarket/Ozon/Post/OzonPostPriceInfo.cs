using Newtonsoft.Json;
using StructLibCore;
using StructLibCore.Marketplace;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
namespace SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post
{
    internal class OzonPostPriceInfo : OzonPost
    {
       private string Error;

        public enum PriceInfoType
        {
            offer, product
        }
        public OzonPostPriceInfo(APISetting aPISetting) : base(aPISetting)
        {
        }
        public List<Item> Get(List<string> Filter, PriceInfoType type)
        {
            HttpWebRequest httpWebRequest = GetRequest(@"v4/product/info/prices");
            using (StreamWriter streamWriter = new(httpWebRequest.GetRequestStream()))
            {
                Request Request = new()
                {
                    Limit = 1000
                };
                switch (type)
                {
                    case PriceInfoType.offer:
                        Request.Filter.OfferId = Filter;
                        break;
                    case PriceInfoType.product:
                        Request.Filter.ProductId = Filter;
                        break;
                    default:
                        break;
                }
                string root = JsonConvert.SerializeObject(Request); streamWriter.Write(root);
            }
            Response response = new();
            try
            {
                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }
                response = JsonConvert.DeserializeObject<Response>(result);
            }
            catch (Exception e) { Error = e.Message; }
            return response?.Result?.Items;
        }
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Action
        {
            [JsonProperty("date_from")]
            public string DateFrom { get; set; }
            [JsonProperty("date_to")]
            public string DateTo { get; set; }
            [JsonProperty("title")]
            public string Title { get; set; }
            [JsonProperty("discount_value")]
            public string DiscountValue { get; set; }
        }
        public class Commissions
        {
            [JsonProperty("sales_percent")]
            public string SalesPercent { get; set; }
            [JsonProperty("fbo_fulfillment_amount")]
            public string FboFulfillmentAmount { get; set; }
            [JsonProperty("fbo_direct_flow_trans_min_amount")]
            public string FboDirectFlowTransMinAmount { get; set; }
            [JsonProperty("fbo_direct_flow_trans_max_amount")]
            public string FboDirectFlowTransMaxAmount { get; set; }
            [JsonProperty("fbo_deliv_to_customer_amount")]
            public string FboDelivToCustomerAmount { get; set; }
            [JsonProperty("fbo_return_flow_amount")]
            public string FboReturnFlowAmount { get; set; }
            [JsonProperty("fbo_return_flow_trans_min_amount")]
            public string FboReturnFlowTransMinAmount { get; set; }
            [JsonProperty("fbo_return_flow_trans_max_amount")]
            public string FboReturnFlowTransMaxAmount { get; set; }
            [JsonProperty("fbs_first_mile_min_amount")]
            public string FbsFirstMileMinAmount { get; set; }
            [JsonProperty("fbs_first_mile_max_amount")]
            public string FbsFirstMileMaxAmount { get; set; }
            [JsonProperty("fbs_direct_flow_trans_min_amount")]
            public string FbsDirectFlowTransMinAmount { get; set; }
            [JsonProperty("fbs_direct_flow_trans_max_amount")]
            public string FbsDirectFlowTransMaxAmount { get; set; }
            [JsonProperty("fbs_deliv_to_customer_amount")]
            public double FbsDelivToCustomerAmount { get; set; }
            [JsonProperty("fbs_return_flow_amount")]
            public string FbsReturnFlowAmount { get; set; }
            [JsonProperty("fbs_return_flow_trans_min_amount")]
            public string FbsReturnFlowTransMinAmount { get; set; }
            [JsonProperty("fbs_return_flow_trans_max_amount")]
            public string FbsReturnFlowTransMaxAmount { get; set; }
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
        public class Item
        {
            [JsonProperty("product_id")]
            public int ProductId { get; set; }
            [JsonProperty("offer_id")]
            public string OfferId { get; set; }
            [JsonProperty("price")]
            public Price Price { get; set; }
            [JsonProperty("price_index")]
            public string PriceIndex { get; set; }
            [JsonProperty("commissions")]
            public Commissions Commissions { get; set; }
            [JsonProperty("marketing_actions")]
            public MarketingActions MarketingActions { get; set; }
            [JsonProperty("volume_weight")]
            public double VolumeWeight { get; set; }
            [JsonProperty("price_indexes")]
            public PriceIndexes PriceIndexes { get; set; }
        }
        public class MarketingActions
        {
            [JsonProperty("current_period_from")]
            public DateTime? CurrentPeriodFrom { get; set; }
            [JsonProperty("current_period_to")]
            public DateTime? CurrentPeriodTo { get; set; }
            [JsonProperty("actions")]
            public List<Action> Actions { get; set; }
            [JsonProperty("ozon_actions_exist")]
            public bool OzonActionsExist { get; set; }
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
        public class Price
        {
            [JsonProperty("price")]
            public string PriceStr { get; set; }
            [JsonProperty("old_price")]
            public string OldPrice { get; set; }
            [JsonProperty("premium_price")]
            public string PremiumPrice { get; set; }
            [JsonProperty("recommended_price")]
            public string RecommendedPrice { get; set; }
            [JsonProperty("retail_price")]
            public string RetailPrice { get; set; }
            [JsonProperty("vat")]
            public string Vat { get; set; }
            [JsonProperty("min_ozon_price")]
            public string MinOzonPrice { get; set; }
            [JsonProperty("marketing_price")]
            public string MarketingPrice { get; set; }
            [JsonProperty("marketing_seller_price")]
            public string MarketingSellerPrice { get; set; }
            [JsonProperty("min_price")]
            public string MinPrice { get; set; }
            [JsonProperty("currency_code")]
            public string CurrencyCode { get; set; }
            [JsonProperty("auto_action_enabled")]
            public bool AutoActionEnabled { get; set; }
        }
        public class PriceIndexes
        {
            [JsonProperty("price_index")]
            public string PriceIndex { get; set; }
            [JsonProperty("external_index_data")]
            public ExternalIndexData ExternalIndexData { get; set; }
            [JsonProperty("ozon_index_data")]
            public OzonIndexData OzonIndexData { get; set; }
            [JsonProperty("self_marketplaces_index_data")]
            public SelfMarketplacesIndexData SelfMarketplacesIndexData { get; set; }
        }
        public class Result
        {
            [JsonProperty("items")]
            public List<Item> Items { get; set; }
            [JsonProperty("total")]
            public int Total { get; set; }
            [JsonProperty("last_id")]
            public string LastId { get; set; }
        }
        public class Response
        {
            [JsonProperty("result")]
            public Result Result { get; set; }
        }
        public class SelfMarketplacesIndexData
        {
            [JsonProperty("minimal_price")]
            public string MinimalPrice { get; set; }
            [JsonProperty("minimal_price_currency")]
            public string MinimalPriceCurrency { get; set; }
            [JsonProperty("price_index_value")]
            public double PriceIndexValue { get; set; }
        }
        public class Filter
        {
            [JsonProperty("offer_id")]
            public List<string> OfferId { get; set; }
            [JsonProperty("product_id")]
            public List<string> ProductId { get; set; }
            [JsonProperty("visibility")]
            public string Visibility { get; set; }
        }
        public class Request
        {
            public Request()
            {
                Filter = new Filter();
            }
            [JsonProperty("filter")]
            public Filter Filter { get; set; }
            [JsonProperty("last_id")]
            public string LastId { get; set; }
            [JsonProperty("limit")]
            public int Limit { get; set; }
        }
    }
}
