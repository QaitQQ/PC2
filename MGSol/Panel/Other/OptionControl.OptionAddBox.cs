using StructLibCore;
using StructLibCore.Marketplace;

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
            public APISetting ApiSetting { get; set; }
            public bool Qadd;
            private int SiAs;

            private string SiText;
            public OptionAddBox(APISetting Setting = null)
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
                    Setting ??= new APISetting();
                    Setting.INN = INNBOX.Text;
                };
                ComboBox Type = new()
                {
                    ItemsSource = Enum.GetValues(typeof(MarketName)),
                    SelectedItem = Setting?.Type
                };
                ListBox listBox = new()
                {
                    ItemsSource = Setting?.ApiString
                };
                //foreach (Control item in listBox.Items)
                //{
                //    item.MouseRightButtonDown += (x, e) => 
                //    {
                //    };
                //} 
                listBox.SelectionChanged += (x, e) =>
                {
                    SiAs = ((ListBox)x).SelectedIndex;
                    SiText = ((ListBox)x).SelectedItem.ToString();
                    ContextMenu M = new();
                    TextBlock X = new() { Text = "Удалить" };
                    X.MouseLeftButtonDown += (o, y) =>
                    {
                    };
                    _ = M.Items.Add(X);
                    TextBlock С = new() { Text = "Изменить" };
                    С.MouseLeftButtonDown += (o, y) =>
                    {
                        ApiSetting.ApiString[SiAs] = "123";
                        ContextMenu N = new();
                        TextBox block = new()
                        {
                            Text = SiText,
                            Width = 100
                        };
                        _ = N.Items.Add(block);
                        N.IsOpen = true;
                        N.Closed += (o, e) =>
                        {
                            ApiSetting.ApiString[SiAs] = block.Text;
                        };
                    };
                    _ = M.Items.Add(С);
                    M.IsOpen = true;
                };
                listBox.MouseRightButtonDown += (x, y) =>
                {
                    ContextMenu M = new();
                    TextBlock X = new() { Text = "Добавить" };
                    X.MouseLeftButtonDown += (x, y) =>
                    {
                        ModalBox MB = new();
                        Setting ??= new APISetting();
                        if ((bool)MB.ShowDialog())
                        {
                            List<string> lst = Setting.ApiString is null ? new List<string>() : Setting.ApiString.ToList();
                            lst.Add(MB.STR);
                            Setting.ApiString = lst.ToArray();
                            listBox.ItemsSource = Setting.ApiString;
                            MB.Close();
                        };
                    };
                    _ = M.Items.Add(X);
                    M.IsOpen = true;
                };
                Grid.SetColumn(NameBox, 0);
                Grid.SetColumn(Type, 1);
                Grid.SetColumn(listBox, 3);
                Grid.SetColumn(INNBOX, 2);
                _ = NoteGrid.Children.Add(NameBox);
                _ = NoteGrid.Children.Add(Type);
                _ = NoteGrid.Children.Add(listBox);
                _ = NoteGrid.Children.Add(INNBOX);
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
                        ApiSetting.Type = (MarketName)Type.SelectedItem;
                    }
                    Qadd = true;
                    DialogResult = true; Close();
                };
                Grid.SetColumn(OK, 2);
                Button Save = new() { Content = "Сохранить", Height = 35, Width = 60, Margin = new Thickness(5) };
                Save.Click += (x, y) =>
                {
                    ApiSetting ??= new APISetting();
                    if (NameBox.Text != "")
                    {
                        ApiSetting.Name = NameBox.Text;
                    }
                    if (Type.SelectedItem != null)
                    {
                        ApiSetting.Type = (MarketName)Type.SelectedItem;
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
                _ = ButtonGrid.Children.Add(OK);
                _ = ButtonGrid.Children.Add(Save);
                _ = ButtonGrid.Children.Add(Cancel);
                _ = MainGrid.Children.Add(NoteGrid);
                _ = MainGrid.Children.Add(ButtonGrid);
                AddChild(MainGrid);
            }
        }
    }
}
