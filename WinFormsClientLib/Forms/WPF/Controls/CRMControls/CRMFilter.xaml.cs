using System;
using System.Collections.Generic;
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

namespace WinFormsClientLib.Forms.WPF.Controls.CRMControls
{
    /// <summary>
    /// Логика взаимодействия для CRMFilter.xaml
    /// </summary>
    public partial class CRMFilter : UserControl
    {

        public string FilterType { get; set; }
        public string FilterValue { get; set; }
        private MainCRMControl _mainCRMControl;

        public CRMFilter(string FilterType, string FilterValue, MainCRMControl mainCRMControl)
        {
            this.FilterType = FilterType; this.FilterValue = FilterValue;
            _mainCRMControl = mainCRMControl;
            InitializeComponent();
            FilterTypeTextBlock.Text = FilterType;
            FilterValueTextBlock.Text = FilterValue;
        }

        private void DelButton_Click(object sender, RoutedEventArgs e)
        {
            _mainCRMControl.Filters.Remove(this);
        }
    }
}
