using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace MGSol.Panel.Other
{
    /// <summary>
    /// Логика взаимодействия для QuestionBox.xaml
    /// </summary>
    public partial class QuestionBox : Window
    {
        public string AnswerTEXT { get; set; }

        public QuestionBox(string Question)
        {
            InitializeComponent();
            QuestionTEXTBox.Text = Question;
        
        
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void AnswerTEXTBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            AnswerTEXT = AnswerTEXTBox.Text;
        }
    }
}
