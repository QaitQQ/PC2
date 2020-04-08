using CRMLibs;

using System.Collections.Generic;
using System.Linq;

namespace Server.Class.Query
{
    #region // Кэширование

    #endregion
    internal class PartnersQuery : DB_Abstract<Partner>
    {
        public override List<CRMLibs.Partner> Find(string Name)
        {
            List<CRMLibs.Partner> QweryResult = null;
            if (Name != null)
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    QweryResult = db.Partner.Where(item => item.Name.Contains(Name)).ToList();
                }
            }
            else
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    QweryResult = db.Partner.ToList();
                }
            }
            return QweryResult;
        }
        public override List<Partner> FindID(int ID)
        {
            List<Partner> QweryResult = null;
            if (ID != 0)
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    QweryResult = db.Partner.Where(item => item.Id == ID).ToList();
                }
            }
            return QweryResult;
        }
    }

}
