namespace Server
{
    internal class XLS_Save
    {
        //public void SaveXLSX ( DataGridView priceData )
        //{
        //    using (ExcelPackage eP = new ExcelPackage())
        //    {
        //        Worksheet = eP.Workbook.Worksheets.Add("Прайс");
        //        int row = 2;
        //        int col = 1;
        //        #region Шапка
        //        eP.Workbook.Properties.Author = "Sab";
        //        eP.Workbook.Properties.Title = "Прайс-лист";
        //        eP.Workbook.Properties.Company = "Sab";
        //        Worksheet.Row(1).Height = 80;
        //        string cont = null;
        //        if (Settings.DictionaryFile.GetValues("Contacts") != null)
        //        {
        //            cont = Settings.DictionaryFile.GetValues("Contacts")[0];
        //            cont = cont.Replace("\\r", "\r");
        //        }

        //        Worksheet[1, 1].Value = cont;
        //        Worksheet["A1:F1"].Merge = true;
        //        var logo = Worksheet.Pictures.Add(Image.FromFile("logo.png"), "logo");
        //        logo.SetPosition(1, 1);
        //        logo.From.Column = 0;
        //        logo.From.Row = 0;
        //        logo.SetSize(230, 80);
        //        ShapkaZap(row, col, "№", 5, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
        //        ShapkaZap(row, col + 1, "Картинка", 12, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
        //        ShapkaZap(row, col + 2, "Номенклатура", 20, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, true);
        //        ShapkaZap(row, col + 3, "Розница", 10, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
        //        ShapkaZap(row, col + 4, "Опт", 10, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
        //        ShapkaZap(row, col + 5, "Описание", 120, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Top, true);
        //        #endregion
        //        row++;
        //        for (int i = 3; i < priceData.RowCount + 2; i++)
        //        {
        //            int m = i - 3;
        //            Worksheet.SetRowHeight(row, 65);
        //            Worksheet[i, col].Value = (m + 1).ToString();
        //            Worksheet[i, col + 2].Value = priceData.Rows[m].Cells[1].Value;
        //            Worksheet[i, col + 3].Value = priceData.Rows[m].Cells[2].Value;
        //            Worksheet[i, col + 4].Value = priceData.Rows[m].Cells[3].Value;
        //            Worksheet[i, col + 5].Value = priceData.Rows[m].Cells[5].Value;
        //            if ((Bitmap)priceData.Rows[m].Cells[4].Value != null)
        //            {
        //                Image x = (Bitmap)priceData.Rows[m].Cells[4].Value;
        //                OfficeOpenXml.Drawing.ExcelPicture picture = Worksheet.Drawings.AddPicture(m.ToString(), x);
        //                picture.SetPosition(2, 2);
        //                picture.From.Column = 1;
        //                picture.From.Row = i - 1;
        //                picture.SetSize(50);
        //                if (picture.Image.Height < 50 || picture.Image.Height > 50) { picture.SetSize(70, 50); }
        //            }
        //            show(m, priceData.RowCount + 2);
        //        }// перенос DataGrid в ячейки
        //        using (ExcelRange cells = Worksheet.Cells[Worksheet.Dimension.Address])
        //        {
        //            cells.Style.Border.Top.Style =
        //            cells.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        //            cells.Style.Border.Right.Style = ExcelBorderStyle.Thin;
        //            cells.Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //        } // добави всем ячейкам рамку
        //        Worksheet[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

        //        Worksheet[1, 1].Style.WrapText = true;
        //        byte[] bin = eP.GetAsByteArray();// записываем
        //        try
        //        {
        //            File.WriteAllBytes(@"Price.xlsx", bin);
        //            System.Diagnostics.Process.Start(@"Price.xlsx");
        //        }
        //        catch
        //        {
        //            MessageBox.Show("Файл Занят");
        //        }

        //    }
        //}//Сохранение
        //private void ShapkaZap ( int row, int col, string name, double Width, HorizontalAlignType H, VerticalAlignType V, bool Wrap )
        //{
        //    Worksheet[row, col].Value = name;
        //    Worksheet.SetColumnWidth(col, Width);
        //    Worksheet.Columns[col].Style.HorizontalAlignment = HorizontalAlignType.Center;
        //    Worksheet.Columns[col].Style.VerticalAlignment = VerticalAlignType.Center;
        //    Worksheet.Columns[col].Style.WrapText = Wrap;
    }
}







