namespace Alchemist
{
    partial class workDetailMotionfrm
    {
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(workDetailMotionfrm));
            this.panelFEED = new System.Windows.Forms.Panel();
            this.btnDOUBLE_MOTION = new System.Windows.Forms.Button();
            this.lblSTRIP = new System.Windows.Forms.Label();
            this.panelClose = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.panelFEED.SuspendLayout();
            this.panelClose.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelFEED
            // 
            this.panelFEED.BackColor = System.Drawing.Color.Black;
            this.panelFEED.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelFEED.Controls.Add(this.btnDOUBLE_MOTION);
            this.panelFEED.Controls.Add(this.lblSTRIP);
            this.panelFEED.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.panelFEED, "panelFEED");
            this.panelFEED.Name = "panelFEED";
            // 
            // btnDOUBLE_MOTION
            // 
            this.btnDOUBLE_MOTION.BackColor = System.Drawing.Color.Red;
            resources.ApplyResources(this.btnDOUBLE_MOTION, "btnDOUBLE_MOTION");
            this.btnDOUBLE_MOTION.ForeColor = System.Drawing.Color.Black;
            this.btnDOUBLE_MOTION.Name = "btnDOUBLE_MOTION";
            this.btnDOUBLE_MOTION.UseVisualStyleBackColor = false;
            // 
            // lblSTRIP
            // 
            this.lblSTRIP.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.lblSTRIP, "lblSTRIP");
            this.lblSTRIP.Name = "lblSTRIP";
            // 
            // panelClose
            // 
            this.panelClose.BackColor = System.Drawing.Color.Black;
            this.panelClose.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelClose.Controls.Add(this.btnClose);
            resources.ApplyResources(this.panelClose, "panelClose");
            this.panelClose.Name = "panelClose";
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // workDetailMotionfrm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panelClose);
            this.Controls.Add(this.panelFEED);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "workDetailMotionfrm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.workDetailMotionfrm_FormClosing);
            this.panelFEED.ResumeLayout(false);
            this.panelClose.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelFEED;
        private System.Windows.Forms.Label lblSTRIP;
        private System.Windows.Forms.Button btnDOUBLE_MOTION;
        private System.Windows.Forms.Panel panelClose;
        private System.Windows.Forms.Button btnClose;



    }
}