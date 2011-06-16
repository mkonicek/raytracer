namespace Lucid.Client
{
    partial class ClientForm
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
            lucidClient.Disconnect();
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
            this.txtLog = new System.Windows.Forms.TextBox();
            this.btnScanServers = new System.Windows.Forms.Button();
            this.grbServers = new System.Windows.Forms.GroupBox();
            this.lsbServers = new System.Windows.Forms.ListBox();
            this.lblServer = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.grbServers.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-2, 147);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Log";
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.Location = new System.Drawing.Point(4, 163);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLog.Size = new System.Drawing.Size(516, 141);
            this.txtLog.TabIndex = 3;
            // 
            // btnScanServers
            // 
            this.btnScanServers.Location = new System.Drawing.Point(12, 12);
            this.btnScanServers.Name = "btnScanServers";
            this.btnScanServers.Size = new System.Drawing.Size(117, 23);
            this.btnScanServers.TabIndex = 7;
            this.btnScanServers.Text = "Search for servers";
            this.btnScanServers.UseVisualStyleBackColor = true;
            this.btnScanServers.Click += new System.EventHandler(this.btnScanServers_Click);
            // 
            // grbServers
            // 
            this.grbServers.Controls.Add(this.lsbServers);
            this.grbServers.Location = new System.Drawing.Point(12, 41);
            this.grbServers.Name = "grbServers";
            this.grbServers.Size = new System.Drawing.Size(214, 95);
            this.grbServers.TabIndex = 8;
            this.grbServers.TabStop = false;
            this.grbServers.Text = "Servers available";
            // 
            // lsbServers
            // 
            this.lsbServers.FormattingEnabled = true;
            this.lsbServers.Location = new System.Drawing.Point(6, 19);
            this.lsbServers.Name = "lsbServers";
            this.lsbServers.Size = new System.Drawing.Size(202, 69);
            this.lsbServers.TabIndex = 0;
            this.lsbServers.SelectedIndexChanged += new System.EventHandler(this.lsbServers_SelectedIndexChanged);
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Location = new System.Drawing.Point(245, 41);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(41, 13);
            this.lblServer.TabIndex = 9;
            this.lblServer.Text = "Server:";
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(248, 60);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(170, 20);
            this.txtServer.TabIndex = 10;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(248, 86);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 11;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(527, 307);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.txtServer);
            this.Controls.Add(this.lblServer);
            this.Controls.Add(this.grbServers);
            this.Controls.Add(this.btnScanServers);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtLog);
            this.Name = "ClientForm";
            this.Text = "Lucid Client";
            this.grbServers.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button btnScanServers;
        private System.Windows.Forms.GroupBox grbServers;
        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.ListBox lsbServers;
    }
}

