using PwMApp.Pages;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
namespace PwMApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        private void SwipeGestureRecognizerRight_Swiped(object sender, SwipedEventArgs e)
        {
            MenuColum.Width = 300;
        }
        private void SwipeGestureRecognizerLeft_Swiped(object sender, SwipedEventArgs e)
        {
            MenuColum.Width = 0;
        }
    }
    public class ManuModel : ContentView
    {
        public int MenuColumWidth { get; private set; }
        public ObservableCollection<VisContentView> Pages { get; private set; }
        public VisContentView ActiveContentView { get; private set; }
        public ManuModel()
        {
            Pages = new ObservableCollection<VisContentView>();
            ActiveContentView = new VisContentView();
            Pages.Add(new VisContentView() { ContentView = new ItemPage(), Title = "Позиции" });
            Pages.Add(new VisContentView() { ContentView = new OptionsPage(), Title = "Настройки" });
            foreach (var item in Pages)
            {
                item.ButtonClick += (cv) => { ActiveContentView.ContentView = cv; };
            }
        }
        public class VisContentView : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            public event Action<ContentView> ButtonClick;
            private ContentView contentView;
            public VisContentView()
            {
                PageActivate = new Command(() => { ButtonClick(contentView); });
            }
            public ContentView ContentView
            {
                get { return contentView; }
                set { contentView = value; OnPropertyChanged(); }
            }
            public string Title { get; set; }
            public ICommand PageActivate { get; private set; }
            public void OnPropertyChanged([CallerMemberName] string name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
