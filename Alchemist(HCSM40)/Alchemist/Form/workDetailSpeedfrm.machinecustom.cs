using System;
using System.Windows.Forms;

namespace Alchemist
{
    public partial class workDetailSpeedfrm : Form
    {
        /* 項目の機械毎の表示・非表示を設定する
         * ベースは、JN03S-A3-WP */
        private void formCustom()
        {
#if FOR_JN03SDWP
            /*  JN03SD-2WP用の表示・非表示設定    */
            /* 同じなので、何もしない */
#elif FOR_JN03SDGP
            /*  JN03SD-GP用の表示・非表示設定    */
            /* 同じなので、何もしない */
#elif FOR_JN03SDC
            lblWIRE_LENGTH_CORRECT1.Visible = false;
            lblWIRE_LENGTH_CORRECT2.Visible = false;
            lblWIRE_LENGTH_CORRECT3.Visible = false;
            textWIRE_LENGTH_CORRECT11.Visible = false;
            textWIRE_LENGTH_CORRECT12.Visible = false;
            textWIRE_LENGTH_CORRECT13.Visible = false;
            lblUNIT6.Visible = false;
            lblUNIT9.Visible = false;
            lblUNIT12.Visible = false;
#else
            /*  JN03S-A3用の表示・非表示設定  */

#endif

        }

    }
}