using System;
using System.Windows.Forms;

namespace Client.Forms
{
    public partial class CreateEvents : Form
    {
        public CreateEvents()
        {
            InitializeComponent();

            Time2.Value = Time2.Value.AddDays(7);

            EventTypeBox.Items.Add("Звонок");

            EventTypeBox.Select(0, 1);
        }
        public string Content() => richTextBox1.Text;
        public DateTime Date1() => Time1.Value;

        public DateTime Date2() => Time2.Value;

        public int TypeContact() => 1;

        private void OK_Click(object sender, EventArgs e)
        {

        }
    }
}
