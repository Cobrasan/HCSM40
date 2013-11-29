using System;
using System.Reflection;
using System.Windows.Forms;


namespace Alchemist
{
    partial class AboutBox1 : Form
    {

        // 初期化設定
        public void Initialize()
        {
            Program.MainForm.AddOwnedForm(this);
        }

        public AboutBox1()
        {
            InitializeComponent();
        }


        private void okButton_Click(object sender, EventArgs e)
        {
            Visible = false;
        }

        private void AboutBox1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Visible = false;
        }

        private void AboutBox1_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible == true)
            {
                //  アセンブリ情報からの製品情報を表示する情報ボックスを初期化します。
                //  アプリケーションのアセンブリ情報設定を次のいずれかにて変更します:
                //  - [プロジェクト] メニューの [プロパティ] にある [アプリケーション] の [アセンブリ情報]
                //  - AssemblyInfo.cs
                this.Text = String.Format("{0}  {1}", Utility.AssemblyTitle, Utility.GetMessageString(SystemConstants.SYSTEM_MSG, SystemConstants.SYSTEM_MSG031));
                this.labelProductName.Text = Utility.AssemblyProduct;
                this.labelVersion.Text = String.Format("{0}  {1}", Utility.GetMessageString(SystemConstants.SYSTEM_MSG, SystemConstants.SYSTEM_MSG032), Utility.AssemblyVersion);
                this.labelCopyright.Text = Utility.AssemblyCopyright;
                this.labelCompanyName.Text = Utility.AssemblyCompany;
                this.textBoxDescription.Text = Utility.AssemblyDescription;
                this.labelMachineVersion.Text = String.Format("{0}  {1}", Utility.GetMessageString(SystemConstants.SYSTEM_MSG, SystemConstants.SYSTEM_MSG032), Utility.AssemblyMachineVersion);
                this.labelOptions.Text = getCompileOptions();
            }
        }

        /// <summary>
        /// コンパイルオプションを追加する
        /// </summary>
        /// <returns>options</returns>
        private string getCompileOptions()
        {
            string options = "";

#if IOWRITEENABLE
            options += "IOWRITABLE ";
#endif

#if ERROR_IMAGE
            options += "ERROR_IMAGE ";
#endif

#if OMOIKANE
            options += "OMOIKANE ";
#endif

#if MAINTELOG
            options += "MAINTELOG ";
#endif

#if HCSM40
            options += "HCSM40";
#endif

            return options;
        }

    }
}
