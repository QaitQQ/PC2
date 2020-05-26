using Network;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WindowsFormsClientLibrary.Class;
using WindowsFormsClientLibrary.Forms;

namespace Client.Forms
{
    public partial class АuthorizationForm : Form
    {
        public АuthorizationForm()
        {
            InitializeComponent();

            int X = Convert.ToInt32(ClientConfig.GetValue("AutologinCheck"));
            if (X == 1)
            {
                AutologinCheck.Checked = true;
            }
            else
            {
                AutologinCheck.Checked = false;
            }
            FillUserList();
        }
        private void FillUserList()
        {
            List<string> List = new Network.Аuthorization.GetUserList().Get<List<string>>(new WrapNetClient());
            foreach (string item in List) { comboBox1.Items.Add(item); }
            comboBox1.SelectedItem = comboBox1.Items[0];
        }
        private void Ok_Button_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != null) { Login(comboBox1.SelectedItem.ToString(), textBox1.Text);   }
            else { Ok_Button.BackColor = System.Drawing.Color.Red; }
        }
        private void Login(string Login, string Pass)
        {
           var Token = new Network.Аuthorization.SetToken().Get<string>(new WrapNetClient(),new object[] { Login, Pass });

            if (Token != null)
            {
                Main.Token = Token;
                Main.CommonWindow = new Mainform();
                Main.CommonWindow.Show();
                this.Hide();
            }
            else { Ok_Button.BackColor = System.Drawing.Color.Red; }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            new Config();
        }
        private void АuthorizationForm_KeyPress(object sender, KeyPressEventArgs e) {    }
        private void AutologinCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (AutologinCheck.Checked)
            {
                ClientConfig.SetValue("AutologinCheck", "1");
            }
            else
            {
                ClientConfig.SetValue("AutologinCheck", "0");
            }
        }
        private void АuthorizationForm_Activated(object sender, EventArgs e)
        {
            if (AutologinCheck.Checked)
            {
                string Log = ClientConfig.GetValue("AutoLogin");
                string Pass = ClientConfig.GetValue("AutoPass");
                Login(Log, Pass);
            }
        }
    }
}