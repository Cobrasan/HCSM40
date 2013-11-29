namespace Alchemist
{
    partial class splushScreenfrm
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
            this.pbJamLogo = new System.Windows.Forms.PictureBox();
            this.lblMsg = new System.Windows.Forms.Label();
            this.pbBackGround = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbJamLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBackGround)).BeginInit();
            this.SuspendLayout();
            // 
            // pbJamLogo
            // 
            this.pbJamLogo.Image = global::Alchemist.Properties.Resources.JAMLogo;
            this.pbJamLogo.Location = new System.Drawing.Point(80, 188);
            this.pbJamLogo.Name = "pbJamLogo";
            this.pbJamLogo.Size = new System.Drawing.Size(146, 54);
            this.pbJamLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbJamLogo.TabIndex = 0;
            this.pbJamLogo.TabStop = false;
            // 
            // lblMsg
            // 
            this.lblMsg.BackColor = System.Drawing.Color.Cyan;
            this.lblMsg.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblMsg.Font = new System.Drawing.Font("メイリオ", 15.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblMsg.Location = new System.Drawing.Point(82, 137);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(142, 40);
            this.lblMsg.TabIndex = 1;
            this.lblMsg.Text = "starting";
            this.lblMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pbBackGround
            // 
            this.pbBackGround.Image = global::Alchemist.Properties.Resources.splushScreenBg2;
            this.pbBackGround.Location = new System.Drawing.Point(3, 7);
            this.pbBackGround.Name = "pbBackGround";
            this.pbBackGround.Size = new System.Drawing.Size(300, 243);
            this.pbBackGround.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbBackGround.TabIndex = 2;
            this.pbBackGround.TabStop = false;
            // 
            // splushScreenfrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(307, 257);
            this.Controls.Add(this.lblMsg);
            this.Controls.Add(this.pbJamLogo);
            this.Controls.Add(this.pbBackGround);
            this.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "splushScreenfrm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.Shown += new System.EventHandler(this.splushScreenfrm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pbJamLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBackGround)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbJamLogo;
        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.PictureBox pbBackGround;
    }
}