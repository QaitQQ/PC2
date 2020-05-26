using Object_Description;

using Pricecona;

using System.Collections.Generic;

namespace Server.Class.ItemProcessor
{
    internal class СhangedItemsTags
    {
        private List<PriceStruct> _List;
        private readonly IEnumerable<IDictionaryPC> Dics;
        public СhangedItemsTags(List<PriceStruct> List, IEnumerable<IDictionaryPC> dics)
        {
            _List = List;
            Dics = dics;
            Enumeration();
        }

        private PriceStruct Doit(PriceStruct Item)
        {

            Item = new FixName().Fix(Item);
            Item = new TagGenerator(Dics).Generate(Item);
            return Item;
        }


        private void Enumeration()
        {
            List<PriceStruct> List = new List<PriceStruct>();

            foreach (PriceStruct item in _List)
            {
                List.Add(Doit(item));
            }
            _List = List;
        }

        public List<PriceStruct> Return() => _List;
    }


}
