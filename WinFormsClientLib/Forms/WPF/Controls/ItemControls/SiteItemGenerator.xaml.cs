
using StructLibs;

using System.Drawing.Imaging;
using System.IO;
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
               
            }

            ImageStack.Children.Add(new System.Windows.Controls.Image() { Source = Image, Width = Image.Width });

            ItemFieldStack.Children.Add(GetGid(null, "Описание", Item.Item.Description));
            ItemFieldStack.Children.Add(GetGid(null, "Цена", Item.Item.PriceRC.ToString()));
            ItemFieldStack.Children.Add(GetGid(null, "ID", Item.Item.Id.ToString()));

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


        private Grid GetGid(string nomber, string name, string content)
        {
            Grid MainGrid = new Grid();

            MainGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(40) });
            MainGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(80) });
            MainGrid.ColumnDefinitions.Add(new ColumnDefinition());

            Grid OneGrid = new Grid() { Children = { new TextBox() { Text = nomber } } };
            Grid TwoGrid = new Grid() { Children = { new TextBox() { Text = name } } };
            Grid ThreeGrid = new Grid() { Children = { new TextBox() { Text = content, TextWrapping = TextWrapping.Wrap, Height = 100 } } };

            Grid.SetColumn(OneGrid, 0);
            Grid.SetColumn(TwoGrid, 1);
            Grid.SetColumn(ThreeGrid, 2);
            MainGrid.Children.Add(OneGrid);
            MainGrid.Children.Add(TwoGrid);
            MainGrid.Children.Add(ThreeGrid);
            return MainGrid;
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
           var D =  new ContextMenu();
            D.Items.Add(new TextBox() { Text = "Привет" });
            D.IsOpen = true;
        }
    }
}
