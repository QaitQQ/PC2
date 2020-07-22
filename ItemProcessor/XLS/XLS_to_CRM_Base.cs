using CRMLibs;

using Spire.Xls;

using System;
using System.Collections.Generic;
using System.IO;

namespace Server
{
    internal class XLS_to_CRM_Base
    {
        private readonly Workbook workbook;
        public XLS_to_CRM_Base(Stream Book)
        {
            workbook = new Workbook();
            workbook.LoadFromStream(Book);
        }

        public List<Partner> Read()
        {

            List<Partner> FinList = new List<Partner>();
            bool EndFile = true;
            int i = 2;

            Worksheet Sheet = workbook.Worksheets[0];


            while (EndFile)
            {
                if (Sheet[i, 1].Value != "")
                {
                    Partner Partner = null;
                    СontactPerson Person = null;
                    Event Event = null;

                    List<Partner> FindPartner = FinList.FindAll(x => x.Name == Sheet[i, 1].Value);

                    if (FindPartner.Count == 0)
                    { Partner = new Partner() { Name = Sheet[i, 1].Value, Аddress = Sheet[i, 3].Value + Sheet[i, 2].Value, Phone = Sheet[i, 4].Value, СontacPersons = new List<СontactPerson>(), Events = new List<Event>() }; }
                    else
                    {
                        Partner = FindPartner[0];
                        FinList.Remove(Partner);
                    }

                    List<СontactPerson> СontacPersons = Partner.СontacPersons.FindAll(x => x.Name == Sheet[i, 6].Value);

                    if (СontacPersons.Count == 0)
                    { Person = new СontactPerson() { Name = Sheet[i, 6].Value }; }
                    else
                    { Person = СontacPersons[0]; }
                    DateTime DateОccurred = DateTime.Now;
                    Partner.СontacPersons.Add(Person);
                    try { DateОccurred = Convert.ToDateTime(Sheet[i, 10].Value); }
                    catch { }

                    Event = new Event() { Сontent = Sheet[i, 7].Value, DateОccurred = DateОccurred, PartnerID = Partner.Id, СontactPersonID = Person.Id };
                    Partner.Events.Add(Event);

                    FinList.Add(Partner);

                    i++;
                }
                else
                {
                    EndFile = false;
                }

            }
            return FinList;


        }
    }
}



