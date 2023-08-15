using Server;

using StructLibCore.Marketplace;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Network.Item.MarketApi
{
    [Serializable]
    public class MarketApi : NetItem { }
    #region Работа с кэшем
    [Serializable]
    public class GetListOption : MarketApi
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            Message.Obj = ((CashClass)Obj).Marketplace.APISettings;
            return Message;
        }
    }
    [Serializable]
    public class SetListOption : MarketApi
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            MarketPlaceCash Z = ((CashClass)Obj).Marketplace;
            Z.APISettings = ((List<APISetting>)Attach);
            ((CashClass)Obj).Marketplace = Z;
            Message.Obj = true;
            return Message;
        }
    }
    [Serializable]
    public class GetItemsList : MarketApi
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            Message.Obj = ((CashClass)Obj).Marketplace.MarketItems;
            return Message;
        }
    }
    [Serializable]
    public class SaveItemsList : MarketApi
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            var Z = ((CashClass)Obj).Marketplace;
            Z.MarketItems = (List<MarketItem>)Attach;
            ((CashClass)Obj).Marketplace = Z;
            Message.Obj = true;
            return Message;
        }
    }
    #endregion
    [Serializable]
    public class RenewItemMarket : MarketApi
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            var X = (IMarketItem[])Attach;
            static List<IGrouping<APISetting, IMarketItem>> ConvertListApi(IMarketItem[] Lst)
            {
                IEnumerable<IGrouping<APISetting, IMarketItem>> X = Lst.GroupBy(x => x.APISetting);
                List<IGrouping<APISetting, IMarketItem>> A = X.ToList();
                return A;
            }
            var Z = ConvertListApi(X);
            bool R = false;
            foreach (var item in Z)
            {
                switch (item.Key.Type)
                {
                    case MarketName.Yandex:
                        R = new Server.Class.IntegrationSiteApi.Market.Yandex.YandexPostItemPrice.YandexPostItemPrice(item.Key).Get(item.ToArray());
                        break;
                    case MarketName.Ozon:
                        //   R = new Server.Class.IntegrationSiteApi.Market.Ozon.OzonSetItem(item.Key).Get(item.ToArray());
                        break;
                    case MarketName.Avito:
                        break;
                    case MarketName.Sber:
                        break;
                    default:
                        break;
                }
            }
            Message.Obj = R;
            return Message;
        }
    }
    [Serializable]
    public class GetListMarket : MarketApi
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            APISetting X = (APISetting)Attach;
            List<IMarketItem> Result = null;
            switch (X.Type)
            {
                case MarketName.Yandex:
                    Result = new Server.Class.IntegrationSiteApi.Market.Yandex.YandexGetName.YandexGetItemList(X).Get();
                    break;
                case MarketName.Ozon:
                    Result = new Server.Class.IntegrationSiteApi.Market.Ozon.OzonPostItemDesc(X).Get();
                    break;
                case MarketName.Avito:
                    break;
                case MarketName.Sber:
                    break;
                default:
                    break;
            }
            Message.Obj = Result;
            return Message;
        }
    }
    [Serializable]
    public class GetListOrders : MarketApi
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            APISetting X = (APISetting)Attach;
            List<object> Result = new List<object>();
            switch (X.Type)
            {
                case MarketName.Yandex:
                    Result = new Server.Class.IntegrationSiteApi.Market.Yandex.YandexGetItemOrders.YandexGetItemOrders(X).Get();
                    break;
                case MarketName.Ozon:
                    Result = new Server.Class.IntegrationSiteApi.Market.Ozon.OzonPortOrderList.OzonPortOrderList(X).Get();
                    break;
                case MarketName.Avito:
                    break;
                case MarketName.Sber:
                    break;
                default:
                    break;
            }
            Message.Obj = Result;
            return Message;
        }
    }
}
