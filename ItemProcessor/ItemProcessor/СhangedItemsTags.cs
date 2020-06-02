using Object_Description;

using Pricecona;

using StructLibs;

using System.Collections.Generic;

namespace Server.Class.ItemProcessor
{
    public class СhangedItemsTags
    {
        private List<ItemPlusImage> _List;
        private readonly Dictionaries Dics;
        public СhangedItemsTags(List<ItemPlusImage> List, Dictionaries dics)
        {
            _List = List;
            Dics = dics;
            Enumeration();
        }

        private ItemPlusImage Doit(ItemPlusImage Item)
        {

            Item.Item = new FixName(Dics).Fix(Item.Item);
            Item.Item = new TagGenerator(Dics).Generate(Item.Item);
            return Item;
        }


        private void Enumeration()
        {
            List<ItemPlusImage> List = new List<ItemPlusImage>();

            foreach (ItemPlusImage item in _List)
            {
                List.Add(Doit(item));
            }
            _List = List;
        }

        public List<ItemPlusImage> Return() => _List;
    }


}
