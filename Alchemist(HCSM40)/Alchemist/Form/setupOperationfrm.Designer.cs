namespace Alchemist
{
    partial class setupOperationfrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(setupOperationfrm));
            this.panelFEED = new System.Windows.Forms.Panel();
            this.btnLOAD = new System.Windows.Forms.Button();
            this.lblFEED = new System.Windows.Forms.Label();
            this.panelCLOSE = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.panelFEED.SuspendLayout();
            this.panelCLOSE.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelFEED
            // 
            this.panelFEED.BackColor = System.Drawing.Color.Black;
            this.panelFEED.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelFEED.Controls.Add(this.btnLOAD);
            this.panelFEED.Controls.Add(this.lblFEED);
            this.panelFEED.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.panelFEED, "panelFEED");
            this.panelFEED.Name = "panelFEED";
            // 
            // btnLOAD
            // 
            this.btnLOAD.BackColor = System.Drawing.Color.Gray;
            resources.ApplyResources(this.btnLOAD, "btnLOAD");
            this.btnLOAD.ForeColor = System.Drawing.Color.Black;
            this.btnLOAD.Name = "btnLOAD";
            this.btnLOAD.UseVisualStyleBackColor = false;
            // 
            // lblFEED
            // 
            this.lblFEED.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.lblFEED, "lblFEED");
            this.lblFEED.Name = "lblFEED";
            // 
            // panelCLOSE
            // 
            this.panelCLOSE.BackColor = System.Drawing.Color.Black;
            this.panelCLOSE.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelCLOSE.Controls.Add(this.btnClose);
            resources.ApplyResources(this.panelCLOSE, "panelCLOSE");
            this.panelCLOSE.Name = "panelCLOSE";
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // setupOperationfrm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panelCLOSE);
            this.Controls.Add(this.panelFEED);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "setupOperationfrm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.setupOperationfrm_FormClosing);
            this.panelFEED.ResumeLayout(false);
            this.panelCLOSE.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelFEED;
        private System.Windows.Forms.Label lblFEED;
        private System.Windows.Forms.Panel panelCLOSE;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnLOAD;
    }
}