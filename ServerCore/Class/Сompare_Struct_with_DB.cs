using Pricecona;

using Server.Class.ItemProcessor;

using StructLibs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Server
{



    public class Сompare_PriceStruct_with_DB
    {
        private List<ItemPlusImage> PC_list;
        private readonly List<ItemDBStruct> DB_list;
        private readonly List<ItemChanges> _Result;
        public Сompare_PriceStruct_with_DB(List<ItemPlusImage> Pc_list)
        {
            PC_list = Pc_list;
            _Result = new List<ItemChanges>();
            using (ApplicationContext db = new ApplicationContext())
            {
                DB_list = db.Item.ToList();
            }
        }
        public async Task<List<ItemChanges>> StartCompare()
        {
            await Task.Factory.StartNew(() => Сomparer());
            Console.WriteLine("Найдено {0} отличий в рознице", _Result.Count);
            Console.WriteLine("Новых позиций {0}", Program.Cash.NewItem.Count);
            return _Result;
        }
        private void Сomparer()
        {
            PC_list = new СhangedItemsTags(PC_list, Program.Cash.Dictionaries).Return();

            foreach (var item in PC_list)
            {
                List<ItemDBStruct> FindResult = DB_list.FindAll(x => x.СomparisonName == item.Item.СomparisonName);

                if (FindResult.Count > 1)
                {
                    Console.WriteLine("найден дубликат" + item.Item.Name + " " + FindResult[0].Id.ToString() + " " + FindResult[1].Id.ToString());
                }
                else if (FindResult.Count == 1)
                {
                    if (FindResult[0].PriceRC != item.Item.PriceRC)
                    {
                        _Result.Add(new ItemChanges()
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
                    if (FindResult[0].Description != item.Item.Description)
                    {

                    }


                    //  FindResult[0].Tags

                }
                else if (FindResult.Count == 0) 
                {
                 var X =   Program.Cash.NewItem.FindAll(x => x.Item.СomparisonName == item.Item.СomparisonName);
                    if (X.Count == 0)
                    {
                        Program.Cash.NewItem.Add(item);
                    }
                }
                
            }
        }
    }


}
