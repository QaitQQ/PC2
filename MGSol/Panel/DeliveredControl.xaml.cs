using SiteApi.IntegrationSiteApi.APIMarket.Ozon.Post;
using StructLibCore.Marketplace;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace MGSol.Panel
{
    public partial class DeliveredControl : UserControl
    {
        public DeliveredControl(MainModel Model)
        {
            InitializeComponent();
            this.Model = Model;
            var list = new List<PostingsList>();
            foreach (StructLibCore.Marketplace.APISetting item in Model.OptionMarketPlace.APISettings)
            {
                if (item.Type == MarketName.Ozon)
                {
                    list.Add(new PostingsList(new OzonPostMonthOrderList(item).Get(), item));
                }
            }
            foreach (var item in list)
            {
                foreach (var X in item.Postings)
                {
                    if (X.AnalyticsData.Region == "" || X.AnalyticsData.Region == null)
                    {
                        X.AnalyticsData.Region = X.AnalyticsData.City;
                    }
                }
            }
            this.DataContext = new DeliveredModel(list);
        }
        public MainModel Model { get; }
        private void Label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Clipboard.SetText(((Label)sender).Content.ToString());
        }
    }
    public class DeliveredModel : UserControl
    {
        public ExName ExNameStr { get; set; }
        public ObservableCollection<PostingsList> _list { get; set; }
        public DeliveredModel(List<PostingsList> list)
        {
            ExNameStr = new ExName();
            ExNameStr.PropertyChanged += (s, e) =>
            {
                if (_list != null && ExNameStr != null)
                {
                    foreach (var item in _list)
                    {
                        item.Filter = ExNameStr.Name.Replace(" ", "").Split(',').ToList();
                    }
                }
            };
            _list = new ObservableCollection<PostingsList>();
            foreach (var item in list)
            {
                _list.Add(item);
            }
            Btn = new Btn_click();
        }
        public ICommand Btn { get; set; }
    }
    public class Btn_click : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {
        }
        public event EventHandler CanExecuteChanged;
    }
    public class ExName : INotifyPropertyChanged
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged(); }
        }
        public void OnPropertyChanged([CallerMemberName] string name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        public event PropertyChangedEventHandler PropertyChanged;
    }
    public class PostingsList : INotifyPropertyChanged
    {
        public PostingsList(OzonPostMonthOrderList.Result list, StructLibCore.Marketplace.APISetting aPISetting)
        {
            this.list = list;
            APISetting = aPISetting;
        }
        private List<string> filter;
        public List<string> Filter
        {
            get { return filter; }
            set { filter = value; OnPropertyChanged(); Postings = new List<OzonPostMonthOrderList.Posting>(); }
        }
        public APISetting APISetting { get; private set; }
        private OzonPostMonthOrderList.Result list { get; set; }
        public List<OzonPostMonthOrderList.Posting> Postings
        {
            set { OnPropertyChanged(); }
            get
            {
                var lst = list.Postings;
                if (Filter?.Count > 0)
                {
                    foreach (var item in Filter)
                    {
                        lst = lst.FindAll(x => !x.AnalyticsData.Region.ToLower().Contains(item.ToLower()));
                    }
                }
                return lst;
                //    return list.Postings;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
