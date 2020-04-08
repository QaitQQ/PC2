using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsClientLibrary.Forms
{
    public partial class StringModalBox : Form
    {
        public string STR;
        public StringModalBox ( )
        {
            InitializeComponent();
        }

        private void button1_Click ( object sender, EventArgs e )
        {
            STR = textBox1.Text;
        }

    }
}
