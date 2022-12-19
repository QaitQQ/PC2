using Object_Description;

using StructLibs;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Server.Class.PriceProcessing
{
    public class PriceProcessingRules
    {
        public event Action<object> СhangeResult;
        private readonly Dictionaries _Dictionaries;
        protected CashClass Cash;
        private object result;
        private readonly Stream _Attach;
        private readonly string _Name;
        private readonly string _Subject;
        public object Result { get => result; set { result = value; СhangeResult?.Invoke(Result); } }
        public PriceProcessingRules() => _Dictionaries = Cash.Dictionaries;
        public PriceProcessingRules(Stream Attach, string Name, string Subject, CashClass Cash)
        {
            this.Cash = Cash;
            _Attach = Attach;
            _Name = Name;
            _Subject = Subject;
            _Dictionaries = Cash.Dictionaries;
        }
        public void Apply_rules()
        {

            KeyValuePair<DictionaryRelate, IDictionaryPC> Identify = RelateIdentify();

            switch (Identify.Key)
            {

                case (DictionaryRelate.Storage):

                    break;
                case (DictionaryRelate.Price):

                    Task.Factory.StartNew(
                    () =>
                    {
                        using XLS.XLS_To_Class X = new XLS.XLS_To_Class();

                        List<int> NomberList = null;

                        if (Identify.Value.Patterns.Count >0)
                        {
                            foreach (var item in Identify.Value.Patterns)
                            {
                                if (item.ToLower().Contains("list"))
                                {
                                  var nombers =  item.Split(':')[1];
                                  var nombersMass = nombers.Split(",");
                                  NomberList = new List<int>();

                                    foreach (var Y in nombersMass)
                                    {
                                        NomberList.Add(Convert.ToInt32(Y));
                                    }
                                    break;
                                }
                            }

                        }


                        Result = (List<ItemPlusImageAndStorege>)X.Read(_Attach, NomberList, _Name, Identify.Value);
                    });

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
