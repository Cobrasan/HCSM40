using System;
using System.Windows.Forms;

namespace Alchemist
{
    public partial class machineOperationfrm : Form
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
            /*  JN03SDC用の表示・非表示設定    */
#else
            /*  JN03S-A3用の表示・非表示設定  */

#endif

        }

    }
}