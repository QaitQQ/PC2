﻿using StructLibCore.Marketplace;
using StructLibs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
namespace MGSol.Panel
{
    /// <summary>
    /// Логика взаимодействия для OptionControl.xaml
    /// </summary>
    public partial class OptionControl : UserControl
    {
        private MainModel Model { get; set; }
        private ObservableCollection<APISetting> ListOption { get; set; }
        private ObservableCollection<InnString> innStrings { get; set; }
        public OptionControl(MainModel model)
        {
            InitializeComponent();
            Model = model;
            ListOption = new ObservableCollection<APISetting>();
            innStrings = new ObservableCollection<InnString>();
            foreach (InnString item in Model.OptionMarketPlace.SellerINN)
            {
                innStrings.Add(item);
            }
            InnSalerBox.ItemsSource = innStrings;
            foreach (APISetting item in Model.OptionMarketPlace.APISettings)
            {
                ListOption.Add(item);
            }
            ListOption.CollectionChanged += (x, y) =>
            {
                List<APISetting> lst = new();
                foreach (APISetting item in ListOption)
                {
                    lst.Add(item);
                }
                Model.OptionMarketPlace.APISettings = lst;
                Model.OptionMarketPlace = Model.OptionMarketPlace;
            };
            UserField.SetBinding(TextBox.TextProperty, new Binding("User") { Source = Model.BaseInfoPrice.LogPass });
            PassField.SetBinding(TextBox.TextProperty, new Binding("Pass") { Source = Model.BaseInfoPrice.LogPass });
            if (Model.BaseInfoPrice.AddressPort == null)
            {
                Model.BaseInfoPrice.AddressPort = new AddressPort();
            }
            IPField.SetBinding(TextBox.TextProperty, new Binding("Address") { Source = Model.BaseInfoPrice.AddressPort });
            PortField.SetBinding(TextBox.TextProperty, new Binding("Port") { Source = Model.BaseInfoPrice.AddressPort });
            UriBaseBox.SetBinding(TextBox.TextProperty, new Binding("UriBase") { Source = Model.BaseInfoPrice });
            TokenBaseBox.SetBinding(TextBox.TextProperty, new Binding("ToketBase") { Source = Model.BaseInfoPrice});
            OptionList.ItemsSource = ListOption;
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            OptionAddBox OptionAddBox = new();
            if ((bool)OptionAddBox.ShowDialog())
            {
                ListOption.Add(OptionAddBox.ApiSetting);
            }
        }
        private void DelButtonClick_Click(object sender, RoutedEventArgs e)
        {
            ListOption.Remove((APISetting)((Button)sender).DataContext);
        }
        private void Grid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Grid grid = (Grid)sender;
            APISetting api = (APISetting)grid.DataContext;
            OptionAddBox OptionAddBox = new(api);
            if ((bool)OptionAddBox.ShowDialog())
            {
                if (OptionAddBox.Qadd)
                {
                    ListOption.Add(OptionAddBox.ApiSetting);
                }
                else
                {
                    _ = OptionAddBox.ApiSetting;
                }
            }
        }
        private void Button_Save(object sender, RoutedEventArgs e)
        {
            Model.Save();
        }
        private void AddButton_Click_1(object sender, RoutedEventArgs e)
        {
            innStrings.Add(new InnString(MarketName.Ozon, "123455"));
            Model.OptionMarketPlace.SellerINN.Add(new InnString(MarketName.Ozon, "123455"));
        }
        private void RemoveInnButton_Click(object sender, RoutedEventArgs e)
        {
            InnString inn = (InnString)((System.Windows.Controls.Button)sender).DataContext;
            innStrings.Remove(inn);
            Model.OptionMarketPlace.SellerINN.Remove(inn);
        }
        private void SyncBaseButton_Click(object sender, RoutedEventArgs e)
        {
            Model.BaseInfoPrice.PriceIDPair = new List<PriceIDPair>();
            foreach (MarketItem item in Model.OptionMarketPlace.MarketItems)
            {
                if (item.BaseID != 0)
                {
                    try
                    {
                        ItemPlusImageAndStorege Item = new Network.Item.GetItemFromId().Get<ItemPlusImageAndStorege>(Model.GetClient(), item.BaseID.ToString());
                        item.Price = Item.Item.PriceRC;
                    }
                    catch 
                    {
                        MessageBox.Show("Problem "+ item.BaseID.ToString());
                    }
                }
            }
        }
        private void TokenBaseBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }
    }
}
