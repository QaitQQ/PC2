
using System;
using System.Windows.Forms;

namespace Client.Forms
{
    partial class ItemForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.TechField = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.DelDouble = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.ShowWebBrowserButton = new System.Windows.Forms.Button();
            this.SiteApiLoadAll = new System.Windows.Forms.Button();
            this.Get_Mapped = new System.Windows.Forms.Button();
            this.SearchTextBox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SearchList = new System.Windows.Forms.ListBox();
            this.MainFieldTable = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel4 = new System.Windows.Forms.Panel();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.panel5 = new System.Windows.Forms.Panel();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.ItemFieldBox = new System.Windows.Forms.ComboBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainFieldTable)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).BeginInit();
            this.splitContainer5.Panel1.SuspendLayout();
            this.splitContainer5.Panel2.SuspendLayout();
            this.splitContainer5.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.SuspendLayout();
            // 
            // TechField
            // 
            this.TechField.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.TechField.Location = new System.Drawing.Point(0, 597);
            this.TechField.Name = "TechField";
            this.TechField.Size = new System.Drawing.Size(1275, 53);
            this.TechField.TabIndex = 0;
            this.TechField.Text = "";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ItemFieldBox);
            this.panel1.Controls.Add(this.DelDouble);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.button5);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.ShowWebBrowserButton);
            this.panel1.Controls.Add(this.SiteApiLoadAll);
            this.panel1.Controls.Add(this.Get_Mapped);
            this.panel1.Controls.Add(this.SearchTextBox);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(140, 597);
            this.panel1.TabIndex = 2;
            // 
            // DelDouble
            // 
            this.DelDouble.Location = new System.Drawing.Point(3, 505);
            this.DelDouble.Name = "DelDouble";
            this.DelDouble.Size = new System.Drawing.Size(63, 40);
            this.DelDouble.TabIndex = 14;
            this.DelDouble.Text = "Удалить Дубилакаты";
            this.DelDouble.UseVisualStyleBackColor = true;
            this.DelDouble.Click += new System.EventHandler(this.DelDouble_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(0, 374);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(129, 20);
            this.textBox1.TabIndex = 13;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(3, 328);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(63, 40);
            this.button5.TabIndex = 12;
            this.button5.Text = "Вывод сопоставленных";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.Retun_Compare_Site_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(74, 129);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(63, 40);
            this.button4.TabIndex = 11;
            this.button4.Text = "Вывод новых";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.Retun_New_Names_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(6, 129);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(63, 40);
            this.button3.TabIndex = 10;
            this.button3.Text = "Вывод сопоставленных";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Retun_Compare_Names_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(72, 551);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(63, 40);
            this.button2.TabIndex = 9;
            this.button2.Text = "Dom";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.DOM_Click);
            // 
            // ShowWebBrowserButton
            // 
            this.ShowWebBrowserButton.Location = new System.Drawing.Point(3, 551);
            this.ShowWebBrowserButton.Name = "ShowWebBrowserButton";
            this.ShowWebBrowserButton.Size = new System.Drawing.Size(63, 40);
            this.ShowWebBrowserButton.TabIndex = 8;
            this.ShowWebBrowserButton.Text = "Браузер";
            this.ShowWebBrowserButton.UseVisualStyleBackColor = true;
            this.ShowWebBrowserButton.Click += new System.EventHandler(this.ShowWebBrowserButton_Click);
            // 
            // SiteApiLoadAll
            // 
            this.SiteApiLoadAll.Location = new System.Drawing.Point(3, 248);
            this.SiteApiLoadAll.Name = "SiteApiLoadAll";
            this.SiteApiLoadAll.Size = new System.Drawing.Size(132, 23);
            this.SiteApiLoadAll.TabIndex = 7;
            this.SiteApiLoadAll.Text = "SiteApiLoadAll";
            this.SiteApiLoadAll.UseVisualStyleBackColor = true;
            this.SiteApiLoadAll.Click += new System.EventHandler(this.SiteApiLoadAll_Click);
            // 
            // Get_Mapped
            // 
            this.Get_Mapped.Location = new System.Drawing.Point(3, 277);
            this.Get_Mapped.Name = "Get_Mapped";
            this.Get_Mapped.Size = new System.Drawing.Size(132, 45);
            this.Get_Mapped.TabIndex = 4;
            this.Get_Mapped.Text = "Выборка сопоставленных";
            this.Get_Mapped.UseVisualStyleBackColor = true;
            this.Get_Mapped.Click += new System.EventHandler(this.Get_Mapped_Click);
            // 
            // SearchTextBox
            // 
            this.SearchTextBox.Location = new System.Drawing.Point(6, 7);
            this.SearchTextBox.Name = "SearchTextBox";
            this.SearchTextBox.Size = new System.Drawing.Size(129, 20);
            this.SearchTextBox.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 33);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(129, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Поиск";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Search_Click);
            // 
            // SearchList
            // 
            this.SearchList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.SearchList.Cursor = System.Windows.Forms.Cursors.Default;
            this.SearchList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SearchList.FormattingEnabled = true;
            this.SearchList.Location = new System.Drawing.Point(0, 0);
            this.SearchList.MinimumSize = new System.Drawing.Size(50, 572);
            this.SearchList.Name = "SearchList";
            this.SearchList.Size = new System.Drawing.Size(192, 597);
            this.SearchList.TabIndex = 4;
            this.SearchList.SelectedIndexChanged += new System.EventHandler(this.SearchList_SelectedIndexChanged);
            // 
            // MainFieldTable
            // 
            this.MainFieldTable.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.MainFieldTable.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.MainFieldTable.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.MainFieldTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.MainFieldTable.ColumnHeadersVisible = false;
            this.MainFieldTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.MainFieldTable.DefaultCellStyle = dataGridViewCellStyle2;
            this.MainFieldTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainFieldTable.GridColor = System.Drawing.SystemColors.ButtonHighlight;
            this.MainFieldTable.Location = new System.Drawing.Point(0, 0);
            this.MainFieldTable.Name = "MainFieldTable";
            this.MainFieldTable.RowHeadersVisible = false;
            this.MainFieldTable.Size = new System.Drawing.Size(906, 597);
            this.MainFieldTable.TabIndex = 5;
            this.MainFieldTable.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.MainFieldTable_Click);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 300;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "";
            this.Column2.Name = "Column2";
            this.Column2.Width = 300;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel6);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(25, 597);
            this.panel2.TabIndex = 2;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.splitContainer4);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(25, 597);
            this.panel6.TabIndex = 1;
            this.panel6.Visible = false;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.splitContainer5);
            this.splitContainer4.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.treeView1);
            this.splitContainer4.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer4.Size = new System.Drawing.Size(25, 597);
            this.splitContainer4.SplitterDistance = this.splitContainer4.Height;
            this.splitContainer4.TabIndex = 0;
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
            this.splitContainer5.Panel1.Controls.Add(this.comboBox1);
            // 
            // splitContainer5.Panel2
            // 
            this.splitContainer5.Panel2.Controls.Add(this.webBrowser);
            this.splitContainer5.Size = new System.Drawing.Size(25, 568);
            this.splitContainer5.SplitterDistance = 32;
            this.splitContainer5.TabIndex = 2;
            // 
            // comboBox1
            // 
            this.comboBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(0, 0);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(25, 21);
            this.comboBox1.TabIndex = 1;
            // 
            // webBrowser
            // 
            this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser.Location = new System.Drawing.Point(0, 0);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.Size = new System.Drawing.Size(25, 532);
            this.webBrowser.TabIndex = 0;
            this.webBrowser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.WebBrowser_DocumentCompleted);
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(25, 25);
            this.treeView1.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.splitContainer1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1275, 597);
            this.panel3.TabIndex = 6;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel4);
            this.splitContainer1.Size = new System.Drawing.Size(1275, 597);
            this.splitContainer1.SplitterDistance = 140;
            this.splitContainer1.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.splitContainer2);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1131, 597);
            this.panel4.TabIndex = 5;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.SearchList);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.panel5);
            this.splitContainer2.Size = new System.Drawing.Size(1131, 597);
            this.splitContainer2.SplitterDistance = 192;
            this.splitContainer2.TabIndex = 0;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.splitContainer3);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(935, 597);
            this.panel5.TabIndex = 0;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.MainFieldTable);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.panel2);
            this.splitContainer3.Size = new System.Drawing.Size(935, 597);
            this.splitContainer3.SplitterDistance = this.splitContainer3.Width;
            this.splitContainer3.TabIndex = 0;
            // 
            // ItemFieldBox
            // 
            this.ItemFieldBox.FormattingEnabled = true;
            this.ItemFieldBox.Location = new System.Drawing.Point(8, 62);
            this.ItemFieldBox.Name = "ItemFieldBox";
            this.ItemFieldBox.Size = new System.Drawing.Size(127, 21);
            this.ItemFieldBox.TabIndex = 16;
            // 
            // ItemForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1275, 650);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.TechField);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ItemForm";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainFieldTable)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.splitContainer5.Panel1.ResumeLayout(false);
            this.splitContainer5.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).EndInit();
            this.splitContainer5.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

  

        #endregion

        private System.Windows.Forms.RichTextBox TechField;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox SearchTextBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox SearchList;
        private System.Windows.Forms.DataGridView MainFieldTable;
        private Button Get_Mapped;
        private Button SiteApiLoadAll;
        private Panel panel2;
        private WebBrowser webBrowser;
        private Button ShowWebBrowserButton;
        private Button button2;
        private Panel panel3;
        private SplitContainer splitContainer1;
        private Panel panel4;
        private SplitContainer splitContainer2;
        private Panel panel5;
        private SplitContainer splitContainer3;
        private Panel panel6;
        private SplitContainer splitContainer4;
        private TreeView treeView1;
        private SplitContainer splitContainer5;
        private ComboBox comboBox1;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private Button button3;
        private Button button4;
        private Button button5;
        private TextBox textBox1;
        private Button DelDouble;
        private ComboBox ItemFieldBox;
    }
}