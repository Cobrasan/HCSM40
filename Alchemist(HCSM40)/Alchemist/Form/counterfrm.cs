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

		// �C�x���g�n���h���ݒ胁�\�b�h
        public void Initialize()
        {
            Program.MainForm.AddOwnedForm(this);

            // �@�B���̕\���E��\���ݒ�
            formCustom();

#if HCSM40
            #region �{�^���C�x���g
            Program.MainForm.SetBtnEvent(SystemConstants.QTY_COUNTER_RESET1_BTN, SystemConstants.BTN_PUSH, btnQTYReset);
            Program.MainForm.SetBtnEvent(SystemConstants.LOT_COUNTER_RESET1_BTN, SystemConstants.BTN_PUSH, btnLOTReset);
            Program.MainForm.SetBtnEvent(SystemConstants.COUNT_UP_BTN, SystemConstants.BTN_PUSH, btnLOTUp);
            Program.MainForm.SetBtnEvent(SystemConstants.COUNT_DOWN_BTN, SystemConstants.BTN_PUSH, btnLOTDown);
            Program.MainForm.SetBtnEvent(SystemConstants.TOTAL_COUNTER_RESET1_BTN, SystemConstants.BTN_PUSH, btnTOTALReset);
            #endregion

            #region �e�L�X�g���̓C�x���g
            Program.MainForm.SetTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.LOT_SET_COUNTER1, textLOTSetNumber);
            Program.MainForm.SetTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.QTY_SET_COUNTER1, textQTYSetNumber);
            #endregion

            #region �e���L�[���͗p�̃N���b�N�C�x���g
            Program.MainForm.ClickTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.TENKEY_INPUT_DATA, SystemConstants.QTY_SET_COUNTER1, textQTYSetNumber);
            Program.MainForm.ClickTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.TENKEY_INPUT_DATA, SystemConstants.LOT_SET_COUNTER1, textLOTSetNumber);
            #endregion
            
#endif
        }

		// �\���X�V�p���\�b�h
		public void refresh() 
		{
			mainfrm mainForm = ((mainfrm)this.Owner);

#if HCSM40
            #region �J�E���^�\��
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