using Newtonsoft.Json;

using StructLibCore.Marketplace;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace Server.Class.IntegrationSiteApi.Market.Yandex.YandexGetItemOrders
{
    public class YandexGetItemOrders : SiteApi.IntegrationSiteApi.APIMarket.Yandex.YandexApiClass
    {
        public YandexGetItemOrders(APISetting APISetting) : base(APISetting)
        {
        }
        public List<object> Get()
        {
            string date = DateTime.Now.AddDays(-10).ToString("dd-MM-yyyy");
            HttpWebRequest httpWebRequest = GetRequest(@"/campaigns/" + ClientID + "/orders.json?fromDate=" + date, "GET");
            HttpWebResponse httpResponse = null;

            try
            {
                httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);

                return null;
         
            }
            




            using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }
            Root root = JsonConvert.DeserializeObject<Root>(result);
            List<object> Lst = new List<object>();
            foreach (Order item in root.Orders)
            {
                item.APISetting = aPISetting;
                Lst.Add(item);
            }
            if (root.Pager.PagesCount > 1)
            {
                int end = root.Pager.PagesCount;
                for (int i = 2; i < end; i++)
                {
                    httpWebRequest = GetRequest(@"/campaigns/" + ClientID + "/orders.json?&page=" + i.ToString(), "GET");
                    httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }
                    root = JsonConvert.DeserializeObject<Root>(result);
                    foreach (Order item in root.Orders)
                    {
                        item.APISetting = aPISetting;
                        Lst.Add(item);
                    }
                }
            }
            return Lst;
        }
        [Serializable]
        public class Pager
        {
            [JsonProperty("total")]
            public int Total { get; set; }
            [JsonProperty("from")]
            public int From { get; set; }
            [JsonProperty("to")]
            public int To { get; set; }
            [JsonProperty("currentPage")]
            public int CurrentPage { get; set; }
            [JsonProperty("pagesCount")]
            public int PagesCount { get; set; }
            [JsonProperty("pageSize")]
            public int PageSize { get; set; }
        }
        [Serializable]
        public class Promo
        {
            [JsonProperty("type")]
            public string Type { get; set; }
            [JsonProperty("subsidy")]
            public int Subsidy { get; set; }
        }
        [Serializable]
        public class Subsidy
        {
            [JsonProperty("type")]
            public string Type { get; set; }
            [JsonProperty("amount")]
            public double Amount { get; set; }
        }
        [Serializable]
        public class Item
        {
            [JsonProperty("id")]
            public int Id { get; set; }
            [JsonProperty("feedId")]
            public int FeedId { get; set; }
            [JsonProperty("offerId")]
            public string OfferId { get; set; }
            [JsonProperty("feedCategoryId")]
            public string FeedCategoryId { get; set; }
            [JsonProperty("offerName")]
            public string OfferName { get; set; }
            [JsonProperty("price")]
            public double Price { get; set; }
            [JsonProperty("count")]
            public int Count { get; set; }
            [JsonProperty("feeUE")]
            public double FeeUE { get; set; }
            [JsonProperty("vat")]
            public string Vat { get; set; }
            [JsonProperty("shopSku")]
            public string ShopSku { get; set; }
            [JsonProperty("subsidy")]
            public double Subsidy { get; set; }
            [JsonProperty("partnerWarehouseId")]
            public string PartnerWarehouseId { get; set; }
            [JsonProperty("promos")]
            public List<Promo> Promos { get; set; }
            [JsonProperty("subsidies")]
            public List<Subsidy> Subsidies { get; set; }
        }
        [Serializable]
        public class Dates
        {
            [JsonProperty("fromDate")]
            public string FromDate { get; set; }
            [JsonProperty("toDate")]
            public string ToDate { get; set; }
            [JsonProperty("fromTime")]
            public string FromTime { get; set; }
            [JsonProperty("toTime")]
            public string ToTime { get; set; }
        }
        [Serializable]
        public class Parent
        {
            [JsonProperty("id")]
            public int Id { get; set; }
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("type")]
            public string Type { get; set; }
            [JsonProperty("parent")]
            public Parent _Parent { get; set; }
        }
        [Serializable]
        public class Region
        {
            [JsonProperty("id")]
            public int Id { get; set; }
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("type")]
            public string Type { get; set; }
            [JsonProperty("parent")]
            public Parent Parent { get; set; }
        }
        [Serializable]
        public class Track
        {
            [JsonProperty("trackCode")]
            public string TrackCode { get; set; }
            [JsonProperty("deliveryServiceId")]
            public int DeliveryServiceId { get; set; }
        }
        [Serializable]
        public class Box
        {
            [JsonProperty("id")]
            public int Id { get; set; }
            [JsonProperty("fulfilmentId")]
            public string FulfilmentId { get; set; }
        }
        [Serializable]
        public class Shipment
        {
            [JsonProperty("id")]
            public int Id { get; set; }
            [JsonProperty("weight")]
            public int Weight { get; set; }
            [JsonProperty("height")]
            public int Height { get; set; }
            [JsonProperty("depth")]
            public int Depth { get; set; }
            [JsonProperty("width")]
            public int Width { get; set; }
            [JsonProperty("status")]
            public string Status { get; set; }
            [JsonProperty("shipmentDate")]
            public string ShipmentDate { get; set; }
            [JsonProperty("tracks")]
            public List<Track> Tracks { get; set; }
            [JsonProperty("items")]
            public List<Item> Items { get; set; }
            [JsonProperty("boxes")]
            public List<Box> Boxes { get; set; }
        }
        [Serializable]
        public class Delivery
        {
            [JsonProperty("type")]
            public string Type { get; set; }
            [JsonProperty("serviceName")]
            public string ServiceName { get; set; }
            [JsonProperty("price")]
            public int Price { get; set; }
            [JsonProperty("deliveryPartnerType")]
            public string DeliveryPartnerType { get; set; }
            [JsonProperty("dates")]
            public Dates Dates { get; set; }
            [JsonProperty("region")]
            public Region Region { get; set; }
            [JsonProperty("deliveryServiceId")]
            public int DeliveryServiceId { get; set; }
            [JsonProperty("tracks")]
            public List<Track> Tracks { get; set; }
            [JsonProperty("shipments")]
            public List<Shipment> Shipments { get; set; }
            [JsonProperty("vat")]
            public string Vat { get; set; }
        }
        [Serializable]
        public class Order : IOrder
        {
            [JsonProperty("id")]
            public int _Id { get; set; }
            [JsonProperty("status")]
            public string _Status { get; set; }
            [JsonProperty("substatus")]
            public string Substatus { get; set; }
            [JsonProperty("creationDate")]
            public string CreationDate { get; set; }
            [JsonProperty("currency")]
            public string Currency { get; set; }
            [JsonProperty("itemsTotal")]
            public double ItemsTotal { get; set; }
            [JsonProperty("total")]
            public double Total { get; set; }
            [JsonProperty("deliveryTotal")]
            public int DeliveryTotal { get; set; }
            [JsonProperty("subsidyTotal")]
            public double SubsidyTotal { get; set; }
            [JsonProperty("paymentType")]
            public string PaymentType { get; set; }
            [JsonProperty("paymentMethod")]
            public string PaymentMethod { get; set; }
            [JsonProperty("fake")]
            public bool Fake { get; set; }
            [JsonProperty("feeUE")]
            public double FeeUE { get; set; }
            [JsonProperty("items")]
            public List<Item> _Items { get; set; }
            [JsonProperty("delivery")]
            public Delivery Delivery { get; set; }
            [JsonProperty("taxSystem")]
            public string TaxSystem { get; set; }
            [JsonProperty("notes")]
            public string Notes { get; set; }
            [JsonProperty("cancelRequested")]
            public bool? CancelRequested { get; set; }
            [JsonProperty("subsidies")]
            public List<Subsidy> Subsidies { get; set; }
            public List<MarketOrderItems> Items
            {
                get
                {
                    List<MarketOrderItems> lst = new List<MarketOrderItems>();
                    foreach (Item item in _Items)
                    {
                        lst.Add(new MarketOrderItems(item.OfferName, item.Count.ToString(), item.Price.ToString(), item.ShopSku, this));
                    }
                    return lst;
                }
            }
            public OrderStatus Status
            {
                get
                {
                    string X = _Status + "_" + Substatus;
                    return _Status == "CANCELLED"
                        ? OrderStatus.CANCELLED
                        : X switch
                        {
                            "PROCESSING_SHIPPED" => OrderStatus.PROCESSING_SHIPPED,
                            "DELIVERED_DELIVERY_SERVICE_DELIVERED" => OrderStatus.PROCESSING_SHIPPED,
                            "PROCESSING_STARTED" => OrderStatus.PROCESSING_STARTED,
                            "PROCESSING_READY_TO_SHIP" => OrderStatus.READY,
                            "PICKUP_PICKUP_SERVICE_RECEIVED" => OrderStatus.PROCESSING_SHIPPED,
                            "DELIVERY_USER_RECEIVED" => OrderStatus.DELIVERED,
                            "DELIVERY_DELIVERY_SERVICE_RECEIVED" => OrderStatus.DELIVERED,
                            "PICKUP_PICKUP_USER_RECEIVED" => OrderStatus.CANCELLED,
                            "REJECTED_RECEIVED_ON_DISTRIBUTION_CENTER" => OrderStatus.CANCELLED,
                            _ => OrderStatus.NONE,
                        };
                }
            }
            public string Id => _Id.ToString();
            public string Date => CreationDate;
            public string DeliveryDate => DateTime.Parse(Delivery.Shipments[0].ShipmentDate).ToShortDateString();
            public APISetting APISetting { get; set; }
        }
        [Serializable]
        public class Root
        {
            [JsonProperty("pager")]
            public Pager Pager { get; set; }
            [JsonProperty("orders")]
            public List<Order> Orders { get; set; }
        }
    }
}
