using Client;
using Object_Description;
using StructLibs;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace WinFormsClientLib.Forms.WPF.Controls.ItemControls
{
    /// <summary>
    /// Логика взаимодействия для SiteItemGenerator.xaml
    /// </summary>
    public partial class SiteItemGenerator : UserControl
    {
        public ItemPlusImageAndStorege Item;
        private BitmapImage Image { get; set; }
        public SiteItemGenerator(ItemPlusImageAndStorege item)
        {
            Item = item;
            InitializeComponent();
            MinimizeButton.Content = Item.Item.Name;

            if (Item.Image != null)
            {
                using System.Drawing.Image Img = Item.Image;
                Image = ConvertIMG(Item.Image);
                if (Image != null)
                {
                    ImageStack.Children.Add(new System.Windows.Controls.Image() { Source = Image, Width = Image.Width });
                }

            }

            ItemFieldStack.Children.Add(new FieldGrid(0, "Name", item.Item.Name));
            ItemFieldStack.Children.Add(new FieldGrid(0, "Description", item.Item.Description));
            ItemFieldStack.Children.Add(new FieldGrid(0, "Price", item.Item.PriceRC.ToString()));
            ItemFieldStack.Children.Add(new FieldGrid(0, "Manufactor", item.Item.ManufactorID.ToString()));


            ParseAttribeteFromDescription(Item.Item.Description);
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
        private BitmapImage ConvertIMG(System.Drawing.Image img)
        {
            try
            {
                using MemoryStream memory = new MemoryStream();
                img.Save(memory, System.Drawing.Imaging.ImageFormat.Jpeg);
                memory.Position = 0;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                memory.Close();
                return bitmapImage;
            }
            catch
            {

                return null;
            }


        }
        private void ImagePlus_Click(object sender, RoutedEventArgs e)
        {
            ContextMenu D = new ContextMenu();
            D.Items.Add(new TextBox() { Text = "Загрузить с сайта производителя" });
            D.Items.Add(new TextBox() { Text = "Загрузить с диска" });
            D.IsOpen = true;
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
                                Lst.Add(Lst[t].Split('|')[c]);
                            }
                            Lst[t] = Lst[t].Split(Item.Item.DescriptionSeparator[i])[0];
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
                        ItemFieldStack.Children.Add(new FieldGrid(item.Id, item.Name, Value));
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
                        ItemFieldStack.Children.Add(new FieldGrid(item.Id, item.Name, null));
                    }
                }
            }
        }

        private sealed class FieldGrid : Grid
        {

            public int ID { get; set; }

            public string FieldName { get; set; }

            public string Description { get; set; }

            public FieldGrid(int id, string fieldName, string description)
            {
                ID = id; FieldName = fieldName; Description = description;

                Binding binding = new Binding();
                TextBox BoxOne = new TextBox();
                binding.Source = this;
                binding.Path = new PropertyPath("ID");
                binding.Mode = BindingMode.TwoWay;
                BoxOne.SetBinding(TextBox.TextProperty, binding);
                Grid OneGrid = new Grid() { Children = { BoxOne } };

                Binding binding2 = new Binding();
                TextBox BoxTwo = new TextBox();
                binding2.Source = this;
                binding2.Path = new PropertyPath("FieldName");
                binding2.Mode = BindingMode.TwoWay;
                BoxTwo.SetBinding(TextBox.TextProperty, binding2);
                Grid TwoGrid = new Grid() { Children = { BoxTwo } };

                Binding binding3 = new Binding();
                TextBox BoxThree = new TextBox();
                binding3.Source = this;
                binding3.Path = new PropertyPath("Description");
                binding3.Mode = BindingMode.TwoWay;
                BoxThree.SetBinding(TextBox.TextProperty, binding3);
                Grid ThreeGrid = new Grid() { Children = { BoxThree } };

                ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(20) });
                ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(80) });
                ColumnDefinitions.Add(new ColumnDefinition());
                Grid.SetColumn(OneGrid, 0);
                Grid.SetColumn(TwoGrid, 1);
                Grid.SetColumn(ThreeGrid, 3);
                Children.Add(OneGrid);
                Children.Add(TwoGrid);
                Children.Add(ThreeGrid);
            }



        }
    }
}
