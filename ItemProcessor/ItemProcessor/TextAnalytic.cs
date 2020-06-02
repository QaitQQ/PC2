



using Object_Description;

using StructLibs;

using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Server.Class.ItemProcessor
{

    public abstract class TextAnalytic
    {
        internal Dictionaries Dictionaries;
        public TextAnalytic(Dictionaries Dictionaries) { this.Dictionaries = Dictionaries; }
        public virtual ItemDBStruct Search(ItemDBStruct Item)
        {
            return Item;
        }
    }
    public class SerchManufactor : TextAnalytic
    {
        public SerchManufactor(Dictionaries Dictionaries) : base(Dictionaries)
        {
        }

        public override ItemDBStruct Search(ItemDBStruct Item)
        {
            IEnumerable<IDictionaryPC> ActiveDictionaries;
            ActiveDictionaries = Dictionaries.GetDictionaryRelate(DictionaryRelate.Manufactor);

            foreach (IDictionaryPC item in ActiveDictionaries)
            {
                foreach (string X in item.Values)
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

    internal class TagGenerator : TextAnalytic
    {
        public TagGenerator(Dictionaries Dictionaries) : base(Dictionaries)
        {
        }

        public ItemDBStruct Generate(ItemDBStruct Item)
        {
            IEnumerable<IDictionaryPC> ActiveDictionaries;
            ActiveDictionaries = Dictionaries.GetDictionaryRelate(DictionaryRelate.Tags);

            foreach (IDictionaryPC item in ActiveDictionaries)
            {
                DicWork(ref Item, item);
            }
            return Item;
        }
        private static void DicWork(ref ItemDBStruct Item, IDictionaryPC Dictionary)
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
    public class FixName : TextAnalytic
    {
        public FixName(Dictionaries Dictionaries) : base(Dictionaries)
        {
        }

        public ItemDBStruct Fix(ItemDBStruct Item)
        {

            Item.Name = Item.Name.Trim().ToUpper();
            DicWork(Item, Dictionaries.Get("NameEdit"));
            Item.СomparisonName = СomparisonNameGenerator.Get(Item.Name);
            return Item;
        }
        private static void DicWork(ItemDBStruct item, IDictionaryPC Dictionary)
        {
            foreach (string X in Dictionary.Values)
            {
                item.Name = item.Name.Replace(X.ToUpper(), "");
            }
        }
    }












}