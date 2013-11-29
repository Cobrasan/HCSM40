namespace Alchemist
{
    partial class errInfoMsgfrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(errInfoMsgfrm));
            this.panelInformation = new System.Windows.Forms.Panel();
            this.listInformation = new System.Windows.Forms.ListBox();
            this.lblInformation = new System.Windows.Forms.Label();
            this.panelError = new System.Windows.Forms.Panel();
            this.pbErrorImage = new System.Windows.Forms.PictureBox();
            this.listError = new System.Windows.Forms.ListBox();
            this.lblError = new System.Windows.Forms.Label();
            this.panelInformation.SuspendLayout();
            this.panelError.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbErrorImage)).BeginInit();
            this.SuspendLayout();
            // 
            // panelInformation
            // 
            this.panelInformation.BackColor = System.Drawing.Color.Blue;
            this.panelInformation.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelInformation.Controls.Add(this.listInformation);
            this.panelInformation.Controls.Add(this.lblInformation);
            resources.ApplyResources(this.panelInformation, "panelInformation");
            this.panelInformation.Name = "panelInformation";
            // 
            // listInformation
            // 
            resources.ApplyResources(this.listInformation, "listInformation");
            this.listInformation.BackColor = System.Drawing.Color.Blue;
            this.listInformation.ForeColor = System.Drawing.Color.White;
            this.listInformation.FormattingEnabled = true;
            this.listInformation.Name = "listInformation";
            this.listInformation.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.listInformation.TabStop = false;
            this.listInformation.UseTabStops = false;
            // 
            // lblInformation
            // 
            this.lblInformation.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.lblInformation, "lblInformation");
            this.lblInformation.ForeColor = System.Drawing.Color.White;
            this.lblInformation.Name = "lblInformation";
            // 
            // panelError
            // 
            this.panelError.BackColor = System.Drawing.Color.Red;
            this.panelError.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelError.Controls.Add(this.pbErrorImage);
            this.panelError.Controls.Add(this.listError);
            this.panelError.Controls.Add(this.lblError);
            resources.ApplyResources(this.panelError, "panelError");
            this.panelError.Name = "panelError";
            // 
            // pbErrorImage
            // 
            this.pbErrorImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbErrorImage.Image = global::Alchemist.Properties.Resources.NoErrImg;
            resources.ApplyResources(this.pbErrorImage, "pbErrorImage");
            this.pbErrorImage.Name = "pbErrorImage";
            this.pbErrorImage.TabStop = false;
            this.pbErrorImage.MouseEnter += new System.EventHandler(this.pbErrorData_MouseEnter);
            // 
            // listError
            // 
            resources.ApplyResources(this.listError, "listError");
            this.listError.BackColor = System.Drawing.Color.Red;
            this.listError.ForeColor = System.Drawing.Color.White;
            this.listError.FormattingEnabled = true;
            this.listError.Name = "listError";
            // 
            // lblError
            // 
            this.lblError.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.lblError, "lblError");
            this.lblError.ForeColor = System.Drawing.Color.White;
            this.lblError.Name = "lblError";
            // 
            // errInfoMsgfrm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.BackColor = System.Drawing.Color.Silver;
            this.Controls.Add(this.panelInformation);
            this.Controls.Add(this.panelError);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "errInfoMsgfrm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.errInfoMsgfrm_FormClosing);
            this.panelInformation.ResumeLayout(false);
            this.panelError.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbErrorImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelInformation;
        private System.Windows.Forms.ListBox listInformation;
        private System.Windows.Forms.Label lblInformation;
        private System.Windows.Forms.Panel panelError;
        private System.Windows.Forms.ListBox listError;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.PictureBox pbErrorImage;
    }
}