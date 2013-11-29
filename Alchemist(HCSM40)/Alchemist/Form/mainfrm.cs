using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Alchemist
{
    public partial class mainfrm : Form
    {
        // �X���b�h
        private Thread thread = null;
        delegate void RefreshDelegate();

        // �\���X�V�X���b�h
        private void monitorRefreshThread()
        {
            while (true)
            {
                // UI�X���b�h��refresh�֐������s������
                Invoke(new RefreshDelegate(refresh));

                // �J�E���^�t�H�[���̕\���X�V
                if (counterForm.Visible == true)
                {
                    Invoke(new RefreshDelegate(counterForm.refresh));
                }

                // �i��葀��̕\���X�V
                if (setupOperationForm.Visible == true)
                {
                    Invoke(new RefreshDelegate(setupOperationForm.refresh));
                }

                // �ʐM��ʂ̕\���X�V
                if (connectOperationForm.Visible == true)
                {
                    Invoke(new RefreshDelegate(connectOperationForm.refresh));
                }

                // ����ʂ̕\���X�V
                Invoke(new RefreshDelegate(errInfoMsgForm.refresh));

                // ���H�ڍאݒ���(�d���ؒf)�̕\���X�V
                if (workDetailItemFormwire1.Visible == true)
                {
                    Invoke(new RefreshDelegate(workDetailItemFormwire1.refresh));
                }

                // ���H�ڍאݒ���(�X�g���b�v1)�̕\���X�V
                if (workDetailItemFormstrip1.Visible == true)
                {
                    Invoke(new RefreshDelegate(workDetailItemFormstrip1.refresh));
                }

                // ���H�ڍאݒ���(�X�g���b�v2)�̕\���X�V
                if (workDetailItemFormstrip2.Visible == true)
                {
                    Invoke(new RefreshDelegate(workDetailItemFormstrip2.refresh));
                }
                
                // ���x�ݒ��ʂ̕\���X�V
                if (workDetailSpeedForm.Visible == true)
                {
                    Invoke(new RefreshDelegate(workDetailSpeedForm.refresh));
                }

                // ���H����ڍאݒ��ʂ̕\���X�V
                if (workDetailMotionForm.Visible == true)
                {
                    Invoke(new RefreshDelegate(workDetailMotionForm.refresh));
                }

                // �i��葀���ʂ̕\���X�V
                if (setupOperationForm.Visible == true)
                {
                    Invoke(new RefreshDelegate(setupOperationForm.refresh));
                }

                // �V�X�e�������ʂ̕\���X�V
                if (systemConfigrationForm.Visible == true)
                {
                    Invoke(new RefreshDelegate(systemConfigrationForm.refresh));
                }

                // �w�K�f�[�^�����ʂ̕\���X�V
                if (databaseForm.Visible)
                {
                    Invoke(new RefreshDelegate(databaseForm.refresh));
                }

                // IO���j�^�̍X�V
                if (ioMonitorForm.Visible)
                {
                    Invoke(new RefreshDelegate(ioMonitorForm.refresh));
                }

                Thread.Sleep(100);
            }
        }


        #region �q�t�H�[���Q�̐���
        private counterfrm counterForm = new counterfrm();			                	            /* �J�E���^�ڍ׉�� */
        private setupOperationfrm setupOperationForm = new setupOperationfrm();	    	            /* �i��葀���� */
        private workDetailItemfrmWIRE1 workDetailItemFormwire1 = new workDetailItemfrmWIRE1();      /* ���H�l�ڍאݒ���(�d���ؒf) */
        private workDetailItemfrmSTRIP1 workDetailItemFormstrip1 = new workDetailItemfrmSTRIP1();   /* ���H�l�ڍאݒ���(1���X�g���b�v) */
        private workDetailItemfrmSTRIP2 workDetailItemFormstrip2 = new workDetailItemfrmSTRIP2();   /* ���H�l�ڍאݒ���(2���X�g���b�v) */
        private workDetailSpeedfrm workDetailSpeedForm = new workDetailSpeedfrm();                  /* ���x�ݒ��� */
        private workDetailMotionfrm workDetailMotionForm = new workDetailMotionfrm();               /* ���H����ڍ׉�� */
        private machineOperationfrm machineOperationForm = new machineOperationfrm();               /* �@�B������ */
        private bankOperationfrm bankOperationForm = new bankOperationfrm();                        /* �o���N������ */
        private systemConfigurationfrm systemConfigrationForm = new systemConfigurationfrm();       /* �V�X�e���ݒ��� */
        private connectOperationfrm connectOperationForm = new connectOperationfrm();			    /* �ڑ��E�ؒf��� */
        private errInfoMsgfrm errInfoMsgForm = new errInfoMsgfrm();                                 /* ���E�G���[���b�Z�[�W��� */
        private passwordCollationfrm passwordcollationForm = new passwordCollationfrm();            /* �p�X���[�h�ƍ���� */
        private AboutBox1 aboutboxForm = new AboutBox1();                                           /* ���{�b�N�X��� */
        private iocheckfrm iocheckForm = new iocheckfrm();                                          /* ���������j�^��� */
        private ioMonitorfrm ioMonitorForm = new ioMonitorfrm();                                    /* �h�n���j�^�[��� */
        private learnDataSearchfrm databaseForm = new learnDataSearchfrm();                         /* �f�[�^�x�[�X��� */
        private learnDataItemEditfrm learnDataItemEditForm = new learnDataItemEditfrm();            /* �w�K�f�[�^���ڕҏW��� */
#if OMOIKANE
        private omoikane.mainfrm omoikaneForm = new omoikane.mainfrm();                             /* �v�����C���t�H�[�� */
#endif
        #endregion

        // ���H�����ʐݒ�table
        private Dictionary<int, workMotionStruct> map = new Dictionary<int, workMotionStruct>();

        // ���H����Visible�ݒ�\����
        private struct workMotionStruct
        {
            public Image image;
            public bool wire_Length;
            public bool strip_Length;
            public bool semi_Strip;
            public bool strip_Depth;
            public bool strip_Pullback;
            public bool crimp_Height;
            public bool crimp_Position;
            public bool seal_Insert_Length;
            public bool seal_Insert_Back;
        };

        // 1���̃{�^��ID�̔z��
        private int[] btnIdArray1 = new int[]{
                SystemConstants.STRIP1_BTN,
                //SystemConstants.CRIMP1_BTN,
                //SystemConstants.SEAL1_BTN,
                SystemConstants.SEMISTRIP1_BTN,
            };

        // 2���̃{�^��ID�̔z��
        private int[] btnIdArray2 = new int[]{
                SystemConstants.STRIP2_BTN,
                //SystemConstants.CRIMP2_BTN,
                //SystemConstants.SEAL2_BTN,
                SystemConstants.SEMISTRIP2_BTN,
            };

        // �R���X�g���N�^
        public mainfrm()
        {
            InitializeComponent();
        }

        // ����������
        private void Initialize()
        {
            workMotionStruct[] workMtnStruct = new workMotionStruct[14];

#if OMOIKANE
            // �v���̃N���X����ݒ肷��
            omoikaneForm.MX20Connector = Program.OmoikaneConnector;
            omoikaneForm.SystemData = Program.OmoikaneSystemData;
            // ��ʍX�V�X���b�h����p�ɔ�\���N��
            omoikaneForm.Show();            
#endif

            // �@�B���̃t�H�[���̕\���E��\���ݒ肷��
            formCustom();

            #region �t�H�[���̏�����
            aboutboxForm.Initialize();
            bankOperationForm.Initialize();
            connectOperationForm.Initialize();
            counterForm.Initialize();
            errInfoMsgForm.Initialize();
            iocheckForm.Initialize();
            ioMonitorForm.Initialize();
            machineOperationForm.Initialize();
            passwordcollationForm.Initialize();
            setupOperationForm.Initialize();
            systemConfigrationForm.Initialize();
            workDetailItemFormwire1.Initialize();
            workDetailItemFormstrip1.Initialize();
            workDetailItemFormstrip2.Initialize();
            workDetailMotionForm.Initialize();
            workDetailSpeedForm.Initialize();
            databaseForm.Initialize();
            learnDataItemEditForm.Initialize();
            #endregion

            // Io���j�^���V�X�e���ݒ�t�H�[���ɓ����
            systemConfigrationForm.ioMonitorForm = ioMonitorForm;

#if HCSM40    
            #region �e�L�X�g�{�b�N�X�ɃC�x���g��ݒ肷��
            SetTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.QTY_SET_COUNTER1, textQTY);  // QTY�{���ݒ�
            SetTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.LOT_SET_COUNTER1, textLOT);  // LOT�{���ݒ�
            SetTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.WIRE_LENGTH1, textWIRE_LENGTH_VALUE);    // �ؒf��
            SetTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.STRIP_LENGTH1, textSTRIP1_VALUE);    // �X�g���b�v1
            SetTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.STRIP_LENGTH2, textSTRIP2_VALUE);    // �X�g���b�v2
            SetTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.STRIP_SEMI_LENGTH1, textSEMI_STRIP1_VALUE); // �n�[�t�X�g���b�v1
            SetTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.STRIP_SEMI_LENGTH2, textSEMI_STRIP2_VALUE); // �n�[�t�X�g���b�v2
            SetTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.STRIP_DEPTH1, textSTRIP_DEPTH_VALUE);    // �؍��[��
            SetTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.STRIP_PULLBACK1, textSTRIP_PULLBACK_VALUE);   // �X�g���b�v�v���o�b�N
            #endregion

            #region �{�^���ɃC�x���g��ݒ肷��
            SetBtnEvent(SystemConstants.STRIP1_BTN, SystemConstants.BTN_PUSH, btnSTRIP1);
            SetBtnEvent(SystemConstants.STRIP2_BTN, SystemConstants.BTN_PUSH, btnSTRIP2);
            SetBtnEvent(SystemConstants.SEMISTRIP1_BTN, SystemConstants.BTN_PUSH, btnSEMISTRIP1);
            SetBtnEvent(SystemConstants.SEMISTRIP2_BTN, SystemConstants.BTN_PUSH, btnSEMISTRIP2);
            SetBtnEvent(SystemConstants.NORMAL_BTN, SystemConstants.BTN_ON, btnNORMAL);
            SetBtnEvent(SystemConstants.EJECT_BTN, SystemConstants.BTN_ON, btnEJECT);
            SetBtnEvent(SystemConstants.SAMPLE_BTN, SystemConstants.BTN_ON, btnSAMPLE);
            SetBtnEvent(SystemConstants.TEST_BTN, SystemConstants.BTN_ON, btnTEST);
            SetBtnEvent(SystemConstants.FREE_BTN, SystemConstants.BTN_ON, btnFREE);
            SetBtnEvent(SystemConstants.JOG_BTN, SystemConstants.BTN_ON, btnJOG);
            SetBtnEvent(SystemConstants.CYCLE_BTN, SystemConstants.BTN_ON, btnCYCLE);
            SetBtnEvent(SystemConstants.AUTO_BTN, SystemConstants.BTN_ON, btnAUTO);
            SetBtnEvent(SystemConstants.LOT_INTERVAL1_BTN, SystemConstants.BTN_PUSH, btnAUTOEXIT);
            SetBtnEvent(SystemConstants.QTY_COUNTER_RESET1_BTN, SystemConstants.BTN_ON, btnQTYReset);
            SetBtnEvent(SystemConstants.LOT_COUNTER_RESET1_BTN, SystemConstants.BTN_ON, btnLOTReset);
            #endregion

            #region �e���L�[���͗p�̃N���b�N�C�x���g
            ClickTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.TENKEY_INPUT_CHECK, SystemConstants.QTY_SET_COUNTER1, textQTY);
            ClickTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.TENKEY_INPUT_DATA, SystemConstants.LOT_SET_COUNTER1, textLOT);
            ClickTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.TENKEY_INPUT_DATA, SystemConstants.WIRE_LENGTH1, textWIRE_LENGTH_VALUE);
            ClickTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.TENKEY_INPUT_DATA, SystemConstants.STRIP_LENGTH1, textSTRIP1_VALUE);
            ClickTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.TENKEY_INPUT_DATA, SystemConstants.STRIP_LENGTH2, textSTRIP2_VALUE);
            ClickTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.TENKEY_INPUT_DATA, SystemConstants.STRIP_PULLBACK1, textSTRIP_PULLBACK_VALUE);
            ClickTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.TENKEY_INPUT_DATA, SystemConstants.STRIP_DEPTH1, textSTRIP_DEPTH_VALUE);
            ClickTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.TENKEY_INPUT_DATA, SystemConstants.STRIP_SEMI_LENGTH1, textSEMI_STRIP1_VALUE);
            ClickTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA,  SystemConstants.TENKEY_INPUT_DATA, SystemConstants.STRIP_SEMI_LENGTH2, textSEMI_STRIP2_VALUE);
            #endregion
#endif

#if HCSM40
            #region ���H���삲�Ƃ̐ݒ�

            // ���H���삪1���̂Ԃ؂�̏ꍇ�̐ݒ�
            workMtnStruct[0].image = Alchemist.Properties.Resources.Fig1_0;
            workMtnStruct[0].wire_Length = true;
            workMtnStruct[0].strip_Length = false;
            workMtnStruct[0].semi_Strip = false;
            workMtnStruct[0].strip_Depth = false;
            workMtnStruct[0].strip_Pullback = false;
            workMtnStruct[0].crimp_Height = false;
            workMtnStruct[0].crimp_Position = false;
            workMtnStruct[0].seal_Insert_Length = false;
            workMtnStruct[0].seal_Insert_Back = false;

            // ���H���삪2���̂Ԃ؂�̏ꍇ�̐ݒ�
            workMtnStruct[1].image = Alchemist.Properties.Resources.Fig2_0;
            workMtnStruct[1].wire_Length = true;
            workMtnStruct[1].strip_Length = false;
            workMtnStruct[1].semi_Strip = false;
            workMtnStruct[1].strip_Depth = false;
            workMtnStruct[1].strip_Pullback = false;
            workMtnStruct[1].crimp_Height = false;
            workMtnStruct[1].crimp_Position = false;
            workMtnStruct[1].seal_Insert_Length = false;
            workMtnStruct[1].seal_Insert_Back = false;

            // ���H���삪1���̃X�g���b�v�̏ꍇ�̐ݒ�
            workMtnStruct[2].image = Alchemist.Properties.Resources.Fig1_1;
            workMtnStruct[2].wire_Length = true;
            workMtnStruct[2].strip_Length = true;
            workMtnStruct[2].semi_Strip = false;
            workMtnStruct[2].strip_Depth = true;
            workMtnStruct[2].strip_Pullback = true;
            workMtnStruct[2].crimp_Height = false;
            workMtnStruct[2].crimp_Position = false;
            workMtnStruct[2].seal_Insert_Length = false;
            workMtnStruct[2].seal_Insert_Back = false;

            // ���H���삪2���̃X�g���b�v�̏ꍇ�̐ݒ�
            workMtnStruct[3].image = Alchemist.Properties.Resources.Fig2_1;
            workMtnStruct[3].wire_Length = true;
            workMtnStruct[3].strip_Length = true;
            workMtnStruct[3].semi_Strip = false;
            workMtnStruct[3].strip_Depth = true;
            workMtnStruct[3].strip_Pullback = true;
            workMtnStruct[3].crimp_Height = false;
            workMtnStruct[3].crimp_Position = false;
            workMtnStruct[3].seal_Insert_Length = false;
            workMtnStruct[3].seal_Insert_Back = false;

            // ���H���삪1���̃n�[�t�X�g���b�v�̏ꍇ�̐ݒ�
            workMtnStruct[4].image = Alchemist.Properties.Resources.Fig1_9;
            workMtnStruct[4].wire_Length = true;
            workMtnStruct[4].strip_Length = true;
            workMtnStruct[4].semi_Strip = true;
            workMtnStruct[4].strip_Depth = true;
            workMtnStruct[4].strip_Pullback = true;
            workMtnStruct[4].crimp_Height = false;
            workMtnStruct[4].crimp_Position = false;
            workMtnStruct[4].seal_Insert_Length = false;
            workMtnStruct[4].seal_Insert_Back = false;

            // ���H���삪2���̃n�[�t�X�g���b�v�̏ꍇ�̐ݒ�
            workMtnStruct[5].image = Alchemist.Properties.Resources.Fig2_9;
            workMtnStruct[5].wire_Length = true;
            workMtnStruct[5].strip_Length = true;
            workMtnStruct[5].semi_Strip = true;
            workMtnStruct[5].strip_Depth = true;
            workMtnStruct[5].strip_Pullback = true;
            workMtnStruct[5].crimp_Height = false;
            workMtnStruct[5].crimp_Position = false;
            workMtnStruct[5].seal_Insert_Length = false;
            workMtnStruct[5].seal_Insert_Back = false;
            #endregion
#endif

            #region ���H���삲�Ƃ̃e�[�u���ݒ�          
            map.Add(0, workMtnStruct[0]);       // 1���̂Ԃ؂�̃e�[�u����ݒ�
            map.Add(1, workMtnStruct[1]);       // 2���̂Ԃ؂�̃e�[�u����ݒ�
            map.Add(2, workMtnStruct[2]);       // 1���̃X�g���b�v�̃e�[�u����ݒ�
            map.Add(3, workMtnStruct[3]);       // 2���̃X�g���b�v�̃e�[�u����ݒ�
            map.Add(6, workMtnStruct[4]);       // 1���̃X�g���b�v�A�����̃e�[�u����ݒ�
            map.Add(7, workMtnStruct[5]);       // 2���̃X�g���b�v�A�����̃e�[�u����ݒ�
            map.Add(8, workMtnStruct[6]);       // 1���̖h���̃e�[�u����ݒ�
            map.Add(9, workMtnStruct[7]);       // 2���̖h���̃e�[�u����ݒ�
            map.Add(10, workMtnStruct[8]);      // 1���̃X�g���b�v�A�h���̃e�[�u����ݒ�
            map.Add(11, workMtnStruct[9]);      // 2���̃X�g���b�v�A�h���̃e�[�u����ݒ�
            map.Add(14, workMtnStruct[10]);     // 1���̃X�g���b�v�A�h���A�����̃e�[�u����ݒ�
            map.Add(15, workMtnStruct[11]);     // 2���̃X�g���b�v�A�h���A�����̃e�[�u����ݒ�
            map.Add(18, workMtnStruct[12]);     // 1���̃X�g���b�v�A�n�[�t�X�g���b�v�̃e�[�u����ݒ�
            map.Add(19, workMtnStruct[13]);     // 2���̃X�g���b�v�A�n�[�t�X�g���b�v�̃e�[�u����ݒ�
            #endregion

            // �t�H�[�����̃e�L�X�g�{�b�N�X�̒�����10�ɂ���B
            SetTextBoxLength(this, 10);

            // �`��X���b�h���J�n����
            thread = new Thread(new ThreadStart(monitorRefreshThread));
            thread.Start();
        }

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
        /// �Z���T�[�s
        /// +--+--+--+--+--+
        /// | 0| 2| 4| 6| 8|
        /// +--+--+--+--+--+
        /// | 1| 3| 5| 7| 9|
        /// +--+--+--+--+--+
        /// 
        /// ����s (�J�n�ԍ��́AMOTION_STATUS_BOX_START �Œ�`)
        /// +--+--+--+--+--+
        /// |30|32|34|36|38|
        /// +--+--+--+--+--+
        /// |31|33|35|37|39|
        /// +--+--+--+--+--+
        /// 
        /// </summar>y
        /// 
        private struct statusDspStruct
        {
            public int BtnID;
            public int MsgID;
            public bool ReverseFlg;
        }
        private Dictionary<int, statusDspStruct> statusDspMap = new Dictionary<int, statusDspStruct>();

        /// <summary>
        /// ��ԕ\�����ڃf�[�^��ǉ�����
        /// </summary>
        /// <param name="DspTimming"></param>
        /// <param name="DspPosition"></param>
        /// <param name="BtnID"></param>
        private void statusDspMapAdd(int DspTimming, int DspPosition, int BtnID, int MsgID, bool ReverseFlg = false)
        {
            // �������s�K�؂ȏꍇ�A��O�𔭐�������
            if (DspTimming < 0 || DspPosition < 0 || DspPosition > 99) new ArgumentException("DspTiming or DspPosition is under");

            // �y�[�W�̍ő吔�����߂�
            if (DspPosition < SystemConstants.MOTION_STATUS_BOX_START)
            {
                if (DspTimming > statusDspSensorPgMax) statusDspSensorPgMax = DspTimming;
            }
            else
            {
                if (DspTimming > statusDspMotionPgMax) statusDspMotionPgMax = DspTimming;
            }

            // �C���f�b�N�X�𐶐�����
            int index = dspIndexCalc(DspTimming, DspPosition);

            // �\���p�f�[�^�ɒǉ�����
            statusDspStruct dsp = new statusDspStruct();
            dsp.BtnID = BtnID;
            dsp.MsgID = MsgID;
            dsp.ReverseFlg = ReverseFlg;

            statusDspMap.Add(index, dsp);
        }

        public static int dspIndexCalc(int DspTimming, int DspPosition)
        {
            return (DspTimming * 100) + DspPosition;
        }


        /// <summary>
        /// ���C���t�H�[��������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mainfrm_Load(object sender, EventArgs e)
        {
            Initialize();
        }

        /// <summary>
        /// �t�H�[��������ꂽ���̏���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mainfrm_FormClosed(object sender, FormClosedEventArgs e)
        {
#if OMOIKANE
            // �v���t�H�[�����I������
            omoikaneForm.FormCloseProcess();
#endif

            // �\���������Ɏ~�߂�
            if (thread != null)
            {
                // �X���b�h���~����
                thread.Abort();
                thread.Join();
            }

            // �@�햼��ۑ�����
            Program.SystemData.machineid = Program.DataController.GetMachineName();

            try
            {
                Program.SystemData.Save();
            }
            catch
            {
                /* ��O�𖳎� */
            }

            #region �t�H�[����j������
            counterForm.Dispose();
            setupOperationForm.Dispose();
            workDetailItemFormwire1.Dispose();
            workDetailItemFormstrip1.Dispose();
            workDetailItemFormstrip2.Dispose();
            workDetailSpeedForm.Dispose();
            workDetailMotionForm.Dispose();
            machineOperationForm.Dispose();
            bankOperationForm.Dispose();
            systemConfigrationForm.Dispose();
            connectOperationForm.Dispose();
            errInfoMsgForm.Dispose();
            passwordcollationForm.Dispose();
            databaseForm.Dispose();
            learnDataItemEditForm.Dispose();
            #endregion

            // �f�[�^�R���g���[�����������
            Program.DataController.Dispose();
            Dispose();
        }

        // �`�揈��
        private void refresh()
        {
            int selectedNo = 0;
            string bankComment = "";


            #region �ڑ���Ԃ̎擾
            if (Program.DataController.IsConnect() == true)
            {
                // true: �w�iGreen �����F White ���� ONLINE
                lblOFFLINE.Text = Utility.GetMessageString(SystemConstants.SYSTEM_MSG, SystemConstants.SYSTEM_MSG029);
                lblOFFLINE.BackColor = System.Drawing.Color.Green;
                lblOFFLINE.ForeColor = System.Drawing.Color.White;
                panelOFFLINE.BackColor = System.Drawing.Color.Green;
            }
            else
            {
                // false: �w�i Red �����F Black ���� OFFLINE
                lblOFFLINE.Text = Utility.GetMessageString(SystemConstants.SYSTEM_MSG, SystemConstants.SYSTEM_MSG030);
                lblOFFLINE.BackColor = System.Drawing.Color.Red;
                lblOFFLINE.ForeColor = System.Drawing.Color.Black;
                panelOFFLINE.BackColor = System.Drawing.Color.Red;
            }
            #endregion

            #region �X�e�[�^�X�ɂ���ă{�^���̉摜��ύX
            CheckBtnAnd_ChangePicture(SystemConstants.STRIP1_BTN, btnSTRIP1, Alchemist.Properties.Resources.StripAON, Alchemist.Properties.Resources.StripAOFF);
            CheckBtnAnd_ChangePicture(SystemConstants.SEMISTRIP1_BTN, btnSEMISTRIP1, Alchemist.Properties.Resources.HalfAON, Alchemist.Properties.Resources.HalfAOFF);
            CheckBtnAnd_ChangePicture(SystemConstants.STRIP2_BTN, btnSTRIP2, Alchemist.Properties.Resources.StripBON, Alchemist.Properties.Resources.StripBOFF);
            CheckBtnAnd_ChangePicture(SystemConstants.SEMISTRIP2_BTN, btnSEMISTRIP2, Alchemist.Properties.Resources.HalfBON, Alchemist.Properties.Resources.HalfBOFF);
            #endregion

            // ���H����̉�ʐݒ���s��
            workMotionDisplay();

            // �Z���T�[��ԕ\���̍X�V���s��
            statusDisplayRefresh();

#if HCSM40
            #region �{�^����ԕ\��
            // ���샂�[�h
            CheckBtnAnd_ChangeColor(SystemConstants.NORMAL_BTN, btnNORMAL);
            CheckBtnAnd_ChangeColor(SystemConstants.EJECT_BTN, btnEJECT);
            CheckBtnAnd_ChangeColor(SystemConstants.SAMPLE_BTN, btnSAMPLE);
            CheckBtnAnd_ChangeColor(SystemConstants.TEST_BTN, btnTEST);
            CheckBtnAnd_ChangeColor(SystemConstants.FREE_BTN, btnFREE);
            
            // �T�C�N�����[�h
            CheckBtnAnd_ChangeColor(SystemConstants.JOG_BTN, btnJOG);
            CheckBtnAnd_ChangeColor(SystemConstants.CYCLE_BTN, btnCYCLE);
            CheckBtnAnd_ChangeColor(SystemConstants.AUTO_BTN, btnAUTO);
            
            // ���H���[�h4
            CheckBtnAnd_ChangeColor(SystemConstants.LOT_INTERVAL1_BTN, btnAUTOEXIT);
            #endregion

            #region ���l�\���X�V
            // �J�E���^�[
            refreshControl(SystemConstants.TOTAL_COUNTER1, lblTOTAL2);
            refreshControl(SystemConstants.QTY_COUNTER1, lblQTY2);
            refreshControl(SystemConstants.LOT_COUNTER1, lblLOT2);
            refreshControl(SystemConstants.QTY_SET_COUNTER1, textQTY);
            refreshControl(SystemConstants.LOT_SET_COUNTER1, textLOT);
            
            // �^�N�g
            refreshControl(SystemConstants.MACHINE_TACT1, lblTact4);
            
            // ���H�l
            refreshControl(SystemConstants.WIRE_LENGTH1, textWIRE_LENGTH_VALUE);
            refreshControl(SystemConstants.STRIP_LENGTH1, textSTRIP1_VALUE);
            refreshControl(SystemConstants.STRIP_LENGTH2, textSTRIP2_VALUE);
            refreshControl(SystemConstants.STRIP_SEMI_LENGTH1, textSEMI_STRIP1_VALUE);
            refreshControl(SystemConstants.STRIP_SEMI_LENGTH2, textSEMI_STRIP2_VALUE);
            refreshControl(SystemConstants.STRIP_DEPTH1, textSTRIP_DEPTH_VALUE);
            refreshControl(SystemConstants.STRIP_PULLBACK1, textSTRIP_PULLBACK_VALUE);
            #endregion
#endif

#if OMOIKANE
            // CFM�̃e�B�[�`��Ԃ𔽉f������

            int omoikaneStatus = omoikane.SystemConstants.CFM_STATUS_NO_SHOW;
            if (Program.OmoikaneConnector.IsConnect())
                omoikaneStatus = Program.OmoikaneConnector.GetCFMStatusData();
            CheckBtnAnd_ChangeColor((omoikaneStatus == omoikane.SystemConstants.CFM_STATUS_TEACH), btnCFMTeach);
#endif

            int workMode = Program.DataController.GetWorkMode();

            #region ���H�f�[�^�ۑ��؂�ւ�
            switch (workMode)
            {
                // �o���N���̏���
                case SystemConstants.WORKMODE_BANK:
                    // ���ݑI������Ă���BankNo���擾
                    Program.DataController.BankNoRead(ref selectedNo);
                    // �o���N�R�����g���擾
                    Program.DataController.BankDataCommentRead(selectedNo, ref bankComment);
                    // ���C����ʂ�bankcomment��ݒ�
                    if (!textBankComment.Focused) textBankComment.Text = bankComment;
                    // �{�^���̐F��ύX����
                    CheckBtnAnd_ChangeColor(true, btnBANK);
                    CheckBtnAnd_ChangeColor(false, btnLEARN);
                    break;
                // �w�K���[�h���̏���
                case SystemConstants.WORKMODE_LEARN:
                    // ���C����ʂ�bankcomment��ݒ�
                    if (!textBankComment.Focused) textBankComment.Text = "";
                    // �{�^���̐F��ύX����
                    CheckBtnAnd_ChangeColor(false, btnBANK);
                    CheckBtnAnd_ChangeColor(true, btnLEARN);
                    break;
            }
            #endregion

            #region �}�V������ݒ肪�{�̂̏ꍇ�́A����{�^���͔�\���ɂ���B
            if (Program.SystemData.machineoperation == "machine")
            {
                // �����ʂ�����
                if (machineOperationForm.Visible != false)
                {
                    machineOperationForm.Visible = false;
                }

                // �p�l��������
                if (pnlMachineOperate.Visible != false)
                {
                    pnlMachineOperate.Visible = false;
                }
            }
            else
            {
                if (pnlMachineOperate.Visible != true)
                {
                    pnlMachineOperate.Visible = true;
                }
            }
            #endregion
        }

        private void tsmiVersion_Click(object sender, EventArgs e)
        {
            aboutboxForm.Show();
        }

        private void btnDetail_Click(object sender, EventArgs e)
        {
            counterForm.Show();
        }

        private void mainfrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = false;
        }

        private void btnINCHING1_Click(object sender, EventArgs e)
        {
            setupOperationForm.Show();
        }

        private void btnWIRE1_Detail_Click(object sender, EventArgs e)
        {
            workDetailItemFormwire1.Show();
        }

        private void btnSTRIP1_Detail_Click(object sender, EventArgs e)
        {
            workDetailItemFormstrip1.Show();
        }

        private void btnCRIMP1_Detail_Click(object sender, EventArgs e)
        {
            //workDetailItemFormcrimp1.Show();
        }

        private void btnSEAL1_Detail_Click(object sender, EventArgs e)
        {
            //workDetailItemFormseal1.Show();
        }

        private void btnSTRIP2_Detail_Click(object sender, EventArgs e)
        {
            workDetailItemFormstrip2.Show();
        }

        private void btnCRIMP2_Detail_Click(object sender, EventArgs e)
        {
            //workDetailItemFormcrimp2.Show();
        }

        private void btnSEAL2_Detail_Click(object sender, EventArgs e)
        {
            //workDetailItemFormseal2.Show();
        }

        private void btnSpeedsetting_Click(object sender, EventArgs e)
        {
            workDetailSpeedForm.Show();
        }

        private void btnWorkMotion_Click(object sender, EventArgs e)
        {
            workDetailMotionForm.Show();
        }

        private void btnOperation_Click(object sender, EventArgs e)
        {
            machineOperationForm.Show();
        }

        private void btnBANK_Click(object sender, EventArgs e)
        {
            bankOperationForm.Show();
        }

        private void btnDataBase_Click(object sender, EventArgs e)
        {
            databaseForm.Show();
        }

        private void btnManagement_setting_Click(object sender, EventArgs e)
        {
            // �p�X���[�h����v������A�V�X�e���ݒ���J��
            passwordCollationfrm pass = new passwordCollationfrm();

            // �p�X���[�h����̏ꍇ�\��
            if (string.IsNullOrEmpty(Program.SystemData.password))
            {
                systemConfigrationForm.Show();
            }
            // �p�X���[�h���ݒ肳��Ă���ꍇ
            else
            {
                // �p�X���[�h���͉�ʂŏƍ���I�񂾏ꍇ
                if (pass.ShowDialog() == DialogResult.OK)
                {
                    // �p�X���[�h����v
                    if (pass.CheckPassword() == true)
                    {
                        systemConfigrationForm.Show();
                    }
                    // �p�X���[�h���s��v
                    else
                    {
                        Utility.ShowErrorMsg(SystemConstants.SYSTEM_MSG017);
                    }
                }
            }
        }

        private void lblOFFLINE_Click(object sender, EventArgs e)
        {
            connectOperationForm.Show();
        }

        private void workMotionDisplay()
        {
            int key1 = 0;
            int key2 = 0;
            int btnStatus = 0;

            //�n�b�V���e�[�u����key�l�Z�o���@
            //���L�̕\�ɑΉ������l���擾
            //
            //  100              10              1
            //�@�n�[�t�@ �b �@�@�@�@�@ �b 1��or�@�@�@�@  �b�@�@�@�b
            //�X�g���b�v �b �X�g���b�v �b 2��(2�����Ɓ�) �b2�i�� �b10�i��
            // �\�\�\�\�\�b�\�\�\�\�\�\�b�\�\�\�\�\�\�\�\�b�\�\�\�b�\�\�\
            //�@�@�@�@�@ �b �@�@�@�@�@ �b�@�@�@�@�@�@�@�@�b00000 �b  0
            //�@�@�@�@�@ �b �@�@�@�@�@ �b  �@�@ �� �@�@�@�b00001 �b  1
            //�@�@�@�@�@ �b �@�@���@�@ �b  �@�@�@�@�@�@�@�b00010 �b  2
            //�@�@�@�@�@ �b �@�@���@�@ �b  �@�@ �� �@�@�@�b00011 �b  3
            //�@�@���@�@ �b �@�@���@�@ �b  �@�@�@�@�@�@�@�b00110 �b  6
            //�@�@���@�@ �b �@�@���@�@ �b  �@�@ �� �@�@�@�b00111 �b  7
            
            // 1����key�l�Z�o
            for (int i = 0; i < btnIdArray1.Length; i++)
            {
                Program.DataController.ReadPushBtn(btnIdArray1[i], ref btnStatus);
                if (btnStatus == SystemConstants.BTN_ON)
                {
                    key1 = key1 + (2 << i);
                }
            }

            // 2����key�l�Z�o
            for (int i = 0; i < btnIdArray2.Length; i++)
            {
                Program.DataController.ReadPushBtn(btnIdArray2[i], ref btnStatus);
                if (btnStatus == SystemConstants.BTN_ON)
                {
                    key2 = key2 + (2 << i);
                }
            }
            // key2��+1����(2����key�l��1����+1)
            key2 = key2 + 1;

            try
            {
                pictureBoxSIDE1.Image = map[key1].image;                                    // 1���̉摜��ݒ�
                lblWIRE_LENGTH_VALUE.Visible = map[key1].wire_Length;                       // �ؒf�����x����Visible��ݒ�
                textWIRE_LENGTH_VALUE.Visible = map[key1].wire_Length;                      // �ؒf���e�L�X�g��Visible��ݒ�
                lblSTRIP1_VALUE.Visible = map[key1].strip_Length;                           // 1���̃X�g���b�v���x����Visible��ݒ�
                textSTRIP1_VALUE.Visible = map[key1].strip_Length;                          // 1���̃X�g���b�v�e�L�X�g��Visible��ݒ�
                lblSEMI_STRIP1_VALUE.Visible = map[key1].semi_Strip;                        // 1���̃n�[�t�X�g���b�v���x����Visible��ݒ�
                textSEMI_STRIP1_VALUE.Visible = map[key1].semi_Strip;                       // 1���̃n�[�t�X�g���b�v�e�L�X�g��Visible��ݒ�
                lblCRIMP_HEIGHT1_VALUE.Visible = map[key1].crimp_Height;                    // 1����CH���x����Visible��ݒ�
                textCRIMP_HEIGHT1_VALUE.Visible = map[key1].crimp_Height;                   // 1����CH�e�L�X�g��Visible��ݒ�
                lblCRIMP_POSITION1_VALUE.Visible = map[key1].crimp_Position;                // 1���̈����ʒu���x����Visible��ݒ�
                textCRIMP_POSITION1_VALUE.Visible = map[key1].crimp_Position;               // 1���̈����ʒu�e�L�X�g��Visible��ݒ�
                lblSEAL_INSERT_LENGTH1_VALUE.Visible = map[key1].seal_Insert_Length;        // 1���̑}���ʃ��x����Visible��ݒ�
                textSEAL_INSERT_LENGTH1_VALUE.Visible = map[key1].seal_Insert_Length;       // 1���̑}���ʃe�L�X�g��Visible��ݒ�
                lblSEAL_INSERT_BACK1_VALUE.Visible = map[key1].seal_Insert_Back;            // 1���̖߂��ʃ��x����Visible��ݒ�
                textSEAL_INSERT_BACK1_VALUE.Visible = map[key1].seal_Insert_Back;           // 1���̖߂��ʃe�L�X�g��Visible��ݒ�

                pictureBoxSIDE2.Image = map[key2].image;                                    // 2���̉摜��ݒ�
                lblWIRE_LENGTH_VALUE.Visible = map[key2].wire_Length;                       // �ؒf�����x����Visible��ݒ�
                textWIRE_LENGTH_VALUE.Visible = map[key2].wire_Length;                      // �ؒf���e�L�X�g��Visible��ݒ�
                lblSTRIP2_VALUE.Visible = map[key2].strip_Length;                           // 2���̃X�g���b�v���x����Visible��ݒ�
                textSTRIP2_VALUE.Visible = map[key2].strip_Length;                          // 2���̃X�g���b�v�e�L�X�g��Visible��ݒ�
                lblSEMI_STRIP2_VALUE.Visible = map[key2].semi_Strip;                        // 2���̃n�[�t�X�g���b�v���x����Visible��ݒ�
                textSEMI_STRIP2_VALUE.Visible = map[key2].semi_Strip;                       // 2���̃n�[�t�X�g���b�v�e�L�X�g��Visible��ݒ�
                lblCRIMP_HEIGHT2_VALUE.Visible = map[key2].crimp_Height;                    // 2����CH���x����Visible��ݒ�
                textCRIMP_HEIGHT2_VALUE.Visible = map[key2].crimp_Height;                   // 2����CH�e�L�X�g��Visible��ݒ�
                lblCRIMP_POSITION2_VALUE.Visible = map[key2].crimp_Position;                // 2���̈����ʒu���x����Visible��ݒ�
                textCRIMP_POSITION2_VALUE.Visible = map[key2].crimp_Position;               // 2���̈����ʒu�e�L�X�g��Visible��ݒ�
                lblSEAL_INSERT_LENGTH2_VALUE.Visible = map[key2].seal_Insert_Length;        // 2���̑}���ʃ��x����Visible��ݒ�
                textSEAL_INSERT_LENGTH2_VALUE.Visible = map[key2].seal_Insert_Length;       // 2���̑}���ʃe�L�X�g��Visible��ݒ�
                lblSEAL_INSERT_BACK2_VALUE.Visible = map[key2].seal_Insert_Back;            // 2���̖߂��ʃ��x����Visible��ݒ�
                textSEAL_INSERT_BACK2_VALUE.Visible = map[key2].seal_Insert_Back;           // 2���̖߂��ʃe�L�X�g��Visible��ݒ�

                // 1���A2���ŃX�g���b�v�{�^���̏�ԂŁA�ǂ��炩���\���ɂȂ��Ă����ꍇ�A�\���ɂ���
                lblSTRIP_DEPTH_VALUE.Visible = map[key1].strip_Depth || map[key2].strip_Depth;  // �؍��[�����x����Visible��ݒ�
                textSTRIP_DEPTH_VALUE.Visible = map[key1].strip_Depth || map[key2].strip_Depth; // �؍��[���e�L�X�g��Visible��ݒ�
                lblSTRIP_PULLBACK_VALUE.Visible = map[key1].strip_Pullback || map[key2].strip_Pullback; // �v���o�b�N���x����Visible��ݒ�
                textSTRIP_PULLBACK_VALUE.Visible = map[key1].strip_Pullback || map[key2].strip_Pullback;    // �v���o�b�N�e�L�X�g��Visible��ݒ�
            }
            catch
            {
                /* ��O�𖳎��i�f�[�^�̎擾�^�C�~���O�Ŗ{���Ƃ肤��͂��̂Ȃ��g�ݍ��킹�������ꍇ�̑΍�j */
            }

            /*
            if ((key1 & 0x0002) == 0 && (key2 & 0x0002) == 0)
            {
                lblSTRIP_DEPTH_VALUE.Visible = false;                                   // �؍��[�����x����Visible��ݒ�
                textSTRIP_DEPTH_VALUE.Visible = false;                                  // �؍��[���e�L�X�g��Visible��ݒ�
                lblSTRIP_PULLBACK_VALUE.Visible = false;                                // �v���o�b�N���x����Visible��ݒ�
                textSTRIP_PULLBACK_VALUE.Visible = false;                               // �v���o�b�N�e�L�X�g��Visible��ݒ�
            }
            // 1���A2���̂ǂ��炩���X�g���b�v�{�^����OFF�������ꍇ
            else
            {
                lblSTRIP_DEPTH_VALUE.Visible = true;                                    // �؍��[�����x����Visible��ݒ�
                textSTRIP_DEPTH_VALUE.Visible = true;                                   // �؍��[���e�L�X�g��Visible��ݒ�
                lblSTRIP_PULLBACK_VALUE.Visible = true;                                 // �v���o�b�N���x����Visible��ݒ�
                textSTRIP_PULLBACK_VALUE.Visible = true;                                // �v���o�b�N�e�L�X�g��Visible��ݒ�
            }
            */
        }

        /// <summary>
        /// �Z���T��ԁA����\���X�V�p�ϐ�
        /// </summary>
        private int statusDspSensorPage = -1;
        private int statusDspMotionPage = -1;
        private int statusDspSensorPgMax = 0;
        private int statusDspMotionPgMax = 0;
        private DateTime statusDisplayedTime = DateTime.Now;
        /// <summary>
        /// �Z���T��ԁA�����ԕ\�����X�V����
        /// </summary>
        private void statusDisplayRefresh()
        {
            // ��莞�Ԍo�ߌ�ɁA�y�[�W��ؑւ���
            TimeSpan ts = DateTime.Now - statusDisplayedTime;
            if (ts.TotalSeconds > SystemConstants.STATUS_DISPLAY_REFRESH_TIME)
            {
                statusDspSensorPage++;
                statusDspMotionPage++;
                statusDisplayedTime = DateTime.Now;
            }

            int dspPage = 0;

            // �Z���T�[���ڂ�\������
            Label[] senLbls = new Label[]{
                lblSensorCheck00,lblSensorCheck01,
                lblSensorCheck02,lblSensorCheck03,
                lblSensorCheck04,lblSensorCheck05,
                lblSensorCheck06,lblSensorCheck07,
                lblSensorCheck08,lblSensorCheck09,
                lblSensorCheck10,lblSensorCheck11,
                lblSensorCheck12,lblSensorCheck13,
                lblSensorCheck14,lblSensorCheck15};


            /* �\���y�[�W���I�[�o���Ă���ꍇ�A�y�[�W��0�ɖ߂�
             * �ϐ���ǉ����Ă���̂́AMax�l�`�F�b�N�� - �\���܂ł̊ԂɎ蓮�Ńy�[�W�l���X�V���ꂽ�ꍇ�A
             * ��O�G���[�𔭐������Ă��܂��ׁA���̑΍�B
             */
            dspPage = statusDspSensorPage;

            while (dspPage <= statusDspSensorPgMax)
            {
                if (existsStatusDataInMap(dspPage, SystemConstants.SENSOR_STATUS_BOX0, senLbls.Length))
                {
                    statusDspSensorPage = dspPage;
                    break;
                }
                else
                    dspPage++;
            }

            if (dspPage > statusDspSensorPgMax)
            {
                dspPage = 0;
                statusDspSensorPage = 0;
            }

            for (int i = 0; i < senLbls.Length; i++)
            {
                int index = dspIndexCalc(dspPage, i);
                statusDspUpdate(index, senLbls[i]);
            }

            // �����ԕ\�����ڂ�\������
            Label[] motionLbls = new Label[]{
                lblWorkMotion00, lblWorkMotion01,
                lblWorkMotion02, lblWorkMotion03,
                lblWorkMotion04, lblWorkMotion05,
                lblWorkMotion06, lblWorkMotion07,
                lblWorkMotion08, lblWorkMotion09,
                lblWorkMotion10, lblWorkMotion11};

            /* �\���y�[�W���I�[�o���Ă���ꍇ�A�y�[�W��0�ɖ߂�
             * �ϐ���ǉ����Ă���̂́AMax�l�`�F�b�N�� - �\���܂ł̊ԂɎ蓮�Ńy�[�W�l���X�V���ꂽ�ꍇ�A
             * ��O�G���[�𔭐������Ă��܂��ׁA���̑΍�B
             */
            dspPage = statusDspMotionPage;

            while (dspPage <= statusDspMotionPgMax)
            {
                if (existsStatusDataInMap(dspPage, SystemConstants.MOTION_STATUS_BOX0, motionLbls.Length))
                {
                    statusDspMotionPage = dspPage;
                    break;
                }
                else
                    dspPage++;
            }

            if (dspPage > statusDspMotionPgMax)
            {
                dspPage = 0;
                statusDspMotionPage = 0;
            }

            for (int i = 0; i < motionLbls.Length; i++)
            {
                int index = dspIndexCalc(statusDspMotionPage, SystemConstants.MOTION_STATUS_BOX_START + i);
#if FOR_Y
                statusDspUpdate(index, motionLbls[i], dspPage);
#else
                statusDspUpdate(index, motionLbls[i]);
#endif
            }
        }

        private bool existsStatusDataInMap(int pg, int startPosIndex, int count)
        {
            bool exists = false;

            int index = 0;
            for (int i = 0; i < count; i++)
            {
                index = dspIndexCalc(pg, startPosIndex);
                if (statusDspMap.ContainsKey(index))
                {
                    exists = true;
                    break;
                }
            }

            return exists;
        }

        private void statusDspUpdate(int index, Control Ctl, bool ReverseFlg = false)
        {
            // �l���擾����
            statusDspStruct dspData = new statusDspStruct();
            if (statusDspMap.TryGetValue(index, out dspData))
            {
                // senLbls[i].Visible = true;
                refreshControl(SystemConstants.STATUS_DISPLAY_MSG, dspData.MsgID, Ctl);
                CheckBtnAnd_ChangeColor(dspData.BtnID, Ctl, ReverseFlg);
            }
            else
            {
                refreshControl("", Ctl);
                CheckBtnAnd_ChangeColor(false, Ctl, ReverseFlg);
                // senLbls[i].Visible = false;
            }

        }

        /// <summary>
        /// Y�d�l�G���h�[�q�I�����̃A�v���I���{�^����Ԕ�\��
        /// </summary>
        /// <param name="index"></param>
        /// <param name="Ctl"></param>
        /// <param name="dspPage"></param>
        /// <param name="ReverseFlg"></param>
#if !HCSM40        
        private void statusDspUpdate(int index, Control Ctl, int dspPage, bool ReverseFlg = false)
        {
            if (dspPage == 0)
            {
                if((CheckBtnOnReturnTrue(SystemConstants.END_TERM1_BTN) && (index == SystemConstants.MOTION_STATUS_BOX4 || index == SystemConstants.MOTION_STATUS_BOX6)) ||
                   (CheckBtnOnReturnTrue(SystemConstants.END_TERM2_BTN) && (index == SystemConstants.MOTION_STATUS_BOX5 || index == SystemConstants.MOTION_STATUS_BOX7)))
                {
                    refreshControl("", Ctl);
                    CheckBtnAnd_ChangeColor(false, Ctl, ReverseFlg);
                    return;
                }                
            }

            // �l���擾����
            statusDspStruct dspData = new statusDspStruct();
            if (statusDspMap.TryGetValue(index, out dspData))
            {
                // senLbls[i].Visible = true;
                refreshControl(SystemConstants.STATUS_DISPLAY_MSG, dspData.MsgID, Ctl);
                CheckBtnAnd_ChangeColor(dspData.BtnID, Ctl, ReverseFlg);
            }
            else
            {
                refreshControl("", Ctl);
                CheckBtnAnd_ChangeColor(false, Ctl, ReverseFlg);
                // senLbls[i].Visible = false;
            }

        }
#endif

        private void panel8_Click(object sender, EventArgs e)
        {
            ActiveControl = null;
        }

        private void pictureBoxSIDE1_Click(object sender, EventArgs e)
        {
            ActiveControl = null;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ActiveControl = null;
        }

        private void pictureBoxSIDE2_Click(object sender, EventArgs e)
        {
            ActiveControl = null;
        }

        /// <summary>
        /// �o���N�R�����g����
        /// </summary>
        /// <param name="e"></param>
        private void textBankComment_EnterKeyDown(EventArgs e)
        {
            int selectedNo = 0;

            // ���ݑI������Ă���o���NNo���擾
            Program.DataController.BankNoRead(ref selectedNo);

            // ���C���t�H�[���̃o���N�R�����g��bankdata.xml�ɏ�������
            mainfrm.BankDataCommentWrite(selectedNo, textBankComment.Text);

            // �t�H�[�J�X���O��
            ActiveControl = null;
        }

        /// <summary>
        /// CFM��ʂ��烁�C����ʂ֖߂�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mainfrm_Shown(object sender, EventArgs e)
        {
#if OMOIKANE
            if (Program.OmoikaneConnector.IsConnect())
            {
                omoikane.Cmd004Struct cmd004Struct;
                Program.OmoikaneConnector.GetCFMStatus(out cmd004Struct);
            }
#endif
        }

        /// <summary>
        /// CFM��ʕ\��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCFMShow_Click(object sender, EventArgs e)
        {
#if OMOIKANE
            omoikaneForm.Show();
#endif
        }

        /// <summary>
        /// �e�B�[�`���O��ݒ肷��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCFMTeach_Click(object sender, EventArgs e)
        {
#if OMOIKANE
            Program.OmoikaneConnector.SetCFMStatus(omoikane.SystemConstants.CFM_STATUS_TEACH);
#endif
        }

        /// <summary>
        /// �Z���T�[��Ԃ̃y�[�W�X�V
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSensorNext_Click(object sender, EventArgs e)
        {
            statusDspSensorPage++;
            statusDisplayedTime = DateTime.Now;
        }

        /// <summary>
        /// ��ƃ{�^����Ԃ̃y�[�W�X�V
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWorkMotionNext_Click(object sender, EventArgs e)
        {
            statusDspMotionPage++;
            statusDisplayedTime = DateTime.Now;
        }

        /// <summary>
        /// ���j���[������I���Ńv���O�������I��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiShutDown_Click(object sender, EventArgs e)
        {
            Program.MainForm.Close();
        }

    }
}