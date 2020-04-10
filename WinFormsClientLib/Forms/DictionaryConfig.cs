using Client.Class.Net;

using Object_Description;

using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WindowsFormsClientLibrary.Forms
{
    public partial class DictionaryConfig : Form
    {
        private Dictionaries Dictionaries;
        private IDictionaryPC ActiveDic;
        public DictionaryConfig()
        {
            InitializeComponent();
            Dictionaries = new BaseNetClass().GetDictionaries();
            comboBox1.DataSource = Enum.GetValues(typeof(DictionaryRelate));
            FillListBox();
            Column4.DataSource = Enum.GetValues(typeof(FillDefinition));
            Column4.ValueType = typeof(FillDefinition);
            Show();
        }
        private void FillListBox()
        {
            listBox1.Items.Clear();

            var Relate = (DictionaryRelate)comboBox1.SelectedItem;
            if (Relate != DictionaryRelate.None) { foreach (IDictionaryPC item in Dictionaries.GetDictionaryRelate(Relate)) { listBox1.Items.Add(item.Name); } }
            else { foreach (IDictionaryPC item in Dictionaries.GetAll()) { listBox1.Items.Add(item.Name); } }


        }
        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                ActiveDic = Dictionaries.Get(listBox1.SelectedItem.ToString());
                FillDataGrid(ActiveDic);
            }

        }
        private void FillDataGrid(IDictionaryPC T)
        {
            dataGridView1.Rows.Clear();

            if (T is DictionaryPrice && ((DictionaryPrice)T).Filling_method_string().Count != 0)
            {
                DictionaryPrice F = T as DictionaryPrice;

                foreach (System.Collections.Generic.KeyValuePair<FillDefinition, string> item in F.Filling_method_string())
                {
                    DataGridViewComboBoxCell X = new DataGridViewComboBoxCell();
                    X.Items.Add(item.Key.ToString());
                    dataGridView1.Rows.Add("", "", item.Value);
                    object D = dataGridView1.Rows[dataGridView1.Rows.Count - 2].Cells[3].Value = item.Key;
                }
            }

            if (dataGridView1.Rows.Count < 6)
            {
                for (int i = dataGridView1.Rows.Count; i < 6; i++)
                {
                    dataGridView1.Rows.Add();
                }
            }
            dataGridView1.Rows[0].Cells[0].Value = "Name";
            dataGridView1.Rows[1].Cells[0].Value = T.Name;
            dataGridView1.Rows[2].Cells[0].Value = "Relate";
            dataGridView1.Rows[4].Cells[0].Value = "ID";
            dataGridView1.Rows[5].Cells[0].Value = T.Id.ToString();

            for (int i = 0; i < T.Values.Count; i++)
            {
                if (dataGridView1.Rows.Count <= i)
                {
                    dataGridView1.Rows.Add();
                }
            }

            for (int i = 0; i < T.Values.Count; i++)
            {
                string D = T.Values[i];
                if (Regex.IsMatch(D, "[\n\r]"))
                {

                }
                dataGridView1.Rows[i].Cells[1] = new DataGridViewTextBoxCell() { Value = @D, };
            }
            DataGridViewComboBoxCell Relate = new DataGridViewComboBoxCell
            {
                DataSource = Enum.GetValues(typeof(DictionaryRelate)),
                ValueType = typeof(DictionaryRelate),
                Value = T.Relate
            };
            dataGridView1.Rows[3].Cells[0] = Relate;
        }
        private void Save_Click(object sender, EventArgs e)
        {


            if (listBox1.SelectedIndex == -1)
            {
                new BaseNetClass().SetDictionaries(Dictionaries);
            }
            else
            {
                IDictionaryPC NewDic;
                string Name = dataGridView1.Rows[1].Cells[0].Value.ToString();
                DictionaryRelate Relate = (DictionaryRelate)dataGridView1.Rows[3].Cells[0].Value;

                if (dataGridView1.Rows[0].Cells[2].Value != null && dataGridView1.Rows[0].Cells[2].Value.ToString() != "")
                {
                    NewDic = new DictionaryPrice(Name, Relate);
                    for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                    { ((DictionaryPrice)NewDic).Set_Filling_method_string((FillDefinition)dataGridView1.Rows[i].Cells[3].Value, dataGridView1.Rows[i].Cells[2].Value.ToString()); }
                }
                else { NewDic = new DictionaryBase(Name, Relate); }
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    if (dataGridView1.Rows[0].Cells[1].Value?.ToString() != "")
                    {
                        if (dataGridView1.Rows[i].Cells[1].Value != null && dataGridView1.Rows[i].Cells[1].Value.ToString() != "")
                        { NewDic.Values.Add(dataGridView1.Rows[i].Cells[1].Value.ToString()); }
                    }
                }
                if (dataGridView1.Rows[5].Cells[0].Value != null && dataGridView1.Rows[5].Cells[0].Value.ToString() != "")
                {
                    NewDic.Id = Convert.ToInt32(dataGridView1.Rows[5].Cells[0].Value);
                }
                Dictionaries.Renew(NewDic);
                new BaseNetClass().SetDictionaries(Dictionaries);
            }
            
        }
        private void AddDic_Button_Click(object sender, EventArgs e)
        {
            StringModalBox Window = new StringModalBox();
            Window.ShowDialog();
            if (Window.DialogResult == DialogResult.OK)
            {
                Dictionaries.Add(new DictionaryBase(Window.STR, DictionaryRelate.None));
                FillListBox();
            }

        }
        private void DelDic_Click(object sender, EventArgs e)
        {
            Dictionaries.Del(ActiveDic);
            FillListBox();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) { FillListBox(); }
        private void ManufFromSite_Click(object sender, EventArgs e)
        {
            new BaseNetClass().ManufFromSite();
            Dictionaries = new BaseNetClass().GetDictionaries();
            FillListBox();
        }
    }
}
