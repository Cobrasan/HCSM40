using System;
using System.Windows.Forms;

namespace Alchemist
{
    public partial class counterfrm : Form
    {
        public counterfrm()
        {
            InitializeComponent();
        }

		// イベントハンドラ設定メソッド
        public void Initialize()
        {
            Program.MainForm.AddOwnedForm(this);

            // 機械毎の表示・非表示設定
            formCustom();

#if HCSM40
            #region ボタンイベント
            Program.MainForm.SetBtnEvent(SystemConstants.QTY_COUNTER_RESET1_BTN, SystemConstants.BTN_PUSH, btnQTYReset);
            Program.MainForm.SetBtnEvent(SystemConstants.LOT_COUNTER_RESET1_BTN, SystemConstants.BTN_PUSH, btnLOTReset);
            Program.MainForm.SetBtnEvent(SystemConstants.COUNT_UP_BTN, SystemConstants.BTN_PUSH, btnLOTUp);
            Program.MainForm.SetBtnEvent(SystemConstants.COUNT_DOWN_BTN, SystemConstants.BTN_PUSH, btnLOTDown);
            Program.MainForm.SetBtnEvent(SystemConstants.TOTAL_COUNTER_RESET1_BTN, SystemConstants.BTN_PUSH, btnTOTALReset);
            #endregion

            #region テキスト入力イベント
            Program.MainForm.SetTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.LOT_SET_COUNTER1, textLOTSetNumber);
            Program.MainForm.SetTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.QTY_SET_COUNTER1, textQTYSetNumber);
            #endregion

            #region テンキー入力用のクリックイベント
            Program.MainForm.ClickTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.TENKEY_INPUT_DATA, SystemConstants.QTY_SET_COUNTER1, textQTYSetNumber);
            Program.MainForm.ClickTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.TENKEY_INPUT_DATA, SystemConstants.LOT_SET_COUNTER1, textLOTSetNumber);
            #endregion
            
#endif
        }

		// 表示更新用メソッド
		public void refresh() 
		{
			mainfrm mainForm = ((mainfrm)this.Owner);

#if HCSM40
            #region カウンタ表示
            mainForm.refreshControl(SystemConstants.QTY_COUNTER1, lblQTY2);
			mainForm.refreshControl(SystemConstants.QTY_SET_COUNTER1, textQTYSetNumber);
			mainForm.refreshControl(SystemConstants.LOT_COUNTER1, lblLOT2);
			mainForm.refreshControl(SystemConstants.LOT_SET_COUNTER1, textLOTSetNumber);
			mainForm.refreshControl(SystemConstants.TOTAL_COUNTER1, lblTOTAL2);
			mainForm.refreshControl(SystemConstants.MACHINE_TACT1, lblTact4);
            #endregion
#endif
        }

		private void btnClose_Click(object sender, EventArgs e)
		{
			Visible = false;
		}

		private void counterfrm_FormClosing(object sender, FormClosingEventArgs e)
		{
			e.Cancel = true;
			Visible = false;
		}
    }
}