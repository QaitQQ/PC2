using CRMLibs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
namespace WinFormsClientLib.Forms.WPF.Controls.CRMControls
{
    public partial class MainCRMControl
    {
        public class VisEvent
        {
            public Event Event { get; set; }
            private List<KeyValuePair<int, string>> Users;
            public VisEvent(Event Ev, List<KeyValuePair<int, string>> Users)
            {
                this.Event = Ev;
                this.Users = Users;
                KeyValuePair<int, string> Un = Users.FirstOrDefault(x => x.Key == Ev.UserID);
                IndexUser = Users.IndexOf(Un);
                if (Un.Value == null)
                {
                    UserName = "None";
                }
                else
                {
                    UserName = Un.Value;
                }
            }
            public int Id { get { return Event.Id; } set { Event.Id = Convert.ToInt32(value); } }
            public string Сontent { get { return Event.Сontent; } set { Event.Сontent = value; } }
            public int TypeID { get { return Event.TypeID; } set { Event.TypeID = Convert.ToInt32(value); } }
            public DateTime DatePlanned { get { return Event.DatePlanned; } set { Event.DatePlanned = value; } }
            public DateTime DateОccurred { get { return Event.DateОccurred; } set { Event.DateОccurred = value; } }
            public string UserName { get; set; }
            public int IndexUser { get; set; }
            public void SetUserId() { Event.UserID = Users[IndexUser].Key; }


        }

        public class VisPartner
        {
            public Partner Partner { get; set; }
            private List<KeyValuePair<int, string>> Users;
            public VisPartner(Partner partner, List<KeyValuePair<int, string>> users)
            {

                Partner = partner;
                Users = users;
                if (Partner.LeadManagerId != null )
                {
                        KeyValuePair<int, string> Un = Users.FirstOrDefault(x => x.Key == Partner.LeadManagerId);
                        LeadManager = Users.IndexOf(Un);

                   
                }
               
            }
            public string Name { get { return Partner.Name; } set { Partner.Name = value; } }

            public string City_Name
            {
                get
                {
                    if (Partner.City == null) { return null; } 
                    else { return Partner.City.Name; } }
                set
                { Partner.City = new City() { Name = value }; }
            }

            public string INN { get { return Partner.INN; } set { Partner.INN = value; } }

            public string Adress { get { return Partner.Adress; } set { Partner.Adress = value; } }

            public string Phone { get { return Partner.Phone; } set { Partner.Phone = value; } }

            public string Email { get { return Partner.Email; } set { Partner.Email = value; } }

            public string Contact_1 { get { return Partner.Contact_1; } set { Partner.Contact_1 = value; } }
            public string Contact_2 { get { return Partner.Contact_2; } set { Partner.Contact_2 = value; } }
            public int LeadManager { get; set; }
        }
    }
}
