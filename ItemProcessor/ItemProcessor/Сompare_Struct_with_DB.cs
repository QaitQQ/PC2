using Pricecona;

using Server.Class.ItemProcessor;

using StructLibs;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    public class Сompare_PriceStruct_with_DB
    {
        private List<PriceStruct> _PC_list;
        private readonly List<ItemDBStruct> _DB_list;
        private readonly List<KeyValuePair<PriceStruct, ItemDBStruct>> _Result;
        public Сompare_PriceStruct_with_DB(List<PriceStruct> PC_list)
        {
            _PC_list = PC_list;
            _Result = new List<KeyValuePair<PriceStruct, ItemDBStruct>>();
            using (ApplicationContext db = new ApplicationContext())
            {
                _DB_list = db.Item.ToList();
            }
        }
        public async Task<List<KeyValuePair<PriceStruct, ItemDBStruct>>> StartCompare()
        {
            await Task.Factory.StartNew(() => Сomparer());
            return _Result;
        }
        private void Сomparer()
        {
            //_PC_list = new СhangedItemsTags(_PC_list).Return();

            //foreach (PriceStruct item in _PC_list)
            //{

            //    List<ItemDBStruct> FindResult = _DB_list.FindAll(x => x.СomparisonName == item.СomparisonName);
            //    if (FindResult.Count == 1)
            //    {
            //        FindResult = FindResult.FindAll(x => x.PriceRC != item.PriceRC || x.Description != item.Description);
            //        if (FindResult.Count == 1) { _Result.Add(new KeyValuePair<PriceStruct, ItemDBStruct>(item, FindResult[0])); }
            //    }
            //    else if (FindResult.Count > 1)
            //    {
            //        ItemDBStruct X = null;
            //        for (int i = 0; i < FindResult.Count; i++)
            //        {
            //            if ((FindResult[i].PriceRC != item.PriceRC || FindResult[i].Description != item.Description) && FindResult[i] != X)
            //            {
            //                _Result.Add(new KeyValuePair<PriceStruct, ItemDBStruct>(item, FindResult[i]));
            //            }
            //            X = FindResult[i];
            //        }
            //    }
            //    else if (FindResult.Count < 1)
            //    {
            //        _Result.Add(new KeyValuePair<PriceStruct, ItemDBStruct>(item, null));
            //    }
            //}
        }
    }


}
