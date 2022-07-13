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

            public bool Qadd;
            public OptionAddBox(StructLibCore.Marketplace.APISetting Setting = null)
            {
                Qadd = false;
                this.Setting = Setting;
                Grid MainGrid = new();
                this.Height = 150;

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
                INNBOX.TextChanged += (x, e) => { Setting.INN = INNBOX.Text; };


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
                    Qadd = true;
                    DialogResult = true; this.Close();
                };
                Grid.SetColumn(OK, 2);
                Button Save = new() { Content = "Сохранить", Height = 35, Width = 60, Margin = new Thickness(5) };
                Save.Click += (x, y) => 
                {
                    Qadd = false;
                    DialogResult = true; this.Close();
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
