using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using WinFormsClientLib.Forms.Wform;
namespace WinFormsClientLib.Forms
{
    public partial class UniversalWPFContainerForm : Form
    {
        public UIElement Control { get; private set; }
        public bool _CanClosed;
        public bool CanClosed { get { return _CanClosed; } set { _CanClosed = value; CloseButton.Visible = true; } }
        public UniversalWPFContainerForm(System.Windows.Controls.UserControl control)
        {
            InitializeComponent();
            Control = control;
            this.panel1.Controls.Add(new DradDropPanel
                 (
                 controls: new System.Windows.Forms.Control[]
                 {
                    new System.Windows.Forms.Integration.ElementHost() { Child = Control },
                 },
                 Orientation: System.Windows.Forms.Orientation.Vertical
                 ));
            if (Control is System.Windows.Controls.UserControl)
            {
                ((System.Windows.Controls.UserControl)Control).DataContext = this;
            }
            this.splitContainer1.SplitterDistance = 10;
            CloseButton.Visible = false;
        }
        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {
            panel2.Capture = false;
            Message m = Message.Create(base.Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
            WndProc(ref m);
        }
        private void panel2_MouseDoubleClick(object sender, MouseEventArgs e) { if (this.WindowState == FormWindowState.Maximized) { WindowState = FormWindowState.Normal; } else { WindowState = FormWindowState.Maximized; } }
        private void CloseButton_Click(object sender, EventArgs e) { Close(); }
    }
}
