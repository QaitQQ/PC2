using Client;

using CRMLibs;
using Object_Description;
using StructLibs;

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WinFormsClientLib.Forms.WPF.ItemControls
{
    /// <summary>
    /// Логика взаимодействия для ItemControl.xaml
    /// </summary>
    public partial class ItemControl : UserControl, IDisposable
    {
        public event Action<string> GoSite;
        public event Action<ItemPlusImageAndStorege> GenSiteItem;
        private ItemPlusImageAndStorege Item { get; set; }
        private Partner ActivePartner { get; set; }
        private PropertyInfo[] Prop { get; set; }
        private List<PropPair> Values { get; set; }
        public ItemControl(ItemPlusImageAndStorege item)
        {
            InitializeComponent();
            Prop = ItemDBStruct.GetProperties();
            Item = item;

            if (Item.Image != null)
            {

            }
            else if (Item.Item.Image != null && Item.Item.Image != "")
            {
                GetImage();
            }


            ActiveValue.ChangedPartner += new Action<Partner>(SetPartner);
            ActiveValue.ChangeSale += new Action<int, int>(RenewPriceDC);
            FillProp();
        }
        private void FillProp()
        {
            Values = new List<PropPair>();
            foreach (PropertyInfo X in Prop)
            {
                PropPair PParir = new PropPair() { PropertyInfo = X, Value = X.GetValue(Item.Item) };

                if (!(PParir.Value is String) && PParir.Value is IEnumerable)
                {
                    string Value = null;


                    foreach (object item in PParir.Value as IEnumerable)
                    {
                        Value += item.ToString() + "\n";
                    }

                    PParir.Value = Value;
                }


                if (PParir.PropertyInfo.Name.Contains("PriceDC"))
                {
                    PParir.Name = "Цена со скидкой";

                }
                else if (PParir.PropertyInfo.Name == "Name")
                {
                    PParir.Name = "";
                }
                else if (PParir.PropertyInfo.Name.Contains("StorageID") && Item.Storages != null)
                {
                    PParir.Name = "Остатки";

                    string Storege = null;

                    foreach (Storage item in Item.Storages)
                    {
                        Storege = Storege + item.Warehouse.Name + "  " + item.Count + "  " + item.SourceName + "  " + item.DateСhange + "\n";
                    }

                    PParir.Value = Storege;

                }
                else if (PParir.PropertyInfo.Name.Contains("PriceRC"))
                {
                    PParir.Name = "Розница";
                }
                else if (PParir.Name == null)
                {
                    PParir.Name = PParir.PropertyInfo.Name;
                }

                PParir.PropertyChanged += RenewItem;
                Values.Add(PParir);


            }
            PropInfo.ItemsSource = Values;
        }
        private void SetPartner(Partner Partner) { ActivePartner = Partner; }
        private void RenewItem(object Obj, PropertyChangedEventArgs e)
        {
            PropPair propPair = (PropPair)Obj;

            if (!propPair.PropertyInfo.Name.Contains("PriceDC"))
            {
                if (propPair.PropertyInfo.PropertyType.Name.Contains("Int"))
                {
                    propPair.PropertyInfo.SetValue(Item.Item, Convert.ToInt32(propPair.Value));
                }
                else if (propPair.PropertyInfo.PropertyType.Name.Contains("Double"))
                {
                    propPair.PropertyInfo.SetValue(Item.Item, Convert.ToDouble(propPair.Value));
                }
                else if (propPair.PropertyInfo.PropertyType.Name.Contains("String"))
                {
                    propPair.PropertyInfo.SetValue(Item.Item, propPair.Value);
                }
            }
        }
        private BitmapImage ConvertIMG(System.Drawing.Image img)
        {
            using MemoryStream memory = new MemoryStream();
            img.Save(memory, ImageFormat.Png);
            memory.Position = 0;
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = memory;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();
            memory.Close();
            return bitmapImage;
        }
        private void RenewPriceDC(int Sale, int Markup)
        {
            try
            {
                if (Values != null)
                {
                    PropPair RC = Values.First(x => x.PropertyInfo.Name.Contains("PriceRC"));
                    double FinDC = ((double)RC.Value);
                    FinDC = FinDC * (1 - ((double)Sale / 100));
                    FinDC = FinDC * (1 + ((double)Markup / 100));
                    FinDC = Math.Round(FinDC, 2);
                    PropPair DC = Values.First(x => x.PropertyInfo.Name.Contains("PriceDC"));
                    DC.Value = FinDC;
                    PropInfo.Items.Refresh();
                }
            }
            catch (Exception E)
            {

                MessageBox.Show(E.Message);
            }

        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

         // Item.Item.SourceName = Main.ActiveUser;

          new Network.Item.EditItem().Get<bool>(new WrapNetClient(), Item.Item);
        }
        private void GoToSiteButton_Click(object sender, RoutedEventArgs e)
        {
            string id = Item.Item.SiteId.ToString();
            string Fulladress = "https://salessab.su/index.php?route=product/product&product_id=" + id;
            GoSite?.Invoke(Fulladress);
        }
        public void Dispose()
        {
            PropInfo.ItemsSource = null;
            PropInfo = null;
            foreach (PropPair item in Values) { item.PropertyChanged -= RenewItem; item.Dispose(); }
            GoSite = null;
            ActivePartner = null;
            Imagebox.Source = null;
            Imagebox = null;
            Item = null;
            Prop = null;
            Content = null;
            Values = null;
            ActiveValue.ChangedPartner -= new Action<Partner>(SetPartner);
            ActiveValue.ChangeSale -= new Action<int, int>(RenewPriceDC);
            GC.SuppressFinalize(this);

        }
        private void GoToManufSiteButton_Click(object sender, RoutedEventArgs e)
        {
            if (Item.Item.ManufactorID != 0)
            {
                ManufactorSite X = new Network.Other.GetManufSite().Get<ManufactorSite>(new WrapNetClient(), Item.Item.ManufactorID);
                string Name = Item.Item.Name;
                string Link = X.SiteLink + X.SearchLink + Name;
                GoSite?.Invoke(Link);
            }

        }
        private void FindManuf_Click(object sender, RoutedEventArgs e)
        {
            bool X = new Network.Other.FindManuf().Get<bool>(new WrapNetClient(), Item.Item);
            if (X)
            {
                ItemPlusImageAndStorege F = new Network.Item.GetItemFromId().Get<ItemPlusImageAndStorege>(new WrapNetClient(), Item.Item.Id);
                Item.Item.ManufactorID = F.Item.ManufactorID;
                FillProp();
            }


        }
        private double ConvertToDouble(string STR)
        {
            string newStr = null;

            foreach (char item in STR)
            {
                if (char.IsDigit(item) || item == '.' || item == ',')
                {
                    if (item == '.')
                    {
                        newStr += ',';
                    }
                    else
                    {
                        newStr += item;
                    }
                }
            }

            return Convert.ToDouble(newStr);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new Network.Item.MergeItem().Get<bool>(new WrapNetClient(), new int[] { Item.Item.Id, Convert.ToInt32(ConvertToDouble(UniID.Text)) });
        }
        private void SiteGen_Click(object sender, RoutedEventArgs e)
        {
            GenSiteItem?.Invoke(Item);
        }
        private async void GetImage()
        {
            using Network.Item.GetItemImage Qwery = new Network.Item.GetItemImage();
            System.Drawing.Image Img = null;

            await System.Threading.Tasks.Task.Factory.StartNew(() => { Img = Qwery.Get<ItemPlusImageAndStorege>(new WrapNetClient(), Item.Item.Id.ToString()).Image; });
            if (Img != null && Item != null)
            {
                BitmapImage image = ConvertIMG(Img);

                if (Imagebox == null)
                {
                    Imagebox = new Image();
                }
                Imagebox.Source = image;
                Item.Image = Img;
            }
        }
        private void PriceChangeHistoryShow(object sender, RoutedEventArgs e)
        {
          var X =  new Network.Item.PriceChangeHistoryShow().Get<List<PriceСhangeHistory>>(new WrapNetClient(), Item.Item.Id);

            string msg = null;

            foreach (var item in X)
            {
                msg = msg + item.PriceRC.ToString() + " " + item.DateСhange.ToString() + " " + item.SourceName + "\n";
            }


            MessageBox.Show(msg);
        }
    }
}
