using System.Collections.Generic;

namespace Object_Description
{
    public interface IDictionaryPC
    {
        public int Id { get; set; }
        public string Name { get; }
        public DictionaryRelate Relate { get; }
        public List<string> Values { get; }
        public List<IDictionaryPC> Branches { get; }
        public void AddValue(string Value);
        public bool Сontain(string Value);
        public IDictionaryPC Сontained(string Value);
        public IDictionaryPC GetBranchFromName(string Value);
        public void AddBranch(IDictionaryPC Branch);

    }
}