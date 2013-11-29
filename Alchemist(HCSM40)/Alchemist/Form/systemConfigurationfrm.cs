using System;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;

namespace Alchemist
{
    public partial class systemConfigurationfrm : Form
    {
#if MAINTELOG
        private int[] showMainteUnitCodes = new int[]{
            SystemConstants.MAINTELOG_UNIT_FEED1,
            SystemConstants.MAINTELOG_UNIT_TRANSFER1,
            SystemConstants.MAINTELOG_UNIT_TRANSFER2,
            SystemConstants.MAINTELOG_UNIT_EJECT1,
            SystemConstants.MAINTELOG_UNIT_SLIDER1,
            SystemConstants.MAINTELOG_UNIT_SLIDER2,
            SystemConstants.MAINTELOG_UNIT_CUTSTRIP1,
            SystemConstants.MAINTELOG_UNIT_CRIMP1,
            SystemConstants.MAINTELOG_UNIT_CRIMP2,
            SystemConstants.MAINTELOG_UNIT_SEAL1,
            SystemConstants.MAINTELOG_UNIT_SEAL2,
            SystemConstants.MAINTELOG_UNIT_OTHER
        };
#endif

        /// <summary>
        /// ��ԕ\���p�f�[�^�N���X���`����
        /// index �́A�ȉ��̃��[���ō쐬����܂��B
        /// 
        ///   X00
        /// 
        /// X: �\���^�C�~���O 0: �����\���A1: 1����
        /// 00: �\���ʒu
        /// 
        /// 
        /// �E�\���ʒu
        /// +--+--+--+
        /// | 0|10|20|
        /// +--+--+--+
        /// | 1|11|21|
        /// +--+--+--+
        /// | 2|12|22|
        /// +--+--+--+
        /// | 3|13|23|
        /// +--+--+--+
        /// | 4|14|24|
        /// 
        /// </summary>
        private class sensorLockWriteBtn
        {
            public int BtnID;
            public int BtnMotion;
        }
        
        /// <summary>
        /// �Z���T�[���b�N��ԓǂݍ��ݗp�{�^���N���X
        /// </summary>
        private class sensorLockReadBtn
        {
            public int BtnID;
            public bool ReverseFlg;
        }
        
        /// <summary>
        /// �Z���T�[���b�N��ԃp�l���̍\����
        /// </summary>
        private struct sensorLockDspStruct
        {
            public int MsgID;
            public sensorLockReadBtn ReadBtn;
            public sensorLockWriteBtn WriteBtn;
        }
        
        private Dictionary<int, sensorLockDspStruct> sensorDspMap = new Dictionary<int, sensorLockDspStruct>();     // �Z���T�[���b�N�{�^���̃f�[�^�}�b�v
        private int statusDspSensorPgMax = 0;   // �Z���T�[���b�N��ԃp�l���̃y�[�W��
        
        /// <summary>
        /// �Z���T�[���b�N�{�^���̃f�[�^�}�b�v�ɓ��e��o�^���鏈��
        /// </summary>
        /// <param name="dspTimming"></param>
        /// <param name="dspPosition"></param>
        /// <param name="msgID"></param>
        /// <param name="readBtnCls"></param>
        /// <param name="writeBtnCls"></param>
        private void sensorLockDspMapAdd(int dspTimming, int dspPosition, int msgID, sensorLockReadBtn readBtnCls, sensorLockWriteBtn writeBtnCls)
        {
            // �������s�K�؂ȏꍇ�A��O�𔭐�������
            if (dspTimming < 0 || dspPosition < 0 || dspPosition > 99) new ArgumentException("SensorLock DspTimming or DspPosition is out of range");

            // �y�[�W�̍ő吔�����߂�
            if (dspTimming > statusDspSensorPgMax) statusDspSensorPgMax = dspTimming;

            // �C���f�b�N�X�𐶐�����
            int index = mainfrm.dspIndexCalc(dspTimming, dspPosition);

            // �ݒ�f�[�^�ɒǉ�����
            sensorLockDspStruct opeStruct = new sensorLockDspStruct();
            opeStruct.MsgID = msgID;
            opeStruct.ReadBtn = readBtnCls;
            opeStruct.WriteBtn = writeBtnCls;

            // �}�b�v�ɒǉ�����
            sensorDspMap.Add(index, opeStruct);
        }

        /// <summary>
        /// �Z���T�[���b�N�{�^���ɏ�Ԃ��������ޏ���
        /// </summary>
        /// <param name="btnID"></param>
        /// <param name="btnMotion"></param>
        /// <returns></returns>
        private sensorLockWriteBtn getWriteBtnClass(int btnID, int btnMotion)
        {
            // �{�^���̓��샂�[�h���قȂ�ꍇ�A��O��Ԃ�
            if (btnMotion != SystemConstants.BTN_PUSH && btnMotion != SystemConstants.BTN_ON && btnMotion != SystemConstants.BTN_OFF) throw new ArgumentException("BtnMotion is out of range");

            sensorLockWriteBtn writeBtn = new sensorLockWriteBtn();
            writeBtn.BtnID = btnID;
            writeBtn.BtnMotion = btnMotion;
            return writeBtn;
        }

        /// <summary>
        /// �Z���T�[���b�N�{�^����Ԃ�Ԃ�����
        /// </summary>
        /// <param name="btnID"></param>
        /// <param name="reverseFlg"></param>
        /// <returns></returns>
        private sensorLockReadBtn getReadBtnClass(int btnID, bool reverseFlg = false)
        {
            sensorLockReadBtn readBtn = new sensorLockReadBtn();
            readBtn.BtnID = btnID;
            readBtn.ReverseFlg = reverseFlg;
            return readBtn;
        }

        /// <summary>
        /// �������[���j�^�[��ʂ̒�`
        /// </summary>
        private iocheckfrm iochecForm = new iocheckfrm();
        public ioMonitorfrm ioMonitorForm = null;
        private learnDataItemEditfrm learndataitemeditForm = new learnDataItemEditfrm();

        /// <summary>
        /// �������ݒ�
        /// </summary>
        public void Initialize()
        {
            Program.MainForm.AddOwnedForm(this);

            // �@�했�̕\���E��\����ύX
            formCustom();

            #region ���o�{�^���C�x���g������
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX0, btnSensorLock00);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX1, btnSensorLock01);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX2, btnSensorLock02);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX3, btnSensorLock03);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX4, btnSensorLock04);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX5, btnSensorLock05);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX6, btnSensorLock06);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX7, btnSensorLock07);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX8, btnSensorLock08);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX9, btnSensorLock09);

            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX10, btnSensorLock10);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX11, btnSensorLock11);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX12, btnSensorLock12);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX13, btnSensorLock13);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX14, btnSensorLock14);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX15, btnSensorLock15);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX16, btnSensorLock16);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX17, btnSensorLock17);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX18, btnSensorLock18);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX19, btnSensorLock19);

            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX20, btnSensorLock20);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX21, btnSensorLock21);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX22, btnSensorLock22);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX23, btnSensorLock23);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX24, btnSensorLock24);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX25, btnSensorLock25);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX26, btnSensorLock26);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX27, btnSensorLock27);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX28, btnSensorLock28);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX29, btnSensorLock29);
            #endregion

            #region �␳�l�E�^�C�~���O�̕\�ւ̓��̓C�x���g�o�^
            setDataGridEvent(SystemConstants.WORKID_TYPE_CORRECTDATA, systemConfiguration_CORR_GROUP_BASEMACHINE1View);
            setDataGridEvent(SystemConstants.WORKID_TYPE_CORRECTDATA, systemConfiguration_CORR_GROUP_FEED1View);
            setDataGridEvent(SystemConstants.WORKID_TYPE_CORRECTDATA, systemConfiguration_CORR_GROUP_SLIDER1View);
            setDataGridEvent(SystemConstants.WORKID_TYPE_CORRECTDATA, systemConfiguration_CORR_GROUP_STRIP1View);
            setDataGridEvent(SystemConstants.WORKID_TYPE_TIMINGDATA, systemConfiguration_TIMM_GROUP_SIDE1VIEW);
            setDataGridEvent(SystemConstants.WORKID_TYPE_TIMINGDATA, systemConfiguration_TIMM_GROUP_SIDE2VIEW);
            #endregion

            #region �e���L�[���͂̂��߂̃O���b�h�N���b�N�C�x���g
            Program.MainForm.ClickDataGridViewEvent(SystemConstants.WORKID_TYPE_CORRECTDATA, SystemConstants.TENKEY_INPUT_DATA, systemConfiguration_CORR_GROUP_BASEMACHINE1View);
            Program.MainForm.ClickDataGridViewEvent(SystemConstants.WORKID_TYPE_CORRECTDATA, SystemConstants.TENKEY_INPUT_DATA, systemConfiguration_CORR_GROUP_FEED1View);
            Program.MainForm.ClickDataGridViewEvent(SystemConstants.WORKID_TYPE_CORRECTDATA, SystemConstants.TENKEY_INPUT_DATA, systemConfiguration_CORR_GROUP_SLIDER1View);
            Program.MainForm.ClickDataGridViewEvent(SystemConstants.WORKID_TYPE_CORRECTDATA, SystemConstants.TENKEY_INPUT_DATA, systemConfiguration_CORR_GROUP_STRIP1View);
            Program.MainForm.ClickDataGridViewEvent(SystemConstants.WORKID_TYPE_TIMINGDATA, SystemConstants.TENKEY_INPUT_DATA, systemConfiguration_TIMM_GROUP_SIDE1VIEW);
            Program.MainForm.ClickDataGridViewEvent(SystemConstants.WORKID_TYPE_TIMINGDATA, SystemConstants.TENKEY_INPUT_DATA, systemConfiguration_TIMM_GROUP_SIDE2VIEW);            
            #endregion

            #region IO�`�F�b�N�̏�����
            iochecForm.Initialize();
            #endregion
        }

        /// <summary>
        /// �t�H�[���̃R���X�g���N�^
        /// </summary>
        public systemConfigurationfrm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// �ʐM�|�[�g���琔�����擾���鏈��
        /// </summary>
        /// <param name="name">COM1�ȂǒʐM�|�[�g</param>
        /// <returns>COM1�Ȃ�1��Ԃ�</returns>
        private int portNameToInt(String name)
        {
            int ret = 0;

            try
            {
                ret = Int32.Parse(name.Replace("COM", ""));
            }
            catch
            {
                /* ���� */
            }

            return ret;
        }

        /// <summary>
        /// MemoryMonitor�{�^���̉����C�x���g
        /// �������[���j�^�[��ʂ�\��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMemoryMonitor_Click(object sender, EventArgs e)
        {
            iochecForm.Show();
        }

        /// <summary>
        /// �\���̍X�V����
        /// ���C���t�H�[���̃X���b�h����Ăяo�����
        /// �␳�l�E�^�C�~���O�E�Z���T�[���b�N�̏�Ԃ��X�V
        /// </summary>
        public void refresh()
        {
            #region �␳�l�E�^�C�~���O�l�̕\���X�V
            GridUpdate(systemConfiguration_CORR_GROUP_BASEMACHINE1View, SystemConstants.WORKID_TYPE_CORRECTDATA);
            GridUpdate(systemConfiguration_CORR_GROUP_FEED1View, SystemConstants.WORKID_TYPE_CORRECTDATA);
            GridUpdate(systemConfiguration_CORR_GROUP_SLIDER1View, SystemConstants.WORKID_TYPE_CORRECTDATA);
            GridUpdate(systemConfiguration_CORR_GROUP_STRIP1View, SystemConstants.WORKID_TYPE_CORRECTDATA);
            GridUpdate(systemConfiguration_TIMM_GROUP_SIDE1VIEW, SystemConstants.WORKID_TYPE_TIMINGDATA);
            GridUpdate(systemConfiguration_TIMM_GROUP_SIDE2VIEW, SystemConstants.WORKID_TYPE_TIMINGDATA);
            #endregion

            // ���o�^�u�̕\���X�V
            sensorLockDspRefresh();
        }

        /// <summary>
        /// �Z���T�[���b�N�\�����X�V���鏈��
        /// �y�[�W�A�{�^��������e���X�V
        /// </summary>
        private void sensorLockDspRefresh()
        {
            // ���b�N�{�^���ꗗ
            Button[] senBtns = new Button[]{
                btnSensorLock00, btnSensorLock01, btnSensorLock02, btnSensorLock03, btnSensorLock04, 
                btnSensorLock05, btnSensorLock06, btnSensorLock07, btnSensorLock08, btnSensorLock09,
                btnSensorLock10, btnSensorLock11, btnSensorLock12, btnSensorLock13, btnSensorLock14,
                btnSensorLock15, btnSensorLock16, btnSensorLock17, btnSensorLock18, btnSensorLock19,
                btnSensorLock20, btnSensorLock21, btnSensorLock22, btnSensorLock23, btnSensorLock24,
                btnSensorLock25, btnSensorLock26, btnSensorLock27, btnSensorLock28, btnSensorLock29
            };

            // �\����� (0�y�[�W�ڌŒ�)
            int dspPage = 0;

            for (int i = 0; i < senBtns.Length; i++)
            {
                int index = mainfrm.dspIndexCalc(dspPage, i);
                sensorLockDspUpate(index, senBtns[i]);
            }
        }

        /// <summary>
        /// �w�����ꂽ�Z���T�[���b�N�{�^���̏�Ԃ�Ԃ�����
        /// </summary>
        /// <param name="index">�{�^���z��̓Y����</param>
        /// <param name="ctl">�{�^��</param>
        private void sensorLockDspUpate(int index, Control ctl)
        {
            // �l���擾����
            sensorLockDspStruct senDsp;
            if (sensorDspMap.TryGetValue(index, out senDsp))
            {
                ctl.Visible = true;
                Program.MainForm.refreshControl(SystemConstants.STATUS_DISPLAY_MSG, senDsp.MsgID, ctl);

                if (senDsp.ReadBtn != null)
                {
                    mainfrm.CheckBtnAnd_ChangeColor(senDsp.ReadBtn.BtnID, ctl, senDsp.ReadBtn.ReverseFlg);
                }
            }
            else
            {
                ctl.Visible = false;
            }
        }

        /// <summary>
        /// �Z���T�[���b�N�C�x���g�����蓖�Ă鏈��
        /// </summary>
        /// <param name="btnSensorLockPosition"></param>
        /// <param name="btn"></param>
        private void setSensorLockBtn(int btnSensorLockPosition, Button btn)
        {
            btn.Click += new EventHandler(delegate(object sender, EventArgs args)
            {
                // �\����ʂ�0�Œ�
                int dspPage = 0;

                // map���犄���f�[�^���擾����
                sensorLockDspStruct senDsp;
                int index = mainfrm.dspIndexCalc(dspPage, btnSensorLockPosition);
                if (sensorDspMap.TryGetValue(index, out senDsp))
                {
                    if (senDsp.WriteBtn != null)
                    {
                        mainfrm.WritePushBtn(senDsp.WriteBtn.BtnID, senDsp.WriteBtn.BtnMotion);
                    }
                }
            });
        }

        /// <summary>
        /// �O���b�h�r���[�Ƀf�[�^��\�������鏈��
        /// </summary>
        /// <param name="view"></param>
        /// <param name="group"></param>
        private void viewDisp(DataGridView view, int type, int group)
        {
            int[] ID = null;

            view.CurrentCell = null;
            Program.DataController.GetMemryDataGroupList(type, group, ref ID);

            int rowCount = 0;
            string min = "";
            string max = "";
            string value = "";
            string name = "";
            string unit = "";

            // ID��null�̏ꍇ�A������
            if (ID == null) return;

            foreach (var workid in ID)
            {
                // �␳�l
                if (type == SystemConstants.WORKID_TYPE_CORRECTDATA)
                {

                    // ���̂��擾����(�ǂ���邩�H�j
                    name = Utility.GetMessageString(SystemConstants.CORRECT_MSG, workid);

                    // �͈͂��擾����
                    Program.DataController.GetCorrectDataRangeStr(workid, ref min, ref max);

                    // �l���擾����
                    Program.DataController.ReadCorrectDataStr(workid, ref value);

                    //�P�ʂ��擾����
                    Program.DataController.GetCorrectDataUnit(workid, ref unit);
                }
                // �^�C�~���O
                else
                {
                    // ���̂��擾����(�ǂ���邩�H�j
                    name = Utility.GetMessageString(SystemConstants.TIMMING_MSG, workid);

                    // �͈͂��擾����
                    Program.DataController.GetTimingDataRangeStr(workid, ref min, ref max);

                    // �l���擾����
                    Program.DataController.ReadTimingDataStr(workid, ref value);

                    //�P�ʂ��擾����
                    Program.DataController.GetTimingDataUnit(workid, ref unit);
                }
                // �l��ݒ肷��
                view.Rows.Add(new Object[] { workid, name, string.Format("{0} <-> {1}", new object[] { min, max }), value, unit });

                // �w�i�F��ݒ肷��
                if ((rowCount % 2) == 0)
                {
                    // �����̏ꍇ�A�w�i���F
                    view[1, rowCount].Style.BackColor = Color.White;
                }
                else
                {
                    // ��̏ꍇ�A�w�i���F
                    view[1, rowCount].Style.BackColor = Color.LightGreen;
                }

                rowCount++;
            }
        }

        /// <summary>
        /// �O���b�h�r���[�̃e�L�X�g�{�b�N�X���Enter�������ꂽ���̏���
        /// </summary>
        /// <param name="view"></param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CellValidating(DataGridView view, int type, object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (Program.SystemData.tachpanel == true) return;
            if (e.ColumnIndex < 3) return;

            int workid = Int32.Parse(view.Rows[e.RowIndex].Cells[0].Value.ToString()) + (e.ColumnIndex - 3);
            object value = e.FormattedValue;

            Program.MainForm.EnterDataGridView(type, workid, value);

#if MAINTELOG
            // �����e�i���X���O���X�V����
            refreshMainteLog();
#endif            
        }

        /// <summary>
        /// ����{�^�������������Ƃ��̃C�x���g
        /// �t�H�[���̕\��������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            Visible = false;
        }

        /// <summary>
        /// �t�H�[������Ă���Ƃ��̃C�x���g
        /// �t�H�[���̕\���������A���Ȃ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void systemConfigurationfrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Visible = false;
        }

        /// <summary>
        /// �O���b�h�r���[���̓C�x���g��ݒ肷�鏈��
        /// </summary>
        /// <param name="WorkIDType"></param>
        /// <param name="dataGridView"></param>
        private void setDataGridEvent(int WorkIDType, DataGridView dataGridView)
        {
            dataGridView.CellValidating += new DataGridViewCellValidatingEventHandler(delegate(object sender, DataGridViewCellValidatingEventArgs args)
            {
                CellValidating(dataGridView, WorkIDType, sender, args);
            });
        }

        /// <summary>
        /// �{�^���̏�Ԃ��擾���A��Ԃɉ����Ĕw�i�F��ύX���鏈��
        /// ON :Gray
        /// Off:Red
        /// </summary>
        /// <param name="BtnID"></param>
        /// <param name="Btn"></param>
        private void CheckBtnAnd_ChangeColor(int BtnID, Button Btn)
        {
            int status = 0;
            Program.DataController.ReadPushBtn(BtnID, ref status);

            if (status == SystemConstants.BTN_ON)
            {
                Btn.BackColor = Color.Red;
            }
            else
            {
                Btn.BackColor = Color.Gray;
            }

        }

        /// <summary>
        /// �ݒ�{�^�������������Ƃ��̃C�x���g
        /// �V�X�e���̐ݒ�𔽉f������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetting_Click(object sender, EventArgs e)
        {
            #region �p�X���[�h�ݒ�
            // �p�X���[�h�Ɗm�F���قȂ�ꍇ�̓G���[
            if (maskedTextPASSWORD.Text != maskedTextCHECK.Text)
            {
                Utility.ShowErrorMsg(SystemConstants.SYSTEM_MSG018);
                return;
            }

            //�p�X���[�h�̕ۑ�
            Program.SystemData.password = maskedTextPASSWORD.Text;
            #endregion

            #region �����@����ݒ�
            // �@�B�݂̂̏ꍇ
            if (radioButtonMain.Checked)
            {
                Program.SystemData.machineoperation = "machine";
            }
            // �����@�EPC�̏ꍇ
            else if (radioButtonMain_PC.Checked)
            {
                Program.SystemData.machineoperation = "both";
            }
            #endregion

            #region �ʐM�ݒ�
            Program.SystemData.comport = comboCOMPORT.Text;
            Program.SystemData.borate = comboBORATE.SelectedIndex + 1;
            Program.SystemData.dataBits = comboDATABIT.SelectedIndex + 1;
            Program.SystemData.stopBits = comboSTOPBIT.SelectedIndex + 1; ;
            Program.SystemData.parity = comboPARITY.SelectedIndex + 1;
            Program.SystemData.handshake = comboflow_control.SelectedIndex + 1;
            #endregion

            #region Language�ݒ�
            if (radioJAPANESE.Checked == true)
            {
                Program.SystemData.culture = "ja-JP";
            }
            else if (radioENGLISH.Checked == true)
            {
                Program.SystemData.culture = "en-US";
            }
            else
            {
                Program.SystemData.culture = "zh-CN";
            }
            #endregion

            #region �e���L�[�g�p�ݒ�
            Program.SystemData.tachpanel = checkBoxTACHPANEL.Checked;
            #endregion

            #region �ݒ��XML�t�@�C���ɕۑ�
            try
            {
                Program.SystemData.Save();
            }
            catch (Exception)
            {
                Utility.ShowErrorMsg(SystemConstants.SYSTEM_MSG013);
                return;
            }

            // �V�X�e���ݒ���s���܂����B
            Utility.ShowInfoMsg(SystemConstants.SYSTEM_MSG019);
            #endregion
        }

        /// <summary>
        /// �O���b�h�r���[�̓��e���X�V���鏈��
        /// </summary>
        /// <param name="view"></param>
        /// <param name="type"></param>
        private void GridUpdate(DataGridView view, int type)
        {
            int rowCount = view.Rows.Count;
            string value = "";

            //GridView�̍X�V��
            for (int y = 0; y < rowCount; y++)
            {
                int workid = Int32.Parse(view.Rows[y].Cells[0].Value.ToString());

                if (type == SystemConstants.WORKID_TYPE_CORRECTDATA)
                {
                    // �l���擾����
                    Program.DataController.ReadCorrectDataStr(workid, ref value);
                }
                else
                {
                    // �l���擾����
                    Program.DataController.ReadTimingDataStr(workid, ref value);
                }
                var cell = view.Rows[y].Cells[3];

                // �l���ҏW���łȂ���΁A�l��ύX����
                if (!cell.IsInEditMode)
                {
                    view.Rows[y].Cells[3].Value = value;
                }
            }

        }

        /// <summary>
        /// �t�H�[�����\�����ꂽ�Ƃ��ɐݒ���擾�\�����e���X�V
        /// �V�X�e���ݒ�̓��e�̍X�V
        /// �����e�i���X������e�̍X�V
        /// </summary>
        private void systemConfigurationfrm_VisibleChanged(object sender, EventArgs e)
        {
            // true->false�ɂȂ����Ƃ��́A�������Ȃ�
            if (Visible == false)
            {
                return;
            }

            string[] com = null;
            
            #region �p�X���[�h�ݒ�
            maskedTextPASSWORD.Text = Program.SystemData.password;
            maskedTextCHECK.Text = Program.SystemData.password;
            #endregion

            #region �����@����ݒ�
            switch (Program.SystemData.machineoperation)
            {
                case "machine":
                    radioButtonMain.Checked = true;
                    break;

                case "both":
                    radioButtonMain_PC.Checked = true;
                    break;
            }
            #endregion

            #region ����ݒ�
            switch (Program.SystemData.culture)
            {

                case "ja-JP":
                    radioJAPANESE.Checked = true;
                    break;
                case "en-US":
                    radioENGLISH.Checked = true;
                    break;
                case "zh-CN":
                    radioCHINESE.Checked = true;
                    break;
            }
            #endregion

            #region �ʐM�ݒ�

            // �g�p���Ă���PC��COM�|�[�g�ݒ���擾
            try
            {
                com = SerialPort.GetPortNames();
            }
            catch (Exception)
            {

            }
            comboCOMPORT.Items.Clear();

            foreach (var tmp in from c in com orderby portNameToInt(c) select c)
            {
                comboCOMPORT.Items.Add(tmp);
            }

            // COM�|�[�g�̒l��ݒ肷��
            comboCOMPORT.SelectedIndex = comboCOMPORT.Items.IndexOf(Program.SystemData.comport);
            
            // �{�[���[�g
            switch (Program.SystemData.borate)
            {
                case 1:
                    comboBORATE.SelectedIndex = 0;
                    break;

                case 2:
                    comboBORATE.SelectedIndex = 1;
                    break;

                case 3:
                    comboBORATE.SelectedIndex = 2;
                    break;

                case 4:
                    comboBORATE.SelectedIndex = 3;
                    break;

                case 5:
                    comboBORATE.SelectedIndex = 4;
                    break;

                case 6:
                    comboBORATE.SelectedIndex = 5;
                    break;

                case 7:
                    comboBORATE.SelectedIndex = 6;
                    break;
            }
            
            // �f�[�^�r�b�g
            switch (Program.SystemData.dataBits)
            {
                case 1:
                    comboDATABIT.SelectedIndex = 0;
                    break;
                case 2:
                    comboDATABIT.SelectedIndex = 1;
                    break;
            }
            
            // �X�g�b�v�r�b�g
            switch (Program.SystemData.stopBits)
            {
                case 1:
                    comboSTOPBIT.SelectedIndex = 0;
                    break;
                case 2:
                    comboSTOPBIT.SelectedIndex = 1;
                    break;
            }
            
            // �p���e�B�r�b�g
            switch (Program.SystemData.parity)
            {
                case 1:
                    comboPARITY.SelectedIndex = 0;
                    break;
                case 2:
                    comboPARITY.SelectedIndex = 1;
                    break;
                case 3:
                    comboPARITY.SelectedIndex = 2;
                    break;
            }
            
            // �t���[����
            switch (Program.SystemData.handshake)
            {
                case 1:
                    comboflow_control.SelectedIndex = 0;
                    break;
                case 2:
                    comboflow_control.SelectedIndex = 1;
                    break;
                case 3:
                    comboflow_control.SelectedIndex = 2;
                    break;
            }
            #endregion

            #region �e���L�[�g�p�ݒ�
            checkBoxTACHPANEL.Checked = Program.SystemData.tachpanel;
            #endregion

#if MAINTELOG
            // �����e�i���X���O���X�V����
            refreshMainteLog();
#endif
        }

        /// <summary>
        /// �t�H�[���\�����ꂽ�Ƃ��̃C�x���g
        /// �^�u�\���������ݒ�
        /// �����e�i���X�����̍��ڃ��X�g�̐ݒ�
        /// </summary>
        private void systemConfigurationfrm_Shown(object sender, EventArgs e)
        {
            #region �O���b�h�r���[�\��
            // �␳�l�^�u
            viewDisp(systemConfiguration_CORR_GROUP_BASEMACHINE1View, SystemConstants.WORKID_TYPE_CORRECTDATA, SystemConstants.CORR_GROUP_BASEMACHINE1);    // �x�[�X�}�V��
            viewDisp(systemConfiguration_CORR_GROUP_FEED1View, SystemConstants.WORKID_TYPE_CORRECTDATA, SystemConstants.CORR_GROUP_FEED1);                  // ���C���[�t�B�[�h
            viewDisp(systemConfiguration_CORR_GROUP_SLIDER1View, SystemConstants.WORKID_TYPE_CORRECTDATA, SystemConstants.CORR_GROUP_POSITION1);              // No1 �O��
            viewDisp(systemConfiguration_CORR_GROUP_STRIP1View, SystemConstants.WORKID_TYPE_CORRECTDATA, SystemConstants.CORR_GROUP_CUTSTRIP1);                // �X�g���b�v
            
            // �^�C�~���O�^�u
            viewDisp(systemConfiguration_TIMM_GROUP_SIDE1VIEW, SystemConstants.WORKID_TYPE_TIMINGDATA, SystemConstants.TIMM_GROUP_SIDE1);                   // 1��
            viewDisp(systemConfiguration_TIMM_GROUP_SIDE2VIEW, SystemConstants.WORKID_TYPE_TIMINGDATA, SystemConstants.TIMM_GROUP_SIDE2);                   // 2��
            #endregion

#if MAINTELOG
            // ���j�b�g�R���{�{�b�N�X��ǉ�����
            addUnitComboList();
            comboUnitName.SelectedIndex = 0;
#endif
        }

        /// <summary>
        /// �����e�i���X�����̓��e���X�V���鏈��
        /// </summary>
        private void refreshMainteLog()
        {
#if MAINTELOG
            string msgStr = "";
            int msgCode = 0;

            // �����𑁂�����ׂɁA�ĕ`����~����
            listMainteHistory.BeginUpdate();

            // �l����������
            listMainteHistory.Items.Clear();

            // �l���N���X����擾����
            MainteLogStruct[] mainteLogs;
            mainteLogs = Program.MainteLog.GetRecords();

            foreach (MainteLogStruct maintelog in mainteLogs)
            {
                // ���t�������쐬����
                msgStr = maintelog.DateStr + " " + maintelog.TimeStr + " ";

                // �����e�i���X���O�̃^�C�v���ɏ����𕪂���
                switch (maintelog.LogType)
                {
                    case SystemConstants.MAINTELOG_RECORD_TYPE_WORKID:
                        switch (maintelog.WorkIDType)
                        {
                            case SystemConstants.WORKID_TYPE_CORRECTDATA:
                                msgCode = SystemConstants.CORRECT_MSG;
                                break;
                            case SystemConstants.WORKID_TYPE_TIMINGDATA:
                                msgCode = SystemConstants.TIMMING_MSG;
                                break;
                            case SystemConstants.WORKID_TYPE_WORKDATA:
                                msgCode = SystemConstants.WORK_MSG;
                                break;
                        }

                        msgStr += "[" + Utility.GetMessageString(msgCode, maintelog.WorkID) + "] "
                            + maintelog.OldValue + " -> " + maintelog.NewValue;
                        break;
                    case SystemConstants.MAINTELOG_RECORD_TYPE_COMMENT:
                        msgStr += Utility.GetMessageString(SystemConstants.MAINTELOG_MSG, maintelog.Unit) + " ";
                        msgStr += maintelog.Comment;
                        break;
                    default:
                        msgStr = "";
                        break;
                }

                if (msgStr.Length > 0)
                    listMainteHistory.Items.Add(msgStr);
            }

            // �`����ĊJ����
            listMainteHistory.EndUpdate();
#endif
        }

        /// <summary>
        /// �ǉ��{�^���̉��������Ƃ��̃C�x���g
        /// �����e�i���X�����֎蓮�Ń��R�[�h��ǉ�
        /// </summary>
        private void btnAddMaintenanceRecord_Click(object sender, EventArgs e)
        {
#if MAINTELOG
            string commentStr = textAddMaintenanceRecord.Text.Trim();
            Program.MainteLog.AddRecord(selectedUnitMsgCode(), commentStr);

            // ���R�[�h���X�V����
            refreshMainteLog();

            // ���͗����󗓂ɂ���
            textAddMaintenanceRecord.Clear();
#endif
        }

        /// <summary>
        /// �����e�i���X�����̑I�΂ꂽ�C���f�b�N�X�̃��b�Z�[�W�R�[�h��Ԃ�����
        /// </summary>
        /// <returns>msgCode</returns>
        private int selectedUnitMsgCode()
        {
            int msgCode = 0;
#if MAINTELOG
            int indexNo;
            indexNo = comboUnitName.SelectedIndex;

            if (indexNo >= 0 && indexNo < showMainteUnitCodes.Count())
            {
                msgCode = showMainteUnitCodes[indexNo];
            }
#endif
            return msgCode;
        }

        /// <summary>
        /// �����e�i���X�����̍��ڂ��R���{�{�b�N�X�ɓo�^���鏈��
        /// </summary>
        private void addUnitComboList()
        {
#if MAINTELOG
            string unitStr;

            // ���X�g���N���A����
            comboUnitName.Items.Clear();

            // �R���{�{�b�N�X�Ƀf�[�^������
            for (int i = 0; i < showMainteUnitCodes.Count(); i++)
            {
                unitStr = Utility.GetMessageString(SystemConstants.MAINTELOG_MSG, showMainteUnitCodes[i]);
                comboUnitName.Items.Add(unitStr);
            }
            comboUnitName.SelectedIndex = 0;
#endif
        }

        /// <summary>
        /// �G�N�X�|�[�g�{�^�������������Ƃ��̃C�x���g
        /// �␳�l�E�^�C�~���O�f�[�^��CSV�t�@�C���Ƃ��ďo�͂���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExport_Click(object sender, EventArgs e)
        {
            // �����t�@�C������ݒ�
            string initialFileName = "exportData.csv";
            int groupType;
            if (sender == btnCorrectionOutput)
            {
                initialFileName = "correctData.csv";
                groupType = SystemConstants.WORKID_TYPE_CORRECTDATA;
            }
            else if (sender == btnTimmingOutput)
            {
                initialFileName = "timmingData.csv";
                groupType = SystemConstants.WORKID_TYPE_TIMINGDATA;
            }
            else
            {
                return;
            }

            // �t�@�C���_�C�A���O��\������
            SaveFileDialog sfd1 = new SaveFileDialog();
            sfd1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            sfd1.FileName = initialFileName;
            sfd1.DefaultExt = "csv";
            sfd1.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
            sfd1.FilterIndex = 1;
            sfd1.AddExtension = true;
            sfd1.OverwritePrompt = true;
            sfd1.RestoreDirectory = false;
            sfd1.CreatePrompt = false;

            int ret;
            if (sfd1.ShowDialog() == DialogResult.OK)
            {
                ret = Program.DataController.ExportCurrentData(groupType, sfd1.FileName);
            }

            sfd1.Dispose();
        }

        /// <summary>
        /// �o���N�f�[�^�G�N�X�|�[�g�{�^���̉��������Ƃ��̃C�x���g
        /// �o���N�f�[�^���w��t�H���_�֕ۑ�����
        /// </summary>
        private void btnBankExport_Click(object sender, EventArgs e)
        {
            string xmlFileName = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\JAM\\Alchemist\\bankdata.xml";

            //�t�@�C���_�C�A���O��\������
            SaveFileDialog sfd1 = new SaveFileDialog();
            sfd1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            sfd1.FileName = "bankdata.xml";
            sfd1.DefaultExt = "xml";
            sfd1.Filter = "xml files (*.xml)|*.xml";
            sfd1.FilterIndex = 1;
            sfd1.AddExtension = true;
            sfd1.OverwritePrompt = true;
            sfd1.RestoreDirectory = false;
            sfd1.CreatePrompt = false;

            if (sfd1.ShowDialog() == DialogResult.OK)
            {
                System.IO.File.Copy(xmlFileName, sfd1.FileName, true);
            }

            sfd1.Dispose();
        }

        /// <summary>
        /// �o���N�f�[�^�C���|�[�g�{�^���̉��������Ƃ��̃C�x���g
        /// �ǂݍ��񂾃o���N�f�[�^�Ńt�@�C����u��������
        /// </summary>
        private void btnBankImport_Click(object sender, EventArgs e)
        {
            string xmlFileName = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\JAM\\Alchemist\\bankdata.xml";
            BankDataStorage bankDataStorage = new BankDataStorage();

            //�t�@�C���_�C�A���O��\������
            OpenFileDialog ofd1 = new OpenFileDialog();
            ofd1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            ofd1.FileName = "bankdata.xml";
            ofd1.DefaultExt = "xml";
            //ofd1.Filter = "xml files (*.xml)|*.xml";
            ofd1.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            ofd1.FilterIndex = 1;
            ofd1.AddExtension = true;

            // �t�@�C����I�����ꂽ�ꍇ
            if (ofd1.ShowDialog() == DialogResult.OK)
            {
                // �t�@�C���̃X�L�[�}�`�F�b�N                
                if (bankDataStorage.Load(ofd1.FileName))
                {
                    // ���݂̃f�[�^��ۑ��o�b�N�A�b�v�t�@�C�������킹�č쐬�B
                    Program.DataController.BankDataSave();

                    // �I�������t�@�C�����I���W�i���ɃR�s�[
                    System.IO.File.Copy(ofd1.FileName,�@xmlFileName, true);

                    // �o���N�f�[�^�t�@�C���ēǍ���
                    Program.DataController.BankDataReLoad();

                    // selectedno��ݒ肷��
                    int selectno = 0;
                    mainfrm.BankNoWrite(selectno);
                    // �o���N�f�[�^�����[�h����
                    int result = mainfrm.BankDataLoad(selectno);
                    // �o���N�f�[�^���Z�[�u����
                    result = mainfrm.BankDataSave(selectno);

                    // �f�[�^���C���|�[�g���܂����B
                    Utility.ShowInfoMsg(SystemConstants.SYSTEM_MSG036);
                }
                else
                {
                    // �f�[�^���C���|�[�g�ł��܂���ł����B
                    Utility.ShowErrorMsg(SystemConstants.SYSTEM_MSG037);               
                }
            }
        }

        /// <summary>
        /// IO���j�^�[�{�^�������������Ƃ��̃C�x���g
        /// IO���j�^�[��ʂ�\������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIoMonitorDsp_Click(object sender, EventArgs e)
        {
            if (ioMonitorForm != null)
            {
                if (!ioMonitorForm.Visible)
                    ioMonitorForm.Show();
            }
        }

        private void btnLearnDataItemEdit_Click(object sender, EventArgs e)
        {
            if (learndataitemeditForm != null)
            {
                if (!learndataitemeditForm.Visible)
                {
                    learndataitemeditForm.Show();
                }
            }
        }

    }
}
