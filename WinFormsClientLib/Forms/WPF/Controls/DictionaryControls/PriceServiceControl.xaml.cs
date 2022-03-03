using Client;
using Server;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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
        private readonly ObservableCollection<Grid> ItemInfoList;
        public PriceStorage ActivePrice;
        public PriceServiceControl()

        {
            STList = new ObservableCollection<PriceStorage>();
            List<PriceStorage> col = new Network.PriceService.GetPriceStorege().Get<List<PriceStorage>>(new WrapNetClient());
            foreach (PriceStorage item in col) { STList.Add(item); }

            InitializeComponent();
            LeftBtn = new ObservableCollection<Button>();
            ItemBtn = new ObservableCollection<Button>();
            ItemInfoList = new ObservableCollection<Grid>();
            ButtonStack.ItemsSource = LeftBtn;
            NameFileList.ItemsSource = STList;
            InfoPrice.ItemsSource = ItemInfoList;
            PriceBtns.ItemsSource = ItemBtn;
            SupportButton.AddButtons(LeftBtn, new RoutedEventHandler[] { AddFile });
            SupportButton.AddButtons(LeftBtn, new RoutedEventHandler[] { RemPrice });
            SupportButton.AddButtons(ItemBtn, new RoutedEventHandler[] { ReadPrice });
            NameFileList.SelectedItem = 0;

        }

        private void RemPrice(object sender, RoutedEventArgs e)
        {
            if (ActivePrice != null)
            {
                bool result = new Network.PriceService.RemovePriceStorege().Get<bool>(new WrapNetClient(), ActivePrice);


                if (result)
                {
                    STList.Remove(ActivePrice);
                }
            }

        }

        private void AddFile(object sender, RoutedEventArgs e)
        {
            STList.Add(new PriceStorage() { Name = "Новый Файл" });
        }

        private Grid GenGrid(PriceStorage ActivePrice, string Property, string FildName)
        {

            Grid grid = new Grid();

            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(50) });
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            Label label = new Label() { Content = FildName };
            Binding binding = new Binding(Property);
            binding.Source = ActivePrice;
            TextBox textBox = new TextBox();
            textBox.SetBinding(TextBox.TextProperty, binding);
            Grid.SetColumn(label, 0);
            Grid.SetColumn(textBox, 1);
            grid.Children.Add(label);
            grid.Children.Add(textBox);
            return grid;
        }

        private void NameFileList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ItemInfoList.Clear();
            if (e.AddedItems.Count != 0)
            {
                ActivePrice = (PriceStorage)e.AddedItems[0];

                ItemInfoList.Add(GenGrid(ActivePrice, "Name", "Имя"));
                ItemInfoList.Add(GenGrid(ActivePrice, "FilePath", "Файл"));
                ItemInfoList.Add(GenGrid(ActivePrice, "Link", "Ссылка"));
                ItemInfoList.Add(GenGrid(ActivePrice, "ReceivingData", "Дата"));
                ItemInfoList.Add(GenGrid(ActivePrice, "PlanedRead", "Запланирован"));
                ItemInfoList.Add(GenGrid(ActivePrice, "PlanedTime", "Планируемая Дата"));

                Grid Grd = new Grid();
                Grd.ColumnDefinitions.Add(new ColumnDefinition());
                Grd.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(10) });

                Grd.Children.Add(new Label() { Content = "Автоматическое чтение" });
                CheckBox CheckBox = new CheckBox();
                CheckBox.IsChecked = ActivePrice.DefaultReading;
                CheckBox.Click += CheckBox_Click;
                Grid.SetColumn(CheckBox, 1);
                Grd.Children.Add(CheckBox);


                var Dic = new Network.PriceService.GetDic().Get<Object_Description.IDictionaryPC>(new WrapNetClient(), ActivePrice);



                ItemInfoList.Add(Grd);

                Grid BtnStack = new Grid();
                BtnStack.RowDefinitions.Add(new RowDefinition());
                BtnStack.RowDefinitions.Add(new RowDefinition());
                var SaveBtn = new Button() { Content = "Save" };
                var DownloadBtn = new Button() { Content = "Download" };
                SaveBtn.Click += SaveBtn_Click;
                DownloadBtn.Click += DownloadBtn_Click;
                Grid.SetRow(DownloadBtn, 1);
                BtnStack.Children.Add(DownloadBtn);
                BtnStack.Children.Add(SaveBtn);
                ItemInfoList.Add(BtnStack);


            }
        }

        private void DownloadBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ActivePrice != null && ActivePrice.Name != "Новый Файл")
            {
                new Network.PriceService.DownloadPriceStorege().Get<bool>(new WrapNetClient(), ActivePrice);
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ActivePrice != null && ActivePrice.Name != "Новый Файл")
            {
                new Network.PriceService.SavePriceStorege().Get<bool>(new WrapNetClient(), ActivePrice);
            }

        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            ActivePrice.DefaultReading = (bool)((CheckBox)sender).IsChecked;
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
