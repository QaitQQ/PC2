using Object_Description;

using Pricecona;

using StructLibs;

using System.Collections.Generic;

namespace Server.Class.ItemProcessor
{
    public class СhangedItemsTags
    {
        private List<ItemPlusImageAndStorege> _List;
        private readonly Dictionaries Dics;
        public СhangedItemsTags(List<ItemPlusImageAndStorege> List, Dictionaries dics)
        {
            _List = List;
            Dics = dics;
            Enumeration();
        }

        private ItemPlusImageAndStorege Doit(ItemPlusImageAndStorege Item)
        {

            Item.Item = new FixName(Dics).Fix(Item.Item);
            Item.Item = new TagGenerator(Dics).Generate(Item.Item);
            return Item;
        }


        private void Enumeration()
        {
            List<ItemPlusImageAndStorege> List = new List<ItemPlusImageAndStorege>();

            foreach (ItemPlusImageAndStorege item in _List)
            {
                List.Add(Doit(item));
            }
            _List = List;
        }

        public List<ItemPlusImageAndStorege> Return() => _List;
    }


}
