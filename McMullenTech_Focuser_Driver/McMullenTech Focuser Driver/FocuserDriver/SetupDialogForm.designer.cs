namespace ASCOM.McMullenTechFocuser.Focuser
{
    partial class SetupDialogForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupDialogForm));
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.labelHeading = new System.Windows.Forms.Label();
            this.imageASCOM = new System.Windows.Forms.PictureBox();
            this.labelComPort = new System.Windows.Forms.Label();
            this.checkBoxTraceOn = new System.Windows.Forms.CheckBox();
            this.comboBoxComPort = new System.Windows.Forms.ComboBox();
            this.checkBoxShowController = new System.Windows.Forms.CheckBox();
            this.labelInfo = new System.Windows.Forms.Label();
            this.comboBoxBaudRate = new System.Windows.Forms.ComboBox();
            this.labelBaudRate = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.imageASCOM)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(148, 135);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(59, 24);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(228, 134);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(59, 25);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // labelHeading
            // 
            this.labelHeading.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHeading.Location = new System.Drawing.Point(11, 9);
            this.labelHeading.Name = "labelHeading";
            this.labelHeading.Size = new System.Drawing.Size(288, 31);
            this.labelHeading.TabIndex = 2;
            this.labelHeading.Text = "McMullenTech Focuser ASCOM Driver";
            // 
            // imageASCOM
            // 
            this.imageASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.imageASCOM.Image = ((System.Drawing.Image)(resources.GetObject("imageASCOM.Image")));
            this.imageASCOM.Location = new System.Drawing.Point(239, 40);
            this.imageASCOM.Name = "imageASCOM";
            this.imageASCOM.Size = new System.Drawing.Size(48, 56);
            this.imageASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.imageASCOM.TabIndex = 3;
            this.imageASCOM.TabStop = false;
            this.imageASCOM.Click += new System.EventHandler(this.BrowseToAscom);
            this.imageASCOM.DoubleClick += new System.EventHandler(this.BrowseToAscom);
            // 
            // labelComPort
            // 
            this.labelComPort.AutoSize = true;
            this.labelComPort.Location = new System.Drawing.Point(12, 64);
            this.labelComPort.Name = "labelComPort";
            this.labelComPort.Size = new System.Drawing.Size(53, 13);
            this.labelComPort.TabIndex = 5;
            this.labelComPort.Text = "COM Port";
            // 
            // checkBoxTraceOn
            // 
            this.checkBoxTraceOn.AutoSize = true;
            this.checkBoxTraceOn.Location = new System.Drawing.Point(15, 119);
            this.checkBoxTraceOn.Name = "checkBoxTraceOn";
            this.checkBoxTraceOn.Size = new System.Drawing.Size(69, 17);
            this.checkBoxTraceOn.TabIndex = 6;
            this.checkBoxTraceOn.Text = "Trace on";
            this.checkBoxTraceOn.UseVisualStyleBackColor = true;
            // 
            // comboBoxComPort
            // 
            this.comboBoxComPort.FormattingEnabled = true;
            this.comboBoxComPort.Location = new System.Drawing.Point(76, 61);
            this.comboBoxComPort.Name = "comboBoxComPort";
            this.comboBoxComPort.Size = new System.Drawing.Size(110, 21);
            this.comboBoxComPort.TabIndex = 7;
            // 
            // checkBoxShowController
            // 
            this.checkBoxShowController.AutoSize = true;
            this.checkBoxShowController.Location = new System.Drawing.Point(15, 142);
            this.checkBoxShowController.Name = "checkBoxShowController";
            this.checkBoxShowController.Size = new System.Drawing.Size(100, 17);
            this.checkBoxShowController.TabIndex = 8;
            this.checkBoxShowController.Text = "Show Controller";
            this.checkBoxShowController.UseVisualStyleBackColor = true;
            // 
            // labelInfo
            // 
            this.labelInfo.AutoSize = true;
            this.labelInfo.Location = new System.Drawing.Point(12, 40);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(60, 13);
            this.labelInfo.TabIndex = 9;
            this.labelInfo.Text = "Version 0.1";
            // 
            // comboBoxBaudRate
            // 
            this.comboBoxBaudRate.FormattingEnabled = true;
            this.comboBoxBaudRate.Location = new System.Drawing.Point(76, 92);
            this.comboBoxBaudRate.Name = "comboBoxBaudRate";
            this.comboBoxBaudRate.Size = new System.Drawing.Size(110, 21);
            this.comboBoxBaudRate.TabIndex = 11;
            // 
            // labelBaudRate
            // 
            this.labelBaudRate.AutoSize = true;
            this.labelBaudRate.Location = new System.Drawing.Point(12, 95);
            this.labelBaudRate.Name = "labelBaudRate";
            this.labelBaudRate.Size = new System.Drawing.Size(53, 13);
            this.labelBaudRate.TabIndex = 10;
            this.labelBaudRate.Text = "Baud rate";
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(299, 171);
            this.Controls.Add(this.comboBoxBaudRate);
            this.Controls.Add(this.labelBaudRate);
            this.Controls.Add(this.labelInfo);
            this.Controls.Add(this.checkBoxShowController);
            this.Controls.Add(this.comboBoxComPort);
            this.Controls.Add(this.checkBoxTraceOn);
            this.Controls.Add(this.labelComPort);
            this.Controls.Add(this.imageASCOM);
            this.Controls.Add(this.labelHeading);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(315, 210);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(315, 210);
            this.Name = "SetupDialogForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "McMullenTechFocuser Setup";
            this.Load += new System.EventHandler(this.SetupDialogForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.imageASCOM)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label labelHeading;
        private System.Windows.Forms.PictureBox imageASCOM;
        private System.Windows.Forms.Label labelComPort;
        private System.Windows.Forms.CheckBox checkBoxTraceOn;
        private System.Windows.Forms.ComboBox comboBoxComPort;
        private System.Windows.Forms.CheckBox checkBoxShowController;
        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.ComboBox comboBoxBaudRate;
        private System.Windows.Forms.Label labelBaudRate;
    }
}