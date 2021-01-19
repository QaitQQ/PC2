
using Client;
using StructLibs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace WinFormsClientLib.Forms.WPF.Controls.ItemControls
{
    /// <summary>
    /// Форма сравнения позиций, данные тянет напрямую с сервера.
    /// </summary>
    public partial class ItemComparer : UserControl
    {
        private FillTable FillTable { get; set; }
        public BindingList<ItemChanges> CommonItemSourse { get; set; }
        System.Reflection.PropertyInfo[] Prop;
        public ItemComparer()
        {
            InitializeComponent();
            CommonItemSourse = new BindingList<ItemChanges>();
            CommonGrid.ItemsSource = CommonItemSourse;
            Prop = ItemDBStruct.GetProperties();

            Button SiteLoadButton = new Button() { Content = "Загрузить Лист с Сайта", Height = 20 };
            SiteLoadButton.Click += new RoutedEventHandler(SiteApiLoadAll_Click);

            Button Retun_Compare_RC = new Button() { Content = "Сопостовление с разницев в РЦ", Height = 20 };
            Retun_Compare_RC.Click += new RoutedEventHandler(Retun_Compare_RC_Click);

            Button Retun_New_Names = new Button() { Content = "Новые позиции", Height = 20 };
            Retun_New_Names.Click += new RoutedEventHandler(Retun_New_Names_Click);

            Button DelDuble = new Button() { Content = "Удалить дубликаты", Height = 20 };
            DelDuble.Click += new RoutedEventHandler(DelDuble_Click);

            UIElement[] Buttons = new UIElement[] {
                SiteLoadButton,
                Retun_Compare_RC,
                Retun_New_Names,
                DelDuble
            };
            ButtonStack.ItemsSource = Buttons;
        }

        private void SiteApiLoadAll_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() => SiteCompare(ButtonRenew, FillAtList, (Button)sender));
            FillTable = FillTable.СhangedSiteTable;

            void SiteCompare(Action<Button, string, bool> ButtonRenew, Action<List<ItemChanges>> FillAtList, Button Button)
            {
                int Code = new Network.Item.SiteApi.RenewSiteList().Get<int>(new WrapNetClient());
                ButtonRenew(Button, "Запросили", false);
                bool Run = false;
                ButtonRenew.Invoke(Button, "Исправляем имена", false);
                while (!Run)
                {
                    Run = new Network.Item.SiteApi.FixNameFromSiteList().Get<bool>(new WrapNetClient(), Code);
                    Thread.Sleep(1000);
                }
                Run = false;
                ButtonRenew.Invoke(Button, "Сравниваем с базой", false);
                while (!Run)
                {
                    Run = new Network.Item.SiteApi.ComparisonWithDB().Get<bool>(new WrapNetClient(), Code);

                    Thread.Sleep(1000);
                }
                Run = false;
                ButtonRenew.Invoke(Button, "Ждем списка", false);
                while (!Run)
                {
                    object List = new Network.Item.SiteApi.ReturnComparisonWithDB().Get<object>(new WrapNetClient(), Code);
                    if (List is bool)
                    {
                        Run = (bool)List;
                    }
                    else
                    {
                        FillAtList((List<ItemChanges>)List);
                        Run = true;
                    }
                    Thread.Sleep(1000);
                }
                ButtonRenew.Invoke(Button, "Загрузить Лист с Сайта", true);
            }

        }  // сравнение и возврат разницы с сайтом
        private void Retun_Compare_RC_Click(object sender, EventArgs e)
        {
            FillTable = FillTable.СhangedItemsTable;
            FillAtList(new Network.Item.Changes.GetChanges().Get<List<ItemChanges>>(new WrapNetClient()));
        } // сравнение с базой данных разница только в рознице    
        private void Retun_New_Names_Click(object sender, EventArgs e)
        {
            FillTable = FillTable.NewItemTable;
            FillAtList(new Network.Item.Changes.GetNewList().Get<List<ItemChanges>>(new WrapNetClient()));
        } //возвращаем неприкаянные позиции
        private void ApplyAllButton_Click(object sender, RoutedEventArgs e)
        {
            new Network.Item.Changes.AllowAllChanges().Get<List<string>>(new WrapNetClient());      
        }
        private void DelDuble_Click(object sender, RoutedEventArgs e) 
        {
            CommonItemSourse.Clear();
            var X = new Network.Item.RemoveDuplicate().Get<List<string>>(new WrapNetClient());

            foreach (var item in X)
            {
                CommonItemSourse.Add(new ItemChanges() { FieldName = item });
            }
           


        }
        private void Table_Button_Click(object sender, RoutedEventArgs e) // клик на позиции в таблице
        {
            int I = (int)(((Button)sender).Tag);
            if (I < CommonItemSourse.Count && CommonItemSourse.Count!= 0)
            {
                ItemChanges X = CommonItemSourse[I];
                switch (FillTable)
                {
                    case FillTable.СhangedItemsTable:
                        UpdateMappedPosition(X);
                        break;
                    case FillTable.NewItemTable:

                        new Network.Item.AddNewItemFromList().Get<bool>(new WrapNetClient(), X.ItemName);  

                        break;
                    case FillTable.СhangedSiteTable:
                        SetPrice(X);
                        break;
                    default:
                        break;
                }
                CommonItemSourse.Remove(X);
            }

            void UpdateMappedPosition(ItemChanges Item)
            {
                ItemPlusImageAndStorege Obj = new Network.Item.GetItemFromId().Get<ItemPlusImageAndStorege>(new WrapNetClient(), Item.ItemID);

                foreach (System.Reflection.PropertyInfo item in Prop)
                {
                    if (item.Name == Item.FieldName)
                    {
                        item.SetValue(Obj.Item, Item.NewValue);
                    }
                }

                Obj.Item.DateСhange = DateTime.Now;
                new Network.Item.EditItem().Get<bool>(new WrapNetClient(), Obj.Item);

                new Network.Item.Changes.DelFromСhangedList().Get<bool>(new WrapNetClient(), Item);
            }  // обновление при FillTable = СhangedItemsTable
            void SetPrice(ItemChanges Item)
            {
                Task.Factory.StartNew(() =>
                {
                    AddLogs(new Network.Item.SiteApi.UpdateSiteRC().Get<string>(new WrapNetClient(), Item));
                });
            } // обновляем цену на сайте   
        }
        private void FillAtList(List<ItemChanges> Query)
        {
            if (Dispatcher.CheckAccess())
            {
                CommonItemSourse.Clear();
                if (Query != null)
                {
                    foreach (ItemChanges item in Query)
                    {
                        CommonItemSourse.Add(item);
                    }
                    CommonGrid.Items.Refresh();
                }
            }
            else
            {
                Dispatcher.Invoke(() =>
                {
                    CommonItemSourse.Clear();
                    if (Query != null)
                    {
                        foreach (ItemChanges item in Query)
                        {
                            CommonItemSourse.Add(item);
                        }
                        CommonGrid.Items.Refresh();
                    }
                }
                );
            }
        } // заполняем лист
        private void ButtonRenew(Button Button, string Str, bool IsEnabled) => Button.Dispatcher.Invoke(() => { Button.Content = Str; Button.IsEnabled = IsEnabled; });
        private void AddLogs(string Str) => Dispatcher.Invoke(() => LogList.Items.Add(Str));

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

