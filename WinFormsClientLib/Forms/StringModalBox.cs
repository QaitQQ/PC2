using System;
using System.Windows.Forms;

namespace WindowsFormsClientLibrary.Forms
{
    public partial class StringModalBox : Form
    {
        public string STR;
        public StringModalBox()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            STR = textBox1.Text;
        }

    }
}
