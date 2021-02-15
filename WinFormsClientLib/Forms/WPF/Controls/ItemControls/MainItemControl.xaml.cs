
using Client;

using CRMLibs;

using StructLibs;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using WinFormsClientLib.Forms.WPF.Controls.ItemControls;
using WinFormsClientLib.Forms.WPF.Controls.Other;

namespace WinFormsClientLib.Forms.WPF.ItemControls
{
    /// <summary>
    /// Логика взаимодействия для MainItemControl.xaml
    /// </summary>
    /// 
    public partial class MainItemControl : UserControl
    {
        public ObservableCollection<СomparisonNameID> SearchList { get; set; }
        public ObservableCollection<PropPair> FilterList { get; set; }
        private readonly System.Reflection.PropertyInfo[] Prop;
        public ObservableCollection<ItemControl> ItemsPanel { get; set; }
        private event Action<int, int> ChangeSale;
        private int[] _SaleValue;
        private int[] SaleValue { get => _SaleValue; set { _SaleValue = value; ChangeSale?.Invoke(SaleValue[0], SaleValue[1]); } }
        public MainItemControl()
        {
            InitializeComponent();
            SearchList = new ObservableCollection<СomparisonNameID>();
            Prop = ItemDBStruct.GetProperties();
            FilterList = new ObservableCollection<PropPair>();
            ItemsPanel = new ObservableCollection<ItemControl>();
            foreach (System.Reflection.PropertyInfo item in Prop)
            {
                PropertyInfoComboBox.Items.Add(item.Name);
                if (item.Name == "Name") { PropertyInfoComboBox.SelectedIndex = PropertyInfoComboBox.Items.Count - 1; }
            }

            FilterStack.ItemsSource = FilterList;
            ItemSearchListBox.ItemsSource = SearchList;

            KeyDown += MainItemControl_KeyDown;

            ChangeSale += new Action<int, int>(SetSeleValue);

            SaleBox.TextChanged += new TextChangedEventHandler(SaleChange);
            MarkupBox.TextChanged += new TextChangedEventHandler(SaleChange);
            ButtonStack.Items.Add(new PercentCalc());

            //Button RenewButton = new Button() { Content = "Обновить Кэш", Margin = new Thickness(2) };
            //RenewButton.Click += RenewCash_Click;
            //ButtonStack.Items.Add(RenewButton);

            //Button TagGenerateButton = new Button() { Content = "Перебрать Теги", Margin = new Thickness(2) };
            //TagGenerateButton.Click += TagGenerate_Click;
            //ButtonStack.Items.Add(TagGenerateButton);

            FilterList.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(AppFilter);
        }
        private void TagGenerate_Click(object sender, RoutedEventArgs e)
        {
            new Network.Item.TagGenerate().Get<bool>(new WrapNetClient());
        }
        private void MainItemControl_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter && SearhTextBox.IsSelectionActive)
            {
                SearchButton_Click(sender, new RoutedEventArgs());
            }
        }
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {

            SearchList.Clear();
            Search(SearhTextBox.Text, PropertyInfoComboBox.SelectedItem.ToString());
            ItemSearchListBox.Items.Refresh();
        }
        private async void ItemSearchListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 1)
            {
                СomparisonNameID X = e.AddedItems[0] as СomparisonNameID;
                using Network.Item.GetItemFromId Qwery = new Network.Item.GetItemFromId();
                ItemPlusImageAndStorege Obj = null;
                await System.Threading.Tasks.Task.Factory.StartNew(() => Obj = Qwery.Get<ItemPlusImageAndStorege>(new WrapNetClient(), X.Id.ToString()));
                ItemControl NewControl = new ItemControl(Obj);
                NewControl.GoSite += ((Client.Forms.Mainform)Client.Main.CommonWindow).GoSite;
                NewControl.GenSiteItem += (i) => { GenSiteItemList.Children.Add(new SiteItemGenerator(i)); SiteGeneratorGrid.Width = new GridLength(100); };
                if (ItemDescriptionBox.Children.Count < 2)
                {
                    if (ItemDescriptionBox.Children.Count == 0)
                    {
                        ItemDescriptionBox.Children.Add(NewControl);
                    }
                    else
                    {
                        UIElement GG = ItemDescriptionBox.Children[0];
                        ItemDescriptionBox.Children.Clear();
                        ItemDescriptionBox.Children.Add(NewControl);
                        ItemDescriptionBox.Children.Add(GG);
                    }
                }
                else
                {
                    foreach (object item in ItemDescriptionBox.Children)
                    {
                        ((ItemControl)item).Dispose();
                    }
                    ItemDescriptionBox.Children.Clear();
                    ItemDescriptionBox.Children.Add(NewControl);
                    GC.Collect();
                }
            }
        }
        private void SaleChange(object Obj, TextChangedEventArgs e)
        {
            static int ConvertToInt(string Str)
            {
                string X = null;
                if (Str != "")
                {
                    foreach (char item in Str)
                    {
                        if (char.IsDigit(item)) { X += item; }
                    }
                    return Convert.ToInt32(X);
                }
                return 0;
            }

            SaleValue = new int[] { ConvertToInt(SaleBox.Text), ConvertToInt(MarkupBox.Text) };
        }
        private void AddFilterButton_Click(object sender, RoutedEventArgs e)
        {
            FilterList.Add(new PropPair() { PropertyInfo = Prop.First(x => x.Name == PropertyInfoComboBox.SelectedItem.ToString()), Value = SearhTextBox.Text });
        }
        private void Search(string Str, string PropName)
        {
            int i;
            for (i = 0; i < Prop.Length; i++)
            {
                if (Prop[i].Name == PropName)
                {
                    break;
                }
            }
            List<СomparisonNameID> Search = new Network.Item.ItemSearch().Get<List<СomparisonNameID>>(new WrapNetClient(), new object[] { Str, i });
            foreach (СomparisonNameID item in Search) { SearchList.Add(item); }
        }
        private void SetSeleValue(int Sale, int MarkupBox) { ActiveValue.SaleValue = new int[] { Sale, MarkupBox }; }
        private void AppFilter(object Obj, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {

                if (FilterList.Count == 1)
                {
                    foreach (object item in Obj as IEnumerable)
                    {
                        PropPair X = (PropPair)item;
                        Search(X.Value.ToString(), X.PropertyInfo.Name);
                    }
                }
                else
                {
                    var X = (PropPair)((e.NewItems)[0]);
                    int i;
                    for (i = 0; i < Prop.Length; i++)
                    {
                        if (Prop[i].Name == X.PropertyInfo.Name)
                        {
                            break;
                        }
                    }
                    List<int> IDs = new List<int>();

                    foreach (var item in SearchList)
                    {
                        IDs.Add(item.Id);
                    }

                    List<СomparisonNameID> Search = new Network.Item.ItemSearchFromList().Get<List<СomparisonNameID>>(new WrapNetClient(), new object[] { IDs, X.Value, i });
                    SearchList.Clear();
                    foreach (СomparisonNameID item in Search) { SearchList.Add(item); }
                }

            }
        }
        private void RenewCash_Click(object sender, RoutedEventArgs e) { new Network.Item.RenewNameCash().Get<bool>(new WrapNetClient()); }

    }






}
