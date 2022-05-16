using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MGSol.Panel
{
    /// <summary>
    /// Логика взаимодействия для ModalBox.xaml
    /// </summary>
    public partial class ModalBox : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public string _STR { get; set; }


        public string STR
        {
            get { return _STR; }
            set
            {
                if (value != _STR)
                {
                    _STR = value;
                    OnPropertyChanged("STR");
                }
            }
        }
        public ModalBox()
        {
            InitializeComponent();
   

        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();        
        }

        private void STRTexBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            STR = ((TextBox)sender).Text;
        }
    }
}
