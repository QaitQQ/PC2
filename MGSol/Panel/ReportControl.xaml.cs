using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;

namespace MGSol.Panel
{
    /// <summary>
    /// Логика взаимодействия для ReportControl.xaml
    /// </summary>
    public partial class ReportControl : UserControl
    {
        private MainModel mModel { get; set; }

        private ParamsColl paramsColl;
        private string[][] ReadMass;
        private List<ItemCell> ItemCells;

        public ReportControl(MainModel Model)
        {
            paramsColl = new ParamsColl
            {
                Npos = new SpanCells()
            };
            mModel = Model;
            ItemCells = new List<ItemCell>();
            string y = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo dir = new DirectoryInfo(y + @"/Report");

            InitializeComponent();

            foreach (FileInfo file in dir.GetFiles())
            {
                if (file.Name.Contains("xlsx"))
                {
                    FileList.Items.Add(file.FullName);
                }
            }
        }
        private void FileList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string t = (string)((System.Windows.Controls.ListBox)sender).SelectedItem;
            string[][] Z = new ItemProcessor.XLS.XLS_ReadReport().Read(t);
            var blst = new List<string[]>();

            for (int i = 0; i < Z.Length - 1; i++)
            {
                string n = null;
                for (int u = 0; u < Z[i].Length - 1; u++)
                {
                    n = n + Z[i][u];
                }
                if (n != "")
                {
                    blst.Add(Z[i]);
                }
            }
            Z = blst.ToArray();

            var LsLsZ = new List<string>[Z.Length];

            for (int i = 0; i < LsLsZ.Length; i++)
            {
                LsLsZ[i] = Z[i].ToList();
            }

            RemoveEmpyCol( LsLsZ);

            blst = new List<string[]>();

            for (int i = 0; i < LsLsZ.Length; i++)
            {
                blst.Add(LsLsZ[i].ToArray());
            }

            Z = blst.ToArray();

            for (int i = 0; i < Z[0].Length; i++)
            {
                var col = new DataGridTextColumn();
                col.Header = "Column " + i;
                col.Binding = new Binding(string.Format("[{0}]", i));
                Dtgreed.Columns.Add(col);
            }

            ReadMass = Z;

            Dtgreed.ItemsSource = ReadMass;
        }

        private static void RemoveEmpyCol(List<string>[] LsLsZ)
        {
            for (int x = 0; x < LsLsZ[0].Count - 1; x++)
            {
                string n = null;
                for (int y = 0; y < LsLsZ.Length - 1; y++)
                {
                    n = n + LsLsZ[y][x];
                }
                if (n == "")
                {
                    foreach (var item in LsLsZ)
                    {
                        item.RemoveAt(x);
                    }
                    RemoveEmpyCol( LsLsZ);
                }
            }
        }

        private void addBtn(ContextMenu menu, string BtnCont, MouseButtonEventHandler handler)
        {
            Label X = new Label() { Content = BtnCont };
            X.MouseLeftButtonDown += handler;
            menu.Items.Add(X);
        }
        private void ContextMenuShow(int X, int Y)
        {
            ContextMenu M = new ContextMenu();

            addBtn(M, "Номер Позиции Начало", (x, y) => { paramsColl.Npos.Start =Y; });
            addBtn(M, "Номер Позиции Конец", (x, y) => { paramsColl.Npos.End = Y; });
            addBtn(M, "Колонка Цены", (x, y) => { paramsColl.PriceCol = X; });
            addBtn(M, "Колонка SKU", (x, y) => { paramsColl.SkuCol = X; });
            addBtn(M, "Колонка Количество", (x, y) => { paramsColl.CountCol = X; });
            addBtn(M, "Колонка Дата", (x, y) => { paramsColl.DateCol = X; });
            addBtn(M, "Колонка Номер отправления", (x, y) => { paramsColl.NPostCol = X; });

            M.IsOpen = true;
            PriceColBox.Text = paramsColl.PriceCol.ToString();
            SkuColBox.Text = paramsColl.SkuCol.ToString();
            CountColBox.Text = paramsColl.CountCol.ToString();


       
        }
        private class DataValue
        {
            public int x;
            public int y;
            public string Value { get; set; }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            for (int i = paramsColl.Npos.Start; i < paramsColl.Npos.End; i++)
            {
                ItemCells.Add(new ItemCell()
                {
                    SKU = ReadMass[i][paramsColl.SkuCol],
                    Count = ReadMass[i][paramsColl.CountCol],
                    Price = ReadMass[i][paramsColl.PriceCol],
                    Date = ReadMass[i][paramsColl.DateCol],
                    NPost = ReadMass[i][paramsColl.NPostCol]
                });

            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<ItemCell>));

            using (FileStream fs = new FileStream("out.xml", FileMode.OpenOrCreate))
            {
                xmlSerializer.Serialize(fs, ItemCells);
            }

        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ParamsColl));
            // десериализуем объект
            using (FileStream fs = new FileStream("paramsColl.xml", FileMode.OpenOrCreate))
            {
                paramsColl = xmlSerializer.Deserialize(fs) as ParamsColl;
            }
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ParamsColl));

            using (FileStream fs = new FileStream("paramsColl.xml", FileMode.OpenOrCreate))
            {
                xmlSerializer.Serialize(fs, paramsColl);
            }
        }
        private void DataGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                DependencyObject dep = (DependencyObject)e.OriginalSource;

         //Шаг через визуальное дерево
                while ((dep != null) && !(dep is DataGridCell))
                {
                    dep = VisualTreeHelper.GetParent(dep);
                }

                //Является ли dep ячейкой или находится за пределами Window1?

                    DataGridCell cell = new DataGridCell();
                    cell = (DataGridCell)dep;
                    while ((dep != null) && !(dep is DataGridRow))
                    {
                        dep = VisualTreeHelper.GetParent(dep);
                    }

                    if (dep == null)
                    {
                        return;
                    }
                    
                    int X = cell.Column.DisplayIndex;

                    DataGridRow row = dep as DataGridRow;
                    int Y = Dtgreed.ItemContainerGenerator.IndexFromContainer(row);

                ContextMenuShow(X, Y);
            }
        }
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            foreach (var item in ItemCells)
            {
                var x = mModel.Option.MarketItems.Find(x => x.SKU == item.SKU);
                if (x != null)
                {
                    item.Article1C = x.Art1C;
                }
                else
                {
                    MessageBox.Show("Не найден товар с SKU:" + item.SKU);
                }

            }

        }
    }
    [Serializable]
    public class ItemCell
    {
        public string Article1C;
        public string SKU;
        public string Price;
        public string Count;
        public string Date;
        public string NPost;    
    }
    [Serializable]
    public class SpanCells
    {
        public int Start;
        public int End;
    }
    [Serializable]
    public class ParamsColl
    {
        public SpanCells Npos;
        public int PriceCol;
        public int SkuCol;
        public int CountCol;
        public int DateCol;
        public int NPostCol;
    }
}

