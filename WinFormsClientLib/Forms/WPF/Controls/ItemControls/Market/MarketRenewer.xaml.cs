using Client;

using Server.Class.IntegrationSiteApi.Market.Ozon;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WinFormsClientLib.Forms.WPF.Controls.ItemControls
{
    /// <summary>
    /// Логика взаимодействия для MarketRenewer.xaml
    /// </summary>
    public partial class MarketRenewer : UserControl
    {
        private ObservableCollection<object> Items;

        public string MarketName { get; set; }


        public MarketRenewer()
        {
            MarketName = "Ozon";
            InitializeComponent();
            Items = new ObservableCollection<object>();
            ItemStack.ItemsSource = Items;
        }

        private void Get_List_Click(object sender, RoutedEventArgs e)
        {
            var X = new Network.Item.MarketApi.GetListOzon().Get<List<object>>(new WrapNetClient());
            foreach (var item in X)
            {
                Items.Add(item);
            }
        }

        private void Get_Click(object sender, RoutedEventArgs e)
        {
           // ((Item_D)((Grid)((Button)sender).Parent).DataContext);
        }
    }
}
