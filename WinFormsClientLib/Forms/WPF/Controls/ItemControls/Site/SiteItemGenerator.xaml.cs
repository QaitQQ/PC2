using Client;

using Object_Description;

using StructLibs;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WinFormsClientLib.Forms.WPF.Controls.ItemControls
{

    /// <summary>
    /// Логика взаимодействия для SiteItemGenerator.xaml
    /// </summary>
    public partial class SiteItemGenerator : UserControl
    {
        public ItemPlusImageAndStorege Item;
        private ObservableCollection<WrapImage> Images;
        private ObservableCollection<FieldGrid> StackSourse { get; set; }
        public SiteItemGenerator(ItemPlusImageAndStorege item)
        {
            Item = item;
            StackSourse = new ObservableCollection<FieldGrid>();
            InitializeComponent();
            MinimizeButton.Content = Item.Item.Name;
            Images = new ObservableCollection<WrapImage>();
            ImageStack.ItemsSource = Images;        
            if (Item.Image != null)
            {
                using System.Drawing.Image Img = Item.Image;
                
                if (item.Image != null)   { Images.Add(new WrapImage(item.Image));  }
            }

            if (Item.Item.DescriptionSeparator == null)
            {
                Item.Item.DescriptionSeparator = ",";
            }

            FieldStack.ItemsSource = StackSourse;

            var Desc = Item.Item.Description;

            var DicManuf = new Network.Dictionary.GetDictionaresByRelate().Get<Dictionaries>(new WrapNetClient(), DictionaryRelate.Manufactor);

            foreach (var X in DicManuf.GetAll())
            {
                if (X.Id == item.Item.ManufactorID)
                {
                    Desc = Desc + Item.Item.DescriptionSeparator+ X.Name;
                    break;
                }
            }
            StackSourse.Add(new FieldGrid(0, "Name", item.Item.Name, FieldType.Name));
            StackSourse.Add(new FieldGrid(0, "Description", item.Item.Description, FieldType.Description));
            StackSourse.Add(new FieldGrid(0, "Price", item.Item.PriceRC.ToString(), FieldType.Price));
            StackSourse.Add(new FieldGrid(0, "Manufactor", item.Item.ManufactorID.ToString(), FieldType.Manufactor));
            StackSourse.Add(new FieldGrid(0, "СomparisonName", item.Item.СomparisonName[0], FieldType.СomparisonName));
            ParseAttribeteFromDescription(Desc);
        }
        private void ImageSearchButton_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {        

        }
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainGrid.Height == 20)
            {
                MainGrid.Height = 420;
            }
            else
            {
                MainGrid.Height = 20;
            }
        }
        private System.Drawing.Image ConvertBitmapImage(BitmapImage img)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(img));     
                enc.Save(outStream);            
                System.Drawing.Image image = System.Drawing.Image.FromStream(outStream);
                using var MS = new MemoryStream();
                image.Save("123.png");
                image.Save(MS, ImageFormat.Png);
                System.Drawing.Image Img = System.Drawing.Image.FromStream(MS);
                Img.Save("223.png");
                return Img;
            }
        }
        private void ImagePlus_Click(object sender, RoutedEventArgs e)
        {
            System.Drawing.Image X = new Network.Item.SiteApi.NetImageParser().Get<System.Drawing.Image>(new WrapNetClient(), Item.Item);
            Images.Add(new WrapImage(X));
        }
        private void ParseAttribeteFromDescription(string Desc)
        {
            Desc = Desc.ToLower();

            Desc = Desc.Replace("\n", "|");
            Desc = Desc.Replace("\r", "|");
            Desc = Desc.Replace("\t", "|");
            Dictionaries Dictionary = new Network.Dictionary.GetDictionaresByRelate().Get<Dictionaries>(new WrapNetClient(), DictionaryRelate.Attribute);

            string[] DescMass = null;

            if (Item.Item.DescriptionSeparator != null)
            {
                DescMass = Desc.Split(Item.Item.DescriptionSeparator[0]);

                List<string> Lst = DescMass.ToList();

                for (int i = 0; i < Item.Item.DescriptionSeparator.Length; i++)
                {
                    char sep = Item.Item.DescriptionSeparator[i];

                    for (int t = 0; t < Lst.Count; t++)
                    {
                        if (Lst[t].Contains(sep))
                        {

                            string[] massstr = Lst[t].Split(sep);
                            for (int c = 1; c < massstr.Length; c++)
                            {
                                    Lst.Add(massstr[c]);                            
                            }
                            Lst[t] = Lst[t].Split(sep)[0];
                        }
                        if (Lst[t].Contains("|"))
                        {

                            string[] massstr = Lst[t].Split('|');
                            for (int c = 1; c < massstr.Length; c++)
                            {
                                Lst.Add(Lst[t].Split('|')[c]);
                            }
                            Lst[t] = Lst[t].Split('|')[0];
                        }
                    }
                    DescMass = Lst.ToArray();
                }
            }

            foreach (IDictionaryPC item in Dictionary.GetAll())
            {

                string DescName = item.Name.ToLower();
                string Value = null;
                List<string> DescValues = item.Values;
                if (DescValues == null || DescValues.Count == 0)
                {
                    DescValues = new List<string>
                    {
                        DescName
                    };
                }

                foreach (string DescVal in DescValues)
                {
                    string X = DescVal;
                    string DefVal = null;
                    bool Double_flag = false;
                    if (X.Contains('|'))
                    {
                        DefVal = X.Split('|')[1];
                        X = X.Split('|')[0];
                    }
                    Regex FX = new Regex(X, RegexOptions.IgnoreCase);

                    if (FX.IsMatch(Desc))
                    {
                        if (DescMass != null)
                        {
                            foreach (string E in DescMass)
                            {
                                if (FX.IsMatch(E))
                                {
                                    MatchCollection D = FX.Matches(E);
                                    Value = E.Trim();
                                    if (DefVal != null)
                                    {
                                        Value = DefVal;
                                    }
                                    Double_flag = true;
                                    break;
                                }
                            }

                        }
                        StackSourse.Add(new FieldGrid(item.Id, item.Name, Value, FieldType.Attribute));
                        if (Double_flag)
                        {
                            break;
                        }
                    }
                }
            }

            Dictionary = new Network.Dictionary.GetDictionaresByRelate().Get<Dictionaries>(new WrapNetClient(), DictionaryRelate.Category);

            foreach (IDictionaryPC item in Dictionary.GetAll())
            {
                List<string> DescValues = item.Values;

                if (DescValues != null && DescValues.Count != 0)
                {
                    bool Flag = true;

                    foreach (var DescValue in DescValues)
                    {
                        Regex regex = new Regex(DescValue, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

                        if (!regex.IsMatch(Desc))
                        {
                            Flag = false;
                            break;
                        }
                    }

                    if (Flag)
                    {
                        StackSourse.Add(new FieldGrid(item.Id, item.Name, null, FieldType.Category));

                        DictionarySiteCategory X = item as DictionarySiteCategory;
                        int i = 0;

                        while (X.Parent_id != 0 || i > 15)
                        {
                            foreach (var t in Dictionary.GetAll())
                            {
                                if (t.Id == X.Parent_id)
                                {
                                    StackSourse.Add(new FieldGrid(t.Id, t.Name, null, FieldType.Category));
                                    X = t as DictionarySiteCategory;
                                    break;
                                }
                            }

                            i++;
                        }
                    }
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<SiteFieldDesc> lst = new List<SiteFieldDesc>();
            foreach (var item in StackSourse)
            {
                lst.Add(new SiteFieldDesc() { id = item.ID.ToString(), Desc = item.Description, Type = item.Type });
            }
            if (Images.Count > 0)
            {
                System.Drawing.Image img = Images[0].GetImage();
                lst.Add(new SiteFieldDesc() { Obj = img, Type = FieldType.Image });
            }

            var I = new Network.Item.SiteApi.AddNewPosition().Get<int>(new WrapNetClient(), lst);
        }
        private void Grid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Right)
            {
                var menu = new ContextMenu();

                var panel = new Grid();
                var button = new TextBlock() { Text = "Удалить" };
                button.MouseDown += Button_MouseDown;
                menu.Items.Add(button);
                ((Grid)sender).ContextMenu = menu;
            }
             void Button_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
            {
                Images.Remove((WrapImage)((Image)((Grid)((ContextMenu)((TextBlock)sender).Parent).PlacementTarget).Children[0]).DataContext);
            }
           
        }
        private static System.Drawing.Image ImageResize(ItemDBStruct Item, System.Drawing.Image newImage)
        {
            if (File.Exists(Item.Image))
            {
                System.Drawing.Image Img = System.Drawing.Image.FromFile(Item.Image);

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
        private static System.Drawing.Bitmap ResizeImage(System.Drawing.Image image, int width, int height)
        {
            System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(0, 0, width, height);
            System.Drawing.Bitmap destImage = new System.Drawing.Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

                using (ImageAttributes wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(System.Drawing.Drawing2D.WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, System.Drawing.GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}
