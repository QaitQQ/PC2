using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
namespace Pwa
{
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
