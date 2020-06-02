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

                    for (int i = 1; i < mass.Length - 1; i++) { F = F.FindAll(x => X.GetValue(x).ToString().Contains(mass[i])); }

                    foreach (var item in F) { List.Add(new СomparisonNameID() { Name = item.Name, СomparisonName = item.СomparisonName, Id = item.Id }); }

                }
                else
                {
                    var F = FindWithParameters(Str, X, Db);
                    if (F != null)
                    {
                        foreach (var item in F)
                        {
                            List.Add(new СomparisonNameID() { Name = item.Name, СomparisonName = item.СomparisonName, Id = item.Id });
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
            Image newImage = null;

            if (Result.Image != null || Result.Image != "")
            {
                newImage = ImageResize(Result, newImage);
            }


            ItemPlusImage itemNetStruct = new ItemPlusImage() { Item = Result, Image = newImage };

            Message.Obj = itemNetStruct;

            return Message;

        }

        private static Image ImageResize(ItemDBStruct Item, Image newImage)
        {
            if (File.Exists(Item.Image))
            {
                newImage = Image.FromFile(Item.Image);

                if (newImage.Width > 799)
                {
                    newImage = ResizeImage(newImage, 800, 800);
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

            ItemDBStruct item = (ItemDBStruct)((ItemPlusImage)Attach).Item;
            Db.Update(item);
            Db.SaveChanges();

            Message.Obj = true;
            return Message;
        }

    }
}


namespace Network.Item.Changes
{
    [Serializable]
    public class GetChanges : NetItem
    {      
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            Message.Obj = ((CashClass)Obj).СhangedItems;



            return Message;
        }
    }

    [Serializable]
    public class GetNewList : NetItem
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            var List = ((CashClass)Obj).NewItem;
            List<ItemChanges> Result = new List<ItemChanges>();

            foreach (var item in List)
            {
                Result.Add(new ItemChanges()
                {
                    ItemName = item.Item.Name,
                    NewValue = item.Item.PriceRC,
                    Source = item.Item.SourceName
                }
                );
            }


            Message.Obj = Result;

            return Message;
        }
    }
}


