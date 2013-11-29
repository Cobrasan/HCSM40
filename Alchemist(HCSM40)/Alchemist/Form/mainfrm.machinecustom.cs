using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Alchemist
{
    partial class mainfrm : Form
    {
        // 項目の機械毎の表示・非表示を設定する
        private void formCustom()
        {
#if HCSM40
            if (Program.SystemData.machineoperation == "botth")
            {
                pnlMachineOperate.Visible = true;
            }
            else
            {
                pnlMachineOperate.Visible = false;
            }

            #region カウンターパネル表示
            panel5.Visible = false;
            #endregion

            #region タクト表示
            lblTact2.Visible = false;
            lblTact3.Visible = false;
            lblTact5.Visible = false;
            lblTact6.Visible = false;
            #endregion

            #region 加工モード表示（ストリップ、ハーフのみ、圧着なし）
            btnCRIMP1.Visible = false;
            btnCRIMP2.Visible = false;
            btnCRIMP_SW1.Visible = false;
            btnCRIMP_SW2.Visible = false;
            btnSEAL1.Visible = false;
            btnSEAL2.Visible = false;           
            #endregion

            #region 加工詳細ボタン表示
            btnCRIMP1_Detail.Visible = false;
            btnCRIMP2_Detail.Visible = false;
            btnSEAL1_Detail.Visible = false;
            btnSEAL2_Detail.Visible = false;
            #endregion

            #region センサーパネル表示
            lblSensor.Visible = false;
            lblSensorCheck00.Visible = false;
            lblSensorCheck01.Visible = false;
            lblSensorCheck02.Visible = false;
            lblSensorCheck03.Visible = false;
            lblSensorCheck04.Visible = false;
            lblSensorCheck05.Visible = false;
            lblSensorCheck06.Visible = false;
            lblSensorCheck07.Visible = false;
            lblSensorCheck08.Visible = false;
            lblSensorCheck09.Visible = false;
            lblSensorCheck10.Visible = false;
            lblSensorCheck11.Visible = false;
            lblSensorCheck12.Visible = false;
            lblSensorCheck13.Visible = false;
            lblSensorCheck14.Visible = false;
            lblSensorCheck15.Visible = false;
            #endregion

            #region センサー状態表示
            {
                // 0ページ目
                //int pg = 0;
                //statusDspMapAdd(pg, SystemConstants.SENSOR_STATUS_BOX0, SystemConstants.STRIP1_SENSOR_LOCK, SystemConstants.STAT_DSP_MSG_STRIPMISS1);

                // ページ切替ボタン
                btnSensorNext.Visible = false;
            }
            #endregion

            #region 動作モード状態表示
            {
                // 0 ページ目
                int pg = 0;
                statusDspMapAdd(pg, SystemConstants.MOTION_STATUS_BOX0, SystemConstants.DOUBLE_MOTION_BTN, SystemConstants.STAT_DSP_MSG_DOUBLEMODE1);

                // ページ切替ボタン
                btnWorkMotionNext.Visible = false;
            }
            #endregion 

            #region ティーチングパネル
            pnlTeaching.Visible = false;
            #endregion

            #region インチング状態
            lblINCHING1.Visible = false;
            lblINCHING2.Visible = false;
            #endregion
#endif

#if OMOIKANE
            pnlCFMShow.Visible = true;
            btnCFMTeach.Visible = true;
#else
            pnlCFMShow.Visible = false;
            btnCFMTeach.Visible = false;
#endif
        }
    }
}