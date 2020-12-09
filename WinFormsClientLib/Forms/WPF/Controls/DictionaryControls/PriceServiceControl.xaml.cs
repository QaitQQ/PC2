using Client;

using Server;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WinFormsClientLib.Forms.WPF.Controls.DictionaryControls
{
    /// <summary>
    /// Логика взаимодействия для PriceServiceControl.xaml
    /// </summary>
    public partial class PriceServiceControl : UserControl
    {

        private ObservableCollection<PriceStorage> STList { get; set; }
        private readonly ObservableCollection<Button> LeftBtn;
        private readonly ObservableCollection<Button> ItemBtn;
        private readonly ObservableCollection<StackPanel> ItemInfoList;
        private PriceStorage ActivePrice;
        public PriceServiceControl()

        {
            STList = new ObservableCollection<PriceStorage>();
            List<PriceStorage> col = new Network.PriceService.GetPriceStorege().Get<List<PriceStorage>>(new WrapNetClient());
            foreach (PriceStorage item in col) { STList.Add(item); }

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

        private StackPanel GenPair(string Label, string Value)
        {
            StackPanel Grd = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };

            Grd.Children.Add(new Label() { Content = Label });
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

            StackPanel Grd = new StackPanel { Orientation = Orientation.Horizontal };
            Grd.Children.Add(new Label() { Content = "Автоматическое чтение" });
            CheckBox CheckBox = new CheckBox();
            CheckBox.IsChecked = ActivePrice.DefaultReading;
            CheckBox.Click += CheckBox_Click;
            Grd.Children.Add(CheckBox);
            ItemInfoList.Add(Grd);



            StackPanel BtnStack = new StackPanel { Orientation = Orientation.Horizontal };
            var SaveBtn = new Button() { Content = "Save" };
            SaveBtn.Click += SaveBtn_Click;
            BtnStack.Children.Add(SaveBtn);
            ItemInfoList.Add(BtnStack);


        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            new Network.PriceService.SavePriceStorege().Get<bool>(new WrapNetClient(), ActivePrice);
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            ActivePrice.DefaultReading =(bool)((CheckBox)sender).IsChecked;
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
