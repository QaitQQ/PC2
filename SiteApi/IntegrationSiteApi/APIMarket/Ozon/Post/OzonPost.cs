﻿
using SiteApi.IntegrationSiteApi;

using StructLibCore.Marketplace;

using System;
using System.Collections.Generic;
using System.Net;

namespace Server.Class.IntegrationSiteApi.Market.Ozon.OzonPost
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
            this.ClientID = aPISetting.ApiString[0];
            this.apiKey = aPISetting.ApiString[1];
        }
        internal HttpWebRequest GetRequest(string Url)
        {
            string url = @"http://api-seller.ozon.ru/" + Url;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
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
    public class OzonItemDesc : IMarketItem
    {
        public OzonItemDesc() { }
        public int id { get; set; }
        public string name { get; set; }
        public string offer_id { get; set; }
        public string barcode { get; set; }
        public string buybox_price { get; set; }
        public int category_id { get; set; }
        public DateTime created_at { get; set; }
        public List<string> images { get; set; }
        public string marketing_price { get; set; }
        public string min_ozon_price { get; set; }
        public string old_price { get; set; }
        public string premium_price { get; set; }
        public string price { get; set; }
        public string recommended_price { get; set; }
        public string min_price { get; set; }
        public List<Source> sources { get; set; }
        public Stocks stocks { get; set; }
        public List<object> errors { get; set; }
        public string vat { get; set; }
        public bool visible { get; set; }
        public VisibilityDetails visibility_details { get; set; }
        public string price_index { get; set; }
        public List<object> images360 { get; set; }
        public string color_image { get; set; }
        public string primary_image { get; set; }
        public Status status { get; set; }
        public string state { get; set; }
        public string service_type { get; set; }
        public string Stocks { get { return stocks.present; } set { stocks.present = value; } }
        public string SKU  {get { return offer_id; }}
        public APISetting APISetting { get; set; }
        public string Name { get { return name; } set { name = value; } }
        public string Price { get { return price; } set { price = value; } }
        public string MinPrice { get { return min_price; } set { min_price = value; } }
        public APISetting APISettingSource { get; set; }
        public string Pic { get { return primary_image; } set { primary_image = value; } }
        public string Barcode { get { return barcode; } set { barcode = value; } }
    }
    [Serializable]
    public class Result_D
    {
        public List<OzonItemDesc> items { get; set; }
    }
    [Serializable]
    public class Root_D
    {
        public Result_D result { get; set; }
    }
    #endregion
}


