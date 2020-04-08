using Object_Description;

using System.Collections.Generic;
using System.IO;

namespace Server.Class.Net
{
    internal class Imap_Rules
    {
        private readonly Dictionaries _Dictionaries;
        private readonly Stream _Attach;
        private readonly string _Name;
        private readonly string _Subject;



        public Imap_Rules() => _Dictionaries = Program.Cash.Dictionaries;

        public Imap_Rules(Stream Attach, string Name, string Subject)
        {
            _Attach = Attach;
            _Name = Name;
            _Subject = Subject;
            _Dictionaries = Program.Cash.Dictionaries;

        }
        public bool Search_Extension(string Value)
        {
            IDictionaryPC Extension_list = _Dictionaries.Get("xls");
            return Extension_list.Сontain(Value);
        }

        public void Apply_rules()
        {

            KeyValuePair<DictionaryRelate, IDictionaryPC> Identify = RelateIdentify();

            switch (Identify.Key)
            {

                case (DictionaryRelate.Storage):
                    new Imap_Handler_Storege(Identify.Value).Process(_Attach, _Name);
                    break;
                case (DictionaryRelate.Price):
                    new Imap_Handler_Price(Identify.Value).Process(_Attach, _Name);
                    break;

                default:
                    break;
            }
        }


        private KeyValuePair<DictionaryRelate, IDictionaryPC> RelateIdentify()
        {
            foreach (IDictionaryPC item in _Dictionaries.GetAll())
            {
                if (item.Relate == DictionaryRelate.Price || item.Relate == DictionaryRelate.Storage)
                {
                    if (item.Сontain(_Name) || item.Сontain(_Subject))
                    {
                        return new KeyValuePair<DictionaryRelate, IDictionaryPC>(item.Relate, item);
                    }
                }
            }
            return new KeyValuePair<DictionaryRelate, IDictionaryPC>();
        }
    }
}
