using System;
namespace Lucid.Server
{
    partial class MainForm
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
            if (lucidServer != null)
            {
                // What if this does not get called on exception?
                try
                {
                    lucidServer.Stop();
                }
                catch (Exception ex)
                {
                    Inv.Common.Exceptions.ExceptionDialog(ex.Message);
                }
                lucidServer = null;
            }

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.label1 = new System.Windows.Forms.Label();
            this.lsbConnected = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnOpenScene = new System.Windows.Forms.ToolStripButton();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.grbCamera = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtLookAtZ = new System.Windows.Forms.TextBox();
            this.txtLookAtY = new System.Windows.Forms.TextBox();
            this.txtLookAtX = new System.Windows.Forms.TextBox();
            this.txtOriginZ = new System.Windows.Forms.TextBox();
            this.txtOriginY = new System.Windows.Forms.TextBox();
            this.txtOriginX = new System.Windows.Forms.TextBox();
            this.grbFillColor = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtFillColorB = new System.Windows.Forms.TextBox();
            this.txtFillColorG = new System.Windows.Forms.TextBox();
            this.txtFillColorR = new System.Windows.Forms.TextBox();
            this.pbResult = new System.Windows.Forms.PictureBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tslTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslSplit = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslLocalAddress = new System.Windows.Forms.ToolStripStatusLabel();
            this.txtJobHeight = new System.Windows.Forms.TextBox();
            this.txtJobWidth = new System.Windows.Forms.TextBox();
            this.grbJobSize = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            this.grbCamera.SuspendLayout();
            this.grbFillColor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbResult)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.grbJobSize.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-3, 488);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Log";
            // 
            // lsbConnected
            // 
            this.lsbConnected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lsbConnected.FormattingEnabled = true;
            this.lsbConnected.Location = new System.Drawing.Point(501, 28);
            this.lsbConnected.Name = "lsbConnected";
            this.lsbConnected.Size = new System.Drawing.Size(167, 95);
            this.lsbConnected.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(368, 1);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Connected";
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.Enabled = false;
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnStart.Location = new System.Drawing.Point(521, 427);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(135, 26);
            this.btnStart.TabIndex = 6;
            this.btnStart.Text = "Start rendering";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnOpenScene});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(677, 25);
            this.toolStrip1.TabIndex = 7;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnOpenScene
            // 
            this.btnOpenScene.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnOpenScene.Image = ((System.Drawing.Image)(resources.GetObject("btnOpenScene.Image")));
            this.btnOpenScene.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOpenScene.Name = "btnOpenScene";
            this.btnOpenScene.Size = new System.Drawing.Size(23, 22);
            this.btnOpenScene.Text = "Open scene...";
            this.btnOpenScene.Click += new System.EventHandler(this.btnOpenScene_Click);
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.Location = new System.Drawing.Point(4, 504);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLog.Size = new System.Drawing.Size(664, 126);
            this.txtLog.TabIndex = 8;
            // 
            // grbCamera
            // 
            this.grbCamera.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.grbCamera.Controls.Add(this.label4);
            this.grbCamera.Controls.Add(this.label3);
            this.grbCamera.Controls.Add(this.txtLookAtZ);
            this.grbCamera.Controls.Add(this.txtLookAtY);
            this.grbCamera.Controls.Add(this.txtLookAtX);
            this.grbCamera.Controls.Add(this.txtOriginZ);
            this.grbCamera.Controls.Add(this.txtOriginY);
            this.grbCamera.Controls.Add(this.txtOriginX);
            this.grbCamera.Location = new System.Drawing.Point(501, 134);
            this.grbCamera.Name = "grbCamera";
            this.grbCamera.Size = new System.Drawing.Size(167, 127);
            this.grbCamera.TabIndex = 9;
            this.grbCamera.TabStop = false;
            this.grbCamera.Text = "Camera";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Look at:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Origin:";
            // 
            // txtLookAtZ
            // 
            this.txtLookAtZ.Location = new System.Drawing.Point(102, 90);
            this.txtLookAtZ.Name = "txtLookAtZ";
            this.txtLookAtZ.Size = new System.Drawing.Size(42, 20);
            this.txtLookAtZ.TabIndex = 5;
            // 
            // txtLookAtY
            // 
            this.txtLookAtY.Location = new System.Drawing.Point(54, 90);
            this.txtLookAtY.Name = "txtLookAtY";
            this.txtLookAtY.Size = new System.Drawing.Size(42, 20);
            this.txtLookAtY.TabIndex = 4;
            // 
            // txtLookAtX
            // 
            this.txtLookAtX.Location = new System.Drawing.Point(6, 90);
            this.txtLookAtX.Name = "txtLookAtX";
            this.txtLookAtX.Size = new System.Drawing.Size(42, 20);
            this.txtLookAtX.TabIndex = 3;
            // 
            // txtOriginZ
            // 
            this.txtOriginZ.Location = new System.Drawing.Point(102, 42);
            this.txtOriginZ.Name = "txtOriginZ";
            this.txtOriginZ.Size = new System.Drawing.Size(42, 20);
            this.txtOriginZ.TabIndex = 2;
            // 
            // txtOriginY
            // 
            this.txtOriginY.Location = new System.Drawing.Point(54, 42);
            this.txtOriginY.Name = "txtOriginY";
            this.txtOriginY.Size = new System.Drawing.Size(42, 20);
            this.txtOriginY.TabIndex = 1;
            // 
            // txtOriginX
            // 
            this.txtOriginX.Location = new System.Drawing.Point(6, 42);
            this.txtOriginX.Name = "txtOriginX";
            this.txtOriginX.Size = new System.Drawing.Size(42, 20);
            this.txtOriginX.TabIndex = 0;
            // 
            // grbFillColor
            // 
            this.grbFillColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.grbFillColor.Controls.Add(this.label6);
            this.grbFillColor.Controls.Add(this.txtFillColorB);
            this.grbFillColor.Controls.Add(this.txtFillColorG);
            this.grbFillColor.Controls.Add(this.txtFillColorR);
            this.grbFillColor.Location = new System.Drawing.Point(501, 267);
            this.grbFillColor.Name = "grbFillColor";
            this.grbFillColor.Size = new System.Drawing.Size(167, 80);
            this.grbFillColor.TabIndex = 10;
            this.grbFillColor.TabStop = false;
            this.grbFillColor.Text = "Background color";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 26);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "RGB:";
            // 
            // txtFillColorB
            // 
            this.txtFillColorB.Location = new System.Drawing.Point(102, 42);
            this.txtFillColorB.Name = "txtFillColorB";
            this.txtFillColorB.Size = new System.Drawing.Size(42, 20);
            this.txtFillColorB.TabIndex = 2;
            this.txtFillColorB.Text = "30";
            // 
            // txtFillColorG
            // 
            this.txtFillColorG.Location = new System.Drawing.Point(54, 42);
            this.txtFillColorG.Name = "txtFillColorG";
            this.txtFillColorG.Size = new System.Drawing.Size(42, 20);
            this.txtFillColorG.TabIndex = 1;
            this.txtFillColorG.Text = "30";
            // 
            // txtFillColorR
            // 
            this.txtFillColorR.Location = new System.Drawing.Point(6, 42);
            this.txtFillColorR.Name = "txtFillColorR";
            this.txtFillColorR.Size = new System.Drawing.Size(42, 20);
            this.txtFillColorR.TabIndex = 0;
            this.txtFillColorR.Text = "30";
            // 
            // pbResult
            // 
            this.pbResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pbResult.Location = new System.Drawing.Point(4, 28);
            this.pbResult.Name = "pbResult";
            this.pbResult.Size = new System.Drawing.Size(491, 457);
            this.pbResult.TabIndex = 11;
            this.pbResult.TabStop = false;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(521, 459);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(135, 26);
            this.btnSave.TabIndex = 12;
            this.btnSave.Text = "Save image...";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslTime,
            this.tslSplit,
            this.tslLocalAddress});
            this.statusStrip1.Location = new System.Drawing.Point(0, 633);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.statusStrip1.Size = new System.Drawing.Size(677, 22);
            this.statusStrip1.TabIndex = 13;
            this.statusStrip1.Text = "statusStrip";
            // 
            // tslTime
            // 
            this.tslTime.Name = "tslTime";
            this.tslTime.Size = new System.Drawing.Size(31, 17);
            this.tslTime.Text = "time";
            // 
            // tslSplit
            // 
            this.tslSplit.Name = "tslSplit";
            this.tslSplit.Size = new System.Drawing.Size(16, 17);
            this.tslSplit.Text = " | ";
            // 
            // tslLocalAddress
            // 
            this.tslLocalAddress.Name = "tslLocalAddress";
            this.tslLocalAddress.Size = new System.Drawing.Size(71, 17);
            this.tslLocalAddress.Text = "Listening at:";
            // 
            // txtJobHeight
            // 
            this.txtJobHeight.Location = new System.Drawing.Point(74, 19);
            this.txtJobHeight.Name = "txtJobHeight";
            this.txtJobHeight.Size = new System.Drawing.Size(42, 20);
            this.txtJobHeight.TabIndex = 15;
            this.txtJobHeight.Text = "80";
            // 
            // txtJobWidth
            // 
            this.txtJobWidth.Location = new System.Drawing.Point(8, 19);
            this.txtJobWidth.Name = "txtJobWidth";
            this.txtJobWidth.Size = new System.Drawing.Size(42, 20);
            this.txtJobWidth.TabIndex = 14;
            this.txtJobWidth.Text = "50";
            // 
            // grbJobSize
            // 
            this.grbJobSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.grbJobSize.Controls.Add(this.label5);
            this.grbJobSize.Controls.Add(this.txtJobWidth);
            this.grbJobSize.Controls.Add(this.txtJobHeight);
            this.grbJobSize.Location = new System.Drawing.Point(503, 353);
            this.grbJobSize.Name = "grbJobSize";
            this.grbJobSize.Size = new System.Drawing.Size(165, 51);
            this.grbJobSize.TabIndex = 16;
            this.grbJobSize.TabStop = false;
            this.grbJobSize.Text = "Task size";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(56, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(12, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "x";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(677, 655);
            this.Controls.Add(this.grbJobSize);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.pbResult);
            this.Controls.Add(this.grbFillColor);
            this.Controls.Add(this.grbCamera);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lsbConnected);
            this.Controls.Add(this.label1);
            this.Name = "MainForm";
            this.Text = "Lucid";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.grbCamera.ResumeLayout(false);
            this.grbCamera.PerformLayout();
            this.grbFillColor.ResumeLayout(false);
            this.grbFillColor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbResult)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.grbJobSize.ResumeLayout(false);
            this.grbJobSize.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lsbConnected;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnOpenScene;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.GroupBox grbCamera;
        private System.Windows.Forms.TextBox txtOriginX;
        private System.Windows.Forms.TextBox txtOriginZ;
        private System.Windows.Forms.TextBox txtOriginY;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtLookAtZ;
        private System.Windows.Forms.TextBox txtLookAtY;
        private System.Windows.Forms.TextBox txtLookAtX;
        private System.Windows.Forms.GroupBox grbFillColor;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtFillColorB;
        private System.Windows.Forms.TextBox txtFillColorG;
        private System.Windows.Forms.TextBox txtFillColorR;
        private System.Windows.Forms.PictureBox pbResult;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tslTime;
        private System.Windows.Forms.ToolStripStatusLabel tslLocalAddress;
        private System.Windows.Forms.ToolStripStatusLabel tslSplit;
        private System.Windows.Forms.TextBox txtJobHeight;
        private System.Windows.Forms.TextBox txtJobWidth;
        private System.Windows.Forms.GroupBox grbJobSize;
        private System.Windows.Forms.Label label5;
    }
}

