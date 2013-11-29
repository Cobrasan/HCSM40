using System;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;
using System.Linq;

namespace Alchemist
{
    public partial class systemConfigurationfrm : Form
    {
        private void formCustom()
        {
#if HCSM40
            /* 表示・非表示設定  */

            // 検出タブを削除
            tabctlSystemConfiguration.TabPages.Remove(sensortabPage);

            // センサロックボタンの設定
            /*{
                // 表示画面は0ページ目固定
                int pg = 0;
                sensorLockDspMapAdd(pg, SystemConstants.SENSOR_LOCK_BOX0, SystemConstants.STAT_DSP_MSG_STRIPMISS1, getReadBtnClass(SystemConstants.STRIP1_SENSOR_LOCK), getWriteBtnClass(SystemConstants.STRIP1_SENSOR_LOCK, SystemConstants.BTN_PUSH));
            }*/

#endif

#if !MAINTELOG
            /* タブは非表示に出来ない為、起動時に削除する */
            tabctlSystemConfiguration.TabPages.Remove(tabpgOperationLog);
#endif

        }

    }
}