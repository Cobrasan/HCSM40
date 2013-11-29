using System;
using System.Windows.Forms;

namespace Alchemist
{
    public partial class setupOperationfrm : Form
    {
        public setupOperationfrm()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            Program.MainForm.AddOwnedForm(this);

            // 機械毎の表示・非表示を行う
            formcustom();

#if HCSM40
            #region ボタンイベント登録
            Program.MainForm.SetBtnEvent(SystemConstants.LOAD1_BTN, SystemConstants.BTN_PUSH, btnLOAD);
            #endregion
#endif

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Visible = false;
        }

        private void setupOperationfrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Visible = false;
        }

        public void refresh()
        {
#if HCSM40
            #region ボタン状態表示登録
            mainfrm.CheckBtnAnd_ChangeColor(SystemConstants.LOAD1_STATUS, btnLOAD);
            #endregion
#endif
        }
    }
}