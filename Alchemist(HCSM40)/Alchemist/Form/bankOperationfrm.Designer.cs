namespace Alchemist
{
    partial class bankOperationfrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(bankOperationfrm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelSelect = new System.Windows.Forms.Panel();
            this.btnSelect = new System.Windows.Forms.Button();
            this.bankOperationView = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelCopy = new System.Windows.Forms.Panel();
            this.lblCopy1 = new System.Windows.Forms.Label();
            this.textCopy2 = new Alchemist.CustomTextBox();
            this.btnCopy = new System.Windows.Forms.Button();
            this.panelClose = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.panelNowBank = new System.Windows.Forms.Panel();
            this.lblNowBankNo2 = new System.Windows.Forms.Label();
            this.lblNowBankNo1 = new System.Windows.Forms.Label();
            this.panelSelect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bankOperationView)).BeginInit();
            this.panelCopy.SuspendLayout();
            this.panelClose.SuspendLayout();
            this.panelNowBank.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelSelect
            // 
            resources.ApplyResources(this.panelSelect, "panelSelect");
            this.panelSelect.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelSelect.Controls.Add(this.btnSelect);
            this.panelSelect.Name = "panelSelect";
            // 
            // btnSelect
            // 
            resources.ApplyResources(this.btnSelect, "btnSelect");
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.button1_Click);
            // 
            // bankOperationView
            // 
            resources.ApplyResources(this.bankOperationView, "bankOperationView");
            this.bankOperationView.AllowUserToAddRows = false;
            this.bankOperationView.AllowUserToDeleteRows = false;
            this.bankOperationView.AllowUserToResizeColumns = false;
            this.bankOperationView.AllowUserToResizeRows = false;
            this.bankOperationView.BackgroundColor = System.Drawing.Color.Black;
            this.bankOperationView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("メイリオ", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.bankOperationView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.bankOperationView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.bankOperationView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.bankOperationView.Name = "bankOperationView";
            this.bankOperationView.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("メイリオ", 9F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.bankOperationView.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.bankOperationView.RowHeadersVisible = false;
            this.bankOperationView.RowTemplate.Height = 21;
            this.bankOperationView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.bankOperationView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.bankOperationView_CellDoubleClick);
            // 
            // Column1
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(1);
            this.Column1.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.Column1, "Column1");
            this.Column1.MaxInputLength = 3;
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column2
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.Column2.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.Column2, "Column2");
            this.Column2.MaxInputLength = 100;
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // panelCopy
            // 
            resources.ApplyResources(this.panelCopy, "panelCopy");
            this.panelCopy.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelCopy.Controls.Add(this.lblCopy1);
            this.panelCopy.Controls.Add(this.textCopy2);
            this.panelCopy.Controls.Add(this.btnCopy);
            this.panelCopy.Name = "panelCopy";
            // 
            // lblCopy1
            // 
            resources.ApplyResources(this.lblCopy1, "lblCopy1");
            this.lblCopy1.BackColor = System.Drawing.Color.Transparent;
            this.lblCopy1.ForeColor = System.Drawing.Color.White;
            this.lblCopy1.Name = "lblCopy1";
            // 
            // textCopy2
            // 
            resources.ApplyResources(this.textCopy2, "textCopy2");
            this.textCopy2.AllowAll = false;
            this.textCopy2.AllowDot = true;
            this.textCopy2.AllowHex = false;
            this.textCopy2.AllowSign = false;
            this.textCopy2.FocusColor = System.Drawing.Color.White;
            this.textCopy2.FocusStringSelect = true;
            this.textCopy2.Name = "textCopy2";
            this.textCopy2.NormalColor = System.Drawing.Color.White;
            this.textCopy2.TextLengthChange = true;
            // 
            // btnCopy
            // 
            resources.ApplyResources(this.btnCopy, "btnCopy");
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // panelClose
            // 
            resources.ApplyResources(this.panelClose, "panelClose");
            this.panelClose.BackColor = System.Drawing.Color.Black;
            this.panelClose.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelClose.Controls.Add(this.btnClose);
            this.panelClose.Name = "panelClose";
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // panelNowBank
            // 
            resources.ApplyResources(this.panelNowBank, "panelNowBank");
            this.panelNowBank.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelNowBank.Controls.Add(this.lblNowBankNo2);
            this.panelNowBank.Controls.Add(this.lblNowBankNo1);
            this.panelNowBank.Name = "panelNowBank";
            // 
            // lblNowBankNo2
            // 
            resources.ApplyResources(this.lblNowBankNo2, "lblNowBankNo2");
            this.lblNowBankNo2.BackColor = System.Drawing.Color.Transparent;
            this.lblNowBankNo2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblNowBankNo2.ForeColor = System.Drawing.Color.White;
            this.lblNowBankNo2.Name = "lblNowBankNo2";
            this.lblNowBankNo2.Click += new System.EventHandler(this.lblNowBankNo2_Click);
            // 
            // lblNowBankNo1
            // 
            resources.ApplyResources(this.lblNowBankNo1, "lblNowBankNo1");
            this.lblNowBankNo1.BackColor = System.Drawing.Color.Transparent;
            this.lblNowBankNo1.ForeColor = System.Drawing.Color.White;
            this.lblNowBankNo1.Name = "lblNowBankNo1";
            // 
            // bankOperationfrm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.panelNowBank);
            this.Controls.Add(this.panelClose);
            this.Controls.Add(this.panelCopy);
            this.Controls.Add(this.bankOperationView);
            this.Controls.Add(this.panelSelect);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "bankOperationfrm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.bankOperationfrm_FormClosing);
            this.VisibleChanged += new System.EventHandler(this.bankOperationfrm_VisibleChanged);
            this.panelSelect.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bankOperationView)).EndInit();
            this.panelCopy.ResumeLayout(false);
            this.panelCopy.PerformLayout();
            this.panelClose.ResumeLayout(false);
            this.panelNowBank.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelSelect;
        private System.Windows.Forms.DataGridView bankOperationView;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Panel panelCopy;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Label lblCopy1;
        private System.Windows.Forms.Panel panelClose;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel panelNowBank;
        private System.Windows.Forms.Label lblNowBankNo2;
        private System.Windows.Forms.Label lblNowBankNo1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private CustomTextBox textCopy2;
    }
}