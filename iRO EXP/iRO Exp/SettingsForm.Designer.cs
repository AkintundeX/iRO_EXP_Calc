namespace iROClassicExp
{
    partial class SettingsForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textWindowTitle = new System.Windows.Forms.TextBox();
            this.textClassicProcess = new System.Windows.Forms.TextBox();
            this.textRenewalProcess = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.comboScreenshotKey = new System.Windows.Forms.ComboBox();
            this.comboReset = new System.Windows.Forms.ComboBox();
            this.comboStop = new System.Windows.Forms.ComboBox();
            this.comboPause = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Screenshot Key";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Reset Key";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "Stop Key";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(12, 78);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 23);
            this.label4.TabIndex = 3;
            this.label4.Text = "Pause Key";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(12, 101);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 23);
            this.label5.TabIndex = 4;
            this.label5.Text = "Window Title";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(12, 124);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 23);
            this.label6.TabIndex = 5;
            this.label6.Text = "Classic Process";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(12, 147);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(100, 23);
            this.label7.TabIndex = 6;
            this.label7.Text = "Renewal Process";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textWindowTitle
            // 
            this.textWindowTitle.Location = new System.Drawing.Point(118, 103);
            this.textWindowTitle.Name = "textWindowTitle";
            this.textWindowTitle.Size = new System.Drawing.Size(153, 20);
            this.textWindowTitle.TabIndex = 7;
            // 
            // textClassicProcess
            // 
            this.textClassicProcess.Location = new System.Drawing.Point(118, 126);
            this.textClassicProcess.Name = "textClassicProcess";
            this.textClassicProcess.Size = new System.Drawing.Size(153, 20);
            this.textClassicProcess.TabIndex = 8;
            // 
            // textRenewalProcess
            // 
            this.textRenewalProcess.Location = new System.Drawing.Point(118, 149);
            this.textRenewalProcess.Name = "textRenewalProcess";
            this.textRenewalProcess.Size = new System.Drawing.Size(153, 20);
            this.textRenewalProcess.TabIndex = 9;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(195, 176);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(114, 175);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 11;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // comboScreenshotKey
            // 
            this.comboScreenshotKey.FormattingEnabled = true;
            this.comboScreenshotKey.Items.AddRange(new object[] {
            "Print Screen",
            "Scroll Lock",
            "Pause",
            "Insert",
            "Delete",
            "Home",
            "End",
            "Page Up",
            "Page Down"});
            this.comboScreenshotKey.Location = new System.Drawing.Point(118, 11);
            this.comboScreenshotKey.Name = "comboScreenshotKey";
            this.comboScreenshotKey.Size = new System.Drawing.Size(152, 21);
            this.comboScreenshotKey.TabIndex = 12;
            // 
            // comboReset
            // 
            this.comboReset.FormattingEnabled = true;
            this.comboReset.Items.AddRange(new object[] {
            "Print Screen",
            "Scroll Lock",
            "Pause",
            "Insert",
            "Delete",
            "Home",
            "End",
            "Page Up",
            "Page Down"});
            this.comboReset.Location = new System.Drawing.Point(118, 34);
            this.comboReset.Name = "comboReset";
            this.comboReset.Size = new System.Drawing.Size(152, 21);
            this.comboReset.TabIndex = 13;
            // 
            // comboStop
            // 
            this.comboStop.FormattingEnabled = true;
            this.comboStop.Items.AddRange(new object[] {
            "Print Screen",
            "Scroll Lock",
            "Pause",
            "Insert",
            "Delete",
            "Home",
            "End",
            "Page Up",
            "Page Down"});
            this.comboStop.Location = new System.Drawing.Point(118, 57);
            this.comboStop.Name = "comboStop";
            this.comboStop.Size = new System.Drawing.Size(152, 21);
            this.comboStop.TabIndex = 14;
            // 
            // comboPause
            // 
            this.comboPause.FormattingEnabled = true;
            this.comboPause.Items.AddRange(new object[] {
            "Print Screen",
            "Scroll Lock",
            "Pause",
            "Insert",
            "Delete",
            "Home",
            "End",
            "Page Up",
            "Page Down"});
            this.comboPause.Location = new System.Drawing.Point(118, 80);
            this.comboPause.Name = "comboPause";
            this.comboPause.Size = new System.Drawing.Size(152, 21);
            this.comboPause.TabIndex = 15;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 205);
            this.Controls.Add(this.comboPause);
            this.Controls.Add(this.comboStop);
            this.Controls.Add(this.comboReset);
            this.Controls.Add(this.comboScreenshotKey);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.textRenewalProcess);
            this.Controls.Add(this.textClassicProcess);
            this.Controls.Add(this.textWindowTitle);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "SettingsForm";
            this.Text = "SettingsForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textWindowTitle;
        private System.Windows.Forms.TextBox textClassicProcess;
        private System.Windows.Forms.TextBox textRenewalProcess;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ComboBox comboScreenshotKey;
        private System.Windows.Forms.ComboBox comboReset;
        private System.Windows.Forms.ComboBox comboStop;
        private System.Windows.Forms.ComboBox comboPause;
    }
}