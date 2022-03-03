using System;
using System.Collections.Generic;
using System.Text;

namespace StructLibCore
{

    public interface IMarketItem
    {
        public int id { get; set; }
        public string name { get; set; }
        public string price { get; set; }
    }


    [Serializable]
   public class MarketItem 
    {
        public string Id { get; set; }
        public string SKU { get; set; }
        public int BaseID { get; set; }
        [NonSerialized] public IMarketItem Item;
    }

}
