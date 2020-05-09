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

namespace WinFormsClientLib.Forms.WPF.Controls.Other
{
    /// <summary>
    /// Логика взаимодействия для PercentCalc.xaml
    /// </summary>
    public partial class PercentCalc : UserControl
    {
        public PercentCalc()
        {
            InitializeComponent();
        }

        private void Plus_Click(object sender, RoutedEventArgs e) { if (SumBox.Text != "") { TotalBox.Text = (Convert.ToDouble(SumBox.Text) * (1 + Convert.ToDouble(PercentBox.Text) / 100)).ToString(); } }
        private void Minus_Click(object sender, RoutedEventArgs e) { if (SumBox.Text != "") { TotalBox.Text = (Convert.ToDouble(SumBox.Text) * (1 - Convert.ToDouble(PercentBox.Text) / 100)).ToString(); } }
    }
}
