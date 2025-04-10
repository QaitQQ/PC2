﻿using Newtonsoft.Json;

using SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post;

using StructLibCore.Marketplace;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace Server.Class.IntegrationSiteApi.Market.Ozon
{
    public class OzonPostItemDesc : SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post.OzonPost
    {
        public OzonPostItemDesc(APISetting aPISetting) : base(aPISetting)
        {
        }

        public List<IMarketItem> Get(List<string> Ids = null)
        {
            var httpWebRequest = GetRequest(@"v3/product/info/list");
            ItemQ itemQ = new ItemQ();
            if (Ids == null)
            {
                var Lst = new OzonPostItemList(this.aPISetting).Get();               
                foreach (var item in Lst) { itemQ.offer_id.Add(item.offer_id); }
            }
            else
            {
                foreach (var item in Ids) { itemQ.offer_id.Add(item); }
            }
            List<IMarketItem> NLST = new List<IMarketItem>();
            try
            {
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream())) { var root = JsonConvert.SerializeObject(itemQ); streamWriter.Write(root); }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }
                Root_D End = JsonConvert.DeserializeObject<Root_D>(result);


                List<string> IDS = new List<string>();

                foreach (var item in End.Items) { IDS.Add(item.Id.ToString()); }

                var PriceInfoList = new OzonPostPriceInfo(aPISetting).Get(IDS, OzonPostPriceInfo.PriceInfoType.product);

                foreach (var item in End.Items) { item.Priceinfo = PriceInfoList?.FirstOrDefault(x => x.ProductId == item.Id); NLST.Add(item); }

            }
            catch (System.Exception e)
            {
                return NLST;

            }


            return NLST;
        }
    }
}


