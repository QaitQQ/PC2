using System;
using System.Windows.Forms;

using WindowsFormsClientLibrary.Forms;

using WinFormsClientLib.Forms;
using WinFormsClientLib.Forms.WPF.Controls.CRMControls;
using WinFormsClientLib.Forms.WPF.Controls.ItemControls;
using WinFormsClientLib.Forms.WPF.ItemControls;

namespace Client.Forms
{

    public partial class Mainform : Form
    {
        private WebBrowser webBrowser;
        public Form CRMForm { get; private set; }
        public Form itemForm { get; private set; }
        public Mainform()
        {
            InitializeComponent();
            CRMForm = new UniversalWPFContainerForm(new MainCRMControl());
            itemForm = new UniversalWPFContainerForm(new MainItemControl());
            CRMForm.MdiParent = this;
            webBrowser = new WebBrowser();
            var Win = new System.Windows.Forms.ToolStripMenuItem() { Text = "Новое окно" };
            Win.Click += new EventHandler(Win_Shown);
            WindowToolStripMenuItem.DropDownItems.Add(Win);
            this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            webBrowser.ScriptErrorsSuppressed = true;
            panel1.Visible = false;
            
            this.webBrowser.TabIndex = 0;
            panel1.Controls.Add(webBrowser);
            this.webBrowser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.WebBrowser_DocumentCompleted);
        }
        private void FormActivator(Form Form)
        {
            if (Form.Visible)
            {
                Form.Hide();
            }
            else
            {
                Form.Show();
                Form.Activate();

            }
        }
        private void CRMToolStripMenuItem_Click(object sender, EventArgs e) => FormActivator(CRMForm);
        private void ItemToolStripMenuItem_Click(object sender, EventArgs e) => FormActivator(itemForm);
        private void NetToolStripMenuItem_Click(object sender, EventArgs e) => new Config();
        private void DictionariesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new DictionaryConfig();
        }
        private void ClientConfigMenuItem_Click(object sender, EventArgs e)
        {
            new Config();
        }
        private void Mainform_Shown(object sender, EventArgs e)
        {
            var XHeight = (int)(Height * 0.935);
            var XWidth = (int)(Width * 0.99);

            CRMForm.Height = (int)(XHeight * 0.33);
            CRMForm.Location = new System.Drawing.Point(0, (int)(XHeight * 0.66));
            itemForm.MdiParent = this;
            itemForm.Height = (int)(XHeight * 0.66);
            itemForm.Location = new System.Drawing.Point(0, 0);
            itemForm.Width = XWidth;
            CRMForm.Width = XWidth;

            FormActivator(CRMForm);
            FormActivator(itemForm);
        }
        private void Win_Shown(object sender, EventArgs e)
        {
            FormActivator(new UniversalWPFContainerForm(new ItemComparer()) { CanClosed = true, MdiParent = this });
        }
        private void ShowWebBrowser(object sender, EventArgs e)
        {
            panel1.Width = this.Width/2;
            if (panel1.Visible == true)
            {
                panel1.Visible = false;
            }
            else
            {
                panel1.Visible = true;
            }
        }
        private void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            webBrowser.Document.MouseOver += new HtmlElementEventHandler(Document_MouseOver);
            webBrowser.Document.MouseLeave += new HtmlElementEventHandler(Document_MouseLeave);
        }
        private void Document_MouseLeave(object sender, HtmlElementEventArgs e) => e.FromElement.Style = null;
        private void Document_MouseOver(object sender, HtmlElementEventArgs e)
        {
            if (e.ToElement != null & e.ToElement.Style == null)
            {
                e.ToElement.Style = "border: 2px solid red;";

           //     TechField.Text = e.ToElement.OuterHtml;

            }
        }
        public void GoSite(string Url)
        {
            webBrowser.Navigate(Url);
        }
    }
}
