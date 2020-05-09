using Client.Class.Net;

using CRMLibs;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WinFormsClientLib.Forms.WPF.Controls.CRMControls
{

    public partial class MainCRMControl : UserControl
    {
        private ObservableCollection<Partner> _PartnerList;

        private Partner _ActivePartner;
        public Partner ActivePartner { get => _ActivePartner; private set { _ActivePartner = value; ChangedPartner?.Invoke(_ActivePartner);} }

        public event Action<Partner> ChangedPartner;

        enum SearchComboBox { Name }

        private ObservableCollection<UIElement> _Filters;
        private ObservableCollection<Event> _Events;

        public ObservableCollection<UIElement> Filters
        {
            get { return _Filters; }
            set { _Filters = value; }
        }
        public ObservableCollection<Partner> PartnerList
        {
            get { return _PartnerList; }
            set { _PartnerList = value; }
        }
        public ObservableCollection<Event> EventsList
        {
            get { return _Events; }
            set { _Events = value; }
        }
        public MainCRMControl()
        {
            _PartnerList = new ObservableCollection<Partner>();
            _Events = new ObservableCollection<Event>();
            _Filters = new ObservableCollection<UIElement>();
            foreach (var item in new CrmNetClass().GetPartnerList()) { _PartnerList.Add(item); }
            InitializeComponent();
            Partners.ItemsSource = PartnerList;
            ComboRelateBox.ItemsSource = Enum.GetValues(typeof(SearchComboBox)).Cast<SearchComboBox>();
            ComboRelateBox.SelectedIndex = 0;
            FilterStack.ItemsSource = Filters;
            Filters.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(FindFilter);
        }

        private void Partners_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ActivePartner = (Partner)Partners.SelectedItem;
            if (ActivePartner != null)
            {
                EventsList = new ObservableCollection<Event>();
                var Events = new CrmNetClass().ShowEvents(ActivePartner.Id);
                if (Events != null)
                {
                    foreach (var item in Events)
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
            if (Filters.Count != 0)
            {


                foreach (var item in Filters)
                {
                    var X = ((CRMFilter)item).FilterValue;
                    IEnumerable<Partner> S = PartnerList.Where(x => x.Name.Contains(X));
                    PartnerList = new ObservableCollection<Partner>();
                    foreach (var Item in S) { PartnerList.Add(Item); }

                    Partners.ItemsSource = PartnerList;
                }
            }
            else
            {
                foreach (var item in new CrmNetClass().GetPartnerList()) { _PartnerList.Add(item); }
            }
        }

    }
}
