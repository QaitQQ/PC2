using Object_Description;

using Pricecona;

using Server.Class.ItemProcessor;

using Spire.Xls;

using StructLibs;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace XLS
{
    internal class XLS_TO_LIST : IDisposable
    {
        private Workbook _workbook;
        private Worksheet _Worksheet;
        private IDictionaryPC _Dictionary;
        public XLS_TO_LIST(Stream stream) { _workbook = new Workbook(); using (Stream Stream = stream) { _workbook.LoadFromStream(Stream); }; }
        #region поиски
        private int RCCellFind(ref int rowRCFin)
        {
            rowRCFin = 1;
            string tester = "";
            int cellRC = 4;
            int rowRC;
            for (rowRC = 1; rowRC < 30; rowRC++)
            {
                for (int cell = 1; cell < 20; cell++)
                {
                    tester = Convert.ToString(_Worksheet[rowRC, cell].Value);
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

            Spire.Xls.Collections.PicturesCollection Pictures = null;
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
                while (image.Count > 1)
                {
                    List<Image> NewListimage = new List<Image>();
                    int Size = image[0].Height + image[0].Width;
                    foreach (Image item in image)
                    {
                        int itemSize = item.Height + item.Width;
                        if (itemSize > Size)
                        {
                            NewListimage.Add(item);
                            Size = itemSize;
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
                                x = x + tester.Length;
                                tester = Convert.ToString(_Worksheet[m + 2, cell].Value);
                                x = x + tester.Length;
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
                foreach (char Ch in STR) { if (char.IsDigit(Ch) || Ch == ',') { ResultSTR += Ch; } }
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
                return ReadStorage(FileName, WorksheetsNumbers);
            }
            else
            {
                return null;
            }

        }
        private List<PriceStruct> ReadPrice(string FileName, List<int> WorksheetsNumbers)
        {
            List<PriceStruct> DataList = new List<PriceStruct>();
            for (int n = 0; n < WorksheetsNumbers.Count; n++)
            {
                int WorksheetsN = WorksheetsNumbers[n];
                _Worksheet = _workbook.Worksheets[WorksheetsN];

                int cellRC = 0, NamePoz = 0, cellDC = 0, DescPoz = 0, MaxRow = 250, IDcall = 0, SKUcall = 0, nullstr = 0, rowRCFin = 1;

                ColumnPriceOrient(ref rowRCFin, ref cellRC, ref NamePoz, ref cellDC, ref DescPoz, ref MaxRow, ref SKUcall);

                for (int Row = rowRCFin; Row < MaxRow;)
                {
                    if (nullstr > 50)
                    {
                        break;
                    }
                    if (Convert.ToString(_Worksheet[Row, cellRC].Value) != "")
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

                        double PriceRC = ReadStringToDouble(_Worksheet[Row, cellRC].Value);
                        if (PriceRC == 0) { Row++; continue; }

                        if (IDcall != 0) { SKU = _Worksheet[Row, SKUcall].Value; } else { SKU = null; }
                        PriceRC = ReadStringToDouble(_Worksheet[Row, cellRC].Value);

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

                        if (PriceRC != -1)
                        {
                            DataList.Add(new PriceStruct()
                            {
                                Name = NameString,
                                PriceRC = PriceRC,
                                PriceDC = PriceDC,
                                Description = DescriptionString,
                                DateСhange = DateСhange,
                                Pic = Pic,
                                Currency = Currency,
                                PriceListName = PriceListName,
                                SourceName = FileName,
                                Sku = SKU
                            });
                        }
                        Row++;
                    }
                    else { Row++; nullstr++; }
                }
            }

            return DataList;

            void ColumnPriceOrient(ref int rowRCFin, ref int cellRC, ref int NamePoz, ref int cellDC, ref int DescPoz, ref int MaxRow, ref int SKUcall)
            {

                if (((DictionaryPrice)_Dictionary).Filling_method_string?.Count > 0)
                {                 
                    if (NamePoz == 0) { NamePoz = ColFindOnValue(((DictionaryPrice)_Dictionary).GetFillingStringUnderRelate(FillDefinitionPrice.Name)); }
                    if (cellRC == 0) { cellRC = RCCellFind(ref rowRCFin); }
                    if (cellDC == 0) { cellDC = DC_CellFind(cellRC, rowRCFin); }
                    if (DescPoz == 0) { DescPoz = DescriptionFind(); }
                }
                else if (((DictionaryPrice)_Dictionary).Filling_method_coll?.Count >0)
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
                else
                {
                    NamePoz = ColFindOnValue(((DictionaryPrice)_Dictionary).GetFillingStringUnderRelate(FillDefinitionPrice.Name));
                    cellRC = RCCellFind(ref rowRCFin);
                    cellDC = DC_CellFind(cellRC, rowRCFin);
                    DescPoz = DescriptionFind();
                }
            }
        }
        private List<Storage> ReadStorage(string FileName, List<int> WorksheetsNumbers)
        {

            List<Storage> DataList = new List<Storage>();

            for (int n = 0; n < WorksheetsNumbers.Count; n++)
            {
                int WorksheetsN = WorksheetsNumbers[n];
                _Worksheet = _workbook.Worksheets[WorksheetsN];





            }







                return null;
        
        
        
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
        void IDisposable.Dispose() => Dispose(true);
        #endregion
    }
}



