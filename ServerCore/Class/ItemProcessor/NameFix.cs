using Object_Description;

using Pricecona;

namespace Server.Class.ItemProcessor
{
    internal class NameFix
    {
        private readonly IDictionaryPC _Dictionary;
        public NameFix(IDictionaryPC Dictionary) => _Dictionary = Dictionary;


        public PriceStruct Fix(PriceStruct item)
        {
            item.Name = item.Name.Trim().ToUpper();
            foreach (string X in _Dictionary.Values)
            {
                item.Name = item.Name.Replace(X.ToUpper(), "");
            }
            item.СomparisonName = СomparisonNameGenerator.Get(item.Name);
            return item;
        }


    }


}
