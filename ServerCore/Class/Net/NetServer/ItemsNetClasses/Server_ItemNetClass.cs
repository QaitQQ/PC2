
using Pricecona;

using Server.Class.ItemProcessor;
using Server.Class.Query;

using StructLibs;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;

using static NetEnum.Selector;
using static NetEnum.Selector.SecondSelector;

namespace Server.Class.Net.NetServer
{
    internal class Server_ItemNetClass : AbstractNetClass
    {
        private readonly PropertyInfo[] Prop;
        public Server_ItemNetClass(TCP_CS_Obj Data)
        {
            this.Data = Data;
            Prop = ItemDBStruct.GetProperties();
            if (Program.Cash.ItemName == null) { Program.Cash.ReloadNameCash(); }
            switch (this.Data.Code[1])
            {
                case EnumItem.ItemsSearch:
                    ItemsSearch();
                    break;
                case EnumItem.GetPositionFromDB:
                    GetPositionFromDB();
                    break;
                case EnumItem.EditPositionFromDB:
                    break;
                case EnumItem.WithMailDB_Selektor:
                    this.Data = new Server_MailItemsDB(this.Data).Result();
                    break;
                case EnumItem.ApiSiteItem_Selektor:
                    this.Data = new Server_NetApiSiteClass(this.Data).Result();
                    break;
                case EnumItem.Del_From_DB:
                    Del_From_DB(Data);
                    break;
                case EnumItem.Add_New_position_from_СhangeList:
                    Add_New_position_from_СhangeList();
                    break;
                case EnumItem.AllowAllСhange:
                    AllowAllСhange();
                    break;
                case EnumItem.DelDouble:
                    new ItemsQuery().DoubleDelete();
                    Program.Cash.ReloadNameCash();
                    break;

                default:
                    break;
            }
        }
        private void ItemsSearch()
        {
            object[] DataMass = (object[])this.Data.Obj;
            PropertyInfo X = Prop.ElementAt((int)DataMass[1]);
            string Str = DataMass[0].ToString();
            if (X.Name == "Name")
            {
                ItemsNameFromCash(Str);
            }
            else
            {
                var List = new List<KeyValuePair<string, int>>();

                if (Str.Contains(@"&"))
                {
                    var mass = Str.Split('&');

                    List<ItemDBStruct> F = new Query.ItemsQuery().FindWithParameters(mass[0], X);

                    for (int i = 1; i < mass.Length - 1; i++)
                    {
                        F = F.FindAll(x => X.GetValue(x).ToString().Contains(mass[i]));
                    }

                    foreach (var item in F)
                    {
                        List.Add(new KeyValuePair<string, int>(item.Name, item.Id));
                    }

                }
                else
                {

                    var F = new Query.ItemsQuery().FindWithParameters(Str, X);
                    foreach (var item in F)
                    {
                        List.Add(new KeyValuePair<string, int>(item.Name, item.Id));
                    }
                }
                this.Data.Obj = List;
            }
        }
        private void AllowAllСhange()
        {
            FillTable FillTable = (FillTable)this.Data.Obj;
            switch (FillTable)
            {
                case FillTable.СhangedItemsTable:
                    List<KeyValuePair<PriceStruct, ItemDBStruct>> СhangedItems = Program.Cash.СhangedItems.FindAll(x => x.Value != null);
                    List<ItemDBStruct> СhangedItemsList = new List<ItemDBStruct>();
                    foreach (var item in СhangedItems)
                    {
                        ItemDBStruct newItem = item.Value;
                        newItem.PriceRC = item.Key.PriceRC;
                        newItem.PriceDC = item.Key.PriceDC;
                        newItem.Description = item.Key.Description;
                        newItem.DateСhange = item.Key.DateСhange;
                        newItem.SourceName = item.Key.SourceName;
                        СhangedItemsList.Add(newItem);
                    }
                    new Query.ItemsQuery().EditList(СhangedItemsList);
                    Program.Cash.ItemName = new Class.Query.NameCash().GetCashItemName();
                    Program.Cash.СhangedItems = Program.Cash.СhangedItems.FindAll(x => x.Value == null);

                    break;
                case FillTable.NewItemTable:

                    List<KeyValuePair<PriceStruct, ItemDBStruct>> Items = Program.Cash.СhangedItems.FindAll(x => x.Value == null);

                    List<PriceStruct> NewList = new List<PriceStruct>();
                    foreach (var item in Items)
                    {
                        NewList.Add(item.Key);
                    }
                    new Query.ItemsQuery().AddListPriceStruct(NewList);
                    Program.Cash.ItemName = new Class.Query.NameCash().GetCashItemName();
                    Program.Cash.СhangedItems = Program.Cash.СhangedItems.FindAll(x => x.Value != null);

                    break;
                case FillTable.СhangedSiteTable:

                    List<KeyValuePair<PriceStruct, ItemDBStruct>> FindList = Program.Cash.SiteItemsСhanged.FindAll(x => x.Value != null);
                    FindList = FindList.FindAll(x => x.Value.PriceRC != x.Key.PriceRC);
                    List<ItemDBStruct> PC_List = new List<ItemDBStruct>();

                    foreach (KeyValuePair<PriceStruct, ItemDBStruct> item in FindList)
                    {
                        if (item.Key.PriceRC != item.Value.PriceRC)
                        {
                            PC_List.Add(item.Value);
                        }
                    }
                    new ItemsQuery().EditList(PC_List);
                    Program.Cash.SiteItemsСhanged = new List<KeyValuePair<PriceStruct, ItemDBStruct>>();

                    break;
            }
        }
        private void Add_New_position_from_СhangeList()
        {
            string СomparisonName = (string)this.Data.Obj;
            List<KeyValuePair<PriceStruct, ItemDBStruct>> Item = Program.Cash.СhangedItems.FindAll(x => x.Key.СomparisonName == СomparisonName);
            new Query.ItemsQuery().AddPriceStruct(Item[0].Key);
            Program.Cash.ItemName = new Class.Query.NameCash().GetCashItemName();
        }
        private static void Del_From_DB(TCP_CS_Obj Data)
        {
            int ID = (int)Data.Obj;
            ItemDBStruct item = new Class.Query.ItemsQuery().FindID(ID)[0];
            new Class.Query.ItemsQuery().Del(item);
            Program.Cash.ItemName.RemoveAll(x => x.Value == ID);
        }
        private void ItemsNameFromCash(string Name)
        {
            List<KeyValuePair<string, int>> Xlist = (from Items in Program.Cash.ItemName where Items.Key.ToUpper().Contains(Name.ToUpper()) select Items).ToList();
            if (Xlist.Count == 0) { Xlist.Add(new KeyValuePair<string, int>("not found", 0)); }
            this.Data.Obj = Xlist;
        }
        private void GetPositionFromDB()
        {
            int ID = Convert.ToInt32(this.Data.Obj);
            List<ItemDBStruct> NameList = new ItemsQuery().FindID(ID).ToList();



            Image newImage = null;
            newImage = ImageResize(NameList, newImage);

            if (NameList.Count > 0)
            {
                var ActiveItem = NameList[0];

                if (ActiveItem.ManufactorID == 0)
                {
                    ActiveItem = new SerchManufactor().Search(ActiveItem);
                    if (ActiveItem.ManufactorID != 0)
                    {
                        new ItemsQuery().Edit(ActiveItem);
                    }
                }
                ItemNetStruct itemNetStruct = new ItemNetStruct() { Item = ActiveItem, Image = newImage }; this.Data.Obj = itemNetStruct;
            }
        }
        private static Image ImageResize(List<ItemDBStruct> NameList, Image newImage)
        {
            if (NameList[0].Image != null)
            {
                newImage = Image.FromFile(NameList[0].Image);
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


}