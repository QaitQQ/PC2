



using Object_Description;

using StructLibs;

using System.Collections.Generic;
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
                        Item.ManufactorID = item.Id;
                        return Item;

                    }
                }

            }

            return Item;

        }















    }















}