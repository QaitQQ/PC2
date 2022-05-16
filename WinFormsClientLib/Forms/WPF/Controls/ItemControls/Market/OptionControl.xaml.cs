using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WinFormsClientLib.Forms.WPF.Controls.ItemControls.Market
{
    /// <summary>
    /// Логика взаимодействия для OptionControl.xaml
    /// </summary>
    public partial class OptionControl : UserControl
    {
        private ObservableCollection<StructLibCore.Marketplace.APISetting> ListOption;
        public OptionControl(List<StructLibCore.Marketplace.APISetting> Option)
        {
            InitializeComponent();
            ListOption = new ObservableCollection<StructLibCore.Marketplace.APISetting>();
      
            foreach (var item in Option)
            {
                ListOption.Add(item);
            }
            ListOption.CollectionChanged += (x, y) =>
           {
               var lst = new List<StructLibCore.Marketplace.APISetting>();
               foreach (var item in ListOption)
               {
                   lst.Add(item);
               }
               new Network.Item.MarketApi.SetListOption().Get<bool>(new Client.WrapNetClient(), lst);
              };

            OptionList.ItemsSource = ListOption;
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var OptionAddBox = new OptionAddBox();
            if ((bool)OptionAddBox.ShowDialog())
            {
                ListOption.Add(OptionAddBox.Setting);
            }
        }
        public class OptionAddBox : Window
        {
            public StructLibCore.Marketplace.APISetting Setting { get; set; }
            public OptionAddBox(StructLibCore.Marketplace.APISetting Setting = null)
            {
                this.Setting = Setting;
                Grid MainGrid = new Grid();

                MainGrid.RowDefinitions.Add(new RowDefinition());
                MainGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(45) });

                Grid NoteGrid = new Grid();
                Grid.SetRow(NoteGrid, 0);
                Grid ButtonGrid = new Grid();
                Grid.SetRow(ButtonGrid, 1);
                NoteGrid.ColumnDefinitions.Add(new ColumnDefinition());
                NoteGrid.ColumnDefinitions.Add(new ColumnDefinition());
                NoteGrid.ColumnDefinitions.Add(new ColumnDefinition());

                TextBox NameBox = new TextBox();
                if (Setting != null)
                {
                    NameBox.Text = Setting.Name;
                }


                ComboBox Type = new ComboBox
                {
                    ItemsSource = Enum.GetValues(typeof(StructLibCore.Marketplace.MarketName))
                };
                Type.SelectedItem = Setting?.Type;
                ListBox listBox = new ListBox();
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

                Button OK = new Button() { Content = "Добавить", Height = 35, Width = 60, Margin = new Thickness(5) };
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

                Button Cancel = new Button() { Content = "Отменить", Height = 35, Width = 60, Margin = new Thickness(5), IsCancel = true };
                Grid.SetColumn(Cancel, 2);

                ButtonGrid.Children.Add(OK);
                ButtonGrid.Children.Add(Cancel);

                MainGrid.Children.Add(NoteGrid);
                MainGrid.Children.Add(ButtonGrid);
                AddChild(MainGrid);
            }
        }
        private void DelButtonClick_Click(object sender, RoutedEventArgs e)
        {
            ListOption.Remove((StructLibCore.Marketplace.APISetting)((Button)sender).DataContext);
        }
        private void Grid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Grid grid = (Grid)sender;
            var OptionAddBox = new OptionAddBox((StructLibCore.Marketplace.APISetting)grid.DataContext);     


            if ((bool)OptionAddBox.ShowDialog())
            {
                ListOption.Add(OptionAddBox.Setting);
            }
        }
    }
}
