using StructLibCore.Marketplace;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MGSol.Panel
{
    /// <summary>
    /// Логика взаимодействия для ReturnControl.xaml
    /// </summary>
    public partial class ReturnControl : UserControl
    {
        private MainModel model;
        private ObservableCollection<APISetting> Options;
        private ObservableCollection<IOrder> VisOrderList;
        private ObservableCollection<IOrder> orderList;
        public ReturnControl(MainModel Model)
        {
            model = Model;
            InitializeComponent();
            Options = new ObservableCollection<APISetting>();
            orderList = new ObservableCollection<IOrder>();
            PrintActBtnStack.ItemsSource = Options;
            VisOrderList = new ObservableCollection<IOrder>();
            StatusBox.ItemsSource = Enum.GetValues(typeof(StructLibCore.Marketplace.OrderStatus));
            OrderStack.ItemsSource = VisOrderList;
        }
        private void StatusBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VisOrderList.Clear();
            IEnumerable<IOrder> X = from x in orderList where x.Status == (OrderStatus)StatusBox.SelectedItem orderby x.DeliveryDate select x;
            foreach (IOrder item in X)
            {
                VisOrderList.Add(item);
            }
            GC.Collect();
        }
        private void LoadNetReturns(List<APISetting> aPISettings)
        {
            orderList.Clear();
            List<IOrder> F = new();
            foreach (APISetting item in aPISettings)
            {
                if (item.Active)
                {
                    List<object> Result = new();
                    switch (item.Type)
                    {
                        case MarketName.Yandex:
                            break;
                        case MarketName.Ozon:
                            Result = new Server.Class.IntegrationSiteApi.Market.Ozon.OzonPost.OzonPostReturnList.OzonPostReturnList(item).Get();
                            break;
                        case MarketName.Avito:
                            break;
                        case MarketName.Sber:
                            break;
                        default:
                            break;
                    }

                    if (Result != null)
                    {
                        Dispatcher.Invoke(() => Options.Add(item));
                        foreach (object t in Result)
                        {
                            Dispatcher.Invoke(() => orderList.Add((StructLibCore.Marketplace.IOrder)t));
                        }
                    }
                }
            }
        }

        private void Fill_VOrders(object sender, System.Windows.RoutedEventArgs e)
        {
            Options.Clear();
            Task.Factory.StartNew(() => LoadNetReturns(model.OptionMarketPlace.APISettings));
            StatusBox.SelectedIndex = 1;
        }

    }
}
