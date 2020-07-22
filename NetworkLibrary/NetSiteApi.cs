using Network;
using Network.Item;

using Server;
using Server.Class.IntegrationSiteApi;
using Server.Class.ItemProcessor;

using StructLibs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Network.Item.SiteApi
{
    [Serializable]
    public class NetSiteApi : NetItem { }
    [Serializable]
    public class RenewSiteList : NetSiteApi
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            CashClass cash = ((CashClass)Obj);
            SiteItem SiteApi = new SiteItem(cash.ApiSiteSettngs, cash.FtpSiteSettngs);

            List<ItemChanges> Resul = new List<ItemChanges>();
            int Code = Resul.GetHashCode();
            cash.ObjBuffer.Add(new QueueOfObj() { ID = Code, Object = false });

            Message.Obj = Code;

            SiteApi.ItemListReady += RenewList;
            Task.Factory.StartNew(() => SiteApi.GetAllItemAsync());

            return Message;

            void RenewList() { cash.SiteList = SiteApi.RetunItemList(); var X = cash.ObjBuffer.Find(x => x.ID == Code); X.Object = true; }
        }
    }
    [Serializable]
    public class FixNameFromSiteList : NetSiteApi
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            CashClass cash = ((CashClass)Obj);

            var X = cash.ObjBuffer.Find(x => x.ID == (int)Attach);


            if (((bool)X.Object))
            {

                X.Object = false;

                for (int i = 0; i < cash.SiteList.Count; i++)
                {
                    cash.SiteList[i] = new FixName(cash.Dictionaries).Fix(cash.SiteList[i]);
                    cash.SiteList[i].СomparisonName = new string[] { СomparisonNameGenerator.Get(cash.SiteList[i].Name) };
                }

                X.Object = true;

                Message.Obj = true;
            }
            else
            {
                Message.Obj = false;
            }


            return Message;
        }
    }
    [Serializable]
    public class ComparisonWithDB : NetSiteApi
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            CashClass cash = ((CashClass)Obj);

            var X = cash.ObjBuffer.Find(x => x.ID == (int)Attach);

            if (((bool)X.Object))
            {

                X.Object = false;
                var DB_list = Db.Item.ToList();
                var cmpr = new Сompare_DB_with_SiteList(cash.SiteList, DB_list);

                cmpr.СhangeResult += AddResult;
                cmpr.StartCompare();
                Message.Obj = true;

                void AddResult()
                {
                    var Rsl = cmpr.Result;
                    X.Object = Rsl;
                }

            }
            else
            {
                Message.Obj = false;
            }


            return Message;


        }
    }
    [Serializable]
    public class ReturnComparisonWithDB : NetSiteApi
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            CashClass cash = ((CashClass)Obj);

            var X = cash.ObjBuffer.Find(x => x.ID == (int)Attach);

            if (X.Object is List<ItemChanges>)
            {
                Message.Obj = X.Object;
                cash.ObjBuffer.Remove(X);
            }
            else
            {
                Message.Obj = false;
            }      

            return Message;

        }
    }

    [Serializable]
    public class UpdateSiteRC : NetSiteApi
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            CashClass cash = ((CashClass)Obj);
            SiteItem SiteApi = new SiteItem(cash.ApiSiteSettngs, cash.FtpSiteSettngs);
            var X = (ItemChanges)Attach;

            SiteApi.SetPrice(new KeyValuePair<int, double>(X.ItemID, (double)X.NewValue));

            Message.Obj =  X.ItemID.ToString() + " Обновлено";


            return Message;
        }
    }


}

//new Сompare_PriceStruct_with_DB(Program.Cash.SiteItems).StartCompare().Result;


//KeyValuePair<int, double> ID_Price = (KeyValuePair<int, double>)this.Data.Obj;
//SiteItem SiteApi = new SiteItem(Settings.ApiSettngs);

//bool Result = Task.Factory.StartNew(() => SiteApi.SetPrice(ID_Price)).Result;
