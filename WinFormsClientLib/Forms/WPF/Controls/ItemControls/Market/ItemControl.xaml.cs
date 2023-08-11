using StructLibCore;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace WinFormsClientLib.Forms.WPF.Controls.ItemControls.Market
{
    /// <summary>
    /// Логика взаимодействия для ItemControl.xaml
    /// </summary>
    /// 
    public partial class ItemControl : UserControl
    {
        private List<MarketItem> ItemsList;
        private ObservableCollection<MarketItem> VisItemsList;
        private List<APISetting> Options;
        public ItemControl(List<APISetting> Option)
        {
            InitializeComponent();
            VisItemsList = new ObservableCollection<MarketItem>();         
            this.Options = Option;
            System.Threading.Tasks.Task.Factory.StartNew(() => Dispatcher.Invoke(() => LoadList()));
            VItemsList.ItemsSource = VisItemsList;
        }
        private void LoadList()
        {
            ItemsList = new Network.Item.MarketApi.GetItemsList().Get<List<MarketItem>>(new Client.WrapNetClient());

            foreach (var item in ItemsList)
            {
                item.Items = new ObservableCollection<IMarketItem>();
                VisItemsList.Add(item);
            }
         
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Threading.Tasks.Task.Factory.StartNew(() => SyncItemList());
        }
        private void SyncItemList()
        {
            foreach (var item in ItemsList)
            {
                item.Items = new ObservableCollection<IMarketItem>();
            }
            foreach (var Option in Options)
            {
                if (Option.Active)
                {
                    var OZ = new Network.Item.MarketApi.GetListMarket().Get<List<object>>(new Client.WrapNetClient(), Option);

                    foreach (var item in OZ)
                    {
                        var It = (IMarketItem)item;
                        It.APISetting = Option;
                        MarketItem X = null;
                        X = ItemsList.FirstOrDefault(x => x.SKU == It.SKU);

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
                                Dispatcher.Invoke(() => { X.Items.Add(It); } );
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
        private void GetButton_click(object sender, RoutedEventArgs e)
        {
            //List<Server.Class.IntegrationSiteApi.Market.Ozon.OzonItemDesc> X = new List<Server.Class.IntegrationSiteApi.Market.Ozon.OzonItemDesc>();
            //var Y = ((MarketItem)((Button)sender).DataContext).Items;
            //foreach (var item in Y)
            //{
            //    X.Add(item as Server.Class.IntegrationSiteApi.Market.Ozon.OzonItemDesc);
            //}
            //var mass = X.ToArray(); 
            //new Network.Item.MarketApi.RenewItemOzon().Get<bool>(new Client.WrapNetClient(), mass);
        }
        private void Renew_click(object sender, RoutedEventArgs e)
        {
            var mass = new IMarketItem[] { ((Button)sender).DataContext as IMarketItem };
            new Network.Item.MarketApi.RenewItemMarket().Get<List<object>>(new Client.WrapNetClient(), mass);
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < ItemsList.Count; i++)
            {
                if (ItemsList[i].Items.Count == 0)
                {
                    ItemsList.Remove(ItemsList[i]);
                }
                else if (ItemsList[i].Items.Count == 1 && ItemsList[i].Items[0].SKU == ItemsList[i].SKU)
                {
                    ItemsList.Remove(ItemsList[i]);
                }
            }
        }
        private void Button_Save(object sender, RoutedEventArgs e)
        {
           new Network.Item.MarketApi.SaveItemsList().Get<bool>(new Client.WrapNetClient(), ItemsList.ToArray().ToList());
        }
        private void Find_fill(object sender, RoutedEventArgs e)
        {
            var selectionItem = from lst in ItemsList where lst.Name.ToLower().Contains(FindField.Text.ToLower()) select lst;
            Fill_Vlist(selectionItem);
        }
        private void Fill_Vlist(IEnumerable<MarketItem> selectionItem)
        {
            VisItemsList.Clear();

            foreach (var item in selectionItem)
            {
                VisItemsList.Add(item);
            }
            GC.Collect();
            VItemsList.ItemsSource = VisItemsList;
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {

           
                var M = new ContextMenu() { DataContext = (IMarketItem)((StackPanel)((Button)sender).Parent).DataContext };
                

            foreach (var item in Options)
            {
                var X = new TextBlock() { Text = item.Name, DataContext = item };
                X.MouseLeftButtonDown += (x, y) =>
                {


                    IMarketItem X = (IMarketItem)((ContextMenu)((TextBlock)x).Parent).DataContext;

                    X.APISetting = (APISetting)((TextBlock)x).DataContext;


                       var mass = new IMarketItem[] { X };

                       new Network.Item.MarketApi.RenewItemMarket().Get<bool>(new Client.WrapNetClient(), mass);


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
    }
}
