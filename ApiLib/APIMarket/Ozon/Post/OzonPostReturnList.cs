using Newtonsoft.Json;

using StructLibCore.Marketplace;

using System.Net;
namespace SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post
{
    public class OzonPostReturnList : OzonPost
    {
        public OzonPostReturnList(APISetting aPISetting) : base(aPISetting)
        {
        }
        public List<object> Get()
        {
            HttpWebRequest httpWebRequest = GetRequest(@"v2/returns/company/fbs");
            ReturnListRoot root = new() { Limit = 100, Offset = 0 };
            using (StreamWriter streamWriter = new(httpWebRequest.GetRequestStream())) { string? json = JsonConvert.SerializeObject(root); streamWriter.Write(json); }
            HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            try
            {
                using StreamReader streamReader = new(httpResponse.GetResponseStream());
                result = streamReader.ReadToEnd();
            }
            catch { }
            ResultRoot Result = JsonConvert.DeserializeObject<ResultRoot>(result!)!;
            List<object> X = new();
            foreach (Return item in Result?.Result!.Returns!)
            {
                item.APISetting = aPISetting!;
                X.Add(item);
            }
            return X;
        }
        public class Filter
        {
            public Filter(List<string> posting_number)
            {
                Posting_number = new List<string>();
            }
            [JsonProperty("posting_number")]
            public List<string> Posting_number { get; set; }
        }
        public class ReturnListRoot
        {
            [JsonProperty("filter")]
            public Filter? Filter { get; set; }
            [JsonProperty("limit")]
            public int Limit { get; set; }
            [JsonProperty("offset")]
            public int Offset { get; set; }
        }
        public class Result
        {
            [JsonProperty("returns")]
            public List<Return>? Returns { get; set; }
            [JsonProperty("count")]
            public int Count { get; set; }
        }
        public class Return : IOrder
        {
            public Return()
            {
                APISetting = new APISetting();
            }

            [JsonProperty("id")]
            public string? Id { get; set; }
            [JsonProperty("clearing_id")]
            public string? ClearingId { get; set; }
            [JsonProperty("posting_number")]
            public string? PostingNumber { get; set; }
            [JsonProperty("product_id")]
            public int ProductId { get; set; }
            [JsonProperty("sku")]
            public int Sku { get; set; }
            [JsonProperty("status")]
            public string? Status { get; set; }
            [JsonProperty("returns_keeping_cost")]
            public int ReturnsKeepingCost { get; set; }
            [JsonProperty("return_reason_name")]
            public string? ReturnReasonName { get; set; }
            [JsonProperty("return_date")]
            public DateTime ReturnDate { get; set; }
            [JsonProperty("quantity")]
            public int Quantity { get; set; }
            [JsonProperty("product_name")]
            public string? ProductName { get; set; }
            [JsonProperty("price")]
            public int Price { get; set; }
            [JsonProperty("waiting_for_seller_date_time")]
            public string? WaitingForSellerDateTime { get; set; }
            [JsonProperty("returned_to_seller_date_time")]
            public string? ReturnedToSellerDateTime { get; set; }
            [JsonProperty("last_free_waiting_day")]
            public string? LastFreeWaitingDay { get; set; }
            [JsonProperty("is_opened")]
            public bool IsOpened { get; set; }
            [JsonProperty("place_id")]
            public object? PlaceId { get; set; }
            [JsonProperty("commission_percent")]
            public int CommissionPercent { get; set; }
            [JsonProperty("commission")]
            public double Commission { get; set; }
            [JsonProperty("price_without_commission")]
            public double PriceWithoutCommission { get; set; }
            [JsonProperty("is_moving")]
            public bool IsMoving { get; set; }
            [JsonProperty("moving_to_place_name")]
            public string? MovingToPlaceName { get; set; }
            [JsonProperty("waiting_for_seller_days")]
            public int WaitingForSellerDays { get; set; }
            [JsonProperty("picking_amount")]
            public object? PickingAmount { get; set; }
            [JsonProperty("accepted_from_customer_moment")]
            public object? AcceptedFromCustomerMoment { get; set; }
            [JsonProperty("picking_tag")]
            public object? PickingTag { get; set; }
            string IOrder.Id => Id ?? string.Empty;

            public APISetting APISetting { get; set; }
            OrderStatus IOrder.Status =>
                Status switch
                {
                    "waiting_for_seller" => OrderStatus.READY,
                    "accepted_from_customer" => OrderStatus.PROCESSING_STARTED,
                    "returned_to_seller" => OrderStatus.DELIVERED,
                    _ => OrderStatus.NONE,
                };
            public List<MarketOrderItems> Items => new() { new MarketOrderItems(ProductName!) };
            public string? Date => ReturnedToSellerDateTime;
            public string? DeliveryDate => WaitingForSellerDateTime;
            public void SetStatus(OrderStatus status)
            {
                switch (status)
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
                        Status = "waiting_for_seller";
                        break;
                    default:
                        break;
                }
            }
            public string? ShipmentDate => WaitingForSellerDateTime;

            public List<IMarketItem> IMtemsList { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        }
        public class ResultRoot
        {
            [JsonProperty("result")]
            public Result? Result { get; set; }
        }
    }
}
