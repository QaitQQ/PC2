using Newtonsoft.Json;

using StructLibCore;
using StructLibCore.Marketplace;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace Server.Class.IntegrationSiteApi.Market.Yandex.YandexPostItemPrice
{
   public class YandexPostItemPrice: SiteApi.IntegrationSiteApi.APIMarket.Yandex.YandexApiClass
    {
        public YandexPostItemPrice(APISetting APISetting) : base(APISetting) {}

        public List<object> Get(StructLibCore.Marketplace.IMarketItem[] List)
        {
            
             //foreach (Server.Class.IntegrationSiteApi.Market.Yandex.YandexGetName.YandexGetItemList.ItemYandex item in List)
            //{

            if (List.Length > 10)
            {
                int countList = List.Length;
                int I = List.Length / 10;
                int End = countList % 10;
                int count = 0;

                for (int i = 0; i < I; i++)
                {
                    count = count + 10;
                    Send(count, count-10);
                   
                }
                if (End >0)
                {
                    Send(count + End, End);
                }
            }
            else { Send(List.Length); }

            void Send(int End,int Start = 0) 
            {
                Root itemsRoot = new Root();
                for (int i = Start; i < End; i++)
                {


                    itemsRoot.offers.Add(new ItemPostPrice(List[i] as Server.Class.IntegrationSiteApi.Market.Yandex.YandexGetName.YandexGetItemList.ItemYandex));
                }
                var httpWebRequest = GetRequest(@"/campaigns/" + ClientID + "/offer-prices/updates.json", "POST");
                string result;
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    var Root = JsonConvert.SerializeObject(itemsRoot);
                    streamWriter.Write(Root);
                }
                try
                {
                    using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                    {
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }
                    }
                }
                catch 
                {
                    MessageBox.Show(Start.ToString() + "=>" + End.ToString());
                 
                }          

            }



            return new List<object>();
        }
    }
    [Serializable]
    public class ItemPostPrice
    {
        public ItemPostPrice(Server.Class.IntegrationSiteApi.Market.Yandex.YandexGetName.YandexGetItemList.ItemYandex Item)
        {
            id = Item.SKU;
            price = Item.Yprice;
            price.value = Item.Price;
        }
        public String id { get; set; }
        public Server.Class.IntegrationSiteApi.Market.Yandex.YandexGetPrice.Price price { get; set; }
    }
    [Serializable]
    public class Root
    {
        public Root() { offers = new List<ItemPostPrice>(); }
        public List<ItemPostPrice> offers { get; set; }
    }

}
