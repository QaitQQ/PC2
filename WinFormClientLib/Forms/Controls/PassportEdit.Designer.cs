namespace PassportEditControl
{
    partial class PassportEdit
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
            if(disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this._cb = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this._mskEdit = new System.Windows.Forms.MaskedTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this._dtp = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(1, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Серия:";
            // 
            // _cb
            // 
            this._cb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cb.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._cb.FormattingEnabled = true;
            this._cb.Items.AddRange(new object[] {
            "00",
            "11",
            "22",
            "33",
            "44",
            "55",
            "66",
            "77",
            "88",
            "99"});
            this._cb.Location = new System.Drawing.Point(56, 1);
            this._cb.Name = "_cb";
            this._cb.Size = new System.Drawing.Size(51, 26);
            this._cb.TabIndex = 1;
            this._cb.SelectedIndexChanged += new System.EventHandler(this._cb_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(113, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "№:";
            // 
            // _mskEdit
            // 
            this._mskEdit.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._mskEdit.Location = new System.Drawing.Point(145, 1);
            this._mskEdit.Mask = "000000";
            this._mskEdit.Name = "_mskEdit";
            this._mskEdit.Size = new System.Drawing.Size(68, 26);
            this._mskEdit.TabIndex = 3;
            this._mskEdit.Validating += new System.ComponentModel.CancelEventHandler(this._mskEdit_Validating);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(219, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "Выдан:";
            // 
            // _dtp
            // 
            this._dtp.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._dtp.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._dtp.Location = new System.Drawing.Point(277, 0);
            this._dtp.Name = "_dtp";
            this._dtp.Size = new System.Drawing.Size(126, 26);
            this._dtp.TabIndex = 5;
            this._dtp.ValueChanged += new System.EventHandler(this._dtp_ValueChanged);
            // 
            // PassportEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._dtp);
            this.Controls.Add(this.label3);
            this.Controls.Add(this._mskEdit);
            this.Controls.Add(this.label2);
            this.Controls.Add(this._cb);
            this.Controls.Add(this.label1);
            this.Name = "PassportEdit";
            this.Size = new System.Drawing.Size(407, 28);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox _cb;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MaskedTextBox _mskEdit;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker _dtp;
    }
}
