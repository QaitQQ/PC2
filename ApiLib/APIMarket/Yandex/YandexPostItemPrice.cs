﻿using Newtonsoft.Json;

using StructLibCore.Marketplace;

using System.Net;
namespace Server.Class.IntegrationSiteApi.Market.Yandex.YandexPostItemPrice
{
    public class YandexPostItemPrice : SiteApi.IntegrationSiteApi.APIMarket.Yandex.YandexApiClass
    {
        public YandexPostItemPrice(APISetting APISetting) : base(APISetting) { }
        public bool Get(IMarketItem[] List)
        {
            if (List.Length > 10)
            {
                int countList = List.Length;
                int I = List.Length / 10;
                int End = countList % 10;
                int count = 0;
                for (int i = 0; i < I; i++)
                {
                    count = count + 10;
                    Send(count, count - 10);
                }
                if (End > 0)
                {
                    Send(count + End, End);
                }
            }
            else { Send(List.Length); }
            void Send(int End, int Start = 0)
            {
                Request itemsRoot = new Request();
                for (int i = Start; i < End; i++)
                {
                    YandexGetName.YandexGetItemList.ItemYandex? X = List?[i]! as YandexGetName.YandexGetItemList.ItemYandex;
                    Offer offer = new Offer(X!);

                    itemsRoot.Offers.Add(offer);
                }
                HttpWebRequest httpWebRequest = GetRequest(@"campaigns/" + ClientID + "/offer-prices/updates", "POST");
                string result;
                using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string Root = JsonConvert.SerializeObject(itemsRoot);
                    streamWriter.Write(Root);
                }
                try
                {
                    using HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream());
                    result = streamReader.ReadToEnd();
                }
                catch
                {

                }
            }
            return true;
        }
    }
    //[Serializable]
    //public class ItemPostPrice
    //{
    //    public ItemPostPrice(Server.Class.IntegrationSiteApi.Market.Yandex.YandexGetName.YandexGetItemList.ItemYandex Item)
    //    {
    //        //OfferId = Item.SKU;
    //        ShopSku = Item.SKU;
    //       // Id = Item.SKU;
    //      //  Price = new PriceDTO();
    //     //   Price.Value = Item.Price.Replace(',','.');
    //      //  price.DiscountBase = Item.Price;
    //    }
    //    [JsonProperty("offerId")]
    //    public string?OfferId { get; set; }
    //    [JsonProperty("shopSku")]
    //    public string?ShopSku { get; set; }
    //    [JsonProperty("id")]
    //    public string?Id { get; set; }
    //  //  [JsonProperty("price")]
    //  //  public PriceDTO Price { get; set; }
    //}
    //[Serializable]
    //public class Root
    //{
    //    public Root() { offers = new List<ItemPostPrice>(); }
    //    [JsonProperty("OfferPriceDTO")]
    //    public List<ItemPostPrice> offers { get; set; }
    //}
    //public class PriceDTO
    //{
    //    public  PriceDTO() { CurrencyId = "RUR";  }
    //    [JsonProperty("value")]
    //    public string?Value { get; set; }
    //    [JsonProperty("discountBase")]
    //    public string?DiscountBase { get; set; }
    //    [JsonProperty("currencyId")]
    //    public string?CurrencyId { get; set; }
    //    [JsonProperty("vat")]
    //    public string?Vat { get; set; }
    //}
    public class Feed
    {
        [JsonProperty("id")]
        public int Id { get; set; }
    }
    public class Offer
    {
        public Offer(Server.Class.IntegrationSiteApi.Market.Yandex.YandexGetName.YandexGetItemList.ItemYandex Item)
        {
            OfferId = Item.SKU;
            Id = Item.SKU;
            Price = new Price
            {
                Value = Item.Price?.Replace(',', '.')
            };
            Feed = new Feed();
        }
        [JsonProperty("offerId")]
        public string? OfferId { get; set; }
        [JsonProperty("id")]
        public string? Id { get; set; }
        [JsonProperty("feed")]
        public Feed Feed { get; set; }
        [JsonProperty("price")]
        public Price Price { get; set; }
        [JsonProperty("marketSku")]
        public string? MarketSku { get; set; }
        [JsonProperty("shopSku")]
        public string? ShopSku { get; set; }
    }
    public class Price
    {
        [JsonProperty("value")]
        public string? Value { get; set; }
        [JsonProperty("discountBase")]
        public string? DiscountBase { get; set; }
        [JsonProperty("currencyId")]
        public string? CurrencyId = "RUR";
        [JsonProperty("vat")]
        public string? Vat { get; set; }
    }
    public class Request
    {
        public Request() { Offers = new List<Offer>(); }
        [JsonProperty("offers")]
        public List<Offer> Offers { get; set; }
    }
}
