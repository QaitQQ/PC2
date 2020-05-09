namespace Client.Forms
{
    partial class CRMForm
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
            this.StructTree = new System.Windows.Forms.TreeView();
            this.ListPartner = new System.Windows.Forms.DataGridView();
            this.Партнер = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.CallButton = new System.Windows.Forms.Button();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.AddEventButton = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.EventBox = new System.Windows.Forms.DataGridView();
            this.Data = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Event = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PlaneButton = new System.Windows.Forms.Button();
            this.DelEvent = new System.Windows.Forms.Button();
            this.EditEvent = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.panel4 = new System.Windows.Forms.Panel();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.performanceCounter1 = new System.Diagnostics.PerformanceCounter();
            ((System.ComponentModel.ISupportInitialize)(this.ListPartner)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EventBox)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).BeginInit();
            this.splitContainer5.Panel1.SuspendLayout();
            this.splitContainer5.Panel2.SuspendLayout();
            this.splitContainer5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.performanceCounter1)).BeginInit();
            this.SuspendLayout();
            // 
            // StructTree
            // 
            this.StructTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StructTree.Location = new System.Drawing.Point(0, 0);
            this.StructTree.Name = "StructTree";
            this.StructTree.Size = new System.Drawing.Size(246, 503);
            this.StructTree.TabIndex = 0;
            // 
            // ListPartner
            // 
            this.ListPartner.AllowUserToAddRows = false;
            this.ListPartner.AllowUserToDeleteRows = false;
            this.ListPartner.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ListPartner.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ListPartner.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Партнер});
            this.ListPartner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ListPartner.GridColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ListPartner.Location = new System.Drawing.Point(0, 0);
            this.ListPartner.Name = "ListPartner";
            this.ListPartner.ReadOnly = true;
            this.ListPartner.Size = new System.Drawing.Size(870, 445);
            this.ListPartner.TabIndex = 1;
            this.ListPartner.SelectionChanged += new System.EventHandler(this.ListPartner_SelectionChanged);
            // 
            // Партнер
            // 
            this.Партнер.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Партнер.HeaderText = "Partner";
            this.Партнер.Name = "Партнер";
            this.Партнер.ReadOnly = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.CallButton);
            this.panel1.Controls.Add(this.textBox4);
            this.panel1.Controls.Add(this.textBox3);
            this.panel1.Controls.Add(this.textBox2);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(232, 561);
            this.panel1.TabIndex = 3;
            // 
            // CallButton
            // 
            this.CallButton.Location = new System.Drawing.Point(168, 6);
            this.CallButton.Name = "CallButton";
            this.CallButton.Size = new System.Drawing.Size(23, 20);
            this.CallButton.TabIndex = 4;
            this.CallButton.UseVisualStyleBackColor = true;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(3, 84);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(158, 20);
            this.textBox4.TabIndex = 3;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(3, 58);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(158, 20);
            this.textBox3.TabIndex = 2;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(3, 32);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(158, 20);
            this.textBox2.TabIndex = 1;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(3, 6);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(158, 20);
            this.textBox1.TabIndex = 0;
            // 
            // AddEventButton
            // 
            this.AddEventButton.AutoSize = true;
            this.AddEventButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.AddEventButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.AddEventButton.Location = new System.Drawing.Point(0, 0);
            this.AddEventButton.Name = "AddEventButton";
            this.AddEventButton.Size = new System.Drawing.Size(113, 23);
            this.AddEventButton.TabIndex = 4;
            this.AddEventButton.Text = "Добаваить";
            this.AddEventButton.UseVisualStyleBackColor = true;
            this.AddEventButton.Click += new System.EventHandler(this.AddEventButton_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.splitContainer1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(5);
            this.panel2.Size = new System.Drawing.Size(870, 112);
            this.panel2.TabIndex = 4;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(5, 5);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.EventBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.AutoScroll = true;
            this.splitContainer1.Panel2.Controls.Add(this.PlaneButton);
            this.splitContainer1.Panel2.Controls.Add(this.DelEvent);
            this.splitContainer1.Panel2.Controls.Add(this.EditEvent);
            this.splitContainer1.Panel2.Controls.Add(this.AddEventButton);
            this.splitContainer1.Size = new System.Drawing.Size(860, 102);
            this.splitContainer1.SplitterDistance = 743;
            this.splitContainer1.TabIndex = 5;
            // 
            // EventBox
            // 
            this.EventBox.AllowUserToAddRows = false;
            this.EventBox.AllowUserToDeleteRows = false;
            this.EventBox.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.EventBox.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.EventBox.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Data,
            this.Event});
            this.EventBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EventBox.GridColor = System.Drawing.SystemColors.ButtonFace;
            this.EventBox.Location = new System.Drawing.Point(0, 0);
            this.EventBox.Name = "EventBox";
            this.EventBox.ReadOnly = true;
            this.EventBox.Size = new System.Drawing.Size(743, 102);
            this.EventBox.TabIndex = 0;
            // 
            // Data
            // 
            this.Data.HeaderText = "Дата";
            this.Data.Name = "Data";
            this.Data.ReadOnly = true;
            // 
            // Event
            // 
            this.Event.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Event.HeaderText = "Событие";
            this.Event.Name = "Event";
            this.Event.ReadOnly = true;
            // 
            // PlaneButton
            // 
            this.PlaneButton.AutoSize = true;
            this.PlaneButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.PlaneButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.PlaneButton.Location = new System.Drawing.Point(0, 69);
            this.PlaneButton.Name = "PlaneButton";
            this.PlaneButton.Size = new System.Drawing.Size(113, 23);
            this.PlaneButton.TabIndex = 7;
            this.PlaneButton.Text = "Запланировать";
            this.PlaneButton.UseVisualStyleBackColor = true;
            // 
            // DelEvent
            // 
            this.DelEvent.AutoSize = true;
            this.DelEvent.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.DelEvent.Dock = System.Windows.Forms.DockStyle.Top;
            this.DelEvent.Location = new System.Drawing.Point(0, 46);
            this.DelEvent.Name = "DelEvent";
            this.DelEvent.Size = new System.Drawing.Size(113, 23);
            this.DelEvent.TabIndex = 6;
            this.DelEvent.Text = "Удалить";
            this.DelEvent.UseVisualStyleBackColor = true;
            // 
            // EditEvent
            // 
            this.EditEvent.AutoSize = true;
            this.EditEvent.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.EditEvent.Dock = System.Windows.Forms.DockStyle.Top;
            this.EditEvent.Location = new System.Drawing.Point(0, 23);
            this.EditEvent.Name = "EditEvent";
            this.EditEvent.Size = new System.Drawing.Size(113, 23);
            this.EditEvent.TabIndex = 5;
            this.EditEvent.Text = "Изменить";
            this.EditEvent.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.splitContainer2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(870, 561);
            this.panel3.TabIndex = 5;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.ListPartner);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.panel2);
            this.splitContainer2.Size = new System.Drawing.Size(870, 561);
            this.splitContainer2.SplitterDistance = 445;
            this.splitContainer2.TabIndex = 5;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.splitContainer3);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1356, 561);
            this.panel4.TabIndex = 6;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.splitContainer5);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.splitContainer4);
            this.splitContainer3.Size = new System.Drawing.Size(1356, 561);
            this.splitContainer3.SplitterDistance = 246;
            this.splitContainer3.TabIndex = 0;
            // 
            // splitContainer5
            // 
            this.splitContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer5.Location = new System.Drawing.Point(0, 0);
            this.splitContainer5.Name = "splitContainer5";
            this.splitContainer5.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer5.Panel1
            // 
            this.splitContainer5.Panel1.Controls.Add(this.comboBox2);
            this.splitContainer5.Panel1.Controls.Add(this.button1);
            this.splitContainer5.Panel1.Controls.Add(this.textBox5);
            this.splitContainer5.Panel1.Controls.Add(this.comboBox1);
            // 
            // splitContainer5.Panel2
            // 
            this.splitContainer5.Panel2.Controls.Add(this.StructTree);
            this.splitContainer5.Size = new System.Drawing.Size(246, 561);
            this.splitContainer5.SplitterDistance = 54;
            this.splitContainer5.TabIndex = 0;
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(45, 8);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(37, 21);
            this.comboBox2.TabIndex = 6;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(221, 7);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(21, 21);
            this.button1.TabIndex = 5;
            this.button1.UseVisualStyleBackColor = true;
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(88, 7);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(127, 20);
            this.textBox5.TabIndex = 1;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(2, 8);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(37, 21);
            this.comboBox1.TabIndex = 0;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.panel3);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.panel1);
            this.splitContainer4.Size = new System.Drawing.Size(1106, 561);
            this.splitContainer4.SplitterDistance = 870;
            this.splitContainer4.TabIndex = 0;
            // 
            // CRMForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1356, 561);
            this.Controls.Add(this.panel4);
            this.Name = "CRMForm";
            this.ShowIcon = false;
            this.Text = "CRMForm";
            this.Activated += new System.EventHandler(this.CRMForm_Activated);
            ((System.ComponentModel.ISupportInitialize)(this.ListPartner)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.EventBox)).EndInit();
            this.panel3.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.splitContainer5.Panel1.ResumeLayout(false);
            this.splitContainer5.Panel1.PerformLayout();
            this.splitContainer5.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).EndInit();
            this.splitContainer5.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);


        }

        #endregion

        private System.Windows.Forms.TreeView StructTree;
        private System.Windows.Forms.DataGridView ListPartner;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Button AddEventButton;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.Button CallButton;
        private System.Windows.Forms.Button PlaneButton;
        private System.Windows.Forms.Button DelEvent;
        private System.Windows.Forms.Button EditEvent;
        private System.Windows.Forms.SplitContainer splitContainer5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Партнер;
        private System.Windows.Forms.DataGridView EventBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn Data;
        private System.Windows.Forms.DataGridViewTextBoxColumn Event;
        private System.Diagnostics.PerformanceCounter performanceCounter1;
    }
}