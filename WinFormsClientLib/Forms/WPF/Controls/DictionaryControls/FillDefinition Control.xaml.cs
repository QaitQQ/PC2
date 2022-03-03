using Object_Description;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace WinFormsClientLib.Forms.WPF.Controls.DictionaryControls
{
    /// <summary>
    /// Логика взаимодействия для FillDefinition_Control.xaml
    /// </summary>
    public partial class FillDefinition_Control : UserControl
    {
        public DictionaryPrice _dictionary { get; set; }
        private string _Name { get; set; }

        public ObservableCollection<object[]> Sourse { get; set; }
        public Array ComboBoxSourse { get; set; }
        public FillDefinition_Control(DictionaryPrice dictionary, string Name)
        {
            InitializeComponent();
            _dictionary = dictionary;
            _Name = Name;
            Sourse = new ObservableCollection<object[]>();
            ComboBoxSourse = Enum.GetValues(typeof(FillDefinitionPrice));

            if (_Name == "Filling_method_coll")
            {
                foreach (KeyValuePair<FillDefinitionPrice, int> item in _dictionary.Filling_method_coll)
                {
                    Sourse.Add(new object[] { item, ComboBoxSourse });
                }
            }
            else
            {
                foreach (KeyValuePair<FillDefinitionPrice, string> item in _dictionary.Filling_method_string)
                {
                    Sourse.Add(new object[] { item, ComboBoxSourse });
                }
            }
            ItemStack.ItemsSource = Sourse as IEnumerable;


        }
        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_Name == "Filling_method_coll")
            {
                var X = (KeyValuePair<FillDefinitionPrice, int>)((object[])((TextBlock)sender).DataContext)[0];
                var Menu = new ContextMenu();
                TextBlock btn = new TextBlock() { Text = "Удалить" };
                btn.MouseDown += (s, e) => { _dictionary.Filling_method_coll.Remove(X); };
                Menu.Items.Add(btn);
                Menu.IsOpen = true;
            }
            else
            {
                var X = (KeyValuePair<FillDefinitionPrice, string>)((object[])((TextBlock)sender).DataContext)[0];
                var Menu = new ContextMenu();
                TextBlock btn = new TextBlock() { Text = "Удалить" };
                btn.MouseDown += (s, e) => { _dictionary.Filling_method_string.Remove(X); };
                Menu.Items.Add(btn);
                Menu.IsOpen = true;
            }

        }

        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {


            var contrl = new UserControl();

            var win = new UniversalWPFContainerForm(contrl);


            contrl.Width = 200;
            contrl.Height = 200;

            var GrM = new Grid();
            GrM.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(25) });
            GrM.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(25) });
            GrM.ColumnDefinitions.Add(new ColumnDefinition());
            GrM.ColumnDefinitions.Add(new ColumnDefinition());

            var Tb = new TextBox();

            var Cb = new ComboBox();
            Cb.ItemsSource = ComboBoxSourse;

            GrM.Children.Add(Tb);
            GrM.Children.Add(Cb);

            Grid.SetColumn(Tb, 0);
            Grid.SetColumn(Cb, 1);
            Grid.SetRow(Tb, 0);
            Grid.SetRow(Cb, 0);

            var Btn1 = new Button() { Content = "Save" };
            Btn1.Click += (s, e) =>
            {
                if (_Name == "Filling_method_coll")
                {
                    _dictionary.Set_Filling_method_coll((FillDefinitionPrice)Cb.SelectedItem, converttoint(Tb.Text));
                }
                else
                {

                    _dictionary.Set_Filling_method_string((FillDefinitionPrice)Cb.SelectedItem, Tb.Text);
                }

                win.Close();
            };



            var Btn2 = new Button() { Content = "Cancel" };
            Btn2.Click += (s, e) => { win.Close(); };

            Grid.SetRow(Btn1, 1);
            Grid.SetRow(Btn2, 1);
            Grid.SetColumn(Btn1, 0);
            Grid.SetColumn(Btn2, 1);

            GrM.Children.Add(Btn1);
            GrM.Children.Add(Btn2);
            contrl.Content = GrM;
            win.Show();
        }

        int converttoint(string str) 
        {
            string Ns = null;

            foreach (var item in str)
            {
                if (char.IsDigit(item))
                {
                    Ns += item;
                }
            }

            return Convert.ToInt32(Ns);
        
        
        
        }
    }
}
