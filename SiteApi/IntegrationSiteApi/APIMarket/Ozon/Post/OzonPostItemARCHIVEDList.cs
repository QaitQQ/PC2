using Newtonsoft.Json;

using SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post;

using StructLibCore.Marketplace;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace Server.Class.IntegrationSiteApi.Market.Ozon
{
    public class OzonPostItemARCHIVEDList : SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post.OzonPost
    {
        public OzonPostItemARCHIVEDList(APISetting aPISetting) : base(aPISetting)
        {
        }

        public List<IMarketItem> Get()
        {
            var httpWebRequest = GetRequest(@"v2/product/list");

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(new ARCHIVEDListRequest());
                streamWriter.Write(json);
            }
            Root RequestId = new Root();
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }
                RequestId = JsonConvert.DeserializeObject<Root>(result);
            }
            catch (System.Exception e)
            {


            }


            ItemQ itemQ = new ItemQ();

            foreach (var item in RequestId.result.items) { itemQ.offer_id.Add(item.offer_id); }

            List<IMarketItem> NLST = new List<IMarketItem>();
            httpWebRequest = GetRequest(@"v2/product/info/list");

            try
            {
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream())) { var root = JsonConvert.SerializeObject(itemQ); streamWriter.Write(root); }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }
                Root_D End = JsonConvert.DeserializeObject<Root_D>(result);


                List<string> IDS = new List<string>();

                foreach (var item in End.result.items) { IDS.Add(item.id.ToString()); }

                var PriceInfoList = new OzonPostPriceInfo(aPISetting).Get(IDS, OzonPostPriceInfo.PriceInfoType.product);

                foreach (OzonItemDesc item in End.result.items) { item.Priceinfo = PriceInfoList?.FirstOrDefault(x => x.ProductId == item.id); NLST.Add(item); }

            }
            catch (System.Exception e)
            {
                return NLST;

            }
            return NLST;

        }

        public class Filter
        {
            public Filter()
            {
                Visibility = "ARCHIVED";
            }

            [JsonProperty("offer_id")]
            public List<string> OfferId { get; set; }

            [JsonProperty("product_id")]
            public List<string> ProductId { get; set; }

            [JsonProperty("visibility")]
            public string Visibility { get; set; }
        }

        public class ARCHIVEDListRequest
        {
            public ARCHIVEDListRequest()
            {
                Filter = new Filter();
                Limit = 1000;
            }

            [JsonProperty("filter")]
            public Filter Filter { get; set; }

            [JsonProperty("last_id")]
            public string LastId { get; set; }

            [JsonProperty("limit")]
            public int Limit { get; set; }
        }


    }

}


