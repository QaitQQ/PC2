using Object_Description;

using Pricecona;

using System.Collections.Generic;

namespace Server.Class.ItemProcessor
{
    internal class СhangedItemsTags
    {
        private List<PriceStruct> _List;
        private readonly IEnumerable<IDictionaryPC> Dics;
        public СhangedItemsTags(List<PriceStruct> List)
        {
            _List = List;
            Dics = Program.Cash.Dictionaries.GetDictionaryRelate(DictionaryRelate.Tags);
            Enumeration();
        }

        private PriceStruct Doit(PriceStruct Item)
        {

            foreach (IDictionaryPC item in Dics)
            {
                Item = new TagGenerator(item).Generate(Item);

                Item = new NameFix(Program.Cash.Dictionaries.Get("NameEdit")).Fix(Item);

            }

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
