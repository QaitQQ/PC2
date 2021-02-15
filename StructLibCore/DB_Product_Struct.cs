using CRMLibs;
using Pricecona;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
        public string DescriptionSeparator { get; set; }
        public DateTime DateСhange { get; set; }
        public string Currency { get; set; }//валюта        
        public int ManufactorID { get; set; }
        public string SourceName { get; set; }
        public string[] СomparisonName { get; set; }
        public List<int> StorageID { get; set; }
        public List<Category> Categories { get; set; }
        public bool SiteFlag { get; set; }
        public string PriceListName { get; set; }
        public List<DetailValue> Details { get; set; }
        public List<string> Tags { get; set; }  
        private static string СomparisonNameGenerator(string NameString)
        {
          string  _СomparisonName = null;
            foreach (char item in NameString)
            {
                if (char.IsDigit(item) || char.IsLetter(item) || item == '+')
                {
                    _СomparisonName += item;
                }
            }

            return _СomparisonName.ToLower();
        }
        public void AddPic(Image Pic)
        {
            if (Pic != null && СomparisonName != null)
            {
                string Imagelink = null;

                if (СomparisonName[0].Length <= 15)
                {
                    Imagelink = @"pic\" + СomparisonName;                  
                }
                else
                {
                     Imagelink = @"pic\" + СomparisonName[0].Remove(15, СomparisonName.Length);
                                 
                }

                for (int i = 16; i < 50; i++)
                {
                    if (File.Exists(Imagelink+ "." + Pic.RawFormat))
                    {
                        Imagelink += i.ToString();

                    }
                }
                Imagelink = Imagelink + "." + Pic.RawFormat;
                try
                {
                    Pic.Save(Imagelink);
                    Image = Imagelink;
                }
                catch (Exception   e)
                {
                    Console.WriteLine(e.Message);
                }
             
            }
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
    public class ManufactorSite
    {
        public int Id { get; set; }
        public int ManufactorId { get; set; }
        public string SiteLink { get; set; }
        public string SearchLink { get; set; }
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
        public int ID { get; set; }
        public int ItemID { get; set; }
        public Warehouse Warehouse { get; set; }
        public int WarehouseID { get; set; }
        public DateTime DateСhange { get; set; }
        public string SourceName { get; set; }
        public int Count { get; set; }

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
        public Partner PartnerID { get; set; }
    }
    [System.Serializable]
    public class PriceСhangeHistory
    {
        public int Id { get; set; }
        public int ItemID { get; set; }
        public DateTime DateСhange { get; set; }
        public double PriceRC { get; set; }
        public double PriceDC { get; set; }
        public string SourceName { get; set; }
        public Partner PartnerID { get; set; }
    }
}


