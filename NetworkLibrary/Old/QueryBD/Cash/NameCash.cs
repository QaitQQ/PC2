using StructLibs;
using System.Collections.Generic;
using System.Linq;

namespace Server.Class.Query
{
    #region // Кэширование
    public class NameCash
    {


        public List<СomparisonNameID> GetCashItemName()
        {
            List<СomparisonNameID> QweryResult = null;
            using (ApplicationContext db = new ApplicationContext())
            {
                QweryResult = (from Item in db.Item select new СomparisonNameID() {Name = Item.Name, СomparisonName = Item.СomparisonName[0], Id=Item.Id }).ToList();
            }
            return QweryResult;
        }
        public List<string> GetCashUserName()
        {
            List<string> QweryResult = null;
            using (ApplicationContext db = new ApplicationContext())
            {
                QweryResult = (from User in db.User select User.Name).ToList();
            }
            return QweryResult;
        }
    }
    #endregion
}
