using Client;
using CRMLibs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace WinFormsClientLib.Forms.WPF.Controls.CRMControls
{
    public partial class MainCRMControl : UserControl
    {
        private Partner _ActivePartner;
        public Partner ActivePartner { get => _ActivePartner; private set { _ActivePartner = value; ChangedPartner?.Invoke(_ActivePartner); } }
        private event Action<Partner> ChangedPartner;
        private enum SearchComboBox { Name }
        public ObservableCollection<UIElement> Filters { get; set; }
        public ObservableCollection<Partner> PartnerList { get; set; }
        public ObservableCollection<VisPartner> VPartnerList { get; set; }
        public ObservableCollection<VisEvent> PartnerEventsList { get; set; }
        public ObservableCollection<VisEvent> TimeEventsList { get; set; }
        private List<KeyValuePair<int, string>> Users { get; set; }
        public MainCRMControl()
        {
            Users = new Network.Аuthorization.GetUserIDList().Get<List<KeyValuePair<int, string>>>(new WrapNetClient());
            PartnerList = new ObservableCollection<Partner>();
            PartnerEventsList = new ObservableCollection<VisEvent>();
            Filters = new ObservableCollection<UIElement>();
            TimeEventsList = new ObservableCollection<VisEvent>();
            VPartnerList = new ObservableCollection<VisPartner>();
            List<Partner> Partn = new Network.CRM.GetAllPartners().Get<List<Partner>>(new WrapNetClient());
            foreach (Partner item in Partn) { PartnerList.Add(item); }
            InitializeComponent();
            foreach (Partner item in PartnerList) { VPartnerList.Add(new VisPartner(item, Users)); }
            Partners.ItemsSource = VPartnerList;
            Filters.CollectionChanged += new NotifyCollectionChangedEventHandler(FindFilter);
            ChangedPartner += new Action<Partner>(RenewActivePartner);
            EventsLst.ItemsSource = PartnerEventsList;
            TimeEvent.ItemsSource = TimeEventsList;
            UsersBox2.ItemsSource = Users;
            UsersBox.ItemsSource = Users;
            UsersBox.SelectedIndex = 0;
            UserFilter.ItemsSource = Users;
            LeadManagerBox.ItemsSource = Users;
            UserFilter.SelectedIndex = Main.ActiveUser;
            if (VPartnerList.Count > 0)
            {
                ActivePartner = VPartnerList[0].Partner;
            }
            else
            {
                ActivePartner = new Partner();
            }
            CompanyInfoGrid.DataContext = new VisPartner(ActivePartner, Users);
        }
        private void Partners_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RenewEvents();
        }
        private void RenewEvents()
        {
            if (Partners.SelectedItem != null)
            {
                EventInfoRow.Height = new GridLength(0);
                var VisP = (VisPartner)Partners.SelectedItem;
                ActivePartner = VisP.Partner;
                CompanyInfoGrid.DataContext = VisP;
                if (ActivePartner != null)
                {
                    PartnerEventsList.Clear();
                    List<Event> Events = new Network.CRM.GetEventsFromPartnerID().Get<List<Event>>(new WrapNetClient(), ActivePartner.Id);
                    if (Events != null)
                    {
                        foreach (Event item in Events)
                        {
                            PartnerEventsList.Add(new VisEvent(item, Users));
                        }
                    }
                }
            }
        }
        public void RenewPartnerList()
        {
            PartnerList = new ObservableCollection<Partner>();
            if (Filters.Count > 0)
            {
                foreach (UIElement item in Filters)
                {
                    List<Partner> Partn = new Network.CRM.PartnersSearch().Get<List<Partner>>(new WrapNetClient(), ((CRMFilter)item).FilterValue);
                    foreach (Partner X in Partn) { PartnerList.Add(X); }
                }
            }
            else
            {
                List<Partner> Partn = new Network.CRM.GetAllPartners().Get<List<Partner>>(new WrapNetClient());
                foreach (Partner X in Partn) { PartnerList.Add(X); }
            }
            Partners.ItemsSource = PartnerList;
        }
        private void FindFilter(object sender, NotifyCollectionChangedEventArgs e)
        {
            //if (Filters.Count != 0)
            //{
            //    foreach (UIElement item in Filters)
            //    {
            //        string X = ((CRMFilter)item).FilterValue;
            //        IEnumerable<Partner> S = PartnerList.Where(x => x.Name.Contains(X));
            //        PartnerList = new ObservableCollection<Partner>();
            //        foreach (Partner Item in S) { PartnerList.Add(Item); }
            //        Partners.ItemsSource = PartnerList;
            //    }
            //}
            //else
            //{
            //    foreach (Partner item in new CrmNetClass().GetPartnerList()) { PartnerList.Add(item); }
            //}
        }
        private void RenewActivePartner(Partner Partner) { ActiveValue.ActivePartner = Partner; }
        private void AddPartner(object Obj, RoutedEventArgs e)
        {
            System.Windows.Forms.Form PW = NewPartnerWindow;
            if (PW.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                Partner NewPartner = new Partner() { Name = ((System.Windows.Forms.TextBox)PW.Controls[1]).Text };
                if (new Network.CRM.AddPartner().Get<bool>(new WrapNetClient(), NewPartner))
                {
                    RenewPartnerList();
                }
            }
        }
        private void DelPartner(object Obj, RoutedEventArgs e)
        {
            ActivePartner = (Partner)Partners.SelectedItem;
            if (new Network.CRM.DelPartner().Get<bool>(new WrapNetClient(), ActivePartner))
            {
                PartnerList.Remove(ActivePartner);
            }
        }
        private void AddEvent(object Obj, RoutedEventArgs e)
        {
            System.Windows.Forms.Form EW = NewEventWindow;
            if (EW.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                Event NewEvent = new Event() { Сontent = ((System.Windows.Forms.RichTextBox)EW.Controls[1]).Text, DateОccurred = DateTime.Now, DatePlanned = ((System.Windows.Forms.DateTimePicker)EW.Controls[3]).Value, PartnerID = ActivePartner.Id, UserID = Main.ActiveUser };
                if (new Network.CRM.AddEvent().Get<bool>(new WrapNetClient(), NewEvent))
                {
                    RenewEvents();
                }
            }
        }
        private System.Windows.Forms.Form NewEventWindow
        {
            get
            {
                System.Windows.Forms.Form form = new System.Windows.Forms.Form
                {
                    Width = 480,
                    Height = 200,
                };
                System.Windows.Forms.Button OK = new System.Windows.Forms.Button
                {
                    Text = "ОК",
                    Location = new System.Drawing.Point(400, 115),
                    Size = new System.Drawing.Size(50, 30),
                    DialogResult = System.Windows.Forms.DialogResult.OK
                };
                form.Controls.Add(OK);
                System.Windows.Forms.DateTimePicker date1 = new System.Windows.Forms.DateTimePicker() { Location = new System.Drawing.Point(5, 5) };
                date1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
                date1.CustomFormat = "dd/MM/yyyy hh:mm:ss";
                System.Windows.Forms.DateTimePicker date2 = new System.Windows.Forms.DateTimePicker() { Location = new System.Drawing.Point(250, 5) };
                date2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
                date2.CustomFormat = "dd/MM/yyyy hh:mm:ss";
                System.Windows.Forms.RichTextBox box = new System.Windows.Forms.RichTextBox
                {
                    Location = new System.Drawing.Point(3, 50),
                    Size = new System.Drawing.Size(450, 60),
                };
                form.Controls.Add(box);
                form.Controls.Add(date1);
                form.Controls.Add(date2);
                form.ShowDialog();
                return form;
            }
        }
        private System.Windows.Forms.Form NewPartnerWindow
        {
            get
            {
                System.Windows.Forms.Form form = new System.Windows.Forms.Form
                {
                    Width = 300,
                    Height = 100
                };
                System.Windows.Forms.Button OK = new System.Windows.Forms.Button
                {
                    Text = "ОК",
                    Location = new System.Drawing.Point(50, 30),
                    Size = new System.Drawing.Size(50, 30),
                    DialogResult = System.Windows.Forms.DialogResult.OK
                };
                form.Controls.Add(OK);
                System.Windows.Forms.TextBox box = new System.Windows.Forms.TextBox
                {
                    Location = new System.Drawing.Point(3, 3),
                    Size = new System.Drawing.Size(280, 25)
                };
                form.Controls.Add(box);
                form.ShowDialog();
                return form;
            }
        }
        private void TimeCal_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TimeCal != null)
            {
                if (TimeCal.SelectedDate == null)
                {
                    TimeCal.SelectedDate = DateTime.Now;
                }
                DateTime Dat = (DateTime)TimeCal.SelectedDate;
                List<Event> Events;
                if (UsersBox != null)
                {
                    if (PlanedBox.SelectedIndex == 1)
                    {
                        Events = new Network.CRM.GetEventFromDateОccurred().Get<List<Event>>(new WrapNetClient(), new KeyValuePair<int, DateTime>(((KeyValuePair<int, string>)UsersBox.SelectedItem).Key, Dat));
                    }
                    else
                    {
                        Events = new Network.CRM.GetEventFromDateDatePlanned().Get<List<Event>>(new WrapNetClient(), new KeyValuePair<int, DateTime>(((KeyValuePair<int, string>)UsersBox.SelectedItem).Key, Dat));
                    }
                    if (Events != null)
                    {
                        TimeEventsList.Clear();
                        foreach (Event item in Events)
                        {
                            TimeEventsList.Add(new VisEvent(item, Users));
                        }
                    }
                }
            }
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            var ViEv = (VisEvent)btn.DataContext;
            if (ViEv != null) { EventGrid.DataContext = ViEv; }
            EventInfoRow.Height = new GridLength(150);
        }
        private void Partners_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            bool endSearch = false;
            if (Partners.IsMouseOver)
            {
                endSearch = false;
                SearchBox.Text = "";
                Partners.ContextMenu.PlacementTarget = Partners;
                Partners.ContextMenu.PlacementRectangle = new Rect(50, 50, 0, 0);
                Partners.ContextMenu.IsOpen = true;
                Keyboard.Focus(SearchBox);
                Partners.ContextMenu.Closed += (e, x) =>
                {
                    if (!endSearch)
                    {
                        try
                        {
                            VPartnerList.Clear();
                            IEnumerable<Partner> S = PartnerList.Where(x => x.Name.ToLower().Contains(SearchBox.Text.ToLower()));
                            foreach (Partner item in S)
                            {
                                VPartnerList.Add(new VisPartner(item, Users));
                            }
                            endSearch = true;
                        }
                        catch { }
                    }
                };
            }
        }
        private void GridSplitter_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.RoutedEvent.Name == "MouseDoubleClick" || e.ClickCount == 2)
            {
                if (ComranyInfoRow.Height.Value == 0)
                {
                    ComranyInfoRow.Height = new GridLength(150);
                }
                else
                {
                    ComranyInfoRow.Height = new GridLength(0);
                }
            }
        }
        private void CityName_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox TB = (TextBox)sender;
            Partner Partner = (CRMLibs.Partner)TB.DataContext;
            if (Partner.City == null && TB.Text != "")
            {
                Partner.City = new City() { Name = TB.Text };
            }
        }
        private void SaveEventButton_Click(object sender, RoutedEventArgs e)
        {
            EventInfoRow.Height = new GridLength(0);
            var VEv = (VisEvent)EventGrid.DataContext;
            VEv.SetUserId();
            Event EV = VEv.Event;
            bool TP = new Network.CRM.SaveEvent().Get<bool>(new WrapNetClient(), EV);
        }
        private void SavePartnerButton_Click(object sender, RoutedEventArgs e)
        {
            ComranyInfoRow.Height = new GridLength(0);
            var VisP = (VisPartner)Partners.SelectedItem;
            if (VisP != null)
            {
                ActivePartner.LeadManagerId = Users[(int)VisP.LeadManager].Key;
            }
            bool TP = new Network.CRM.UpdatePartner().Get<bool>(new WrapNetClient(), ActivePartner);
        }
        private void UserFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UserFilter.SelectedItem != null)
            {
                try
                {
                    VPartnerList.Clear();
                    IEnumerable<Partner> S = PartnerList.Where(x => x.LeadManagerId == ((KeyValuePair<int, string>)UserFilter.SelectedItem).Key);
                    foreach (Partner item in S)
                    {
                        VPartnerList.Add(new VisPartner(item, Users));
                    }
                }
                catch { }
            }
            else
            {
                foreach (Partner item in PartnerList)
                {
                    VPartnerList.Add(new VisPartner(item, Users));
                }
            }
        }
        private void UserFilterNull_Click(object sender, RoutedEventArgs e)
        {
            UserFilter.SelectedItem = null;
        }
    }
}
