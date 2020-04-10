using Server.Class.HDDClass;

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WindowsFormsClientLibrary.Forms
{
    public partial class Config : Form
    {
        public Config()
        {
            InitializeComponent();

            dataGridView1.Columns.Add("", "");
            dataGridView1.Columns.Add("", "");
            foreach (KeyValuePair<string, string> item in WindowsFormsClientLibrary.Class.ClientConfig.ConfigValue)
            {
                dataGridView1.Rows.Add(item.Key, item.Value);
            }

            Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           var List  = new List<KeyValuePair<string, string>>();
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                List.Add(new KeyValuePair<string, string>(dataGridView1.Rows[i].Cells[0].Value.ToString(), dataGridView1.Rows[i].Cells[1].Value.ToString()));
            }

            Class.ClientConfig.ConfigValue = List;

            Close();
        }
    }
}
