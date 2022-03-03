using Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace Network.Item.MarketApi
{
    [Serializable]
    public class NetSiteApi : NetItem { }

    [Serializable]
    public class GetListOzon : NetSiteApi
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            Message.Obj = new Server.Class.IntegrationSiteApi.Market.Ozon.OzonGetItemDesc("205511", "ac7753b5-132b-4b72-a4fd-f21dccbc9587").Get();
            return Message;      
        }
    }













}
