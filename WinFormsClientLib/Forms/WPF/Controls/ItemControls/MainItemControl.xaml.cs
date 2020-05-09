using Client.Class.Net;

using StructLibs;

using System;
using System.Collections;
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
            foreach (var item in Prop)
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
                var X = e.AddedItems[0] as СomparisonNameID;
                ItemNetStruct Obj = new ItemNetClass(X.Id.ToString()).Retun_Item_And_Image() as ItemNetStruct;

                var NewControl = new ItemControl(Obj);
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
                    foreach (var item in Str)
                    {
                        if (char.IsDigit(item)) { X += item; }
                    }
                    return Convert.ToInt32(X);
                }
                return 0;
            }

            SaleValue = new int[] { ConvertToInt(SaleBox.Text), ConvertToInt(MarkupBox.Text) };
        } 
        private void AddFilterButton_Click(object sender, RoutedEventArgs e) => FilterList.Add(new PropPair() { PropertyInfo = Prop.First(x => x.Name == PropertyInfoComboBox.SelectedItem.ToString()), Value = SearhTextBox.Text });
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
            var Search = new ItemNetClass(Str).Retun_Item_List(i);
            foreach (var item in Search) { SearchList.Add(item); }
        }
        private void AppFilter(object Obj, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in Obj as IEnumerable)
                {
                    var X = (PropPair)item;
                    Search(X.Value.ToString(), X.PropertyInfo.Name);
                }
            }
        }      
    }
}

//        private void Del_From_ID(DataGridViewCellEventArgs e)
//        {
//            int id = Convert.ToInt32(MainFieldTable.Rows[e.RowIndex].Cells[1].Value);
//            Task.Factory.StartNew(() => new ItemNetClass().Del_Item_From_ID(id));
//        }