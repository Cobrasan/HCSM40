using System;
using System.Windows.Forms;

namespace Alchemist
{
    public partial class workDetailSpeedfrm : Form
    {
        /// <summary>
        /// �������ݒ�
        /// </summary>
        public void Initialize()
        {
            Program.MainForm.AddOwnedForm(this);

            // �@�했�̐ݒ�����s����
            formCustom();
        }

        /// <summary>
        /// �t�H�[���̃R���X�g���N�^
        /// </summary>
        public workDetailSpeedfrm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// �t�H�[���������̃C�x���g�o�^���̏���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void workDetailSpeedfrm_Shown(object sender, EventArgs e)
        {            
            #region �e�L�X�g���̓C�x���g
            Program.MainForm.SetTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.FEED_SPEED1, textFEED_SPEED1);
            Program.MainForm.SetTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.FEED_ACCEL1, textFEED_ACCEL1);
            Program.MainForm.SetTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.WIRE_LENGTH_CORRECT1, textWIRE_LENGTH_CORRECT11);
            Program.MainForm.SetTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.FEED_SPEED2, textFEED_SPEED2);
            Program.MainForm.SetTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.FEED_ACCEL2, textFEED_ACCEL2);
            Program.MainForm.SetTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.WIRE_LENGTH_CORRECT2, textWIRE_LENGTH_CORRECT12);
            Program.MainForm.SetTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.FEED_SPEED3, textFEED_SPEED3);
            Program.MainForm.SetTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.FEED_ACCEL3, textFEED_ACCEL3);
            Program.MainForm.SetTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.WIRE_LENGTH_CORRECT3, textWIRE_LENGTH_CORRECT13);
            Program.MainForm.SetTextBoxLength(this, 10);
            #endregion

            #region �e���L�[���͗p�̃N���b�N�C�x���g
            Program.MainForm.ClickTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.TENKEY_INPUT_DATA, SystemConstants.FEED_SPEED1, textFEED_SPEED1);
            Program.MainForm.ClickTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.TENKEY_INPUT_DATA, SystemConstants.FEED_ACCEL1, textFEED_ACCEL1);
            Program.MainForm.ClickTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.TENKEY_INPUT_DATA, SystemConstants.WIRE_LENGTH_CORRECT1, textWIRE_LENGTH_CORRECT11);
            Program.MainForm.ClickTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.TENKEY_INPUT_DATA, SystemConstants.FEED_SPEED2, textFEED_SPEED2);
            Program.MainForm.ClickTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.TENKEY_INPUT_DATA, SystemConstants.FEED_ACCEL2, textFEED_ACCEL2);
            Program.MainForm.ClickTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.TENKEY_INPUT_DATA, SystemConstants.WIRE_LENGTH_CORRECT2, textWIRE_LENGTH_CORRECT12);
            Program.MainForm.ClickTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.TENKEY_INPUT_DATA, SystemConstants.FEED_SPEED3, textFEED_SPEED3);
            Program.MainForm.ClickTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.TENKEY_INPUT_DATA, SystemConstants.FEED_ACCEL3, textFEED_ACCEL3);
            Program.MainForm.ClickTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.TENKEY_INPUT_DATA, SystemConstants.WIRE_LENGTH_CORRECT3, textWIRE_LENGTH_CORRECT13);
            Program.MainForm.ClickDataGridViewEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.TENKEY_INPUT_DATA, workDetailSpeedView);
            Program.MainForm.ClickTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.TENKEY_INPUT_WIRETHRES, SystemConstants.FEED_SPEED_THRES1, textFEED_SPEED_THRES1);
            Program.MainForm.ClickTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.TENKEY_INPUT_WIRETHRES, SystemConstants.FEED_SPEED_THRES2, textFEED_SPEED_THRES2);
            #endregion

            #region �O���b�h�̓��e�\��
            int[] ID = null;
            workDetailSpeedView.CurrentCell = null;
            Program.DataController.GetMemryDataGroupList(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.WORK_GROUP_SPEED1, ref ID);

            if (ID != null)
            {
                string min = "";
                string max = "";
                string value = "";
                string name = "";
                string unit = "";

                foreach (var workid in ID)
                {
                    // ���̂��擾����(�ǂ���邩�H�j
                    name = Utility.GetMessageString(SystemConstants.WORK_MSG, workid);

                    // �͈͂��擾����
                    Program.DataController.GetWorkDataRangeStr(workid, ref min, ref max);

                    // �l���擾����
                    Program.DataController.ReadWorkDataStr(workid, ref value);

                    //�P�ʂ��擾����B
                    Program.DataController.GetWorkDataUnit(workid, ref unit);

                    // �l��ݒ肷��
                    workDetailSpeedView.Rows.Add(new Object[] { workid, name, string.Format("{0} <-> {1}", new object[] { min, max }), value, unit });
                }
            }
            #endregion
        }

        /// <summary>
        /// �t�H�[�������C�x���g
        /// �I�����Ȃ��Ŕ�\���ɂ���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void workDetailSpeedfrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Visible = false;
        }

        /// <summary>
        /// ����{�^���̃C�x���g
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            Visible = false;
        }

        /// <summary>
        /// ��ʂ̕\�����e���X�V���鏈��
        /// ��ʕ\�����̓��C���t�H�[���̃X���b�h����Ă΂��
        /// </summary>
        public void refresh()
        {
            #region �ݒ荀�ڂ̕\���X�V
            Program.MainForm.refreshControl(SystemConstants.FEED_SPEED_THRES1, textFEED_SPEED_THRES1);
            Program.MainForm.refreshControl(SystemConstants.FEED_SPEED_THRES2, textFEED_SPEED_THRES2);
            Program.MainForm.refreshControl(SystemConstants.FEED_SPEED1, textFEED_SPEED1);
            Program.MainForm.refreshControl(SystemConstants.FEED_ACCEL1, textFEED_ACCEL1);
            Program.MainForm.refreshControl(SystemConstants.WIRE_LENGTH_CORRECT1, textWIRE_LENGTH_CORRECT11);
            Program.MainForm.refreshControl(SystemConstants.FEED_SPEED2, textFEED_SPEED2);
            Program.MainForm.refreshControl(SystemConstants.FEED_ACCEL2, textFEED_ACCEL2);
            Program.MainForm.refreshControl(SystemConstants.WIRE_LENGTH_CORRECT2, textWIRE_LENGTH_CORRECT12);
            Program.MainForm.refreshControl(SystemConstants.FEED_SPEED3, textFEED_SPEED3);
            Program.MainForm.refreshControl(SystemConstants.FEED_ACCEL3, textFEED_ACCEL3);
            Program.MainForm.refreshControl(SystemConstants.WIRE_LENGTH_CORRECT3, textWIRE_LENGTH_CORRECT13);

            //臒l���x���\��
            Program.MainForm.refreshControlSpeedThres(SystemConstants.FEED_SPEED_THRES1, lblFEED_SPEED_THRES2, " -");
            Program.MainForm.refreshControlSpeedThres(SystemConstants.FEED_SPEED_THRES2, lblFEED_SPEED_THRES3, " -");
            #endregion

            #region �O���b�h�̓��e�̍X�V
            int rowCount = workDetailSpeedView.Rows.Count;
            string value = "";

            //GridView�̍X�V��
            for (int y = 0; y < rowCount; y++)
            {
                int workid = Int32.Parse(workDetailSpeedView.Rows[y].Cells[0].Value.ToString());

                // �l���擾����
                Program.DataController.ReadWorkDataStr(workid, ref value);

                var cell = workDetailSpeedView.Rows[y].Cells[3];

                // �l���ҏW���łȂ���΁A�l��ύX����
                if (!cell.IsInEditMode)
                {
                    workDetailSpeedView.Rows[y].Cells[3].Value = value;
                }
            }
            #endregion
        }

        /// <summary>
        /// �f�[�^�O���b�h�̃Z����ύX���ꂽ�甭������C�x���g
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void workDetailSpeedView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (Program.SystemData.tachpanel == true) return;
            if (e.ColumnIndex != 3) return;

            DataGridView view = workDetailSpeedView;
            int workidtype = SystemConstants.WORKID_TYPE_WORKDATA;
            int workid = Int32.Parse(view.Rows[e.RowIndex].Cells[0].Value.ToString());
            object value = e.FormattedValue;

            Program.MainForm.EnterDataGridView(workidtype, workid, value);
        }

        /// <summary>
        /// 臒l�Q�̊m�菈��
        /// </summary>
        /// <param name="e"></param>
        private void textFEED_SPEED_THRES2_EnterKeyDown(EventArgs e)
        {
            /*
            double workdata = 0;
            string errMessage;
            double outValue;

            // �`���`�F�b�N
            if (Program.MainForm.checkTextBoxValue(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.FEED_SPEED_THRES2, textFEED_SPEED_THRES2.Text, out outValue, out errMessage) == false)
            {
                Utility.ShowErrorMsg(errMessage);
                return;
            }

            // 臒l�P��ǂ�
            Program.DataController.ReadWorkData(SystemConstants.FEED_SPEED_THRES1, ref workdata);

            // �͈̓`�F�b�N
            if (outValue <= workdata)
            {
                string workname = Utility.GetMessageString(SystemConstants.WORK_MSG, SystemConstants.FEED_SPEED_THRES2);
                Utility.ShowErrorMsg(SystemConstants.SYSTEM_MSG022, workname, lblFEED_SPEED_THRES2.Text);
                return;
            }

            // ���[�N�f�[�^����������
            mainfrm.WriteWorkData(SystemConstants.FEED_SPEED_THRES2, outValue);
            */

            Program.MainForm.EnterTextBoxWireLengthThres(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.FEED_SPEED_THRES2, textFEED_SPEED_THRES2);

            // �t�H�[�J�X�A�E�g����
            Form frm = textFEED_SPEED_THRES2.FindForm();
            frm.ActiveControl = null;
        }

        /// <summary>
        ///  臒l�P�̊m�菈��
        /// </summary>
        /// <param name="e"></param>
        private void textFEED_SPEED_THRES1_EnterKeyDown_1(EventArgs e)
        {
            /*
            double outValue;
            string errMessage;
            double workdata = 0;

            // �`���`�F�b�N
            if (Program.MainForm.checkTextBoxValue(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.FEED_SPEED_THRES1, textFEED_SPEED_THRES1.Text, out outValue, out errMessage) == false)
            {
                Utility.ShowErrorMsg(errMessage);
                return;
            }

            // 臒l�Q��ǂ�
            Program.DataController.ReadWorkData(SystemConstants.FEED_SPEED_THRES2, ref workdata);

            // �K���͈̓`�F�b�N
            if (textFEED_SPEED_THRES1.Text == "0" || workdata <= outValue)
            {
                string workname = Utility.GetMessageString(SystemConstants.WORK_MSG, SystemConstants.FEED_SPEED_THRES2);
                Utility.ShowErrorMsg(SystemConstants.SYSTEM_MSG027, workname, "0", textFEED_SPEED_THRES2.Text);
                return;
            }

            // ���[�N�f�[�^����������
            mainfrm.WriteWorkData(SystemConstants.FEED_SPEED_THRES1, outValue);
            */

            Program.MainForm.EnterTextBoxWireLengthThres(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.FEED_SPEED_THRES1, textFEED_SPEED_THRES1);

            // �t�H�[�J�X�A�E�g����
            Form frm = textFEED_SPEED_THRES1.FindForm();
            frm.ActiveControl = null;
        }
    }
}