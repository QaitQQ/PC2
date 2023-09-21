using Newtonsoft.Json;

using StructLibCore.Marketplace;

using System.Net;

namespace SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post
{
    public class OzonPostMonthOrderList : OzonPost
    {
        public OzonPostMonthOrderList(APISetting aPISetting) : base(aPISetting)
        {
        }
        public Result Get()
        {
            HttpWebRequest httpWebRequest = GetRequest(@"v3/posting/fbs/list");
            DateTime today = DateTime.Today;
            DateTime month = new DateTime(today.Year, today.Month, 1);
            DateTime first = month.AddMonths(-1);
            DateTime last = month.AddDays(-1);
            using (StreamWriter streamWriter = new(httpWebRequest.GetRequestStream())) { string root = JsonConvert.SerializeObject(new Request(Status.delivered, OzDate(first), OzDate(last))); streamWriter.Write(root); }
            HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (StreamReader streamReader = new(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }

            Response End = JsonConvert.DeserializeObject<Response>(result)!;


            return End.Result!;
        }

        public enum Status
        {
            awaiting_registration,
            acceptance_in_progress,
            awaiting_approve,
            awaiting_packaging,
            awaiting_deliver,
            arbitration,
            client_arbitration,
            delivering,
            driver_pickup,
            delivered,
            cancelled,
            not_accepted,
            sent_by_seller
        }


        private string OzDate(DateTime dateTime)
        {
            return dateTime.ToString("u").Replace(" ", "T");
        }


        public class Filter
        {
            [JsonProperty("since")]
            public string? Since { get; set; }

            [JsonProperty("status")]
            public string? Status { get; set; }
            public void SetStatus(Status status) { Status = status.ToString(); }

            [JsonProperty("to")]
            public string? To { get; set; }
        }

        public class Request
        {
            public Request(Status status, string At, string To)
            {
                Dir = "ASC";
                Filter fl = new Filter();
                fl.SetStatus(status);
                fl.Since = At; fl.To = To;
                Filter = fl;
                Limit = "1000";
                Offset = "0";
                Translit = true;
                With = new With(true, true);
            }

            [JsonProperty("dir")]
            public string? Dir { get; set; }

            [JsonProperty("filter")]
            public Filter Filter { get; set; }

            [JsonProperty("limit")]
            public string? Limit { get; set; }

            [JsonProperty("offset")]
            public string? Offset { get; set; }

            [JsonProperty("translit")]
            public bool Translit { get; set; }

            [JsonProperty("with")]
            public With With { get; set; }
        }

        public class With
        {
            public With(bool analyticsData, bool financialData)
            {
                AnalyticsData = analyticsData;
                FinancialData = financialData;
            }

            [JsonProperty("analytics_data")]
            public bool AnalyticsData { get; set; }

            [JsonProperty("financial_data")]
            public bool FinancialData { get; set; }
        }

        public class AnalyticsData
        {
            [JsonProperty("region")]
            public string? Region { get; set; }

            [JsonProperty("city")]
            public string? City { get; set; }

            [JsonProperty("delivery_type")]
            public string? DeliveryType { get; set; }

            [JsonProperty("is_premium")]
            public bool IsPremium { get; set; }

            [JsonProperty("payment_type_group_name")]
            public string? PaymentTypeGroupName { get; set; }

            [JsonProperty("warehouse_id")]
            public string? WarehouseId { get; set; }

            [JsonProperty("warehouse")]
            public string? Warehouse { get; set; }

            [JsonProperty("tpl_provider_id")]
            public string? TplProviderId { get; set; }

            [JsonProperty("tpl_provider")]
            public string? TplProvider { get; set; }

            [JsonProperty("delivery_date_begin")]
            public string? DeliveryDateBegin { get; set; }

            [JsonProperty("delivery_date_end")]
            public string? DeliveryDateEnd { get; set; }

            [JsonProperty("is_legal")]
            public bool IsLegal { get; set; }
        }

        public class Cancellation
        {
            [JsonProperty("cancel_reason_id")]
            public string? CancelReasonId { get; set; }

            [JsonProperty("cancel_reason")]
            public string? CancelReason { get; set; }

            [JsonProperty("cancellation_type")]
            public string? CancellationType { get; set; }

            [JsonProperty("cancelled_after_ship")]
            public bool CancelledAfterShip { get; set; }

            [JsonProperty("affect_cancellation_rating")]
            public bool AffectCancellationRating { get; set; }

            [JsonProperty("cancellation_initiator")]
            public string? CancellationInitiator { get; set; }
        }

        public class DeliveryMethod
        {
            [JsonProperty("id")]
            public string? Id { get; set; }

            [JsonProperty("name")]
            public string? Name { get; set; }

            [JsonProperty("warehouse_id")]
            public string? WarehouseId { get; set; }

            [JsonProperty("warehouse")]
            public string? Warehouse { get; set; }

            [JsonProperty("tpl_provider_id")]
            public string? TplProviderId { get; set; }

            [JsonProperty("tpl_provider")]
            public string? TplProvider { get; set; }
        }

        public class FinancialData
        {

            [JsonProperty("products")]
            public List<Product>? Products { get; set; }

            [JsonProperty("posting_services")]
            public PostingServices? PostingServices { get; set; }

            [JsonProperty("cluster_from")]
            public string? ClusterFrom { get; set; }

            [JsonProperty("cluster_to")]
            public string? ClusterTo { get; set; }

        }

        public class ItemServices
        {
            [JsonProperty("marketplace_service_item_fulfillment")]
            public string? MarketplaceServiceItemFulfillment { get; set; }

            [JsonProperty("marketplace_service_item_pickup")]
            public string? MarketplaceServiceItemPickup { get; set; }

            [JsonProperty("marketplace_service_item_dropoff_pvz")]
            public string? MarketplaceServiceItemDropoffPvz { get; set; }

            [JsonProperty("marketplace_service_item_dropoff_sc")]
            public string? MarketplaceServiceItemDropoffSc { get; set; }

            [JsonProperty("marketplace_service_item_dropoff_ff")]
            public string? MarketplaceServiceItemDropoffFf { get; set; }

            [JsonProperty("marketplace_service_item_direct_flow_trans")]
            public string? MarketplaceServiceItemDirectFlowTrans { get; set; }

            [JsonProperty("marketplace_service_item_return_flow_trans")]
            public string? MarketplaceServiceItemReturnFlowTrans { get; set; }

            [JsonProperty("marketplace_service_item_deliv_to_customer")]
            public string? MarketplaceServiceItemDelivToCustomer { get; set; }

            [JsonProperty("marketplace_service_item_return_not_deliv_to_customer")]
            public string? MarketplaceServiceItemReturnNotDelivToCustomer { get; set; }

            [JsonProperty("marketplace_service_item_return_part_goods_customer")]
            public string? MarketplaceServiceItemReturnPartGoodsCustomer { get; set; }

            [JsonProperty("marketplace_service_item_return_after_deliv_to_customer")]
            public string? MarketplaceServiceItemReturnAfterDelivToCustomer { get; set; }
        }

        public class Posting
        {
            public double CurrencyExchangeRate { get; set; }

            [JsonProperty("posting_number")]
            public string? PostingNumber { get; set; }

            [JsonProperty("order_id")]
            public string? OrderId { get; set; }

            [JsonProperty("order_number")]
            public string? OrderNumber { get; set; }

            [JsonProperty("status")]
            public string? Status { get; set; }

            [JsonProperty("delivery_method")]
            public DeliveryMethod? DeliveryMethod { get; set; }

            [JsonProperty("tracking_number")]
            public string? TrackingNumber { get; set; }

            [JsonProperty("tpl_integration_type")]
            public string? TplIntegrationType { get; set; }

            [JsonProperty("in_process_at")]
            public string? InProcessAt { get; set; }

            [JsonProperty("shipment_date")]
            public string? ShipmentDate { get; set; }

            [JsonProperty("delivering_date")]
            public string? DeliveringDate { get; set; }

            [JsonProperty("cancellation")]
            public Cancellation? Cancellation { get; set; }

            [JsonProperty("customer")]
            public object? Customer { get; set; }

            [JsonProperty("products")]
            public List<Product>? Products { get; set; }

            [JsonProperty("addressee")]
            public object? Addressee { get; set; }

            [JsonProperty("barcodes")]
            public object? Barcodes { get; set; }

            [JsonProperty("analytics_data")]
            public AnalyticsData? AnalyticsData { get; set; }

            [JsonProperty("financial_data")]
            public FinancialData? FinancialData { get; set; }

            [JsonProperty("is_express")]
            public bool IsExpress { get; set; }

            [JsonProperty("requirements")]
            public Requirements? Requirements { get; set; }

            [JsonProperty("parent_posting_number")]
            public string? ParentPostingNumber { get; set; }

            [JsonProperty("available_actions")]
            public List<object>? AvailableActions { get; set; }

            [JsonProperty("multi_box_qty")]
            public string? MultiBoxQty { get; set; }

            [JsonProperty("is_multibox")]
            public bool IsMultibox { get; set; }

            [JsonProperty("substatus")]
            public string? Substatus { get; set; }

            [JsonProperty("prr_option")]
            public string? PrrOption { get; set; }
        }

        public class PostingServices
        {
            [JsonProperty("marketplace_service_item_fulfillment")]
            public string? MarketplaceServiceItemFulfillment { get; set; }

            [JsonProperty("marketplace_service_item_pickup")]
            public string? MarketplaceServiceItemPickup { get; set; }

            [JsonProperty("marketplace_service_item_dropoff_pvz")]
            public string? MarketplaceServiceItemDropoffPvz { get; set; }

            [JsonProperty("marketplace_service_item_dropoff_sc")]
            public string? MarketplaceServiceItemDropoffSc { get; set; }

            [JsonProperty("marketplace_service_item_dropoff_ff")]
            public string? MarketplaceServiceItemDropoffFf { get; set; }

            [JsonProperty("marketplace_service_item_direct_flow_trans")]
            public string? MarketplaceServiceItemDirectFlowTrans { get; set; }

            [JsonProperty("marketplace_service_item_return_flow_trans")]
            public string? MarketplaceServiceItemReturnFlowTrans { get; set; }

            [JsonProperty("marketplace_service_item_deliv_to_customer")]
            public string? MarketplaceServiceItemDelivToCustomer { get; set; }

            [JsonProperty("marketplace_service_item_return_not_deliv_to_customer")]
            public string? MarketplaceServiceItemReturnNotDelivToCustomer { get; set; }

            [JsonProperty("marketplace_service_item_return_part_goods_customer")]
            public string? MarketplaceServiceItemReturnPartGoodsCustomer { get; set; }

            [JsonProperty("marketplace_service_item_return_after_deliv_to_customer")]
            public string? MarketplaceServiceItemReturnAfterDelivToCustomer { get; set; }
        }

        public class Product
        {
            public double UsdPrice { get; set; }

            [JsonProperty("price")]
            public string? Price { get; set; }

            [JsonProperty("offer_id")]
            public string? OfferId { get; set; }

            [JsonProperty("name")]
            public string? Name { get; set; }

            [JsonProperty("sku")]
            public string? Sku { get; set; }

            [JsonProperty("quantity")]
            public string? Quantity { get; set; }

            [JsonProperty("mandatory_mark")]
            public List<string>? MandatoryMark { get; set; }

            [JsonProperty("currency_code")]
            public string? CurrencyCode { get; set; }

            [JsonProperty("commission_amount")]
            public double CommissionAmount { get; set; }

            [JsonProperty("commission_percent")]
            public string? CommissionPercent { get; set; }

            [JsonProperty("payout")]
            public double Payout { get; set; }

            [JsonProperty("product_id")]
            public string? ProductId { get; set; }

            [JsonProperty("old_price")]
            public double OldPrice { get; set; }

            [JsonProperty("total_discount_value")]
            public string? TotalDiscountValue { get; set; }

            [JsonProperty("total_discount_percent")]
            public double TotalDiscountPercent { get; set; }

            [JsonProperty("actions")]
            public List<string>? Actions { get; set; }

            [JsonProperty("picking")]
            public object? Picking { get; set; }

            [JsonProperty("client_price")]
            public string? ClientPrice { get; set; }

            [JsonProperty("item_services")]
            public ItemServices? ItemServices { get; set; }
        }

        public class Requirements
        {
            [JsonProperty("products_requiring_gtd")]
            public List<object>? ProductsRequiringGtd { get; set; }

            [JsonProperty("products_requiring_country")]
            public List<object>? ProductsRequiringCountry { get; set; }

            [JsonProperty("products_requiring_mandatory_mark")]
            public List<object>? ProductsRequiringMandatoryMark { get; set; }

            [JsonProperty("products_requiring_rnpt")]
            public List<object>? ProductsRequiringRnpt { get; set; }

            [JsonProperty("products_requiring_jw_uin")]
            public List<object>? ProductsRequiringJwUin { get; set; }
        }

        public class Result
        {
            [JsonProperty("postings")]
            public List<Posting>? Postings { get; set; }

            [JsonProperty("has_next")]
            public bool HasNext { get; set; }
        }

        public class Response
        {
            [JsonProperty("result")]
            public Result? Result { get; set; }
        }


    }
}
