using System;
using System.Windows.Forms;

using WindowsFormsClientLibrary.Forms;

namespace Client.Forms
{
    public partial class Mainform : Form
    {
        private readonly CRMForm cRM;
        private readonly ItemForm itemForm;

        public Mainform()
        {
            InitializeComponent();
            cRM = new CRMForm();
            itemForm = new ItemForm();
            cRM.MdiParent = this;
            itemForm.MdiParent = this;
            FormActivator(itemForm);

        }

        private void FormActivator(Form Form)
        {
            if (Form.Visible)
            {
                Form.Hide();
            }
            else
            {
                Form.Show();
                Form.Activate();
                Form.WindowState = FormWindowState.Maximized;
            }


        }

        private void cRMToolStripMenuItem_Click(object sender, EventArgs e) => FormActivator(cRM);

        private void ItemToolStripMenuItem_Click(object sender, EventArgs e) => FormActivator(itemForm);

        private void Mainform_FormClosed(object sender, FormClosedEventArgs e) => Main.АuthorizationWindow.Close();

        private void NetToolStripMenuItem_Click(object sender, EventArgs e) => new Config();

        private void DictionariesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new DictionaryConfig();
        }
    }
}
