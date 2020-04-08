
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsClientLibrary.Forms
{
    public partial class Okno : Form
    {


        public Okno ( )
        {
            InitializeComponent();

            elementHost1.Child = new UserControl1();

        }

        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }
    }


    public partial class ICQ
    {

        //[DllImport("user32.dll", SetLastError = true)]
        //private static IntPtr SetParent ( IntPtr hWndChild, IntPtr hWndNewParent ) { }
        //[DllImport("user32.dll", SetLastError = true)]
        //private static IntPtr FindWindow ( IntPtr ZeroOnly, string lpWindowName ) { }

        //private void ICQ_Activated ( object sender, EventArgs e )
        //{
        //    Process p = Process.Start("calc.exe");
        //    Thread.Sleep(500);
        //    // SetParent(p.MainWindowHandle, this.Handle);
        //    //  this.WindowState = FormWindowState.Maximized;

        //}


    }
}
