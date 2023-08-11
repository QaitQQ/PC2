using SiteApi.IntegrationSiteApi.APIMarket.Yandex.YandexPUTStocks;

using StructLibCore;
using StructLibCore.Marketplace;

using StructLibs;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Xml.Serialization;
namespace MGSol.Panel
{
    /// <summary>
    /// Логика взаимодействия для ItemControl.xaml
    /// </summary>
    public partial class ItemControl : UserControl
    {
        private List<MarketItem> ItemsList;
        private ObservableCollection<VisMarketItem> VisItemsList { get; set; }
        private Выгрузка items1C;
        private List<WS> WCList { get; set; }
        private List<APISetting> Options { get; set; }
        private MainModel Model { get; set; }
        private List<StructLibCore.IC> StorageList { get; set; }
        private List<СomparisonNameID> ComparisonsNames { get; set; }
        public ItemControl(MainModel model)
        {
            Model = model;
            InitializeComponent();
            VisItemsList = new ObservableCollection<VisMarketItem>();
            Options = model.OptionMarketPlace.APISettings;
            //  _ = System.Threading.Tasks.Task.Factory.StartNew(() => Dispatcher.Invoke(() => LoadList()));
            ItemsList = new List<MarketItem>();
            VItemsList.ItemsSource = VisItemsList;
            SortedBox.ItemsSource = typeof(MarketItem).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            ApiBox.ItemsSource = model.GetApi();
            foreach (APISetting item in Options)
            {
                if (item != null)
                {
                    _ = ProcessingPanelApiBox.Items.Add(item.Name);
                }
            }
        }
        #region Кнопки
        private void SyncItem_Button_Click(object sender, RoutedEventArgs e)
        {
            VisItemsList.Clear();
            _ = System.Threading.Tasks.Task.Factory.StartNew(() => SyncItemList());
        }
        private void Renew_click(object sender, RoutedEventArgs e)
        {
            IMarketItem[] mass = new IMarketItem[] { ((Control)sender).DataContext as IMarketItem };
            RenewPrice(mass);
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            ContextMenu M = new() { DataContext = (IMarketItem)((StackPanel)((Button)sender).Parent).DataContext };
            foreach (APISetting item in Options)
            {
                TextBlock X = new() { Text = item.Name, DataContext = item };
                X.MouseLeftButtonDown += (x, y) =>
                {
                    IMarketItem X = (IMarketItem)((ContextMenu)((TextBlock)x).Parent).DataContext;
                    X.APISettingSource = X.APISetting;
                    X.APISetting = (APISetting)((TextBlock)x).DataContext;
                    IMarketItem[] mass = new IMarketItem[] { X };
                    string P = ImportItems(mass);
                    IEnumerable<VisMarketItem> Z = from o in VisItemsList where o.SKU == X.SKU select o;
                    foreach (VisMarketItem item in Z)
                    {
                        item.Items.Add(new UniMarketItem() { APISetting = X.APISetting, SKU = X.SKU, Price = P });
                    }
                };
                _ = M.Items.Add(X);
            }
            M.IsOpen = true;
        }
        private void MiniClick(object sender, RoutedEventArgs e)
        {
            Grid grid = (System.Windows.Controls.Grid)((StackPanel)((Label)sender).Parent).Parent;
            grid.Children[1].Visibility = grid.Children[1].Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }
        private void Button_Save(object sender, RoutedEventArgs e)
        {
            Model.OptionMarketPlace.MarketItems = ItemsList;
            Model.Save();
            //new Network.Item.MarketApi.SaveItemsList().Get<bool>(new Client.WrapNetClient(), ItemsList.ToArray().ToList());
        }
        private void LoadBaseItem_Click(object sender, RoutedEventArgs e)
        {
            XmlSerializer xmlSerializer = new(typeof(Выгрузка));
            using FileStream fs = new("Выгрузка.xml", FileMode.OpenOrCreate); items1C = xmlSerializer.Deserialize(fs) as Выгрузка;
        }
        private void RemClick(object sender, RoutedEventArgs e)
        {
            _ = ItemsList.Remove((MarketItem)((System.Windows.Controls.Button)sender).DataContext);
            Fill_V_list(ItemsList);
        }
        #endregion
        #region Поиск и сортировка
        private void Find_fill(object sender, RoutedEventArgs e)
        {
            IEnumerable<MarketItem> selectionItem = from lst in ItemsList where lst.Name != null && (lst.Name.ToLower().Contains(FindField.Text.ToLower()) || lst.SKU.Contains(FindField.Text)) select lst;
            Fill_V_list(selectionItem);
        }
        private void Fill_V_list(IEnumerable<MarketItem> selectionItem)
        {
            VisItemsList.Clear();

            foreach (MarketItem item in selectionItem)
            {
                VisItemsList.Add(new VisMarketItem(item, WCList));
            }

            GC.Collect();
        }
        private void ItemSorted(object sender, SelectionChangedEventArgs e)
        {
            List<MarketItem> nlist = new List<MarketItem>();
            foreach (var item in VisItemsList)
            {
                nlist.Add(item.Item);
            }

            switch (((ComboBox)sender).SelectedItem.ToString())
            {
                case "System.String name":
                    Fill_V_list(from lst in nlist orderby lst.Name select lst);
                    break;
                case "System.String SKU":
                    Fill_V_list(from lst in nlist orderby lst.SKU select lst);
                    break;
                case "System.Collections.ObjectModel.ObservableCollection`1[StructLibCore.Marketplace.IMarketItem] Items":    
                    Fill_V_list(selectionItem: from lst in nlist orderby lst.Items?.Count descending select lst);
                    break;                   
                default:
                    break;
            }
        }
        #endregion
        private void SyncItemList()
        {
            foreach (MarketItem item in ItemsList)
            {
                item.Items = new ObservableCollection<IMarketItem>();
            }
            ItemsList = Model.OptionMarketPlace.MarketItems;
            SyncStorages();
            foreach (APISetting Option in Options)
            {
                _ = Task.Factory.StartNew(() =>
                {
                    List<object> Result = null;
                    if (Option.Active)
                    {
                        switch (Option.Type)
                        {
                            case MarketName.Yandex:
                                Result = new Server.Class.IntegrationSiteApi.Market.Yandex.YandexGetName.YandexGetItemList(Option).Get();
                                break;
                            case MarketName.Ozon:
                                Result = new Server.Class.IntegrationSiteApi.Market.Ozon.OzonPostItemDesc(Option).Get();
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
                            foreach (object item in Result)
                            {
                                IMarketItem It = (IMarketItem)item;
                                It.APISetting = Option;
                                MarketItem X = null;
                                X = ItemsList.FirstOrDefault(x => x.SKU == It.SKU);
                                if (X != null && X.Name == null) { X.Name = It.Name; }
                                if (X != null)
                                {
                                    Dispatcher.Invoke(() => { if (X.Items == null) { X.Items = new ObservableCollection<IMarketItem>(); } X.Items.Add(It); });
                                }
                                else
                                {
                                    X = ItemsList.FirstOrDefault(x => x.Name == It.Name);
                                    if (X == null)
                                    {
                                        Dispatcher.Invoke(() =>
                                        {
                                            ItemsList.Add(new MarketItem()
                                            {
                                                SKU = It.SKU,
                                                Name = It.Name,
                                                Items = new ObservableCollection<IMarketItem>() { It }
                                            });
                                        });
                                    }
                                    else if (X != null)
                                    {
                                        Dispatcher.Invoke(() => { if (X.Items == null) { X.Items = new ObservableCollection<IMarketItem>(); } X.Items.Add(It); });
                                    }
                                }
                            }
                        }
                    }
                    Dispatcher.Invoke(() =>
                    {
                        Fill_V_list(ItemsList);
                    });
                });
            }
        }
        private static string ImportItems(IMarketItem[] mass)
        {
            IMarketItem[] X = mass;
            static List<IGrouping<APISetting, IMarketItem>> ConvertListApi(IMarketItem[] Lst)
            {
                IEnumerable<IGrouping<APISetting, IMarketItem>> X = Lst.GroupBy(x => x.APISetting);
                List<IGrouping<APISetting, IMarketItem>> A = X.ToList();
                return A;
            }
            List<IGrouping<APISetting, IMarketItem>> Z = ConvertListApi(X);
            object R = null;
            foreach (IGrouping<APISetting, IMarketItem> item in Z)
            {
                switch (item.Key.Type)
                {
                    case MarketName.Yandex:
                        R = new SiteApi.IntegrationSiteApi.APIMarket.Yandex.YandexPostImport.YandexPostImport(item.Key).Get(item.ToArray());
                        break;
                    case MarketName.Ozon:
                        R = new Server.Class.IntegrationSiteApi.Market.Ozon.OzonPostSetItem(item.Key).Get(item.ToArray());
                        break;
                    case MarketName.Avito:
                        break;
                    case MarketName.Sber:
                        break;
                    default:
                        break;
                }
            }
            return R.ToString();
        }
        private static void RenewPrice(IMarketItem[] mass)
        {
            IMarketItem[] X = mass;
            static List<IGrouping<APISetting, IMarketItem>> ConvertListApi(IMarketItem[] Lst)
            {
                IEnumerable<IGrouping<APISetting, IMarketItem>> X = Lst.GroupBy(x => x.APISetting);
                List<IGrouping<APISetting, IMarketItem>> A = X.ToList();
                return A;
            }
            List<IGrouping<APISetting, IMarketItem>> Z = ConvertListApi(X);
            object P = null;
            object S = null;
            foreach (IGrouping<APISetting, IMarketItem> item in Z)
            {
                switch (item.Key.Type)
                {
                    case MarketName.Yandex:
                        P = new Server.Class.IntegrationSiteApi.Market.Yandex.YandexPostItemPrice.YandexPostItemPrice(item.Key).Get(item.ToArray());
                        S = new YandexPUTStocks(item.Key).Get(item.ToArray());
                        break;
                    case MarketName.Ozon:
                        P = new Server.Class.IntegrationSiteApi.Market.Ozon.OzonPostSetPrice(item.Key).Get(item.ToArray());
                        S = new Server.Class.IntegrationSiteApi.Market.Ozon.OzonPostSetStoks(item.Key).Get(item.ToArray());
                        break;
                    case MarketName.Avito:
                        break;
                    case MarketName.Sber:
                        break;
                    default:
                        break;
                }
            }
        }
        private static void AddBtn(ContextMenu menu, string BtnCont, MouseButtonEventHandler handler)
        {
            Label X = new()
            {
                Content = BtnCont
            };
            X.MouseLeftButtonDown += handler;
            _ = menu.Items.Add(X);
        }
        private void SearchItemIn1CBase_Click(object sender, RoutedEventArgs e)
        {
            MarketItem X = (MarketItem)((Button)sender).DataContext;
            ContextMenu M = new();
            if (items1C == null)
            {
                LoadBaseItem_Click(null, null);
            }
            IEnumerable<Номенклатура> T = from O in items1C?.Номенклатура where O.Наименование.ToLower().Contains(X.Art1C) select O;
            foreach (Номенклатура item in items1C.Номенклатура)
            {
                if (item.Наименование.ToLower().Contains(X.Art1C.ToLower()))
                {
                    AddBtn(M, item.Наименование, (e, x) => { X.Art1C = item.Код.ToString(); }); ;
                }
            }
            M.IsOpen = true;
        }
        private void BaseIDBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            MarketItem X = (MarketItem)((TextBox)sender).DataContext;
            if (X.BaseID == 0)
            {
                List<СomparisonNameID> Search = new Network.Item.ItemSearch().Get<List<СomparisonNameID>>(Model.GetClient(), new object[] { ((System.Windows.Controls.TextBox)sender).Text, 3 });
                ContextMenu M = new();
                for (int i = 0; i < 10; i++)
                {
                    AddBtn(M, Search[i].Name, (e, x) => { X.BaseID = Search[i].Id; (sender as TextBox).Text = Search[i].Id.ToString(); }); ;
                }
                M.IsOpen = true;
            }
        }
        private void TextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Label)sender).Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(Color.Green.R, Color.Green.G, Color.Green.B));
        }
        private void TextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Label)sender).Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(Color.Gray.R, Color.Gray.G, Color.Gray.B));
        }
        private void TextBlockStocks_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                ContextMenu menu = new();
                TextBlock TX = (TextBlock)sender;
                string N = TX.Name;
                IMarketItem item = (IMarketItem)TX.DataContext;
                TextBox Box = new()
                {
                    Width = 50
                };
                switch (N)
                {
                    case "TextBlockPrice":
                        Box.Text = item.Price;
                        break;
                    case "TextBlockStocks":
                        Box.Text = item.Stocks;
                        break;
                    case "TextBlockMinPrice":
                        Box.Text = item.MinPrice;
                        break;
                    default:
                        break;
                }
                _ = menu.Items.Add(Box);
                menu.IsOpen = true;
                _ = Box.Focus();
                Box.Select(0, Box.Text.Length);
                menu.Closed += (e, s) =>
                {
                    if (Box.Text is not null and not "")
                    {
                        TX.Text = Box.Text;
                        switch (N)
                        {
                            case "TextBlockPrice":
                                item.Price = Box.Text;
                                break;
                            case "TextBlockStocks":
                                item.Stocks = Box.Text;
                                break;
                            case "TextBlockMinPrice":
                                item.MinPrice = Box.Text;
                                break;
                            default:
                                break;
                        }
                    }
                };
            }
        }
        private async void SyncStorages()
        {
            SiteStoregeStruct P = null;
            bool Loading = false;
            using (HttpClient client = new())
            {

                try
                {
                    using HttpResponseMessage response = await client.GetAsync("https://salessab.su/STList.xml");
                    using Stream streamToReadFrom = await response.Content.ReadAsStreamAsync();
                    XmlSerializer serializer = new(typeof(SiteStoregeStruct));
                    P = (SiteStoregeStruct)serializer.Deserialize(streamToReadFrom);
                    WCList = P.Warehouses;
                    StorageList = P.ItmsCount;
                    Loading = true;
                }
                catch (Exception e)
                {

                    MessageBox.Show(e.Message);
                }


            }
            if (Loading)
            {
                foreach (MarketItem item in ItemsList)
                {
                    item.StoregeList = P.ItmsCount.FindAll(x => x.Id == item.BaseID).ToList();
                }
            }

        }
        private async void DownloadName_Click(object sender, RoutedEventArgs e)
        {
            using HttpClient client = new();
            using HttpResponseMessage response = await client.GetAsync("https://salessab.su/ComNameList.xml");
            using Stream streamToReadFrom = await response.Content.ReadAsStreamAsync();
            XmlSerializer serializer = new(typeof(List<СomparisonNameID>));
            ComparisonsNames = (List<СomparisonNameID>)serializer.Deserialize(streamToReadFrom);
        }
        private void BaseIDBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                ContextMenu menu = new();
                TextBox Box = new();
                Control TX = (Control)sender;
                MarketItem MITEM = ((VisMarketItem)TX.DataContext).Item;
                Box.Width = 50;
                _ = menu.Items.Add(Box);
                menu.IsOpen = true;
                _ = Box.Focus();
                Box.Select(0, Box.Text.Length);
                ContextMenu menu2 = new();
                if (ComparisonsNames == null)
                {
                    DownloadName_Click(null, null);
                }
                menu.Closed += (e, s) =>
                {
                    if (Box.Text.Length >= 3)
                    {
                        if (ComparisonsNames != null)
                        {
                            List<СomparisonNameID> Find = ComparisonsNames.FindAll(x => x.СomparisonName.Contains(Box.Text.ToLower()));
                            if (Find != null && Find.Count > 0 && Find.Count < 30)
                            {
                                foreach (СomparisonNameID item in Find)
                                {
                                    var Fl = StorageList.FirstOrDefault(x => x.Id == item.Id);
                                    var TxTName = item.Name;
                                    if (Fl != null)
                                    {
                                        TxTName = TxTName + " " + Fl.C;
                                    }
                                    TextBlock block = new()
                                    {
                                        Text = TxTName,
                                        DataContext = item
                                    };
                                    block.MouseDown += (sender, e) => { MITEM.BaseID = ((СomparisonNameID)((TextBlock)sender).DataContext).Id; Box.Text = MITEM.BaseID.ToString(); };
                                    _ = menu2.Items.Add(block);
                                }
                            }
                            menu2.IsOpen = true;
                        }
                        else { _ = MessageBox.Show("Не загружены имена"); }
                    }
                };
            }
        }
        private async void FindText(TextBox Box, ContextMenu menu2, bool repeat)
        {
            if (Box.Text.Length > 4)
            {
                await Task.Delay(2000);
                Dispatcher.Invoke(() =>
                {
                    if (ComparisonsNames != null)
                    {
                        List<СomparisonNameID> Find = ComparisonsNames.FindAll(x => x.СomparisonName.Contains(Box.Text));
                        if (Find != null && Find.Count > 0 && Find.Count < 10)
                        {
                            foreach (СomparisonNameID item in Find)
                            {
                                TextBlock block = new()
                                {
                                    Text = item.Name
                                };
                                _ = menu2.Items.Add(block);
                            }
                        }
                        menu2.IsOpen = true;
                    }
                });
                if (repeat)
                {
                    FindText(Box, menu2, false);
                }
            }
        }
        private void Art1CBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                ContextMenu menu = new();
                TextBox Box = new();
                Label TX = (Label)sender;
                MarketItem MITEM = ((VisMarketItem)TX.DataContext).Item;
                Box.Width = 50;
                _ = Box.Focus();
                Box.Select(0, Box.Text.Length);
                menu.Closed += (e, s) => { MITEM.Art1C = Box.Text; };
                _ = menu.Items.Add(Box);
                menu.IsOpen = true;
            }
        }
        #region AutoProcessing
        private void PlusPricePercent(object sender, RoutedEventArgs e)
        {
            List<IMarketItem> mass = new();
            foreach (VisMarketItem I in VisItemsList)
            {
                MarketItem item = I.Item;
                if (I.Checked)
                {
                    if (item.Items is not null)
                    {
                        foreach (IMarketItem X in item.Items)
                        {
                            if (X.Price != null)
                            {
                                if (ProcessingPanelApiBox.SelectedItem != null)
                                {
                                    if (X.APISetting.Name == ProcessingPanelApiBox.SelectedItem.ToString())
                                    {
                                        if (X.Price.Contains('.'))
                                        {
                                            X.Price = X.Price.Replace(".", ",");
                                        }
                                        double Z = double.Parse(X.Price, System.Globalization.NumberStyles.AllowDecimalPoint);
                                        double P = double.Parse(ProcessingPanelPercentBox.Text);
                                        X.Price = (((P / 100) + 1) * Z).ToString();
                                        mass.Add(X);
                                    }
                                }
                                else
                                {

                                    if (X.Price.Contains('.'))
                                    {
                                        X.Price = X.Price.Replace(".", ",");
                                    }
                                    double Z = double.Parse(X.Price, System.Globalization.NumberStyles.AllowDecimalPoint);
                                    double P = double.Parse(ProcessingPanelPercentBox.Text);
                                    X.Price = (((P / 100) + 1) * Z).ToString();
                                    mass.Add(X);
                                }

                            }
                        }
                    }
                }
            }
            RenewPrice(mass.ToArray());
        }
        private void MinusPricePercent(object sender, RoutedEventArgs e)
        {
            List<IMarketItem> mass = new();
            foreach (VisMarketItem I in VisItemsList)
            {
                if (I.Checked)
                {
                    MarketItem item = I.Item;
                    foreach (IMarketItem X in item.Items)
                    {
                        if (ProcessingPanelApiBox.SelectedItem != null && X.APISetting.Name == ProcessingPanelApiBox.SelectedItem.ToString())
                        {
                            if (X.Price.Contains('.')) { X.Price = X.Price.Replace(".", ","); }
                            double Z = double.Parse(X.Price, System.Globalization.NumberStyles.AllowDecimalPoint);
                            double P = double.Parse(ProcessingPanelPercentBox.Text);
                            X.Price = (Z - (P / 100 * Z)).ToString();
                            mass.Add(X);
                        }
                    }
                }
            }
            RenewPrice(mass.ToArray());
        }
        private void ConversionToRC_Button_Click(object sender, RoutedEventArgs e)
        {
            List<IMarketItem> mass = new();
            foreach (VisMarketItem I in VisItemsList)
            {
                if (I.Checked)
                {
                    MarketItem item = I.Item;
                    foreach (IMarketItem X in item.Items)
                    {
                        if (item.Price != 0)
                        {
                            X.Price = item.Price.ToString();
                            X.MinPrice = (item.Price * 0.95).ToString();
                            mass.Add(X);
                        }
                    }
                }
            }
            RenewPrice(mass.ToArray());
        }
        #endregion
        private void NullStack_Button_Click(object sender, RoutedEventArgs e)
        {
            List<IMarketItem> mass = new();
            foreach (VisMarketItem I in VisItemsList)
            {
                if (I.Checked)
                {
                    MarketItem item = I.Item;
                    foreach (IMarketItem X in item.Items)
                    {
                        if (X.Stocks is not "0" and not "" and not null)
                        {
                            X.Stocks = "0";
                            mass.Add(X);
                        }
                    }
                }
            }
            RenewPrice(mass.ToArray());
        }
        private void MarkAll_Click(object sender, RoutedEventArgs e)
        {
            ((CheckBox)sender).IsChecked = true;
            foreach (VisMarketItem item in VisItemsList)
            {
                item.Checked = true;
            }
        }
        private void UnmarkAll_Click(object sender, RoutedEventArgs e)
        {
            ((CheckBox)sender).IsChecked = false;
            foreach (VisMarketItem item in VisItemsList)
            {
                item.Checked = false;
            }
        }
        private void CopyName_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Clipboard.SetDataObject(((MGSol.Panel.ItemControl.VisMarketItem)((TextBlock)sender).DataContext).Name);
        }
        private void ApiBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IEnumerable<MarketItem> selectionItem = from lst in ItemsList where lst.Items.FirstOrDefault(x => x.APISetting.Name == ApiBox.SelectedItem.ToString()) != null select lst;
            Fill_V_list(selectionItem);
        }
        private void DownloadItemPicsButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            GetImage((Control)sender);
        }
        private static void GetImage(Control sender)
        {
            Control Btn = sender;
            Btn.IsEnabled = false;
            StackPanel PR1 = (StackPanel)Btn.Parent;
            IMarketItem OrderItem = (IMarketItem)sender.DataContext;
            switch (OrderItem.APISetting.Type)
            {
                case MarketName.Yandex:
                    foreach (string X in OrderItem.Pic)
                    {
                        AddImage(X, OrderItem);
                    }
                    break;
                case MarketName.Ozon:
                    foreach (string X in OrderItem.Pic)
                    {
                        AddImage(X, OrderItem);
                    }
                    break;
                case MarketName.Avito:
                    break;
                case MarketName.Sber:
                    break;
                default:
                    break;
            }
            void AddImage(string url, IMarketItem Item)
            {
                WebClient c = new();
                byte[] bytes = c.DownloadData(url);
                MemoryStream ms = new(bytes);
                BitmapImage bi = new();
                bi.BeginInit();
                bi.StreamSource = ms;
                bi.EndInit();
                System.Windows.Controls.Image img = new()
                {
                    DataContext = new object[] { url, Item },
                    Width = 100,
                    Height = 100,
                    Source = bi
                };
                img.MouseLeftButtonDown += (s, e) =>
                {
                    System.Windows.Controls.Image img = (System.Windows.Controls.Image)s;
                    if (img.Width == 100)
                    {
                        img.Width = SystemParameters.PrimaryScreenWidth / 3; //img.Source.Width;
                        img.Height = SystemParameters.PrimaryScreenHeight / 3;
                    }
                    else
                    {
                        img.Width = 100;
                        img.Height = 100;
                    }
                };
                img.MouseRightButtonDown += (s, e) =>
                {
                    ContextMenu CM = new()
                    {
                        DataContext = s
                    };
                    AddBtn(CM, "Удалить", (s, e) =>
                    {
                        System.Windows.Controls.Image image = (System.Windows.Controls.Image)((System.Windows.Controls.Label)s).DataContext;
                        object[] DM = (object[])image.DataContext;
                        IMarketItem item = (IMarketItem)DM[1];
                        string url = (string)DM[0];
                        _ = item.Pic.Remove(url);
                        _ = ImportItems(new IMarketItem[] { item });
                        ((StackPanel)image.Parent).Children.Remove(image);
                    });
                    CM.IsOpen = true;
                };
                _ = PR1.Children.Add(img);
            }
        }
        private void MarketItemBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var T = (Control)sender;
            var S = (StackPanel)T.Parent;
            foreach (var item in S.Children)
            {
                if (item is Grid && (item as Grid).Name == "HiddenGrid")
                {
                    if ((item as Grid).Visibility == Visibility.Collapsed)
                    {
                        S.Width = 500;
                        (item as Grid).Visibility = Visibility.Visible;
                    }
                    else
                    {
                        S.Width = 100;
                        (item as Grid).Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        private void PlusStack_Button_Click(object sender, RoutedEventArgs e)
        {
            List<IMarketItem> mass = new();
            foreach (VisMarketItem I in VisItemsList)
            {
                if (I.Checked)
                {
                    MarketItem item = I.Item;
                    foreach (IMarketItem X in item.Items)
                    {

                            X.Stocks = ProcessingPanelPercentBox.Text;
                            mass.Add(X);
                        
                    }
                }
            }
            RenewPrice(mass.ToArray());
        }
    }
}
