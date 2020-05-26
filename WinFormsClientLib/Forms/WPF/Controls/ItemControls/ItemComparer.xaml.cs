
using StructLibs;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

using static NetEnum.Selector;

namespace WinFormsClientLib.Forms.WPF.Controls.ItemControls
{
    /// <summary>
    /// Логика взаимодействия для ItemComparer.xaml
    /// </summary>
    public partial class ItemComparer : UserControl
    {
        private FillTable FillTable { get; set; }
        public BindingList<СomparisonItems> CommonItemSourse { get; set; }
        public ItemComparer()
        {
            InitializeComponent();
            CommonItemSourse = new BindingList<СomparisonItems>();
            CommonGrid.ItemsSource = CommonItemSourse;

            //Button SiteLoadButton = new Button() { Content = "SiteLoad", Height = 20 };
            //SiteLoadButton.Click += new RoutedEventHandler(SiteApiLoadAll_Click);

            //Button RetunCompareNamesButton = new Button() { Content = "RetunCompareNames", Height = 20 };
            //RetunCompareNamesButton.Click += new RoutedEventHandler(Retun_Compare_Names_Click);

            //Button Retun_Compare_RC = new Button() { Content = "Retun_Compare_RC", Height = 20 };
            //Retun_Compare_RC.Click += new RoutedEventHandler(Retun_Compare_RC_Click);

            //Button Retun_New_Names = new Button() { Content = "Retun_New_Names", Height = 20 };
            //Retun_New_Names.Click += new RoutedEventHandler(Retun_New_Names_Click);

            //Button Get_Mapped = new Button() { Content = "Get_Mapped", Height = 20 };
            //Get_Mapped.Click += new RoutedEventHandler(Get_Mapped_Click);

            //Button Retun_Compare_Site = new Button() { Content = "Retun_Compare_Site", Height = 20 };
            //Retun_Compare_Site.Click += new RoutedEventHandler(Retun_Compare_Site_Click);

            //Button DelDouble = new Button() { Content = "DelDouble", Height = 20 };
            //Retun_Compare_Site.Click += new RoutedEventHandler(DelDouble_Click);

            //UIElement[] Buttons = new UIElement[] { SiteLoadButton, RetunCompareNamesButton, Retun_Compare_RC, Retun_New_Names, Get_Mapped, Retun_Compare_Site };

            //ButtonStack.ItemsSource = Buttons;
        }
        private void SiteApiLoadAll_Click(object sender, EventArgs e)
        {
          //  new ItemNetClass().LoadAllItemFromSite();
        }
        private void Retun_Compare_Names_Click(object sender, EventArgs e)
        {
            FillTable = FillTable.СhangedItemsTableSelected;
        //    FillAtList(new ItemNetClass().Retun_Compare_Names());
        }
        private void Retun_Compare_RC_Click(object sender, EventArgs e)
        {
            FillTable = FillTable.СhangedItemsTable;
         //   FillAtList(new ItemNetClass().Retun_Compare_RC());
        }
        private void FillAtList(List<СomparisonItems> Query)
        {
            CommonItemSourse.Clear();
            if (Query != null)
            {
                foreach (СomparisonItems item in Query)
                {
                    CommonItemSourse.Add(item);
                }
                CommonGrid.Items.Refresh();
            }
        }
        private void Retun_New_Names_Click(object sender, EventArgs e)
        {
            FillTable = FillTable.NewItemTable;
           // FillAtList(new ItemNetClass().Retun_New_Names());
        }
        private void Get_Mapped_Click(object sender, EventArgs e)
        {
            FillTable = FillTable.СhangedSiteTable;
          //  FillAtList(new ItemNetClass().CompareFromSite());
        }
        private void Retun_Compare_Site_Click(object sender, EventArgs e)
        {
            FillTable = FillTable.СhangedSiteTable;
         //   FillAtList(new ItemNetClass().Retun_Compare_Site());
        }
        private void DelDouble_Click(object sender, EventArgs e)
        {
         //   new ItemNetClass().DelDouble();
        }
        private void ApplyAllButton_Click(object sender, RoutedEventArgs e)
        {
            if (FillTable == FillTable.СhangedItemsTableSelected)
            {
                List<string> List = new List<string>();
                foreach (СomparisonItems item in CommonItemSourse)
                {
                    List.Add(item.BaseId.ToString());
                }
              //  Task.Factory.StartNew(() => new ItemNetClass().AllowAll(FillTable, List));
            }
            else
            {
             //   new ItemNetClass().AllowAll(FillTable);
            }
        }
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            СomparisonItems X = CommonItemSourse[(int)(((Button)sender).Tag)];
            switch (FillTable)
            {
                case FillTable.СhangedItemsTable:
                    UpdateMappedPosition(X);
                    break;
                case FillTable.NewItemTable:
                    Add_New_position_from_СhangeList(X);
                    break;
                case FillTable.СhangedSiteTable:
                    SetPrice(X.SiteId, X.NewRC);
                    break;
                case FillTable.СhangedItemsTableSelected:
                    break;
                default:
                    break;
            }
            CommonItemSourse.Remove(X);

        }
        private void UpdateMappedPosition(СomparisonItems Item)
        {
          //  ItemNetStruct Obj = new ItemNetClass(Item.BaseId).Retun_Item_And_Image() as ItemNetStruct;
         //   Obj.Item.PriceRC = Item.NewRC;
         //   Obj.Item.PriceDC = Item.NewDC;
         //   Obj.Item.Description = Item.Description;
         //   Obj.Item.SourceName = Item.SourceName;
         //   Obj.Item.DateСhange = DateTime.Now;
          //  new ItemNetClass().EditItemFromDBAndDelFromMappedList(Obj.Item);
        }
        private void ResultRenew(string STR) { }
        private sealed class Setprice
        {
            public void SetPriceFromSite(int v1, double v2, Action<string> action)
            {
            //    bool I = new ItemNetClass().SetPrice(v1, v2);
             //   action(v1.ToString() + I.ToString());
            }
        }
        private void Add_New_position_from_СhangeList(СomparisonItems Item)
        {
           // Task.Factory.StartNew(() => new ItemNetClass().Add_New_position_from_СhangeList(Item.СomparisonName));
        }
        private void SetPrice(int ID, double NewPrice)
        {
            Task.Factory.StartNew(() => new Setprice().SetPriceFromSite(ID, NewPrice, ResultRenew));
        }
    }
}

