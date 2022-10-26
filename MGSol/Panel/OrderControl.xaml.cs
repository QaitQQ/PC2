using SiteApi.IntegrationSiteApi.APIMarket.Yandex;

using StructLibCore.Marketplace;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MGSol.Panel
{
    /// <summary>
    /// Логика взаимодействия для OrderControl.xaml
    /// </summary>
    public partial class OrdersControl : UserControl
    {
        private ObservableCollection<APISetting> Options;
        private ObservableCollection<IOrder> orderList;
        private ObservableCollection<IGrouping<DateTime, IOrder>> VisOrderList;
        private event Action<List<IOrder>> LoadOrders;
        private MainModel model;
        public ObservableCollection<IOrder> OrderList { get => orderList; set => orderList = value; }
        public OrdersControl(MainModel Model)
        {
            model = Model;
            InitializeComponent();
            Options = new ObservableCollection<APISetting>();
            orderList = new ObservableCollection<StructLibCore.Marketplace.IOrder>();
            VisOrderList = new ObservableCollection<IGrouping<DateTime, StructLibCore.Marketplace.IOrder>>();
            Task.Factory.StartNew(() => LoadNetOrders(model.OptionMarketPlace.APISettings));
            OrderStack.ItemsSource = VisOrderList;
            StatusBox.ItemsSource = Enum.GetValues(typeof(StructLibCore.Marketplace.OrderStatus));
            LoadOrders += FillOrders;
            PrintActBtnStack.ItemsSource = Options;
        }
        private void FillOrders(List<IOrder> orders)
        {
            orderList.Clear();
            foreach (IOrder order in orders)
            {
                orderList.Add(order);
            }
            if (StatusBox.SelectedIndex != 1)
            {
                StatusBox.SelectedIndex = 1;
            }
            else
            {
                StatusBox_SelectionChanged(null, null);
            }
        }
        private void LoadNetOrders(List<APISetting> aPIs)
        {
            Dispatcher.Invoke(() => Options.Clear());
            List<IOrder> F = new();
            foreach (APISetting item in aPIs)
            {
                if (item != null && item.Active)
                {
                    List<object> Result = new();
                    switch (item.Type)
                    {
                        case StructLibCore.Marketplace.MarketName.Yandex:
                            Result = new Server.Class.IntegrationSiteApi.Market.Yandex.YandexGetItemOrders.YandexGetItemOrders(item).Get();
                            break;
                        case StructLibCore.Marketplace.MarketName.Ozon:
                            Result = new Server.Class.IntegrationSiteApi.Market.Ozon.OzonPortOrderList.OzonPortOrderList(item).Get();
                            break;
                        case StructLibCore.Marketplace.MarketName.Avito:
                            break;
                        case StructLibCore.Marketplace.MarketName.Sber:
                            break;
                        default:
                            break;
                    }
                    if (Result != null)
                    {
                        Dispatcher.Invoke(() => Options.Add(item));
                        foreach (object t in Result)
                        {
                            F.Add((StructLibCore.Marketplace.IOrder)t);
                        }
                    }//  var X = new Network.Item.MarketApi.GetListOrders().Get<List<object>>(new WrapNetClient(), item);
                }
            }
            Dispatcher.Invoke(() => LoadOrders(F));
        }
        private void Fill_VOrders(object sender, RoutedEventArgs e)
        {
            VisOrderList.Clear();
            Task.Factory.StartNew(() => LoadNetOrders(model.OptionMarketPlace.APISettings));
        }
        private void StatusBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VisOrderList.Clear();
            Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU");
            IEnumerable<IOrder> X = from x in orderList where x.Status == (OrderStatus)StatusBox.SelectedItem orderby x.DeliveryDate select x;
            IEnumerable<IGrouping<DateTime, IOrder>> Z = from x in X group x by DateTime.Parse(x.DeliveryDate, new CultureInfo("ru-RU"));
            Z = Z.OrderByDescending(x => x.Key);
            foreach (IGrouping<DateTime, IOrder> item in Z)
            {
                VisOrderList.Add(item);
            }
            GC.Collect();
        }
        private void ListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Clipboard.SetText(((ListBox)sender).SelectedItem.ToString());
        }
        private void Button_List(object sender, RoutedEventArgs e)
        {
            IEnumerable<IOrder> X = VisOrderList.SelectMany(x => x).Where(x => x.Status == (StructLibCore.Marketplace.OrderStatus)StatusBox.SelectedItem);
            string S = null;
            foreach (IOrder item in X)
            {
                foreach (string t in item.Items)
                {
                    S = S + t + "\t" + item.DeliveryDate + "\t" + "\n";
                }
            }
            Clipboard.SetText(S);
        }
        private void Ship_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            IOrder X = (IOrder)btn.DataContext;
            bool Z = false;
            try
            {
                switch (X.APISetting.Type)
                {
                    case MarketName.Yandex:
                        Z = new SiteApi.IntegrationSiteApi.APIMarket.Yandex.YandexPUTShip(X.APISetting).Get(X);
                        break;
                    case MarketName.Ozon:
                        Z = new SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post.OzonPostShip(X.APISetting).Get(X);
                        break;
                    case MarketName.Avito:
                        break;
                    case MarketName.Sber:
                        break;
                    default:
                        break;
                }
                if (Z)
                {
                    btn.Content = "✅";
                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message);
            }
        }
        private void Label_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            IOrder X = (IOrder)btn.DataContext;
            Browser.Navigate("http://localhost/");
            string Z = null;
            switch (X.APISetting.Type)
            {
                case MarketName.Yandex:
                    Z = new YandexGetLabel(X.APISetting).Get(X);
                    break;
                case MarketName.Ozon:
                    Z = new SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post.OzonPostLabel(X.APISetting).Get(X);
                    break;
                case MarketName.Avito:
                    break;
                case MarketName.Sber:
                    break;
                default:
                    break;
            }
            BNavi(Z);
        }
        private void Browser_hide(object sender, RoutedEventArgs e)
        {
            if (BCol.Width.Value == 200)
            {
                BCol.Width = new GridLength(1);
            }
            else
            {
                BCol.Width = new GridLength(200);
            }
        }
        private void GetActClick(object sender, RoutedEventArgs e)
        {
            Grid btn = (Grid)sender;
            APISetting X = (StructLibCore.Marketplace.APISetting)btn.DataContext;
            Browser.Navigate("http://localhost/");
            string Z = null;
            switch (X.Type)
            {
                case MarketName.Yandex:
                    Z = new YandexGETAct(X).Get();
                    break;
                case MarketName.Ozon:
                    IEnumerable<IOrder> t = from c in orderList where c.APISetting == X && c.Status == OrderStatus.READY select c;
                    List<IOrder> m = t.ToList();
                    if (m.Count > 0) { Z = new SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post.OzonPostAct(X).Get(m); }
                    break;
                case MarketName.Avito:
                    break;
                case MarketName.Sber:
                    break;
                default:
                    break;
            }
            BNavi(Z);
        }
        private void BNavi(string Z)
        {
            string y = AppDomain.CurrentDomain.BaseDirectory;
            if (Z == null || !Z.Contains("none"))
            {
                Browser.Navigate(@"file:///" + y + Z);
            }
            else
            {
                Browser.NavigateToString(Z);
            }
        }
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            VisOrderList.Clear();
            Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU");
            IEnumerable<IOrder> X = from x in orderList where x.Id.Contains(SearchBox.Text) orderby x.DeliveryDate select x;
            IEnumerable<IGrouping<DateTime, IOrder>> Z = from x in X orderby x.DeliveryDate group x by DateTime.Parse(x.DeliveryDate, new CultureInfo("ru-RU"));
            Z = Z.OrderByDescending(x => x.Key);
            if (!Z.Any())
            {
                List<IOrder> R = new List<IOrder>();
                foreach (APISetting item in model.OptionMarketPlace.APISettings)
                {
                    object P = null;
                    switch (item.Type)
                    {
                        case MarketName.Yandex:
                            P = new YandexGetOrder(item).Get(SearchBox.Text);
                            break;
                        case MarketName.Ozon:
                            P = new SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post.OzonPostOrdrInfo(item).Get(SearchBox.Text);
                            break;
                        case MarketName.Avito:
                            break;
                        case MarketName.Sber:
                            break;
                        default:
                            break;
                    }
                    if (P != null)
                    {
                        R.Add((IOrder)P);
                    }
                }
                Z = from x in R orderby x.DeliveryDate group x by DateTime.Parse(x.DeliveryDate, new CultureInfo("ru-RU"));
                Z = Z.OrderByDescending(x => x.Key);
                foreach (IGrouping<DateTime, IOrder> item in Z) { VisOrderList.Add(item); }
            }
            else
            {
                foreach (IGrouping<DateTime, IOrder> item in Z)
                {
                    VisOrderList.Add(item);
                }
            }
            GC.Collect();
        }
        private void ActButton_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Grid Btn = (Grid)sender;
            Btn.Background = new SolidColorBrush(Color.FromRgb(168, 125, 125));
        }

        private void ActButton_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Grid Btn = (Grid)sender;
            Btn.Background = new SolidColorBrush(Color.FromRgb(173, 247, 139));
        }
    }
}
