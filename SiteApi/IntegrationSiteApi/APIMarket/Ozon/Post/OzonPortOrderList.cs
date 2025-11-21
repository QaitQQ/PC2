using Newtonsoft.Json;

using SiteApi.IntegrationSiteApi;

using StructLibCore;
using StructLibCore.Marketplace;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
namespace Server.Class.IntegrationSiteApi.Market.Ozon.OzonPortOrderList
{
    public class OzonPortOrderList : SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post.OzonPost
    {
        public OzonPortOrderList(APISetting aPISetting) : base(aPISetting){}
        public List<object> Get()
        {
            try
            {
                HttpWebRequest httpWebRequest = GetRequest(@"v3/posting/fbs/unfulfilled/list");
                PostList root = new PostList() { Limit = 100, Offset = 0, Dir = "ASC", Filter = new Filter() { CutoffFrom = DateTime.Now.AddMonths(-1), CutoffTo = DateTime.Now.AddDays(20) }, With = new With() { AnalyticsData = true, Barcodes = true, FinancialData = true, Translit = true } };
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream())) { var json = JsonConvert.SerializeObject(root); streamWriter.Write(json); }
                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }
                OrderList OrderList = JsonConvert.DeserializeObject<OrderList>(result);
                List<object> X = new List<object>();
                foreach (var item in OrderList.Result.Postings) { item.APISetting = aPISetting; X.Add(item); }
                return X;
            }
            catch 
            {
                
                return null;
            }

        }
    }



    [Serializable]
    public class Filter
    {
        [JsonProperty("cutoff_from")]
        public DateTime CutoffFrom;
        [JsonProperty("cutoff_to")]
        public DateTime CutoffTo;
        [JsonProperty("delivery_method_id")]
        public List<object> DeliveryMethodId;
        [JsonProperty("provider_id")]
        public List<object> ProviderId;
        [JsonProperty("status")]
        public string Status;
        [JsonProperty("warehouse_id")]
        public List<object> WarehouseId;

        public Filter()
        {
            DeliveryMethodId = new List<object>();
            ProviderId = new List<object>();
            WarehouseId = new List<object>();
        }
    }
    [Serializable]
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
    [Serializable]
    public class PostList
    {
        [JsonProperty("dir")]
        public string Dir;
        [JsonProperty("filter")]
        public Filter Filter;
        [JsonProperty("limit")]
        public int Limit;
        [JsonProperty("offset")]
        public int Offset;
        [JsonProperty("with")]
        public With With;
    }
    [Serializable]
    public class LegalInfo
    {
        [JsonProperty("company_name")]
        public string CompanyName { get; set; }

        [JsonProperty("inn")]
        public string Inn { get; set; }

        [JsonProperty("kpp")]
        public string Kpp { get; set; }
    }
    [Serializable]
    public class Optional
    {
        [JsonProperty("products_with_possible_mandatory_mark")]
        public List<object> ProductsWithPossibleMandatoryMark { get; set; }
    }

    public class DeliveryMethod
    {
        [JsonProperty("id")]
        public object Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("warehouse_id")]
        public object WarehouseId { get; set; }

        [JsonProperty("warehouse")]
        public string Warehouse { get; set; }

        [JsonProperty("tpl_provider_id")]
        public int? TplProviderId { get; set; }

        [JsonProperty("tpl_provider")]
        public string TplProvider { get; set; }
    }
    [Serializable]
    public class Cancellation
    {
        [JsonProperty("cancel_reason_id")]
        public int? CancelReasonId { get; set; }

        [JsonProperty("cancel_reason")]
        public string CancelReason { get; set; }

        [JsonProperty("cancellation_type")]
        public string CancellationType { get; set; }

        [JsonProperty("cancelled_after_ship")]
        public bool? CancelledAfterShip { get; set; }

        [JsonProperty("affect_cancellation_rating")]
        public bool? AffectCancellationRating { get; set; }

        [JsonProperty("cancellation_initiator")]
        public string CancellationInitiator { get; set; }
    }

    [Serializable]
    public class Product
    {
        [JsonProperty("price")]
        public string Price { get; set; }

        [JsonProperty("offer_id")]
        public string OfferId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("sku")]
        public object Sku { get; set; }

        [JsonProperty("quantity")]
        public int? Quantity { get; set; }

        [JsonProperty("mandatory_mark")]
        public List<object> MandatoryMark { get; set; }

        [JsonProperty("currency_code")]
        public string CurrencyCode { get; set; }

        [JsonProperty("is_blr_traceable")]
        public bool? IsBlrTraceable { get; set; }

        [JsonProperty("commission_amount")]
        public int? CommissionAmount { get; set; }

        [JsonProperty("commission_percent")]
        public int? CommissionPercent { get; set; }

        [JsonProperty("payout")]
        public int? Payout { get; set; }

        [JsonProperty("product_id")]
        public object ProductId { get; set; }

        [JsonProperty("old_price")]
        public double? OldPrice { get; set; }

        [JsonProperty("total_discount_value")]
        public double? TotalDiscountValue { get; set; }

        [JsonProperty("total_discount_percent")]
        public double? TotalDiscountPercent { get; set; }

        [JsonProperty("actions")]
        public List<object> Actions { get; set; }

        [JsonProperty("picking")]
        public object Picking { get; set; }

        [JsonProperty("client_price")]
        public string ClientPrice { get; set; }

        [JsonProperty("item_services")]
        public ItemServices ItemServices { get; set; }

        [JsonProperty("customer_currency_code")]
        public string CustomerCurrencyCode { get; set; }

        [JsonProperty("customer_price")]
        public double? CustomerPrice { get; set; }
    }


    public class Barcodes
    {
        [JsonProperty("upper_barcode")]
        public string UpperBarcode { get; set; }

        [JsonProperty("lower_barcode")]
        public string LowerBarcode { get; set; }
    }
    [Serializable]
    public class AnalyticsData
    {
        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("delivery_type")]
        public string DeliveryType { get; set; }

        [JsonProperty("is_premium")]
        public bool? IsPremium { get; set; }

        [JsonProperty("payment_type_group_name")]
        public string PaymentTypeGroupName { get; set; }

        [JsonProperty("warehouse_id")]
        public object WarehouseId { get; set; }

        [JsonProperty("warehouse")]
        public string Warehouse { get; set; }

        [JsonProperty("tpl_provider_id")]
        public int? TplProviderId { get; set; }

        [JsonProperty("tpl_provider")]
        public string TplProvider { get; set; }

        [JsonProperty("delivery_date_begin")]
        public DateTime? DeliveryDateBegin { get; set; }

        [JsonProperty("delivery_date_end")]
        public DateTime? DeliveryDateEnd { get; set; }

        [JsonProperty("is_legal")]
        public bool? IsLegal { get; set; }
    }

    [Serializable]
    public class ItemServices
    {
        [JsonProperty("marketplace_service_item_fulfillment")]
        public int MarketplaceServiceItemFulfillment;
        [JsonProperty("marketplace_service_item_pickup")]
        public int MarketplaceServiceItemPickup;
        [JsonProperty("marketplace_service_item_dropoff_pvz")]
        public int MarketplaceServiceItemDropoffPvz;
        [JsonProperty("marketplace_service_item_dropoff_sc")]
        public int MarketplaceServiceItemDropoffSc;
        [JsonProperty("marketplace_service_item_dropoff_ff")]
        public int MarketplaceServiceItemDropoffFf;
        [JsonProperty("marketplace_service_item_direct_flow_trans")]
        public int MarketplaceServiceItemDirectFlowTrans;
        [JsonProperty("marketplace_service_item_return_flow_trans")]
        public int MarketplaceServiceItemReturnFlowTrans;
        [JsonProperty("marketplace_service_item_deliv_to_customer")]
        public int MarketplaceServiceItemDelivToCustomer;
        [JsonProperty("marketplace_service_item_return_not_deliv_to_customer")]
        public int MarketplaceServiceItemReturnNotDelivToCustomer;
        [JsonProperty("marketplace_service_item_return_part_goods_customer")]
        public int MarketplaceServiceItemReturnPartGoodsCustomer;
        [JsonProperty("marketplace_service_item_return_after_deliv_to_customer")]
        public int MarketplaceServiceItemReturnAfterDelivToCustomer;
    }
    [Serializable]
    public class PostingServices
    {
        [JsonProperty("marketplace_service_item_fulfillment")]
        public int MarketplaceServiceItemFulfillment;
        [JsonProperty("marketplace_service_item_pickup")]
        public int MarketplaceServiceItemPickup;
        [JsonProperty("marketplace_service_item_dropoff_pvz")]
        public int MarketplaceServiceItemDropoffPvz;
        [JsonProperty("marketplace_service_item_dropoff_sc")]
        public int MarketplaceServiceItemDropoffSc;
        [JsonProperty("marketplace_service_item_dropoff_ff")]
        public int MarketplaceServiceItemDropoffFf;
        [JsonProperty("marketplace_service_item_direct_flow_trans")]
        public int MarketplaceServiceItemDirectFlowTrans;
        [JsonProperty("marketplace_service_item_return_flow_trans")]
        public int MarketplaceServiceItemReturnFlowTrans;
        [JsonProperty("marketplace_service_item_deliv_to_customer")]
        public int MarketplaceServiceItemDelivToCustomer;
        [JsonProperty("marketplace_service_item_return_not_deliv_to_customer")]
        public int MarketplaceServiceItemReturnNotDelivToCustomer;
        [JsonProperty("marketplace_service_item_return_part_goods_customer")]
        public int MarketplaceServiceItemReturnPartGoodsCustomer;
        [JsonProperty("marketplace_service_item_return_after_deliv_to_customer")]
        public int MarketplaceServiceItemReturnAfterDelivToCustomer;
    }
    [Serializable]
    public class FinancialData
    {
        [JsonProperty("products")]
        public List<Product> Products { get; set; }

        [JsonProperty("posting_services")]
        public object PostingServices { get; set; }

        [JsonProperty("cluster_from")]
        public string ClusterFrom { get; set; }

        [JsonProperty("cluster_to")]
        public string ClusterTo { get; set; }
    }
    [Serializable]
    public class Requirements
    {
        [JsonProperty("products_requiring_gtd")]
        public List<long?> ProductsRequiringGtd { get; set; }

        [JsonProperty("products_requiring_country")]
        public List<object> ProductsRequiringCountry { get; set; }

        [JsonProperty("products_requiring_mandatory_mark")]
        public List<object> ProductsRequiringMandatoryMark { get; set; }

        [JsonProperty("products_requiring_rnpt")]
        public List<object> ProductsRequiringRnpt { get; set; }

        [JsonProperty("products_requiring_jw_uin")]
        public List<object> ProductsRequiringJwUin { get; set; }
    }
    [Serializable]
    public class Oder: IOrder
    {
        [JsonProperty("posting_number")]
        public string PostingNumber;
        [JsonProperty("order_id")]
        public string OrderId;
        [JsonProperty("order_number")]
        public string OrderNumber;
        [JsonProperty("status")]
        public string status;
        [JsonProperty("delivery_method")]
        public DeliveryMethod DeliveryMethod;
        [JsonProperty("tracking_number")]
        public string TrackingNumber;
        [JsonProperty("tpl_integration_type")]
        public string TplIntegrationType;
        [JsonProperty("in_process_at")]
        public DateTime InProcessAt;
        [JsonProperty("shipment_date")]
        public DateTime ShipmentDate;
        [JsonProperty("delivering_date")]
        public object DeliveringDate;
        [JsonProperty("cancellation")]
        public Cancellation Cancellation;
        [JsonProperty("customer")]
        public object Customer;
        [JsonProperty("products")]
        public List<Product> Products;
        [JsonProperty("addressee")]
        public object Addressee;
        [JsonProperty("barcodes")]
        public Barcodes Barcodes;
        [JsonProperty("analytics_data")]
        public AnalyticsData AnalyticsData;
        [JsonProperty("financial_data")]
        public FinancialData FinancialData;
        [JsonProperty("is_express")]
        public bool IsExpress;
        [JsonProperty("requirements")]
        public Requirements Requirements;
        public string Id { get { return PostingNumber; } }
        public APISetting APISetting { get; set; }
        public OrderStatus Status => status switch
        {
            ("awaiting_deliver") => OrderStatus.READY,
            ("awaiting_packaging") => OrderStatus.PROCESSING_STARTED,
            ("delivering") => OrderStatus.DELIVERED,
            _ => OrderStatus.NONE,
        };
        public List<MarketOrderItems> Items
        {
            get
            {
                var X = new List<MarketOrderItems>();
                foreach (var item in Products)
                {
                    X.Add(new MarketOrderItems(item.Name, item.Quantity.ToString(), item.Price.ToString(), item.OfferId, this));
                }
                return X;
            } 
        }
        public string Date { get { return InProcessAt.ToString(); } }
        public string DeliveryDate { get { return ShipmentDate.ToShortDateString(); } }

        public void SetStatus(OrderStatus _status)
        {
            switch (_status)
            {
                case OrderStatus.NONE:
                    break;
                case OrderStatus.PROCESSING_STARTED:
                    break;
                case OrderStatus.PROCESSING_SHIPPED:
                    break;
                case OrderStatus.DELIVERED:
                    break;
                case OrderStatus.CANCELLED:
                    break;
                case OrderStatus.READY:
                    status = "awaiting_deliver";

                    break;
                default:
                    break;
            }
        }

        string IOrder.ShipmentDate => ShipmentDate.ToString();

        public List<IMarketItem> IMtemsList { get; set; }
    }
    [Serializable]
    public class Result
    {
        [JsonProperty("postings")]
        public List<Order> Postings;
        [JsonProperty("count")]
        public int Count;
    }
    [Serializable]
    public class OrderList
    {
        [JsonProperty("result")]
        public Result Result;
    }
    [Serializable]
    public class Order : IOrder
    {
        [JsonProperty("posting_number")]
        public string PostingNumber { get; set; }

        [JsonProperty("order_id")]
        public object OrderId { get; set; }

        [JsonProperty("order_number")]
        public string OrderNumber { get; set; }

        [JsonProperty("status")]
        public string status { get; set; }

        [JsonProperty("delivery_method")]
        public DeliveryMethod DeliveryMethod { get; set; }

        [JsonProperty("tracking_number")]
        public string TrackingNumber { get; set; }

        [JsonProperty("tpl_integration_type")]
        public string TplIntegrationType { get; set; }

        [JsonProperty("in_process_at")]
        public DateTime? InProcessAt { get; set; }

        [JsonProperty("shipment_date")]
        public DateTime ShipmentDate { get; set; }

        [JsonProperty("delivering_date")]
        public DateTime? DeliveringDate { get; set; }

        [JsonProperty("cancellation")]
        public Cancellation Cancellation { get; set; }

        [JsonProperty("customer")]
        public object Customer { get; set; }

        [JsonProperty("products")]
        public List<Product> Products { get; set; }

        [JsonProperty("addressee")]
        public object Addressee { get; set; }

        [JsonProperty("barcodes")]
        public Barcodes Barcodes { get; set; }

        [JsonProperty("analytics_data")]
        public AnalyticsData AnalyticsData { get; set; }

        [JsonProperty("financial_data")]
        public FinancialData FinancialData { get; set; }

        [JsonProperty("is_express")]
        public bool? IsExpress { get; set; }

        [JsonProperty("requirements")]
        public Requirements Requirements { get; set; }

        [JsonProperty("parent_posting_number")]
        public string ParentPostingNumber { get; set; }

        [JsonProperty("available_actions")]
        public List<string> AvailableActions { get; set; }

        [JsonProperty("multi_box_qty")]
        public int? MultiBoxQty { get; set; }

        [JsonProperty("is_multibox")]
        public bool? IsMultibox { get; set; }

        [JsonProperty("substatus")]
        public string Substatus { get; set; }

        [JsonProperty("prr_option")]
        public string PrrOption { get; set; }

        [JsonProperty("quantum_id")]
        public int? QuantumId { get; set; }

        [JsonProperty("tariffication")]
        public Tariffication Tariffication { get; set; }

        [JsonProperty("destination_place_id")]
        public int? DestinationPlaceId { get; set; }

        [JsonProperty("destination_place_name")]
        public string DestinationPlaceName { get; set; }

        [JsonProperty("is_presortable")]
        public bool? IsPresortable { get; set; }

        [JsonProperty("pickup_code_verified_at")]
        public object PickupCodeVerifiedAt { get; set; }

        [JsonProperty("optional")]
        public Optional Optional { get; set; }

        [JsonProperty("legal_info")]
        public LegalInfo LegalInfo { get; set; }




        public string Id { get { return PostingNumber; } }
        public APISetting APISetting { get; set; }
        public OrderStatus Status => status switch
        {
            ("awaiting_deliver") => OrderStatus.READY,
            ("awaiting_packaging") => OrderStatus.PROCESSING_STARTED,
            ("delivering") => OrderStatus.DELIVERED,
            _ => OrderStatus.NONE,
        };



        public List<MarketOrderItems> Items
        {
            get
            {
                var X = new List<MarketOrderItems>();
                foreach (var item in Products)
                {
                    X.Add(new MarketOrderItems(item.Name, item.Quantity.ToString(), item.Price.ToString(), item.OfferId, this));
                }
                return X;
            }
        }
        public string Date { get { return InProcessAt.ToString(); } }
        public string DeliveryDate { get { return ShipmentDate.ToShortDateString(); } }

        public void SetStatus(OrderStatus _status)
        {
            switch (_status)
            {
                case OrderStatus.NONE:
                    break;
                case OrderStatus.PROCESSING_STARTED:
                    break;
                case OrderStatus.PROCESSING_SHIPPED:
                    break;
                case OrderStatus.DELIVERED:
                    break;
                case OrderStatus.CANCELLED:
                    break;
                case OrderStatus.READY:
                    status = "awaiting_deliver";

                    break;
                default:
                    break;
            }
        }

        string IOrder.ShipmentDate => ShipmentDate.ToString();

        public List<IMarketItem> IMtemsList { get; set; }
    }
    [Serializable]
    public class Tariffication
    {
        [JsonProperty("current_tariff_rate")]
        public int? CurrentTariffRate { get; set; }

        [JsonProperty("current_tariff_type")]
        public string CurrentTariffType { get; set; }

        [JsonProperty("current_tariff_charge")]
        public string CurrentTariffCharge { get; set; }

        [JsonProperty("current_tariff_charge_currency_code")]
        public string CurrentTariffChargeCurrencyCode { get; set; }

        [JsonProperty("next_tariff_rate")]
        public int? NextTariffRate { get; set; }

        [JsonProperty("next_tariff_type")]
        public string NextTariffType { get; set; }

        [JsonProperty("next_tariff_charge")]
        public string NextTariffCharge { get; set; }

        [JsonProperty("next_tariff_starts_at")]
        public DateTime? NextTariffStartsAt { get; set; }

        [JsonProperty("next_tariff_charge_currency_code")]
        public string NextTariffChargeCurrencyCode { get; set; }
    }


}