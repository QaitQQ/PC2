using Object_Description;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace WinFormsClientLib.Forms.Controls
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class KeyValueControl : UserControl
    {
        KeyValuePair<FillDefinitionPrice, string> ValuePair { get; set; }

        public KeyValueControl(KeyValuePair<FillDefinitionPrice, string> valuePair)
        {

            InitializeComponent();
            ValuePair = valuePair;
            CommonGrid.DataContext = ValuePair;
            ComboRelateBox.ItemsSource = Enum.GetValues(typeof(FillDefinitionPrice)).Cast<FillDefinitionPrice>();

        }
    }
}
