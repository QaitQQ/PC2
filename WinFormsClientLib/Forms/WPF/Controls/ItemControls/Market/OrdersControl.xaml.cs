using Client;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using StructLibCore;
using StructLibCore.Marketplace;

namespace WinFormsClientLib.Forms.WPF.Controls.ItemControls.Market
{
    /// <summary>
    /// Логика взаимодействия для OrdersControl.xaml
    /// </summary>
    public partial class OrdersControl : UserControl
    {
        private List<APISetting> Options;

        private ObservableCollection<IOrder> OrderList;
        private ObservableCollection<IOrder> VisOrderList;

        private event Action<List<IOrder>> LoadOrders;
        public OrdersControl(List<APISetting> Option)
        {
            InitializeComponent();
            this.Options = Option;
            OrderList = new ObservableCollection<IOrder>();
            VisOrderList = new ObservableCollection<IOrder>();
            System.Threading.Tasks.Task.Factory.StartNew(()  => LoadNetOrders(Options));

            OrderStack.ItemsSource = VisOrderList;
            StatusBox.ItemsSource = Enum.GetValues(typeof(OrderStatus));

            LoadOrders += FillOrders;
        }

        private void FillOrders(List<IOrder> orders)
        {
            OrderList.Clear();

            foreach (var order in orders)
            {
                OrderList.Add((IOrder)order);
            }

            StatusBox.SelectedIndex = 1;
        }

        private void LoadNetOrders(List<APISetting> aPIs) 
        {
            var F = new List<IOrder>();

            foreach (var item in aPIs)
            {
                if (item.Active)
                {
                    var X = new Network.Item.MarketApi.GetListOrders().Get<List<object>>(new WrapNetClient(), item);
                    foreach (var t in X)
                    {
                        F.Add((IOrder)t);
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
            IEnumerable<IOrder> X = OrderList.Where(x => x.Status == (OrderStatus)StatusBox.SelectedItem);

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
