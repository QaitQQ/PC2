using Newtonsoft.Json;

using StructLibCore.Marketplace;

using System.Net;
namespace Server.Class.IntegrationSiteApi.Market.Ozon.OzonPortOrderList
{
    public class OzonPortOrderList : SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post.OzonPost
    {
        public OzonPortOrderList(APISetting aPISetting) : base(aPISetting) { }
        public List<object> Get()
        {
            try
            {
                HttpWebRequest httpWebRequest = GetRequest(@"v3/posting/fbs/unfulfilled/list");
                PostList root = new PostList() { Limit = 100, Offset = 0, Dir = "ASC", Filter = new Filter() { CutoffFrom = DateTime.Now.AddMonths(-1), CutoffTo = DateTime.Now.AddDays(20) }, With = new With() { AnalyticsData = true, Barcodes = true, FinancialData = true, Translit = true } };
                using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream())) { string json = JsonConvert.SerializeObject(root); streamWriter.Write(json); }
                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }
                OrderList OrderList = JsonConvert.DeserializeObject<OrderList>(result!)!;
                List<object> X = new List<object>();
                foreach (Order item in OrderList.Result?.Postings!) { item.APISetting = aPISetting != null ? aPISetting : new APISetting(); X.Add(item); }
                return X;
            }
            catch
            {

                return null!;
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
        public string? Status;
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
        public string? Dir;
        [JsonProperty("filter")]
        public Filter? Filter;
        [JsonProperty("limit")]
        public int Limit;
        [JsonProperty("offset")]
        public int Offset;
        [JsonProperty("with")]
        public With? With;
    }

    [Serializable]
    public class DeliveryMethod
    {
        [JsonProperty("id")]
        public long Id;
        [JsonProperty("name")]
        public string? Name;
        [JsonProperty("warehouse_id")]
        public long WarehouseId;
        [JsonProperty("warehouse")]
        public string? Warehouse;
        [JsonProperty("tpl_provider_id")]
        public int TplProviderId;
        [JsonProperty("tpl_provider")]
        public string? TplProvider;
    }
    [Serializable]
    public class Cancellation
    {
        [JsonProperty("cancel_reason_id")]
        public int CancelReasonId;
        [JsonProperty("cancel_reason")]
        public string? CancelReason;
        [JsonProperty("cancellation_type")]
        public string? CancellationType;
        [JsonProperty("cancelled_after_ship")]
        public bool CancelledAfterShip;
        [JsonProperty("affect_cancellation_rating")]
        public bool AffectCancellationRating;
        [JsonProperty("cancellation_initiator")]
        public string? CancellationInitiator;
    }
    [Serializable]
    public class Product
    {
        [JsonProperty("price")]
        public double Price;
        [JsonProperty("offer_id")]
        public string? OfferId;
        [JsonProperty("name")]
        public string? Name;
        [JsonProperty("sku")]
        public int Sku;
        [JsonProperty("quantity")]
        public int Quantity;
        [JsonProperty("mandatory_mark")]
        public List<object>? MandatoryMark;
        [JsonProperty("commission_amount")]
        public int CommissionAmount;
        [JsonProperty("commission_percent")]
        public int CommissionPercent;
        [JsonProperty("payout")]
        public int Payout;
        [JsonProperty("product_id")]
        public int ProductId;
        [JsonProperty("old_price")]
        public double OldPrice;
        [JsonProperty("total_discount_value")]
        public double TotalDiscountValue;
        [JsonProperty("total_discount_percent")]
        public double TotalDiscountPercent;
        [JsonProperty("actions")]
        public List<string>? Actions;
        [JsonProperty("picking")]
        public object? Picking;
        [JsonProperty("client_price")]
        public string? ClientPrice;
        [JsonProperty("item_services")]
        public ItemServices? ItemServices;
    }
    [Serializable]
    public class Barcodes
    {
        [JsonProperty("upper_barcode")]
        public string? UpperBarcode;
        [JsonProperty("lower_barcode")]
        public string? LowerBarcode;
    }
    [Serializable]
    public class AnalyticsData
    {
        [JsonProperty("region")]
        public string? Region;
        [JsonProperty("city")]
        public string? City;
        [JsonProperty("delivery_type")]
        public string? DeliveryType;
        [JsonProperty("is_premium")]
        public bool IsPremium;
        [JsonProperty("payment_type_group_name")]
        public string? PaymentTypeGroupName;
        [JsonProperty("warehouse_id")]
        public long WarehouseId;
        [JsonProperty("warehouse")]
        public string? Warehouse;
        [JsonProperty("tpl_provider_id")]
        public int TplProviderId;
        [JsonProperty("tpl_provider")]
        public string? TplProvider;
        [JsonProperty("delivery_date_begin")]
        public string? DeliveryDateBegin;
        [JsonProperty("delivery_date_end")]
        public string? DeliveryDateEnd;
        [JsonProperty("is_legal")]
        public bool IsLegal;
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
        public List<Product>? Products;
        [JsonProperty("posting_services")]
        public PostingServices? PostingServices;
    }
    [Serializable]
    public class Requirements
    {
        [JsonProperty("products_requiring_gtd")]
        public List<object>? ProductsRequiringGtd;
        [JsonProperty("products_requiring_country")]
        public List<object>? ProductsRequiringCountry;
    }
    [Serializable]
    public class Order : IOrder
    {
        [JsonProperty("posting_number")]
        public string? PostingNumber;
        [JsonProperty("order_id")]
        public string? OrderId;
        [JsonProperty("order_number")]
        public string? OrderNumber;
        [JsonProperty("status")]
        public string? status;
        [JsonProperty("delivery_method")]
        public DeliveryMethod? DeliveryMethod;
        [JsonProperty("tracking_number")]
        public string? TrackingNumber;
        [JsonProperty("tpl_integration_type")]
        public string? TplIntegrationType;
        [JsonProperty("in_process_at")]
        public DateTime InProcessAt;
        [JsonProperty("shipment_date")]
        public DateTime ShipmentDate;
        [JsonProperty("delivering_date")]
        public object? DeliveringDate;
        [JsonProperty("cancellation")]
        public Cancellation? Cancellation;
        [JsonProperty("customer")]
        public object? Customer;
        [JsonProperty("products")]
        public List<Product>? Products;
        [JsonProperty("addressee")]
        public object? Addressee;
        [JsonProperty("barcodes")]
        public Barcodes? Barcodes;
        [JsonProperty("analytics_data")]
        public AnalyticsData? AnalyticsData;
        [JsonProperty("financial_data")]
        public FinancialData? FinancialData;
        [JsonProperty("is_express")]
        public bool IsExpress;
        [JsonProperty("requirements")]
        public Requirements? Requirements;

        public Order()
        {

            APISetting = new APISetting();
            IMtemsList = new List<IMarketItem>();


        }

        public string? Id { get { return PostingNumber; } }
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
                List<MarketOrderItems> X = new List<MarketOrderItems>();
                foreach (Product item in Products!)
                {
                    X.Add(new MarketOrderItems(item.Name!, item.Quantity.ToString(), item.Price.ToString(), item.OfferId!, this));
                }
                return X;
            }
        }
        public string? Date { get { return InProcessAt.ToString(); } }
        public string? DeliveryDate { get { return ShipmentDate.ToShortDateString(); } }

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
        public List<Order>? Postings;
        [JsonProperty("count")]
        public int Count;
    }
    [Serializable]
    public class OrderList
    {
        [JsonProperty("result")]
        public Result? Result;
    }



}