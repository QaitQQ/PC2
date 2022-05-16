using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MGSol.Panel
{
    /// <summary>
    /// Логика взаимодействия для OptionControl.xaml
    /// </summary>
    public partial class OptionControl : UserControl
    {
        MainModel _model { get; set; }
        ObservableCollection<StructLibCore.Marketplace.APISetting> ListOption { get; set; }

        public OptionControl(MainModel model)
        {
            InitializeComponent();

            _model = model;

            ListOption = new ObservableCollection<StructLibCore.Marketplace.APISetting>();

            foreach (var item in _model.Option.APISettings)
            {
                ListOption.Add(item);
            }
            ListOption.CollectionChanged += (x, y) =>
            {
                var lst = new List<StructLibCore.Marketplace.APISetting>();
                foreach (var item in ListOption)
                {
                    lst.Add(item);
                }
                _model.Option.APISettings = lst;
                _model.Option = _model.Option;
            };

            OptionList.ItemsSource = ListOption;
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var OptionAddBox = new OptionAddBox();
            if ((bool)OptionAddBox.ShowDialog())
            {
                ListOption.Add(OptionAddBox.Setting);
            }
        }
        private void DelButtonClick_Click(object sender, RoutedEventArgs e)
        {
            ListOption.Remove((StructLibCore.Marketplace.APISetting)((Button)sender).DataContext);
        }
        private void Grid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Grid grid = (Grid)sender;
            var OptionAddBox = new OptionAddBox((StructLibCore.Marketplace.APISetting)grid.DataContext);


            if ((bool)OptionAddBox.ShowDialog())
            {
                ListOption.Add(OptionAddBox.Setting);
            }
        }
    }
}
