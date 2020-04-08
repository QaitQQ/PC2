namespace Server.Class
{
    //internal class FillBase
    //{
    //    private List<PriceStruct> ReadXLS ( Stream stream )
    //    {

    //        List<PriceStruct> list;
    //        XLS.XLS_TO_LIST Book = new XLS.XLS_TO_LIST(stream);
    //        list = Book.Read(null);

    //        return list;
    //    }
    //    public void FillAtPrice ( )
    //    {
    //        using (ApplicationContext db = new ApplicationContext())
    //        {

    //            List<PriceStruct> list;

    //            using (FileStream stream = File.OpenRead($"DSSL2.xlsx"))
    //            {
    //                list = ReadXLS(stream);
    //            }

    //            foreach (PriceStruct item in list)
    //            {
    //                PriceStruct _NewItem = item;
    //                if (item.Pic != null)
    //                {
    //                    _NewItem.Imagelink = @"pic\" + item.СomparisonName + "." + item.Pic.RawFormat;
    //                    item.Pic.Save(_NewItem.Imagelink);

    //                }

    //                ItemDBStruct NewItem = new ItemDBStruct(_NewItem);
    //                db.Add(NewItem);
    //                db.SaveChanges();
    //            }

    //        }
    //    }
    //    public void FillAtMail ( )
    //    {

    //        Stream Post = new Imap_Connector().GetAttach(out string FileName, out string Subject);
    //        List<PriceStruct> @base = new List<PriceStruct>();
    //        if (Post != null)
    //        {

    //            XLS.XLS_TO_LIST Xls = new XLS.XLS_TO_LIST(Post);
    //            @base = Xls.Read(null, FileName: null);


    //        }

    //        FillAtStruct(@base);

    //    }
    //    public void FillAtStruct ( List<PriceStruct> @base )
    //    {

    //        using (ApplicationContext db = new ApplicationContext())
    //        {



    //            foreach (PriceStruct item in @base)
    //            {
    //                PriceStruct _NewItem = item;
    //                _NewItem.Imagelink = item.SaveImage();
    //                ItemDBStruct NewItem = new ItemDBStruct(_NewItem);
    //                db.Add(NewItem);
    //                db.SaveChanges();
    //            }

    //        }


    //    }
    //    public void AddUser ( )
    //    {
    //        using (ApplicationContext db = new ApplicationContext())
    //        {

    //            db.Add(new Object_Description.Access_Struct.User() { Name = "Admin", Pass = "123" });

    //            db.SaveChanges();
    //        }
    //    }
    //    public void FillCRM ( )
    //    {

    //        using (ApplicationContext db = new ApplicationContext())
    //        {

    //            using (FileStream stream = File.OpenRead($"Руслан.xls"))
    //            {

    //                List<CRMLibs.Partner> list = new XLS_to_CRM_Base(stream).Read();



    //                foreach (CRMLibs.Partner item in list)
    //                {
    //                    db.Add(item);
    //                }

    //            }

    //            db.SaveChanges();
    //        }
    //    }
    //}
}
