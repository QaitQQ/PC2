using Server.Class.ItemProcessor;

using StructLibs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    public class Сompare_NewPrice_with_DB
    {
        public Action<string> СhangeResult;
        private List<ItemPlusImageAndStorege> PC_list;
        private readonly List<ItemDBStruct> DB_list;
        private List<ItemChanges> result;
        public List<ItemChanges> Result { get => result; set { result = value; СhangeResult?.Invoke(result.Count.ToString() + "совпадений" + Cash.NewItem.Count.ToString() + "новых"); } }
        private readonly CashClass Cash;
        public Сompare_NewPrice_with_DB(List<ItemPlusImageAndStorege> Pc_list, List<ItemDBStruct> DB_list, CashClass Cash)
        {
            PC_list = Pc_list;
            result = new List<ItemChanges>();
            this.DB_list = DB_list;
            this.Cash = Cash;
        }
        public async void StartCompare()
        {
            await Task.Factory.StartNew(() => Сomparer());
            Cash.ChangedItems = Result;
        }
        private void Сomparer()
        {
            PC_list = new СhangedItemsTags(PC_list, Cash.Dictionaries).Return();

            List<ItemPlusImageAndStorege> storegeUpdateList = new List<ItemPlusImageAndStorege>();

            foreach (ItemPlusImageAndStorege item in PC_list)
            {
                if (item == null)
                {
                    continue;
                }

                List<ItemDBStruct> FindResult = DB_list.FindAll(t => t.СomparisonName.Intersect(item.Item.СomparisonName).Any());

                if (FindResult.Count > 1)  
                {

                  var  F2 = FindResult.FindAll(x => x.Name == item.Item.Name);

                    if (F2.Count > 0)
                    {
                        AddMappetItem(storegeUpdateList, item, F2);
                    }

                }
                else if (FindResult.Count == 1)
                {
                    AddMappetItem(storegeUpdateList, item, FindResult);
                }
                else if (FindResult.Count == 0)
                {
                        List<ItemPlusImageAndStorege> X = Cash.NewItem.FindAll(t => t != null && t.Item.СomparisonName.Intersect(item.Item.СomparisonName).Any());
                        if (X.Count == 0 && item != null && item.Item.СomparisonName != null) { Cash.NewItem.Add(item); }

                  

                    //List<ItemDBStruct> FindResult2 = DB_list.FindAll(x => x.СomparisonName.Contains(item.Item.СomparisonName));
                    //if (FindResult2.Count == 1)
                    //{
                      
                    //}
                    //if (FindResult2.Count > 1)
                    //{
                    //}
                    //else if (FindResult2.Count == 0)
                    //{
                    //    List<ItemDBStruct> FindResult3 = DB_list.FindAll(x => item.Item.СomparisonName.Contains(x.СomparisonName));
                    //    if (FindResult3.Count == 1)
                    //    {
                    //      //  AddMappetItem(storegeUpdateList, item, FindResult3); 
                    //    }
                    //    if (FindResult3.Count == 0)
                    //    {
                    //      //  List<ItemPlusImageAndStorege> X = Cash.NewItem.FindAll(x => x.Item.СomparisonName == item.Item.СomparisonName); if (X.Count == 0)  {   Cash.NewItem.Add(item); }
                    //    }
                    //    if (FindResult3.Count > 1 && FindResult3[0].Name.Length > 4)
                    //    {

                    //    }
                //    }            
                }
            }
            if (storegeUpdateList.Count>0)
            {
                new UpdatingStorege(storegeUpdateList);
            }


        }

        private void AddMappetItem(List<ItemPlusImageAndStorege> storegeUpdateList, ItemPlusImageAndStorege item, List<ItemDBStruct> FindResult)
        {
            if (item.Storages != null)
            {
                item.Item.Id = FindResult[0].Id;
                storegeUpdateList.Add(item);
            }
            if (FindResult[0].PriceRC != item.Item.PriceRC && item.Item.PriceRC != 0)
            {
                Result.Add(new ItemChanges()
                {
                    DateTime = DateTime.Now,
                    ItemID = FindResult[0].Id,
                    NewValue = item.Item.PriceRC,
                    OldValue = FindResult[0].PriceRC,
                    FieldName = "PriceRC",
                    ItemName = FindResult[0].Name,
                    Source = item.Item.SourceName

                });

            }
            if (FindResult[0].PriceDC != item.Item.PriceDC && item.Item.PriceDC != 0)
            {
                Result.Add(new ItemChanges()
                {
                    DateTime = DateTime.Now,
                    ItemID = FindResult[0].Id,
                    NewValue = item.Item.PriceDC,
                    OldValue = FindResult[0].PriceDC,
                    FieldName = "PriceDC",
                    ItemName = FindResult[0].Name,
                    Source = item.Item.SourceName

                });

            }
            if (FindResult[0].Description != item.Item.Description)
            {

            }
        }
    }


    public class Сompare_DB_with_SiteList
    {
        public event Action СhangeResult;
        private readonly List<ItemDBStruct> DB_list;
        private readonly List<ItemDBStruct> Site_list;
        private List<ItemChanges> result;
        public List<ItemChanges> Result { get => result; set { result = value; СhangeResult?.Invoke(); } }
        public async void StartCompare()
        {
            await Task.Factory.StartNew(() => Сomparer());
        }
        private void Сomparer()
        {
            foreach (ItemDBStruct item in Site_list)
            {
                List<ItemDBStruct> FindResult = DB_list.FindAll(x => item.СomparisonName == x.СomparisonName);

                if (FindResult.Count == 0)
                {
                    FindResult = DB_list.FindAll(x=> item.СomparisonName.Intersect(x.СomparisonName).Any());

                    if (FindResult.Count == 1)
                    {
                        if (FindResult[0].PriceRC != item.PriceRC )
                        {
                            Result.Add(new ItemChanges()
                            {
                                DateTime = DateTime.Now,
                                ItemID = item.Id,
                                NewValue = FindResult[0].PriceRC,
                                OldValue = item.PriceRC,
                                FieldName = "PriceRC",
                                ItemName = FindResult[0].Name + "=?=" + item.Name,
                                Source = FindResult[0].SourceName

                            });

                        }
                    }

                }
                else if (FindResult.Count == 1)
                {
                    if (FindResult[0].PriceRC != item.PriceRC)
                    {
                        Result.Add(new ItemChanges()
                        {
                            DateTime = DateTime.Now,
                            ItemID = item.Id,
                            NewValue = FindResult[0].PriceRC,
                            OldValue = item.PriceRC,
                            FieldName = "PriceRC",
                            ItemName = FindResult[0].Name,
                            Source = FindResult[0].SourceName

                        });
                    }
                    if (FindResult[0].Description != item.Description)
                    {

                    }
                }
            }

            СhangeResult?.Invoke();

        }

        public Сompare_DB_with_SiteList(List<ItemDBStruct> Site_list, List<ItemDBStruct> DB_list)
        {
            this.Site_list = Site_list;
            result = new List<ItemChanges>();
            this.DB_list = DB_list;
        }
    }
}
