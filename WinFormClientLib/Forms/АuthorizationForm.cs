using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using WindowsFormsClientLibrary.Forms;

namespace Client.Forms
{
    public partial class АuthorizationForm : Form
    {
        public АuthorizationForm ( )
        {
            InitializeComponent();
            FillUserList();
        }

        private void FillUserList ( )
        {
            bool ConnectStatus = false;

            try
            {
                 ConnectStatus = new Class.Net.Аuthorization().ConnectStatus();
            }
            catch 
            {
                System.Windows.MessageBox.Show("Нет соединения с сервером");
            }
           

            if (ConnectStatus)
            {
                List<string> List = new Class.Net.Аuthorization().GetUsersList();
                foreach (string item in List) { comboBox1.Items.Add(item); }
                comboBox1.SelectedItem = comboBox1.Items[0];

            }

        }

        private void Ok_Button_Click ( object sender, EventArgs e )
        {
            if (textBox1.Text!=null)
            {
                if (new Class.Net.Аuthorization().SetToken(new string[] { comboBox1.SelectedItem.ToString(), textBox1.Text }))
                {
                    Mainform mainform = new Mainform();
                    mainform.Show();
                    Hide();
                }
                else { Ok_Button.BackColor = System.Drawing.Color.Red; }
            }
            else { Ok_Button.BackColor = System.Drawing.Color.Red; }
        }

        private void button3_Click ( object sender, EventArgs e ) => new Config();

        private void АuthorizationForm_KeyPress ( object sender, KeyPressEventArgs e )
        {
            var i = 1;
        }
    }
}