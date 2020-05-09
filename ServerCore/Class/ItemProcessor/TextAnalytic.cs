



using Object_Description;

using Pricecona;

using StructLibs;

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Server.Class.ItemProcessor
{

    public abstract class TextAnalytic
    {
        internal IEnumerable<IDictionaryPC> _Dictionaries;
        public TextAnalytic() { }
        public virtual ItemDBStruct Search(ItemDBStruct Item)
        {
            return Item;
        }
    }

    public class SerchManufactor : TextAnalytic
    {
        public SerchManufactor() { _Dictionaries = Program.Cash.Dictionaries.GetDictionaryRelate(DictionaryRelate.Manufactor); }
        public override ItemDBStruct Search(ItemDBStruct Item)
        {

            foreach (var item in _Dictionaries)
            {
                foreach (var X in item.Values)
                {
                    if (Regex.IsMatch(Item.Name, X))
                    {
                        Item.СomparisonName = Item.СomparisonName.Replace(X, "");

                        Item.ManufactorID = item.Id;
                        return Item;

                    }
                }

            }

            return Item;

        }
    }

    internal class TagGenerator: TextAnalytic
    {
        public TagGenerator() => _Dictionaries  = Program.Cash.Dictionaries.GetDictionaryRelate(DictionaryRelate.Tags);
        public PriceStruct Generate(PriceStruct Item)
        {
            foreach (var item in _Dictionaries)
            {
                DicWork(ref Item, item);
            }        
            return Item;
        }
        private static void DicWork( ref PriceStruct Item, IDictionaryPC Dictionary)
        {
            foreach (string item in Dictionary.Values)
            {
                if (Item.СomparisonName.Contains(item))
                {
                    Item.СomparisonName = Item.СomparisonName.Replace(item, "");
                    if (Item.Tags == null)
                    {
                        Item.Tags = new List<string>();
                    }

                    Item.Tags.Add(Dictionary.Name);
                }
            }
        }
    }
    internal class FixName : TextAnalytic
    {
        public FixName() { _Dictionaries = new List<IDictionaryPC>(); _Dictionaries.ToList().Add(Program.Cash.Dictionaries.Get("NameEdit")); }
        public PriceStruct Fix(PriceStruct Item)
        {
            Item.Name = Item.Name.Trim().ToUpper();
            foreach (var item in _Dictionaries)
            {
                DicWork(Item, item);
            }

            Item.СomparisonName = СomparisonNameGenerator.Get(Item.Name);
            return Item;
        }
        private static void DicWork(PriceStruct item, IDictionaryPC Dictionary)
        {
            foreach (string X in Dictionary.Values)
            {
                item.Name = item.Name.Replace(X.ToUpper(), "");
            }
        }
    }












}