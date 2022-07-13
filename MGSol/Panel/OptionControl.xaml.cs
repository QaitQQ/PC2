using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MGSol.Panel
{
    /// <summary>
    /// Логика взаимодействия для OptionControl.xaml
    /// </summary>
    public partial class OptionControl : UserControl
    {
        private MainModel _model { get; set; }
        private ObservableCollection<StructLibCore.Marketplace.APISetting> ListOption { get; set; }
        private ObservableCollection<StructLibCore.Marketplace.InnString> innStrings;
        public OptionControl(MainModel model)
        {
            InitializeComponent();

            _model = model;

            ListOption = new ObservableCollection<StructLibCore.Marketplace.APISetting>();
            innStrings = new ObservableCollection<StructLibCore.Marketplace.InnString>();

            foreach (var item in _model.Option.SellerINN)
            {
                innStrings.Add(item);
            }


            InnSalerBox.ItemsSource = innStrings;

            foreach (StructLibCore.Marketplace.APISetting item in _model.Option.APISettings)
            {
                ListOption.Add(item);
            }
            ListOption.CollectionChanged += (x, y) =>
            {
                List<StructLibCore.Marketplace.APISetting> lst = new List<StructLibCore.Marketplace.APISetting>();
                foreach (StructLibCore.Marketplace.APISetting item in ListOption)
                {
                    lst.Add(item);
                }
                _model.Option.APISettings = lst;
                _model.Option = _model.Option;
            };

            OptionList.ItemsSource = ListOption;
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            OptionAddBox OptionAddBox = new OptionAddBox();
            if ((bool)OptionAddBox.ShowDialog())
            {
                ListOption.Add(OptionAddBox.Setting);
            }
        }
        private void DelButtonClick_Click(object sender, RoutedEventArgs e)
        {
            ListOption.Remove((StructLibCore.Marketplace.APISetting)((Button)sender).DataContext);
        }
        private void Grid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Grid grid = (Grid)sender;

            StructLibCore.Marketplace.APISetting api = (StructLibCore.Marketplace.APISetting)grid.DataContext;

            OptionAddBox OptionAddBox = new OptionAddBox(api);

            if ((bool)OptionAddBox.ShowDialog())
            {
                if (OptionAddBox.Qadd)
                {
                    ListOption.Add(OptionAddBox.Setting);
                }
                else
                {
                    api = OptionAddBox.Setting;
                }
            }
        }
        private void Button_Save(object sender, RoutedEventArgs e)
        {
            _model.Save();
        }
        private void AddButton_Click_1(object sender, RoutedEventArgs e)
        {
            innStrings.Add(new StructLibCore.Marketplace.InnString(StructLibCore.Marketplace.MarketName.Ozon, "123455"));
            _model.Option.SellerINN.Add(new StructLibCore.Marketplace.InnString(StructLibCore.Marketplace.MarketName.Ozon, "123455"));
        }
        private void RemoveInnButton_Click(object sender, RoutedEventArgs e)
        {
           var inn = (StructLibCore.Marketplace.InnString)((System.Windows.Controls.Button)sender).DataContext;
            innStrings.Remove(inn);
            _model.Option.SellerINN.Remove(inn);
        }
    }
}
