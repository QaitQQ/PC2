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
    public enum FillDefinitionPrice
    {
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
        public DictionaryRelate Relate { get; set; }
        public List<string> Values { get; set; }
        public List<IDictionaryPC> Branches { get; set; }
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
        public List<KeyValuePair<FillDefinitionPrice, int>> Filling_method_coll;
        public List<KeyValuePair<FillDefinitionPrice, string>> Filling_method_string;
        public DictionaryPrice(string name, DictionaryRelate Relate) { Name = name; Values = new List<string>(); Filling_method_coll = new List<KeyValuePair<FillDefinitionPrice, int>>(); Filling_method_string = new List<KeyValuePair<FillDefinitionPrice, string>>(); this.Relate = Relate; }
        public DictionaryPrice(string name, List<string> Values, DictionaryRelate Relate) { Name = name; this.Values = Values; Filling_method_coll = new List<KeyValuePair<FillDefinitionPrice, int>>(); Filling_method_string = new List<KeyValuePair<FillDefinitionPrice, string>>(); this.Relate = Relate; }
        public void Set_Filling_method_coll(FillDefinitionPrice Key, int Value) => Filling_method_coll.Add(new KeyValuePair<FillDefinitionPrice, int>(Key, Value));
        public void Set_Filling_method_string(FillDefinitionPrice Key, string Value) => Filling_method_string.Add(new KeyValuePair<FillDefinitionPrice, string>(Key, Value));  
        public List<KeyValuePair<FillDefinitionPrice, string>> GetFillingStringUnderRelate(FillDefinitionPrice Key) => Filling_method_string?.Where(x => x.Key == Key).ToList();
    }
    public class DictionaryStorage : DictionaryBase
    { }
    }
