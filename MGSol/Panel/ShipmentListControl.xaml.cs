using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
namespace MGSol.Panel
{
    public partial class ShipmentListControl : UserControl
    {
        private MainModel Model { get; set; }
        private ObservableCollection<SiteApi.IntegrationSiteApi.ApiBase.ObjDesc.Order> shipmentOrders { get; set; }
        public ShipmentListControl(MainModel model)
        {
            InitializeComponent();
            Model = model;
            shipmentOrders = new ObservableCollection<SiteApi.IntegrationSiteApi.ApiBase.ObjDesc.Order>();
            DTtable.ItemsSource = shipmentOrders;
        }
        public void LoadData()
        {
            shipmentOrders.Clear();
             var X =  new SiteApi.IntegrationSiteApi.ApiBase.Get.GetAllOrders(Model.BaseInfoPrice.ToketBase, Model.BaseInfoPrice.UriBase).Get();

            foreach (SiteApi.IntegrationSiteApi.ApiBase.ObjDesc.Order item in X)
            {
                shipmentOrders.Add(item);
            }
        }
        private void Get_Click(object sender, RoutedEventArgs e)
        {
        //  var  BoxingDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local);

              this.Dispatcher.BeginInvoke(new Action(() => { LoadData(); }));
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Model.ShipmentOrders = new List<ShipmentOrder>();
            shipmentOrders.Clear();
            this.Dispatcher.BeginInvoke(new Action(() => { LoadData(); }));
        }
    }
}
