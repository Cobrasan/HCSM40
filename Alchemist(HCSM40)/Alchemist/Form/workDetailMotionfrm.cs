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

            // 機械毎にフォームの表示を合わせる
            formCustom();

#if HCSM40            
            Program.MainForm.SetBtnEvent(SystemConstants.DOUBLE_MOTION_BTN, SystemConstants.BTN_PUSH, btnDOUBLE_MOTION);
            Program.MainForm.SetBtnEvent(SystemConstants.OUTPUT_BTN, SystemConstants.BTN_PUSH, btnOUTPUT);
#endif

        }


        public void refresh()
        {
#if HCSM40
            mainfrm.CheckBtnAnd_ChangeColor(SystemConstants.DOUBLE_MOTION_BTN, btnDOUBLE_MOTION);
            mainfrm.CheckBtnAnd_ChangeColor(SystemConstants.OUTPUT_BTN, btnOUTPUT);
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