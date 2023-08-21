using Newtonsoft.Json;
using StructLibCore.Marketplace;
using System;
using System.Collections.Generic;
namespace SiteApi.IntegrationSiteApi.ApiBase.ObjDesc
{
    public class OrderItem
    {
        [JsonProperty("sku")]
        public string Sku { get; set; }
        [JsonProperty("count")]
        public int Count { get; set; }
        [JsonProperty("price")]
        public string Price { get; set; }
    }
    public class Order
    {
        public Order() { }
        public Order(IOrder order)
        {
            OrderItems = new List<OrderItem>();
            foreach (MarketOrderItems item in order.Items)
            {
                OrderItems.Add(new OrderItem() { Sku = item.Sku, Count = int.Parse(item.Count), Price = item.Price });
            }
            OrderDate = DateTime.Parse(order.Date);
            BoxingDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,  TimeZoneInfo.Local);
            OrderNomber = order.Id;
        }
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("orderItems")]
        public List<OrderItem> OrderItems { get; set; }
        [JsonProperty("orderDate")]
        public DateTime OrderDate { get; set; }
        [JsonProperty("boxingDate")]
        public DateTime BoxingDate { get; set; }
        [JsonProperty("orderNomber")]
        public string OrderNomber { get; set; }
    }
}
