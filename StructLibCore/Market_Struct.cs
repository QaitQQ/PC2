﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace StructLibCore.Marketplace
{
    public enum OrderStatus { NONE, PROCESSING_STARTED, PROCESSING_SHIPPED, DELIVERED, CANCELLED, READY }

    [Serializable]
    public class InnString
    {
        public InnString(MarketName marketName, string iNN)
        {
            MarketName = marketName;
            INN = iNN;
        }

        public InnString() { MarketName = MarketName.Ozon; }
        public MarketName MarketName { get; set; }
        public string INN { get; set; }
    }


    [Serializable]
    public class MarketPlaceCash
    {
        public List<MarketItem> MarketItems { get; set; }
        public List<APISetting> APISettings { get; set; }
        public List<InnString> SellerINN { get; set; }
        public List<IOrder> Orders { get; set; }
        public MarketPlaceCash() { MarketItems = new List<MarketItem>(); APISettings = new List<APISetting>(); Orders = new List<IOrder>(); }
    }
    [Serializable]
    public class MarketOrderItems 
    {
        public MarketOrderItems(string name, string count = null, string price = null, string sku = null)
        {
            Name = name;
            Count = count;
            Price = price;
            Sku = sku;
        }

        public string Name { get; set; }

        public string Count { get; set; }

        public string Price { get; set; }

        public string Sku { get; set; }
    }
    public interface IOrder
    {
        public string Id { get; }
        public APISetting APISetting { get; set; }
        public OrderStatus Status { get; }
        public List<MarketOrderItems> Items { get; }
        public string Date { get; }
        public string DeliveryDate { get; }
    }
    [Serializable]
    public class APISetting
    {
        public string Name { get; set; }
        public MarketName Type { get; set; }
        public string[] ApiString { get; set; }
        public bool Active { get; set; }
        public string INN { get; set; }
    }
    [Serializable]
    public enum MarketName { Yandex, Ozon, Avito, Sber }

    public interface IMarketItem
    {
        public string Name { get; set; }
        public string Price { get; set; }
        public string SKU { get; }
        public string Stocks { get; set; }
        public string MinPrice { get; set; }
        public APISetting APISetting { get; set; }
        public APISetting APISettingSource { get; set; }
        public string Pic { get; set; }
        public string Barcode { get; set; }
      
    }
    public class UniMarketItem : IMarketItem {
        public string Name { get; set ; }
        public string Price { get; set; }
        public string SKU { get; set; }
        public string Stocks { get; set; }
        public string MinPrice { get; set; }
        public APISetting APISetting { get; set; }
        public APISetting APISettingSource { get; set; }
        public string Pic { get; set; }
        public string Barcode { get; set; }
    }
    [Serializable]
   public class MarketItem 
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string Id { get; set; }
        public string SKU { get; set; }
        public int BaseID { get; set; }
        public string Art1C { get; set; }


    [NonSerialized]
        private ObservableCollection<StructLibCore.Marketplace.IMarketItem> items;
        public ObservableCollection<StructLibCore.Marketplace.IMarketItem> Items
        {
            get { return items; }
            set { items = value; }
        }
    }
}
