using System;
using System.Windows.Forms;

namespace Alchemist
{
    public partial class counterfrm : Form
    {
        /// <summary>
        /// 項目の機械毎の表示・非表示を設定する
        /// </summary>
        private void formCustom()
        {
#if HCSM40
            // 画面高さ
            this.Height = 570;

            // タクト表示
            lblTact2.Visible = false;
            lblTact3.Visible = false;
            lblTact5.Visible = false;
            lblTact6.Visible = false;

            // 圧着カウントパネル表示
            panelCRIMP.Visible = false;
#else
            // 画面高さ
            this.Height = 700;  // 稼働時間表示
#endif
        }

    }

}