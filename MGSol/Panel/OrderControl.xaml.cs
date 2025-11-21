using Server.Class.IntegrationSiteApi.Market.Ozon;
using SiteApi.IntegrationSiteApi.ApiBase.Post;
using SiteApi.IntegrationSiteApi.ApiMainSite.Post;
using SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post;
using SiteApi.IntegrationSiteApi.APIMarket.Yandex;
using StructLibCore.Marketplace;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
namespace MGSol.Panel
{
    public partial class OrdersControl : UserControl
    {
        private ObservableCollection<APISetting> Options { get; set; }
        private ObservableCollection<IGrouping<DateTime, IOrder>> VisOrderList { get; set; }
        private MainModel Model { get; set; }
        public event Action RenewEvent;
        public ObservableCollection<IOrder> OrderList { get; set; }
        private ReturnControl ReturnControl { get; set; }
        public OrdersControl(MainModel Model)
        {
            this.Model = Model;
            InitializeComponent();
            Options = new ObservableCollection<APISetting>();
            OrderList = new ObservableCollection<IOrder>();
            VisOrderList = new ObservableCollection<IGrouping<DateTime, IOrder>>();
            OrderStack.ItemsSource = VisOrderList;
            StatusBox.ItemsSource = Enum.GetValues(typeof(OrderStatus));
            PrintActBtnStack.ItemsSource = Options;
            ReturnControl = new ReturnControl(this.Model, this);
            _ = ReturnGrid.Children.Add(ReturnControl);
            _ = Task.Factory.StartNew(() => LoadNetOrders(this.Model.OptionMarketPlace.APISettings));
            _ = Task.Factory.StartNew(() => new MainSiteAutorization("", @"https://sabsb.ru/index.php?route=api/").Autorization());
        }
        private void FillOrders(List<IOrder> orders = null)
        {
            if (orders != null)
            {
                OrderList.Clear();
                for (int i = 0; i < orders.Count - 1; i++)
                {
                    OrderList.Add(orders[i]);
                }
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
            Dispatcher.Invoke(() => OrderList.Clear());
            List<IOrder> F = new();
            foreach (APISetting item in aPIs)
            {
                var T = Task.Factory.StartNew(() =>
                {
                    List<object> Result = null;
                    if (item != null && item.Active)
                    {
                        Result = new();
                        switch (item.Type)
                        {
                            case MarketName.Yandex:
                                Result = new Server.Class.IntegrationSiteApi.Market.Yandex.YandexGetItemOrders.YandexGetItemOrders(item).Get();
                                break;
                            case MarketName.Ozon:
                                Result = new Server.Class.IntegrationSiteApi.Market.Ozon.OzonPortOrderList.OzonPortOrderList(item).Get();
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
                                Dispatcher.Invoke(() => { OrderList.Add((IOrder)t); FillOrders(); });
                            }
                        }
                    }
                });
            }
            RenewEvent();
        }
        private void Fill_VOrders(object sender, RoutedEventArgs e)
        {
            VisOrderList.Clear();
            _ = Task.Factory.StartNew(() => LoadNetOrders(Model.OptionMarketPlace.APISettings));
        }
        private void StatusBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VisOrderList.Clear();
            Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU");
            IEnumerable<IOrder> X = from x in OrderList where x.Status == (OrderStatus)StatusBox.SelectedItem orderby x.DeliveryDate select x;
            IEnumerable<IGrouping<DateTime, IOrder>> Z = from x in X group x by DateTime.Parse(x.DeliveryDate, new CultureInfo("ru-RU"));
            Z = Z.OrderBy(x => x.Key);
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
            IEnumerable<IOrder> X = VisOrderList.SelectMany(x => x).Where(x => x.Status == (OrderStatus)StatusBox.SelectedItem);
            string S = null;
            foreach (IOrder item in X)
            {
                foreach (MarketOrderItems t in item.Items)
                {
                    S = S + t.Count + "\t" + t.Name + "\n";
                }
            }
            Clipboard.SetDataObject(S);
        }
        private void Ship_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            IOrder X = (IOrder)btn.DataContext;
            bool Z = false;
            if (X != null)
            {
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
                        bool AddToBase = new PostOrderBase(Model.BaseInfoPrice.ToketBase, Model.BaseInfoPrice.UriBase).Post(X);
                        if (AddToBase)
                        {
                            X.SetStatus(OrderStatus.READY);
                            StatusBox_SelectionChanged(null, null);
                        }
                        else
                        {
                            MessageBox.Show("Не удалось добавить в базу");
                        }
                        //List<string> ItwmLIst = new();
                        //foreach (MarketOrderItems item in X.Items)
                        //{
                        //    bool v1 = double.TryParse(item.Count, out double Icount);
                        //    bool v = double.TryParse(item.Price, out double Iprice);
                        //    ItwmLIst.Add(item.Sku + "|" + item.Count + "|" + item.Price + "|" + (Icount * Iprice).ToString());
                        //}
                        // string data = DateTime.Now.ToString("dd/MM/yy");
                        // model.ShipmentOrders.Add(new ShipmentOrder() { Date = X.Date, DateShipment = DateTime.Now.ToString(), ID = model.ShipmentOrders.Count + 1.ToString() + data, Nomber = X.Id, Items = ItwmLIst });
                    }
                }
                catch (Exception E)
                {
                    _ = MessageBox.Show(E.Message);
                }
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
        private void GetActClick(object sender, RoutedEventArgs e)
        {
            Control btn = (Control)sender;
            APISetting X = (APISetting)btn.DataContext;
            Browser.Navigate("http://localhost/");
            string Z = null;
            switch (X.Type)
            {
                case MarketName.Yandex:
                    IEnumerable<IOrder> P = OrderList.Where(x => x.APISetting == X && x.Status == OrderStatus.READY);
                    if (P.Count() == 0)
                    {
                        _ = MessageBox.Show("По магазину " + X.Name + " нет заказов");
                    }
                    else
                    {
                        Z = new YandexGETAct(X).Get();
                    }
                    break;
                case MarketName.Ozon:
                    IEnumerable<IOrder> t = from c in OrderList where c.APISetting == X && c.Status == OrderStatus.READY select c;
                    List<IOrder> m = t.ToList();
                    if (m.Count > 0) {
                        //  Z = new SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post.OzonPostAct(X).Get(m);
                        var D = new SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post.OzonPost_v1_carriage_create(X).Get(m);
                    }
                    break;
                case MarketName.Avito:
                    break;
                case MarketName.Sber:
                    break;
                default:
                    break;
            }
            bool CN = Z is not null && Z.ToLower().Contains("error");
            if (CN)
            {
                _ = MessageBox.Show(Z);
            }
            else if (Z != null)
            {
                if (Z == "true")
                {
                    btn.Background = new SolidColorBrush(Color.FromRgb(0, 0, 255));
                }
                else if (Z == "false")
                {
                    btn.Background = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                }
                else
                {
                    BNavi(Z);
                }
            }
        }
        private void BNavi(string Z)
        {
            WebBrowserRow.Height = new GridLength(390);
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
            IEnumerable<IOrder> X = from x in OrderList where x.Id.Contains(SearchBox.Text) orderby x.DeliveryDate select x;
            IEnumerable<IGrouping<DateTime, IOrder>> Z = from x in X orderby x.DeliveryDate group x by DateTime.Parse(x.DeliveryDate, new CultureInfo("ru-RU"));
            Z = Z.OrderBy(x => x.Key);
            if (!Z.Any())
            {
                List<IOrder> R = new();
                foreach (APISetting item in Model.OptionMarketPlace.APISettings)
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
                    if (P != null) { R.Add((IOrder)P); }
                }
                Z = from x in R orderby x.DeliveryDate group x by DateTime.Parse(x.DeliveryDate, new CultureInfo("ru-RU"));
                Z = Z.OrderBy(x => x.Key);
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
            Control Btn = (Control)sender;
            Btn.Background = new SolidColorBrush(Color.FromRgb(168, 125, 125));
        }
        private void ActButton_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Control Btn = (Control)sender;
            Btn.Background = new SolidColorBrush(Color.FromRgb(173, 247, 139));
        }
        private void CopyItemButton_Click(object sender, RoutedEventArgs e)
        {
            MarketOrderItems item = (MarketOrderItems)((Button)sender).DataContext;
            string S = null;
            S = S + item.Count + "шт.\t" + item.Name;
            Clipboard.SetDataObject(S);
        }
        private void GetImage_Click(object sender, RoutedEventArgs e)
        {
            Button Btn = (Button)sender;
            Btn.IsEnabled = false;
            StackPanel St = (StackPanel)((StackPanel)((StackPanel)((StackPanel)((StackPanel)Btn.Parent).Parent).Parent).Parent).Children[1];
            MarketOrderItems OrderItem = (MarketOrderItems)((Button)sender).DataContext;
            switch (OrderItem.Order.APISetting.Type)
            {
                case MarketName.Yandex:
                    break;
                case MarketName.Ozon:
                    Task.Factory.StartNew(() =>
                    {
                        IMarketItem item = (IMarketItem)new OzonPostItemDesc(OrderItem.Order.APISetting).Get(new List<string> { OrderItem.Sku })[0];
                        foreach (string X in item.Pic)
                        {
                            Dispatcher.BeginInvoke(() => AddImage(X, St));
                        }
                    });
                    break;
                case MarketName.Avito:
                    break;
                case MarketName.Sber:
                    break;
                default:
                    break;
            }
        }
        private void HeadStack_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                StackPanel p = (StackPanel)sender;
                var order = (IOrder)p.DataContext;
                Task.Factory.StartNew(() =>
                {
                    if (order.IMtemsList == null)
                    {
                        order.IMtemsList = new List<IMarketItem>();
                        switch (order.APISetting.Type)
                        {
                            case MarketName.Yandex:
                                break;
                            case MarketName.Ozon:
                                List<string> list = new List<string>();
                                foreach (var PItem in order.Items)
                                {
                                    list.Add(PItem.Sku);
                                }
                                order.IMtemsList = (List<IMarketItem>)new OzonPostItemDesc(order.APISetting).Get(list);
                                if (Dispatcher.Invoke(() => LoadImageCheck.IsChecked == true))
                                {
                                    LoadItmAndPicOzon(order);
                                }
                                break;
                            case MarketName.Avito:
                                break;
                            case MarketName.Sber:
                                break;
                            default:
                                break;
                        }
                    }
                    Dispatcher.Invoke(() => MainOrderBoard.DataContext = order);
                });
                WebBrowserRow.Height = new GridLength(0);
                Border F = (Border)p.Parent;
                StackPanel Z = (StackPanel)F.Parent;
                StackPanel T = (StackPanel)Z.Children[1];
                T.Visibility = T.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
                void LoadItmAndPicOzon(IOrder order)
                {
                    foreach (var PItem in order.Items)
                    {
                        Task.Factory.StartNew(() =>
                        PItem.Image = GetBitmap(order.IMtemsList.FirstOrDefault(x => x.SKU == PItem.Sku).Pic[0]
                        ));
                    }
                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message);
            }
        }
        private void Barcode_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            if (btn.Name == "DelBarcode")
            {
                var barcode = btn.DataContext.ToString();
                foreach (var item in OrderList)
                {
                    if (item.IMtemsList != null)
                    {
                        var Itm = item.IMtemsList.FirstOrDefault(x => x.Barcodes.Contains(barcode));
                        if (Itm != null)
                        {
                            Itm.Barcodes.Remove(barcode);
                            // var R = new Server.Class.IntegrationSiteApi.Market.Ozon.OzonPostSetItem(Itm.APISetting).Get(new IMarketItem[] { Itm });
                        }
                    }
                }
            }
            else if (btn.Name == "AddBarcode")
            {
                var mdbox = new ModalBox();
                var order = ((MarketOrderItems)btn.DataContext).Order;
                var sku = ((MarketOrderItems)btn.DataContext).Sku;
                IMarketItem itm = ((MarketOrderItems)btn.DataContext).Order.IMtemsList?.FirstOrDefault(x => x.SKU == sku);
                if (mdbox.ShowDialog() == true)
                {
                    itm.Barcodes.Add(mdbox._STR);
                    itm.APISetting = order.APISetting;
                }
                var R = new Server.Class.IntegrationSiteApi.Market.Ozon.OzonPostSetItem(order.APISetting).Get(new IMarketItem[] { itm });
                //  ShowStatus.DataContext = order.APISetting;
                // ShowStatus.Content = R;
            }
        }
        private void ShowStatus_Click(object sender, RoutedEventArgs e)
        {
            var btn = (TextBox)sender;
            string id = GetId(btn.Text);
            if (id != "")
            {
                var DD = new OzonPOSTShowStatusImport(((StructLibCore.Marketplace.MarketOrderItems)btn.DataContext).Order.APISetting).Get(id);
            }
            string GetId(string str)
            {
                string result = null;
                foreach (char item in str)
                {
                    if (Char.IsDigit(item))
                    {
                        result = result + item;
                    }
                }
                return result;
            }
        }
        void AddImage(string url, StackPanel St)
        {
            try
            {
                var bi = GetBitmap(url);
                Image img = new()
                {
                    Width = 100,
                    Height = 100,
                    Source = bi,
                    DataContext = bi
                };
                img.MouseLeftButtonDown += (s, e) =>
                {
                    var win = new Window();
                    Image img = (Image)s;
                    Image newImg = new()
                    {
                        Width = SystemParameters.PrimaryScreenWidth / 3,
                        Height = SystemParameters.PrimaryScreenWidth / 3,
                        Source = (BitmapImage)img.DataContext
                    };
                    win.Content = newImg;
                    win.Width = SystemParameters.PrimaryScreenWidth / 3;
                    win.Height = SystemParameters.PrimaryScreenWidth / 3;
                    win.Show();
                    win.Activate();
                };
                St.Children.Insert(0, img);
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message);
            }
        }
        BitmapImage GetBitmap(string url)
        {
            WebClient c = new();
            byte[] bytes = c.DownloadData(url);
            MemoryStream ms = new(bytes);
            BitmapImage bi = new();
            bi.BeginInit();
            bi.StreamSource = ms;
            bi.EndInit();
            return bi;
        }
        private void TextBlock_Initialized(object sender, EventArgs e)
        {
            StackPanel ST = (StackPanel)((TextBlock)sender).Parent;
            StackPanel PT = (StackPanel)((StackPanel)((StackPanel)((TextBlock)sender).Parent).Parent).Parent;
            var X = ((StructLibCore.Marketplace.MarketOrderItems)ST.DataContext).Order.IMtemsList;
            if (LoadImageCheck.IsChecked == true)
            {
                foreach (var item in X)
                {
                    AddImage(item.Pic[0], PT);
                }
            }
        }
        private void ItemBox_Initialized(object sender, EventArgs e)
        {
        }
    }
}
