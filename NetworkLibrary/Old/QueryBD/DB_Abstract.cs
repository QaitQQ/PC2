using System.Collections;
using System.Collections.Generic;

namespace Server.Class.Query
{

    public abstract class DB_Abstract<T>
    {
        public virtual List<T> Find(string Str) { List<T> QweryResult = null; return QweryResult; }
        public virtual List<T> FindID(int ID) { List<T> QweryResult = null; return QweryResult; }
        public virtual void Add(T Obj)
        {
            if (Obj != null)
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    db.Add(Obj);
                    db.SaveChanges();
                }
            }
        }
        public virtual void Del(T Obj)
        {

            if (Obj != null)
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    db.Remove(Obj);
                    db.SaveChanges();
                }
            }


        }
        public virtual void Edit(T Obj)
        {
            if (Obj != null)
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    db.Update(Obj);
                    db.SaveChanges();
                }
            }
        }
        public virtual void EditList(List<T> Obj)
        {
            if (Obj != null)
            {
                using ApplicationContext db = new ApplicationContext();
                foreach (object item in Obj as IEnumerable)
                {
                    db.Update(item);
                }
                db.SaveChanges();
            }
        }


    }

}
