using ApiLib.ApiBase.ItemApi;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using static SiteApi.IntegrationSiteApi.ApiBase.ItemApi.AbstractItemApi;
namespace Pwa.Pages;
public partial class ItemPage : ContentView { public ItemPage() { InitializeComponent(); } }
public partial class ItemsModel : ContentView
{
    public ICommand SearchClick { get; private set; }
    public string Name { get; private set; }
    public VisSelectedItem _ListBoxSelectedItem { get; private set; }
    public ObservableCollection<IAbstractItem> FindItems { get; private set; }
    public VisSelectedItem ActiveItem { get; private set; }
    public ObservableCollection<ContentView> ItemList { get; private set; }
    public string SearchText { get; set; }
    public ItemsModel()
    {
        FindItems = [];
        _ListBoxSelectedItem = new VisSelectedItem();
        ActiveItem = new VisSelectedItem();
        SearchBind();
        SelectedBind();
        ItemList = [];

    }
    private void SearchBind()
    {
        SearchClick = new Command(() =>
        {
            if (SearchText?.Length > 0)
            {
                object T = new ItemSearch("a225d71e16ddf2d0780728e88073e8f409dd2a75", @"http://192.168.8.112:65530").Post(SearchText);
                if (T != null && T is IList && (T as IList).Count > 0)
                {
                    FindItems.Clear();
                    foreach (object item in T as IEnumerable)
                    {
                        if (item is IAbstractItem)
                        {
                            FindItems.Add(item as IAbstractItem);
                        }
                    }
                }
            }
        });
    }
    private void SelectedBind()
    {
        _ListBoxSelectedItem.PropertyChanged += (s, e) =>
        {
            ActiveItem.Item = new GetItem("a225d71e16ddf2d0780728e88073e8f409dd2a75", @"http://192.168.8.112:65530").Get(((ApiLib.ApiBase.ItemApi.ItemSearch.FItem)((Pwa.Pages.ItemsModel.VisSelectedItem)s).Item).Id.ToString());
            if (ItemList.Count == 3)
            {
                ItemList.Clear();
            }

            ItemList.Add(new Controls.ItemControl(ActiveItem.Item));
        };
    }
    public class VisSelectedItem : INotifyPropertyChanged
    {
        private IAbstractItem item;
        public IAbstractItem Item
        {
            get => item;
            set { item = value; OnPropertyChanged("Item"); }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
