using System.Drawing;
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
            AddTab(new Panel.OrdersControl(MModel), "Заказы");
            AddTab(new Panel.ItemControl(MModel), "Товары");
            AddTab(new Panel.ReportControl(MModel), "Отчеты");
            AddTab(new Panel.OptionControl(MModel), "Настройки");
        }

        private void AddTab(UserControl control, string Header)
        {
            MModel.Tabs.Add(new TabItem() { Content = control, Header = new TextBlock() { Text = Header }, LayoutTransform = new RotateTransform { Angle = -90 } });
        }


        public MainModel MModel { get => _mModel; set => _mModel = value; }
    }
}
