using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using System;
using System.IO;

namespace Alchemist
{
    public partial class errInfoMsgfrm : Form
    {
        protected Stopwatch reDispTimer = new Stopwatch();
        protected Stopwatch pictureChangeTimer = new Stopwatch();
        int[] errPictureList = new int[0];
        static int errPictureIndex = 0;
        bool closeFlg = false;
        static int[] before_errBit = null;
        static int before_MachineStatus = 0;
        static bool before_IsConnectStatus = true;
        private errImageZoomfrm errImageZoomForm = new errImageZoomfrm();

        // �������ݒ�
        public void Initialize()
        {
            Program.MainForm.AddOwnedForm(this);
            this.AddOwnedForm(errImageZoomForm);

            // �@�B���̕\���E��\�����s��
            formcustom();

            pictureChangeTimer.Restart();
        }

        public errInfoMsgfrm()
        {
            InitializeComponent();
        }

        // �\���X�V�p���\�b�h
        public void refresh()
        {
            int errCode;
            int[] errBit = null;
            int messageID = 0;
            bool infoFlg = false;
            bool errFlg = false;
            bool dispFlg = false;
            bool compareRet = false;
            bool firstErr = false;
            bool isConnect = true;
            string message;

            const int BIT_SEEK_ORIGIN = 0x0001;     /* ���_�T�[�`�r�b�g */
            const int BIT_ERROR = 0x0008;           /* �G���[�r�b�g */
            const int BIT_LOT_COMPLETE = 0x0010;    /* ���b�g�ݒ薞���r�b�g */
            const int BIT_SET_COMPLETE = 0x0020;    /* �ݒ�{�������r�b�g */

            bool isConnectStatus = Program.DataController.GetComError() == SystemConstants.COM_ERROR_NORMAL;
            int machineStatus = Program.DataController.GetMachineStatus();
            Program.DataController.GetErrorCode(ref errBit);

            // �t�H�[������Ă���30�b�����������ꍇ
            if (closeFlg == true && reDispTimer.ElapsedMilliseconds < SystemConstants.INFOMATION_REDISPLAY_INTERVAL)
            {
                // �O��̃G���[�r�b�g�ɒl���ݒ肳��Ă����ꍇ
                if (before_errBit != null)
                {
                    // �O��̃G���[�r�b�g�Ɣ�r
                    compareRet = compareErrBit(errBit);
                }

                // �O��̃G���[�A�O���machineStatus�Ɣ�ׂāA�ω��Ȃ��̏ꍇ�A�����Ȃ�
                // �t�H�[������Ă���30�b�o�ߌゾ������\���������s��
                if (compareRet == true && before_MachineStatus == machineStatus && dispFlg == false && before_IsConnectStatus == isConnectStatus)
                {
                    return;
                }

                // 30�b�o�߃t���O�𗧂Ă�
                dispFlg = true;

                // �t�H�[���N���[�Y�t���O�𗎂Ƃ�
                closeFlg = false;
            }
            // �t�H�[������Ă���30�b�ȏゾ�����ꍇ
            else if (closeFlg == true)
            {
                // �ŏ�����(�N��������)�G���[���N���Ă����ꍇ�͕\������B
                // errBit�̗v�f����(8�v�f)���[�v����
                for (int elem = 0; elem < errBit.Length; elem++)
                {
                    if (errBit[elem] > 0)
                    {
                        errFlg = true;
                    }
                }

                // �G���[��ERR_MSG516�݂̂̏ꍇ
                if (errFlg == false && (machineStatus & BIT_SEEK_ORIGIN) == 0 && isConnectStatus == false)
                {
                    // �ĕ\�����s��Ȃ�
                    isConnect = false;
                }
                // 30�b�o�߃t���O�𗧂Ă�
                dispFlg = true;

                // �t�H�[���N���[�Y�t���O�𗎂Ƃ�
                closeFlg = false;
            }

            try
            {
                // �u���_�T�[�`���v�A�u�G���[�������v�A�u���b�g�����v�A�u�ݒ�{�������v
                // �܂��́AisConnect���ʐM�ُ��false�̏ꍇ
                if (((machineStatus & BIT_SEEK_ORIGIN) != 0) || ((machineStatus & BIT_ERROR) != 0) ||
                    ((machineStatus & BIT_LOT_COMPLETE) != 0) || ((machineStatus & BIT_SET_COMPLETE) != 0) || (isConnectStatus == false))
                {
                    // �O��̃G���[�r�b�g�ɒl���ݒ肳��Ă����ꍇ
                    if (before_errBit != null)
                    {
                        // �O��̃G���[�r�b�g�Ɣ�r
                        compareRet = compareErrBit(errBit);
                    }
                    // �O��̃G���[�r�b�g�ɒl���ݒ肳��Ă��Ȃ������ꍇ(�ŏ��̃G���[�R�[�h�擾��)
                    else
                    {
                        // �ŏ�����(�N��������)�G���[���N���Ă����ꍇ�͕\������B
                        // errBit�̗v�f����(8�v�f)���[�v����
                        for (int elem = 0; elem < errBit.Length; elem++)
                        {
                            if (errBit[elem] > 0)
                            {
                                firstErr = true;
                            }
                        }
                        // �u���_�T�[�`���v�A�u���b�g�����v�A�u�ݒ�{�������v�A�܂���IsConnect���ʐM�ُ킩��false�������ꍇ
                        if ((machineStatus & BIT_SEEK_ORIGIN) != 0 || ((machineStatus & BIT_LOT_COMPLETE) != 0) ||
                            ((machineStatus & BIT_SET_COMPLETE) != 0) || (isConnectStatus == false))
                        {
                            firstErr = true;
                        }
                    }

                    // �O��̃G���[�A�O���machineStatus�Ɣ�ׂāA�ω��Ȃ��̏ꍇ��
                    if (compareRet == true && before_MachineStatus == machineStatus)
                    {


                        // 30�b�o�߃t���O��false�AisConnect���O��ƕω����Ȃ��A�N�����G���[�ł͂Ȃ��ꍇ�A�����Ȃ�
                        if (dispFlg == false && before_IsConnectStatus == isConnectStatus && firstErr == false)
                        {
#if ERROR_IMAGE
                            // �G���[�C���[�W���X�V����
                            updatePicture();
#endif
                            return;
                        }
                    }

                    // �O��IsConnect�̍X�V
                    before_IsConnectStatus = isConnectStatus;

                    // �O��G���[�r�b�g�̍X�V
                    before_errBit = errBit;

                    // �O��machineStatus�̍X�V
                    before_MachineStatus = machineStatus;

#if ERROR_IMAGE
                    // �G���[��ʗp�z����N���A����
                    Array.Resize(ref errPictureList, 0);
#endif
                    // �`�揈�����Ō�ɂ܂Ƃ߂čs�����߁A�`����ꎞ��~
                    listError.BeginUpdate();
                    listInformation.BeginUpdate();

                    // listbox�����N���A����
                    listError.Items.Clear();
                    listInformation.Items.Clear();

                    // �u���_�T�[�`���v�������ꍇ
                    if ((machineStatus & BIT_SEEK_ORIGIN) != 0)
                    {
                        // ERR_MSG500���擾�������̂Ƃ��Ĉ���
                        message = Utility.GetErrorMessage(SystemConstants.ERR_MSG500);
                        infoFlg = true;
                        addListItem(SystemConstants.ERR_MSG_INFORMATION, SystemConstants.ERR_MSG500, message);
                    }

                    // �u���b�g�����v�������ꍇ
                    if ((machineStatus & BIT_LOT_COMPLETE) != 0)
                    {
#if FOR_JN03SDWP
                        // ERR_MSG025���擾�������̂Ƃ��Ĉ���
                        message = Utility.GetErrorMessage(SystemConstants.ERR_MSG025);
                        addListItem(SystemConstants.ERR_MSG_INFORMATION, SystemConstants.ERR_MSG025, message);
#elif FOR_JN03SDGP
                        // ERR_MSG025���擾�������̂Ƃ��Ĉ���
                        message = Utility.GetErrorMessage(SystemConstants.ERR_MSG025);
                        addListItem(SystemConstants.ERR_MSG_INFORMATION, SystemConstants.ERR_MSG025, message);
#elif FOR_JN03SDC
                        // ERR_MSG501���擾�������̂Ƃ��Ĉ���
                        message = Utility.GetErrorMessage(SystemConstants.ERR_MSG501);
                        addListItem(SystemConstants.ERR_MSG_INFORMATION, SystemConstants.ERR_MSG501, message);
#else
                        // ERR_MSG114���擾�������̂Ƃ��Ĉ���
                        message = Utility.GetErrorMessage(SystemConstants.ERR_MSG114);
                        addListItem(SystemConstants.ERR_MSG_INFORMATION, SystemConstants.ERR_MSG114, message);
#endif
                        infoFlg = true;
                    }

                    // �u�ݒ�{�������v�������ꍇ
                    if ((machineStatus & BIT_SET_COMPLETE) != 0)
                    {
#if FOR_JN03SDWP
                        // ERR_MSG024���擾�������̂Ƃ��Ĉ���
                        message = Utility.GetErrorMessage(SystemConstants.ERR_MSG024);
                        addListItem(SystemConstants.ERR_MSG_INFORMATION, SystemConstants.ERR_MSG024, message);
#elif FOR_JN03SDGP
                        // ERR_MSG024���擾�������̂Ƃ��Ĉ���
                        message = Utility.GetErrorMessage(SystemConstants.ERR_MSG024);
                        addListItem(SystemConstants.ERR_MSG_INFORMATION, SystemConstants.ERR_MSG024, message);
#elif FOR_JN03SDC
                        // ERR_MSG502���擾�������̂Ƃ��Ĉ���
                        message = Utility.GetErrorMessage(SystemConstants.ERR_MSG502);
                        addListItem(SystemConstants.ERR_MSG_INFORMATION, SystemConstants.ERR_MSG502, message);
#else
                        // ERR_MSG113���擾�������̂Ƃ��Ĉ���
                        message = Utility.GetErrorMessage(SystemConstants.ERR_MSG113);
                        addListItem(SystemConstants.ERR_MSG_INFORMATION, SystemConstants.ERR_MSG113, message);
#endif
                        infoFlg = true;
                    }

                    // IsConnect���ʐM�ُ��false�ɂȂ�����
                    if (isConnectStatus == false)
                    {
                        // ERR_MSG516���擾�������̂Ƃ��Ĉ���
                        message = Utility.GetErrorMessage(SystemConstants.ERR_MSG516);
                        errFlg = true;
                        addListItem(SystemConstants.ERR_MSG_ERROR, SystemConstants.ERR_MSG516, message);

                        // �t�H�[���N���[�Y����30�b�o�ߌゾ�����ꍇ
                        if (isConnect == false)
                        {
                            // �G���[�t���O�͗��ĂȂ�(�ĕ\�������Ȃ�����)
                            errFlg = false;
                        }

                    }

                    // errBit�̗v�f����(8�v�f)���[�v����
                    for (int elem = 0; elem < errBit.Length; elem++)
                    {
                        // 1�A�h���X��(16bit)���[�v����
                        for (int bit = 0; bit < 16; bit++)
                        {
                            // �G���[�r�b�g�������Ă����ꍇ
                            if ((errBit[elem] & (1 << bit)) != 0)
                            {
                                // �����Ă���G���[�r�b�g�̒萔�l���Z�o����
                                messageID = (elem * 16) + bit;

                                // �G���[�R�[�h�̋敪���擾���� 
                                errCode = Program.DataController.GetErrorType(messageID);

                                // �Ή����郁�b�Z�[�W��GetMessageString�Ŏ擾
                                message = Utility.GetErrorMessage(messageID);

                                // �u���v�敪�������ꍇ
                                if (errCode == SystemConstants.ERR_MSG_INFORMATION)
                                {
                                    infoFlg = true;
                                }
                                // �u�G���[�v�敪�������ꍇ
                                else
                                {
                                    errFlg = true;
                                }

                                // ���X�g�Ƀ��b�Z�[�W��ǉ�����
                                addListItem(errCode, messageID, message);
                            }
                        }
                    }


                    // �擾�����G���[�R�[�h���u���v�݂̂̏ꍇ
                    if (errFlg == false && infoFlg == true)
                    {
                        // Error����Visible=false�ɐݒ肷��
                        panelInformation.Visible = true;
                        panelError.Visible = false;

                        // ���W��ݒ�
                        panelInformation.Location = new Point(3, 3);
                        panelInformation.Size = new Size(620, 573);
                    }
                    // �擾�����G���[�R�[�h���u�G���[�v�݂̂̏ꍇ
                    else if (infoFlg == false && errFlg == true)
                    {
                        // Information����Visible=false�ɐݒ肷��
                        panelInformation.Visible = false;
                        panelError.Visible = true;

                        // ���W��ݒ�
                        panelError.Location = new Point(3, 3);
                        panelError.Size = new Size(620, 573);
                    }
                    // �擾�����G���[�R�[�h���u���v�A�u�G���[�v����
                    else
                    {
                        panelInformation.Visible = true;
                        panelError.Visible = true;

                        // ���W��ݒ�
                        panelInformation.Location = new Point(3, 3);
                        panelInformation.Size = new Size(620, 228);
                        panelError.Location = new Point(3, 231);
                        panelError.Size = new Size(620, 345);
                    }

                    // �G���[��ERR_MSG516�݂̂̏ꍇ(IsConnect��false�������ꍇ)
                    if (isConnect == false && infoFlg == false && errFlg == false)
                    {
                        // �ĕ\�����s��Ȃ�
                        return;
                    }
                    // ���E�G���[���b�Z�[�W��ʂ�\������
                    this.Visible = true;

                }
                // �u�G���[�������v�łȂ��ꍇ���AIsConnect��true�̏ꍇ�A
                else if ((machineStatus & BIT_ERROR) == 0 && isConnectStatus == true)
                {

                    // �O��IsConnect�̍X�V
                    before_IsConnectStatus = isConnectStatus;

                    // �O��G���[�r�b�g�̍X�V
                    before_errBit = errBit;

                    // �O��machineStatus�̍X�V
                    before_MachineStatus = machineStatus;

                    // �t�H�[�������
                    this.Visible = false;
                }
            }
            finally
            {
                // �`�揈�����Ō�ɂ܂Ƃ߂čs��
                listError.EndUpdate();
                listInformation.EndUpdate();
            }

#if ERROR_IMAGE
            // �G���[�C���[�W���X�V����
            updatePicture();
#endif
        }

        // �O��G���[�r�b�g�Ƃ̔�r
        private bool compareErrBit(int[] ErrBit)
        {
            // �O��G���[�r�b�g�Ƃ̔�r
            for (int i = 0; i < ErrBit.Length; i++)
            {
                if (before_errBit[i] != ErrBit[i])
                {
                    return false;
                }
            }
            return true;
        }

        private void addListItem(int ErrCode, int MessageID, string Message)
        {
            string message = "";

            // ERR_MSG127���ゾ�����ꍇ(ERR_MSG500�ȏ�)
            if (MessageID > 0x07F)
            {
                // �I�t�Z�b�g��ݒ肷��
                MessageID = MessageID + 0x00F4;
            }

            // �u���v�敪�������ꍇ
            if (ErrCode == SystemConstants.ERR_MSG_INFORMATION)
            {
                // �uInformation�v���ɍ��ڂ�\������
                // �擾�������b�Z�[�W�̑O�ɁuMSG�v+�u�G���[�ԍ��v+�u: �v��t���ĕ\��
                this.panelInformation.Visible = true;
                message = "MSG" + MessageID + ": " + Message;
                this.listInformation.Items.Add(message);
            }
            else
            {
                // �uError�v���ɍ��ڂ�\������
                // �擾�������b�Z�[�W�̑O�ɁuMSG�v+�u�G���[�ԍ��v+�u: �v��t���ĕ\��
                this.panelError.Visible = true;
                message = "MSG" + MessageID + ": " + Message;
                this.listError.Items.Add(message);

#if ERROR_IMAGE
                int errListIndex;
                // �z��T�C�Y��ύX���A�G���[�ԍ���ǉ�����
                Array.Resize(ref errPictureList, errPictureList.Length + 1);
                errListIndex = errPictureList.Length - 1;
                errPictureList[errListIndex] = MessageID;
#endif
            }
        }

        private void errInfoMsgfrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // �t�H�[������Ă���̎��Ԃ��v������
            reDispTimer.Restart();

            // �t�H�[���N���[�Y�t���O��true�ɂ���
            closeFlg = true;

            e.Cancel = true;
            Visible = false;
        }

        private void pbErrorData_MouseEnter(object sender, System.EventArgs e)
        {
#if ERROR_IMAGE
            Point screenPoint = pbErrorImage.PointToScreen(new Point(0, 0));

            errImageZoomForm.Location = screenPoint;
            errImageZoomForm.Show();
#endif
        }

        // �摜���X�V����
        private void updatePicture()
        {
            // �g���ʂ��\�����͍X�V���s��Ȃ�
            if (errImageZoomForm.Visible) return;

            // �G���[���Ȃ��ꍇ�A�C���[�W����������
            if (errPictureList.Length == 0)
            {
                pbErrorImage.Image = null;
                errImageZoomForm.UpdatePicture(null);
            }

            // �G���[������A�Q�b�o�ߌ�ɉ摜���X�V����
            if ((errPictureList.Length <= 0) || (pictureChangeTimer.ElapsedMilliseconds < SystemConstants.ERR_PICTURE_REFRESH_INTERVAL)) return;

            // �G���[�摜�ԍ����͈͂𒴂��Ă���ꍇ (�G���[���������ꂽ�ꍇ)�́A0����J�n����
            if (errPictureIndex > errPictureList.Length - 1) errPictureIndex = 0;

            string imgFolder = getErrPictureFolder();
            int errMsgCode = errPictureList[errPictureIndex++];
            string imgFileName = string.Format(imgFolder + "\\err{0:X3}.jpg", errMsgCode);
            Image errImg;

            try
            {
                if (File.Exists(imgFileName))
                    errImg = Image.FromFile(imgFileName);
                else
                    errImg = Alchemist.Properties.Resources.NoErrImg;
            }
            catch
            {
                errImg = Alchemist.Properties.Resources.NoErrImg;
            }

            // �摜���X�V����
            if (pbErrorImage.Image != errImg)
            {
                pbErrorImage.Image = errImg;
                pbErrorImage.Refresh();
            }
            errImageZoomForm.UpdatePicture(errImg);

            // �\���^�C�}�����Z�b�g����
            pictureChangeTimer.Restart();
        }


    }
}