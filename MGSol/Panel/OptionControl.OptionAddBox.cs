using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
namespace MGSol.Panel
{
    public partial class OptionControl
    {
        public class OptionAddBox : Window
        {
            public StructLibCore.Marketplace.APISetting ApiSetting { get; set; }
            public bool Qadd;
            public OptionAddBox(StructLibCore.Marketplace.APISetting Setting = null)
            {
                Qadd = false;
                ApiSetting = Setting;
                Grid MainGrid = new();
                Height = 150;
                MainGrid.RowDefinitions.Add(new RowDefinition());
                MainGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(45) });
                Grid NoteGrid = new();
                Grid.SetRow(NoteGrid, 0);
                Grid ButtonGrid = new();
                Grid.SetRow(ButtonGrid, 1);
                NoteGrid.ColumnDefinitions.Add(new ColumnDefinition());
                NoteGrid.ColumnDefinitions.Add(new ColumnDefinition());
                NoteGrid.ColumnDefinitions.Add(new ColumnDefinition());
                NoteGrid.ColumnDefinitions.Add(new ColumnDefinition());
                TextBox NameBox = new();
                if (Setting != null)
                {
                    NameBox.Text = Setting.Name;
                }
                TextBox INNBOX = new();
                if (Setting != null)
                {
                    INNBOX.Text = Setting.INN;
                }
                INNBOX.TextChanged += (x, e) =>
                {
                    if (Setting == null)
                    {
                        Setting = new StructLibCore.Marketplace.APISetting();
                    }
                    Setting.INN = INNBOX.Text;
                };
                ComboBox Type = new()
                {
                    ItemsSource = Enum.GetValues(typeof(StructLibCore.Marketplace.MarketName))
                };
                Type.SelectedItem = Setting?.Type;
                ListBox listBox = new();
                listBox.ItemsSource = Setting?.ApiString;
                listBox.MouseRightButtonDown += (x, y) =>
                {
                    ContextMenu M = new();
                    TextBlock X = new() {Text = "Добавить" };
                    X.MouseLeftButtonDown += (x, y) =>
                    {
                        ModalBox MB = new();
                        if (Setting == null)
                        {
                            Setting = new StructLibCore.Marketplace.APISetting();
                        }
                        if ((bool)MB.ShowDialog())
                        {
                            List<string> lst = Setting.ApiString is null ? new List<string>() : Setting.ApiString.ToList();
                            lst.Add(MB.STR);
                            Setting.ApiString = lst.ToArray();
                            listBox.ItemsSource = Setting.ApiString;
                            MB.Close();
                        };
                    };
                    M.Items.Add(X);
                    M.IsOpen = true;
                };
                Grid.SetColumn(NameBox, 0);
                Grid.SetColumn(Type, 1);
                Grid.SetColumn(listBox, 3);
                Grid.SetColumn(INNBOX, 2);
                NoteGrid.Children.Add(NameBox);
                NoteGrid.Children.Add(Type);
                NoteGrid.Children.Add(listBox);
                NoteGrid.Children.Add(INNBOX);
                ButtonGrid.ColumnDefinitions.Add(new ColumnDefinition());
                ButtonGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(70) });
                ButtonGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(70) });
                ButtonGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(70) });
                Button OK = new() { Content = "Копировать", Height = 35, Width = 60, Margin = new Thickness(5) };
                OK.Click += (x, y) =>
                {
                    List<string> mass = new();
                    foreach (object item in listBox.Items)
                    {
                        mass.Add(item.ToString());
                    }
                    ApiSetting = new StructLibCore.Marketplace.APISetting() { ApiString = mass.ToArray(), Name = NameBox.Text };
                    if (Type.SelectedItem != null)
                    {
                        ApiSetting.Type = (StructLibCore.Marketplace.MarketName)Type.SelectedItem;
                    }
                    Qadd = true;
                    DialogResult = true; Close();
                };
                Grid.SetColumn(OK, 2);
                Button Save = new() { Content = "Сохранить", Height = 35, Width = 60, Margin = new Thickness(5) };
                Save.Click += (x, y) =>
                {

                    if (ApiSetting == null) { ApiSetting = new StructLibCore.Marketplace.APISetting(); }

                    if (NameBox.Text != "")
                    {
                        ApiSetting.Name = NameBox.Text;
                    }
                    if (Type.SelectedItem != null)
                    {
                        ApiSetting.Type = (StructLibCore.Marketplace.MarketName)Type.SelectedItem;
                    }


                    if (INNBOX.Text != "")
                    {
                        ApiSetting.INN = INNBOX.Text;
                    }

                    if (listBox.Items.Count > 0)
                    {
                        List<string> lst = new();
                        foreach (object item in listBox.Items)
                        {
                            lst.Add(item.ToString());
                        }
                        ApiSetting.ApiString = lst.ToArray();
                    }

                    Qadd = false;
                    DialogResult = true; Close();
                };
                Grid.SetColumn(Save, 1);
                Button Cancel = new() { Content = "Отменить", Height = 35, Width = 60, Margin = new Thickness(5), IsCancel = true };
                Grid.SetColumn(Cancel, 3);
                ButtonGrid.Children.Add(OK);
                ButtonGrid.Children.Add(Save);
                ButtonGrid.Children.Add(Cancel);
                MainGrid.Children.Add(NoteGrid);
                MainGrid.Children.Add(ButtonGrid);
                AddChild(MainGrid);
            }
        }
    }
}
