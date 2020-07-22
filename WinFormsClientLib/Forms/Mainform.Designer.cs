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
            this.cRMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ItemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EventsBox = new System.Windows.Forms.ToolStripMenuItem();
            this.configformToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.словариToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сетевыеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ClientConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.WindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RightPanel = new System.Windows.Forms.Panel();
            this.LeftPanel = new System.Windows.Forms.Panel();
            this.ShowRightPanelButton = new System.Windows.Forms.Button();
            this.ShowLeftPanelButton = new System.Windows.Forms.Button();
            this.RightPanelSplitter = new System.Windows.Forms.Splitter();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.LeftPanelSplitter = new System.Windows.Forms.Splitter();
            this.menuStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
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
            this.cRMToolStripMenuItem,
            this.ItemsToolStripMenuItem,
            this.EventsBox});
            this.видToolStripMenuItem.Name = "видToolStripMenuItem";
            this.видToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.видToolStripMenuItem.Text = "Вид";
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
            // WindowToolStripMenuItem
            // 
            this.WindowToolStripMenuItem.Name = "WindowToolStripMenuItem";
            this.WindowToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.WindowToolStripMenuItem.Text = "Окно";
            // 
            // RightPanel
            // 
            this.RightPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.RightPanel.Location = new System.Drawing.Point(505, 24);
            this.RightPanel.Name = "RightPanel";
            this.RightPanel.Size = new System.Drawing.Size(194, 628);
            this.RightPanel.TabIndex = 3;
            // 
            // LeftPanel
            // 
            this.LeftPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.LeftPanel.Location = new System.Drawing.Point(3, 24);
            this.LeftPanel.Name = "LeftPanel";
            this.LeftPanel.Size = new System.Drawing.Size(121, 628);
            this.LeftPanel.TabIndex = 3;
            // 
            // ShowRightPanelButton
            // 
            this.ShowRightPanelButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ShowRightPanelButton.BackColor = System.Drawing.SystemColors.HotTrack;
            this.ShowRightPanelButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ShowRightPanelButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ShowRightPanelButton.Location = new System.Drawing.Point(0, 0);
            this.ShowRightPanelButton.Margin = new System.Windows.Forms.Padding(0);
            this.ShowRightPanelButton.Name = "ShowRightPanelButton";
            this.ShowRightPanelButton.Size = new System.Drawing.Size(10, 628);
            this.ShowRightPanelButton.TabIndex = 4;
            this.ShowRightPanelButton.UseVisualStyleBackColor = false;
            this.ShowRightPanelButton.Click += new System.EventHandler(this.ShowWebBrowser);
            // 
            // ShowLeftPanelButton
            // 
            this.ShowLeftPanelButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ShowLeftPanelButton.BackColor = System.Drawing.Color.LightSkyBlue;
            this.ShowLeftPanelButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ShowLeftPanelButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ShowLeftPanelButton.Location = new System.Drawing.Point(0, 0);
            this.ShowLeftPanelButton.Margin = new System.Windows.Forms.Padding(0);
            this.ShowLeftPanelButton.Name = "ShowLeftPanelButton";
            this.ShowLeftPanelButton.Size = new System.Drawing.Size(10, 628);
            this.ShowLeftPanelButton.TabIndex = 4;
            this.ShowLeftPanelButton.UseVisualStyleBackColor = false;
            this.ShowLeftPanelButton.Click += new System.EventHandler(this.ShowLeftPanelButton_Click);
            // 
            // RightPanelSplitter
            // 
            this.RightPanelSplitter.Dock = System.Windows.Forms.DockStyle.Right;
            this.RightPanelSplitter.Location = new System.Drawing.Point(502, 24);
            this.RightPanelSplitter.Name = "RightPanelSplitter";
            this.RightPanelSplitter.Size = new System.Drawing.Size(3, 628);
            this.RightPanelSplitter.TabIndex = 6;
            this.RightPanelSplitter.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ShowRightPanelButton);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(492, 24);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(10, 628);
            this.panel2.TabIndex = 8;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ShowLeftPanelButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(124, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(10, 628);
            this.panel1.TabIndex = 10;
            // 
            // LeftPanelSplitter
            // 
            this.LeftPanelSplitter.Location = new System.Drawing.Point(0, 24);
            this.LeftPanelSplitter.Name = "LeftPanelSplitter";
            this.LeftPanelSplitter.Size = new System.Drawing.Size(3, 628);
            this.LeftPanelSplitter.TabIndex = 11;
            this.LeftPanelSplitter.TabStop = false;
            // 
            // Mainform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(699, 652);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.LeftPanel);
            this.Controls.Add(this.LeftPanelSplitter);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.RightPanelSplitter);
            this.Controls.Add(this.RightPanel);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "Mainform";
            this.Text = "Mainform";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
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
        private System.Windows.Forms.Panel RightPanel;
        private System.Windows.Forms.Panel LeftPanel;
        private System.Windows.Forms.Button ShowRightPanelButton;
        private System.Windows.Forms.Button ShowLeftPanelButton;
        private System.Windows.Forms.Splitter RightPanelSplitter;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Splitter LeftPanelSplitter;
    }
}