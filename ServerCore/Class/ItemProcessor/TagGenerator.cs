using Object_Description;

using Pricecona;

using System.Collections.Generic;

namespace Server.Class.ItemProcessor
{
    internal class TagGenerator
    {
        private readonly IDictionaryPC _Dictionary;
        public TagGenerator(IDictionaryPC Dictionary) => _Dictionary = Dictionary;
        public PriceStruct Generate(PriceStruct Item)
        {

            foreach (string item in _Dictionary.Values)
            {
                if (Item.СomparisonName.Contains(item))
                {
                    Item.СomparisonName = Item.СomparisonName.Replace(item, "");
                    if (Item.Tags == null)
                    {
                        Item.Tags = new List<string>();
                    }

                    Item.Tags.Add(_Dictionary.Name);
                }
            }

            return Item;
        }
    }


}
