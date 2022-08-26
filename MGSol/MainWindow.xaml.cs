using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
            AddTab(new Panel.ReturnControl(MModel), "Возвраты");
            AddTab(new Panel.ReportControl(MModel), "Отчеты");
            AddTab(new Panel.OptionControl(MModel), "Настройки");
        }
        private void AddTab(UserControl control, string Header)
        {
            MModel.Tabs.Add(new TabItem() { Content = control, Header = new TextBlock() { Text = Header }, LayoutTransform = new RotateTransform { Angle = -90 } });
        }
        public MainModel MModel { get => _mModel; set => _mModel = value; }
        private void Heading_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (WindowState == System.Windows.WindowState.Normal)
                {
                    WindowState = System.Windows.WindowState.Maximized;
                }
                else
                {
                    WindowState = System.Windows.WindowState.Normal;
                }
            }
            else
            {
                DragMove();
            }
        }
        private void Сlose_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        bool isWiden = false;
        private void window_initiateWiden(object sender, System.Windows.Input.MouseEventArgs e)
        {
            isWiden = true;
        }
        private void window_endWiden(object sender, System.Windows.Input.MouseEventArgs e)
        {
            isWiden = false;

            // Make sure capture is released.
            Border rect = (Border)sender;
            rect.ReleaseMouseCapture();
        }

        private void window_Widen(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Border rect = (Border)sender;
            if (isWiden)
            {
                rect.CaptureMouse();
                double newWidth = e.GetPosition(this).X + 5;
                if (newWidth > 0) this.Width = newWidth;
            }
        }

        private void titleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }







    }
}
