using System.Collections.Generic;
using System.Linq;

namespace Server.Class.Query
{
    #region // Кэширование
    internal class NameCash
    {
        public List<KeyValuePair<string, int>> GetCashItemName()
        {
            List<KeyValuePair<string, int>> QweryResult = null;
            using (ApplicationContext db = new ApplicationContext())
            {
                QweryResult = (from Item in db.Item select new KeyValuePair<string, int>(Item.Name, Item.Id)).ToList();
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
