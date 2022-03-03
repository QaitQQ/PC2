using System;
using System.Drawing;
using System.Windows.Forms;

namespace Server
{
    public class Ico
    {
        private void Close(object sender, EventArgs e) { System.Environment.Exit(0); }
        public Ico()
        {
            NotifyIcon icon = new NotifyIcon();
           // icon.MouseClick += Menustrip;

            icon.MouseDown += new MouseEventHandler(Close);
            icon.Icon = new Icon("134.ico");
            icon.Visible = true;
        }

        private void Menustrip(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var X = new MenuStrip();
                X.Items.Add("Exit", null, Close);
                X.Show();
            }
        }
    }
}
