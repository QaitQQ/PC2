using Object_Description;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

using WinFormsClientLib.Forms.Controls;

namespace WinFormsClientLib.Forms
{
    /// <summary>
    /// Логика взаимодействия для UserControl2.xaml
    /// </summary>
    /// 
    

   
    public partial class DictionaryControl : UserControl
    {


        public IDictionaryPC Dictionary_active { get; set; }
        public List<KeyValuePair<FillDefinitionPrice, string>> FillDefinitionList { get; set; }
        public DictionaryControl()
        {
            InitializeComponent();
            ComboBox_1.ItemsSource = Enum.GetValues(typeof(DictionaryRelate)).Cast<DictionaryRelate>();
            Dictionary_active = new DictionaryPrice("Hi", DictionaryRelate.None);
            Dictionary_active.Values.Add("1");
            Dictionary_active.Values.Add("2");
            Dictionary_active.Values.Add("3");



            ((DictionaryPrice)Dictionary_active).Filling_method_string.Add(new KeyValuePair<FillDefinitionPrice, string>(FillDefinitionPrice.Name, "Имя"));
            ((DictionaryPrice)Dictionary_active).Filling_method_string.Add(new KeyValuePair<FillDefinitionPrice, string>(FillDefinitionPrice.PriceRC, "Розница"));

            DataContext = Dictionary_active;
            FillDefinitionList = (Dictionary_active as DictionaryPrice).Filling_method_string;

            if ((Dictionary_active is DictionaryPrice) || (Dictionary_active is DictionaryStorage))
            {
                FillingMethodStackPanelButton.Visibility = Visibility.Visible;
            }

            //if (Dictionary_active is DictionaryPrice price)
            //{
            //    foreach (var item in price.Filling_method_string)
            //    {
            //        FillingMethodStackPanel.Children.Add(new KeyValueControl(item));
            //    }
            //}   
            
        }

        private void ValuesStackPanel_Click(object sender, RoutedEventArgs e)
        {
            if (ValuesStackPanel.Visibility == Visibility.Collapsed)
            {
                ValuesStackPanel.Visibility = Visibility.Visible;
            }
            else
            {
                ValuesStackPanel.Visibility = Visibility.Collapsed;
            }        
        }

        private void HideCommonStackPanel_Click(object sender, RoutedEventArgs e)
        {
            if (CommonStackPanel.Visibility == Visibility.Collapsed)
            {
                CommonStackPanel.Visibility = Visibility.Visible;
            }
            else
            {
                CommonStackPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void FillingMethodStackPanel_Click(object sender, RoutedEventArgs e)
        {
            if (FillingMethodStackPanel.Visibility == Visibility.Collapsed)
            {
                FillingMethodStackPanel.Visibility = Visibility.Visible;
            }
            else
            {
                FillingMethodStackPanel.Visibility = Visibility.Collapsed;
            }
        }
    }
}
