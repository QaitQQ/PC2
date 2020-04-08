using StructLibs;

using System.Linq;

namespace Server
{
    internal class Сompare_ItemDBStruct_with_DB
    {
        private readonly ItemDBStruct _Item;

        public Сompare_ItemDBStruct_with_DB(ItemDBStruct Item) => _Item = Item;

        private void Сomparer()
        {
            IQueryable<ItemDBStruct> reply;

            using (ApplicationContext db = new ApplicationContext())
            {
                reply = (from Item in db.Item where Item.СomparisonName == _Item.СomparisonName select Item);

                if (reply.Count() == 1)
                {
                    db.Update(_Item);
                    db.SaveChanges();
                }

            }



        }

    }


}
