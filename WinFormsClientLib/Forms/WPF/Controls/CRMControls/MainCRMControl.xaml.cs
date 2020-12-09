
using Client;

using CRMLibs;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
        public ObservableCollection<Event> EventsList { get; set; }
        private readonly ObservableCollection<Button> ButtonsPartnerInteractionPanel;
        private readonly ObservableCollection<Button> ButtonsEventInteractionPanel;
        public MainCRMControl()
        {
            PartnerList = new ObservableCollection<Partner>();
            EventsList = new ObservableCollection<Event>();
            Filters = new ObservableCollection<UIElement>();
            List<Partner> Partn = new Network.CRM.GetAllPartners().Get<List<Partner>>(new WrapNetClient());
            foreach (Partner item in Partn) { PartnerList.Add(item); }
            InitializeComponent();
            Partners.ItemsSource = PartnerList;
            ComboRelateBox.ItemsSource = Enum.GetValues(typeof(SearchComboBox)).Cast<SearchComboBox>();
            ComboRelateBox.SelectedIndex = 0;
            FilterStack.ItemsSource = Filters;
            Filters.CollectionChanged += new NotifyCollectionChangedEventHandler(FindFilter);
            ButtonsPartnerInteractionPanel = new ObservableCollection<Button>();
            ButtonsEventInteractionPanel = new ObservableCollection<Button>();
            PartnerInteractionPanel.ItemsSource = ButtonsPartnerInteractionPanel;
            EventInteractionPanel.ItemsSource = ButtonsEventInteractionPanel;
            ChangedPartner += new Action<Partner>(RenewActivePartner);
            SupportButton.AddButtons(ButtonsPartnerInteractionPanel, new RoutedEventHandler[] { AddPartner, DelPartner });
            SupportButton.AddButtons(ButtonsEventInteractionPanel, new RoutedEventHandler[] { AddEvent, DelEvent });
        }
        private void Partners_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RenewEvents();
        }

        private void RenewEvents()
        {
            ActivePartner = (Partner)Partners.SelectedItem;
            if (ActivePartner != null)
            {
                EventsList = new ObservableCollection<Event>();
                List<Event> Events = new Network.CRM.GetEventsFromPartnerID().Get<List<Event>>(new WrapNetClient(), ActivePartner.Id);
                if (Events != null)
                {
                    foreach (Event item in Events)
                    {
                        EventsList.Add(item);
                    }
                    EventsLst.ItemsSource = EventsList;
                }
            }
        }

        private void AddFilter_Click(object sender, RoutedEventArgs e)
        {
            Filters.Add(new CRMFilter(ComboRelateBox.Text, FilterTextBox.Text, this));
            RenewPartnerList();
        }
        public void RenewPartnerList()
        {
            PartnerList = new ObservableCollection<Partner>();
            if (Filters.Count > 0)
            {
                foreach (var item in Filters)
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
            System.Windows.Forms.Form PW = NewPartnerWindow();
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
            System.Windows.Forms.Form EW = NewEventWindow();
            if (EW.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                Event NewEvent = new Event() { Сontent = ((System.Windows.Forms.TextBox)EW.Controls[1]).Text, DateОccurred = DateTime.Now, PartnerID = ActivePartner.Id };

                if (new Network.CRM.AddEvent().Get<bool>(new WrapNetClient(), NewEvent))
                {
                    RenewEvents();
                }
            }
        }
        private void DelEvent(object Obj, RoutedEventArgs e)
        {

            var ActiveEvent = (Event)EventsLst.SelectedItem;

            if (new Network.CRM.DelEvent().Get<bool>(new WrapNetClient(), ActiveEvent))
            {
                RenewEvents();
            }

        }
        private System.Windows.Forms.Form NewEventWindow()
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
        private System.Windows.Forms.Form NewPartnerWindow()
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
}
