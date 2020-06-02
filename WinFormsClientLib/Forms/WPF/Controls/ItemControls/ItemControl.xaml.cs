using Client;
using Client.Forms;

using CRMLibs;

using StructLibs;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;

using WinFormsClientLib.Forms.WPF.Controls.CRMControls;

namespace WinFormsClientLib.Forms.WPF.ItemControls
{
    /// <summary>
    /// Логика взаимодействия для ItemControl.xaml
    /// </summary>
    public partial class ItemControl : UserControl, IDisposable
    {
        public event Action<string> GoSite;
        private ItemPlusImage Item { get; set; }
        private Partner ActivePartner { get; set; }
        private BitmapImage Image { get; set; }
        private PropertyInfo[] Prop { get; set; }
        private List<PropPair> Values { get; set; }
        public ItemControl(ItemPlusImage item, System.Reflection.PropertyInfo[] Prop)
        {
            this.Prop = Prop;
            Item = item;
            InitializeComponent();
            if (Item.Image != null)
            {
                Image = ConvertIMG(Item.Image);
                Image.CacheOption = BitmapCacheOption.OnLoad;
                Imagebox.Source = Image;
            }
            (((Main.CommonWindow as Mainform).CRMForm as UniversalWPFContainerForm).Control as MainCRMControl).ChangedPartner += new Action<Partner>(SetPartner);
            (((Main.CommonWindow as Mainform).itemForm as UniversalWPFContainerForm).Control as MainItemControl).ChangeSale += new Action<int, int>(RenewPriceDC);
           
            Values = new List<PropPair>();
            FillProp();
        }
        private void FillProp()
        {
            foreach (PropertyInfo X in Prop)
            {
                PropPair PParir = new PropPair() { PropertyInfo = X, Value = X.GetValue(Item.Item) };

                if (PParir.PropertyInfo.Name.Contains("PriceDC"))
                {
                    PParir.Name = "Цена со скидкой";
                }
                else if (PParir.PropertyInfo.Name.Contains("Name"))
                {
                    PParir.Name = "";
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

                PropInfo.ItemsSource = Values;
            }

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
            return bitmapImage;
        }
        private void RenewPriceDC(int Sale, int Markup)
        {
            PropPair RC = Values.First(x => x.PropertyInfo.Name.Contains("PriceRC"));
            double FinDC = ((double)RC.Value);
            FinDC = FinDC * (1 - ((double)Sale / 100));
            FinDC = FinDC * (1 + ((double)Markup / 100));
            PropPair DC = Values.First(x => x.PropertyInfo.Name.Contains("PriceDC"));
            DC.Value = FinDC;
            PropInfo.Items.Refresh();
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            new Network.Item.EditItem().Get<bool>(new WrapNetClient(), Item);
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
            (((Main.CommonWindow as Mainform).CRMForm as UniversalWPFContainerForm).Control as MainCRMControl).ChangedPartner -= new Action<Partner>(SetPartner);
            (((Main.CommonWindow as Mainform).itemForm as UniversalWPFContainerForm).Control as MainItemControl).ChangeSale -= new Action<int, int>(RenewPriceDC);
            foreach (var item in Values)
            {
                item.PropertyChanged -= RenewItem;
                item.Dispose();
            }
            GoSite = null;
            ActivePartner = null;
            Imagebox.Source = null;
            Imagebox = null;
            Item = null;
            Prop = null;
            Image = null;
            Values = null;
            GC.SuppressFinalize(this);
        }
    }
}
