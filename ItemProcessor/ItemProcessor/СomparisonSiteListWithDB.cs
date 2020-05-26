using Pricecona;


using StructLibs;

using System.Collections.Generic;
using System.Windows;

namespace Server.Class.ItemProcessor
{
    public class СomparisonSiteListWithDB
    {

        private readonly List<ItemDBStruct> DBList;

        private readonly List<PriceStruct> _TargetList;
        private readonly List<PriceStruct> _ResultList;
      //  public СomparisonSiteListWithDB(List<PriceStruct> TargetList)
        //{
        //    DBList = new ItemsQuery().GetAll();

        //    _TargetList = TargetList;
        //    _ResultList = new List<PriceStruct>();
        //    MessageBox.Show(DBList.Count.ToString() + " " + _TargetList.Count.ToString());


        //    foreach (PriceStruct item in TargetList)
        //    {
        //        PriceStruct newitem = СomparisonNameGenerator.GetForPriceStruct(item);
        //        List<ItemDBStruct> FindResult = DBList.FindAll(x => x.СomparisonName.Contains(newitem.СomparisonName));
        //        switch (FindResult.Count)
        //        {
        //            case 0:
        //                _ResultList.Add(newitem);
        //                break;
        //            case 1:
        //                newitem = new PriceStruct(newitem, BaseId: FindResult[0].Id);
        //                _ResultList.Add(newitem);
        //                break;
        //            default:
        //                List<ItemDBStruct> TotalResult = FindResult.FindAll(x => newitem.СomparisonName == x.СomparisonName);

        //                if (TotalResult.Count == 1)
        //                {
        //                    newitem = new PriceStruct(newitem, BaseId: TotalResult[0].Id);
        //                    _ResultList.Add(newitem);
        //                }
        //                else
        //                {
        //                    string str = newitem.СomparisonName + "\n";
        //                    foreach (ItemDBStruct XXX in FindResult)
        //                    {
        //                        str = str + XXX.СomparisonName + XXX.Id.ToString() + "\n";
        //                    }
        //                    _ResultList.Add(newitem);

        //                }
        //                break;
        //        }
        //    }
        //    Program.Cash.SiteItems = _ResultList;
      //  }
    }


}
