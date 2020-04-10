namespace Client.Forms
{
    partial class АuthorizationForm
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
            this.Ok_Button = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.AutologinCheck = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // Ok_Button
            // 
            this.Ok_Button.Location = new System.Drawing.Point(371, 52);
            this.Ok_Button.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Ok_Button.Name = "Ok_Button";
            this.Ok_Button.Size = new System.Drawing.Size(78, 69);
            this.Ok_Button.TabIndex = 0;
            this.Ok_Button.Text = "Ok";
            this.Ok_Button.UseVisualStyleBackColor = false;
            this.Ok_Button.Click += new System.EventHandler(this.Ok_Button_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(398, 1);
            this.button3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(88, 27);
            this.button3.TabIndex = 2;
            this.button3.Text = "Настройки";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(110, 52);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(234, 23);
            this.comboBox1.TabIndex = 3;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(110, 98);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(234, 23);
            this.textBox1.TabIndex = 4;
            this.textBox1.UseSystemPasswordChar = true;
            // 
            // AutologinCheck
            // 
            this.AutologinCheck.AutoSize = true;
            this.AutologinCheck.Location = new System.Drawing.Point(371, 141);
            this.AutologinCheck.Name = "AutologinCheck";
            this.AutologinCheck.Size = new System.Drawing.Size(80, 19);
            this.AutologinCheck.TabIndex = 5;
            this.AutologinCheck.Text = "Авто вход";
            this.AutologinCheck.UseVisualStyleBackColor = true;
            this.AutologinCheck.CheckedChanged += new System.EventHandler(this.AutologinCheck_CheckedChanged);
            // 
            // АuthorizationForm
            // 
            this.AcceptButton = this.Ok_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(486, 200);
            this.Controls.Add(this.AutologinCheck);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.Ok_Button);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "АuthorizationForm";
            this.Text = "АuthorizationWindow";
            this.Activated += new System.EventHandler(this.АuthorizationForm_Activated);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.АuthorizationForm_KeyPress);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Ok_Button;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckBox AutologinCheck;
    }
}