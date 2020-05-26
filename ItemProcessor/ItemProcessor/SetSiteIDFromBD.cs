using Pricecona;


using StructLibs;

using System.Collections.Generic;

namespace Server.Class.ItemProcessor
{
    public class SetSiteIDFromBD
    {
        private readonly List<ItemDBStruct> DBList;
        private readonly List<PriceStruct> ComparSiteList;
        public SetSiteIDFromBD()
        {

            //ComparSiteList = Program.Cash.SiteItems.FindAll(x => x.BaseId != 0);
            //DBList = new ItemsQuery().GetAll();
            //foreach (PriceStruct item in ComparSiteList)
            //{
            //    ItemDBStruct Result = DBList.Find(x => x.Id == item.BaseId);
            //    Result.SiteId = item.Id;
            //    new Query.ItemsQuery().Edit(Result);
            //}

        }

    }


}
