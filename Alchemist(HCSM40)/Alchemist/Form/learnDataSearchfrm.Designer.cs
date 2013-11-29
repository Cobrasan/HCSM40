namespace Alchemist
{
    partial class learnDataSearchfrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(learnDataSearchfrm));
            this.pnlApply = new System.Windows.Forms.Panel();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.gbWire = new System.Windows.Forms.GroupBox();
            this.comboColor2 = new System.Windows.Forms.ComboBox();
            this.lblColor2 = new System.Windows.Forms.Label();
            this.lblColor1 = new System.Windows.Forms.Label();
            this.comboColor1 = new System.Windows.Forms.ComboBox();
            this.comboCoreSize = new System.Windows.Forms.ComboBox();
            this.lblCoreSize = new System.Windows.Forms.Label();
            this.lblWireType = new System.Windows.Forms.Label();
            this.comboWireType = new System.Windows.Forms.ComboBox();
            this.lblStripLength1 = new System.Windows.Forms.Label();
            this.lblStripLength2 = new System.Windows.Forms.Label();
            this.pnlClose = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.textStripLength2 = new Alchemist.CustomTextBox();
            this.textStripLength1 = new Alchemist.CustomTextBox();
            this.gbWorkMode = new System.Windows.Forms.GroupBox();
            this.lblWireLength = new System.Windows.Forms.Label();
            this.textWireLength = new Alchemist.CustomTextBox();
            this.pnlApply.SuspendLayout();
            this.gbWire.SuspendLayout();
            this.pnlClose.SuspendLayout();
            this.gbWorkMode.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlApply
            // 
            resources.ApplyResources(this.pnlApply, "pnlApply");
            this.pnlApply.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlApply.Controls.Add(this.btnClear);
            this.pnlApply.Controls.Add(this.btnApply);
            this.pnlApply.Name = "pnlApply";
            // 
            // btnClear
            // 
            resources.ApplyResources(this.btnClear, "btnClear");
            this.btnClear.Name = "btnClear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnApply
            // 
            resources.ApplyResources(this.btnApply, "btnApply");
            this.btnApply.Name = "btnApply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // gbWire
            // 
            this.gbWire.Controls.Add(this.comboColor2);
            this.gbWire.Controls.Add(this.lblColor2);
            this.gbWire.Controls.Add(this.lblColor1);
            this.gbWire.Controls.Add(this.comboColor1);
            this.gbWire.Controls.Add(this.comboCoreSize);
            this.gbWire.Controls.Add(this.lblCoreSize);
            this.gbWire.Controls.Add(this.lblWireType);
            this.gbWire.Controls.Add(this.comboWireType);
            resources.ApplyResources(this.gbWire, "gbWire");
            this.gbWire.Name = "gbWire";
            this.gbWire.TabStop = false;
            // 
            // comboColor2
            // 
            this.comboColor2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.comboColor2.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboColor2.BackColor = System.Drawing.Color.White;
            this.comboColor2.ForeColor = System.Drawing.Color.Black;
            this.comboColor2.FormattingEnabled = true;
            resources.ApplyResources(this.comboColor2, "comboColor2");
            this.comboColor2.Name = "comboColor2";
            this.comboColor2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.upCase_KeyPress);
            // 
            // lblColor2
            // 
            resources.ApplyResources(this.lblColor2, "lblColor2");
            this.lblColor2.Name = "lblColor2";
            // 
            // lblColor1
            // 
            resources.ApplyResources(this.lblColor1, "lblColor1");
            this.lblColor1.Name = "lblColor1";
            // 
            // comboColor1
            // 
            this.comboColor1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.comboColor1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboColor1.BackColor = System.Drawing.Color.White;
            this.comboColor1.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.comboColor1, "comboColor1");
            this.comboColor1.Name = "comboColor1";
            this.comboColor1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.upCase_KeyPress);
            // 
            // comboCoreSize
            // 
            this.comboCoreSize.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.comboCoreSize.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboCoreSize.BackColor = System.Drawing.Color.White;
            this.comboCoreSize.ForeColor = System.Drawing.Color.Black;
            this.comboCoreSize.FormattingEnabled = true;
            resources.ApplyResources(this.comboCoreSize, "comboCoreSize");
            this.comboCoreSize.Name = "comboCoreSize";
            this.comboCoreSize.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.upCase_KeyPress);
            // 
            // lblCoreSize
            // 
            resources.ApplyResources(this.lblCoreSize, "lblCoreSize");
            this.lblCoreSize.Name = "lblCoreSize";
            // 
            // lblWireType
            // 
            resources.ApplyResources(this.lblWireType, "lblWireType");
            this.lblWireType.Name = "lblWireType";
            // 
            // comboWireType
            // 
            this.comboWireType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.comboWireType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboWireType.BackColor = System.Drawing.Color.White;
            this.comboWireType.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.comboWireType, "comboWireType");
            this.comboWireType.Name = "comboWireType";
            this.comboWireType.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.upCase_KeyPress);
            // 
            // lblStripLength1
            // 
            resources.ApplyResources(this.lblStripLength1, "lblStripLength1");
            this.lblStripLength1.Name = "lblStripLength1";
            this.lblStripLength1.Click += new System.EventHandler(this.forcusOut_Click);
            // 
            // lblStripLength2
            // 
            resources.ApplyResources(this.lblStripLength2, "lblStripLength2");
            this.lblStripLength2.Name = "lblStripLength2";
            this.lblStripLength2.Click += new System.EventHandler(this.forcusOut_Click);
            // 
            // pnlClose
            // 
            resources.ApplyResources(this.pnlClose, "pnlClose");
            this.pnlClose.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlClose.Controls.Add(this.btnClose);
            this.pnlClose.Name = "pnlClose";
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // textStripLength2
            // 
            this.textStripLength2.AllowAll = false;
            this.textStripLength2.AllowDot = true;
            this.textStripLength2.AllowHex = false;
            this.textStripLength2.AllowSign = false;
            this.textStripLength2.BackColor = System.Drawing.Color.White;
            this.textStripLength2.FocusColor = System.Drawing.Color.Blue;
            this.textStripLength2.FocusStringSelect = true;
            this.textStripLength2.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.textStripLength2, "textStripLength2");
            this.textStripLength2.Name = "textStripLength2";
            this.textStripLength2.NormalColor = System.Drawing.Color.White;
            this.textStripLength2.TextLengthChange = true;
            // 
            // textStripLength1
            // 
            this.textStripLength1.AllowAll = false;
            this.textStripLength1.AllowDot = true;
            this.textStripLength1.AllowHex = false;
            this.textStripLength1.AllowSign = false;
            this.textStripLength1.BackColor = System.Drawing.Color.White;
            this.textStripLength1.FocusColor = System.Drawing.Color.Blue;
            this.textStripLength1.FocusStringSelect = true;
            this.textStripLength1.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.textStripLength1, "textStripLength1");
            this.textStripLength1.Name = "textStripLength1";
            this.textStripLength1.NormalColor = System.Drawing.Color.White;
            this.textStripLength1.TextLengthChange = true;
            // 
            // gbWorkMode
            // 
            this.gbWorkMode.Controls.Add(this.lblWireLength);
            this.gbWorkMode.Controls.Add(this.textWireLength);
            this.gbWorkMode.Controls.Add(this.lblStripLength2);
            this.gbWorkMode.Controls.Add(this.textStripLength2);
            this.gbWorkMode.Controls.Add(this.lblStripLength1);
            this.gbWorkMode.Controls.Add(this.textStripLength1);
            resources.ApplyResources(this.gbWorkMode, "gbWorkMode");
            this.gbWorkMode.Name = "gbWorkMode";
            this.gbWorkMode.TabStop = false;
            // 
            // lblWireLength
            // 
            resources.ApplyResources(this.lblWireLength, "lblWireLength");
            this.lblWireLength.Name = "lblWireLength";
            // 
            // textWireLength
            // 
            this.textWireLength.AllowAll = false;
            this.textWireLength.AllowDot = true;
            this.textWireLength.AllowHex = false;
            this.textWireLength.AllowSign = false;
            this.textWireLength.BackColor = System.Drawing.Color.White;
            this.textWireLength.FocusColor = System.Drawing.Color.Blue;
            this.textWireLength.FocusStringSelect = true;
            this.textWireLength.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.textWireLength, "textWireLength");
            this.textWireLength.Name = "textWireLength";
            this.textWireLength.NormalColor = System.Drawing.Color.White;
            this.textWireLength.TextLengthChange = true;
            // 
            // learnDataSearchfrm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.gbWorkMode);
            this.Controls.Add(this.pnlClose);
            this.Controls.Add(this.gbWire);
            this.Controls.Add(this.pnlApply);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "learnDataSearchfrm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.databasefrm_FormClosing);
            this.VisibleChanged += new System.EventHandler(this.learnDataSearchfrm_VisibleChanged);
            this.Click += new System.EventHandler(this.forcusOut_Click);
            this.pnlApply.ResumeLayout(false);
            this.gbWire.ResumeLayout(false);
            this.pnlClose.ResumeLayout(false);
            this.gbWorkMode.ResumeLayout(false);
            this.gbWorkMode.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlApply;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.GroupBox gbWire;
        private System.Windows.Forms.Label lblWireType;
        private System.Windows.Forms.ComboBox comboWireType;
        private System.Windows.Forms.ComboBox comboCoreSize;
        private System.Windows.Forms.Label lblCoreSize;
        private CustomTextBox textStripLength1;
        private System.Windows.Forms.Label lblStripLength1;
        private CustomTextBox textStripLength2;
        private System.Windows.Forms.Label lblStripLength2;
        private System.Windows.Forms.Panel pnlClose;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.ComboBox comboColor2;
        private System.Windows.Forms.Label lblColor2;
        private System.Windows.Forms.Label lblColor1;
        private System.Windows.Forms.ComboBox comboColor1;
        private System.Windows.Forms.GroupBox gbWorkMode;
        private System.Windows.Forms.Label lblWireLength;
        private CustomTextBox textWireLength;
    }
}