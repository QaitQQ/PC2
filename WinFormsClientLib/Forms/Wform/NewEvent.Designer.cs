namespace Client.Forms
{
    partial class CreateEvents
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose ( bool disposing )
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
        private void InitializeComponent ( )
        {
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.Time1 = new System.Windows.Forms.DateTimePicker();
            this.OK = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.Time2 = new System.Windows.Forms.DateTimePicker();
            this.EventTypeBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(23, 126);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(752, 205);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // Time1
            // 
            this.Time1.Location = new System.Drawing.Point(23, 12);
            this.Time1.Name = "Time1";
            this.Time1.Size = new System.Drawing.Size(200, 20);
            this.Time1.TabIndex = 1;
            // 
            // OK
            // 
            this.OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OK.Location = new System.Drawing.Point(632, 415);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(75, 23);
            this.OK.TabIndex = 2;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // Cancel
            // 
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(713, 415);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 3;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // Time2
            // 
            this.Time2.Location = new System.Drawing.Point(575, 12);
            this.Time2.Name = "Time2";
            this.Time2.Size = new System.Drawing.Size(200, 20);
            this.Time2.TabIndex = 4;
            // 
            // comboBox1
            // 
            this.EventTypeBox.FormattingEnabled = true;
            this.EventTypeBox.Location = new System.Drawing.Point(23, 53);
            this.EventTypeBox.Name = "comboBox1";
            this.EventTypeBox.Size = new System.Drawing.Size(200, 21);
            this.EventTypeBox.TabIndex = 5;
            // 
            // CreateEvents
            // 
            this.AcceptButton = this.OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.EventTypeBox);
            this.Controls.Add(this.Time2);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.Time1);
            this.Controls.Add(this.richTextBox1);
            this.Name = "CreateEvents";
            this.Text = "new Event";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.DateTimePicker Time1;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.DateTimePicker Time2;
        private System.Windows.Forms.ComboBox EventTypeBox;
    }
}