using Pricecona;

using Server.Class.Query;

using StructLibs;

using System.Collections.Generic;

using static NetEnum.Selector;
using static NetEnum.Selector.TripleSelector;

namespace Server.Class.Net.NetServer
{
    class Server_MailItemsDB : AbstractNetClass
    {
        private readonly EnumMailDB_TS _Selector;
        public Server_MailItemsDB(TCP_CS_Obj Data) { _Selector = (EnumMailDB_TS)Data.Code[2]; this.Data = Data; Doit(); }
        private void Doit()
        {
            List<СomparisonItems> MassName = new List<СomparisonItems>();

            switch (_Selector)
            {
                case EnumMailDB_TS.CheckMail:
                    new Imap_Checker().Start_Check();
                    break;
                case EnumMailDB_TS.GetMappedPositionName:
                    GetMappedPositionName(MassName);
                    break;
                case EnumMailDB_TS.GetNewPositionName:
                    GetNewPositionName(MassName);
                    break;
                case EnumMailDB_TS.EditItemFromDBAndDelFromMappedList:
                    EditItemFromDBAndDelFromMappedList();
                    break;
                case EnumMailDB_TS.DelItemFromСhangeList:
                    DelItemFromСhangeList();
                    break;
                case EnumMailDB_TS.SearchNewPairName:
                    break;
                case EnumMailDB_TS.SearchMappedPairName:
                    break;
                case EnumMailDB_TS.Retun_Compare_RC:
                    Retun_Compare_RC(MassName);
                    break;
                default:
                    break;
            }
        }

        private void Retun_Compare_RC(List<СomparisonItems> MassName)
        {
            List<KeyValuePair<PriceStruct, ItemDBStruct>> list = Program.Cash.СhangedItems.FindAll(x => x.Value != null && x.Value.PriceRC != x.Key.PriceRC);
            foreach (KeyValuePair<PriceStruct, ItemDBStruct> item in list)
            {
                MassName.Add(new СomparisonItems(item.Key, item.Value));
            }
            this.Data.Obj = MassName;
        }
        private void DelItemFromСhangeList()
        {
            object[] Item = (object[])this.Data.Obj;
            string СomparisonName = (string)Item[0];
            FillTable ListType = (FillTable)Item[1];
            switch (ListType)
            {
                case FillTable.СhangedItemsTable:
                    Program.Cash.СhangedItems = Program.Cash.СhangedItems.FindAll(x => x.Key.СomparisonName != СomparisonName);
                    break;
                case FillTable.NewItemTable:
                    Program.Cash.СhangedItems = Program.Cash.СhangedItems.FindAll(x => x.Key.СomparisonName != СomparisonName);
                    break;
                case FillTable.СhangedSiteTable:
                    Program.Cash.SiteItemsСhanged = Program.Cash.СhangedItems.FindAll(x => x.Key.СomparisonName != СomparisonName);
                    break;
                case FillTable.СhangedItemsTableSelected:
                    Program.Cash.СhangedItems = Program.Cash.СhangedItems.FindAll(x => x.Key.СomparisonName != СomparisonName);
                    break;
                default:
                    break;
            }
        }
        private void EditItemFromDBAndDelFromMappedList()
        {
            ItemDBStruct Item = (ItemDBStruct)this.Data.Obj;
            new ItemsQuery().Edit(Item);
            KeyValuePair<Pricecona.PriceStruct, ItemDBStruct> FindDel = Program.Cash.СhangedItems.Find(x => x.Value != null && x.Value.Id == Item.Id);
            Program.Cash.СhangedItems.Remove(FindDel);
            Program.Cash.СhangedItems = Program.Cash.СhangedItems;
            this.Data.Obj = true;
        }
        private void GetNewPositionName(List<СomparisonItems> MassName)
        {
            List<KeyValuePair<Pricecona.PriceStruct, ItemDBStruct>> NewItemList = Program.Cash.СhangedItems.FindAll(x => x.Value == null);

                foreach (KeyValuePair<PriceStruct, ItemDBStruct> item in NewItemList)
                {
                    MassName.Add(new СomparisonItems(item.Key, item.Value));
                if (MassName.Count > 500)
                {
                    break;
                }
            }

            
            this.Data.Obj = MassName;
        }
        private void GetMappedPositionName(List<СomparisonItems> MassName)
        {
            List<KeyValuePair<Pricecona.PriceStruct, ItemDBStruct>> list = Program.Cash.СhangedItems.FindAll(x => x.Value != null);

            foreach (KeyValuePair<PriceStruct, ItemDBStruct> item in list)
            {
                MassName.Add(new СomparisonItems(item.Key, item.Value));
            }
            this.Data.Obj = MassName;
        }
    }
}