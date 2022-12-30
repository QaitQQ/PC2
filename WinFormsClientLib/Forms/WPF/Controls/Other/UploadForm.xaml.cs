using Client;

using Network.Item;

using System;
using System.Collections.Generic;
using System.Drawing;
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

namespace WinFormsClientLib.Forms.WPF.Controls.Other
{
    /// <summary>
    /// Логика взаимодействия для UploadForm.xaml
    /// </summary>
    public partial class UploadForm : UserControl
    {
        public UploadForm()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var t = new UploadItemComparerToSite().Get<bool>(new WrapNetClient());

            if (t)
            {
                ((Button)sender).Background = new SolidColorBrush(new System.Windows.Media.Color() { A = System.Drawing.Color.Green.A, B = System.Drawing.Color.Green.B, G = System.Drawing.Color.Green.G });
            }
        }
    }
}
