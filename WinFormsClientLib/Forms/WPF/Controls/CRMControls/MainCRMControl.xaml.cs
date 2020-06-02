
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
        public event Action<Partner> ChangedPartner;
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
            var Partn = new Network.CRM.GetAllPartners().Get<List<Partner>>(new WrapNetClient());

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

            SupportButton.AddButtons(ButtonsPartnerInteractionPanel, new RoutedEventHandler[] { AddPartner, DelPartner });
            SupportButton.AddButtons(ButtonsEventInteractionPanel, new RoutedEventHandler[] { AddEvent, DelEvent });
        }
        private void Partners_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ActivePartner = (Partner)Partners.SelectedItem;
            if (ActivePartner != null)
            {
              EventsList = new ObservableCollection<Event>();
                List<Event> Events = new Network.CRM.GetEventsFromPartnerID().Get<List<Event>>(new WrapNetClient(),ActivePartner.Id);
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



        private void AddPartner(object Obj, RoutedEventArgs e) { }
        private void DelPartner(object Obj, RoutedEventArgs e) { }
        private void AddEvent(object Obj, RoutedEventArgs e) { }
        private void DelEvent(object Obj, RoutedEventArgs e) { }
    }
}
