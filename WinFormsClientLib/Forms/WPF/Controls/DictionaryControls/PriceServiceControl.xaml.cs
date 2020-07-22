using Client;

using Server;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WinFormsClientLib.Forms.WPF.Controls.DictionaryControls
{
    /// <summary>
    /// Логика взаимодействия для PriceServiceControl.xaml
    /// </summary>
    public partial class PriceServiceControl : UserControl
    {

        private ObservableCollection<PriceStorage> STList;
        private ObservableCollection<Button> LeftBtn;
        private ObservableCollection<Button> ItemBtn;
        private ObservableCollection<StackPanel> ItemInfoList;
        private PriceStorage ActivePrice;
        public PriceServiceControl()

        {
            STList = new ObservableCollection<PriceStorage>();
            var col = new Network.PriceService.GetPriceStorege().Get<List<PriceStorage>>(new WrapNetClient());
            foreach (var item in col){STList.Add(item);}

            InitializeComponent();
            LeftBtn = new ObservableCollection<Button>();
            ItemBtn = new ObservableCollection<Button>();
            ItemInfoList = new ObservableCollection<StackPanel>();
            ButtonStack.ItemsSource = LeftBtn;
            NameFileList.ItemsSource = STList;
            InfoPrice.ItemsSource = ItemInfoList;
            PriceBtns.ItemsSource = ItemBtn;
            SupportButton.AddButtons(LeftBtn, new RoutedEventHandler[] { AddFile });
            SupportButton.AddButtons(ItemBtn, new RoutedEventHandler[] { ReadPrice });
            NameFileList.SelectedItem = 0;

        }

        private void AddFile(object sender, RoutedEventArgs e) { STList.Add(new PriceStorage() { Name = "Новый Файл" }); }

        private StackPanel GenPair(string Label,string Value) 
        {
            var Grd = new StackPanel();
            Grd.Orientation = Orientation.Horizontal;

            Grd.Children.Add(new Label() {Content= Label });
            Grd.Children.Add(new Label() { Content = Value });

            return Grd;
        }

        private void NameFileList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ItemInfoList.Clear();
            ActivePrice = (PriceStorage)e.AddedItems[0];
            ItemInfoList.Add(GenPair("Имя ", ActivePrice.Name));
            ItemInfoList.Add(GenPair("Путь ", ActivePrice.FilePath));
            ItemInfoList.Add(GenPair("Дата ", ActivePrice.ReceivingData.ToString()));
            ItemInfoList.Add(GenPair("Атрибуты ", string.Join(",", ActivePrice.Attributes)));
        }
        private void ReadPrice(object sender, RoutedEventArgs e)
        {
            if (ActivePrice != null)
            {
                MessageBox.Text = new Network.PriceService.ReadPrice().Get<string>(new WrapNetClient(), ActivePrice);
            }
          
        }
    }
}
