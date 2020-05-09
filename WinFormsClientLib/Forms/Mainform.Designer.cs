using System;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Mainform));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.видToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.WindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cRMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ItemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EventsBox = new System.Windows.Forms.ToolStripMenuItem();
            this.configformToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.словариToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сетевыеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ClientConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel2 = new System.Windows.Forms.Panel();
            this.menuStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
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
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(699, 24);
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
            this.WindowToolStripMenuItem});
            this.видToolStripMenuItem.Name = "видToolStripMenuItem";
            this.видToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.видToolStripMenuItem.Text = "Вид";
            // 
            // WindowToolStripMenuItem
            // 
            this.WindowToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cRMToolStripMenuItem,
            this.ItemsToolStripMenuItem,
            this.EventsBox});
            this.WindowToolStripMenuItem.Name = "WindowToolStripMenuItem";
            this.WindowToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.WindowToolStripMenuItem.Text = "Окно";
            // 
            // cRMToolStripMenuItem
            // 
            this.cRMToolStripMenuItem.Name = "cRMToolStripMenuItem";
            this.cRMToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.cRMToolStripMenuItem.Text = "Партнеры";
            this.cRMToolStripMenuItem.Click += new System.EventHandler(this.CRMToolStripMenuItem_Click);
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
            // configformToolStripMenuItem
            // 
            this.configformToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.словариToolStripMenuItem,
            this.сетевыеToolStripMenuItem,
            this.ClientConfig});
            this.configformToolStripMenuItem.Name = "configformToolStripMenuItem";
            this.configformToolStripMenuItem.Size = new System.Drawing.Size(82, 20);
            this.configformToolStripMenuItem.Text = " Настройки";
            // 
            // словариToolStripMenuItem
            // 
            this.словариToolStripMenuItem.Name = "словариToolStripMenuItem";
            this.словариToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.словариToolStripMenuItem.Text = "Словари";
            this.словариToolStripMenuItem.Click += new System.EventHandler(this.DictionariesToolStripMenuItem_Click);
            // 
            // сетевыеToolStripMenuItem
            // 
            this.сетевыеToolStripMenuItem.Name = "сетевыеToolStripMenuItem";
            this.сетевыеToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.сетевыеToolStripMenuItem.Text = "Сетевые";
            this.сетевыеToolStripMenuItem.Click += new System.EventHandler(this.NetToolStripMenuItem_Click);
            // 
            // ClientConfig
            // 
            this.ClientConfig.Name = "ClientConfig";
            this.ClientConfig.Size = new System.Drawing.Size(181, 22);
            this.ClientConfig.Text = "Настройки клиента";
            this.ClientConfig.Click += new System.EventHandler(this.ClientConfigMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(505, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(194, 628);
            this.panel1.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button1.BackColor = System.Drawing.SystemColors.HotTrack;
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Margin = new System.Windows.Forms.Padding(0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(10, 628);
            this.button1.TabIndex = 4;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.ShowWebBrowser);
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter1.Location = new System.Drawing.Point(502, 24);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 628);
            this.splitter1.TabIndex = 6;
            this.splitter1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(492, 24);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(10, 628);
            this.panel2.TabIndex = 8;
            // 
            // Mainform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(699, 652);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "Mainform";
            this.Text = "Mainform";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Shown += new System.EventHandler(this.Mainform_Shown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }




        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem видToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem WindowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cRMToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ItemsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem EventsBox;
        private System.Windows.Forms.ToolStripMenuItem configformToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem словариToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ClientConfig;
        private System.Windows.Forms.ToolStripMenuItem сетевыеToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panel2;
    }
}