
using Pricecona;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;

namespace StructLibs
{
    [Serializable]
    public enum FillTable
    {
        NewItemTable,
        СhangedItemsTable,
        СhangedSiteTable
    }

    [Serializable]
    public class ItemChanges
    {
        public int ItemID { get; set; }
        public string FieldName { get; set; }
        public string ItemName { get; set; }
        public object OldValue { get; set; }
        public object NewValue { get; set; }
        public string Source { get; set; }
        public DateTime DateTime { get; set; }
    }
    [System.Serializable]

    public class СomparisonNameID
    {
        public string Name { get; set; }
        public string СomparisonName { get; set; }
        public int Id { get; set; }
    }

    [System.Serializable]
    public class ItemPlusImageAndStorege
    {
        public ItemDBStruct Item { get; set; }
        public Image Image { get; set; }
        public Storage[] Storages { get; set; }
    }
    [System.Serializable]
    public class СomparisonItems : INotifyPropertyChanged
    {
        public double _NewRC;
        public int BaseId { get; set; }
        public string Name { get; set; }
        public double OldRC { get; set; }
        public double NewRC { get => _NewRC; set { _NewRC = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("СomparisonItems")); } }
        public double OldDC { get; set; }
        public double NewDC { get; set; }
        public string SourceName { get; set; }
        public string Description { get; set; }
        public int SiteId { get; set; }
        public string СomparisonName { get; set; }
        public List<string> Tags { get; set; }
        public static PropertyInfo[] GetProperties() => typeof(СomparisonItems).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        public СomparisonItems(PriceStruct NewItem, ItemDBStruct BaseItem = null)
        {
            BaseId = BaseItem?.Id ?? 0; Name = NewItem.Name ?? "";
            СomparisonName = NewItem.СomparisonName ?? "";
            Description = NewItem.Description ?? "";
            OldRC = BaseItem?.PriceRC ?? 0;
            OldDC = BaseItem?.PriceDC ?? 0;
            NewRC = NewItem.PriceRC;
            NewDC = NewItem.PriceDC;
            SourceName = NewItem.SourceName ?? "";
            Tags = new List<string>();
            SiteId = BaseItem?.SiteId ?? 0;
            if (SiteId == 0)
            {
                SiteId = NewItem.Id;
            }

            if (NewItem.Tags != null)
            {
                foreach (string item in (NewItem.Tags))
                {
                    Tags.Add(item);
                }
            }
            if (BaseItem != null && BaseItem.Tags != null)
            {
                foreach (string item in (BaseItem.Tags))
                {
                    Tags.Add(item);
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
