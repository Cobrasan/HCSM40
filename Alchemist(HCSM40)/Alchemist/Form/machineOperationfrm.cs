using System;
using System.Windows.Forms;

namespace Alchemist
{
    public partial class machineOperationfrm : Form
    {
        // èâä˙âªê›íË
        public void Initialize()
        {
            Program.MainForm.AddOwnedForm(this);
        }

        public machineOperationfrm()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (IsKeyPushedDown(Keys.ShiftKey) == true)
            {
                mainfrm.WritePushBtn(SystemConstants.MACHINE_START1_BTN, SystemConstants.BTN_ON, true);
            }
        }

        private static bool IsKeyPushedDown(System.Windows.Forms.Keys vKey)
        {
            return 0 != (WinAPI.GetAsyncKeyState(vKey) & 0x8000);
        }


        private void btnReset_Click(object sender, EventArgs e)
        {
            mainfrm.WritePushBtn(SystemConstants.MACHINE_RESET1_BTN, SystemConstants.BTN_ON, true);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            mainfrm.WritePushBtn(SystemConstants.MACHINE_STOP1_BTN, SystemConstants.BTN_ON, true);
        }

        private void machineOperationfrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Visible = false;
        }

    }
}