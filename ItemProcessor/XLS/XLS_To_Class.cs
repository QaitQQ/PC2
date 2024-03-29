﻿using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

using Object_Description;

using Spire.Xls;

using StructLibs;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
namespace XLS
{
    public class XLS_To_Class : IDisposable
    {
        private IWorkbook _workbook;
        private ISheet _Worksheet;
        private IDictionaryPC _Dictionary;
        public XLS_To_Class()
        {
        }
        #region поиски
        private int FindStringValueCell(string Str)
        {
            Str = Str.ToLower();
            for (int x = 1; x < 50; x++)
            {
                for (int y = 1; y < 25; y++)
                {
                    string tester = GetCellValue(x, y);
                    if (tester == null || tester == "")
                    {
                        continue;
                    }
                    if (tester.ToLower().Contains(Str))
                    {
                        return y;
                    }
                }
            }
            return 0;
        }
        private int RCCellFind(ref int rowRCFin)
        {
            rowRCFin = 1;
            string tester;
            int cellRC = 4;
            int rowRC;
            for (rowRC = 1; rowRC < 30; rowRC++)
            {
                for (int cell = 1; cell < 20; cell++)
                {
                    tester = GetCellValue(rowRC, cell);
                    if (tester == null || tester == "")
                    {
                        continue;
                    }
                    foreach (KeyValuePair<FillDefinitionPrice, string> item in ((DictionaryPrice)_Dictionary).GetFillingStringUnderRelate(FillDefinitionPrice.PriceRC))
                    {
                        if (tester.ToUpper().Contains(item.Value.ToUpper()))
                        {
                            cellRC = cell;
                            CellRangeAddress r = GetMergedRegion(_Worksheet, rowRC, cell);
                            if (r != null)
                            {
                                int Testrow = rowRC;
                                for (int i = Testrow++; i < Testrow + 6; i++)
                                {
                                    double One = ReadStringToDouble(_Worksheet.GetRow(rowRC).GetCell(cell).StringCellValue);
                                    double Two = ReadStringToDouble(_Worksheet.GetRow(rowRC).GetCell(cell).StringCellValue);
                                    if (One > Two)
                                    {
                                        cellRC = r.FirstColumn;
                                    }
                                    else
                                    {
                                        cellRC = r.LastColumn;
                                    }
                                }
                            }
                            rowRC++;
                            rowRCFin = rowRC;
                            return cellRC;
                        };
                    }
                }
            }
            return cellRC;
        }   // пытаемся найти розницу
        private int DC_CellFind(int cellRC, int rowRCFin)
        {
            int cellDC = cellRC;
            double RC;
            double[] String = new double[20];
            if (((DictionaryPrice)_Dictionary).GetFillingStringUnderRelate(FillDefinitionPrice.PriceDC).Count > 0)
            {
                for (int rowRC = 1; rowRC < 30; rowRC++)
                {
                    for (int cell = 1; cell < 20; cell++)
                    {
                        string tester = GetCellValue(rowRC, cell);
                        if (tester == null || tester == "")
                        {
                            continue;
                        }
                        foreach (KeyValuePair<FillDefinitionPrice, string> item in ((DictionaryPrice)_Dictionary).GetFillingStringUnderRelate(FillDefinitionPrice.PriceDC))
                        {
                            if (tester.ToUpper().Contains(item.Value.ToUpper()))
                            {
                                cellDC = cell;
                                CellRangeAddress r = GetMergedRegion(_Worksheet, rowRC, cell);
                                if (r != null)
                                {
                                    int Testrow = rowRC;
                                    for (int i = Testrow++; i < Testrow + 6; i++)
                                    {
                                        double One = ReadStringToDouble(_Worksheet.GetRow(rowRC).GetCell(cell).StringCellValue);
                                        double Two = ReadStringToDouble(_Worksheet.GetRow(rowRC).GetCell(cell).StringCellValue);
                                        if (One < Two)
                                        {
                                            cellDC = r.FirstColumn;
                                        }
                                        else
                                        {
                                            cellDC = r.LastColumn;
                                        }
                                    }
                                }
                                rowRC++;
                                if (cellDC != cellRC)
                                {
                                    return cellDC;
                                }
                            };
                        }
                    }
                }
            }
            for (int m = 0; m < 20; m++)
            {
                string str = GetCellValue(rowRCFin + m, cellRC);
                if (str != "")
                {
                    RC = ReadStringToDouble(str);
                    if (RC != -1)
                    {
                        if (RC != 0)
                        {
                            for (int i = 1; i < 20; i++)
                            {

                                var WCL = _Worksheet.GetRow(rowRCFin + m).GetCell(i);

                                if (WCL != null && WCL.CellType == CellType.Formula)
                                {
                                    String[i] = ReadStringToDouble(_Worksheet.GetRow(rowRCFin + m).GetCell(i).CellFormula?.ToString());
                                }
                                else
                                {
                                    String[i] = ReadStringToDouble(GetCellValue((rowRCFin + m), i));
                                }
                            }
                        }
                        double smallest = String[0];
                        for (int i = 1; i < 20; i++)
                        {
                            if (String[i] == 0 || String[i] < RC / 2) { continue; }
                            if (smallest > String[i] && smallest != 0)
                            {
                                smallest = String[i];
                            }
                            if (String[i] != 0 && smallest == 0) { smallest = String[i]; }
                        }
                        for (int i = 0; i < 20; i++)
                        {
                            if (smallest == String[i] && smallest != 0)
                            {
                                cellDC = i;
                                break;
                            }
                        }
                        if (cellDC != cellRC)
                        {
                            break;
                        }
                    }
                }
            }
            return cellDC;
        }//пытаемся определить наименьшую цену из ряда  
        private Image ImageFind(int Row)
        {
            _ = new List<Image>();
            // IDrawing Pictures = null;
            //try
            //{
            //    Pictures = _Worksheet.DrawingPatriarch;
            //    _Worksheet.GetRow().GetCell().
            //}
            //catch
            //{
            //    return null;
            //}
            //foreach (ExcelPicture Picture in Pictures)
            //{
            //    int _row = Picture.TopRow;
            //    int _col = Picture.LeftColumn;
            //    if (_row == Row)
            //    {
            //        image.Add(Picture.Picture);
            //    }
            //    else
            //    {
            //        CellRange Range = _Worksheet[_row, _col].MergeArea;
            //        if (Range != null && Range.Row <= Row && Row <= Range.LastRow)
            //        {
            //            image.Add(Picture.Picture);
            //        }
            //    }
            //}
            //if (image.Count > 1)
            //{
            //    while (image.Count > 1)
            //    {
            //        List<Image> NewListimage = new List<Image>();
            //        int Size = image[0].Height + image[0].Width;
            //        foreach (Image item in image)
            //        {
            //            int itemSize = item.Height + item.Width;
            //            if (itemSize > Size)
            //            {
            //                NewListimage.Add(item);
            //                Size = itemSize;
            //            }
            //        }
            //        if (NewListimage.Count > 0)
            //        {
            //            image = NewListimage;
            //        }
            //        else
            //        {
            //            image.RemoveRange(1, image.Count - 1);
            //        }
            //    }
            //}
            //else
            //{ image.Add(null); }
            return null;
        } //пытаемся найти изображений
        private int ColFindOnValue(List<KeyValuePair<FillDefinitionPrice, string>> FillingStringUnderRelate)   // пытаемся найти наименование
        {
            int NamePoz = 0;
            {
                for (int i = 1; i < 30; i++)
                {
                    for (int cell = 1; cell < 10; cell++)
                    {
                        string tester = GetCellValue(i, cell);
                        if (tester == null || tester == "")
                        {
                            continue;
                        }
                        if (FillingStringUnderRelate.Count != 0)
                        {
                            foreach (KeyValuePair<FillDefinitionPrice, string> item in FillingStringUnderRelate)
                            {
                                string vl = item.Value;
                                if (tester.ToUpper().Contains(vl.ToUpper()))
                                {
                                    NamePoz = cell;
                                    return NamePoz;
                                };
                            }
                        }
                    }
                }
            }
            if (NamePoz == 0)
            {
                NamePoz = 1;
            }
            return NamePoz;
        }
        private int DescriptionFind()   // пытаемся найти описание
        {
            int DescPoz = 0;
            foreach (KeyValuePair<FillDefinitionPrice, string> item in ((DictionaryPrice)_Dictionary).GetFillingStringUnderRelate(FillDefinitionPrice.Description))
            {
                for (int i = 1; i < 30; i++)
                {
                    for (int cell = 1; cell < 10; cell++)
                    {
                        int x = 0;
                        string tester = GetCellValue(i, cell);
                        if (tester == null || tester == "") { continue; }
                        if (Regex.IsMatch(tester, item.Value, RegexOptions.IgnoreCase))
                        {
                            for (int m = i; m < 30; m++)
                            {
                                m++;
                                tester = GetCellValue(m, cell);
                                if (tester == null || tester == "") { continue; }
                                x = tester.Length;
                                tester = GetCellValue(m + 1, cell);
                                if (tester == null || tester == "") { continue; }
                                x += tester.Length;
                                tester = GetCellValue(m + 2, cell);
                                if (tester == null || tester == "") { continue; }
                                x += tester.Length;
                                if (x > 120)
                                {
                                    DescPoz = cell;
                                    i++;
                                    break;
                                }
                                else { continue; }
                            }
                            if (DescPoz != 0) { break; }
                        }
                    }
                    if (DescPoz != 0) { break; }
                }
                if (DescPoz != 0) { break; }
            }
            if (DescPoz == 0) { DescPoz = 1; }
            return DescPoz;
        }
        private double ReadStringToDouble(string STR)
        {
            if (STR != null && Regex.IsMatch(STR, @"\d") && STR.Length < 30)
            {
                STR = STR.Replace(".", ",");
                string ResultSTR = null;
                int comma = 0;
                for (int i = 0; i < STR.Length; i++)
                {
                    if (char.IsDigit(STR[i]) || STR[i] == ',')
                    {
                        if (STR[i] == ',' && comma == 0)
                        {
                            comma = i;
                        }
                        else if (STR[i] == ',' && comma != 0)
                        {
                            ResultSTR = ResultSTR.Replace(",", "");
                            comma = i;
                        }
                        ResultSTR += STR[i];
                    }
                }
                Regex regex = new Regex(@"^,");
                Regex regex2 = new Regex(@",$");
                if (ResultSTR != null && ResultSTR != "" && !regex.IsMatch(ResultSTR) && !regex2.IsMatch(ResultSTR) && !ResultSTR.Contains(",,"))
                {
                    return Convert.ToDouble(ResultSTR);
                }
            }
            return -1;
        }
        private string FindCurrency(int row, int RCcell, int CurrencyCell)
        {
            string Currency = null;
            // var X = _Worksheet.GetRow(row).GetCell(RCcell).CellStyle;
            Currency = _Worksheet.GetRow(row).GetCell(RCcell).CellStyle.GetDataFormatString();
            Currency += GetCellValue(row, RCcell);
            Currency += GetCellValue(row, CurrencyCell);
            if (Currency.Contains("р.") || Currency.Contains("руб")) { Currency = "RUB"; }
            else if (Currency.Contains("USD")) { Currency = "USD"; }
            return Currency;
        }
        private int CellCurrencyFind()
        {
            for (int rowRC = 1; rowRC < 30; rowRC++)
            {
                for (int cell = 1; cell < 20; cell++)
                {
                    string tester = GetCellValue(rowRC, cell);
                    if (tester == null || tester == "")
                    {
                        continue;
                    }
                    foreach (KeyValuePair<FillDefinitionPrice, string> item in ((DictionaryPrice)_Dictionary).GetFillingStringUnderRelate(FillDefinitionPrice.Currency))
                    {
                        if (tester.ToUpper().Contains(item.Value.ToUpper()))
                        {
                            return cell;
                        }
                    }
                }
            }
            return 0;
        }
        #endregion
        public object Read(Stream stream, List<int> WorksheetsNumbers, string FileName = null, IDictionaryPC Dictionary = null)
        {
            _Dictionary = Dictionary;
            Stream St2 = new MemoryStream();
            using (Stream Stream = stream)
            {
                try
                {
                    stream.CopyTo(St2);
                    XLS_To_ClassSpire X = new XLS_To_ClassSpire(stream);
                    return X.Read(WorksheetsNumbers, FileName, Dictionary);
                }
                catch
                {
                    try
                    {
                        St2.Position = 0;
                        _workbook = new XSSFWorkbook(St2);
                    }
                    catch
                    {
                        try
                        {
                            _workbook = new HSSFWorkbook(St2);
                        }
                        catch
                        {
                        }
                    }
                }
            }
            if ((WorksheetsNumbers == null || WorksheetsNumbers.Count == 0) && _workbook != null) { WorksheetsNumbers = new List<int>(); for (int i = 0; i < _workbook.NumberOfSheets; i++) { WorksheetsNumbers.Add(i); } }
            if (Dictionary.Relate == DictionaryRelate.Price)
            {
                return ReadPrice(FileName, WorksheetsNumbers);
            }
            else if (Dictionary.Relate == DictionaryRelate.Storage)
            {
                return null;
            }
            else
            {
                return null;
            }
        }
        private List<ItemPlusImageAndStorege> ReadPrice(string FileName, List<int> WorksheetsNumbers)
        {
            List<ItemPlusImageAndStorege> DataList = new List<ItemPlusImageAndStorege>();
            for (int n = 0; n < WorksheetsNumbers.Count; n++)
            {
                int WorksheetsN = WorksheetsNumbers[n]; // тащим номер страницы из сообщенного списка
                _Worksheet = _workbook.GetSheetAt(WorksheetsN); // тащим страницу в общую переменную согласно списку
                int cellRC = 0, NamePoz = 0, cellDC = 0, DescPoz = 0, MaxRow = 250, SKUcall = 0, nullstr = 0, rowRCFin = 1, cellCurrency = 0; //объявляем переменные
                KeyValuePair<int, string>[] cellStorege = null;
                ColumnPriceOrient(ref rowRCFin, ref cellRC, ref NamePoz, ref cellDC, ref DescPoz, ref MaxRow, ref SKUcall, ref cellStorege, ref cellCurrency);
                List<KeyValuePair<FillDefinitionPrice, string>> PriceRCE = ((DictionaryPrice)_Dictionary).GetFillingStringUnderRelate(FillDefinitionPrice.PriceRCException);
                string PriceRCException = null;
                if (PriceRCE != null && PriceRCE.Count != 0)
                {
                    PriceRCException = PriceRCE[0].Value;
                }
                for (int Row = rowRCFin; Row < MaxRow; Row++)
                {
                    if (nullstr > 50)
                    {
                        break;
                    }
                    string tester = GetCellValue(Row, cellRC);
                    if (tester != null && tester != "")
                    {
                        if (nullstr > 10) { nullstr = 0; }
                        DateTime DateСhange = DateTime.Now;
                        string PriceListName = _Worksheet.SheetName;
                        string NameString = GetCellValue(Row, NamePoz);
                        if (NameString == null || NameString == "")
                        {
                            continue;
                        }
                        Image Pic = ImageFind(Row);
                        string SKU
                            //   DCstring
                            ;
                        string DescriptionString = "";
                        if (NameString == "" || NameString == " ") { Row++; continue; }
                        if (((DictionaryPrice)_Dictionary).GetFillingStringUnderRelate(FillDefinitionPrice.NamePattern).Count == 1)
                        {
                            string vl = ((DictionaryPrice)_Dictionary).GetFillingStringUnderRelate(FillDefinitionPrice.NamePattern)[0].Value;
                            Regex regex = new Regex(vl, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
                            NameString = NameString.Replace(".", ",");
                            if (regex.Matches(NameString).Count > 0) { NameString = regex.Matches(NameString)[0].Value; }
                        }
                        string PriceRCRow = GetCellValue(Row, cellRC);
                        double PriceRC = 0;
                        if (PriceRCException != null && PriceRCRow.Contains(PriceRCException))
                        {
                            PriceRC = 0;
                        }
                        else
                        {
                            PriceRC = ReadStringToDouble(GetCellValue(Row, cellRC));
                        }
                        if (SKUcall != 0) { SKU = _Worksheet.GetRow(Row).GetCell(SKUcall).StringCellValue; } else { SKU = null; }
                        //  if (_Worksheet.GetRow(Row).GetCell(cellDC).IsPartOfArrayFormulaGroup) { DCstring = _Worksheet.GetRow(Row).GetCell(cellDC).CellFormula; }
                        //   else { DCstring = GetCellValue(Row, cellDC); }
                        double PriceDC = PriceRC;
                        //  ReadStringToDouble(DCstring);
                        //if (_Worksheet.GetRow(Row).GetCell(DescPoz).IsMergedCell)
                        //{
                        //    if (_Worksheet.GetRow(Row).GetCell(DescPoz).CellType == CellType.Formula) 
                        //    { DescriptionString = _Worksheet.GetRow(Row).GetCell(DescPoz).CellFormula?.ToString(); }
                        //    else { DescriptionString = GetMergedRegion(_Worksheet, Row, DescPoz).FormatAsString(); }
                        //}
                        //else
                        //{
                        //    if (_Worksheet.GetRow(Row).GetCell(DescPoz).IsPartOfArrayFormulaGroup) 
                        //    { DescriptionString = _Worksheet.GetRow(Row).GetCell(DescPoz).CellFormula; }
                        //    else { DescriptionString = GetCellValue(Row, DescPoz); }
                        //}
                        string Currency = FindCurrency(Row, cellRC, cellCurrency);
                        List<Storage> stList = null;
                        if (cellStorege != null)
                        {
                            stList = new List<Storage>();
                            foreach (KeyValuePair<int, string> item in cellStorege)
                            {
                                string X = GetCellValue(Row, item.Key);
                                if (X != null)
                                {
                                    stList.Add(new Storage()
                                    {
                                        Warehouse = new Warehouse() { Name = FileName+":"+ item.Value },
                                        Count = (int)ReadStringToDouble(X),
                                        DateСhange = DateTime.Now,
                                        SourceName = NameString
                                    });
                                }
                            }
                        }
                        if (PriceRC != -1)
                        {
                            DataList.Add(new ItemPlusImageAndStorege()
                            {
                                Image = Pic,
                                Item = new ItemDBStruct()
                                {
                                    Name = NameString,
                                    PriceRC = PriceRC,
                                    PriceDC = PriceDC,
                                    Description = DescriptionString,
                                    DateСhange = DateСhange,
                                    Currency = Currency,
                                    PriceListName = PriceListName,
                                    SourceName = FileName,
                                    Sku = SKU
                                },
                                Storages = stList?.ToArray()
                            });
                        }
                    }
                    else { nullstr++; }
                }
            }
            return DataList;
            void ColumnPriceOrient(ref int rowRCFin, ref int cellRC, ref int NamePoz, ref int cellDC, ref int DescPoz, ref int MaxRow, ref int SKUcall, ref KeyValuePair<int, string>[] callsStorege, ref int cellCurrency)
            {
                if (((DictionaryPrice)_Dictionary).Filling_method_coll?.Count > 0)
                {
                    foreach (KeyValuePair<FillDefinitionPrice, int> item in ((DictionaryPrice)_Dictionary).Filling_method_coll)
                    {
                        switch (item.Key)
                        {
                            case FillDefinitionPrice.Sku:
                                SKUcall = item.Value;
                                break;
                            case FillDefinitionPrice.Name:
                                NamePoz = item.Value;
                                break;
                            case FillDefinitionPrice.PriceRC:
                                cellRC = item.Value;
                                for (int i = 0; i < 40; i++)
                                {
                                    string X = GetCellValue(i, cellRC);
                                    if (ReadStringToDouble(X) != 0)
                                    {
                                        rowRCFin = i;
                                        break;
                                    }
                                }
                                break;
                            case FillDefinitionPrice.PriceDC:
                                cellDC = item.Value;
                                break;
                            case FillDefinitionPrice.Description:
                                DescPoz = item.Value;
                                break;
                            case FillDefinitionPrice.Currency:
                                break;
                            case FillDefinitionPrice.MaxRow:
                                MaxRow = item.Value;
                                break;
                            default:
                                break;
                        }
                    }
                }
                if (((DictionaryPrice)_Dictionary).Filling_method_string?.Count > 0)
                {
                    if (NamePoz == 0) { NamePoz = ColFindOnValue(((DictionaryPrice)_Dictionary).GetFillingStringUnderRelate(FillDefinitionPrice.Name)); }
                    if (cellRC == 0) { cellRC = RCCellFind(ref rowRCFin); }
                    if (cellDC == 0) { cellDC = DC_CellFind(cellRC, rowRCFin); }
                    if (DescPoz == 0) { DescPoz = DescriptionFind(); }
                    if (cellCurrency == 0) { cellCurrency = CellCurrencyFind(); }
                    if (callsStorege == null)
                    {
                        List<KeyValuePair<FillDefinitionPrice, string>> STinfo = ((DictionaryPrice)_Dictionary).GetFillingStringUnderRelate(FillDefinitionPrice.Storege);
                        if (STinfo.Count > 0)
                        {
                            List<KeyValuePair<int, string>> Lst = new List<KeyValuePair<int, string>>();
                            foreach (KeyValuePair<FillDefinitionPrice, string> item in STinfo)
                            {
                                if (item.Value.Contains(':'))
                                {
                                    string[] mass = item.Value.Split(':');
                                    int СellТumber = Convert.ToInt32(mass[0]);
                                    string Name = mass[1];
                                    Lst.Add(new KeyValuePair<int, string>(СellТumber, Name));
                                }
                                else
                                {
                                    Lst.Add(new KeyValuePair<int, string>(FindStringValueCell(item.Value), item.Value));
                                }
                            }
                            callsStorege = Lst.ToArray();
                        }
                    }
                    if (MaxRow == 250)
                    {
                        List<KeyValuePair<FillDefinitionPrice, string>> X = ((DictionaryPrice)_Dictionary).GetFillingStringUnderRelate(FillDefinitionPrice.MaxRow);
                        if (X.Count != 0)
                        {
                            MaxRow = Convert.ToInt32(X[0].Value);
                        }
                    }
                }
            }
        }
        private CellRangeAddress GetMergedRegion(ISheet sheet, int rowNum, int cellNum)
        {
            for (int i = 0; i < sheet.NumMergedRegions; i++)
            {
                CellRangeAddress merged = sheet.GetMergedRegion(i);
                if (merged.IsInRange(rowNum, cellNum))
                {
                    return merged;
                }
            }
            return null;
        }
        private string GetCellValue(int Row, int Col)
        {
            ICell X = _Worksheet.GetRow(Row)?.GetCell(Col);
            if (X == null)
            {
                return null;
            }
            return X.CellType switch
            {
                CellType.Unknown => null,
                CellType.Numeric => X.NumericCellValue.ToString(),
                CellType.String => X.StringCellValue,
                CellType.Formula => X.CellFormula,
                CellType.Blank => "",
                CellType.Boolean => X.BooleanCellValue.ToString(),
                CellType.Error => null,
                _ => null,
            };
        }
        #region IDisposable Support
        private bool disposedValue = false; // Для определения избыточных вызовов
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _workbook = null;
                    _Worksheet = null;
                    _Dictionary = null;
                }
                disposedValue = true;
            }
        }
        void IDisposable.Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
    public class XLS_To_ClassSpire : IDisposable
    {
        private Workbook _workbook;
        private Worksheet _Worksheet;
        private IDictionaryPC _Dictionary;
        public XLS_To_ClassSpire(Stream stream) { _workbook = new Workbook(); using (Stream Stream = stream) { _workbook.LoadFromStream(Stream); }; }
        #region поиски
        private int FindStringValueCell(string Str)
        {
            Str = Str.ToLower();
            for (int x = 1; x < 50; x++)
            {
                for (int y = 1; y < 25; y++)
                {
                    string tester = Convert.ToString(_Worksheet[x, y].Value);
                    if (tester.ToLower().Contains(Str))
                    {
                        return y;
                    }
                }
            }
            return 0;
        }
        private int RCCellFind(ref int rowRCFin)
        {
            rowRCFin = 1;
            int cellRC = 4;
            int rowRC;
            for (rowRC = 1; rowRC < 30; rowRC++)
            {
                for (int cell = 1; cell < 20; cell++)
                {
                    string tester = Convert.ToString(_Worksheet[rowRC, cell].Value);
                    foreach (KeyValuePair<FillDefinitionPrice, string> item in ((DictionaryPrice)_Dictionary).GetFillingStringUnderRelate(FillDefinitionPrice.PriceRC))
                    {
                        if (tester.ToUpper().Contains(item.Value.ToUpper()))
                        {
                            cellRC = cell;
                            CellRange r = _Worksheet[rowRC, cellRC].MergeArea;
                            if (r != null)
                            {
                                int Testrow = rowRC;
                                for (int i = Testrow++; i < Testrow + 6; i++)
                                {
                                    double One = ReadStringToDouble(_Worksheet[i, r.Column].Value);
                                    double Two = ReadStringToDouble(_Worksheet[i, r.LastColumn].Value);
                                    if (One > Two)
                                    {
                                        cellRC = r.Column;
                                    }
                                    else
                                    {
                                        cellRC = r.LastColumn;
                                    }
                                }
                            }
                            rowRC++;
                            rowRCFin = rowRC;
                            return cellRC;
                        };
                    }
                }
            }
            return cellRC;
        }   // пытаемся найти розницу
        private int DC_CellFind(int cellRC, int rowRCFin)
        {
            int cellDC = cellRC;
            double RC;
            double[] String = new double[20];
            if (((DictionaryPrice)_Dictionary).GetFillingStringUnderRelate(FillDefinitionPrice.PriceDC).Count > 0)
            {
                for (int rowRC = 1; rowRC < 30; rowRC++)
                {
                    for (int cell = 1; cell < 20; cell++)
                    {
                        string tester = Convert.ToString(_Worksheet[rowRC, cell].Value);
                        foreach (KeyValuePair<FillDefinitionPrice, string> item in ((DictionaryPrice)_Dictionary).GetFillingStringUnderRelate(FillDefinitionPrice.PriceDC))
                        {
                            if (tester.ToUpper().Contains(item.Value.ToUpper()))
                            {
                                cellDC = cell;
                                CellRange r = _Worksheet[rowRC, cellDC].MergeArea;
                                if (r != null)
                                {
                                    int Testrow = rowRC;
                                    for (int i = Testrow++; i < Testrow + 6; i++)
                                    {
                                        double One = ReadStringToDouble(_Worksheet[i, r.Column].Value);
                                        double Two = ReadStringToDouble(_Worksheet[i, r.LastColumn].Value);
                                        if (One < Two)
                                        {
                                            cellDC = r.Column;
                                        }
                                        else
                                        {
                                            cellDC = r.LastColumn;
                                        }
                                    }
                                }
                                rowRC++;
                                if (cellDC != cellRC)
                                {
                                    return cellDC;
                                }
                            };
                        }
                    }
                }
            }
            for (int m = 0; m < 20; m++)
            {
                string str = _Worksheet[rowRCFin + m, cellRC].Value;
                if (str != "")
                {
                    RC = ReadStringToDouble(str);
                    if (RC != -1)
                    {
                        if (RC != 0)
                        {
                            for (int i = 1; i < 20; i++)
                            {
                                String[i] = ReadStringToDouble(_Worksheet[rowRCFin + m, i].FormulaValue?.ToString());
                            }
                        }
                        double smallest = String[0];
                        for (int i = 1; i < 20; i++)
                        {
                            if (String[i] == 0 || String[i] < RC / 2) { continue; }
                            if (smallest > String[i] && smallest != 0)
                            {
                                smallest = String[i];
                            }
                            if (String[i] != 0 && smallest == 0) { smallest = String[i]; }
                        }
                        for (int i = 0; i < 20; i++)
                        {
                            if (smallest == String[i] && smallest != 0)
                            {
                                cellDC = i;
                                break;
                            }
                        }
                        if (cellDC != cellRC)
                        {
                            break;
                        }
                    }
                }
            }
            return cellDC;
        }//пытаемся определить наименьшую цену из ряда  
        private Image ImageFind(int Row)
        {
            List<Image> image = new List<Image>();
            Spire.Xls.Collections.PicturesCollection Pictures;
            try
            {
                Pictures = _Worksheet.Pictures;
            }
            catch
            {
                return null;
            }
            foreach (ExcelPicture Picture in Pictures)
            {
                int _row = Picture.TopRow;
                int _col = Picture.LeftColumn;
                if (_row == Row)
                {
                    image.Add(Picture.Picture);
                }
                else
                {
                    CellRange Range = _Worksheet[_row, _col].MergeArea;
                    if (Range != null && Range.Row <= Row && Row <= Range.LastRow)
                    {
                        image.Add(Picture.Picture);
                    }
                }
            }
            if (image.Count > 1)
            {
                int Size = 0;
                for (int i = 0; i < image.Count; i++)
                {
                    List<Image> NewListimage = new List<Image>();
                    if (image[i] != null)
                    {
                        if (image[0] != null)
                        {
                            Size = image[0].Height + image[0].Width;
                        }
                        foreach (Image item in image)
                        {
                            if (item != null)
                            {
                                int itemSize = item.Height + item.Width;
                                if (itemSize >= Size)
                                {
                                    NewListimage.Add(item);
                                    Size = itemSize;
                                }
                            }
                        }
                        if (NewListimage.Count > 0)
                        {
                            image = NewListimage;
                        }
                        else
                        {
                            image.RemoveRange(1, image.Count - 1);
                        }
                    }
                }
            }
            else
            { image.Add(null); }
            return image[0];
        } //пытаемся найти изображений
        private int ColFindOnValue(List<KeyValuePair<FillDefinitionPrice, string>> FillingStringUnderRelate)   // пытаемся найти наименование
        {
            int NamePoz = 0;
            {
                for (int i = 1; i < 30; i++)
                {
                    for (int cell = 1; cell < 10; cell++)
                    {
                        string tester = Convert.ToString(_Worksheet[i, cell].Value);
                        if (FillingStringUnderRelate.Count != 0)
                        {
                            foreach (KeyValuePair<FillDefinitionPrice, string> item in FillingStringUnderRelate)
                            {
                                string vl = item.Value;
                                if (tester.ToUpper().Contains(vl.ToUpper()))
                                {
                                    NamePoz = cell;
                                    return NamePoz;
                                };
                            }
                        }
                    }
                }
            }
            if (NamePoz == 0)
            {
                NamePoz = 1;
            }
            return NamePoz;
        }
        private int DescriptionFind()   // пытаемся найти описание
        {
            int DescPoz = 0;
            foreach (KeyValuePair<FillDefinitionPrice, string> item in ((DictionaryPrice)_Dictionary).GetFillingStringUnderRelate(FillDefinitionPrice.Description))
            {
                for (int i = 1; i < 30; i++)
                {
                    for (int cell = 1; cell < 10; cell++)
                    {
                        int x = 0;
                        string tester = Convert.ToString(_Worksheet[i, cell].Value);
                        if (Regex.IsMatch(tester, item.Value, RegexOptions.IgnoreCase))
                        {
                            for (int m = i; m < 30; m++)
                            {
                                m++;
                                tester = Convert.ToString(_Worksheet[m, cell].Value);
                                x = tester.Length;
                                tester = Convert.ToString(_Worksheet[m + 1, cell].Value);
                                x += tester.Length;
                                tester = Convert.ToString(_Worksheet[m + 2, cell].Value);
                                x += tester.Length;
                                if (x > 120)
                                {
                                    DescPoz = cell;
                                    i++;
                                    break;
                                }
                                else { continue; }
                            }
                            if (DescPoz != 0) { break; }
                        }
                    }
                    if (DescPoz != 0) { break; }
                }
                if (DescPoz != 0) { break; }
            }
            if (DescPoz == 0) { DescPoz = 1; }
            return DescPoz;
        }
        private double ReadStringToDouble(string STR)
        {
            if (STR != null && Regex.IsMatch(STR, @"\d") && STR.Length < 30)
            {
                STR = STR.Replace(".", ",");
                string ResultSTR = null;
                int comma = 0;
                for (int i = 0; i < STR.Length; i++)
                {
                    if (char.IsDigit(STR[i]) || STR[i] == ',')
                    {
                        if (STR[i] == ',' && comma == 0)
                        {
                            comma = i;
                        }
                        else if (STR[i] == ',' && comma != 0)
                        {
                            ResultSTR = ResultSTR.Replace(",", "");
                            comma = i;
                        }
                        ResultSTR += STR[i];
                    }
                }
                Regex regex = new Regex(@"^,");
                Regex regex2 = new Regex(@",$");
                if (ResultSTR != null && ResultSTR != "" && !regex.IsMatch(ResultSTR) && !regex2.IsMatch(ResultSTR) && !ResultSTR.Contains(",,"))
                {
                    return Convert.ToDouble(ResultSTR);
                }
            }
            return -1;
        }
        private string FindCurrency(string cell)
        {
            string Currency = null;
            if (cell.Contains("р.") || cell.Contains("руб")) { Currency = "RUB"; }
            else if (cell.Contains("USD")) { Currency = "USD"; }
            return Currency;
        }
        #endregion
        public object Read(List<int> WorksheetsNumbers, string FileName = null, IDictionaryPC Dictionary = null)
        {
            _Dictionary = Dictionary;
            if (WorksheetsNumbers == null || WorksheetsNumbers.Count == 0) { WorksheetsNumbers = new List<int>(); for (int i = 0; i < _workbook.Worksheets.Count; i++) { WorksheetsNumbers.Add(i); } }
            if (Dictionary.Relate == DictionaryRelate.Price)
            {
                return ReadPrice(FileName, WorksheetsNumbers);
            }
            else if (Dictionary.Relate == DictionaryRelate.Storage)
            {
                return null;
            }
            else
            {
                return null;
            }
        }
        private List<ItemPlusImageAndStorege> ReadPrice(string FileName, List<int> WorksheetsNumbers)
        {
            List<ItemPlusImageAndStorege> DataList = new List<ItemPlusImageAndStorege>();
            for (int n = 0; n < WorksheetsNumbers.Count; n++)
            {
                int WorksheetsN = WorksheetsNumbers[n]; // тащим номер страницы из сообщенного списка
                _Worksheet = _workbook.Worksheets[WorksheetsN]; // тащим страницу в общую переменную согласно списку
                int cellRC = 0, NamePoz = 0, cellDC = 0, DescPoz = 0, MaxRow = 250, SKUcall = 0, nullstr = 0, rowRCFin = 1; //объявляем переменные
                KeyValuePair<int, string>[] cellStorege = null;
                ColumnPriceOrient(ref rowRCFin, ref cellRC, ref NamePoz, ref cellDC, ref DescPoz, ref MaxRow, ref SKUcall, ref cellStorege);
                List<KeyValuePair<FillDefinitionPrice, string>> PriceRCE = ((DictionaryPrice)_Dictionary).GetFillingStringUnderRelate(FillDefinitionPrice.PriceRCException);
                string PriceRCException = null;
                if (PriceRCE != null && PriceRCE.Count != 0)
                {
                    PriceRCException = PriceRCE[0].Value;
                }
                for (int Row = rowRCFin; Row < MaxRow;)
                {
                    if (nullstr > 50)
                    {
                        break;
                    }



                    if (Convert.ToString(_Worksheet[Row, cellRC].Value) != "" ||(cellStorege != null && Convert.ToString(_Worksheet[Row, cellStorege[0].Key].Value) != ""))
                    {
                        DateTime DateСhange = DateTime.Now;
                        string PriceListName = _Worksheet.Name;
                        string NameString = _Worksheet[Row, NamePoz].Value;
                        Image Pic = ImageFind(Row);
                        string SKU, DescriptionString, DCstring;
                        if (NameString == "" || NameString == " ") { Row++; continue; }
                        if (((DictionaryPrice)_Dictionary).GetFillingStringUnderRelate(FillDefinitionPrice.NamePattern).Count == 1)
                        {
                            string vl = ((DictionaryPrice)_Dictionary).GetFillingStringUnderRelate(FillDefinitionPrice.NamePattern)[0].Value;
                            Regex regex = new Regex(vl, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
                            NameString = NameString.Replace(".", ",");
                            if (regex.Matches(NameString).Count > 0) { NameString = regex.Matches(NameString)[0].Value; }
                        }
                        string PriceRCRow = _Worksheet[Row, cellRC].Value;
                        double PriceRC = 0;
                        if (PriceRCException != null && PriceRCRow.Contains(PriceRCException))
                        {
                            PriceRC = 0;
                        }
                        else
                        {
                            PriceRC = ReadStringToDouble(_Worksheet[Row, cellRC].Value);
                        }
                        if (SKUcall != 0) { SKU = _Worksheet[Row, SKUcall].Value; } else { SKU = null; }
                        if (_Worksheet[Row, cellDC].HasFormula) { DCstring = _Worksheet[Row, cellDC].FormulaValue.ToString(); }
                        else { DCstring = _Worksheet[Row, cellDC].Value; }
                        double PriceDC = ReadStringToDouble(DCstring);
                        if (_Worksheet[Row, DescPoz].HasMerged)
                        {
                            if (_Worksheet[Row, DescPoz].HasFormula) { DescriptionString = _Worksheet[Row, DescPoz].MergeArea.FormulaValue?.ToString(); }
                            else { DescriptionString = _Worksheet[Row, DescPoz].MergeArea.DisplayedText; }
                        }
                        else
                        {
                            if (_Worksheet[Row, DescPoz].HasFormula) { DescriptionString = _Worksheet[Row, DescPoz].FormulaValue.ToString(); }
                            else { DescriptionString = _Worksheet[Row, DescPoz].Value; }
                        }
                        string Currency = FindCurrency(_Worksheet[Row, cellRC].Style.NumberFormat);
                        List<Storage> stList = null;
                        if (cellStorege != null)
                        {
                            stList = new List<Storage>();
                            foreach (KeyValuePair<int, string> item in cellStorege)
                            {
                                stList.Add(new Storage()
                                {
                                    Warehouse = new Warehouse() { Name = FileName+":"+ item.Value },
                                    Count = (int)ReadStringToDouble(_Worksheet[Row, item.Key].Value),
                                    DateСhange = DateTime.Now,
                                    SourceName = NameString
                                });
                            }
                        }
                        if (PriceRC != -1)
                        {
                            DataList.Add(new ItemPlusImageAndStorege()
                            {
                                Image = Pic,
                                Item = new ItemDBStruct()
                                {
                                    Name = NameString,
                                    PriceRC = PriceRC,
                                    PriceDC = PriceDC,
                                    Description = DescriptionString,
                                    DateСhange = DateСhange,
                                    Currency = Currency,
                                    PriceListName = PriceListName,
                                    SourceName = FileName,
                                    Sku = SKU
                                },
                                Storages = stList?.ToArray()
                            });
                        }
                        Row++;
                     
                    }
                    else { Row++; nullstr++; }
                }
            }
            return DataList;
            void ColumnPriceOrient(ref int rowRCFin, ref int cellRC, ref int NamePoz, ref int cellDC, ref int DescPoz, ref int MaxRow, ref int SKUcall, ref KeyValuePair<int, string>[] callsStorege)
            {
                if (((DictionaryPrice)_Dictionary).Filling_method_coll?.Count > 0)
                {
                    foreach (KeyValuePair<FillDefinitionPrice, int> item in ((DictionaryPrice)_Dictionary).Filling_method_coll)
                    {
                        switch (item.Key)
                        {
                            case FillDefinitionPrice.Sku:
                                SKUcall = item.Value;
                                break;
                            case FillDefinitionPrice.Name:
                                NamePoz = item.Value;
                                break;
                            case FillDefinitionPrice.PriceRC:
                                cellRC = item.Value;
                                break;
                            case FillDefinitionPrice.PriceDC:
                                cellDC = item.Value;
                                break;
                            case FillDefinitionPrice.Description:
                                DescPoz = item.Value;
                                break;
                            case FillDefinitionPrice.Currency:
                                break;
                            case FillDefinitionPrice.MaxRow:
                                MaxRow = item.Value;
                                break;
                            default:
                                break;
                        }
                    }
                }
                if (((DictionaryPrice)_Dictionary).Filling_method_string?.Count > 0)
                {
                    if (NamePoz == 0) { NamePoz = ColFindOnValue(((DictionaryPrice)_Dictionary).GetFillingStringUnderRelate(FillDefinitionPrice.Name)); }
                    if (cellRC == 0) { cellRC = RCCellFind(ref rowRCFin); }
                    if (cellDC == 0) { cellDC = DC_CellFind(cellRC, rowRCFin); }
                    if (DescPoz == 0) { DescPoz = DescriptionFind(); }
                    if (callsStorege == null)
                    {
                        List<KeyValuePair<FillDefinitionPrice, string>> STinfo = ((DictionaryPrice)_Dictionary).GetFillingStringUnderRelate(FillDefinitionPrice.Storege);
                        if (STinfo.Count > 0)
                        {
                            List<KeyValuePair<int, string>> Lst = new List<KeyValuePair<int, string>>();
                            foreach (KeyValuePair<FillDefinitionPrice, string> item in STinfo)
                            {
                                if (item.Value.Contains(':'))
                                {
                                    string[] mass = item.Value.Split(':');
                                    int СellТumber = Convert.ToInt32(mass[0]);
                                    string Name = mass[1];
                                    Lst.Add(new KeyValuePair<int, string>(СellТumber, Name));
                                }
                                else
                                {
                                    Lst.Add(new KeyValuePair<int, string>(FindStringValueCell(item.Value), item.Value));
                                }
                            }
                            callsStorege = Lst.ToArray();
                        }
                    }
                    if (MaxRow == 250)
                    {
                        List<KeyValuePair<FillDefinitionPrice, string>> X = ((DictionaryPrice)_Dictionary).GetFillingStringUnderRelate(FillDefinitionPrice.MaxRow);
                        if (X.Count != 0)
                        {
                            MaxRow = Convert.ToInt32(X[0].Value);
                        }
                    }
                }
            }
        }
        #region IDisposable Support
        private bool disposedValue = false; // Для определения избыточных вызовов
        public virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _workbook = null;
                    _Worksheet = null;
                    _Dictionary = null;
                }
                disposedValue = true;
            }
        }
        void IDisposable.Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
