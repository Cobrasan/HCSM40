namespace Alchemist
{
    partial class errImageZoomfrm
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
            this.pbZoomImage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbZoomImage)).BeginInit();
            this.SuspendLayout();
            // 
            // pbZoomImage
            // 
            this.pbZoomImage.BackColor = System.Drawing.Color.Red;
            this.pbZoomImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbZoomImage.Image = global::Alchemist.Properties.Resources.NoErrImg;
            this.pbZoomImage.Location = new System.Drawing.Point(0, 0);
            this.pbZoomImage.Name = "pbZoomImage";
            this.pbZoomImage.Size = new System.Drawing.Size(486, 344);
            this.pbZoomImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbZoomImage.TabIndex = 0;
            this.pbZoomImage.TabStop = false;
            this.pbZoomImage.MouseLeave += new System.EventHandler(this.pbZoomImage_MouseLeave);
            // 
            // errImageZoomfrm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Red;
            this.ClientSize = new System.Drawing.Size(486, 344);
            this.Controls.Add(this.pbZoomImage);
            this.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "errImageZoomfrm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "errImageZoom";
            this.MouseLeave += new System.EventHandler(this.errImageZoomfrm_MouseLeave);
            ((System.ComponentModel.ISupportInitialize)(this.pbZoomImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbZoomImage;
    }
}