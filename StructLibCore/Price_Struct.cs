using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace Pricecona
{
    [Serializable]
    public struct PriceStruct : IEquatable<PriceStruct>
    {
        public PriceStruct(PriceStruct item, int Id = 0, int SiteId = 0, int BaseId = 0, string Name = null, Image Pic = null, string Imagelink = null, double PriceRC = 0, double PriceDC = 0, string Description = null, string Сomment = null, string PriceListName = null, DateTime DateСhange = new DateTime(), string Currency = null, List<Detail> Details = null, string Manufactor = null, string SourceName = null, string СomparisonName = null, List<KeyValuePair<string, string>> Storage = null, string Sku = null, List<int> CategoryID = null, bool SiteFlag = false, List<string> Tags = null)
        {
            if (Id == 0) { this.Id = item.Id; } else { this.Id = Id; }
            if (SiteId == 0) { this.SiteId = item.SiteId; } else { this.SiteId = SiteId; }
            if (BaseId == 0) { this.BaseId = item.BaseId; } else { this.BaseId = BaseId; }
            if (СomparisonName == null) { this.СomparisonName = item.Name; } else { this.СomparisonName = СomparisonName; }
            if (Name == null) { this.Name = item.Name; } else { this.Name = Name; }
            if (PriceRC == 0) { this.PriceRC = item.PriceRC; } else { this.PriceRC = PriceRC; }
            if (PriceDC == 0) { this.PriceDC = item.PriceDC; } else { this.PriceDC = PriceDC; }
            if (Description == null) { this.Description = item.Description; } else { this.Description = Description; }
            if (DateСhange == new DateTime()) { this.DateСhange = item.DateСhange; } else { this.DateСhange = DateСhange; }
            if (Pic == null) { this.Pic = null; } else { this.Pic = Pic; }// передовать обязательно если нужна
            if (PriceListName == null) { this.PriceListName = item.PriceListName; } else { this.PriceListName = PriceListName; }
            if (Imagelink == null) { this.Imagelink = item.Imagelink; } else { this.Imagelink = Imagelink; }
            if (Currency == null) { this.Currency = item.Currency; } else { this.Currency = Currency; }
            if (Details == null) { this.Details = item.Details; } else { this.Details = Details; }
            if (Manufactor == null) { this.Manufactor = item.Manufactor; } else { this.Manufactor = Manufactor; }
            if (Сomment == null) { this.Сomment = item.Сomment; } else { this.Сomment = Сomment; }
            if (SourceName == null) { this.SourceName = item.SourceName; } else { this.SourceName = SourceName; }
            if (Storage == null) { this.Storage = item.Storage; } else { this.Storage = Storage; }
            if (Sku == null) { this.Sku = item.Sku; } else { this.Sku = Sku; }
            if (CategoryID == null || CategoryID.Count == 0) { this.CategoryID = item.CategoryID; }
            if (Tags == null || Tags.Count == 0) { this.Tags = item.Tags; } else { this.Tags = Tags; }

            {

                List<int> IDList = new List<int>();
                if (item.CategoryID != null)
                {
                    foreach (int ID in item.CategoryID)
                    {
                        IDList.Add(ID);
                    }
                }

                foreach (int ID in CategoryID)
                {
                    IDList.Add(ID);
                }



                this.CategoryID = IDList;

            }
            if (SiteFlag == false) { this.SiteFlag = item.SiteFlag; } else { this.SiteFlag = SiteFlag; }
        }

        public int Id { get; set; }
        public int SiteId { get; set; }
        public int BaseId { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public Image Pic { get; set; }
        public string Imagelink { get; set; }
        public double PriceRC { get; set; }
        public double PriceDC { get; set; }
        public string Description { get; set; }
        public string Сomment { get; set; }
        public string PriceListName { get; set; }
        public DateTime DateСhange { get; set; }
        public string Currency { get; set; }
        public List<Detail> Details { get; set; }
        public string Manufactor { get; set; }
        public string SourceName { get; set; }
        public string СomparisonName { get; set; }
        public List<KeyValuePair<string, string>> Storage { get; set; }
        public List<int> CategoryID { get; set; }
        public bool SiteFlag { get; set; }
        public List<string> Tags { get; set; }

        public void AddCategoryID(int Id) { if (CategoryID == null) { CategoryID = new List<int>(); } CategoryID.Add(Id); }
        public void SetManufactor(string Manufactor) => this.Manufactor = Manufactor;
        public bool Equals(PriceStruct other)
        {
            if (Name == null)
            {
                return false;
            }
            else
            {
                return Name.Equals(other.Name);
            }
        }
        public override int GetHashCode() => Name.GetHashCode();

        public string SaveImage()
        {
            string Link = null;
            if (Pic != null)
            {

                Link = @"pic\" + СomparisonName + "." + Pic.RawFormat;
                Pic.Save(Link);

            }
            return Link;
        }
    }

    [Serializable]
    public struct Detail
    {
        public Detail(string Name, int id = 0, OptDic Dic = null)
        {

            this.Name = Name;
            this.id = id;
            if (Dic != null && id == 0)
            {
                foreach (string item in Dic.AllKeys)
                {

                    if (item.ToUpper() == Name.ToUpper())
                    {
                        this.id = Convert.ToInt32(Dic.Get(item));
                    }
                }
            }


            DetailValues = new List<string>();
        }
        public Detail(string Name, string Value, int id = 0, OptDic Dic = null)
        {

            this.Name = Name;
            this.id = id;
            DetailValues = new List<string> { Value };
            if (Dic != null && id == 0)
            {
                foreach (string item in Dic.AllKeys)
                {

                    if (item.ToUpper() == Name.ToUpper())
                    {
                        this.id = Convert.ToInt32(Dic.Get(item));
                    }
                }
            }
        }
        public void Add(string Value) => ComparerAdd(Value);
        public string GetName() => Name;
        public int GetID() => id;
        public string GetDetailValues()
        {
            string Str = null;
            foreach (string item in DetailValues)
            {
                Str = Str + item + " ";
            }
            return Str;
        }
        public List<string> GetList() => DetailValues;
        private void ComparerAdd(string Value)
        {

            List<string> G = DetailValues.FindAll(item => item.Contains(Value));

            if (G.Count == 0)
            {
                DetailValues.Add(Value);
            }


        }
        private readonly string Name;
        private readonly int id;
        private readonly List<string> DetailValues;
    }
    [Serializable]
    public class WordTable : Option, IEnumerable
    {
        private List<int> LinkedID;
        private int Id;
        private string Name;
        private string AliasName;
        private int Parent_id;

        private List<string> Values;
        private List<WordTable> Children;

        public void ClearLinkedID()
        {
            LinkedID = new List<int>();
            foreach (WordTable item in Children)
            {
                item.ClearLinkedID();
            }
        }

        public WordTable() { Children = new List<WordTable>(); LinkedID = new List<int>(); Values = new List<string>(); }
        public WordTable(int Id, string Name)
        {
            this.Id = Id;
            this.Name = Name;
            Children = new List<WordTable>(); LinkedID = new List<int>(); Values = new List<string>();
        }
        public WordTable(int Id, string Name, int Parent_id)
        {
            this.Id = Id;
            this.Name = Name;
            this.Parent_id = Parent_id;
            Children = new List<WordTable>(); LinkedID = new List<int>(); Values = new List<string>();
        }

        private void SetParent_id()
        {
            foreach (WordTable item in Children)
            {
                item.Parent_id = Id;
                item.SetParent_id();
            }
        }
        public string GetName_ID() => Name + "_" + Id;
        private void ClearParent_id()
        {
            foreach (WordTable item in Children)
            {
                item.Parent_id = 0;
                item.ClearParent_id();
            }
        }
        public int GetParent_id() => Parent_id;
        public void SetID(int Id) => this.Id = Id;
        public int GetID() => Id;
        public void SetName(string Name) => this.Name = Name;
        public string GetName() => Name;
        public void SetAliasName(string Alias) => AliasName = Alias;
        public string[] GetChildAlias(string Name)
        {
            List<string> ChildList = new List<string>();
            foreach (WordTable item in Children)
            {
                if (item.AliasName != null)
                {
                    ChildList.Add(item.AliasName);
                }
                else
                {
                    ChildList.Add(item.Name);
                }
            }
            return ChildList.ToArray();
        }
        public int CountChildren() => Children.Count;
        public int[] GetChildrenID()
        {
            List<int> Ids = new List<int>();
            foreach (WordTable item in Children)
            {
                Ids.Add(item.Id);
            }
            return Ids.ToArray();
        }
        public void SetChildrenLinkedID()
        {


            if (Id != 0 && !LinkedID.Contains(Id))
            {
                LinkedID.Add(Id);
            }
            if (Children.Count > 0)
            {
                foreach (WordTable item in Children)
                {

                    foreach (int linkedID in LinkedID)
                    {
                        if (!item.LinkedID.Contains(linkedID))
                        {
                            item.AddLinkedID(linkedID);
                        }

                    }

                    item.SetChildrenLinkedID();

                }
            }


        }
        public List<WordTable> GetAllScions()

        {
            List<WordTable> Scions = new List<WordTable>();

            if (CountChildren() > 0)
            {
                foreach (WordTable item in Children)
                {
                    Scions.Add(item);
                    foreach (WordTable Scion in item.GetAllScions())
                    {
                        Scions.Add(Scion);
                    }

                }

            }

            return Scions;

        }
        public WordTable GetOneScion(string Name)
        {


            List<WordTable> Scions = new List<WordTable>();

            if (CountChildren() > 0)
            {
                foreach (WordTable item in Children)
                {
                    Scions.Add(item);
                    foreach (WordTable Scion in item.GetAllScions())
                    {
                        Scions.Add(Scion);
                    }

                }

            }

            return Scions.Find(item => item.GetName() == Name);


        }
        public WordTable GetOneScionNameId(string NameId) => GetAllScions().Find(item => item.GetName_ID() == NameId);
        public WordTable GetOneScion(int Id)
        {

            List<WordTable> Scions = new List<WordTable>();

            if (CountChildren() > 0)
            {
                foreach (WordTable item in Children)
                {
                    Scions.Add(item);
                    foreach (WordTable Scion in item.GetAllScions())
                    {
                        Scions.Add(Scion);
                    }

                }

            }

            return Scions.Find(item => item.GetID() == Id);



        }
        public int[] GetAllScionsID()
        {
            List<int> Result = new List<int>();

            foreach (WordTable item in GetAllScions())
            {
                Result.Add(item.GetID());
            }


            return Result.ToArray();
        }
        public void AddLinkedID(int Id) => LinkedID.Add(Id);
        public void DelLinkedID(int Id) => LinkedID = LinkedID.FindAll(item => item != Id);
        public int[] GetLinkedID()
        {
            List<int> ChildList = new List<int>();

            foreach (int item in LinkedID)
            {

                ChildList.Add(item);
            }
            return ChildList.ToArray();
        }
        public WordTable GetChild(string ChildName)

        {
            foreach (WordTable item in Children)
            {
                if (item.Name == ChildName)
                {
                    return item;
                }
            }
            return null;
        }
        public WordTable GetChild(int index) => Children[index];
        public TreeNode[] GetChildrenNodes()
        {

            TreeNode[] Node = new TreeNode[Children.Count];

            for (int i = 0; i < Node.Length; i++)
            {
                if (GetChild(i).CountChildren() > 0)
                {
                    Node[i] = new TreeNode(GetChild(i).GetName(), GetChild(i).GetChildrenNodes())
                    {
                        Name = GetChild(i).GetName_ID()
                    };
                }
                else
                {
                    Node[i] = new TreeNode(GetChild(i).GetName())
                    {
                        Name = GetChild(i).GetName_ID()
                    };
                }
            }


            return Node;



        }
        public string[] GetChildrenName()
        {
            List<string> ChildList = new List<string>();
            foreach (WordTable item in Children)
            {
                ChildList.Add(item.Name);
            }
            return ChildList.ToArray();
        }
        public void AddChild(WordTable Child) => Children.Add(Child);
        public List<string> GetValues() => Values;
        public void AddValue(string Value) => Values.Add(Value);
        public void DelValue(string Value) => Values = Values.FindAll(item => item != Value);
        public void ClearValues() => Values.Clear();
        public new IEnumerator GetEnumerator() => ((IEnumerable)Name).GetEnumerator();
        public void Serialization(string FilePath)
        {
            if (File.Exists(FilePath) == false)
            {
                Stream SaveFileStream = File.Create(FilePath);
                BinaryFormatter serializer = new BinaryFormatter();
                serializer.Serialize(SaveFileStream, this);
                SaveFileStream.Close();
            }
            else
            {
                Stream SaveFileStream = File.OpenWrite(FilePath);
                BinaryFormatter serializer = new BinaryFormatter();
                serializer.Serialize(SaveFileStream, this);
                SaveFileStream.Close();
            }

        }
        public void DelScion(string Name)
        {
            Children = Children.FindAll(item => item.GetName() != Name);

            foreach (WordTable item in Children)
            {
                item.DelScion(Name);
            }

        }
        public void TransportScion(string Who, string Where)
        {

            WordTable who = GetOneScionNameId(Who);

            DelScion(Who);
            GetOneScionNameId(Where).AddChild(who);
            SetParent_id();
        }



    }
    [Serializable]
    public class Option : IEnumerable
    {
        //private readonly int Id;
        //private readonly string Name;
        //private readonly string AliasName;
        //private readonly int Parent_id;
        //private readonly List<string> Values;
        //private readonly List<WordTable> Children;

        public IEnumerator GetEnumerator() => null;//((IEnumerable)Name).GetEnumerator();
    }

}