using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

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
                    item.Dock = DockStyle.Fill;
                    var panel = new Panel();
                    panel.Controls.Add(item);
                    panel.Dock = DockStyle.Fill;
                    panel.BackColor = Color.White;
                    panel.Padding = new Padding(0, 10, 0, 0);
                    panel.MouseDown += new MouseEventHandler(DropUp_MouseDown);
                    panels.Add(panel);
                }

                PanelCount = panels.Count - 1;
            }
            if (PanelCount != -1)
            {
                AddPanels(PanelCount, Orientation, FirstPanelSize);
            }
            InitializeComponent();
        }
        private void AddPanels(int PanelCount, Orientation Orientation, int FirstPanelSize)
        {
            for (int i = PanelCount; i >= 0; i--)
            {
                Panel panel = new Panel();
                panel.MouseDown += ContextMenuStrip_Clic;

                if (panels != null)
                {
                    panel.Controls.Add (panels[i]);
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
                    Controls.Add(Splt);
                }
                Controls.Add(panel);
            }
        }

        private void ContextMenuStrip_Clic(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var X = new ContextMenuStrip();

                X.Items.Add("1");
                X.Items.Add("2");

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
