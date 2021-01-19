
using Client;

using Object_Description;

using StructLibs;

using System.Drawing.Imaging;
using System.IO;
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
                ImageStack.Children.Add(new System.Windows.Controls.Image() { Source = Image, Width = Image.Width });
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
        private void ImagePlus_Click(object sender, RoutedEventArgs e)
        {
            var D = new ContextMenu();
            D.Items.Add(new TextBox() { Text = "Загрузить с сайта производителя" });
            D.Items.Add(new TextBox() { Text = "Загрузить с диска" });
            D.IsOpen = true;
        }

        private void ParseAttribeteFromDescription(string Desc)
        {

          var Dictionary =  new Network.Dictionary.GetDictionaresByRelate().Get<Dictionaries>(new WrapNetClient(), DictionaryRelate.Attribute);

            foreach (var item in Dictionary.GetAll())
            {
                if (Desc.Contains(item.Name))
                {
                    ItemFieldStack.Children.Add(new FieldGrid(item.Id, item.Name, null));
                }
            }

             Dictionary = new Network.Dictionary.GetDictionaresByRelate().Get<Dictionaries>(new WrapNetClient(), DictionaryRelate.Category);

            foreach (var item in Dictionary.GetAll())
            {
                if (Desc.Contains(item.Name))
                {
                    ItemFieldStack.Children.Add(new FieldGrid(item.Id, item.Name, null));
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
                var BoxOne = new TextBox();
                binding.Source = this;
                binding.Path = new PropertyPath("ID");
                binding.Mode = BindingMode.TwoWay;
                BoxOne.SetBinding(TextBox.TextProperty, binding);
                Grid OneGrid = new Grid() { Children = { BoxOne } };

                Binding binding2 = new Binding();
                var BoxTwo = new TextBox();
                binding2.Source = this;
                binding2.Path = new PropertyPath("FieldName");
                binding2.Mode = BindingMode.TwoWay;
                BoxTwo.SetBinding(TextBox.TextProperty, binding2);
                Grid TwoGrid = new Grid() { Children = { BoxTwo } };

                Binding binding3 = new Binding();
                var BoxThree = new TextBox();
                binding3.Source = this;
                binding3.Path = new PropertyPath("Description");
                binding3.Mode = BindingMode.TwoWay;
                BoxThree.SetBinding(TextBox.TextProperty, binding3);
                Grid ThreeGrid = new Grid() { Children = { BoxThree } };

                this.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(20) });
                this.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(80) });
                this.ColumnDefinitions.Add(new ColumnDefinition());
                Grid.SetColumn(OneGrid, 0);
                Grid.SetColumn(TwoGrid, 1);
                Grid.SetColumn(ThreeGrid, 3);
                this.Children.Add(OneGrid);
                this.Children.Add(TwoGrid);
                this.Children.Add(ThreeGrid);
            }



        }
    }
}
