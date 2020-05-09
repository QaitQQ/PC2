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

namespace WinFormsClientLib.Forms
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    /// 


    


    public partial class СommonDictionaryControl : UserControl
    {
        public List<DictionaryControl> control_1s { get; set; }
        public СommonDictionaryControl()
        {
            
            InitializeComponent();
            control_1s = new List<DictionaryControl>();
            ItemsControl_1.ItemsSource = control_1s;

            control_1s.Add(new DictionaryControl());
            control_1s.Add(new DictionaryControl());
            control_1s.Add(new DictionaryControl());

        }
    }
}
