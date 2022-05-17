using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Serialization;

namespace MGSol.Panel
{
    /// <summary>
    /// Логика взаимодействия для ReportControl.xaml
    /// </summary>
    public partial class ReportControl : UserControl
    {
        private MainModel mModel { get; set; }
        List<List<DataValue>> ListStr;
        List<ItemCell> ItemCells;
        ParamsColl paramsColl;
        public ReportControl(MainModel Model)
        {
            paramsColl = new ParamsColl();
            paramsColl.Npos = new SpanCells();
            this.mModel = Model;
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
            var Z = new ItemProcessor.XLS.XLS_ReadReport().Read(t);
            ListStr = new List<List<DataValue>>();

            for (int i = 0; i < Z.Length - 1; i++)
            {
                ListStr.Add(new List<DataValue>());

                for (int u = 0; u < Z[i].Length - 1; u++)
                {
                    ListStr[i].Add(new DataValue() { x = i, y = u, Value = Z[i][u] });
                }
            }
            lst.ItemsSource = ListStr;

        }
        void addBtn(ContextMenu menu, string BtnCont, MouseButtonEventHandler handler)
        {
            var X = new Label() { Content = BtnCont };
            X.MouseLeftButtonDown += handler;
            menu.Items.Add(X);
        }
        private void TextBlock_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {

            var box = (System.Windows.Controls.TextBlock)sender;
            MGSol.Panel.ReportControl.DataValue dc = (MGSol.Panel.ReportControl.DataValue)box.DataContext;
            var M = new ContextMenu();

            addBtn(M, "Номер Позиции Начало", (x, y) => { paramsColl.Npos.Start = dc.x; });
            addBtn(M, "Номер Позиции Конец", (x, y) => { paramsColl.Npos.End = dc.x; });
            addBtn(M, "Колонка Цены", (x, y) => { paramsColl.PriceCol = dc.y; });
            addBtn(M, "Колонка SKU", (x, y) => { paramsColl.SkuCol = dc.y; });
            addBtn(M, "Колонка Количество", (x, y) => { paramsColl.CountCol = dc.y; });
            addBtn(M, "Колонка Дата", (x, y) => { paramsColl.DateCol = dc.y; });
            addBtn(M, "Колонка Номер отправления", (x, y) => { paramsColl.NPostCol = dc.y; });

            M.IsOpen = true;
            PriceColBox.Text = paramsColl.PriceCol.ToString();
            SkuColBox.Text = paramsColl.SkuCol.ToString();
            CountColBox.Text = paramsColl.CountCol.ToString();
        }
        class DataValue
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
                    SKU = ListStr[i][paramsColl.SkuCol].Value,
                    Count = ListStr[i][paramsColl.CountCol].Value,
                    Price = ListStr[i][paramsColl.PriceCol].Value,
                    Date = ListStr[i][paramsColl.DateCol].Value,
                    NPost = ListStr[i][paramsColl.NPostCol].Value
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
    }
    [Serializable]
    public class ItemCell
    {
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

