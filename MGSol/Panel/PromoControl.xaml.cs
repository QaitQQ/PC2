using SiteApi.IntegrationSiteApi.APIMarket.Ozon.Get;

using StructLibCore;
using StructLibCore.Marketplace;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MGSol.Panel
{
    /// <summary>
    /// Логика взаимодействия для PromoControl.xaml
    /// </summary>
    public partial class PromoControl : UserControl
    {
        private MainModel model { get; set; }
        private ObsList VisList { get; set; }
        private ObservableCollection<APISetting> Options { get; set; }
        public PromoControl(MainModel model)
        {
            this.model = model;
            VisList = new ObsList();
            InitializeComponent();
            Options = new ObservableCollection<APISetting>();
            DtaTable.ItemsSource = VisList;
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            _ = Task.Factory.StartNew(() => LoadPromo(this.model.OptionMarketPlace.APISettings));
        }


        private void LoadPromo(List<APISetting> aPIs)
        {
            Dispatcher.Invoke(() => Options.Clear());
            Dispatcher.Invoke(() => VisList.Clear());

            List<Promo> F = new();
            foreach (APISetting item in aPIs)
            {
                var T = Task.Factory.StartNew(() =>
                {
                    object Result = null;
                    if (item != null && item.Active)
                    {
                        Result = new();
                        switch (item.Type)
                        {
                            case MarketName.Yandex:
                                break;
                            case MarketName.Ozon:
                                Result = new OzonGetPromoList(item).Get();
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
                            foreach (var X in Result as List<Promo>)
                            {
                                Dispatcher.Invoke(() => {
                                    VisList.Add(new ShopPromoItems(item,X)); });
                            }
                          

                        }
                    }
                });

            }
        }


        private class ObsList : ObservableCollection<ShopPromoItems>
        {
            new public void Add(ShopPromoItems item) { base.Add(item); OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("This_add")); }
        }


        private class ShopPromoItems
        {
            public ShopPromoItems(APISetting Api, Promo Promo)
            {
                this.Api = Api;this.Promo = Promo;
            }

            public APISetting Api { get; set; }
            public string Name { get { return Api.Name; } }
            public Promo Promo { get; set; }

        }





    }
}
