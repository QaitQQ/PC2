using Pricecona;

using StructLibs;

using System;
using System.Collections.Generic;

namespace Object_Description
{
    [Serializable]
    public class Compare_Item_Result
    {
        public List<KeyValuePair<PriceStruct, ItemDBStruct>> Mapped { get; set; }

        public List<PriceStruct> New { get; set; }


    }


}
