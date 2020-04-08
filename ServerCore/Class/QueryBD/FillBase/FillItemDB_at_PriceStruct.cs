using Pricecona;

using StructLibs;

using System.Collections.Generic;

namespace Server.Class.Query
{
    #region // Заполнение базы
    internal class FillItemDB_at_PriceStruct
    {
        public FillItemDB_at_PriceStruct()
        {
            List<PriceStruct> List = new OpenAtFile().RetunList();
            List<Manufactor> Manufactors = new List<Manufactor>();


            using (ApplicationContext db = new ApplicationContext())
            {
                foreach (PriceStruct item in List)
                {
                    Manufactor _Manufactor;

                    List<Manufactor> Man = Manufactors.FindAll(X => X.Name == item.Manufactor);


                    if (Man.Count == 0)
                    {
                        _Manufactor = new Manufactor() { Name = item.Manufactor };
                    }
                    else
                    {
                        _Manufactor = Man[0];
                    }

                    _Manufactor = new Manufactor() { Name = item.Manufactor };

                    db.Item.Add(new ItemDBStruct() { Id = item.Id, Currency = item.Currency, Image = item.Imagelink, DateСhange = item.DateСhange, Description = item.Description, Sku = item.Sku, Name = item.Name, SourceName = item.SourceName, СomparisonName = item.СomparisonName, PriceDC = item.PriceDC, PriceRC = item.PriceRC, ManufactorID = _Manufactor.Id, PriceListName = item.PriceListName });
                }

                db.SaveChanges();
            }

        }
    }
    #endregion
}
