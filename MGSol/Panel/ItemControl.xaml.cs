using StructLibCore.Marketplace;

using StructLibs;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Serialization;
namespace MGSol.Panel
{
    /// <summary>
    /// Логика взаимодействия для ItemControl.xaml
    /// </summary>
    public partial class ItemControl : UserControl
    {
        private List<MarketItem> ItemsList;
        private ObservableCollection<MarketItem> VisItemsList { get; set; }
        private Выгрузка items1C;
        private List<APISetting> Options { get; set; }
        private MainModel Model { get; set; }
        public ItemControl(MainModel model)
        {
            Model = model;
            InitializeComponent();
            VisItemsList = new ObservableCollection<MarketItem>();
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
            IMarketItem[] mass = new IMarketItem[] { ((Button)sender).DataContext as IMarketItem };
            //  new Network.Item.MarketApi.RenewItemMarket().Get<List<object>>(new Client.WrapNetClient(), mass);
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
                    IEnumerable<MarketItem> Z = from o in VisItemsList where o.SKU == X.SKU select o;
                    foreach (MarketItem item in Z)
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
            Grid grid = (((System.Windows.Controls.Grid)(((StackPanel)((Button)sender).Parent).Parent)));
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
                VisItemsList.Add(item);
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
            foreach (APISetting Option in Options)
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
                    foreach (object item in Result)
                    {
                        IMarketItem It = (IMarketItem)item;
                        It.APISetting = Option;
                        MarketItem X = null;
                        X = ItemsList.FirstOrDefault(x => x.SKU == It.SKU);
                        if (X != null && X.SKU.Contains("1057"))
                        {
                        }
                        if (X != null && X.Name == null)
                        {
                            X.Name = It.Name;
                        }
                        if (X != null)
                        {
                            Dispatcher.Invoke(() => { X.Items.Add(It); });
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
        }
        private void LoadList()
        {
            ItemsList = Model.OptionMarketPlace.MarketItems;
            if (ItemsList != null)
            {
                foreach (MarketItem item in ItemsList)
                {
                    item.Items = new ObservableCollection<IMarketItem>();
                    VisItemsList.Add(item);
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
            foreach (IGrouping<APISetting, IMarketItem> item in Z)
            {
                switch (item.Key.Type)
                {
                    case StructLibCore.Marketplace.MarketName.Yandex:
                        new Server.Class.IntegrationSiteApi.Market.Yandex.YandexPostItemPrice.YandexPostItemPrice(item.Key).Get(item.ToArray());
                        break;
                    case StructLibCore.Marketplace.MarketName.Ozon:
                        new Server.Class.IntegrationSiteApi.Market.Ozon.OzonPostSetPrice(item.Key).Get(item.ToArray());
                        new Server.Class.IntegrationSiteApi.Market.Ozon.OzonPostSetStoks(item.Key).Get(item.ToArray());
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
        private void PlusPricePercent(object sender, RoutedEventArgs e)
        {
            List<IMarketItem> mass = new();
            foreach (MarketItem item in ItemsList)
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
            RenewPrice(mass.ToArray());
        }
        private void MinusPricePercent(object sender, RoutedEventArgs e)
        {
            List<IMarketItem> mass = new();
            foreach (MarketItem item in ItemsList)
            {
                foreach (IMarketItem X in item.Items)
                {
                    if (ProcessingPanelApiBox.SelectedItem != null && X.APISetting.Name == ProcessingPanelApiBox.SelectedItem.ToString())
                    {
                        if (X.Price.Contains("."))
                        {
                            X.Price = X.Price.Replace(".", ",");
                        }
                        double Z = double.Parse(X.Price, System.Globalization.NumberStyles.AllowDecimalPoint);
                        double P = double.Parse(ProcessingPanelPercentBox.Text);
                        X.Price = (Z - ((P / 100) * Z)).ToString();
                        mass.Add(X);
                    }
                }
            }
            RenewPrice(mass.ToArray());
        }
        private void BaseIDBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            MarketItem X = (MarketItem)((TextBox)sender).DataContext;
            if (X.BaseID == 0)
            {
                List<СomparisonNameID> Search = new Network.Item.ItemSearch().Get<List<СomparisonNameID>>(Model.GetClient(), new object[] { ((System.Windows.Controls.TextBox)sender).Text, 3 });
                ContextMenu M = new();

                foreach (СomparisonNameID item in Search)
                {
                    AddBtn(M, item.Name, (e, x) => { X.BaseID = item.Id; (sender as TextBox).Text = item.Id.ToString(); }); ;
                }
                M.IsOpen = true;
            }


        }
    }
}
