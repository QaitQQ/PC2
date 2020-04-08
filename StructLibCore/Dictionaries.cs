using System;
using System.Collections.Generic;
using System.Linq;

namespace Object_Description
{
    [Serializable]
    public class Dictionaries
    {
        private List<IDictionaryPC> _Dictionaries;
        public List<IDictionaryPC> RetunUnderRelate(DictionaryRelate Relate) => _Dictionaries.FindAll(x => x.Relate == Relate);
        public Dictionaries() => _Dictionaries = new List<IDictionaryPC>();
        public void Add(IDictionaryPC Dictionary) => _Dictionaries.Add(Dictionary);
        public void Renew(IDictionaryPC Dictionary) { _Dictionaries = _Dictionaries.FindAll(x => x.Name != Dictionary.Name); _Dictionaries.Add(Dictionary); }
        public List<IDictionaryPC> GetAll() => _Dictionaries;
        public IDictionaryPC Get(string name) { IEnumerable<IDictionaryPC> Result = _Dictionaries.Where(x => x.Name == name); return Result.First(); }
        public bool Contains(string Name)
        {
            var X = _Dictionaries.FindAll(x => x.Name == Name);


            if (X.Count > 0)
            {
                return true;
            }
            return false;
        }
        public void Del(IDictionaryPC Dictionary) => _Dictionaries.Remove(Dictionary);
        public IEnumerable<IDictionaryPC> GetDictionaryRelate(DictionaryRelate Relate) { IEnumerable<IDictionaryPC> Result = _Dictionaries.Where(x => x.Relate == Relate); return Result; }
    }


}
