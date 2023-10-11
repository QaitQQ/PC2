
using Pwa.Pages;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
namespace Pwa
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

        private void MenuButton_Clicked(object sender, EventArgs e)
        {
            if (MenuColum.Width == 0)
            {
                MenuColum.Width = 300;
            }
            else
            {
                MenuColum.Width = 0;
            }
        }   
    }
    public partial class ManuModel : ContentView
    {
        public string Title { get; set; }
        public object PageActivate { get; set; }
        public object ContentView { get; set; }
        public ChangesObject<GridLength> MenuColumWidth { get; set; }
        public ObservableCollection<VisContentView> Pages { get; private set; }
        public VisContentView ActiveContentView { get; private set; }
        public ManuModel()
        {
            MenuColumWidth = new ChangesObject<GridLength>(new GridLength()); 
            Title = "ManuModel";
            Pages = [];
            ActiveContentView = new VisContentView();


            Pages.Add(new VisContentView() { ContentView = new ItemPage(), Title = "Позиции" });
            Pages.Add(new VisContentView() { ContentView = new OptionsPage(), Title = "Настройки" });
            foreach (var item in Pages)
            {
                item.ButtonClick += (cv) => {
                    ActiveContentView.ContentView = cv; MenuColumWidth.Object = 0; 
                };
            }
        }

        public class ChangesObject<T>(object @object) : INotifyPropertyChanged 
        {
            public event PropertyChangedEventHandler PropertyChanged;
            private object _object = @object;

            public object Object
            {
                get { return _object; }
                set { _object = value; OnPropertyChanged(); }
            }
            public void OnPropertyChanged([CallerMemberName] string name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
