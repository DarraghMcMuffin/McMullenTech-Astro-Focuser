namespace ASCOM.McMullenTechFocuser
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.buttonChoose = new System.Windows.Forms.Button();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.labelDriverId = new System.Windows.Forms.Label();
            this.textBoxCurrentPosition = new System.Windows.Forms.TextBox();
            this.labelCurrentPosition = new System.Windows.Forms.Label();
            this.textBoxConsoleLog = new System.Windows.Forms.TextBox();
            this.textBoxCommand = new System.Windows.Forms.TextBox();
            this.buttonCommandSend = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonMoveInSmall = new System.Windows.Forms.Button();
            this.buttonMoveInLarge = new System.Windows.Forms.Button();
            this.buttonMoveOutSmall = new System.Windows.Forms.Button();
            this.buttonMoveOutLarge = new System.Windows.Forms.Button();
            this.buttonEStop = new System.Windows.Forms.Button();
            this.numericUpDownMoveRate = new System.Windows.Forms.NumericUpDown();
            this.labelMoveRate = new System.Windows.Forms.Label();
            this.numericUpDownGoToPosition = new System.Windows.Forms.NumericUpDown();
            this.labelGoToPosition = new System.Windows.Forms.Label();
            this.buttonGoTo = new System.Windows.Forms.Button();
            this.labelStepSize = new System.Windows.Forms.Label();
            this.numericUpDownStepSize = new System.Windows.Forms.NumericUpDown();
            this.labelStatus = new System.Windows.Forms.Label();
            this.labelConsole = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMoveRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownGoToPosition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStepSize)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonChoose
            // 
            this.buttonChoose.BackColor = System.Drawing.Color.White;
            this.buttonChoose.ForeColor = System.Drawing.Color.Black;
            this.buttonChoose.Location = new System.Drawing.Point(235, 20);
            this.buttonChoose.Name = "buttonChoose";
            this.buttonChoose.Size = new System.Drawing.Size(72, 23);
            this.buttonChoose.TabIndex = 0;
            this.buttonChoose.Text = "Choose";
            this.buttonChoose.UseVisualStyleBackColor = false;
            this.buttonChoose.Click += new System.EventHandler(this.buttonChoose_Click);
            // 
            // buttonConnect
            // 
            this.buttonConnect.ForeColor = System.Drawing.Color.Black;
            this.buttonConnect.Location = new System.Drawing.Point(313, 20);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(72, 23);
            this.buttonConnect.TabIndex = 1;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // labelDriverId
            // 
            this.labelDriverId.BackColor = System.Drawing.Color.White;
            this.labelDriverId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelDriverId.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ASCOM.McMullenTechFocuser.Properties.Settings.Default, "DriverId", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.labelDriverId.ForeColor = System.Drawing.Color.Black;
            this.labelDriverId.Location = new System.Drawing.Point(15, 20);
            this.labelDriverId.Name = "labelDriverId";
            this.labelDriverId.Size = new System.Drawing.Size(210, 21);
            this.labelDriverId.TabIndex = 2;
            this.labelDriverId.Text = global::ASCOM.McMullenTechFocuser.Properties.Settings.Default.DriverId;
            this.labelDriverId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxCurrentPosition
            // 
            this.textBoxCurrentPosition.ForeColor = System.Drawing.Color.Black;
            this.textBoxCurrentPosition.Location = new System.Drawing.Point(73, 84);
            this.textBoxCurrentPosition.Name = "textBoxCurrentPosition";
            this.textBoxCurrentPosition.ReadOnly = true;
            this.textBoxCurrentPosition.Size = new System.Drawing.Size(100, 20);
            this.textBoxCurrentPosition.TabIndex = 3;
            // 
            // labelCurrentPosition
            // 
            this.labelCurrentPosition.AutoSize = true;
            this.labelCurrentPosition.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.labelCurrentPosition.Location = new System.Drawing.Point(12, 85);
            this.labelCurrentPosition.Name = "labelCurrentPosition";
            this.labelCurrentPosition.Size = new System.Drawing.Size(44, 13);
            this.labelCurrentPosition.TabIndex = 6;
            this.labelCurrentPosition.Text = "Position";
            // 
            // textBoxConsoleLog
            // 
            this.textBoxConsoleLog.Location = new System.Drawing.Point(12, 274);
            this.textBoxConsoleLog.Multiline = true;
            this.textBoxConsoleLog.Name = "textBoxConsoleLog";
            this.textBoxConsoleLog.ReadOnly = true;
            this.textBoxConsoleLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxConsoleLog.Size = new System.Drawing.Size(291, 86);
            this.textBoxConsoleLog.TabIndex = 9;
            // 
            // textBoxCommand
            // 
            this.textBoxCommand.AcceptsReturn = true;
            this.textBoxCommand.Location = new System.Drawing.Point(12, 366);
            this.textBoxCommand.Name = "textBoxCommand";
            this.textBoxCommand.Size = new System.Drawing.Size(234, 20);
            this.textBoxCommand.TabIndex = 10;
            // 
            // buttonCommandSend
            // 
            this.buttonCommandSend.ForeColor = System.Drawing.Color.Black;
            this.buttonCommandSend.Location = new System.Drawing.Point(252, 364);
            this.buttonCommandSend.Name = "buttonCommandSend";
            this.buttonCommandSend.Size = new System.Drawing.Size(51, 23);
            this.buttonCommandSend.TabIndex = 11;
            this.buttonCommandSend.Text = "Send";
            this.buttonCommandSend.UseVisualStyleBackColor = true;
            this.buttonCommandSend.Click += new System.EventHandler(this.buttonCommandSend_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.ForeColor = System.Drawing.Color.Black;
            this.buttonStop.Location = new System.Drawing.Point(106, 125);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(49, 23);
            this.buttonStop.TabIndex = 12;
            this.buttonStop.Text = "STOP";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // buttonMoveInSmall
            // 
            this.buttonMoveInSmall.ForeColor = System.Drawing.Color.Black;
            this.buttonMoveInSmall.Location = new System.Drawing.Point(57, 125);
            this.buttonMoveInSmall.Name = "buttonMoveInSmall";
            this.buttonMoveInSmall.Size = new System.Drawing.Size(30, 23);
            this.buttonMoveInSmall.TabIndex = 13;
            this.buttonMoveInSmall.Text = "<";
            this.buttonMoveInSmall.UseVisualStyleBackColor = true;
            this.buttonMoveInSmall.Click += new System.EventHandler(this.buttonMoveInSmall_Click);
            // 
            // buttonMoveInLarge
            // 
            this.buttonMoveInLarge.ForeColor = System.Drawing.Color.Black;
            this.buttonMoveInLarge.Location = new System.Drawing.Point(21, 125);
            this.buttonMoveInLarge.Name = "buttonMoveInLarge";
            this.buttonMoveInLarge.Size = new System.Drawing.Size(30, 23);
            this.buttonMoveInLarge.TabIndex = 14;
            this.buttonMoveInLarge.Text = "<<";
            this.buttonMoveInLarge.UseVisualStyleBackColor = true;
            this.buttonMoveInLarge.Click += new System.EventHandler(this.buttonMoveInLarge_Click);
            // 
            // buttonMoveOutSmall
            // 
            this.buttonMoveOutSmall.ForeColor = System.Drawing.Color.Black;
            this.buttonMoveOutSmall.Location = new System.Drawing.Point(174, 125);
            this.buttonMoveOutSmall.Name = "buttonMoveOutSmall";
            this.buttonMoveOutSmall.Size = new System.Drawing.Size(30, 23);
            this.buttonMoveOutSmall.TabIndex = 15;
            this.buttonMoveOutSmall.Text = ">";
            this.buttonMoveOutSmall.UseVisualStyleBackColor = true;
            // 
            // buttonMoveOutLarge
            // 
            this.buttonMoveOutLarge.ForeColor = System.Drawing.Color.Black;
            this.buttonMoveOutLarge.Location = new System.Drawing.Point(212, 125);
            this.buttonMoveOutLarge.Name = "buttonMoveOutLarge";
            this.buttonMoveOutLarge.Size = new System.Drawing.Size(30, 23);
            this.buttonMoveOutLarge.TabIndex = 16;
            this.buttonMoveOutLarge.Text = ">>";
            this.buttonMoveOutLarge.UseVisualStyleBackColor = true;
            this.buttonMoveOutLarge.Click += new System.EventHandler(this.buttonMoveOutLarge_Click);
            // 
            // buttonEStop
            // 
            this.buttonEStop.BackColor = System.Drawing.Color.OrangeRed;
            this.buttonEStop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.buttonEStop.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.buttonEStop.FlatAppearance.BorderSize = 0;
            this.buttonEStop.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Maroon;
            this.buttonEStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonEStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonEStop.ForeColor = System.Drawing.Color.Black;
            this.buttonEStop.Location = new System.Drawing.Point(266, 125);
            this.buttonEStop.Name = "buttonEStop";
            this.buttonEStop.Size = new System.Drawing.Size(55, 23);
            this.buttonEStop.TabIndex = 17;
            this.buttonEStop.Text = "E-STOP";
            this.buttonEStop.UseVisualStyleBackColor = false;
            this.buttonEStop.Click += new System.EventHandler(this.buttonEStop_Click);
            // 
            // numericUpDownMoveRate
            // 
            this.numericUpDownMoveRate.Location = new System.Drawing.Point(73, 214);
            this.numericUpDownMoveRate.Name = "numericUpDownMoveRate";
            this.numericUpDownMoveRate.Size = new System.Drawing.Size(78, 20);
            this.numericUpDownMoveRate.TabIndex = 18;
            // 
            // labelMoveRate
            // 
            this.labelMoveRate.AutoSize = true;
            this.labelMoveRate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.labelMoveRate.Location = new System.Drawing.Point(29, 217);
            this.labelMoveRate.Name = "labelMoveRate";
            this.labelMoveRate.Size = new System.Drawing.Size(30, 13);
            this.labelMoveRate.TabIndex = 19;
            this.labelMoveRate.Text = "Rate";
            // 
            // numericUpDownGoToPosition
            // 
            this.numericUpDownGoToPosition.Location = new System.Drawing.Point(77, 176);
            this.numericUpDownGoToPosition.Name = "numericUpDownGoToPosition";
            this.numericUpDownGoToPosition.Size = new System.Drawing.Size(78, 20);
            this.numericUpDownGoToPosition.TabIndex = 20;
            // 
            // labelGoToPosition
            // 
            this.labelGoToPosition.AutoSize = true;
            this.labelGoToPosition.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.labelGoToPosition.Location = new System.Drawing.Point(26, 179);
            this.labelGoToPosition.Name = "labelGoToPosition";
            this.labelGoToPosition.Size = new System.Drawing.Size(29, 13);
            this.labelGoToPosition.TabIndex = 21;
            this.labelGoToPosition.Text = "Dest";
            // 
            // buttonGoTo
            // 
            this.buttonGoTo.ForeColor = System.Drawing.Color.Black;
            this.buttonGoTo.Location = new System.Drawing.Point(161, 176);
            this.buttonGoTo.Name = "buttonGoTo";
            this.buttonGoTo.Size = new System.Drawing.Size(49, 23);
            this.buttonGoTo.TabIndex = 22;
            this.buttonGoTo.Text = "GoTo";
            this.buttonGoTo.UseVisualStyleBackColor = true;
            // 
            // labelStepSize
            // 
            this.labelStepSize.AutoSize = true;
            this.labelStepSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.labelStepSize.Location = new System.Drawing.Point(194, 85);
            this.labelStepSize.Name = "labelStepSize";
            this.labelStepSize.Size = new System.Drawing.Size(50, 13);
            this.labelStepSize.TabIndex = 24;
            this.labelStepSize.Text = "Step size";
            // 
            // numericUpDownStepSize
            // 
            this.numericUpDownStepSize.Location = new System.Drawing.Point(266, 85);
            this.numericUpDownStepSize.Name = "numericUpDownStepSize";
            this.numericUpDownStepSize.Size = new System.Drawing.Size(78, 20);
            this.numericUpDownStepSize.TabIndex = 23;
            this.numericUpDownStepSize.ValueChanged += new System.EventHandler(this.numericUpDownStepSize_ValueChanged);
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.labelStatus.Location = new System.Drawing.Point(263, 179);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(69, 17);
            this.labelStatus.TabIndex = 25;
            this.labelStatus.Text = "At Home";
            // 
            // labelConsole
            // 
            this.labelConsole.AutoSize = true;
            this.labelConsole.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.labelConsole.Location = new System.Drawing.Point(14, 254);
            this.labelConsole.Name = "labelConsole";
            this.labelConsole.Size = new System.Drawing.Size(55, 13);
            this.labelConsole.TabIndex = 26;
            this.labelConsole.Text = "Serial Port";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(404, 411);
            this.Controls.Add(this.labelConsole);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.labelStepSize);
            this.Controls.Add(this.numericUpDownStepSize);
            this.Controls.Add(this.buttonGoTo);
            this.Controls.Add(this.labelGoToPosition);
            this.Controls.Add(this.numericUpDownGoToPosition);
            this.Controls.Add(this.labelMoveRate);
            this.Controls.Add(this.numericUpDownMoveRate);
            this.Controls.Add(this.buttonEStop);
            this.Controls.Add(this.buttonMoveOutLarge);
            this.Controls.Add(this.buttonMoveOutSmall);
            this.Controls.Add(this.buttonMoveInLarge);
            this.Controls.Add(this.buttonMoveInSmall);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.buttonCommandSend);
            this.Controls.Add(this.textBoxCommand);
            this.Controls.Add(this.textBoxConsoleLog);
            this.Controls.Add(this.labelCurrentPosition);
            this.Controls.Add(this.textBoxCurrentPosition);
            this.Controls.Add(this.labelDriverId);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.buttonChoose);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(420, 450);
            this.MinimumSize = new System.Drawing.Size(420, 450);
            this.Name = "MainWindow";
            this.Text = "McMullenTech Focuser Control";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMoveRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownGoToPosition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStepSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonChoose;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Label labelDriverId;
        private System.Windows.Forms.TextBox textBoxCurrentPosition;
        private System.Windows.Forms.Label labelCurrentPosition;
        private System.Windows.Forms.TextBox textBoxConsoleLog;
        private System.Windows.Forms.TextBox textBoxCommand;
        private System.Windows.Forms.Button buttonCommandSend;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Button buttonMoveInSmall;
        private System.Windows.Forms.Button buttonMoveInLarge;
        private System.Windows.Forms.Button buttonMoveOutSmall;
        private System.Windows.Forms.Button buttonMoveOutLarge;
        private System.Windows.Forms.Button buttonEStop;
        private System.Windows.Forms.NumericUpDown numericUpDownMoveRate;
        private System.Windows.Forms.Label labelMoveRate;
        private System.Windows.Forms.NumericUpDown numericUpDownGoToPosition;
        private System.Windows.Forms.Label labelGoToPosition;
        private System.Windows.Forms.Button buttonGoTo;
        private System.Windows.Forms.Label labelStepSize;
        private System.Windows.Forms.NumericUpDown numericUpDownStepSize;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Label labelConsole;
    }
}

