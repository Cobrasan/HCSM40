using System;
using System.Windows.Forms;

namespace Alchemist
{
    public partial class workDetailMotionfrm : Form
    {
        public workDetailMotionfrm()
        {
            InitializeComponent();

        }

        public void Initialize()
        {
            Program.MainForm.AddOwnedForm(this);

            // �@�B���Ƀt�H�[���̕\�������킹��
            formCustom();

#if HCSM40            
            Program.MainForm.SetBtnEvent(SystemConstants.DOUBLE_MOTION_BTN, SystemConstants.BTN_PUSH, btnDOUBLE_MOTION);
#endif

        }


        public void refresh()
        {
#if HCSM40
            mainfrm.CheckBtnAnd_ChangeColor(SystemConstants.DOUBLE_MOTION_BTN, btnDOUBLE_MOTION);
#endif
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Visible = false;
        }

        private void workDetailMotionfrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Visible = false;
        }

    }
}