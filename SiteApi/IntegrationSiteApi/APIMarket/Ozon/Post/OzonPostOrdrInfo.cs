using Newtonsoft.Json;

using StructLibCore.Marketplace;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post
{
    public class OzonPostOrdrInfo : Server.Class.IntegrationSiteApi.Market.Ozon.OzonPost.OzonPost
    {

        public OzonPostOrdrInfo(APISetting aPISetting) : base(aPISetting)
        {
        }
        public object Get(string postingNumber)
        {
            var httpWebRequest = GetRequest(@"v3/posting/fbs/get");
            var root = new PostQwery(postingNumber) { };
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                var json = JsonConvert.SerializeObject(root); streamWriter.Write(json);
            }
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }

                ResultRoot OrderList = JsonConvert.DeserializeObject<ResultRoot>(result);

                return OrderList.Result;
            }
            catch 
            {
                return null;
            }

        }
        public class PostQwery
        {
            [JsonProperty("posting_number")]
            public string PostingNumber;

            [JsonProperty("with")]
            public With With;

            public PostQwery(string postingNumber)
            {
                PostingNumber = postingNumber;
            }
        }

        public class With
        {
            [JsonProperty("analytics_data")]
            public bool AnalyticsData;

            [JsonProperty("barcodes")]
            public bool Barcodes;

            [JsonProperty("financial_data")]
            public bool FinancialData;

            [JsonProperty("translit")]
            public bool Translit;
        }

        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Cancellation
        {
            [JsonProperty("cancel_reason_id")]
            public int CancelReasonId;

            [JsonProperty("cancel_reason")]
            public string CancelReason;

            [JsonProperty("cancellation_type")]
            public string CancellationType;

            [JsonProperty("cancelled_after_ship")]
            public bool CancelledAfterShip;

            [JsonProperty("affect_cancellation_rating")]
            public bool AffectCancellationRating;

            [JsonProperty("cancellation_initiator")]
            public string CancellationInitiator;
        }

        public class DeliveryMethod
        {
            [JsonProperty("id")]
            public long Id;

            [JsonProperty("name")]
            public string Name;

            [JsonProperty("warehouse_id")]
            public long WarehouseId;

            [JsonProperty("warehouse")]
            public string Warehouse;

            [JsonProperty("tpl_provider_id")]
            public int TplProviderId;

            [JsonProperty("tpl_provider")]
            public string TplProvider;
        }

        public class Dimensions
        {
            [JsonProperty("height")]
            public string Height;

            [JsonProperty("length")]
            public string Length;

            [JsonProperty("weight")]
            public string Weight;

            [JsonProperty("width")]
            public string Width;
        }

        public class Product
        {
            [JsonProperty("price")]
            public string Price;

            [JsonProperty("offer_id")]
            public string OfferId;

            [JsonProperty("name")]
            public string Name;

            [JsonProperty("sku")]
            public int Sku;

            [JsonProperty("quantity")]
            public int Quantity;

            [JsonProperty("mandatory_mark")]
            public List<object> MandatoryMark;

            [JsonProperty("dimensions")]
            public Dimensions Dimensions;

            [JsonProperty("currency_code")]
            public string CurrencyCode;
        }

        public class Requirements
        {
            [JsonProperty("products_requiring_gtd")]
            public List<object> ProductsRequiringGtd;

            [JsonProperty("products_requiring_country")]
            public List<object> ProductsRequiringCountry;

            [JsonProperty("products_requiring_mandatory_mark")]
            public List<object> ProductsRequiringMandatoryMark;

            [JsonProperty("products_requiring_rnpt")]
            public List<object> ProductsRequiringRnpt;
        }

        public class Result
        {
            [JsonProperty("posting_number")]
            public string PostingNumber;

            [JsonProperty("order_id")]
            public int OrderId;

            [JsonProperty("order_number")]
            public string OrderNumber;

            [JsonProperty("status")]
            public string Status;

            [JsonProperty("delivery_method")]
            public DeliveryMethod DeliveryMethod;

            [JsonProperty("tracking_number")]
            public string TrackingNumber;

            [JsonProperty("tpl_integration_type")]
            public string TplIntegrationType;

            [JsonProperty("in_process_at")]
            public string InProcessAt;

            [JsonProperty("shipment_date")]
            public string ShipmentDate;

            [JsonProperty("delivering_date")]
            public string DeliveringDate;

            [JsonProperty("provider_status")]
            public string ProviderStatus;

            [JsonProperty("delivery_price")]
            public string DeliveryPrice;

            [JsonProperty("cancellation")]
            public Cancellation Cancellation;

            [JsonProperty("customer")]
            public object Customer;

            [JsonProperty("addressee")]
            public object Addressee;

            [JsonProperty("products")]
            public List<Product> Products;

            [JsonProperty("barcodes")]
            public object Barcodes;

            [JsonProperty("analytics_data")]
            public object AnalyticsData;

            [JsonProperty("financial_data")]
            public object FinancialData;

            [JsonProperty("additional_data")]
            public List<object> AdditionalData;

            [JsonProperty("is_express")]
            public bool IsExpress;

            [JsonProperty("requirements")]
            public Requirements Requirements;

            [JsonProperty("product_exemplars")]
            public object ProductExemplars;

            [JsonProperty("courier")]
            public object Courier;
        }

        public class ResultRoot
        {
            [JsonProperty("result")]
            public Result Result;
        }


    }
}
