using StructLibCore;
using StructLibCore.Marketplace;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WinFormsClientLib.Forms.WPF.Controls.ItemControls.Market
{
    /// <summary>
    /// Логика взаимодействия для MainMarketPlaceControl.xaml
    /// </summary>
    /// 
    public partial class MainMarketPlaceControl : UserControl
    {
        List<APISetting> Option;

        private event Action OrdersLoaded;


        bool OrdersL;
        bool ItemsL;
        bool OptionL;

        public MainMarketPlaceControl()
        {
            InitializeComponent();

            OrdersL = false;
            ItemsL = false;
            OptionL = false;
            Loaded += (e, x) =>
            {
                System.Threading.Tasks.Task.Factory.StartNew(() =>
                {

                    var X = new Network.Item.MarketApi.GetListOption().Get<List<APISetting>>(new Client.WrapNetClient());

                    Dispatcher.Invoke(() => Option = X);

                    Dispatcher.Invoke(() => OrdersLoaded());


                });
            };


            OrdersLoaded += () =>
            {
                Orders.Content = new OrdersControl(Option);
                OrdersL = true;
            };
        }



        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabItem selectedTab = e.AddedItems[0] as TabItem;
            if (Option != null && selectedTab != null)
            {
                if (selectedTab.Name == "Orders" && !OrdersL)
                {
                    Orders.Content = new OrdersControl(Option);
                    OrdersL = true;
                }
                else if (selectedTab.Name == "Items" && !ItemsL)
                {
                    Items.Content = new ItemControl(Option);
                    ItemsL = true;
                }
                else if (selectedTab.Name == "Options" && !OptionL)
                {
                    Options.Content = new OptionControl(Option);
                    OptionL = true;
                }
            }
          

        }
    }
}
