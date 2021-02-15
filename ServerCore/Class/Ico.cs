using System.Drawing;
using System.Windows.Forms;

namespace Server
{
    public class Ico
    {
        private void Close(object sender, MouseEventArgs e) { System.Environment.Exit(0); }
        public Ico()
        {
            NotifyIcon icon = new NotifyIcon();
            icon.MouseDown += new MouseEventHandler(Close);
            icon.Icon = new Icon("134.ico");
            icon.Visible = true;
        }
    }
}
