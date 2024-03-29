﻿
using Client;

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
            Dictionaries = new Network.Dictionary.GetDictionares().Get<Dictionaries>(new WrapNetClient());
            comboBox1.DataSource = Enum.GetValues(typeof(DictionaryRelate));
            FillListBox();
            Show();
        }
        private void FillListBox()
        {
            listBox1.Items.Clear();
            DictionaryRelate Relate = (DictionaryRelate)comboBox1.SelectedItem;
            if (Relate != DictionaryRelate.None) { foreach (IDictionaryPC item in Dictionaries.GetDictionaryRelate(Relate)) { listBox1.Items.Add(item.Name); } }
            else { foreach (IDictionaryPC item in Dictionaries.GetAll()) { listBox1.Items.Add(item.Name); } }
        }
        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                ActiveDic = Dictionaries.Get(listBox1.SelectedItem.ToString());
                if (ActiveDic.Relate == DictionaryRelate.Price)
                {
                    Column4.DataSource = Enum.GetValues(typeof(FillDefinitionPrice));
                    Column4.ValueType = typeof(FillDefinitionPrice);
                }

                FillDataGrid(ActiveDic);
            }

        }
        private void FillDataGrid(IDictionaryPC T)
        {
            dataGridView1.Rows.Clear();

            if (T is DictionaryPrice && ((DictionaryPrice)T).Filling_method_string.Count != 0)
            {
                DictionaryPrice F = T as DictionaryPrice;

                foreach (System.Collections.Generic.KeyValuePair<FillDefinitionPrice, string> item in F.Filling_method_string)
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
                new Network.Dictionary.SetDictionares().Get<bool>(new WrapNetClient(), Dictionaries);
            }
            else
            {

                string Name = dataGridView1.Rows[1].Cells[0].Value.ToString();
                DictionaryRelate Relate = (DictionaryRelate)dataGridView1.Rows[3].Cells[0].Value;
                IDictionaryPC NewDic = new DictionaryBase(Name, Relate);


                if (dataGridView1.Rows[0].Cells[2].Value != null && dataGridView1.Rows[0].Cells[2].Value.ToString() != "")
                {
                    if (Relate == DictionaryRelate.Price)
                    {
                        NewDic = new DictionaryPrice(Name, Relate);
                        for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                        {
                            if (dataGridView1.Rows[i].Cells[3].Value != null && dataGridView1.Rows[i].Cells[2].Value != null)
                            {
                                ((DictionaryPrice)NewDic).Set_Filling_method_string((FillDefinitionPrice)dataGridView1.Rows[i].Cells[3].Value, dataGridView1.Rows[i].Cells[2].Value.ToString());
                            }
                        }
                    }
                }

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
                new Network.Dictionary.SetDictionares().Get<bool>(new WrapNetClient(), Dictionaries);
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
        private void SiteSync_Click(object sender, EventArgs e)
        {
            new Network.Dictionary.SiteSync().Get<bool>(new WrapNetClient());
            Dictionaries = new Network.Dictionary.GetDictionares().Get<Dictionaries>(new WrapNetClient());

        }
    }
}
