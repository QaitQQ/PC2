using ApiLib.ApiBase.ItemApi;
using Pwa.Api.ApiBase.ItemApi;
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
    private ApiBase _apiBase { get; set; }
    public ICommand SearchClick { get; private set; }
    public ChangedObject<string> Message { get; set; }
    public ChangedObject<int> ErrorDeskHeight { get; set; }
    public string Name { get; private set; }
    public VisSelectedItem _ListBoxSelectedItem { get; private set; }
    public ObservableCollection<IAbstractItem> FindItems { get; private set; }
    public VisSelectedItem ActiveItem { get; private set; }
    public ObservableCollection<ContentView> ItemList { get; private set; }
    public string SearchText { get; set; }
    public ItemsModel()
    {
        Message = new ChangedObject<string>("");
        ErrorDeskHeight = new ChangedObject<int>(0);
        FindItems = [];
        _ListBoxSelectedItem = new VisSelectedItem();
        ActiveItem = new VisSelectedItem();
        SearchBind();
        SelectedBind();
        ItemList = [];
        _apiBase = new ApiBase() { Token = "a225d71e16ddf2d0780728e88073e8f409dd2a75", SiteUrl = @"https://192.168.8.112:65530" };
    }
    private void SearchBind()
    {
        SearchClick = new Command(() =>
        {
            if (SearchText?.Length > 0)
            {
                using var client = _apiBase.Go<ItemSearch>();
                object T = client.Post(SearchText);
                client.Dispose();
                ExceptionView(T);
                if (T != null && T is IList && (T as IList).Count > 0)
                {
                    foreach (var item in FindItems)
                    {
                        item.Dispose();
                    }
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
    private void ExceptionView(object T)
    {
        if (T.ToString().ToLower().Contains("exception"))
        {
            string msg = T.ToString();
            Message.Object = msg;
            if (msg.Length < 100)
            {
                ErrorDeskHeight.Object = 100;
            }
            else
            {
                ErrorDeskHeight.Object = msg.Length / 10;
            }
        }
        else
        {
            ErrorDeskHeight.Object = 0;
        }
    }
    private void SelectedBind()
    {
        _ListBoxSelectedItem.PropertyChanged += (s, e) => Getitem(s, e);
    }
    private void Getitem(object s, object e)
    {
        var _VisSelectedItem = (ItemsModel.VisSelectedItem)s;
        if (_VisSelectedItem.Item != null)
        {
            var Id = ((ItemSearch.FItem)(_VisSelectedItem).Item).Id.ToString();
            ActiveItem.Item = _apiBase.Go<GetItem>().Go(Id);
            if (ItemList.Count == 3)
            {
                foreach (var item in ItemList)
                {
                    ((Controls.ItemControl)item).Dispose();
                }
                ItemList.Clear();
                GC.Collect();
            }
            ItemList.Add(new Controls.ItemControl(ActiveItem.Item));
        }
    }
    public class VisSelectedItem : INotifyPropertyChanged, IDisposable
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
        public void Dispose()
        {
            item = null;
            GC.SuppressFinalize(this);
        }
    }
}
