using Server.Class.IntegrationSiteApi.Market.Ozon;
using SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post;
using Spire.Pdf.Exporting.XPS.Schema;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
namespace MGSol.Panel
{
    /// <summary>
    /// Логика взаимодействия для CopyFromIdControl.xaml
    /// </summary>
    public partial class CopyFromIdControl : UserControl
    {
        private MainModel Model { get; set; }
        private ObservableCollection<OzonPostItemCopyIDList.TaskIdName> TaskIds { get; set; }
        public CopyFromIdControl(MainModel model)
        {
            InitializeComponent();
            Model = model;
            ApiListBox.ItemsSource = model.GetApi();
            TaskIds = new ObservableCollection<OzonPostItemCopyIDList.TaskIdName>();
            TaskList.ItemsSource = TaskIds;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ApiListBox.SelectedItem != null)
            {
                var api = Model.GetApiFromName(ApiListBox.SelectedItem.ToString());
                OzonPostItemCopyIDList.TaskIdName NewTask = new OzonPostItemCopyIDList(api).Get(new List<string> { IdBox.Text });
                TaskIds.Add(NewTask);
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var api = Model.GetApiFromName(ApiListBox.SelectedItem.ToString());
            var btn = (Button)sender;
            var itemName = ((OzonPostItemCopyIDList.TaskIdName)btn.DataContext).Names[0];
            string NameBox = null;
            string SKUBox = null;
            string PriceBox = null;
            foreach (var item in ((Grid)btn.Parent).Children)
            {
                if (item is StackPanel)
                {
                    foreach (var X in ((StackPanel)item).Children)
                    {
                        if (X is Grid)
                        {
                            foreach (var Y in ((Grid)X).Children)
                            {
                                if (Y is TextBox)
                                {
                                    switch (((TextBox)Y).Name)
                                    {
                                        case "NameBox":
                                            NameBox = ((TextBox)Y).Text;
                                            break;
                                        case "SKUBox":
                                            SKUBox = ((TextBox)Y).Text;
                                            break;
                                        case "PriceBox":
                                            PriceBox = ((TextBox)Y).Text;
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            var items = (new OzonPostItemDesc(api).Get(new List<string> { itemName }));
            if (items != null && items.Count > 0)
            {
                btn.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(255,0, 0));


                var item = (OzonItemDesc)items[0];
                item.offer_id = Environment.TickCount.ToString();
                OzonPostGetAttr.Response attr = new OzonPostGetAttr(api).Get(new List<string> { itemName });
                if (attr != null)
                {
                    item.attributes = new List<object>();
                    foreach (var X in attr.Result)
                    {
                        item.attributes.Add(X);
                    }
                    item.name = NameBox;
                    item.offer_id = SKUBox;
                    item.price = PriceBox;
                    item.old_price = ((double.Parse(item.price)) * 1.3).ToString();
                    item.premium_price = item.price;
                    item.min_ozon_price = ((double.Parse(item.price)) * 0.9).ToString();
                    item.min_price = ((double.Parse(item.price)) * 0.9).ToString();
                    var copyItem = new OzonPostItemCopyObject(api).Get(new List<StructLibCore.Marketplace.IMarketItem> { item });
                    if (copyItem != null)
                    {
                        var ToArchive = new OzonPostItemToArchive(api).Get(new List<string> { itemName });
                    }
                }
            }
        }
    }
}
