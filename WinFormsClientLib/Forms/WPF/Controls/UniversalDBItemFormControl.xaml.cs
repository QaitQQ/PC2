using Client;

using Network.CRM;

using Server;

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
        private Mode mode;
        private enum Mode 
        {

            Manufactor_Clik, ManufactorSite_Clik, Warehouse_Clik, Target_Clik


        }

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


            Button C = new Button() { Content = "Склады", Height = 20, Width = 40 };
            C.Click += Warehouse_Clik;
            ControlsCollection.Add(C);

            Button B = new Button() { Content = "Цели", Height = 20, Width = 40 };
            B.Click += Target_Clik;
            ControlsCollection.Add(B);

            ControlStack.ItemsSource = ControlsCollection;
            DataTable.ItemsSource = ObjList;
            ControlsCollection.Add(X);
        }

        private void Target_Clik(object sender, RoutedEventArgs e)
        {
            var V = new Network.Target.GetTarget().Get<List<Server.Target>>(new WrapNetClient());
            mode = Mode.Target_Clik;
            FillTable(V);
        }

        private void Warehouse_Clik(object sender, RoutedEventArgs e)
        {
            var V = new GetStorage().Get<List<Warehouse>>(new WrapNetClient());
            mode = Mode.Warehouse_Clik;
            FillTable(V);
        }

        private void FillTable(object List) { ObjList.Clear(); foreach (object item in List as IEnumerable) { ObjList.Add(item); } }
        private void Manufactor_Clik(object sender, RoutedEventArgs e) { mode = Mode.Manufactor_Clik; FillTable(new Network.Other.GetManufactors().Get<List<Manufactor>>(new WrapNetClient())); }
        private void ManufactorSite_Clik(object sender, RoutedEventArgs e) { mode = Mode.ManufactorSite_Clik; FillTable(new Network.Other.GetManufSite().Get<object>(new WrapNetClient())); }
        private void Table_Button_Click(object sender, RoutedEventArgs e)
        {

            switch (mode)
            {
                case Mode.Manufactor_Clik:
                    break;
                case Mode.ManufactorSite_Clik:
                    break;
                case Mode.Warehouse_Clik:
                    int I = (int)((Button)sender).Tag;
                    if (I < ObjList.Count && ObjList.Count != 0)
                    {
                        var X = ObjList[I];
                        var V = new RemoveStorage().Get<bool>(new WrapNetClient(), X);
                    }
                    break;
                case Mode.Target_Clik:
                    break;
            }




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
