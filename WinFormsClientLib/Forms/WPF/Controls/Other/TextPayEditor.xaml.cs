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
    /// Логика взаимодействия для TextPayEditor.xaml
    /// </summary>
    public partial class TextPayEditor : UserControl
    {
        public TextPayEditor()
        {
            InitializeComponent();
            TextBox_1.TextChanged += (s, a) =>
              {
                  TextBox_2.Document = new FlowDocument();

                  string X = (new TextRange(TextBox_1.Document.ContentStart, TextBox_1.Document.ContentEnd)).Text;

                  X = X.Replace('\t',' ');
                  X = X.Replace("\n", "");
                  X = X.Replace("\r", ", " );
                  X = X.Replace("  ", " ");
                  TextBox_2.AppendText(X);
              };
            ReplceButton.Click += (s, a) =>
            {
                string X = (new TextRange(TextBox_2.Document.ContentStart, TextBox_2.Document.ContentEnd)).Text;
                X= X.Replace(FindText.Text, PasteText.Text);
                TextBox_2.Document = new FlowDocument();
                TextBox_2.AppendText(X);

            };
        }








    }
}
