using System.Collections.Generic;
using System.Linq;

namespace Server.Class.Query
{
    internal class EventQuery : DB_Abstract<CRMLibs.Event>
    {
        public override List<CRMLibs.Event> Find(string Str)
        {
            List<CRMLibs.Event> QweryResult = null;
            if (Str != null)
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    QweryResult = db.Events.Where(item => item.Сontent.Contains(Str)).ToList();
                }
            }

            return QweryResult;
        }
        public override List<CRMLibs.Event> FindID(int ID)
        {
            List<CRMLibs.Event> QweryResult = null;
            if (ID != 0)
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    QweryResult = db.Events.Where(item => item.PartnerID == ID).ToList();
                }
            }

            return QweryResult;


        }

    }

}
