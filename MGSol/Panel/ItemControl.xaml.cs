using NPOI.HSSF.EventUserModel;

using StructLibCore;
using StructLibCore.Marketplace;

using StructLibs;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
        private List<СomparisonNameID> сomparisonNames { get; set; }
        public ItemControl(MainModel model)
        {
            Model = model;
            InitializeComponent();
            VisItemsList = new ObservableCollection<VisMarketItem>();
            Options = model.OptionMarketPlace.APISettings;
            System.Threading.Tasks.Task.Factory.StartNew(() => Dispatcher.Invoke(() => LoadList()));
            VItemsList.ItemsSource = VisItemsList;
            SortedBox.ItemsSource = typeof(MarketItem).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (APISetting item in Options)
            {
                if (item != null)
                {
                    ProcessingPanelApiBox.Items.Add(item.Name);
                }
            }
        }
        #region Кнопки
        private void SyncItem_Button_Click(object sender, RoutedEventArgs e)
        {
            VisItemsList.Clear();
            System.Threading.Tasks.Task.Factory.StartNew(() => SyncItemList());
        }
        private void Renew_click(object sender, RoutedEventArgs e)
        {
            IMarketItem[] mass = new IMarketItem[] { ((TextBlock)sender).DataContext as IMarketItem };
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
                M.Items.Add(X);
            }
            M.IsOpen = true;
        }
        private void MiniClick(object sender, RoutedEventArgs e)
        {
            Grid grid = (((System.Windows.Controls.Grid)(((StackPanel)((TextBlock)sender).Parent).Parent)));
            if (grid.Children[1].Visibility == Visibility.Collapsed)
            {
                grid.Children[1].Visibility = Visibility.Visible;
            }
            else
            {
                grid.Children[1].Visibility = Visibility.Collapsed;
            }
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
            ItemsList.Remove((MarketItem)((System.Windows.Controls.Button)sender).DataContext);
            Fill_Vlist(ItemsList);
        }
        #endregion
        #region Поиск и сортировка
        private void Find_fill(object sender, RoutedEventArgs e)
        {
            IEnumerable<MarketItem> selectionItem = from lst in ItemsList where lst.Name != null && (lst.Name.ToLower().Contains(FindField.Text.ToLower()) || lst.SKU.Contains(FindField.Text)) select lst;
            Fill_Vlist(selectionItem);
        }
        private void Fill_Vlist(IEnumerable<MarketItem> selectionItem)
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
            switch (((System.Windows.Controls.ComboBox)sender).SelectedItem.ToString())
            {
                case "System.String name":
                    Fill_Vlist(from lst in ItemsList orderby lst.Name select lst);
                    break;
                case "System.String SKU":
                    Fill_Vlist(from lst in ItemsList orderby lst.SKU select lst);
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
            SyncStoreges();
            foreach (APISetting Option in Options)
            {
                
                Task.Factory.StartNew(() =>
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
                                Result = new Server.Class.IntegrationSiteApi.Market.Ozon.OzonGetItemDesc(Option).Get();
                               
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
                                if (X != null) { Dispatcher.Invoke(() => { X.Items.Add(It); }); }
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
                                        Dispatcher.Invoke(() => { X.Items.Add(It); });
                                    }
                                }
                            }
                        }
                    }
                    Dispatcher.Invoke(() =>
                    {
                        Fill_Vlist(ItemsList);
                    });
                });
            }
        }
        private void LoadList()
        {
            ItemsList = Model.OptionMarketPlace.MarketItems;
            if (ItemsList != null)
            {
                foreach (MarketItem item in ItemsList)
                {
                    item.Items = new ObservableCollection<IMarketItem>();
                    VisItemsList.Add(new VisMarketItem(item, WCList));
                }
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
            static List<IGrouping<StructLibCore.Marketplace.APISetting, StructLibCore.Marketplace.IMarketItem>> ConvertListApi(StructLibCore.Marketplace.IMarketItem[] Lst)
            {
                IEnumerable<IGrouping<StructLibCore.Marketplace.APISetting, StructLibCore.Marketplace.IMarketItem>> X = Lst.GroupBy(x => x.APISetting);
                List<IGrouping<StructLibCore.Marketplace.APISetting, StructLibCore.Marketplace.IMarketItem>> A = X.ToList();
                return A;
            }
            List<IGrouping<APISetting, IMarketItem>> Z = ConvertListApi(X);
            object P = null;
            object S = null;
            foreach (IGrouping<APISetting, IMarketItem> item in Z)
            {
                switch (item.Key.Type)
                {
                    case StructLibCore.Marketplace.MarketName.Yandex:
                        P = new Server.Class.IntegrationSiteApi.Market.Yandex.YandexPostItemPrice.YandexPostItemPrice(item.Key).Get(item.ToArray());
                        break;
                    case StructLibCore.Marketplace.MarketName.Ozon:
                        P = new Server.Class.IntegrationSiteApi.Market.Ozon.OzonPostSetPrice(item.Key).Get(item.ToArray());
                        S = new Server.Class.IntegrationSiteApi.Market.Ozon.OzonPostSetStoks(item.Key).Get(item.ToArray());
                        break;
                    case StructLibCore.Marketplace.MarketName.Avito:
                        break;
                    case StructLibCore.Marketplace.MarketName.Sber:
                        break;
                    default:
                        break;
                }
            }
        }
        private static void AddBtn(ContextMenu menu, string BtnCont, MouseButtonEventHandler handler)
        {
            Label X = new();
            X.Content = BtnCont;
            X.MouseLeftButtonDown += handler;
            menu.Items.Add(X);
        }
        private void SearchItemIn1CBase_Click(object sender, RoutedEventArgs e)
        {
            MarketItem X = ((StructLibCore.Marketplace.MarketItem)((Button)sender).DataContext);
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
            ((TextBlock)sender).Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(Color.Green.R, Color.Green.G, Color.Green.B));
        }
        private void TextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            ((TextBlock)sender).Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(Color.Gray.R, Color.Gray.G, Color.Gray.B));
        }
        private void TextBlockStocks_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                ContextMenu menu = new ContextMenu();
                TextBlock TX = (TextBlock)sender;
                string N = TX.Name;
                IMarketItem item = (IMarketItem)TX.DataContext;
                TextBox Box = new TextBox();
                Box.Width = 50;
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
                menu.Items.Add(Box);
                menu.IsOpen = true;
                Box.Focus();
                Box.Select(0, Box.Text.Length);
                menu.Closed += (e, s) =>
                {
                    if (Box.Text != null && Box.Text != "")
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
        private async void SyncStoreges()
        {
            SiteStoregeStruct P = null;
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync("https://salessab.su/STList.xml"))
                using (Stream streamToReadFrom = await response.Content.ReadAsStreamAsync())
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(SiteStoregeStruct));
                    P = (SiteStoregeStruct)serializer.Deserialize(streamToReadFrom);
                    WCList = P.Warehouses;
                }
            }
            foreach (MarketItem item in ItemsList)
            {
                item.StoregeList = P.ItmsCount.FindAll(x => x.Id == item.BaseID).ToList();
            }
        }
        private async void DownloadName_Click(object sender, RoutedEventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync("https://salessab.su/ComNameList.xml"))
                using (Stream streamToReadFrom = await response.Content.ReadAsStreamAsync())
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<СomparisonNameID>));
                    сomparisonNames = (List<СomparisonNameID>)serializer.Deserialize(streamToReadFrom);
                }
            }
        }
        private void BaseIDBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                ContextMenu menu = new ContextMenu();
                TextBox Box = new TextBox();
                TextBlock TX = (TextBlock)sender;
                MarketItem MITEM = ((VisMarketItem)TX.DataContext).Item;
                Box.Width = 50;
                menu.Items.Add(Box);
                menu.IsOpen = true;
                Box.Focus();
                Box.Select(0, Box.Text.Length);
                ContextMenu menu2 = new ContextMenu();
                menu.Closed += (e, s) =>
                {
                    if (Box.Text.Length >= 3)
                    {
                        if (сomparisonNames != null)
                        {
                            List<СomparisonNameID> Find = сomparisonNames.FindAll(x => x.СomparisonName.Contains(Box.Text));
                            if (Find != null && Find.Count > 0 && Find.Count < 30)
                            {
                                foreach (СomparisonNameID item in Find)
                                {
                                    TextBlock block = new();
                                    block.Text = item.Name;
                                    block.DataContext = item;
                                    block.MouseDown += (sender, e) => { MITEM.BaseID = ((СomparisonNameID)((TextBlock)sender).DataContext).Id; };
                                    menu2.Items.Add(block);
                                }
                            }
                            menu2.IsOpen = true;
                        }
                        else { MessageBox.Show("Не загружены имена"); }
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
                    if (сomparisonNames != null)
                    {
                        List<СomparisonNameID> Find = сomparisonNames.FindAll(x => x.СomparisonName.Contains(Box.Text));
                        if (Find != null && Find.Count > 0 && Find.Count < 10)
                        {
                            foreach (СomparisonNameID item in Find)
                            {
                                TextBlock block = new TextBlock();
                                block.Text = item.Name;
                                menu2.Items.Add(block);
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
                ContextMenu menu = new ContextMenu();
                TextBox Box = new TextBox();
                TextBlock TX = (TextBlock)sender;
                MarketItem MITEM = ((VisMarketItem)TX.DataContext).Item;
                Box.Width = 50;
                Box.Focus();
                Box.Select(0, Box.Text.Length);
                menu.Closed += (e, s) => { MITEM.Art1C = Box.Text; };
                menu.Items.Add(Box);
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
                if (I.Cheked)
                {
                    foreach (IMarketItem X in item.Items)
                    {
                        if (X.APISetting.Name == ProcessingPanelApiBox.SelectedItem.ToString())
                        {
                            if (X.Price.Contains("."))
                            {
                                X.Price = X.Price.Replace(".", ",");
                            }
                            double Z = double.Parse(X.Price, System.Globalization.NumberStyles.AllowDecimalPoint);
                            double P = double.Parse(ProcessingPanelPercentBox.Text);
                            X.Price = ((P / 100 + 1) * Z).ToString();
                            mass.Add(X);
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
                if (I.Cheked)
                {
                    MarketItem item = I.Item;
                    foreach (IMarketItem X in item.Items)
                    {
                        if (ProcessingPanelApiBox.SelectedItem != null && X.APISetting.Name == ProcessingPanelApiBox.SelectedItem.ToString())
                        {
                            if (X.Price.Contains(".")) { X.Price = X.Price.Replace(".", ","); }
                            double Z = double.Parse(X.Price, System.Globalization.NumberStyles.AllowDecimalPoint);
                            double P = double.Parse(ProcessingPanelPercentBox.Text);
                            X.Price = (Z - ((P / 100) * Z)).ToString();
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
                if (I.Cheked)
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
                if (I.Cheked)
                {
                    MarketItem item = I.Item;
                    foreach (IMarketItem X in item.Items)
                    {
                        if (X.Stocks != "0" && X.Stocks != "" && X.Stocks != null)
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
                item.Cheked = true;
            }
        }
        private void UnmarkAll_Click(object sender, RoutedEventArgs e)
        {
            ((CheckBox)sender).IsChecked = false;
            foreach (VisMarketItem item in VisItemsList)
            {
                item.Cheked = false;
            }
        }
    }
}
