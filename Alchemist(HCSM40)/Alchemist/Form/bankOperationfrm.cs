using System;
using System.Windows.Forms;

namespace Alchemist
{
    public partial class bankOperationfrm : Form
    {
        // �������ݒ�
        public void Initialize()
        {
            Program.MainForm.AddOwnedForm(this);

            // �@�했�̐ݒ�����s����
            formCustom();

            // �e���L�[�p���̓C�x���g�o�^
            Program.MainForm.ClickTextBoxEvent(0, SystemConstants.TENKEY_INPUT_ONLY, 0, textCopy2);
        }

        public bankOperationfrm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// �o���N�I������
        /// </summary>
        private void selectBank()
        {
            int selectno = bankOperationView.CurrentRow.Index;

            // selectedno��ݒ肷��
            mainfrm.BankNoWrite(selectno);

            // �o���N�f�[�^�����[�h����
            int result = mainfrm.BankDataLoad(selectno);

            // �o���N�f�[�^���Z�[�u����
            result = mainfrm.BankDataSave(selectno);

            // �\�����X�V����
            lblNowBankNo2.Text = (selectno + 1).ToString();
            Utility.ShowErrorCode(result);

            // CFM�̋L�^���~����
            mainfrm.CFMRecordStop(omoikane.SystemConstants.CHANNEL_NO1);
            mainfrm.CFMRecordStop(omoikane.SystemConstants.CHANNEL_NO2);

            // �t�H�[�������
            Visible = false;
        }

        // �I���{�^���������̏���
        private void button1_Click(object sender, EventArgs e)
        {
            selectBank();
        }

        // �R�s�[�{�^���������̏���
        private void btnCopy_Click(object sender, EventArgs e)
        {
            string bankComment = "";
            int sourceNo, destNo, currentNo = 0;
            sourceNo = bankOperationView.SelectedRows[0].Index;
            destNo = Int32.Parse(textCopy2.Text) - 1;

            // �R�s�[��̃o���N�i���o�[��0�ȉ��ABANK_MAX + 1�ȏ�̐��������͂��ꂽ�ꍇ
            if (textCopy2.Text != "")
            {
                // ���݂̃o���NNo���擾����
                Program.DataController.BankNoRead(ref currentNo);

                // �R�s�[���ƃR�s�[�悪�����������ꍇ�A���̓R�s�[�悪���݂̔ԍ��̏ꍇ�A�������Ȃ�
                if (sourceNo == destNo || destNo == currentNo)
                {
                    // �����Ȃ�
                    return;
                }

                if (destNo < 0 || destNo >= SystemConstants.BANK_MAX + 1)
                {
                    // �͈͊O�ł���|�y�ѓ��͔͈͂̃��b�Z�[�W��\������
                    Utility.ShowErrorMsg(SystemConstants.SYSTEM_MSG026, lblCopy1.Text, 1, SystemConstants.BANK_MAX);
                }
                else
                {
                    // �I������Ă���o���NNo���w�肳�ꂽ�o���NNo�ɃR�s�[
                    Program.DataController.CopyBankData(sourceNo, destNo);

                    // �R�s�[��̃o���N�f�[�^����o���N�R�����g���擾
                    Program.DataController.BankDataCommentRead(destNo, ref bankComment);

                    // �R�s�[��̃o���N�R�����g�̕\�����X�V
                    bankOperationView.Rows[destNo].Cells[1].Value = bankComment;
                }
            }
            else
            {
                Utility.ShowErrorMsg(SystemConstants.SYSTEM_MSG026, lblCopy1.Text, 1, SystemConstants.BANK_MAX);
            }
        }

        // ����{�^���������̏���
        private void btnClose_Click(object sender, EventArgs e)
        {
            Visible = false;
        }

        // �t�H�[�����鏈��
        private void bankOperationfrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Visible = false;
        }


        // Visible��ԕύX���̏���
        private void bankOperationfrm_VisibleChanged(object sender, EventArgs e)
        {

            string bankComment = "";
            int selectedno = 0;

            if (Visible == true)
            {

                // ���݂���Ă���o���N�i���o�[���擾
                Program.DataController.BankNoRead(ref selectedno);

                // ���ݑI������Ă���o���N�i���o�[�����݃o���NNo�ɐݒ�
                lblNowBankNo2.Text = (selectedno + 1).ToString();

                this.SuspendLayout();

                // �O���b�h�r���[�̒��g��S�ċ�ɂ���
                bankOperationView.RowCount = 0;

                for (int i = 0; i < SystemConstants.BANK_MAX; i++)
                {
                    // i�Ԗڂ̃o���N�R�����g���擾
                    int result = Program.DataController.BankDataCommentRead(i, ref bankComment);

                    // BankDataCommentRead����ERR_NO_BANK_DATA���Ԃ��Ă����ꍇ
                    if (result == SystemConstants.ERR_NO_BANK_DATA)
                    {
                        bankOperationView.Rows.Add(new Object[] { (i + 1).ToString(), "[No Bank Data]" });
                    }
                    // BankDataCommentRead�œǂݍ��߂��ꍇ
                    else
                    {
                        bankOperationView.Rows.Add(new Object[] { (i + 1).ToString(), bankComment });
                    }
                }

                // �I������������
                bankOperationView.ClearSelection();

                // �s��I��
                bankOperationView.Rows[selectedno].Selected = true;

                this.ResumeLayout();
            }
        }

        // �Z�����_�u���N���b�N�������̏����i�I�������j
        private void bankOperationView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            selectBank();
        }

        private void lblNowBankNo2_Click(object sender, EventArgs e)
        {

        }
    }
}