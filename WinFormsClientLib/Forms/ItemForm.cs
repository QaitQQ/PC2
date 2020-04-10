using Client.Class.Net;

using StructLibs;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using WindowsFormsClientLibrary.Class;

using static NetEnum.Selector;

namespace Client.Forms
{
    public partial class ItemForm : Form
    {
        #region field
        private readonly System.Reflection.PropertyInfo[] Prop;
        private FillTable _FillTable;
        private void AddToListBox(string Str)
        {
            if (SearchList.InvokeRequired) { SearchList.Invoke(new Action<string>((s) => SearchList.Items.Add(s)), Str); }
            else
            {
                SearchList.Items.Add(Str);
            }
        }
        private readonly Action<string> ListBoxChanged;

        private List<KeyValuePair<string, int>> _SearchList;
        #endregion
        public ItemForm()
        {
            InitializeComponent();
            panel6.Visible = false;
            splitContainer3.SplitterDistance = splitContainer3.Width;
            Prop = ItemDBStruct.GetProperties();
            SetBrowser.SetBrowserFeatureControl();
            webBrowser.ScriptErrorsSuppressed = true;
            ListBoxChanged += AddToListBox;
            foreach (var item in Prop)
            {
                ItemFieldBox.Items.Add(item.Name);
                if (item.Name == "Name")
                {
                    ItemFieldBox.SelectedIndex = ItemFieldBox.Items.Count - 1;
                }
            }

        }
        #region Buttons
        private void SiteApiLoadAll_Click(object sender, EventArgs e) => new ItemNetClass().LoadAllItemFromSite();
        private void ShowWebBrowserButton_Click(object sender, EventArgs e) => ShowBrowser();
        private void MainFieldTable_Click(object sender, DataGridViewCellEventArgs e) => TableButtons(e);
        private void Retun_Compare_Names_Click(object sender, EventArgs e)
        {
            FillAtList(new ItemNetClass().Retun_Compare_Names());
            _FillTable = FillTable.СhangedItemsTable;
        }
        private void Retun_Compare_RC_Click(object sender, EventArgs e)
        {
            FillAtList(new ItemNetClass().Retun_Compare_RC());
            _FillTable = FillTable.СhangedItemsTable;
        }
        private void Retun_New_Names_Click(object sender, EventArgs e)
        {
            FillAtList(new ItemNetClass().Retun_New_Names());
            _FillTable = FillTable.NewItemTable;
        }
        private void Get_Mapped_Click(object sender, EventArgs e) => FillAtList(new ItemNetClass().CompareFromSite());
        private void Retun_Compare_Site_Click(object sender, EventArgs e)
        {
            FillAtList(new ItemNetClass().Retun_Compare_Site());
            _FillTable = FillTable.СhangedSiteTable;
        }
        private void DelDouble_Click(object sender, EventArgs e)
        {
            new ItemNetClass().DelDouble();
        }
        private void Search_Click(object sender, EventArgs e)
        {
            SearchList.Items.Clear();
            var X = ItemFieldBox.SelectedIndex;
            _SearchList = new ItemNetClass(SearchTextBox.Text).Retun_Item_List(X);
            foreach (KeyValuePair<string, int> item in _SearchList)
            { SearchList.Items.Add(item.Key.Replace(@"\n", "").Trim()); }
        }
        private void DOM_Click(object sender, EventArgs e)
        {
            // удаляем непонятные элементы metrikaId...
            //Regex _remove_IE_bug = new Regex(" ?metrikaId_[\\d\\.]*?=\"\\d*?\"", RegexOptions.IgnoreCase);
            //// result - фрагмент html, полученный после применения метода 1 или 2
            //result = _remove_IE_bug.Replace(result, "");
            //HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            //// вот оно ЧУДО
            //htmlDoc.OptionFixNestedTags = true;
            //htmlDoc.LoadHtml(result);
            //if (htmlDoc.ParseErrors != null && htmlDoc.ParseErrors.Count() > 0)
            //{
            //    MessageBox.Show("Ошибочка!");
            //    return;
            //}
            //result = htmlDoc.DocumentNode.InnerHtml;

        }

        #endregion
        #region browser
        private void ShowBrowser()

        {

            if (panel6.Visible == true)
            {
                panel6.Visible = false;
                splitContainer3.SplitterDistance = splitContainer3.Width;
            }
            else
            {
                splitContainer3.SplitterDistance = splitContainer3.Width / 2;
                panel6.Visible = true;
            }



        }
        private void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            webBrowser.Document.MouseOver += new HtmlElementEventHandler(Document_MouseOver);
            webBrowser.Document.MouseLeave += new HtmlElementEventHandler(Document_MouseLeave);
        }
        private void Document_MouseOver(object sender, HtmlElementEventArgs e)
        {
            if (e.ToElement != null & e.ToElement.Style == null)
            {
                e.ToElement.Style = "border: 2px solid red;";

                TechField.Text = e.ToElement.OuterHtml;

            }
        }
        private void Document_MouseLeave(object sender, HtmlElementEventArgs e) => e.FromElement.Style = null;
        #endregion
        private void SearchList_SelectedIndexChanged(object sender, EventArgs e)
        {
            object SelectedItem = SearchList.SelectedItem;
            if (SelectedItem != null && _SearchList != null)
            {

                string Name = SelectedItem.ToString();

                int ID = _SearchList.Find(x => x.Key.Trim() == Name).Value;
                SearchFill(ID);

            }
        }
        private void SearchFill(int ID)
        {
            MainFieldTable.Rows.Clear();
            MainFieldTable.Columns.Clear();
            MainFieldTable.Columns.AddRange(new DataGridViewTextBoxColumn[] { new DataGridViewTextBoxColumn() { Width = 100 }, new DataGridViewTextBoxColumn() { Width = 300 } });

            if (ID != 0)
            {
                ItemNetStruct Obj = new ItemNetClass(ID.ToString()).Retun_Item_And_Image() as ItemNetStruct;
                if (Obj != null)
                {
                    double PriceDC = 0;

                    MainFieldTable.Rows.Add();
                    ItemDBStruct Item = Obj.Item;
                    if (Obj.Image != null)
                    {
                        MainFieldTable.Rows[0].Cells[0] = new DataGridViewImageCell() { Value = Obj.Image };
                        MainFieldTable.Columns[0].Width = Obj.Image.Width;
                    }
                    int i = 0;
                    foreach (System.Reflection.PropertyInfo item in Prop)
                    {
                        i++;

                        object ItemValue = item.GetValue(Item);

                        if (item.GetValue(Item) is IEnumerable)
                        {
                            string NewStr = null;

                            foreach (object X in ItemValue as IEnumerable)
                            {
                                NewStr += X.ToString();
                            }
                            ItemValue = NewStr;
                        }


                        if (ItemValue != null)
                        {
                            if (item.Name.ToString() == "SiteId")
                            {
                                MainFieldTable.Rows.Add(item.Name.ToString(), ItemValue.ToString());
                                MainFieldTable.Rows[i].Cells[0] = new DataGridViewButtonCell() { Value = "Go to site" };
                            }
                            else if (item.Name.ToString() == "Id")
                            {
                                MainFieldTable.Rows.Add(item.Name.ToString(), ItemValue.ToString());
                                MainFieldTable.Rows[i].Cells[0] = new DataGridViewButtonCell() { Value = "Del From ID" };
                            }
                            else if (item.Name.ToString() == "Id")
                            {
                                MainFieldTable.Rows.Add(item.Name.ToString(), ItemValue.ToString());
                                MainFieldTable.Rows[i].Cells[0] = new DataGridViewButtonCell() { Value = "Del From ID" };
                            }
                            else if (item.Name.ToString() == "PriceRC")
                            {
                                MainFieldTable.Rows.Add(item.Name.ToString(), ItemValue.ToString());
                                PriceDC = (double)ItemValue;
                            }
                            else if (item.Name.ToString() == "PriceDC")
                            {
                                double Sale = 0;
                                double Markup = 0;
                                if (DiscountTextBox.Text != "")
                                {
                                    Sale = Convert.ToDouble(DiscountTextBox.Text);
                                }

                                if (MarkupTextBox.Text != "")
                                {
                                    Markup = Convert.ToDouble(MarkupTextBox.Text);
                                }

                                if (PriceDC != 0) { MainFieldTable.Rows.Add(item.Name.ToString(), PriceDC * (1 - (Sale / 100)) * (1 - (Markup / 100))); }
                                else { MainFieldTable.Rows.Add(item.Name.ToString(), ItemValue.ToString()); }
                            }
                            else { MainFieldTable.Rows.Add(item.Name.ToString(), ItemValue.ToString()); }
                        }
                    }
                }
            }
        }
        private void ProcessElement(HtmlElement parentElement, TreeNodeCollection nodes)
        {
            foreach (HtmlElement element in parentElement.Children)
            {
                TreeNode node = new TreeNode
                {
                    Text = "<" + element.TagName + ">"
                };
                nodes.Add(node);
                if ((element.Children.Count == 0) && (element.InnerText != null))
                {
                    node.Nodes.Add(element.InnerText);
                }
                else
                {
                    ProcessElement(element, node.Nodes);
                }
            }

        }
        private void TableButtons(DataGridViewCellEventArgs e)
        {
            object Coordinate = MainFieldTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            if (Coordinate != null)
            {
                if (Coordinate.ToString() == "Go to site") { GoToMainSite(e); }
                if (Coordinate.ToString() == "Del From ID") { Del_From_ID(e); }
                if (Coordinate.ToString() == "Принять" && _FillTable == FillTable.СhangedItemsTable) { UpdateMappedPosition(e); }
                if (Coordinate.ToString() == "Принять" && _FillTable == FillTable.СhangedSiteTable) { SetPrice(e); }
                if (Coordinate.ToString() == "Принять" && _FillTable == FillTable.NewItemTable) { Add_New_position_from_СhangeList(e); }
                if (Coordinate.ToString() == "Отменить")
                {

                    object[] obj = new object[] { MainFieldTable.Rows[e.RowIndex].Cells[1].Value.ToString(), _FillTable };

                    new ItemNetClass().Del_Item_From_СhangeList(obj);
                }
                if (Coordinate.ToString() == "Принять все") { new ItemNetClass().AllowAll(_FillTable); }

            }
        }
        private void Add_New_position_from_СhangeList(DataGridViewCellEventArgs e)
        {
            Task.Factory.StartNew(() => new ItemNetClass().Add_New_position_from_СhangeList(MainFieldTable.Rows[e.RowIndex].Cells[1].Value.ToString()));
            MainFieldTable.Rows[e.RowIndex].Visible = false;
        }
        private void SetPrice(DataGridViewCellEventArgs e)
        {
            Task.Factory.StartNew(() => new Setprice().SetPriceFromSite(Convert.ToInt32(MainFieldTable.Rows[e.RowIndex].Cells[0].Value), Convert.ToDouble(MainFieldTable.Rows[e.RowIndex].Cells[2].Value), ListBoxChanged));
            MainFieldTable.Rows[e.RowIndex].Visible = false;
        }
        private void Del_From_ID(DataGridViewCellEventArgs e)
        {
            int id = Convert.ToInt32(MainFieldTable.Rows[e.RowIndex].Cells[1].Value);
            Task.Factory.StartNew(() => new ItemNetClass().Del_Item_From_ID(id));
        }
        private void GoToMainSite(DataGridViewCellEventArgs e)
        {
            string id = MainFieldTable.Rows[e.RowIndex].Cells[1].Value.ToString();
            if (panel6.Visible == false)
            {
                splitContainer3.SplitterDistance = splitContainer3.Width / 2;
                panel6.Visible = true;
            }
            webBrowser.Navigate("https://salessab.su/index.php?route=product/product&product_id=" + id);
        }
        private void UpdateMappedPosition(DataGridViewCellEventArgs e)
        {
            ItemNetStruct Obj = new ItemNetClass(MainFieldTable.Rows[e.RowIndex].Cells[0].Value.ToString()).Retun_Item_And_Image() as ItemNetStruct;

            Obj.Item.PriceRC = Convert.ToDouble(MainFieldTable.Rows[e.RowIndex].Cells[2].Value.ToString().Split('>')[1]);
            Obj.Item.PriceDC = Convert.ToDouble(MainFieldTable.Rows[e.RowIndex].Cells[3].Value.ToString().Split('>')[1]);
            Obj.Item.Description = MainFieldTable.Rows[e.RowIndex].Cells[4].Value.ToString();
            Obj.Item.SourceName = MainFieldTable.Rows[e.RowIndex].Cells[5].Value.ToString();
            Obj.Item.DateСhange = DateTime.Now;
            new ItemNetClass().EditItemFromDBAndDelFromMappedList(Obj.Item);
            MainFieldTable.Rows[e.RowIndex].Visible = false;
        }
        private void FillAtList(List<string[]> list)
        {
            MainFieldTable.Rows.Clear();
            MainFieldTable.Columns.Clear();
            if (list != null && list.Count != 0)
            {
                for (int i = 0; i < list[0].Length; i++)
                {
                    MainFieldTable.Columns.Add(new DataGridViewTextBoxColumn() { });
                }
                MainFieldTable.Columns.AddRange(new DataGridViewButtonColumn(), new DataGridViewButtonColumn());

                MainFieldTable.Rows.Add();
                MainFieldTable.Rows[0].Cells[MainFieldTable.Columns.Count - 2] = new DataGridViewButtonCell() { Value = "Принять все" };
                MainFieldTable.Rows[0].Cells[MainFieldTable.Columns.Count - 1] = new DataGridViewButtonCell() { Value = "Отменить все" };

                foreach (string[] item in list)
                {
                    string[] Row = item.Concat(new string[] { "Принять", "Отменить" }).ToArray();
                    MainFieldTable.Rows.Add(Row);
                }
            }
        }
        private sealed class Setprice { public void SetPriceFromSite(int v1, double v2, Action<string> action)
            {
                bool I = new ItemNetClass().SetPrice(v1, v2);
                action(v1.ToString() + I.ToString());
            } }

    }



}
