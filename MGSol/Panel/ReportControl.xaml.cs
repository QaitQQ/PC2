using System;
using System.Collections.Generic;
using System.IO;
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

namespace MGSol.Panel
{
    /// <summary>
    /// Логика взаимодействия для ReportControl.xaml
    /// </summary>
    public partial class ReportControl : UserControl
    {
        private MainModel mModel;

        public ReportControl(MainModel mModel)
        {
            InitializeComponent();
            this.mModel = mModel;
            string y = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo dir = new DirectoryInfo(y+@"/Report");
            foreach (FileInfo file in dir.GetFiles())
            {
                if (file.Name.Contains("xlsx"))
                {
                    FileList.Items.Add(file.FullName);
                }            
            }
        }
    }
}
