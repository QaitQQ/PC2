using Server.Class.HDDClass;
using StructLibCore.Marketplace;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MGSol
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainModel _mModel;
        public MainWindow()
        {
            InitializeComponent();
            MModel = new MainModel();
            MainTabControl.ItemsSource = MModel.Tabs;
            MModel.Tabs.Add(new TabItem() { Content = new Panel.OrdersControl(MModel), Header = new TextBlock() {Text = "Заказы"/*, Background =  new SolidColorBrush(System.Windows.Media.Color.FromRgb(102,204,0))*/}, LayoutTransform = new RotateTransform { Angle = -90 } });
            MModel.Tabs.Add(new TabItem() { Content = new Panel.ItemControl(MModel), Header = new TextBlock() { Text = "Товары" }, LayoutTransform = new RotateTransform { Angle = -90 } });
            MModel.Tabs.Add(new TabItem() { Content = new Panel.OptionControl(MModel), Header = new TextBlock() { Text = "Настройки" }, LayoutTransform = new RotateTransform { Angle = -90 } });
            MModel.Tabs.Add(new TabItem() { Content = new Panel.ReportControl(MModel), Header = new TextBlock() { Text = "Отчеты" }, LayoutTransform = new RotateTransform { Angle = -90 } });

        }

        public MainModel MModel { get => _mModel; set => _mModel = value; }
    }

    public class MainModel 
    {
        private event Action<string, object> СhangeList;

        private MarketPlaceCash options;
        public MarketPlaceCash Option
        {
            get { return options; }
            set { options = value; СhangeList?.Invoke("Option.bin", options); }
        }
        internal ObservableCollection<Control> Tabs { get; set; }
        public MainModel() 
        { 
            Tabs = new ObservableCollection<Control>();
            options = new MarketPlaceCash();
            LoadFromFile(ref options, "Option.bin");
            СhangeList += Serializer.Doit;
        }
        private Serializer<object> Serializer = new Serializer<object>();
        private void LoadFromFile<T>(ref T Object, string Path)
        {
            T Obj = Task.Run(() => new Deserializer<T>(Path).Doit()).Result;
            if (Obj != null)
            {
                Object = Obj;
            }
        }
    }
}
