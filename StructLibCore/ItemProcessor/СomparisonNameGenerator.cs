using Object_Description;

using Pricecona;

using StructLibs;

namespace Server.Class.ItemProcessor
{
    internal static class СomparisonNameGenerator
    {
        private static string _СomparisonName;
        public static string Get(string NameString)
        {
            _СomparisonName = null;
            foreach (char item in NameString)
            {
                if (char.IsDigit(item) || char.IsLetter(item) || item == '+')
                {
                    _СomparisonName += item;
                }
            }

            return _СomparisonName.ToLower();
        }
        public static PriceStruct GetForPriceStruct(PriceStruct item) => new PriceStruct(item, СomparisonName: Get(item.Name));
    }

    internal class TagGenerator
    {
        private readonly IDictionaryPC _Dictionary;
        public TagGenerator(IDictionaryPC Dictionary) => _Dictionary = Dictionary;
        public ItemDBStruct Generate(ItemDBStruct Item)
        {

            foreach (string item in _Dictionary.Values)
            {
                if (Item.СomparisonName.Contains(item))
                {
                    Item.СomparisonName = Item.СomparisonName.Replace(item, "");
                    Item.Tags.Add(_Dictionary.Name);
                }
            }

            return Item;
        }
    }


}
