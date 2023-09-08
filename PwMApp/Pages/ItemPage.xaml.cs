using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
namespace PwMApp.Pages;
public partial class ItemPage : ContentView
{
	public ItemPage()
	{
        Cash.Items = new List<Cash.Item> { new Cash.Item() { Name = "Первая" }, new Cash.Item() { Name = "Вторая" }, new Cash.Item() { Name = "Третья" } };
        InitializeComponent();
	}
    private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
    }
}
public class ItemsModel : ContentView
{
    public ICommand SearchClick { get; private set; }
    public VisSelectedItem _ListBoxSelectedItem { get; private set; }
    public ObservableCollection<Cash.Item> FindItems { get; private set; }
    public VisSelectedItem ActiveItem { get; private set; }
    public string SearchText { get; set; }
    public ItemsModel()
    {
        FindItems = new ObservableCollection<Cash.Item>();
        _ListBoxSelectedItem = new VisSelectedItem();
        ActiveItem =new VisSelectedItem();
        SearchClick = new Command(() =>
        {
            if (SearchText?.Length >0)
            {
                var fnd = Cash.Items.FindAll(x => x.Name.ToLower().Contains(SearchText.ToLower()));
                if (fnd != null)
                {
                    FindItems.Clear();
                    foreach (var item in fnd)
                    {
                        FindItems.Add(item);
                    }
                }
            }
        });
        _ListBoxSelectedItem.PropertyChanged += (s,e) => { ActiveItem.Item = _ListBoxSelectedItem.Item; };
    }
    public class VisSelectedItem : INotifyPropertyChanged 
    {
        private Cash.Item item;
        public Cash.Item Item
        {
            get { return item; }
            set { item = value; OnPropertyChanged(); }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
    }
