using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
namespace MGSol.Panel
{
    /// <summary>
    /// Логика взаимодействия для ShipmentListControl.xaml
    /// </summary>
    public partial class ShipmentListControl : UserControl
    {
        private MainModel Model { get; set; }
        private ObservableCollection<ShipmentOrder> shipmentOrders { get; set; }
        public ShipmentListControl(MainModel model)
        {
            InitializeComponent();
            Model = model;
            shipmentOrders = new ObservableCollection<ShipmentOrder>();
            DTtable.ItemsSource = shipmentOrders;
        }
        public void LoadData()
        {
            foreach (ShipmentOrder item in Model.ShipmentOrders)
            {
                shipmentOrders.Add(item);
            }
        }
        private void Get_Click(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(() => { LoadData(); }));
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Model.ShipmentOrders = shipmentOrders.ToList();
        }
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Model.ShipmentOrders = new List<ShipmentOrder>();
            shipmentOrders.Clear();
            this.Dispatcher.BeginInvoke(new Action(() => { LoadData(); }));
        }
    }
}
