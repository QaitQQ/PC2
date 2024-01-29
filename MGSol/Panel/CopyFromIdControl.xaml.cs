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
            var api = Model.GetApiFromName(ApiListBox.SelectedItem.ToString());

            OzonPostItemCopyIDList.TaskIdName NewTask = new OzonPostItemCopyIDList(api).Get(new List<string> { IdBox.Text });
            TaskIds.Add(NewTask);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var api = Model.GetApiFromName(ApiListBox.SelectedItem.ToString());
            var itemName =((OzonPostItemCopyIDList.TaskIdName)((Button)sender).DataContext).Names[0];

            var item =(OzonItemDesc)(new OzonPostItemDesc(api).Get(new List<string> { itemName })[0]);

            item.offer_id = Environment.TickCount.ToString();

            OzonPostGetAttr.Response attr = new OzonPostGetAttr(api).Get(new List<string> { itemName });
            item.attributes = new List<object>();

            foreach (var X in attr.Result)
            {
                item.attributes.Add(X);
            }

            var copyItem = new OzonPostItemCopyObject(api).Get(new List<StructLibCore.Marketplace.IMarketItem> { item });
        }
    }
}
