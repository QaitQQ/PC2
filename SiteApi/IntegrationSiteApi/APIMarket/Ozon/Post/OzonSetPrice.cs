using Newtonsoft.Json;

using StructLibCore;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;

namespace Server.Class.IntegrationSiteApi.Market.Ozon
{
    //public class OzonSetPrice : OzonPost
    //{
    //    public OzonSetPrice(StructLibCore.Marketplace.IMarketItem[] Lst) { this.Lst = Lst; }
    //    private StructLibCore.Marketplace.IMarketItem[] Lst { get; set; }
    //    public List<object> Get()
    //    {
    //        var LST = ConvertListApi();
    //        foreach (var item in LST)
    //        {
    //            ClientID = item.Key.ApiString[0];
    //            apiKey = item.Key.ApiString[1];

    //            var httpWebRequest = GetRequest(@"v2/product/import");

    //            Root items = new Root() { items = ConverItems(item.ToList()) };

    //            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
    //            {
    //                var root = JsonConvert.SerializeObject(items);
    //                streamWriter.Write(root);
    //            }
    //            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
    //            using var streamReader = new StreamReader(httpResponse.GetResponseStream()); result = streamReader.ReadToEnd();
    //        }
    //        return new List<object>();
    //    }

    //    private List<IGrouping<StructLibCore.Marketplace.APISetting, StructLibCore.Marketplace.IMarketItem>> ConvertListApi()
    //    {
    //        IEnumerable<IGrouping<StructLibCore.Marketplace.APISetting, StructLibCore.Marketplace.IMarketItem>> X = Lst.GroupBy(x => x.APISetting);
    //        List<IGrouping<StructLibCore.Marketplace.APISetting, StructLibCore.Marketplace.IMarketItem>> A = X.ToList();
    //        return A;
    //    }
    //    private List<SetOzonItem> ConverItems(List<StructLibCore.Marketplace.IMarketItem> lst)
    //    {
    //        List<SetOzonItem> Nlst = new List<SetOzonItem>();

    //        foreach (var item in lst)
    //        {
    //            Nlst.Add(new SetOzonItem((OzonItemDesc)item));
    //        }

    //        return Nlst;
    //    }
    
    //}
}


