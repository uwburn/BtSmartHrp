namespace BtSmartHrp
{
    partial class FrmBtSmartHrp
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
            this.lbBpmLabel = new System.Windows.Forms.Label();
            this.lbBpm = new System.Windows.Forms.Label();
            this.btnStartStop = new System.Windows.Forms.Button();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.nudInitDelay = new System.Windows.Forms.NumericUpDown();
            this.nudCharacteristic = new System.Windows.Forms.NumericUpDown();
            this.lbCharacteristic = new System.Windows.Forms.Label();
            this.lbInitDelay = new System.Windows.Forms.Label();
            this.cbDevice = new System.Windows.Forms.ComboBox();
            this.lbDevice = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudInitDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCharacteristic)).BeginInit();
            this.SuspendLayout();
            // 
            // lbBpmLabel
            // 
            this.lbBpmLabel.Location = new System.Drawing.Point(12, 16);
            this.lbBpmLabel.Name = "lbBpmLabel";
            this.lbBpmLabel.Size = new System.Drawing.Size(143, 23);
            this.lbBpmLabel.TabIndex = 0;
            this.lbBpmLabel.Text = "BPM";
            this.lbBpmLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbBpm
            // 
            this.lbBpm.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbBpm.Location = new System.Drawing.Point(12, 39);
            this.lbBpm.Name = "lbBpm";
            this.lbBpm.Size = new System.Drawing.Size(143, 69);
            this.lbBpm.TabIndex = 1;
            this.lbBpm.Text = "--";
            this.lbBpm.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnStartStop
            // 
            this.btnStartStop.Location = new System.Drawing.Point(12, 294);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(130, 23);
            this.btnStartStop.TabIndex = 2;
            this.btnStartStop.Text = "Start";
            this.btnStartStop.UseVisualStyleBackColor = true;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            // 
            // tbLog
            // 
            this.tbLog.Location = new System.Drawing.Point(161, 12);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ReadOnly = true;
            this.tbLog.Size = new System.Drawing.Size(382, 305);
            this.tbLog.TabIndex = 3;
            // 
            // nudInitDelay
            // 
            this.nudInitDelay.Location = new System.Drawing.Point(12, 249);
            this.nudInitDelay.Name = "nudInitDelay";
            this.nudInitDelay.Size = new System.Drawing.Size(130, 20);
            this.nudInitDelay.TabIndex = 4;
            // 
            // nudCharacteristic
            // 
            this.nudCharacteristic.Location = new System.Drawing.Point(12, 203);
            this.nudCharacteristic.Name = "nudCharacteristic";
            this.nudCharacteristic.Size = new System.Drawing.Size(130, 20);
            this.nudCharacteristic.TabIndex = 5;
            // 
            // lbCharacteristic
            // 
            this.lbCharacteristic.AutoSize = true;
            this.lbCharacteristic.Location = new System.Drawing.Point(12, 187);
            this.lbCharacteristic.Name = "lbCharacteristic";
            this.lbCharacteristic.Size = new System.Drawing.Size(71, 13);
            this.lbCharacteristic.TabIndex = 6;
            this.lbCharacteristic.Text = "Characteristic";
            // 
            // lbInitDelay
            // 
            this.lbInitDelay.AutoSize = true;
            this.lbInitDelay.Location = new System.Drawing.Point(12, 233);
            this.lbInitDelay.Name = "lbInitDelay";
            this.lbInitDelay.Size = new System.Drawing.Size(49, 13);
            this.lbInitDelay.TabIndex = 7;
            this.lbInitDelay.Text = "Init delay";
            // 
            // cbDevice
            // 
            this.cbDevice.DisplayMember = "Name";
            this.cbDevice.FormattingEnabled = true;
            this.cbDevice.Location = new System.Drawing.Point(12, 152);
            this.cbDevice.Name = "cbDevice";
            this.cbDevice.Size = new System.Drawing.Size(130, 21);
            this.cbDevice.TabIndex = 8;
            this.cbDevice.ValueMember = "DeviceInfo";
            // 
            // lbDevice
            // 
            this.lbDevice.AutoSize = true;
            this.lbDevice.Location = new System.Drawing.Point(12, 136);
            this.lbDevice.Name = "lbDevice";
            this.lbDevice.Size = new System.Drawing.Size(41, 13);
            this.lbDevice.TabIndex = 9;
            this.lbDevice.Text = "Device";
            // 
            // FrmBtSmartHrp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(555, 329);
            this.Controls.Add(this.lbDevice);
            this.Controls.Add(this.cbDevice);
            this.Controls.Add(this.lbInitDelay);
            this.Controls.Add(this.lbCharacteristic);
            this.Controls.Add(this.nudCharacteristic);
            this.Controls.Add(this.nudInitDelay);
            this.Controls.Add(this.tbLog);
            this.Controls.Add(this.btnStartStop);
            this.Controls.Add(this.lbBpm);
            this.Controls.Add(this.lbBpmLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmBtSmartHrp";
            this.Text = "Bluetooth Smart HRP";
            this.Load += new System.EventHandler(this.FrmBtSmartHrp_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudInitDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCharacteristic)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbBpmLabel;
        private System.Windows.Forms.Label lbBpm;
        private System.Windows.Forms.Button btnStartStop;
        private System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.NumericUpDown nudInitDelay;
        private System.Windows.Forms.NumericUpDown nudCharacteristic;
        private System.Windows.Forms.Label lbCharacteristic;
        private System.Windows.Forms.Label lbInitDelay;
        private System.Windows.Forms.ComboBox cbDevice;
        private System.Windows.Forms.Label lbDevice;
    }
}

