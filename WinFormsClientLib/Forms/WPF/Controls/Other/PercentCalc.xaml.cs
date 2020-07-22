using System;
using System.Windows;
using System.Windows.Controls;

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

        private void Plus_Click(object sender, RoutedEventArgs e) { if (SumBox.Text != "") { TotalBox.Text = (ConvertToDouble(SumBox.Text) * (1 + ConvertToDouble(PercentBox.Text) / 100)).ToString(); } }
        private void Minus_Click(object sender, RoutedEventArgs e) { if (SumBox.Text != "") { TotalBox.Text = (ConvertToDouble(SumBox.Text) * (1 - ConvertToDouble(PercentBox.Text) / 100)).ToString(); } }
        private double ConvertToDouble(string STR)
        {
            string newStr = null;

            foreach (char item in STR)
            {
                if (char.IsDigit(item) || item == '.' || item == ',')
                {
                    if (item == '.')
                    {
                        newStr += ',';
                    }
                    else
                    {
                        newStr += item;
                    }
                }
            }

            return Convert.ToDouble(newStr);
        }
    }



}
