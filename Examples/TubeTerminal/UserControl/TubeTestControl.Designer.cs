namespace TubeTerminal.UserControl
{
    partial class TubeTestControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.txtLocalId = new System.Windows.Forms.TextBox();
            this.txtClientSendData = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtClientSendMsg = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.txtLogWindow = new System.Windows.Forms.TextBox();
            this.btnStartStop = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblAddressLabel = new System.Windows.Forms.Label();
            this.lblClientServerTitle = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.timerUpdate = new System.Windows.Forms.Timer(this.components);
            this.lstConnections = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // txtLocalId
            // 
            this.txtLocalId.Location = new System.Drawing.Point(83, 24);
            this.txtLocalId.Name = "txtLocalId";
            this.txtLocalId.Size = new System.Drawing.Size(20, 20);
            this.txtLocalId.TabIndex = 25;
            this.txtLocalId.Text = "10";
            // 
            // txtClientSendData
            // 
            this.txtClientSendData.Location = new System.Drawing.Point(61, 158);
            this.txtClientSendData.Name = "txtClientSendData";
            this.txtClientSendData.Size = new System.Drawing.Size(317, 20);
            this.txtClientSendData.TabIndex = 24;
            this.txtClientSendData.Text = "This is a test message data";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 161);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 23;
            this.label2.Text = "Data";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 135);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "Msg ID";
            // 
            // txtClientSendMsg
            // 
            this.txtClientSendMsg.Location = new System.Drawing.Point(61, 132);
            this.txtClientSendMsg.Name = "txtClientSendMsg";
            this.txtClientSendMsg.Size = new System.Drawing.Size(159, 20);
            this.txtClientSendMsg.TabIndex = 21;
            this.txtClientSendMsg.Text = "TestMessage1";
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(23, 184);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(355, 23);
            this.btnSend.TabIndex = 20;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(83, 50);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(93, 20);
            this.txtAddress.TabIndex = 19;
            this.txtAddress.Text = "127.0.0.1:1222";
            // 
            // txtLogWindow
            // 
            this.txtLogWindow.BackColor = System.Drawing.SystemColors.Info;
            this.txtLogWindow.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLogWindow.Location = new System.Drawing.Point(23, 240);
            this.txtLogWindow.Multiline = true;
            this.txtLogWindow.Name = "txtLogWindow";
            this.txtLogWindow.ReadOnly = true;
            this.txtLogWindow.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLogWindow.Size = new System.Drawing.Size(355, 198);
            this.txtLogWindow.TabIndex = 18;
            this.txtLogWindow.WordWrap = false;
            // 
            // btnStartStop
            // 
            this.btnStartStop.Location = new System.Drawing.Point(23, 76);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(153, 23);
            this.btnStartStop.TabIndex = 17;
            this.btnStartStop.Text = "Open";
            this.btnStartStop.UseVisualStyleBackColor = true;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartClient_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(11, 113);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 26;
            this.label3.Text = "Broadcast";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 27;
            this.label4.Text = "Local ID";
            // 
            // lblAddressLabel
            // 
            this.lblAddressLabel.AutoSize = true;
            this.lblAddressLabel.Location = new System.Drawing.Point(20, 53);
            this.lblAddressLabel.Name = "lblAddressLabel";
            this.lblAddressLabel.Size = new System.Drawing.Size(59, 13);
            this.lblAddressLabel.TabIndex = 28;
            this.lblAddressLabel.Text = "Connect to";
            // 
            // lblClientServerTitle
            // 
            this.lblClientServerTitle.AutoSize = true;
            this.lblClientServerTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClientServerTitle.Location = new System.Drawing.Point(11, 4);
            this.lblClientServerTitle.Name = "lblClientServerTitle";
            this.lblClientServerTitle.Size = new System.Drawing.Size(131, 13);
            this.lblClientServerTitle.TabIndex = 29;
            this.lblClientServerTitle.Text = "Client/Server Address";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(11, 220);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(123, 13);
            this.label7.TabIndex = 30;
            this.label7.Text = "\'GetMessage\' output";
            // 
            // timerUpdate
            // 
            this.timerUpdate.Enabled = true;
            this.timerUpdate.Interval = 10;
            this.timerUpdate.Tick += new System.EventHandler(this.timerUpdate_Tick);
            // 
            // lstConnections
            // 
            this.lstConnections.FormattingEnabled = true;
            this.lstConnections.IntegralHeight = false;
            this.lstConnections.Location = new System.Drawing.Point(258, 24);
            this.lstConnections.Name = "lstConnections";
            this.lstConnections.Size = new System.Drawing.Size(120, 75);
            this.lstConnections.TabIndex = 31;
            // 
            // TubeTestControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lstConnections);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblClientServerTitle);
            this.Controls.Add(this.lblAddressLabel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtLocalId);
            this.Controls.Add(this.txtClientSendData);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtClientSendMsg);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.txtLogWindow);
            this.Controls.Add(this.btnStartStop);
            this.Name = "TubeTestControl";
            this.Size = new System.Drawing.Size(394, 449);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtLocalId;
        private System.Windows.Forms.TextBox txtClientSendData;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtClientSendMsg;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.TextBox txtLogWindow;
        private System.Windows.Forms.Button btnStartStop;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblAddressLabel;
        private System.Windows.Forms.Label lblClientServerTitle;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Timer timerUpdate;
        private System.Windows.Forms.ListBox lstConnections;
    }
}
