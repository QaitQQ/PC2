using SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post;

using StructLibCore.Marketplace;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
namespace MGSol.Panel
{
    public partial class ReturnControl : UserControl
    {
        private readonly MainModel model;
        private readonly ObservableCollection<APISetting> Options;
        private readonly ObservableCollection<IOrder> VisOrderList;
        private readonly ObservableCollection<IOrder> orderList;
        public OrdersControl ParentControl { get; set; }
        public ReturnControl(MainModel Model, OrdersControl parentControl = null)
        {
            model = Model;
            InitializeComponent();
            Options = new ObservableCollection<APISetting>();
            orderList = new ObservableCollection<IOrder>();
            VisOrderList = new ObservableCollection<IOrder>();
            ParentControl = parentControl;
            ParentControl.RenewEvent += () => Dispatcher.Invoke(() => LoadNetReturns());
            LoadNetReturns();
            OrderStack.ItemsSource = VisOrderList;
        }
        public void LoadNetReturns()
        {
            VisOrderList.Clear();
            orderList.Clear();
            List<IOrder> F = new();
            foreach (APISetting item in model.OptionMarketPlace.APISettings)
            {
                if (item.Active)
                {
                    List<object> Result = new();
                    switch (item.Type)
                    {
                        case MarketName.Yandex:
                            break;
                        case MarketName.Ozon:
                            Result = new OzonPostReturnList(item).Get();
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
                            Dispatcher.Invoke(() => orderList.Add((IOrder)t));
                        }
                    }
                }
            }
            List<IOrder> ReadyList = orderList.Where(x => x.Status == OrderStatus.READY).ToList();
            foreach (IOrder item in ReadyList)
            {
                VisOrderList.Add(item);
            }
            OrderStack.ItemsSource = VisOrderList;
        }
    }
}
