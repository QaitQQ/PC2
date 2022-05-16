using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace MGSol.Panel
{
    public partial class OptionControl
    {
        public class OptionAddBox : Window
        {
            public StructLibCore.Marketplace.APISetting Setting { get; set; }
            public OptionAddBox(StructLibCore.Marketplace.APISetting Setting = null)
            {
                this.Setting = Setting;
                Grid MainGrid = new();

                MainGrid.RowDefinitions.Add(new RowDefinition());
                MainGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(45) });

                Grid NoteGrid = new();
                Grid.SetRow(NoteGrid, 0);
                Grid ButtonGrid = new();
                Grid.SetRow(ButtonGrid, 1);
                NoteGrid.ColumnDefinitions.Add(new ColumnDefinition());
                NoteGrid.ColumnDefinitions.Add(new ColumnDefinition());
                NoteGrid.ColumnDefinitions.Add(new ColumnDefinition());

                TextBox NameBox = new();
                if (Setting != null)
                {
                    NameBox.Text = Setting.Name;
                }
                ComboBox Type = new()
                {
                    ItemsSource = Enum.GetValues(typeof(StructLibCore.Marketplace.MarketName))
                };
                Type.SelectedItem = Setting?.Type;
                ListBox listBox = new();
                listBox.ItemsSource = Setting?.ApiString;

                listBox.MouseRightButtonDown += (x, y) =>
                {
                    var M = new ContextMenu();
                    var X = new TextBlock() { Text = "Добавить" };
                    X.MouseLeftButtonDown += (x, y) =>
                    {
                        var MB = new ModalBox();

                        if ((bool)MB.ShowDialog())
                        {
                            listBox.Items.Add(MB.STR);
                            MB.Close();
                        };
                    };
                    M.Items.Add(X);
                    M.IsOpen = true;
                };

                Grid.SetColumn(NameBox, 0);
                Grid.SetColumn(Type, 1);
                Grid.SetColumn(listBox, 2);
                NoteGrid.Children.Add(NameBox);
                NoteGrid.Children.Add(Type);
                NoteGrid.Children.Add(listBox);

                ButtonGrid.ColumnDefinitions.Add(new ColumnDefinition());
                ButtonGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(70) });
                ButtonGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(70) });

                Button OK = new() { Content = "Добавить", Height = 35, Width = 60, Margin = new Thickness(5) };
                OK.Click += (x, y) =>
                {

                    var mass = new List<string>();
                    foreach (var item in listBox.Items)
                    {
                        mass.Add(item.ToString());
                    }
                    this.Setting = new StructLibCore.Marketplace.APISetting() { ApiString = mass.ToArray(), Name = NameBox.Text };

                    if (Type.SelectedItem != null)
                    {
                        this.Setting.Type = (StructLibCore.Marketplace.MarketName)Type.SelectedItem;
                    }
                    DialogResult = true; this.Close();
                };
                Grid.SetColumn(OK, 1);

                Button Cancel = new() { Content = "Отменить", Height = 35, Width = 60, Margin = new Thickness(5), IsCancel = true };
                Grid.SetColumn(Cancel, 2);

                ButtonGrid.Children.Add(OK);
                ButtonGrid.Children.Add(Cancel);

                MainGrid.Children.Add(NoteGrid);
                MainGrid.Children.Add(ButtonGrid);
                AddChild(MainGrid);
            }
        }
    }
}
