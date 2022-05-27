using StructLibCore.Marketplace;

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
        private MainModel _model { get; set; }
        public ItemControl(MainModel model)
        {
            _model = model;
            InitializeComponent();
            VisItemsList = new ObservableCollection<StructLibCore.Marketplace.MarketItem>();
            Options = model.Option.APISettings;
            System.Threading.Tasks.Task.Factory.StartNew(() => Dispatcher.Invoke(() => LoadList()));
            VItemsList.ItemsSource = VisItemsList;
            SortedBox.ItemsSource = typeof(MarketItem).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        }
        #region Кнопки
        private void Button_Click(object sender, RoutedEventArgs e)
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
            ContextMenu M = new ContextMenu() { DataContext = (IMarketItem)((StackPanel)((Button)sender).Parent).DataContext };

            foreach (APISetting item in Options)
            {
                TextBlock X = new TextBlock() { Text = item.Name, DataContext = item };
                X.MouseLeftButtonDown += (x, y) =>
                {
                    IMarketItem X = (IMarketItem)((ContextMenu)((TextBlock)x).Parent).DataContext;

                    X.APISettingSource = X.APISetting;

                    X.APISetting = (APISetting)((TextBlock)x).DataContext;

                    IMarketItem[] mass = new IMarketItem[] { X };
                    string P = ImportItems(mass);
                    //   new Network.Item.MarketApi.RenewItemMarket().Get<bool>(new Client.WrapNetClient(), mass);

                    IEnumerable<MarketItem> Z = from o in VisItemsList where o.SKU == X.SKU select o;



                    foreach (MarketItem item in Z)
                    {
                        item.Items.Add(new UniMarketItem() { APISetting = X.APISetting, SKU = X.SKU, Price = P });
                    }



                    //((TextBlock)x).Parent

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
            _model.Option.MarketItems = ItemsList;
            _model.Save();
            //new Network.Item.MarketApi.SaveItemsList().Get<bool>(new Client.WrapNetClient(), ItemsList.ToArray().ToList());
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
                        case StructLibCore.Marketplace.MarketName.Yandex:
                            Result = new Server.Class.IntegrationSiteApi.Market.Yandex.YandexGetName.YandexGetItemList(Option).Get();
                            break;
                        case StructLibCore.Marketplace.MarketName.Ozon:
                            Result = new Server.Class.IntegrationSiteApi.Market.Ozon.OzonGetItemDesc(Option).Get();
                            break;
                        case StructLibCore.Marketplace.MarketName.Avito:
                            break;
                        case StructLibCore.Marketplace.MarketName.Sber:
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
            ItemsList = _model.Option.MarketItems;
            if (ItemsList != null)
            {
                foreach (MarketItem item in ItemsList)
                {
                    item.Items = new ObservableCollection<IMarketItem>();
                    VisItemsList.Add(item);
                }
            }
        }
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
            //  VItemsList.ItemsSource = VisItemsList;
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
        private string ImportItems(IMarketItem[] mass)
        {
            IMarketItem[] X = mass;

            static List<IGrouping<StructLibCore.Marketplace.APISetting, StructLibCore.Marketplace.IMarketItem>> ConvertListApi(StructLibCore.Marketplace.IMarketItem[] Lst)
            {
                IEnumerable<IGrouping<StructLibCore.Marketplace.APISetting, StructLibCore.Marketplace.IMarketItem>> X = Lst.GroupBy(x => x.APISetting);
                List<IGrouping<StructLibCore.Marketplace.APISetting, StructLibCore.Marketplace.IMarketItem>> A = X.ToList();
                return A;
            }
            List<IGrouping<APISetting, IMarketItem>> Z = ConvertListApi(X);
            object R = null;

            foreach (IGrouping<APISetting, IMarketItem> item in Z)
            {
                switch (item.Key.Type)
                {
                    case StructLibCore.Marketplace.MarketName.Yandex:
                        R = new SiteApi.IntegrationSiteApi.APIMarket.Yandex.YandexPostImport.YandexPostImport(item.Key).Get(item.ToArray());
                        break;
                    case StructLibCore.Marketplace.MarketName.Ozon:
                        R = new Server.Class.IntegrationSiteApi.Market.Ozon.OzonSetItem(item.Key).Get(item.ToArray());
                        break;
                    case StructLibCore.Marketplace.MarketName.Avito:
                        break;
                    case StructLibCore.Marketplace.MarketName.Sber:
                        break;
                    default:
                        break;
                }
            }
            return R.ToString();
        }
        private void RenewPrice(IMarketItem[] mass)
        {
            IMarketItem[] X = mass;
            static List<IGrouping<StructLibCore.Marketplace.APISetting, StructLibCore.Marketplace.IMarketItem>> ConvertListApi(StructLibCore.Marketplace.IMarketItem[] Lst)
            {
                IEnumerable<IGrouping<StructLibCore.Marketplace.APISetting, StructLibCore.Marketplace.IMarketItem>> X = Lst.GroupBy(x => x.APISetting);
                List<IGrouping<StructLibCore.Marketplace.APISetting, StructLibCore.Marketplace.IMarketItem>> A = X.ToList();
                return A;
            }
            List<IGrouping<APISetting, IMarketItem>> Z = ConvertListApi(X);
            object R = null;

            foreach (IGrouping<APISetting, IMarketItem> item in Z)
            {
                switch (item.Key.Type)
                {
                    case StructLibCore.Marketplace.MarketName.Yandex:
                        R = new Server.Class.IntegrationSiteApi.Market.Yandex.YandexPostItemPrice.YandexPostItemPrice(item.Key).Get(item.ToArray());
                        break;
                    case StructLibCore.Marketplace.MarketName.Ozon:
                        R = new Server.Class.IntegrationSiteApi.Market.Ozon.OzonSetItem(item.Key).Get(item.ToArray());
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
        private void LoadBaseItem_Click(object sender, RoutedEventArgs e)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Выгрузка));

            // десериализуем объект
            using (FileStream fs = new FileStream("Выгрузка.xml", FileMode.OpenOrCreate))
            {
                items1C = xmlSerializer.Deserialize(fs) as Выгрузка;

            }
        }
        private void RemClick(object sender, RoutedEventArgs e)
        {
            ItemsList.Remove((MarketItem)((System.Windows.Controls.Button)sender).DataContext);
            Fill_Vlist(ItemsList);
        }

        private void addBtn(ContextMenu menu, string BtnCont, MouseButtonEventHandler handler)
        {
            Label X = new Label() { Content = BtnCont };
            X.MouseLeftButtonDown += handler;
            menu.Items.Add(X);
        }
        private void SearchItemIn1CBase_Click(object sender, RoutedEventArgs e)
        {
            MarketItem X = ((StructLibCore.Marketplace.MarketItem)((Button)sender).DataContext);
            ContextMenu M = new ContextMenu();

            if (items1C == null)
            {
                LoadBaseItem_Click(null, null);
            }

            IEnumerable<Номенклатура> T = from O in items1C?.Номенклатура where O.Наименование.ToLower().Contains(X.Art1C) select O;

            foreach (Номенклатура item in items1C.Номенклатура)
            {
                if (item.Наименование.ToLower().Contains(X.Art1C.ToLower()))
                {
                    addBtn(M, item.Наименование, (e, x) => { X.Art1C = item.Код.ToString(); }); ;
                }
            }
            M.IsOpen = true;
        }
    }

    [XmlRoot(ElementName = "Номенклатура")]
    public class Номенклатура
    {
        [XmlElement(ElementName = "Наименование")]
        public string Наименование;

        [XmlElement(ElementName = "Артикул")]
        public string Артикул;

        [XmlElement(ElementName = "Код")]
        public string Код;

        [XmlElement(ElementName = "БазоваяЕдиницаИзмерения")]
        public string БазоваяЕдиницаИзмерения;

        [XmlElement(ElementName = "ВидНоменклатуры")]
        public string ВидНоменклатуры;
    }

    [XmlRoot(ElementName = "Выгрузка")]
    public class Выгрузка
    {
        [XmlElement(ElementName = "Номенклатура")]
        public List<Номенклатура> Номенклатура;
    }
}
