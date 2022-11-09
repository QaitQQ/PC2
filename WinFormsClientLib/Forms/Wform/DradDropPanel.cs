using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
namespace WinFormsClientLib.Forms.Wform
{
    public partial class DradDropPanel : UserControl
    {
        private List<Panel> panels;
        public DradDropPanel(int PanelCount = -1, Control[] controls = null, int FirstPanelSize = 600, Orientation Orientation = Orientation.Vertical)
        {
            this.Dock = DockStyle.Fill;
            if (controls != null)
            {
                panels = new List<Panel>();
                foreach (var item in controls)
                {
                    AddControl(item);
                }
                PanelCount = panels.Count - 1;
            }
            if (PanelCount != -1)
            {
                AddPanels(PanelCount, Orientation, FirstPanelSize);
            }
            InitializeComponent();
        }
        public void AddControl(Control item)
        {
            item.Dock = DockStyle.Fill;
            var panel = new Panel();
            panel.Controls.Add(item);
            panel.Dock = DockStyle.Fill;
            panel.BackColor = Color.White;
            panel.Padding = new Padding(0, 10, 0, 0);
            panel.MouseDown += new MouseEventHandler(DropUp_MouseDown);
            panels.Add(panel);
        }
        public void AddPanels(int PanelCount = 0, Orientation Orientation = Orientation.Horizontal, int FirstPanelSize = 200, Panel MainPanel = null)
        {
            Controls.Clear();
            for (int i = PanelCount; i >= 0; i--)
            {
                Panel panel = new Panel();
                panel.MouseDown += ContextMenuStrip_Clic;
                if (panels != null)
                {
                    panel.Controls.Add(this.panels[i]);
                }
                panel.DragOver += PanelDragOver;
                panel.DragDrop += PanelDragDrop;
                panel.AllowDrop = true;
                if (i == PanelCount)
                {
                    panel.BackColor = Color.Gray;
                    panel.Dock = DockStyle.Fill;
                }
                else
                {
                    var Splt = new Splitter();
                    if (Orientation == Orientation.Vertical)
                    {
                        panel.Dock = DockStyle.Left;
                        panel.Size = new Size(FirstPanelSize, 0);
                        Splt.Dock = DockStyle.Left;
                    }
                    else
                    {
                        panel.Dock = DockStyle.Top;
                        panel.Size = new Size(0, FirstPanelSize);
                        Splt.Dock = DockStyle.Top;
                    }
                    if (MainPanel == null)
                    {
                        Controls.Add(Splt);
                    }
                    else
                    {
                        MainPanel.Controls.Add(Splt);
                    }
                }
                if (MainPanel == null)
                {
                    Controls.Add(panel);
                }
                else
                {
                    MainPanel.Controls.Add(panel);
                }
            }
        }
        private void ContextMenuStrip_Clic(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var X = new ContextMenuStrip();
                X.Items.Add("Разделить Вертикально");
                X.Items[0].Click += (s, e) =>
                {
                    var pb = ((s as ToolStripMenuItem).Owner as ContextMenuStrip).SourceControl as Panel;
                    pb.Controls.Add(new DradDropPanel(3, new Control[] { new Panel(), new Panel() }, 200, Orientation.Vertical));
                };
                X.Items.Add("Разделить Горизонтально");
                X.Items[1].Click += (s, e) =>
                {
                    var pb = ((s as ToolStripMenuItem).Owner as ContextMenuStrip).SourceControl as Panel;
                    pb.Controls.Add(new DradDropPanel(3, new Control[] { new Panel(), new Panel() }, 200, Orientation.Horizontal));
                };
                ((Panel)sender).ContextMenuStrip = X;
            }
        }
        private void PanelDragDrop(System.Object sender, DragEventArgs e)
        {
            Control c = e.Data.GetData(e.Data.GetFormats()[0]) as Control;
            Panel panel = sender as Panel;
            if (c != null)
            {
                panel.Controls.Clear();
                c.Location = panel.PointToClient(new Point(e.X, e.Y));
                c.Dock = DockStyle.Fill;
                c.Width = panel.Width;
                panel.Controls.Add(c);
            }
        }
        private void PanelDragOver(System.Object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }
        private void DropUp_MouseDown(object sender, MouseEventArgs e)
        {
            Control c = sender as Control;
            c.DoDragDrop(c, DragDropEffects.Move);
        }
    }
}
