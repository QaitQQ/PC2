using Client;
using Client.Forms;

using CRMLibs;

using StructLibs;

using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using WinFormsClientLib.Forms.WPF.Controls.CRMControls;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using System.Windows.Controls;
using System.ComponentModel;

namespace WinFormsClientLib.Forms.WPF.ItemControls
{


    /// <summary>
    /// Логика взаимодействия для ItemControl.xaml
    /// </summary>
    public partial class ItemControl : UserControl
    {

        public event Action<string> GoSite;

        private ItemNetStruct Item { get; set; }
        private Partner ActivePartner { get; set; }
        private string[] SalePair { get; set; }
        private PropertyInfo[] Prop { get; set; }
        private ObservableCollection<PropPair> Values { get; set; }
        public ItemControl(ItemNetStruct item)
        {
            Item = item;
            InitializeComponent();
            if (Item.Image!=null)  {Imagebox.Source = ConvertIMG(Item.Image); }
            (((Main.CommonWindow as Mainform).CRMForm as UniversalWPFContainerForm).Control as MainCRMControl).ChangedPartner += new Action<Partner>(SetPartner);
            (((Main.CommonWindow as Mainform).itemForm as UniversalWPFContainerForm).Control as MainItemControl).ChangeSale += new Action<int,int>(RenewPriceDC);
            Prop = ItemDBStruct.GetProperties();
            Values = new ObservableCollection<PropPair>();
            foreach (var X in Prop)
            {
                var PParir = new PropPair() { PropertyInfo = X, Value = X.GetValue(Item.Item) };
                PParir.PropertyChanged += new PropertyChangedEventHandler(RenewItem);
                Values.Add(PParir);
            }
            PropInfo.ItemsSource = Values;
     
        }
        private void SetPartner(Partner Partner) { ActivePartner = Partner; }
        private void RenewItem(object Obj, PropertyChangedEventArgs e)
        {
            var propPair = (PropPair)Obj;

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
            using var memory = new MemoryStream();
            img.Save(memory, ImageFormat.Png);
            memory.Position = 0;
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = memory;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();
            return bitmapImage;
        }
        private void RenewPriceDC(int Sale,int Markup) 
        {
            var RC = Values.First(x => x.PropertyInfo.Name.Contains("PriceRC"));
            double FinDC = ((double)RC.Value);
            FinDC = FinDC * (1 - ((double)Sale / 100));
            FinDC = FinDC * (1 + ((double)Markup / 100));


            var DC = Values.First(x => x.PropertyInfo.Name.Contains("PriceDC"));
            DC.Value = FinDC;
            PropInfo.Items.Refresh();
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e) { }
        private void GoToSiteButton_Click(object sender, RoutedEventArgs e)
        {
            string id = Item.Item.SiteId.ToString();
            string Fulladress = "https://salessab.su/index.php?route=product/product&product_id=" + id;
            GoSite(Fulladress);
        }
    }
}
