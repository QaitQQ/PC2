using Client;

using StructLibs;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace WinFormsClientLib.Forms.WPF.Controls
{
    /// <summary>
    /// Логика взаимодействия для UniversalDBItemFormControl.xaml
    /// </summary>
    public partial class UniversalDBItemFormControl : UserControl
    {
        private readonly ObservableCollection<UIElement> ControlsCollection;
        private readonly ObservableCollection<object> ObjList;
        public UniversalDBItemFormControl()
        {
            InitializeComponent();
            ControlsCollection = new ObservableCollection<UIElement>();
            ObjList = new ObservableCollection<object>();

            Button X = new Button() { Content = "Производители", Height = 20, Width = 40 };
            X.Click += Manufactor_Clik;
            ControlsCollection.Add(X);

            Button A = new Button() { Content = "Сайты Производителей", Height = 20, Width = 40 };
            A.Click += ManufactorSite_Clik;
            ControlsCollection.Add(A);

            ControlStack.ItemsSource = ControlsCollection;
            DataTable.ItemsSource = ObjList;
            ControlsCollection.Add(X);
        }
        private void FillTable(object List) { ObjList.Clear(); foreach (object item in List as IEnumerable) { ObjList.Add(item); } }
        private void Manufactor_Clik(object sender, RoutedEventArgs e) => FillTable(new Network.Other.GetManufactors().Get<List<Manufactor>>(new WrapNetClient()));
        private void ManufactorSite_Clik(object sender, RoutedEventArgs e) => FillTable(new Network.Other.GetManufSite().Get<object>(new WrapNetClient()));
        private void Table_Button_Click(object sender, RoutedEventArgs e)
        {
            var Z = DiologTwoString();

            var X = new ManufactorSite()
            {
                ManufactorId = ((Manufactor)(ObjList[Convert.ToInt32(((Button)sender).Tag)])).Id,
                SiteLink = Z[0],
                SearchLink = Z[1]
            };

            new Network.Other.NewManufSite().Get<bool>(new WrapNetClient(), X);

        }
        private void NewManufactorSite_Clik(object sender, RoutedEventArgs e)
        {

          
        }


        private string[] DiologTwoString()
        {

            System.Windows.Forms.Form Form = new System.Windows.Forms.Form();
            System.Windows.Forms.Button button1 = new System.Windows.Forms.Button();
            System.Windows.Forms.Button button2 = new System.Windows.Forms.Button();

            System.Windows.Forms.TextBox textBox1 = new System.Windows.Forms.TextBox();
            System.Windows.Forms.TextBox textBox2 = new System.Windows.Forms.TextBox();


            textBox1.Width = 200;
            textBox2.Width = 200;
            textBox1.Location = new System.Drawing.Point(10, 10);
            textBox2.Location = new System.Drawing.Point(textBox1.Left, textBox1.Height + textBox1.Top + 10);

            button1.Text = "OK";
            button1.Location = new System.Drawing.Point(textBox2.Left, textBox2.Height + textBox2.Top + 10);
            button2.Text = "Cancel";
            button2.Location = new System.Drawing.Point(button1.Left, button1.Height + button1.Top + 10);
            button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Form.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Form.Controls.Add(button1);
            Form.Controls.Add(button2);
            Form.Controls.Add(textBox1);
            Form.Controls.Add(textBox2);

            Form.ShowDialog();

            if (Form.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                return new string[] { textBox1.Text, textBox2.Text };

            }
            else
            {
                return null;
            }
        }


    }
}
