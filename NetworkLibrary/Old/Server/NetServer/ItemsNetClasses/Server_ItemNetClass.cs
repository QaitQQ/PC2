
//namespace Server.Class.Net.NetServer
//{
//    internal class Server_ItemNetClass : AbstractNetClass
//    {
//        private void AllowAllСhange()
//        {

//            var mesrge = (object[])this.Data.Obj;
//            FillTable FillTable = (FillTable)mesrge[0];
//            switch (FillTable)
//            {
//                case FillTable.СhangedItemsTable:
//                    List<KeyValuePair<PriceStruct, ItemDBStruct>> СhangedItems = Program.Cash.СhangedItems.FindAll(x => x.Value != null);
//                    List<ItemDBStruct> СhangedItemsList = new List<ItemDBStruct>();
//                    foreach (var item in СhangedItems)
//                    {
//                        ItemDBStruct newItem = item.Value;
//                        newItem.PriceRC = item.Key.PriceRC;
//                        newItem.PriceDC = item.Key.PriceDC;
//                        newItem.Description = item.Key.Description;
//                        newItem.DateСhange = item.Key.DateСhange;
//                        newItem.SourceName = item.Key.SourceName;
//                        СhangedItemsList.Add(newItem);
//                    }
//                    new Query.ItemsQuery().EditList(СhangedItemsList);
//                    Program.Cash.ItemName = new Class.Query.NameCash().GetCashItemName();
//                    Program.Cash.СhangedItems = Program.Cash.СhangedItems.FindAll(x => x.Value == null);

//                    break;
//                case FillTable.NewItemTable:

//                    List<KeyValuePair<PriceStruct, ItemDBStruct>> Items = Program.Cash.СhangedItems.FindAll(x => x.Value == null);

//                    List<PriceStruct> NewList = new List<PriceStruct>();
//                    foreach (var item in Items)
//                    {
//                        NewList.Add(item.Key);
//                    }
//                    new Query.ItemsQuery().AddListPriceStruct(NewList);
//                    Program.Cash.ItemName = new Class.Query.NameCash().GetCashItemName();
//                    Program.Cash.СhangedItems = Program.Cash.СhangedItems.FindAll(x => x.Value != null);

//                    break;
//                case FillTable.СhangedSiteTable:

//                    KeyValuePair<int, double> ID_Price = (KeyValuePair<int, double>)this.Data.Obj;
//                    SiteItem SiteApi = new SiteItem(Settings.ApiSettngs);

//                    bool Result = Task.Factory.StartNew(() => SiteApi.SetPrice(ID_Price)).Result;

//                    this.Data.Obj = Result;

//                    if (Result)
//                    {
//                        KeyValuePair<Pricecona.PriceStruct, ItemDBStruct> accept = Program.Cash.SiteItemsСhanged.Find(x => x.Key.Id == ID_Price.Key);
//                        Program.Cash.SiteItemsСhanged.Remove(accept);
//                    }


//                    break;
//                case NetEnum.Selector.FillTable.СhangedItemsTableSelected:

//                    List<string> listName = (List<string>)mesrge[1];
//                    List<ItemDBStruct> FindListСhangedItemsTableSelected = new List<ItemDBStruct>();
//                    foreach (var Item in listName)
//                    {
//                        int ID = Convert.ToInt32(Item);

//                        var item = Program.Cash.СhangedItems.Find(x => x.Value?.Id == ID);

//                        if (item.Value != null)
//                        {
//                            ItemDBStruct newItem = item.Value;

//                            newItem.PriceRC = item.Key.PriceRC;
//                            newItem.PriceDC = item.Key.PriceDC;
//                            newItem.Description = item.Key.Description;
//                            newItem.DateСhange = item.Key.DateСhange;
//                            newItem.SourceName = item.Key.SourceName;

//                            FindListСhangedItemsTableSelected.Add(newItem);

//                            Program.Cash.СhangedItems = Program.Cash.СhangedItems.FindAll(x => x.Value?.Id == ID);
//                        }

//                    }
//                    new Query.ItemsQuery().EditList(FindListСhangedItemsTableSelected);
//                    Program.Cash.ItemName = new Class.Query.NameCash().GetCashItemName();

//                    break;
//            }
//        }
//        private void Add_New_position_from_СhangeList()
//        {
//            string СomparisonName = (string)this.Data.Obj;
//            List<KeyValuePair<PriceStruct, ItemDBStruct>> Item = Program.Cash.СhangedItems.FindAll(x => x.Key.СomparisonName == СomparisonName);
//            new Query.ItemsQuery().AddPriceStruct(Item[0].Key);
//            Program.Cash.ItemName = new Class.Query.NameCash().GetCashItemName();
//        }
//        private static void Del_From_DB(TCP_CS_Obj Data)
//        {
//            int ID = (int)Data.Obj;
//            ItemDBStruct item = new Class.Query.ItemsQuery().FindID(ID)[0];
//            new Class.Query.ItemsQuery().Del(item);
//            Program.Cash.ItemName.RemoveAll(x => x.Id == ID);
//        }
//        public Server_MailItemsDB(TCP_CS_Obj Data) { _Selector = (EnumMailDB_TS)Data.Code[2]; this.Data = Data; Doit(); }
//        private void Retun_Compare_RC(List<СomparisonItems> MassName)
//        {
//            List<KeyValuePair<PriceStruct, ItemDBStruct>> list = Program.Cash.СhangedItems.FindAll(x => x.Value != null && x.Value.PriceRC != x.Key.PriceRC);
//            foreach (KeyValuePair<PriceStruct, ItemDBStruct> item in list)
//            {
//                MassName.Add(new СomparisonItems(item.Key, item.Value));
//            }
//            this.Data.Obj = MassName;
//        }
//        private void DelItemFromСhangeList()
//        {
//            object[] Item = (object[])this.Data.Obj;
//            string СomparisonName = (string)Item[0];
//            FillTable ListType = (FillTable)Item[1];
//            switch (ListType)
//            {
//                case FillTable.СhangedItemsTable:
//                    Program.Cash.СhangedItems = Program.Cash.СhangedItems.FindAll(x => x.Key.СomparisonName != СomparisonName);
//                    break;
//                case FillTable.NewItemTable:
//                    Program.Cash.СhangedItems = Program.Cash.СhangedItems.FindAll(x => x.Key.СomparisonName != СomparisonName);
//                    break;
//                case FillTable.СhangedSiteTable:
//                    Program.Cash.SiteItemsСhanged = Program.Cash.СhangedItems.FindAll(x => x.Key.СomparisonName != СomparisonName);
//                    break;
//                case FillTable.СhangedItemsTableSelected:
//                    Program.Cash.СhangedItems = Program.Cash.СhangedItems.FindAll(x => x.Key.СomparisonName != СomparisonName);
//                    break;
//                default:
//                    break;
//            }
//        }
//        private void EditItemFromDBAndDelFromMappedList()
//        {
//            ItemDBStruct Item = (ItemDBStruct)this.Data.Obj;
//            new ItemsQuery().Edit(Item);
//            KeyValuePair<Pricecona.PriceStruct, ItemDBStruct> FindDel = Program.Cash.СhangedItems.Find(x => x.Value != null && x.Value.Id == Item.Id);
//            Program.Cash.СhangedItems.Remove(FindDel);
//            Program.Cash.СhangedItems = Program.Cash.СhangedItems;
//            this.Data.Obj = true;
//        }
//        private void GetNewPositionName(List<СomparisonItems> MassName)
//        {
//            List<KeyValuePair<Pricecona.PriceStruct, ItemDBStruct>> NewItemList = Program.Cash.СhangedItems.FindAll(x => x.Value == null);

//            foreach (KeyValuePair<PriceStruct, ItemDBStruct> item in NewItemList)
//            {
//                MassName.Add(new СomparisonItems(item.Key, item.Value));
//                if (MassName.Count > 500)
//                {
//                    break;
//                }
//            }


//            this.Data.Obj = MassName;
//        }
//        private void GetMappedPositionName(List<СomparisonItems> MassName)
//        {
//            List<KeyValuePair<Pricecona.PriceStruct, ItemDBStruct>> list = Program.Cash.СhangedItems.FindAll(x => x.Value != null);

//            foreach (KeyValuePair<PriceStruct, ItemDBStruct> item in list)
//            {
//                MassName.Add(new СomparisonItems(item.Key, item.Value));
//            }
//            this.Data.Obj = MassName;
//        }
//        private void GetComparedItems(List<СomparisonItems> MassName)
//        {
//            List<KeyValuePair<Pricecona.PriceStruct, ItemDBStruct>> list = Program.Cash.SiteItemsСhanged.FindAll(x => x.Value != null);

//            foreach (KeyValuePair<Pricecona.PriceStruct, ItemDBStruct> item in list)
//            {
//                if (item.Key.PriceRC != item.Value.PriceRC)
//                {
//                    MassName.Add(new СomparisonItems(item.Key, item.Value));
//                }

//            }
//            this.Data.Obj = MassName;
//        }
//        private static void SiteItemCompared()
//        {
//            Program.Cash.SiteItemsСhanged = new Сompare_PriceStruct_with_DB(Program.Cash.SiteItems).StartCompare().Result;
//        }
//        private void SetPrice()
//        {
//            KeyValuePair<int, double> ID_Price = (KeyValuePair<int, double>)this.Data.Obj;
//            SiteItem SiteApi = new SiteItem(Settings.ApiSettngs);

//            bool Result = Task.Factory.StartNew(() => SiteApi.SetPrice(ID_Price)).Result;

//            this.Data.Obj = Result;

//            if (Result)
//            {
//                KeyValuePair<Pricecona.PriceStruct, ItemDBStruct> accept = Program.Cash.SiteItemsСhanged.Find(x => x.Key.Id == ID_Price.Key);
//                Program.Cash.SiteItemsСhanged.Remove(accept);
//            }
//        }
//        private void RenewSiteListAsync()
//        {
//            SiteItem SiteApi = new SiteItem(Settings.ApiSettngs);
//            ItemDeligate itemDeligate = new ItemDeligate(SiteApi.RetunItemList);
//            Task.Factory.StartNew(() => SiteApi.GetAllItemAsync());
//            SiteApi.ItemListReady += itemDeligate;
//        }
//    }
//}