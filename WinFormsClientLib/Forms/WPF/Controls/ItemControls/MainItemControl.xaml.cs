
using Client;

using StructLibs;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
        public event Action<int, int> ChangeSale;
        private int[] _SaleValue;
        private int[] SaleValue { get => _SaleValue; set { _SaleValue = value; ChangeSale?.Invoke(SaleValue[0], SaleValue[1]); } }
        public MainItemControl()
        {
            InitializeComponent();
            SearchList = new ObservableCollection<СomparisonNameID>();
            Prop = ItemDBStruct.GetProperties();
            FilterList = new ObservableCollection<PropPair>();
            foreach (System.Reflection.PropertyInfo item in Prop)
            {
                PropertyInfoComboBox.Items.Add(item.Name);
                if (item.Name == "Name") { PropertyInfoComboBox.SelectedIndex = PropertyInfoComboBox.Items.Count - 1; }
            }
            ButtonStack.Items.Add(new Button() { Name = "Plus", Height = 20, Content = "Кнопка" });
            FilterStack.ItemsSource = FilterList;
            ItemSearchListBox.ItemsSource = SearchList;
            SaleBox.TextChanged += new TextChangedEventHandler(SaleChange);
            MarkupBox.TextChanged += new TextChangedEventHandler(SaleChange);
            ButtonStack.Items.Add(new PercentCalc());
            FilterList.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(AppFilter);
        }
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            SearchList.Clear();
            Search(SearhTextBox.Text, PropertyInfoComboBox.SelectedItem.ToString());
            ItemSearchListBox.Items.Refresh();
        }
        private void ItemSearchListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                СomparisonNameID X = e.AddedItems[0] as СomparisonNameID;           
                ItemNetStruct Obj = new Network.Item.GetItemFromId().Get<ItemNetStruct>(new WrapNetClient(), X.Id.ToString());
                ItemControl NewControl = new ItemControl(Obj);
                NewControl.GoSite += ((Client.Forms.Mainform)Client.Main.CommonWindow).GoSite;
                if (ItemDescriptionBox.Children.Count < 3) { ItemDescriptionBox.Children.Add(NewControl); }
                else { ItemDescriptionBox.Children.Clear(); ItemDescriptionBox.Children.Add(NewControl); }
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
            var Search = new Network.Item.ItemSearch().Get<List<СomparisonNameID>>(new WrapNetClient(), new object[] { Str, i}); 
            foreach (var item in Search) { SearchList.Add(item); }
        }
        private void AppFilter(object Obj, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (object item in Obj as IEnumerable)
                {
                    PropPair X = (PropPair)item;
                    Search(X.Value.ToString(), X.PropertyInfo.Name);
                }
            }
        }
    }
}
