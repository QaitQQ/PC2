using System;
using System.Collections.Generic;
using System.Linq;

namespace Object_Description
{
    #region Enum
    public enum DictionaryRelate
    {
        Extension,
        Universal,
        Price,
        Storage,
        Tags,
        None,
        Manufactor
    }
    public enum FillDefinition
    {
        Id,
        Sku,
        Name,
        PriceRC,
        PriceDC,
        Description,
        Currency,
        MaxRow,
        NamePattern

    }
    #endregion

    [Serializable]
    public class DictionaryBase : IDictionaryPC
    {
        public int Id { get; set; }
        public string Name { get; protected set; }
        public DictionaryRelate Relate { get; protected set; }
        public List<string> Values { get; protected set; }
        public List<IDictionaryPC> Branches { get; protected set; }
        public void AddValue(string Value) => Values.Add(Value);
        public IDictionaryPC Сontained(string Value)
        {
            if (Сontain(Value)) { return this; } else { foreach (IDictionaryPC item in Branches) { return Сontained(Value); } }
            return null;
        }
        public IDictionaryPC GetBranchFromName(string Value)
        {
            if (Name == Value) { return this; } else { foreach (IDictionaryPC item in Branches) { return GetBranchFromName(Value); } }
            return null;
        }
        public void AddBranch(IDictionaryPC Branch) => Branches.Add(Branch);

        public bool Сontain(string Value)
        {
            foreach (string item in Values) { if (Value.ToLower().Contains(item.ToLower())) { return true; } }
            return false;
        }
        public DictionaryBase() => Values = new List<string>();
        public DictionaryBase(string name, List<string> Values, DictionaryRelate Relate) { Name = name; this.Values = Values; this.Relate = Relate; }
        public DictionaryBase(string name, DictionaryRelate Relate) { Name = name; this.Relate = Relate; Values = new List<string>(); }
    }

    [Serializable]
    public class DictionaryPrice : DictionaryBase
    {
        private readonly Dictionary<FillDefinition, int> _Filling_method_coll;
        private readonly List<KeyValuePair<FillDefinition, string>> _Filling_method_string;
        public DictionaryPrice(string name, DictionaryRelate Relate) { Name = name; Values = new List<string>(); _Filling_method_coll = new Dictionary<FillDefinition, int>(); _Filling_method_string = new List<KeyValuePair<FillDefinition, string>>(); this.Relate = Relate; }
        public DictionaryPrice(string name, List<string> Values, DictionaryRelate Relate) { Name = name; this.Values = Values; _Filling_method_coll = new Dictionary<FillDefinition, int>(); _Filling_method_string = new List<KeyValuePair<FillDefinition, string>>(); this.Relate = Relate; }
        public void Set_Filling_method_coll(FillDefinition Key, int Value) => _Filling_method_coll.Add(Key, Value);
        public void Set_Filling_method_string(FillDefinition Key, string Value) => _Filling_method_string.Add(new KeyValuePair<FillDefinition, string>(Key, Value));
        public Dictionary<FillDefinition, int> Filling_method_coll() => _Filling_method_coll;
        public List<KeyValuePair<FillDefinition, string>> Filling_method_string() => _Filling_method_string;
        public List<KeyValuePair<FillDefinition, string>> GetFillingStringUnderRelate(FillDefinition Key) => _Filling_method_string.Where(x => x.Key == Key).ToList();
    }
}
