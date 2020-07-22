using Server;
using Server.Class.ItemProcessor;

using StructLibs;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Network.Item
{
    [Serializable]
    public abstract class NetItem : NetQwerry { }
    [Serializable]
    public class ItemSearch : NetItem
    {
        private PropertyInfo[] Prop { get; set; }
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            Prop = ItemDBStruct.GetProperties();
            var List = new List<СomparisonNameID>();
            object[] DataMass = (object[])this.Attach;
            PropertyInfo X = Prop.ElementAt((int)DataMass[1]);
            string Str = DataMass[0].ToString();

            if (X.Name == "Name")
            {
                List = ItemsNameFromCash(Str, ((CashClass)Obj).ItemName);
            }
            else
            {
                if (Str.Contains(@"&"))
                {
                    var mass = Str.Split('&');

                    List<ItemDBStruct> F = FindWithParameters(mass[0], X, Db);

                    for (int i = 1; i < mass.Length ; i++) { F = F.FindAll(x => X.GetValue(x).ToString().Contains(mass[i])); }

                    foreach (var item in F) { List.Add(new СomparisonNameID() { Name = item.Name, СomparisonName = item.СomparisonName[0], Id = item.Id }); }

                }
                else
                {
                    var F = FindWithParameters(Str, X, Db);
                    if (F != null)
                    {
                        foreach (var item in F)
                        {
                            List.Add(new СomparisonNameID() { Name = item.Name, СomparisonName = item.СomparisonName[0], Id = item.Id });
                        }
                    }
                }


            }

            Message.Obj = List;

            return Message;
        }
        private List<СomparisonNameID> ItemsNameFromCash(string Name, List<СomparisonNameID> CashItemName)
        {
            List<СomparisonNameID> Xlist;
            Name = СomparisonNameGenerator.Get(Name);

            Xlist = (from Items in CashItemName where Items.СomparisonName.ToUpper().Contains(Name.ToUpper()) select Items).ToList();

            for (int i = 0; i < 10; i++)
            {
                if (Xlist.Count == 0)
                {
                    Name = Name.Remove(Name.Length - 1);
                    Name = Name.Remove(0, 1);
                    Xlist = (from Items in CashItemName where Items.СomparisonName.ToUpper().Contains(Name.ToUpper()) select Items).ToList();
                }
                else { break; }

            }
            if (Xlist.Count == 0) { Xlist.Add(new СomparisonNameID() { Name = "not found" }); }

            return Xlist;

        }
        public List<ItemDBStruct> FindWithParameters(string Str, PropertyInfo Field, ApplicationContext Db)
        {
            List<ItemDBStruct> QweryResult = null;

            if (Field.PropertyType.Name.Contains("List"))
            {
                QweryResult = Db.Item.ToList();
                QweryResult = QweryResult.FindAll(item => (Field.GetValue(item) as List<string>) != null && (Field.GetValue(item) as List<string>).Contains(Str));
            }
            else
            {
                QweryResult = Db.Item.ToList();
                QweryResult = QweryResult.FindAll(item => Field.GetValue(item).ToString().ToUpper().Contains(Str.ToUpper()));
            }


            return QweryResult;
        }
    }
    [Serializable]
    public class GetItemFromId : NetQwerry
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            int ID = Convert.ToInt32(this.Attach);
            ItemDBStruct Result = Db.Item.First(item => item.Id == ID);
            Storage[] Storege = null;

            if (Result.StorageID !=null && Result.StorageID.Count>0)
            {
               Storege = (from X in Db.Storage where X.ItemID == ID select X).ToArray();

                var Warehouses = Db.Warehouse.ToList();

                foreach (var item in Storege)
                {
                    item.Warehouse = Warehouses.First(x => x.Id == item.WarehouseID);
                }
            }
          

            Image newImage = null;

            if (Result.Image != null || Result.Image != "")
            {
                newImage = ImageResize(Result, newImage);
            }



            ItemPlusImageAndStorege itemNetStruct = new ItemPlusImageAndStorege() { Item = Result, Image = newImage , Storages = Storege };

            Message.Obj = itemNetStruct;

            return Message;

        }
        private static Image ImageResize(ItemDBStruct Item, Image newImage)
        {
            if (File.Exists(Item.Image))
            {
                 Image Img = Image.FromFile(Item.Image);

                if (Img.Width > 799)
                {
                    newImage = ResizeImage(Img, 800, 800);
                }
                else
                {
                    newImage = Img;
                }
            }
            return newImage;
        }
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            Rectangle destRect = new Rectangle(0, 0, width, height);
            Bitmap destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (Graphics graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (ImageAttributes wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
    [Serializable]
    public class DelItemFromId : NetQwerry
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            int ID = (int)Attach;
            ItemDBStruct item = Db.Item.First(item => item.Id == ID);
            Db.Remove(item);
            Db.SaveChanges();
            ((List<СomparisonNameID>)Obj).RemoveAll(x => x.Id == ID);
            Message.Obj = true;
            return Message;

        }
    }
    [Serializable]
    public class EditItem : NetItem
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            ItemDBStruct item = (ItemDBStruct)Attach;
            Db.Update(item);
            Db.SaveChanges();
            Message.Obj = true;
            return Message;
        }

    }
    [Serializable]
    public class RemoveDuplicate : NetItem
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            var Items = Db.Item.ToList();

            var List = new List<string>();

            foreach (var item in Items)
            {
                var FindItems = Items.FindAll(x => x.СomparisonName == item.СomparisonName);
                if (FindItems.Count > 1)
                { CharComparer(ref Db, List, FindItems); }

                FindItems = Items.FindAll(x => x.СomparisonName.Intersect(item.СomparisonName).Any());
                if (FindItems.Count > 1)  { CharComparer(ref Db, List, FindItems);}

                FindItems = Items.FindAll(x => x.СomparisonName.Intersect(item.СomparisonName).Any());
                if (FindItems.Count > 1)   {   CharComparer(ref Db, List, FindItems);}

            }
            var str = Db.Storage.ToList();

            foreach (var item in str)
            {

                var FindItems = Items.FindAll(x => x.Id == item.ItemID);

                if (FindItems.Count == 0)
                {
                    Db.Remove(item);
                }

            }

            Db.SaveChanges();

            Message.Obj = List;

            return Message;


        }

        private static void CharComparer(ref ApplicationContext Db, List<string> List, List<ItemDBStruct> FindItems)
        {
            int X = 0;
            X = FindItems[0].СomparisonName.Length;
            if (FindItems[0].СomparisonName.Length == FindItems[1].СomparisonName.Length)
            {
                for (int i = 0; i < FindItems[0].СomparisonName.Length; i++)
                {
                    if (FindItems[0].СomparisonName[i] == FindItems[1].СomparisonName[i])
                    {
                        X--;
                    }
                }

                if (X == 0)
                {
                    List.Add(FindItems[1].Name + FindItems[1].Id);
                    Db.Remove(FindItems[1]);
                }
            }

           
        }
    }
    [Serializable]
    public class AddNewItemFromList : NetItem
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            string Name = (string)Attach;

            List<ItemPlusImageAndStorege> List = ((CashClass)Obj).NewItem;

            ItemPlusImageAndStorege itemPlusImageAndStorege = List.First(x => x.Item.Name == Name);

            var Item = itemPlusImageAndStorege.Item;

            Item.AddPic(itemPlusImageAndStorege.Image);

            Db.Item.Add(Item);

            Db.SaveChanges();

            Message.Obj = true;
            return Message;
        }

    }
    [Serializable]
    public class RenewNameCash : NetItem
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
          var X =  ((CashClass)Obj);
            X.LoadCash();

            Message.Obj = true;
            return Message;
        }

    }
    /// <summary>
    /// на входе должен быть массив из 2х интов
    /// на выходе bool
    /// </summary>
     [Serializable]
    public class MergeItem : NetItem
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            int[] IDs = (int[])Attach;


            ItemDBStruct ActualItem = Db.Item.First(item => item.Id == IDs[0]);
            ItemDBStruct DublelItem = Db.Item.First(item => item.Id == IDs[1]);

            List<string> Cnames = new List<string>();


            foreach (var item in ActualItem.СomparisonName)
            {
                Cnames.Add(item);
            }
            foreach (var item in DublelItem.СomparisonName)
            {
                Cnames.Add(item);
            }

            ActualItem.СomparisonName = Cnames.ToArray();

            Db.Update(ActualItem);
            Db.Remove(DublelItem);
            Db.SaveChanges();


            ((CashClass)Obj).ItemName.RemoveAll(X => X.Id == IDs[1]);

            Message.Obj = true;
            return Message;
        }

    }

}



