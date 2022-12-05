using System.Windows.Forms;
namespace Client.Forms
{
    public partial class Mainform
    {
        class WebB : WebBrowser
        {
            public void Disp() { Dispose(true); }
            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);
            }
        }
    }
}
