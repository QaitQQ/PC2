using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WinFormsClientLib.Forms.WPF.Controls.ItemControls
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class MarketControl : UserControl
    {
        private ObservableCollection<object> Items;
        public MarketControl()
        {
            InitializeComponent();

            Items = new ObservableCollection<object>();

            Tabs.ItemsSource = Items;

            var X = new MarketRenewer();

            Items.Add(new TabItem() {Header = X.MarketName, Content = X } );
        }
    }
}
