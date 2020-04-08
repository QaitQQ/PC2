using Client.Class.Net;
//using OpenQA.Selenium;
//using OpenQA.Selenium.Firefox;
using StructLibs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client.Forms
{
    public partial class ItemForm : Form
    {
        System.Reflection.PropertyInfo[] Prop;
        int where_to_look = 0;
        public ItemForm ( )
        {
            InitializeComponent();
            panel6.Visible = false;
            this.splitContainer3.SplitterDistance = splitContainer3.Width;
            Prop = ItemDBStruct.GetProperties();
 
        }
        private void Search ( object sender, EventArgs e )
        {
            where_to_look = 0;

            SearchList.Items.Clear();

            try
            {
                List<string> List = new ItemNetClass(SearchTextBox.Text).Retun_Item_List();
                foreach (string item in List)
                { SearchList.Items.Add(item.Replace(@"\n", "").Trim()); }
            }
            catch (Exception E)
            { TechField.AppendText(E.Message.ToString() + "\n"); }

        }
        private void SearchList_SelectedIndexChanged ( object sender, EventArgs e )
        {
            MainFieldTable.Rows.Clear();
            string Name = SearchList.SelectedItem.ToString();

            switch (where_to_look)
            {
                case 0:
                    SearchFill(Name);
                    break;
                case 1:
                    ComparerFill(Name);
                    break;
                case 2:
                    NewFill(Name);
                    break;

                default:
                    break;
            }

        }
        private void Get_Comparer_Click ( object sender, EventArgs e )
        {
            SearchList.Items.Clear();
            where_to_look = 1;

            var Massname = new ItemNetClass(Name).Retun_Compare_Names();
            foreach (string item in Massname)
            { SearchList.Items.Add(item.Replace(@"\n", "").Trim()); }

        }
        private void ComparerFill ( string Name )
        {
            var CompTarget = new ItemNetClass(Name).Retun_Compare_Position();
            try
            {
                MainFieldTable.Columns.Add("Сравнение", "");
                for (int i = 0; i < 6; i++)
                {
                    MainFieldTable.Rows.Add();
                }

                MainFieldTable.Rows[0].Cells[1].Value = CompTarget.Key.Name;
                MainFieldTable.Rows[0].Cells[0].Value = CompTarget.Value.Name;

                MainFieldTable.Rows[1].Cells[1].Value = CompTarget.Key.Description;
                MainFieldTable.Rows[1].Cells[0].Value = CompTarget.Value.Description;
                if (CompTarget.Key.Description != CompTarget.Value.Description)
                {
                    MainFieldTable.Rows[1].Cells[1].Style.ForeColor = System.Drawing.Color.Red;
                }

                MainFieldTable.Rows[2].Cells[1].Value = CompTarget.Key.PriceRC;
                MainFieldTable.Rows[2].Cells[0].Value = CompTarget.Value.PriceRC;
                if (CompTarget.Key.PriceRC != CompTarget.Value.PriceRC)
                {
                    MainFieldTable.Rows[2].Cells[1].Style.ForeColor = System.Drawing.Color.Red;
                }

                MainFieldTable.Rows[3].Cells[1].Value = CompTarget.Key.PriceDC;
                MainFieldTable.Rows[3].Cells[0].Value = CompTarget.Value.PriceDC;
                if (CompTarget.Key.PriceDC != CompTarget.Value.PriceDC)
                {
                    MainFieldTable.Rows[3].Cells[1].Style.ForeColor = System.Drawing.Color.Red;
                }

                MainFieldTable.Rows[4].Cells[1].Value = CompTarget.Key.SourceName;
                MainFieldTable.Rows[4].Cells[0].Value = CompTarget.Value.SourceName;


                MainFieldTable.Rows[5].Cells[1].Value = CompTarget.Key.DateСhange;
                MainFieldTable.Rows[5].Cells[0].Value = CompTarget.Value.DateСhange;

                if (CompTarget.Key.Pic != null)
                {
                    MainFieldTable.Rows.Add();
                    MainFieldTable.Rows[7].Cells[0] = new DataGridViewImageCell() { Value = CompTarget.Key.Pic };
                }
            }
            catch
            {
            }

        }
        private void SearchFill ( string Name )
        {
            if (Name != null)
            {
                ItemNetStruct Obj = new ItemNetClass(Name.ToString()).Retun_Item_And_Image() as ItemNetStruct;

                ItemDBStruct Item = Obj.Item;

                if (Obj.Image != null)
                {
                    MainFieldTable.Rows.Add();
                    MainFieldTable.Rows[0].Cells[0] = new DataGridViewImageCell() { Value = Obj.Image };
                }


                foreach (var item in Prop)
                {
                    try
                    {
                        
                        if (item.Name == "SiteId")
                        {
                            MainFieldTable.Rows[MainFieldTable.Rows.Count - 1].Cells[0] = new DataGridViewButtonCell()
                            {
                               Value = "Go to Site"
                            };
                            MainFieldTable.Rows[MainFieldTable.Rows.Count - 1].Cells[1] = new DataGridViewTextBoxCell()
                            {
                                Value = item.GetValue(Item).ToString()
                            };

                        }
                        else
                        {
                            MainFieldTable.Rows.Add(item.Name.ToString(), item.GetValue(Item).ToString());
                        }
                    }
                    catch
                    {
                        MainFieldTable.Rows.Add("не удалось вывести" + item.Name.ToString());
                    }

                }

            }
        }
        private void NewFill ( string Name )
        {
            if (Name != null)
            {
                var Obj = new ItemNetClass(Name.ToString()).Retun_New_Position();
                try
                {


                    MainFieldTable.Rows.Add(Obj.Name);
                    MainFieldTable.Rows.Add(Obj.Description);
                    MainFieldTable.Rows.Add(Obj.PriceRC);
                    MainFieldTable.Rows.Add(Obj.PriceDC);
                    MainFieldTable.Rows.Add(Obj.SourceName);
                    MainFieldTable.Rows.Add(Obj.DateСhange);

                    if (Obj.Pic != null)
                    {
                        MainFieldTable.Rows.Add();
                        MainFieldTable.Rows[7].Cells[0] = new DataGridViewImageCell() { Value = Obj.Pic };
                    }
                }
                catch
                {
                }
            }
        }
        private void Get_new_Click ( object sender, EventArgs e )
        {
            SearchList.Items.Clear();
            where_to_look = 2;
            var Massname = new ItemNetClass(Name).Retun_New_Names();
            foreach (string item in Massname)
            { SearchList.Items.Add(item.Replace(@"\n", "").Trim()); }
        }
        private void Load_at_post_button_Click ( object sender, EventArgs e )
        {
            if (Item_Flag.CheckEmail == false)
            {
                Item_Flag.CheckEmail = true;
                Task.Run(( ) => new ItemNetClass().Compare_Email());
            }

        }
        private void SiteApiLoadAll_Click ( object sender, EventArgs e )
        {
            new ItemNetClass().LoadAllItemFromSite();
        }
        private void ShowWebBrowserButton_Click ( object sender, EventArgs e )
        {
            if (panel6.Visible == true)
            {
                panel6.Visible = false;
                this.splitContainer3.SplitterDistance = splitContainer3.Width;
            }
            else
            {
                this.splitContainer3.SplitterDistance = splitContainer3.Width/2;
                panel6.Visible = true;
            }
        }
        private void ProcessElement ( HtmlElement parentElement, TreeNodeCollection nodes )
        {
            foreach (HtmlElement element in parentElement.Children)
            {
                TreeNode node = new TreeNode();
                node.Text = "<" + element.TagName + ">";
                nodes.Add(node);
                if ((element.Children.Count == 0) && (element.InnerText != null))
                    node.Nodes.Add(element.InnerText);
                else
                    ProcessElement(element, node.Nodes);
            }

        }
        private void DOM_Click ( object sender, EventArgs e )
        {
            //// Объект DOM из WebBrowser
            // HtmlDocument dom = (HtmlDocument)webBrowser.Document;

            //foreach (var item in dom.All)
            //{
            //   ProcessElement((HtmlElement)item, treeView1.Nodes);         
            //} 

            //FirefoxDriver driver = new FirefoxDriver();
            //driver.Navigate().GoToUrl("http://www.google.com");
           
        }
        private void comboBox1_TextUpdate ( object sender, EventArgs e )
        {
         
        }
        private void MainFieldTable_CellContentClick ( object sender, DataGridViewCellEventArgs e )
        {
            var X = e.ToString();
            if (MainFieldTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Go to Site")
            {
                this.splitContainer3.SplitterDistance = splitContainer3.Width / 2;
                panel6.Visible = true;

                string ID = MainFieldTable.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value.ToString();

                if (ID !=null && ID!= "")
                {
                    webBrowser.Navigate("https://salessab.su/index.php?route=product/product&product_id=" + ID);
                }
                
               
            }

        }





    }
}
