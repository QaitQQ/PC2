using Client;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Linq;

namespace WinFormsClientLib.Forms.WPF.Controls.ItemControls.Market
{
    /// <summary>
    /// Логика взаимодействия для OrdersControl.xaml
    /// </summary>
    public partial class OrdersControl : UserControl
    {
        private List<StructLibCore.Marketplace.APISetting> Options;

        private ObservableCollection<StructLibCore.Marketplace.IOrder> OrderList;
        private ObservableCollection<StructLibCore.Marketplace.IOrder> VisOrderList;

        private event Action<List<StructLibCore.Marketplace.IOrder>> LoadOrders;
        public OrdersControl(List<StructLibCore.Marketplace.APISetting> Option)
        {
            InitializeComponent();
            this.Options = Option;
            OrderList = new ObservableCollection<StructLibCore.Marketplace.IOrder>();
            VisOrderList = new ObservableCollection<StructLibCore.Marketplace.IOrder>();
            System.Threading.Tasks.Task.Factory.StartNew(()  => LoadNetOrders(Options));

            OrderStack.ItemsSource = VisOrderList;
            StatusBox.ItemsSource = Enum.GetValues(typeof(StructLibCore.Marketplace.OrderStatus));

            LoadOrders += FillOrders;
        }

        private void FillOrders(List<StructLibCore.Marketplace.IOrder> orders)
        {
            OrderList.Clear();

            foreach (var order in orders)
            {
                OrderList.Add((StructLibCore.Marketplace.IOrder)order);
            }

            StatusBox.SelectedIndex = 1;
        }

        private void LoadNetOrders(List<StructLibCore.Marketplace.APISetting> aPIs) 
        {
            var F = new List<StructLibCore.Marketplace.IOrder>();

            foreach (var item in aPIs)
            {
                if (item.Active)
                {
                    var X = new Network.Item.MarketApi.GetListOrders().Get<List<object>>(new WrapNetClient(), item);
                    foreach (var t in X)
                    {
                        F.Add((StructLibCore.Marketplace.IOrder)t);
                    }
                }    

              
                
            }

           Dispatcher.Invoke(()=>   LoadOrders(F));

        }




        private void Fill_VOrders(object sender, RoutedEventArgs e)
        {
            System.Threading.Tasks.Task.Factory.StartNew(() => LoadNetOrders(Options));
        }

        private void StatusBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VisOrderList.Clear();
            IEnumerable<StructLibCore.Marketplace.IOrder> X = OrderList.Where(x => x.Status == (StructLibCore.Marketplace.OrderStatus)StatusBox.SelectedItem);

            foreach (var item in X)
            {
                VisOrderList.Add(item);
            }

            GC.Collect();
        
        }

        private void ListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Clipboard.SetText(((ListBox)sender).SelectedItem.ToString());

            
        }
    }
}
