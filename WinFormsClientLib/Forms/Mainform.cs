using System;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

using WindowsFormsClientLibrary.Forms;

using WinFormsClientLib.Forms;
using WinFormsClientLib.Forms.Wform;
using WinFormsClientLib.Forms.WPF.Controls;
using WinFormsClientLib.Forms.WPF.Controls.CRMControls;
using WinFormsClientLib.Forms.WPF.Controls.DictionaryControls;
using WinFormsClientLib.Forms.WPF.Controls.ItemControls;
using WinFormsClientLib.Forms.WPF.Controls.ItemControls.Market;
using WinFormsClientLib.Forms.WPF.Controls.Other;
using WinFormsClientLib.Forms.WPF.ItemControls;
namespace Client.Forms
{
    public partial class Mainform : Form
    {
        private WebBrowser webBrowser;
        public ElementHost CRMForm { get; private set; }
        public ElementHost ItemForm { get; private set; }
        public Mainform()
        {
            Controls.Add(new DradDropPanel
                 ( FirstPanelSize:1200,
                 controls: new Control[]
                 {
                    new ElementHost() { Child = new MainItemControl() },
                    new ElementHost() { Child = new MainCRMControl() },
                 },
                 Orientation: Orientation.Vertical
                 )
                 );
            InitializeComponent();
            LeftPanel.Controls.Add(new DradDropPanel(
                controls: new Control[]
                {
                 new ElementHost() { Child = new TextPayEditor()  },
                 new ElementHost() {Child = new WinFormsClientLib.Forms.WPF.Controls.Target.TargetPanel()}
                }
                ));
            AddNewForm("Сравнение", new EventHandler((x, r) => { FormActivator(new UniversalWPFContainerForm(new ItemComparer()) { CanClosed = true }); }), видToolStripMenuItem);
            AddNewForm("Номенклатура", new EventHandler((x, r) => { FormActivator(new UniversalWPFContainerForm(new MainItemControl()) { CanClosed = true }); }), видToolStripMenuItem);
            AddNewForm("Работа с прайсами", new EventHandler((x, r) => { FormActivator(new UniversalWPFContainerForm(new PriceServiceControl()) { CanClosed = true }); }), видToolStripMenuItem);
            AddNewForm("Работа с таблицами", new EventHandler(NewForm), видToolStripMenuItem);
            AddNewForm("Работа со словорями", new EventHandler((x, r) => { FormActivator(new UniversalWPFContainerForm(new DictionaryControl()) { CanClosed = true }); }), видToolStripMenuItem);
            AddNewForm("Дочернее окно", new EventHandler(ContainerForm), файлToolStripMenuItem);
            AddNewForm("Завершить сервер", new EventHandler(CloseServer), файлToolStripMenuItem);
            AddNewForm("Работа с маркетами", new EventHandler(MarketControl), видToolStripMenuItem);
            AddNewForm("Партнеры", new EventHandler((x, r) => { FormActivator(new UniversalWPFContainerForm(new MainCRMControl()) { CanClosed = true }); }), видToolStripMenuItem);
            RightPanel.Visible = false;
            RightPanelSplitter.Visible = false;
            LeftPanel.Visible = false;
            LeftPanelSplitter.Visible = false;
        }
        private static void CloseServer(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }
        private static void AddNewForm(string Text, EventHandler ClikEvent, ToolStripMenuItem toolStrip)
        {
            ToolStripMenuItem Win = new System.Windows.Forms.ToolStripMenuItem() { Text = Text };
            Win.Click += ClikEvent;
            toolStrip.DropDownItems.Add(Win);
        }
        private void FormActivator(Form Form)
        {
            Control.ControlCollection X = Controls;
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
        private void NetToolStripMenuItem_Click(object sender, EventArgs e) => new Config();
        private void DictionariesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new DictionaryConfig();
        }
        private void MarketControl(object sender, EventArgs e)
        {
            FormActivator(new UniversalWPFContainerForm(new MainMarketPlaceControl()) { CanClosed = true });
        }
        private void ShowWebBrowser(object sender, EventArgs e)
        {
            if (RightPanel.Visible == true)
            {
                RightPanelSplitter.Visible = false;
                RightPanel.Visible = false;
                if (webBrowser != null)
                {
                    this.webBrowser.DocumentCompleted -= new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.WebBrowser_DocumentCompleted);
                    RightPanel.Controls.Remove(webBrowser);
                    webBrowser.Dispose();
                    ((WebB)webBrowser).Disp();
                    GC.SuppressFinalize(webBrowser);
                }
            }
            else
            {
                RightPanel.Width = this.Width / 2;
                RightPanelSplitter.Visible = true;
                RightPanel.Visible = true;
            }
        }
        private void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            webBrowser.Document.MouseOver += new HtmlElementEventHandler(Document_MouseOver);
            webBrowser.Document.MouseLeave += new HtmlElementEventHandler(Document_MouseLeave);
        }
        private void Document_MouseLeave(object sender, HtmlElementEventArgs e) => e.FromElement.Style = null;
        private static void Document_MouseOver(object sender, HtmlElementEventArgs e)
        {
            if (e.ToElement != null & e.ToElement.Style == null)
            {
                e.ToElement.Style = "border: 2px solid red;";
                //     TechField.Text = e.ToElement.OuterHtml;
            }
        }
        public void GoSite(string Url)
        {
            if (!RightPanel.Visible)
            {
                RenewWebBrowser();
                RightPanel.Width = this.Width / 2;
                RightPanelSplitter.Visible = true;
                RightPanel.Visible = true;
                Thread.Sleep(100);
            }
            if (webBrowser == null)
            {
                webBrowser = new WebBrowser();
            }
            webBrowser.Navigate(Url);
        }
        private void ShowLeftPanelButton_Click(object sender, EventArgs e)
        {
            LeftPanel.Width = this.Width / 2;
            if (LeftPanel.Visible == true)
            {
                LeftPanelSplitter.Visible = false;
                LeftPanel.Visible = false;
            }
            else
            {
                LeftPanelSplitter.Visible = true;
                LeftPanel.Visible = true;
            }
        }
        private void NewForm(object sender, EventArgs e)
        {
            try
            {
                FormActivator(new UniversalWPFContainerForm(new UniversalDBItemFormControl()) { CanClosed = true });
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message);
            }
        }
        private void RenewWebBrowser()
        {
            WebB WB = new();
            webBrowser = WB;
            this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            webBrowser.ScriptErrorsSuppressed = true;
            this.webBrowser.TabIndex = 0;
            RightPanel.Controls.Add(webBrowser);
            this.webBrowser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.WebBrowser_DocumentCompleted);
        }
        private static void ContainerForm(object sender, EventArgs e) { new ContainerForm().Show(); }
    }
}
