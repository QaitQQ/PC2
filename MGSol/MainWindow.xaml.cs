using System.Threading.Tasks.Dataflow;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
namespace MGSol
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MModel = new MainModel();
            MainTabControl.ItemsSource = MModel.Tabs;
            AddTab(new Panel.OrdersControl(MModel), "Заказы");
            Panel.ItemControl Items = new Panel.ItemControl(MModel);
            Items.BeginInit();
            AddTab(Items, "Товары");
           // AddTab(new Panel.ReturnControl(MModel), "Возвраты");
            AddTab(new Panel.ReportControl(MModel), "Отчеты");
            AddTab(new Panel.ShipmentListControl(MModel), "Отгрузки");
            AddTab(new Panel.OptionControl(MModel), "Настройки");
            AddTab(new Panel.PromoControl(MModel), "Акции");
            VirtualizingStackPanel.SetIsVirtualizing(Items, true);
        }
        private void AddTab(System.Windows.Controls.UserControl control, string Header)
        {
            MModel.Tabs.Add(new TabItem() { Content = control, Header = new TextBlock() { Text = Header }, LayoutTransform = new RotateTransform { Angle = -90 } });
        }
        public MainModel MModel { get; set; }
        private void Heading_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {

                if (this.WindowState == WindowState.Maximized)
                {
                    this.WindowState = WindowState.Normal;
                    ((Grid)sender).Height = 18;
                }
                else
                {
                    System.Drawing.Rectangle r = Screen.GetWorkingArea(new System.Drawing.Point((int)this.Left, (int)this.Top));
                    this.Left = 8;
                    this.Top = 10;
                    this.MaxWidth = r.Width +7 ;
                    this.MaxHeight = r.Height +7;
                    this.WindowState = WindowState.Maximized;

                    ((Grid)sender).Height = 25;
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
        private bool isWiden = false;
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
                if (newWidth > 0)
                {
                    Width = newWidth;
                }
            }
        }
        private void titleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
    }
}
