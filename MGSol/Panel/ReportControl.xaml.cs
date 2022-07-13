﻿using System;
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
        private MainModel mModel { get; set; }
        private ObservableCollection<ObservableCollection<ColorCell>> colorCellsList { get; set; }
        private FileInfo[] FolderFile;
        private ObservableCollection<FileOption> InfoFolderFile;
        private object PressBtn;
        private TextBlock ColtextBlock;
        private ObservableCollection<ColorCell> ParamStack { get; set; }
        private List<object[]> ParamButtons { get; set; }
        private FileOption ActiveFile;
        public static List<object[]> RetunList()
        {
            List<object[]> lst = new List<object[]>();
            foreach (object item in Enum.GetValues(typeof(ParamEnum)))
            {
                object[] mass = new object[2] { item, new SolidColorBrush(Color.FromRgb((byte)new Random().Next(255), (byte)new Random().Next(255), (byte)new Random().Next(255))) };
                lst.Add(mass);
            }
            return lst;
        }
        public ReportControl(MainModel Model)
        {
            mModel = Model;
            InfoFolderFile = new ObservableCollection<FileOption>();
            string y = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo dir = new DirectoryInfo(y + @"/Report");
            FolderFile = dir.GetFiles();
            ParamStack = new ObservableCollection<ColorCell>();
            InitializeComponent();
            ParamButtons = RetunList();
            ButtonFieldStack.ItemsSource = ParamButtons;
            foreach (FileInfo file in FolderFile)
            {
                if (file.Name.Contains("xlsx") && !file.Name.Contains("#"))
                {
                    FileOption FL = new FileOption() { FileName = file.Name, FullPath = file.FullName };
                    if (File.Exists(file.FullName.Replace("xlsx", "xml")))
                    {
                        FL = LoadFileOptioins(FL);
                    }
                    InfoFolderFile.Add(FL);
                }
            }
            FileList.ItemsSource = InfoFolderFile;
        }
        private static string[][][] ReadFileToMass(string t)
        {
            string[][][] Z = new ItemProcessor.XLS.XLS_ReadReport().Read(t);
            for (int i = 0; i < Z.Length; i++)
            {
                Z[i] = RemNullStr(Z[i]);
            }
            return Z;
        }
        private static string[][] RemNullStr(string[][] Z)
        {
            List<string[]> blst = new List<string[]>();
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
                    n = n + LsLsZ[y][x];
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
                        string OrderPrice = colorCellsList[i][GiveColorCell(ParamEnum.ЦенаПродажи).Y].Value;
                        string Count = colorCellsList[i][GiveColorCell(ParamEnum.Количество).Y].Value;
                        string NPost = colorCellsList[i][GiveColorCell(ParamEnum.НомерЗаказа).Y].Value;
                        string Date = colorCellsList[i][GiveColorCell(ParamEnum.Дата).Y].Value;
                        FindPost = Document.Orders.Find(x => x.DepartureNumber == NPost);
                        SF SchetFaktura = null;
                        if (ActiveFile.APISetting.Type == StructLibCore.Marketplace.MarketName.Ozon)
                        {
                            string SFNomber = colorCellsList[i][GiveColorCell(ParamEnum.НомерСФ).Y].Value;
                            string Bonus = colorCellsList[i][GiveColorCell(ParamEnum.Бонус).Y].Value;
                            if (Bonus != "" && Bonus != null)
                            {
                                OrderPrice = ((double.Parse(OrderPrice) * double.Parse(Count) + double.Parse(Bonus)) / double.Parse(Count)).ToString();
                            }
                            if (SFNomber != "")
                            {
                                SchetFaktura = GetSchetFaktura(Document.Nomber, SFNomber, ref byerSFmass);
                            }
                            SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post.OzonPostOrdrInfo.Result Info = (SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post.OzonPostOrdrInfo.Result)new SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post.OzonPostOrdrInfo(ActiveFile.APISetting).Get(NPost);
                            if (Info != null)
                            {
                                Date = Info.ShipmentDate.Split("T")[0];
                            }
                        }
                        if (!editNonber && ActiveFile.APISetting.Type == StructLibCore.Marketplace.MarketName.Yandex)
                        {
                            string z = DateTime.Parse(Date).Month.ToString();
                            char p = DateTime.Parse(Date).Year.ToString().Last();
                            Document.Nomber = Document.Nomber + z + p;
                            editNonber = true;
                        }
                        if (OrderPrice != "")
                        {
                            OrderItem NOrderItem = new()
                            {
                                Count = Count,
                                Price = OrderPrice,
                                SKU = colorCellsList[i][GiveColorCell(ParamEnum.SKU).Y].Value,
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
                            NOrderItem.Article1C = FindArt1C(NOrderItem.SKU);
                            AddItem(Document, NPost, Date, FindPost, NOrderItem, SchetFaktura);
                        }
                        if (ActiveFile.APISetting.Type == StructLibCore.Marketplace.MarketName.Ozon) // Возврат работает только для озона
                        {
                            string OrderReturnPrice = colorCellsList[i][GiveColorCell(ParamEnum.ВозвратСумма).Y].Value;
                            if (OrderReturnPrice != "" && ActiveFile.APISetting.Type == StructLibCore.Marketplace.MarketName.Ozon)
                            {
                                FindPost = Document.Orders.Find(x => x.DepartureNumber == NPost);
                                OrderItem NOrderItem = new()
                                {
                                    Count = colorCellsList[i][GiveColorCell(ParamEnum.Количество).Y].Value,
                                    Price = (double.Parse(OrderReturnPrice) / double.Parse(colorCellsList[i][GiveColorCell(ParamEnum.ВозвратКолич).Y].Value)).ToString(),
                                    SKU = colorCellsList[i][GiveColorCell(ParamEnum.SKU).Y].Value,
                                    Type = OrdersTaypeEnum.Возврат
                                };
                                NOrderItem.Article1C = mModel.Option.MarketItems.First(x => x.SKU == NOrderItem.SKU).Art1C;
                                AddItem(Document, NPost, Date, FindPost, NOrderItem, SchetFaktura);
                            }
                        }
                    }
                    if (ActiveFile.APISetting.Type == StructLibCore.Marketplace.MarketName.Yandex) // Возврат для Яндекса 
                    {
                        for (int i = GiveColorCell(ParamEnum.НачСтрокВозврат).X; i < GiveColorCell(ParamEnum.КонСтрокВозврат).X + 1; i++)
                        {
                            string OrderReturnPrice = colorCellsList[i][GiveColorCell(ParamEnum.ВозвратСумма).Y].Value;
                            if (OrderReturnPrice != "0" && OrderReturnPrice != "")
                            {
                                string NPost = colorCellsList[i][GiveColorCell(ParamEnum.НомерВозврата).Y].Value;
                                string Date = DateTime.Now.ToString().Split(" ")[0];
                                SF SchetFaktura = null;
                                if (OrderReturnPrice != "")
                                {
                                    FindPost = Document.Orders.Find(x => x.DepartureNumber == NPost);
                                    OrderItem NOrderItem = new()
                                    {
                                        Count = colorCellsList[i][GiveColorCell(ParamEnum.ВозвратКолич).Y].Value,
                                        Price = (double.Parse(OrderReturnPrice) / double.Parse(colorCellsList[i][GiveColorCell(ParamEnum.ВозвратКолич).Y].Value)).ToString(),
                                        SKU = colorCellsList[i][GiveColorCell(ParamEnum.SKU).Y].Value,
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

                                    NOrderItem.Article1C = mModel.Option.MarketItems.First(x => x.SKU == NOrderItem.SKU).Art1C;
                                    AddItem(Document, NPost, Date, FindPost, NOrderItem, SchetFaktura);
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
                        Order NOrder = new Order() { DepartureNumber = NPost, DepartureDate = Date, SchetFaktura = SchetFaktura };
                        NOrder.Items.Add(NOrderItem);
                        Document.Orders.Add(NOrder);
                    }
                }
            }
            catch (Exception E)
            {
                TestParamStack();
                MessageBox.Show(E.Message);
            }
        }
        private string FindArt1C(string sKU)
        {
            string X = mModel.Option.MarketItems.First(x => x.SKU == sKU).Art1C;
            if (X == "")
            {
                MessageBox.Show("Не найден артикул для SKU" + sKU);
            }
            return X;
        }
        private static void SaveDocument(Document Document)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Document));
            using (FileStream fs = new FileStream(Document.Nomber + ".xml", FileMode.OpenOrCreate))
            {
                xmlSerializer.Serialize(fs, Document);
            }
        }
        private SF GetSchetFaktura(string NNomer, string SFNomber, ref string[][] byerSFmass)
        {
            FileInfo Fn = FolderFile.First(x => x.Name.Contains(NNomer) && x.Name.Contains("DocumentB2BSales") && !x.Name.Contains("#"));
            if (byerSFmass == null) { try { byerSFmass = ReadFileToMass(Fn.FullName)[0]; } catch { MessageBox.Show("Не удалось загрузить файл B2B"); } }
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
                Document Document = new Document
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
                MessageBox.Show(E.Message);
                return null;
            }

        }
        private void TestParamStack()
        {
            foreach (object[] item in ParamButtons)
            {
                IEnumerable<ColorCell> X = from x in ParamStack where x.Param == (ParamEnum)item[0] select x;
                if (X.Count() == 0)
                {
                    MessageBox.Show("Не заполнен " + ((ParamEnum)item[0]).ToString());
                    break;
                }
           }

        }
        private void Save_Options(object sender, RoutedEventArgs e)
        {
            if (ActiveFile != null)
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(FileOption));
                ActiveFile.ParamsColl = new List<ParamsColl>();
                foreach (ColorCell item in ParamStack)
                {
                    ActiveFile.ParamsColl.Add(new ParamsColl() { Param = item.Param, X = item.X, Y = item.Y });
                }
                if (File.Exists(ActiveFile.FullPath.Replace("xlsx", "xml")))
                {
                    File.Delete(ActiveFile.FullPath.Replace("xlsx", "xml"));
                }
                using (FileStream fs = new FileStream(ActiveFile.FullPath.Replace("xlsx", "xml"), FileMode.OpenOrCreate))
                {
                    xmlSerializer.Serialize(fs, ActiveFile);
                }
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
                MessageBox.Show("Не удалось загрузить настройки");
            }
          
        }
        private FileOption LoadFileOptioins(FileOption FL)
        {
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(FileOption));
                using (FileStream fs = new FileStream(FL.FullPath.Replace("xlsx", "xml"), FileMode.OpenOrCreate))
                {
                    return FL = xmlSerializer.Deserialize(fs) as FileOption;
                }
            }
            catch (Exception E)
            {
                MessageBox.Show("Не удалось прочитать"+ FL.FullPath.Replace("xlsx", "xml"));
                return null;
            }
        }
        private void ReFiilParamsCol(FileOption FL)
        {
            if (colorCellsList != null)
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
                                colorCellsList[item.X][item.Y].Color = (SolidColorBrush)btn[1];
                                colorCellsList[item.X][item.Y].Param = (ParamEnum)btn[0];
                                ParamStack.Add(colorCellsList[item.X][item.Y]);
                                break;
                            }
                        }
                        catch{MessageBox.Show("Не удалось разместить" + item.Param.ToString());}
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
                    while (fnd.Count() > 0)
                    {
                        fnd[0].Color = null;
                        if (ColtextBlock != null)
                        {
                            ColtextBlock.Background = null;
                        }
                        ParamStack.Remove(fnd[0]);
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
            Border borber = ((Border)((Button)sender).Parent);
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
            return null;
        }
        private void Read_file_button_Click(object sender, RoutedEventArgs e)
        {
            FileOption FileI = (FileOption)((Button)sender).DataContext;
            ActiveFile = FileI;
            string[][][] Z = ReadFileToMass(FileI.FullPath);
            colorCellsList = new ObservableCollection<ObservableCollection<ColorCell>>();
            int D = 0;
 

            for (int i = 0; i < Z.Length; i++)
            {
                if (ActiveFile.APISetting != null && ActiveFile.APISetting.Type == StructLibCore.Marketplace.MarketName.Yandex)
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
                    colorCellsList.Add(new ObservableCollection<ColorCell>());
                    for (int x = 0; x < Z[i][p].Length; x++)
                    {
                        colorCellsList[D].Add(new ColorCell() { Value = Z[i][p][x], X = D, Y = x, Lst = i });
                    }
                    D++;
                }
            }
            DTtable.ItemsSource = colorCellsList;
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 1)
            {
                FileOption FL = (MGSol.Panel.FileOption)((ComboBox)sender).DataContext;
                FL.APISetting = mModel.GetApiFromName(e.AddedItems[0].ToString());
            }
        }
        private void ByerComboBoxINN_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 1)
            {
                FileOption FL = (MGSol.Panel.FileOption)((ComboBox)sender).DataContext;
                FL.InnString = mModel.GetInnFromName(e.AddedItems[0].ToString());
            }
        }
    }
}