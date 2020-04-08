namespace Client.Forms
{
    partial class Mainform
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.видToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.окноToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cRMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ItemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EventsBox = new System.Windows.Forms.ToolStripMenuItem();
            this.iCQToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configformToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.словариToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сетевыеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem,
            this.видToolStripMenuItem,
            this.configformToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1429, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // видToolStripMenuItem
            // 
            this.видToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.окноToolStripMenuItem});
            this.видToolStripMenuItem.Name = "видToolStripMenuItem";
            this.видToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.видToolStripMenuItem.Text = "Вид";
            // 
            // окноToolStripMenuItem
            // 
            this.окноToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cRMToolStripMenuItem,
            this.ItemsToolStripMenuItem,
            this.EventsBox,
            this.iCQToolStripMenuItem});
            this.окноToolStripMenuItem.Name = "окноToolStripMenuItem";
            this.окноToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.окноToolStripMenuItem.Text = "Окно";
            // 
            // cRMToolStripMenuItem
            // 
            this.cRMToolStripMenuItem.Name = "cRMToolStripMenuItem";
            this.cRMToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.cRMToolStripMenuItem.Text = "Партнеры";
            this.cRMToolStripMenuItem.Click += new System.EventHandler(this.cRMToolStripMenuItem_Click);
            // 
            // ItemsToolStripMenuItem
            // 
            this.ItemsToolStripMenuItem.Name = "ItemsToolStripMenuItem";
            this.ItemsToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.ItemsToolStripMenuItem.Text = "Номенклатура";
            this.ItemsToolStripMenuItem.Click += new System.EventHandler(this.ItemToolStripMenuItem_Click);
            // 
            // EventsBox
            // 
            this.EventsBox.Name = "EventsBox";
            this.EventsBox.Size = new System.Drawing.Size(155, 22);
            this.EventsBox.Text = "Задачи";
            // 
            // iCQToolStripMenuItem
            // 
            this.iCQToolStripMenuItem.Name = "iCQToolStripMenuItem";
            this.iCQToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.iCQToolStripMenuItem.Text = "ICQ";
            // 
            // configformToolStripMenuItem
            // 
            this.configformToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.словариToolStripMenuItem,
            this.сетевыеToolStripMenuItem});
            this.configformToolStripMenuItem.Name = "configformToolStripMenuItem";
            this.configformToolStripMenuItem.Size = new System.Drawing.Size(82, 20);
            this.configformToolStripMenuItem.Text = " Настройки";
            // 
            // словариToolStripMenuItem
            // 
            this.словариToolStripMenuItem.Name = "словариToolStripMenuItem";
            this.словариToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.словариToolStripMenuItem.Text = "Словари";
            this.словариToolStripMenuItem.Click += new System.EventHandler(this.DictionariesToolStripMenuItem_Click);
            // 
            // сетевыеToolStripMenuItem
            // 
            this.сетевыеToolStripMenuItem.Name = "сетевыеToolStripMenuItem";
            this.сетевыеToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.сетевыеToolStripMenuItem.Text = "Сетевые";
            this.сетевыеToolStripMenuItem.Click += new System.EventHandler(this.NetToolStripMenuItem_Click);
            // 
            // Mainform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1429, 565);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Mainform";
            this.Text = "Mainform";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Mainform_FormClosed);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem видToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem окноToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cRMToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ItemsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem EventsBox;
        private System.Windows.Forms.ToolStripMenuItem configformToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem iCQToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem словариToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сетевыеToolStripMenuItem;
    }
}