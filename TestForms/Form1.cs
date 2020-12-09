using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace TestForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Group = false;

            textBox4.Text = Program.attribute_description.Length.ToString();
        }

        private bool Group;
        private int str;
        private List<object> lst;

        private void button1_Click(object sender, EventArgs e)
        {
            checkedListBox1.Items.Clear();

            string Z = Program.ИсправитьТекст(textBox1.Text.ToLower());


            IEnumerable<Server.Class.IntegrationSiteApi.StructSite.attribute_description> X = Program.attribute_description.Where(X => X.name.ToLower().Contains(textBox1.Text.ToLower()));

            foreach (Server.Class.IntegrationSiteApi.StructSite.attribute_description item in X)
            {
                checkedListBox1.Items.Add(item.attribute_id + "|" + item.name, CheckState.Checked);
            }


        }
        private void button2_Click(object sender, EventArgs e)
        {
            int BaseID = Convert.ToInt32(checkedListBox1.SelectedItem.ToString().Split('|')[0]); // беру главынй ID

            List<int> SecondaryIDs = new List<int>(); // беру вторичные

            for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
            {
                int D = Convert.ToInt32(checkedListBox1.CheckedItems[i].ToString().Split('|')[0]);
                if (D != BaseID)
                {
                    SecondaryIDs.Add(D);
                }
            }
            foreach (int item in SecondaryIDs)
            {
                new Server.Class.IntegrationSiteApi.StructSite("https://salessab.su" + "/index.php?route=api").Attribute_replacement(item, BaseID);
            }

        }
        private void button3_Click(object sender, EventArgs e)
        {
            Program.УдалениеПустых(Program.product_attribute, Program.attribute_description);
            Program.attribute_description = new Server.Class.IntegrationSiteApi.StructSite("https://salessab.su" + "/index.php?route=api").GetTable<Server.Class.IntegrationSiteApi.StructSite.attribute_description>("attribute_description");



        }
        private void button12_Click(object sender, EventArgs e)
        {
            Program.ПоискПоИмени(Program.attribute_description, new Action<string>(checkedListBoxText));
        }
        private void checkedListBoxText(string Text) { checkedListBox1.Items.Add(Text); }
        private void button13_Click(object sender, EventArgs e)
        {
            foreach (Server.Class.IntegrationSiteApi.StructSite.product_attribute item in Program.product_attribute)
            {
                IEnumerable<Server.Class.IntegrationSiteApi.StructSite.attribute_description> T = Program.attribute_description.Where(x => x.attribute_id == item.attribute_id);

                if (T.Count() == 0)
                {
                    new Server.Class.IntegrationSiteApi.StructSite("https://salessab.su" + "/index.php?route=api").DeleteAttribute(Convert.ToInt32(item.attribute_id));
                }


            }
        }
        private void button14_Click(object sender, EventArgs e)
        {

            if (!Group)
            {
                lst = new List<object>();

                Group = true;
                var X = from PRAT in Program.product_attribute join ATDISC in Program.attribute_description on PRAT.attribute_id equals ATDISC.attribute_id select new { Value = PRAT.text, ATT_ID = PRAT.attribute_id, Desc = ATDISC.name };
                var Z = X.GroupBy(X => X.Value).ToArray();

                foreach (var item in Z)
                {
                    string A = item.First().Desc;
                    foreach (var Item in item)
                    {
                        if (Item.Desc != A)
                        {
                            var Y = item.ToList().GroupBy(X => X.Desc); // 1 тип 2 тип 3 тип

                            var mst = new List<KeyValuePair<string, string>>();

                            foreach (var VV in Y)
                            {
                                mst.Add(new KeyValuePair<string, string>(VV.Key, VV.First().ATT_ID));
                            }
                            if (mst.Count() < 4)
                            {
                                lst.Add(mst);
                            }

                            break;
                        }
                    }
                }


            }
            checkedListBox1.Items.Clear();

            foreach (dynamic item in lst[str] as IEnumerable)
            {
                checkedListBox1.Items.Add(item.Value + "|" + item.Key, CheckState.Checked);
            }
            str++;




        }

        private void button15_Click(object sender, EventArgs e)
        {

        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

            if (sender is System.Windows.Forms.ContextMenuStrip)
            {



                var X = ((System.Windows.Forms.ContextMenuStrip)sender).SourceControl;

                contextMenuStrip1.Items.Clear();

                if (X.Text != "")
                {
                    var R = X.Text.Split('|')[0];

                    contextMenuStrip1.Items.Add("Удалить", null, (e, s) =>
                    {
                        new Server.Class.IntegrationSiteApi.StructSite("https://salessab.su" + "/index.php?route=api").DeleteAttribute(Convert.ToInt32(R));
                        Program.attribute_description = Program.attribute_description.Where(x => x.attribute_id != R).ToArray();
                        button1_Click(null, null);

                    });

                    contextMenuStrip1.Items.Add("Очистить выделение", null, (e, s) =>
                    {


                        checkedListBox1.Items.Clear();

                        string Z = Program.ИсправитьТекст(textBox1.Text.ToLower());


                        IEnumerable<Server.Class.IntegrationSiteApi.StructSite.attribute_description> X = Program.attribute_description.Where(X => X.name.ToLower().Contains(textBox1.Text.ToLower()));

                        foreach (Server.Class.IntegrationSiteApi.StructSite.attribute_description item in X)
                        {
                            checkedListBox1.Items.Add(item.attribute_id + "|" + item.name, CheckState.Unchecked);
                        }

                    });




                }

               


            }
        }


    }
}






