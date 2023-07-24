using StructLibCore.Marketplace;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;

namespace MGSol.Panel
{
    public partial class ReturnControl : UserControl
    {
        private readonly MainModel model;
        private readonly ObservableCollection<APISetting> Options;
        private readonly ObservableCollection<IOrder> VisOrderList;
        private readonly ObservableCollection<IOrder> orderList;
        public ReturnControl(MainModel Model)
        {
            model = Model;
            InitializeComponent();
            Options = new ObservableCollection<APISetting>();
            orderList = new ObservableCollection<IOrder>();
            VisOrderList = new ObservableCollection<IOrder>();
            OrderStack.ItemsSource = VisOrderList;
        }
        public void LoadNetReturns(List<APISetting> aPISettings)
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
            List<IOrder> ReadyList = orderList.Where(x => x.Status == OrderStatus.READY).ToList();
            foreach (IOrder item in ReadyList)
            {
                VisOrderList.Add(item);
            }
        }
    }
}
