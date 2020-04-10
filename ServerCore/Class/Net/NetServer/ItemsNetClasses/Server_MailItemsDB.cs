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
            List<string[]> MassName = new List<string[]>();

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

                    List<KeyValuePair<PriceStruct, ItemDBStruct>> list = Program.Cash.СhangedItems.FindAll(x => x.Value != null && x.Value.PriceRC != x.Key.PriceRC);


                    string t = "=>";
                    foreach (KeyValuePair<PriceStruct, ItemDBStruct> item in list)
                    { MassName.Add(new string[] { item.Value.Id.ToString(), item.Value.СomparisonName, item.Value.PriceRC.ToString() + t + item.Key.PriceRC.ToString(), item.Value.PriceDC.ToString() + t + item.Key.PriceDC.ToString(), item.Key.Description, item.Key.SourceName }); }
                    this.Data.Obj = MassName;
                    break;
                default:
                    break;
            }
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
        private void GetNewPositionName(List<string[]> MassName)
        {
            List<KeyValuePair<Pricecona.PriceStruct, ItemDBStruct>> NewItemList = Program.Cash.СhangedItems.FindAll(x => x.Value == null);
            foreach (KeyValuePair<Pricecona.PriceStruct, ItemDBStruct> item in NewItemList)
            {
                string tags = null;
                if (item.Key.Tags != null)
                {
                    foreach (string X in item.Key.Tags)
                    {
                        tags += X + "/";
                    }
                }
                MassName.Add(new string[] { item.Key.Name + "/" + tags, item.Key.СomparisonName, item.Key.PriceRC.ToString(), item.Key.PriceRC.ToString(), item.Key.Description });
                if (MassName.Count > 500)
                {
                    break;
                }
            }
            this.Data.Obj = MassName;
        }
        private void GetMappedPositionName(List<string[]> MassName)
        {
            List<KeyValuePair<Pricecona.PriceStruct, ItemDBStruct>> list = Program.Cash.СhangedItems.FindAll(x => x.Value != null);
            string t = "=>";
            foreach (KeyValuePair<Pricecona.PriceStruct, ItemDBStruct> item in list)
            { MassName.Add(new string[] { item.Value.Id.ToString(), item.Value.СomparisonName, item.Value.PriceRC.ToString() + t + item.Key.PriceRC.ToString(), item.Value.PriceDC.ToString() + t + item.Key.PriceDC.ToString(), item.Key.Description, item.Key.SourceName }); }
            this.Data.Obj = MassName;
        }
    }
}