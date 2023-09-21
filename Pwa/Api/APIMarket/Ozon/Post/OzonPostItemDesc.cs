using Newtonsoft.Json;

using SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post;

using StructLibCore.Marketplace;

using System.Net;

namespace Server.Class.IntegrationSiteApi.Market.Ozon
{
    public class OzonPostItemDesc : SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post.OzonPost
    {
        public OzonPostItemDesc(APISetting aPISetting) : base(aPISetting)
        {
        }

        public List<IMarketItem> Get(List<string> Ids = null!)
        {
            HttpWebRequest httpWebRequest = GetRequest(@"v2/product/info/list");
            ItemQ itemQ = new ItemQ();
            if (Ids == null)
            {
                List<Item> Lst = new OzonPostItemList(this.aPISetting!).Get();
                foreach (Item item in Lst) { itemQ.offer_id?.Add(item.offer_id!); }
            }
            else
            {
                foreach (string item in Ids) { itemQ.offer_id?.Add(item); }
            }


            using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream())) { string root = JsonConvert.SerializeObject(itemQ); streamWriter.Write(root); }
            HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }
            if (result != null)
            {
                Root_D End = JsonConvert.DeserializeObject<Root_D>(result)!;


                List<IMarketItem> NLST = new List<IMarketItem>();

                List<string> IDS = new List<string>();

                foreach (OzonItemDesc item in End?.result?.items!) { IDS.Add(item.id.ToString()); }

                List<OzonPostPriceInfo.Item> PriceInfoList = new OzonPostPriceInfo(aPISetting!).Get(IDS, OzonPostPriceInfo.PriceInfoType.product);

                foreach (OzonItemDesc item in End.result.items) { item.PriceInfo = PriceInfoList.FirstOrDefault(x => x.ProductId == item.id); NLST.Add(item); }


                return NLST;
            }
            return null!;
        }
    }
}


