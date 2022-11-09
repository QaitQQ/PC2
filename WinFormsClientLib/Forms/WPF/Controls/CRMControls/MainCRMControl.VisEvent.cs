
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
            public VisEvent(Event Ev, List<KeyValuePair<int, string>> Users, ObservableCollection<Partner> PartnerList)
            {
                Id = Ev.Id;
                Сontent = Ev.Сontent;
                TypeID = Ev.TypeID;
                DatePlanned = Ev.DatePlanned;
                DateОccurred = Ev.DateОccurred;
                UserID = Ev.UserID;
                PartnerID = Ev.PartnerID;
                Event = Ev;
               
                KeyValuePair<int, string> Un = Users.FirstOrDefault(x => x.Key == UserID);

                IndexUser = Users.IndexOf(Un);
                if (Un.Value == null)
                {
                    UserName = "None";
                }
                else
                {
                    UserName = Un.Value;
                }
                if (PartnerID != 0)
                {
                    Patrner = PartnerList.FirstOrDefault(x => x.Id == PartnerID).Name;
                }
            }
            public int Id { get; set; }
            public string Сontent { get; set; }
            public int TypeID { get; set; }
            public DateTime DatePlanned { get; set; }
            public DateTime DateОccurred { get; set; }
            public int UserID { get; set; }
            public string UserName { get; set; }
            public int PartnerID { get; set; }
            public string Patrner { get; set; }
            public Event Event { get; set; }
            public int IndexUser { get; set; }

        }
    }
}
