using Pricecona;

using Server.Class.ItemProcessor;

using System;
using System.Collections.Generic;
using System.Reflection;

namespace StructLibs
{
    [System.Serializable]
    public class ItemDBStruct
    {
        public int Id { get; set; }
        public int SiteId { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public double PriceRC { get; set; }
        public double PriceDC { get; set; }
        public string Description { get; set; }
        public DateTime DateСhange { get; set; }
        public string Currency { get; set; }//валюта        
        public int ManufactorID { get; set; }
        public string SourceName { get; set; }
        public string СomparisonName { get; set; }
        public int StorageID { get; set; }
        public List<Category> Categories { get; set; }
        public bool SiteFlag { get; set; }
        public string PriceListName { get; set; }
        public List<DetailValue> Details { get; set; }
        public List<string> Tags { get; set; }
        public ItemDBStruct(PriceStruct item)
        {
            Name = item.Name;
            Sku = item.Sku;

            if (item.СomparisonName == null)
            {
                СomparisonName = СomparisonNameGenerator.Get(Name);
            }
            else
            {
                СomparisonName = item.СomparisonName;
            }

            if (item.Pic != null)
            {
                try
                {
                    if (item.СomparisonName.Length < 15)
                    {

                        string Imagelink = @"pic\" + item.СomparisonName + "." + item.Pic.RawFormat;
                        item.Pic.Save(Imagelink);
                        Image = Imagelink;
                    }
                    else
                    {
                        string Imagelink = @"pic\" + item.СomparisonName.Remove(15, СomparisonName.Length - 15) + "." + item.Pic.RawFormat;
                        item.Pic.Save(Imagelink);
                        Image = Imagelink;

                    }
                }
                catch (Exception E)
                {

                }



            }

            PriceRC = item.PriceRC;
            PriceDC = item.PriceDC;
            Description = item.Description;
            DateСhange = item.DateСhange;
            Currency = item.Currency;
            SourceName = item.SourceName;
            PriceListName = item.PriceListName;
            Tags = item.Tags;
            SiteId = item.SiteId;

        }
        public ItemDBStruct()
        {
            DateСhange = DateTime.Now;
            Categories = new List<Category>();
            Details = new List<DetailValue>();
            Tags = new List<string>();
        }

        public static PropertyInfo[] GetProperties() => typeof(ItemDBStruct).GetProperties(BindingFlags.Public | BindingFlags.Instance);




    }

    [System.Serializable]
    public class DetailValue
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public int ItemID { get; set; }
        public int DetailsID { get; set; }
    }
    [System.Serializable]
    public class Manufactor
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    [System.Serializable]
    public class Details
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    [System.Serializable]
    public class Storage
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int WarehouseID { get; set; }

    }
    [System.Serializable]
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    [System.Serializable]
    public class Warehouse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PartnerID { get; set; }
    }

}


