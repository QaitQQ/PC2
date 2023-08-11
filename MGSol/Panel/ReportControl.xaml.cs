using MGSol.Panel.Other;

using StructLibCore.Marketplace;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;
namespace MGSol.Panel
{
    public partial class ReportControl : UserControl
    {
        private MainModel Model { get; set; }
        private ObservableCollection<ObservableCollection<ColorCell>> ColorCellsList { get; set; }
        private FileInfo[] FolderFile;
        private ObservableCollection<FileOption> InfoFolderFile;
        private object PressBtn;
        private TextBlock ColtextBlock;
        private ObservableCollection<ColorCell> ParamStack { get; set; }
        private List<object[]> ParamButtons { get; set; }
        private FileOption ActiveFile;
        public static List<object[]> RetunList()
        {
            List<object[]> lst = new();
            foreach (object item in Enum.GetValues(typeof(ParamEnum)))
            {
                object[] mass = new object[2] { item, new SolidColorBrush(Color.FromRgb((byte)new Random().Next(255), (byte)new Random().Next(255), (byte)new Random().Next(255))) };
                lst.Add(mass);
            }
            return lst;
        }
        public ReportControl(MainModel Model)
        {
            this.Model = Model;
            RenewFile_Click(null, null);
        }
        private static string[][][] ReadFileToMass(string t)
        {
            string[][][] Z = new ItemProcessor.XLS.XLS_ReadReport().Read(t);
            if (Z == null)
            {
                return Array.Empty<string[][]>();
            }
            for (int i = 0; i < Z.Length; i++)
            {
                Z[i] = RemNullStr(Z[i]);
            }
            return Z;
        }
        private static string[][] RemNullStr(string[][] Z)
        {
            List<string[]> blst = new();
            for (int i = 0; i < Z.Length - 1; i++)
            {
                string n = null;
                for (int u = 0; u < Z[i].Length - 1; u++)
                {
                    n += Z[i][u];
                }
                if (n != "")
                {
                    blst.Add(Z[i]);
                }
            }
            Z = blst.ToArray();
            List<string>[] LsLsZ = new List<string>[Z.Length];
            for (int i = 0; i < LsLsZ.Length; i++)
            {
                LsLsZ[i] = Z[i].ToList();
            }
            RemoveEmpyCol(LsLsZ);
            blst = new List<string[]>();
            for (int i = 0; i < LsLsZ.Length; i++)
            {
                blst.Add(LsLsZ[i].ToArray());
            }
            Z = blst.ToArray();
            return Z;
        }
        private static void RemoveEmpyCol(List<string>[] LsLsZ)
        {
            for (int x = 0; x < LsLsZ[0].Count - 1; x++)
            {
                string n = null;
                for (int y = 0; y < LsLsZ.Length - 1; y++)
                {
                    n += LsLsZ[y][x];
                }
                if (n == "")
                {
                    foreach (List<string> item in LsLsZ)
                    {
                        item.RemoveAt(x);
                    }
                    RemoveEmpyCol(LsLsZ);
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ActiveFile != null)
                {
                    Document Document = GenDocument();
                    string[][] byerSFmass = null;
                    bool editNonber = false;
                    Order FindPost;
                    for (int i = GiveColorCell(ParamEnum.НачСтрок).X; i < GiveColorCell(ParamEnum.КонецСтрок).X + 1; i++)
                    {
                        string OrderPrice = ColorCellsList[i][GiveColorCell(ParamEnum.ЦенаПродажи).Y].Value;
                        string Count = ColorCellsList[i][GiveColorCell(ParamEnum.Количество).Y].Value;
                        string NPost = ColorCellsList[i][GiveColorCell(ParamEnum.НомерЗаказа).Y].Value;
                        string Date = ColorCellsList[i][GiveColorCell(ParamEnum.Дата).Y].Value;
                        FindPost = Document.Orders.Find(x => x.DepartureNumber == NPost);
                        SF SchetFaktura = null;
                        if (ActiveFile.APISetting.Type == MarketName.Ozon)
                        {
                            string SFNomber = ColorCellsList[i][GiveColorCell(ParamEnum.НомерСФ).Y].Value;
                            string Bonus = ColorCellsList[i][GiveColorCell(ParamEnum.Бонус).Y].Value;
                            if (Bonus is not "" and not null)
                            {
                                OrderPrice = (((double.Parse(OrderPrice) * double.Parse(Count)) + double.Parse(Bonus)) / double.Parse(Count)).ToString();
                            }
                            if (SFNomber != "")
                            {
                                SchetFaktura = GetSchetFaktura(Document.Nomber, SFNomber, ref byerSFmass);
                            }
                            Server.Class.IntegrationSiteApi.Market.Ozon.OzonPortOrderList.Order Info = (Server.Class.IntegrationSiteApi.Market.Ozon.OzonPortOrderList.Order)new SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post.OzonPostOrdrInfo(ActiveFile.APISetting).Get(NPost);
                            if (Info != null)
                            {
                                Date = Info.ShipmentDate.ToString();
                            }
                        }
                        if (!editNonber && ActiveFile.APISetting.Type == MarketName.Yandex)
                        {
                            string z = DateTime.Parse(Date).Month.ToString();
                            char p = DateTime.Parse(Date).Year.ToString().Last();
                            Document.Nomber = z + p + Document.Nomber;
                            editNonber = true;
                        }
                        if (OrderPrice != "")
                        {
                            OrderItem NOrderItem = new()
                            {
                                Count = Count,
                                Price = OrderPrice,
                                SKU = ColorCellsList[i][GiveColorCell(ParamEnum.SKU).Y].Value,
                                Type = OrdersTaypeEnum.Продажа
                            };
                            if (NOrderItem.SKU == "100001")
                            {
                                NOrderItem.SKU = "1001";
                            }
                            if (NOrderItem.SKU == "10078")
                            {
                                NOrderItem.SKU = "1128";
                            }
                            if (NOrderItem.SKU == "10035")
                            {
                                NOrderItem.SKU = "1021";
                            }
                            string Article1C = FindArt1C(NOrderItem.SKU);
                            if (Article1C is null)
                            {
                                string RepSTR = null;
                                foreach (ColorCell item in ColorCellsList[i])
                                {
                                    RepSTR = RepSTR + "   " + item.Value;
                                }
                                QuestionBox QS = new(RepSTR);
                                if (QS.ShowDialog() == true)
                                {
                                    MarketItem X = Model.OptionMarketPlace.MarketItems.FirstOrDefault(x => x.SKU == NOrderItem.SKU);
                                    if (X != null)
                                    {
                                        X.Art1C = QS.AnswerTEXT;
                                        Model.Save();
                                        Article1C = X.Art1C;
                                    }
                                    else
                                    {
                                        ModalBox MB = new()
                                        {
                                            _STR = "не удалось найти позицию в базе с таким SKU, создать?"
                                        };
                                        if (MB.ShowDialog() == true)
                                        {
                                            Model.OptionMarketPlace.MarketItems.Add(new StructLibCore.Marketplace.MarketItem() { SKU = NOrderItem.SKU, Art1C = Article1C });
                                        }
                                        Model.Save();
                                    }
                                }
                            }
                            NOrderItem.Article1C = Article1C;
                            AddItem(Document, NPost, Date, FindPost, NOrderItem, SchetFaktura);
                        }
                        if (ColorCellsList[i][GiveColorCell(ParamEnum.SKU).Y].Value == "1162")
                        {
                        }
                        if (ActiveFile.APISetting.Type == MarketName.Ozon) // Возврат работает только для озона
                        {
                            string OrderReturnPrice = ColorCellsList[i][GiveColorCell(ParamEnum.ВозвратСумма).Y].Value;
                            int u = GiveColorCell(ParamEnum.ВозвратКолич).Y;
                            Count = ColorCellsList[i][u].Value;
                            if (OrderReturnPrice != "" && ActiveFile.APISetting.Type == MarketName.Ozon)
                            {
                                FindPost = Document.Orders.Find(x => x.DepartureNumber == NPost);
                                OrderItem NOrderItem = new()
                                {
                                    Count = Count,
                                    Price = (double.Parse(OrderReturnPrice) / double.Parse(ColorCellsList[i][GiveColorCell(ParamEnum.ВозвратКолич).Y].Value)).ToString(),
                                    SKU = ColorCellsList[i][GiveColorCell(ParamEnum.SKU).Y].Value,
                                    Type = OrdersTaypeEnum.Возврат
                                };
                                NOrderItem.Article1C = Model.OptionMarketPlace.MarketItems.First(x => x.SKU == NOrderItem.SKU).Art1C;
                                AddItem(Document, NPost, Date, FindPost, NOrderItem, SchetFaktura);
                            }
                        }
                    }
                    if (ActiveFile.APISetting.Type == MarketName.Yandex) // Возврат для Яндекса 
                    {
                        ColorCell I = GiveColorCell(ParamEnum.НачСтрокВозврат);
                        int Y = 0;
                        if (I != null)
                        {
                            Y = I.X;
                        }
                        for (int i = Y; i < GiveColorCell(ParamEnum.КонСтрокВозврат).X + 1; i++)
                        {
                            string OrderReturnPrice;
                            try
                            {
                                OrderReturnPrice = ColorCellsList[i][GiveColorCell(ParamEnum.ВозвратСумма).Y].Value;
                            }
                            catch
                            {
                                break;
                            }
                            if (OrderReturnPrice is not "0" and not "")
                            {
                                string NPost = ColorCellsList[i][GiveColorCell(ParamEnum.НомерЗаказа).Y]?.Value;
                                string Date = DateTime.Now.ToString().Split(" ")[0];
                                SF SchetFaktura = null;
                                try
                                {
                                    if (OrderReturnPrice != "")
                                    {
                                        FindPost = Document.Orders.Find(x => x.DepartureNumber == NPost);
                                        OrderItem NOrderItem = new()
                                        {
                                            Count = ColorCellsList[i][GiveColorCell(ParamEnum.ВозвратКолич).Y].Value,
                                            Price = (double.Parse(OrderReturnPrice) / double.Parse(ColorCellsList[i][GiveColorCell(ParamEnum.ВозвратКолич).Y].Value)).ToString(),
                                            SKU = ColorCellsList[i][GiveColorCell(ParamEnum.SKU).Y].Value,
                                            Type = OrdersTaypeEnum.Возврат
                                        };
                                        if (NOrderItem.SKU == "100001")
                                        {
                                            NOrderItem.SKU = "1001";
                                        }
                                        if (NOrderItem.SKU == "10078")
                                        {
                                            NOrderItem.SKU = "1128";
                                        }
                                        NOrderItem.Article1C = Model.OptionMarketPlace.MarketItems.First(x => x.SKU == NOrderItem.SKU).Art1C;
                                        AddItem(Document, NPost, Date, FindPost, NOrderItem, SchetFaktura);
                                    }
                                }
                                catch
                                {
                                    _ = MessageBox.Show("Не распознан Возврат");
                                }
                            }
                        }
                    }
                    SaveDocument(Document);
                }
                static void AddItem(Document Document, string NPost, string Date, Order FindPost, OrderItem NOrderItem, SF SchetFaktura)
                {
                    if (FindPost != null)
                    {
                        FindPost.Items.Add(NOrderItem);
                    }
                    else
                    {
                        Order NOrder = new() { DepartureNumber = NPost, DepartureDate = Date, SchetFaktura = SchetFaktura };
                        NOrder.Items.Add(NOrderItem);
                        Document.Orders.Add(NOrder);
                    }
                }
            }
            catch (Exception E)
            {
                TestParamStack();
                _ = MessageBox.Show(E.Message);
            }
        }
        private string FindArt1C(string sKU)
        {
            string X = Model.OptionMarketPlace.MarketItems.FirstOrDefault(x => x.SKU == sKU)?.Art1C;
            return X is "" or null ? null : X;
        }
        private static void SaveDocument(Document Document)
        {
            string Name = Document.Nomber + DateTime.Now.Second + ".xml";
            string filename;
            Microsoft.Win32.SaveFileDialog dlg = new()
            {
                FileName = Name, // Default file name
                DefaultExt = ".xml", // Default file extension
                Filter = "Text documents (.xml)|*.xml" // Filter files by extension
            };
            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();
            // Process save file dialog box results
            if (result == true)
            {
                filename = dlg.FileName;
                XmlSerializer xmlSerializer = new(typeof(Document));
                using FileStream fs = new(filename, FileMode.OpenOrCreate);
                xmlSerializer.Serialize(fs, Document);
            }
        }
        private SF GetSchetFaktura(string NNomer, string SFNomber, ref string[][] byerSFmass)
        {
            try
            {
                FileInfo Fn = FolderFile.First(x => x.Name.Contains(NNomer) && x.Name.Contains("DocumentB2BSales") && !x.Name.Contains('#'));
                if (byerSFmass == null) { try { byerSFmass = ReadFileToMass(Fn.FullName)[0]; } catch { _ = MessageBox.Show("Не удалось загрузить файл B2B"); } }
                string[] nameCollstring = byerSFmass.First(x => x.Contains("Наименование покупателя"));
                int ind = Array.IndexOf(nameCollstring, "Наименование покупателя");
                string[] valString = byerSFmass.First(x => x.Contains(SFNomber));
                string NameVal = valString[ind];
                ind = Array.IndexOf(nameCollstring, "ИНН");
                string INNVal = valString[ind];
                ind = Array.IndexOf(nameCollstring, "Дата\nсчета-фактуры\nпродавца");
                string DataVal = valString[ind];
                return new SF() { NameBuyer = NameVal, Date = DataVal, INN = INNVal, Nomber = SFNomber };
            }
            catch
            {
                _ = MessageBox.Show("Ненайден файл с счетфактурами");
            }
            return null;
        }
        private Document GenDocument()
        {
            try
            {
                string Nomber = GiveColorCell(ParamEnum.НомерОтчета).Value;
                string NNomer = null;
                DateTime date = DateTime.Now;
                foreach (char item in Nomber)
                {
                    if (char.IsDigit(item))
                    {
                        NNomer += item;
                    }
                }
                string INN_Byer = ActiveFile.InnString.INN;
                string INN_Seller = ActiveFile.APISetting.INN;
                Document Document = new()
                {
                    INN_Byer = INN_Byer,
                    INN_Seller = INN_Seller,
                    Nomber = NNomer,
                    Data = date.ToString()
                };
                return Document;
            }
            catch (Exception E)
            {
                _ = MessageBox.Show(E.Message);
                return null;
            }
        }
        private void TestParamStack()
        {
            foreach (object[] item in ParamButtons)
            {
                IEnumerable<ColorCell> X = from x in ParamStack where x.Param == (ParamEnum)item[0] select x;
                if (!X.Any())
                {
                    _ = MessageBox.Show("Не заполнен " + ((ParamEnum)item[0]).ToString());
                    break;
                }
            }
        }
        private void Save_Options(object sender, RoutedEventArgs e)
        {
            if (ActiveFile != null)
            {
                XmlSerializer xmlSerializer = new(typeof(FileOption));
                ActiveFile.ParamsColl = new List<ParamsColl>();
                foreach (ColorCell item in ParamStack)
                {
                    ActiveFile.ParamsColl.Add(new ParamsColl() { Param = item.Param, X = item.X, Y = item.Y });
                }
                if (File.Exists(ActiveFile.FullPath.Replace("xlsx", "xml")))
                {
                    File.Delete(ActiveFile.FullPath.Replace("xlsx", "xml"));
                }
                using FileStream fs = new(ActiveFile.FullPath.Replace("xlsx", "xml"), FileMode.OpenOrCreate);
                xmlSerializer.Serialize(fs, ActiveFile);
            }
        }
        private void Load_options(object sender, RoutedEventArgs e)
        {
            try
            {
                FileOption FL = (MGSol.Panel.FileOption)((Button)sender).DataContext;
                FL = LoadFileOptioins(FL);
                ReFiilParamsCol(FL);
            }
            catch
            {
                _ = MessageBox.Show("Не удалось загрузить настройки");
            }
        }
        private static FileOption LoadFileOptioins(FileOption FL)
        {
            try
            {
                if (File.Exists(FL.FullPath))
                {
                    XmlSerializer xmlSerializer = new(typeof(FileOption));
                    using FileStream fs = new(FL.FullPath.Replace("xlsx", "xml"), FileMode.OpenOrCreate);
                    return FL = xmlSerializer.Deserialize(fs) as FileOption;
                }
                else { return null; }
            }
            catch
            {
                _ = MessageBox.Show("Не удалось прочитать" + FL.FullPath.Replace("xlsx", "xml"));
                return null;
            }
        }
        private void ReFiilParamsCol(FileOption FL)
        {
            if (ColorCellsList != null && FL != null)
            {
                ParamStack = new ObservableCollection<ColorCell>();
                foreach (ParamsColl item in FL.ParamsColl)
                {
                    foreach (object[] btn in ParamButtons)
                    {
                        try
                        {
                            if ((ParamEnum)btn[0] == item.Param)
                            {
                                ColorCellsList[item.X][item.Y].Color = (SolidColorBrush)btn[1];
                                ColorCellsList[item.X][item.Y].Param = (ParamEnum)btn[0];
                                ParamStack.Add(ColorCellsList[item.X][item.Y]);
                                break;
                            }
                        }
                        catch { _ = MessageBox.Show("Не удалось разместить" + item.Param.ToString()); }
                    }
                }
            }
        }
        private void Table_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (PressBtn != null)
            {
                TextBlock txtbox = (TextBlock)sender;
                txtbox.Background = null;
                Button btn = (Button)PressBtn;
                ColorCell val = (ColorCell)txtbox.DataContext;
                val.Color = btn.Background;
                ParamEnum paramsColl = (ParamEnum)((object[])btn.DataContext)[0];
                List<ColorCell> fnd = (from x in ParamStack where x.Param == paramsColl select x).ToList();
                if (fnd.Count > 0)
                {
                    ColorCell TM = fnd[0];
                    while (fnd.Count > 0)
                    {
                        fnd[0].Color = null;
                        if (ColtextBlock != null)
                        {
                            ColtextBlock.Background = null;
                        }
                        _ = ParamStack.Remove(fnd[0]);
                        fnd = (from x in ParamStack where x.Param == paramsColl select x).ToList();
                    }
                    if (TM != val)
                    {
                        val.Param = paramsColl;
                        txtbox.Background = val.Color;
                        ParamStack.Add(val);
                    }
                }
                else if (fnd.Count == 0)
                {
                    val.Param = paramsColl;
                    txtbox.Background = val.Color;
                    ParamStack.Add(val);
                }
                ColtextBlock = txtbox;
            }
        }
        private void Border_Click(object sender, RoutedEventArgs e)
        {
            Border borber = (Border)((Button)sender).Parent;
            if (borber.BorderThickness.Left != 2)
            {
                borber.BorderBrush = Brushes.Green;
                borber.BorderThickness = new Thickness(2);
                borber.CornerRadius = new CornerRadius(3);
                if (PressBtn != null && PressBtn != sender)
                {
                    Border_Click(PressBtn, null);
                }
                PressBtn = sender;
            }
            else
            {
                borber.BorderThickness = new Thickness(0);
                borber.CornerRadius = new CornerRadius(0);
                PressBtn = null;
            }
        }
        private ColorCell GiveColorCell(ParamEnum param)
        {
            foreach (ColorCell item in ParamStack)
            {
                if (item.Param == param)
                {
                    return item;
                }
            }
            return new ColorCell();
        }
        private void Read_file_button_Click(object sender, RoutedEventArgs e)
        {
            FileOption FileI = (FileOption)((Button)sender).DataContext;
            ActiveFile = FileI;
            string[][][] Z = ReadFileToMass(FileI.FullPath);
            ColorCellsList = new ObservableCollection<ObservableCollection<ColorCell>>();
            int D = 0;
            for (int i = 0; i < Z.Length; i++)
            {
                if (ActiveFile.APISetting != null && ActiveFile.APISetting.Type == MarketName.Yandex)
                {
                    if (i == 0)
                    {
                        i = 2;
                    }
                    if (i == 3)
                    {
                        i = 4;
                    }
                }
                for (int p = 0; p < Z[i].Length; p++)
                {
                    ColorCellsList.Add(new ObservableCollection<ColorCell>());
                    for (int x = 0; x < Z[i][p].Length; x++)
                    {
                        ColorCellsList[D].Add(new ColorCell() { Value = Z[i][p][x], X = D, Y = x, Lst = i });
                    }
                    D++;
                }
            }
            DTtable.ItemsSource = ColorCellsList;
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 1)
            {
                FileOption FL = (MGSol.Panel.FileOption)((ComboBox)sender).DataContext;
                FL.APISetting = Model.GetApiFromName(e.AddedItems[0].ToString());
            }
        }
        private void ByerComboBoxINN_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 1)
            {
                FileOption FL = (MGSol.Panel.FileOption)((ComboBox)sender).DataContext;
                FL.InnString = Model.GetInnFromName(e.AddedItems[0].ToString());
            }
        }
        private void RenewFile_Click(object sender, RoutedEventArgs e)
        {
            InfoFolderFile = new ObservableCollection<FileOption>();
            string y = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo dir = new(y + @"/Report");
            FolderFile = dir.GetFiles();
            ParamStack = new ObservableCollection<ColorCell>();
            InitializeComponent();
            ParamButtons = RetunList();
            ButtonFieldStack.ItemsSource = ParamButtons;
            foreach (FileInfo file in FolderFile)
            {
                if (file.Name.Contains("xlsx") && !file.Name.Contains('#'))
                {
                    FileOption fileOption = new() { FileName = file.Name, FullPath = file.FullName };
                    if (File.Exists(file.FullName.Replace("xlsx", "xml")))
                    {
                        fileOption = LoadFileOptioins(fileOption);
                    }
                    fileOption ??= new() { FileName = file.Name, FullPath = file.FullName };
                    InfoFolderFile.Add(fileOption);
                }
            }
            FileList.ItemsSource = InfoFolderFile;
        }
    }
}