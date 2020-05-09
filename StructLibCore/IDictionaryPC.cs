using System.Collections.Generic;

namespace Object_Description
{
    public interface IDictionaryPC
    {
        public int Id { get; set; }
        public string Name { get; }
        public DictionaryRelate Relate { get; set; }
        public List<string> Values { get; set; }
        public List<IDictionaryPC> Branches { get; }
        public bool Сontain(string Value);
        public IDictionaryPC Сontained(string Value);
        public IDictionaryPC GetBranchFromName(string Value);
        public void AddBranch(IDictionaryPC Branch);

    }
}