using SiteApi.IntegrationSiteApi.APIMarket.Ozon.Get;

using StructLibCore;
using StructLibCore.Marketplace;

using System;
using System.Collections.ObjectModel;
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

        public PromoControl(MainModel model)
        {
            this.model = model;
            VisList = new ObsList();
            InitializeComponent();
            DtaTable.ItemsSource = VisList;



        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            var X = new OzonGetPromoList(model.OptionMarketPlace.APISettings[0]).Get();

            VisList.Clear();
            foreach (var item in X) { VisList.Add(item);}
           

        }


        private class ObsList : ObservableCollection<Promo>
        {
            new public void Add(Promo item) { base.Add(item); OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("This_add")); }





        }


    }
}
