using System.IO;

namespace Server.Class.Query
{
    #region // Заполнение базы
    internal class QueryBD
    {
        public QueryBD(string FilePath)
        {
            Stream openFileStream = File.OpenRead(FilePath);
        ////    System.Collections.Generic.List<CRMLibs.Partner> _list = new XLS_to_CRM_Base(openFileStream).Read();


        //    using (ApplicationContext db = new ApplicationContext())
        //    {
        //        foreach (CRMLibs.Partner item in _list)
        //        {

        //            db.Partner.Add(item);
        //        }

        //        db.SaveChanges();
        //    }

        }
    }
    #endregion
}
