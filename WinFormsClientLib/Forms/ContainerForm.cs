using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using WinFormsClientLib.Forms.Wform;

namespace WinFormsClientLib.Forms
{
    public partial class ContainerForm : Form
    {

        public ContainerForm()
        {
            InitializeComponent();
            this.Controls.Add(new DradDropPanel(1) { Dock = DockStyle.Fill });
            this.Controls.Add(this.Menu);
           
        }

    }
}

