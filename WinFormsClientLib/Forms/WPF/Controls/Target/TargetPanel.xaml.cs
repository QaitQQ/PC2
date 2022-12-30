using Client;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;


namespace WinFormsClientLib.Forms.WPF.Controls.Target
{
    public partial class TargetPanel : UserControl
    {

        private List<Server.Target> Targets { get; set; }


        public TargetPanel()
        {
            InitializeComponent();
            RenewTargets();
            TargetList.ItemsSource = Targets;
        }


        private void RenewTargets() { Targets = new Network.Target.GetTarget().Get<List<Server.Target>>(new WrapNetClient()); }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RenewTargets();
            TargetList.ItemsSource = Targets;
        }
    }
}
