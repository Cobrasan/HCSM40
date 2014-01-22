using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Alchemist
{
    // 
    public partial class ButtonControl
    {
        private void addButtonControl()
        {
#if HCSM40
            #region HCSM40用 ボタンコントロール

            // 動作モード
            Add(SystemConstants.NORMAL_BTN, bitControlAction, 0, null, null, modeGroupAction, SystemConstants.ADDR_MOVE_MODE, SystemConstants.BTN_BANK_UNSAVED);			            // 標準
            Add(SystemConstants.EJECT_BTN, bitControlAction, 1, null, null, modeGroupAction, SystemConstants.ADDR_MOVE_MODE, SystemConstants.BTN_BANK_UNSAVED);				            // 排出
            Add(SystemConstants.TEST_BTN, bitControlAction, 2, null, null, modeGroupAction, SystemConstants.ADDR_MOVE_MODE, SystemConstants.BTN_BANK_UNSAVED);				            // 試行
            Add(SystemConstants.FREE_BTN, bitControlAction, 3, null, checkWireLoad_FreeOn, modeGroupAction, SystemConstants.ADDR_MOVE_MODE, SystemConstants.BTN_BANK_UNSAVED);				            // 空運転
            Add(SystemConstants.SAMPLE_BTN, bitControlAction, 4, null, null, modeGroupAction, SystemConstants.ADDR_MOVE_MODE, SystemConstants.BTN_BANK_UNSAVED);			            // サンプル

            // サイクルモード
            Add(SystemConstants.JOG_BTN, bitControlAction, 0, null, null, cycleModeAction, SystemConstants.ADDR_CYCLE_MODE, SystemConstants.BTN_BANK_UNSAVED);				            // ジョグ
            Add(SystemConstants.CYCLE_BTN, bitControlAction, 1, null, null, cycleModeAction, SystemConstants.ADDR_CYCLE_MODE, SystemConstants.BTN_BANK_UNSAVED);		                // 単動
            Add(SystemConstants.AUTO_BTN, bitControlAction, 2, null, null, cycleModeAction, SystemConstants.ADDR_CYCLE_MODE, SystemConstants.BTN_BANK_UNSAVED);				            // 連動

            // 加工モード1
            Add(SystemConstants.STRIP1_BTN, bitControlAction, 0, null, null, strip1Action, SystemConstants.ADDR_WORK_PROCESS1, SystemConstants.BTN_BANK_SAVED);				            // 1側ストリップ
            Add(SystemConstants.SEMISTRIP1_BTN, bitControlAction, 3, null, checkSemiStrip1_Btn, null, SystemConstants.ADDR_WORK_PROCESS1, SystemConstants.BTN_BANK_SAVED);	            // 1側ハーフストリップ

            // 加工モード2
            Add(SystemConstants.STRIP2_BTN, bitControlAction, 0, null, null, strip2Action, SystemConstants.ADDR_WORK_PROCESS2, SystemConstants.BTN_BANK_SAVED);				            // 2側ストリップ
            Add(SystemConstants.SEMISTRIP2_BTN, bitControlAction, 3, null, checkSemiStrip2_Btn, null, SystemConstants.ADDR_WORK_PROCESS2, SystemConstants.BTN_BANK_SAVED);	            // 2側ハーフストリップ

            // 加工モード3（未使用）
            
            // 加工モード4
            Add(SystemConstants.LOT_INTERVAL1_BTN, valueSetAction, 1, null, null, null, SystemConstants.ADDR_LOT_INTERVAL1, SystemConstants.BTN_BANK_UNSAVED);				            // 自動復帰
            Add(SystemConstants.DOUBLE_MOTION_BTN, valueSetAction, 1, null, null, null, SystemConstants.ADDR_DOUBLE_MOTION, SystemConstants.BTN_BANK_SAVED, SystemConstants.DB_GROUP_SEARCH_TYPE_WIRE1);			// 2段動作
            Add(SystemConstants.OUTPUT_BTN, valueSetAction, 1, null, null, null, SystemConstants.ADDR_OUTPUT, SystemConstants.BTN_BANK_SAVED, SystemConstants.DB_GROUP_SEARCH_TYPE_WIRE1);				            // 外部出力

            // 段取り
            Add(SystemConstants.LOAD1_BTN, valueSetAction, 1, null, checkFreeMode_LoadOn, autoOffAction, SystemConstants.ADDR_LOAD1, SystemConstants.BTN_BANK_UNSAVED);				    // 電線ロード
            Add(SystemConstants.LOAD1_STATUS, bitControlAction, 7, null, null, readonlyAction, SystemConstants.ADDR_MACHINE_STATUS, SystemConstants.BTN_BANK_UNSAVED);	                // 電線ロード状態
            
            // 操作
            Add(SystemConstants.MACHINE_START1_BTN, valueSetAction, 1, null, null, autoOffAction, SystemConstants.ADDR_MACHINE_START1, SystemConstants.BTN_BANK_UNSAVED);	            // 自動機スタート
            Add(SystemConstants.MACHINE_STOP1_BTN, valueSetAction, 1, null, null, autoOffAction, SystemConstants.ADDR_MACHINE_STOP1, SystemConstants.BTN_BANK_UNSAVED);		            // 自動機ストップ
            Add(SystemConstants.MACHINE_RESET1_BTN, valueSetAction, 1, null, null, autoOffAction, SystemConstants.ADDR_MACHINE_RESET1, SystemConstants.BTN_BANK_UNSAVED);	            // 自動機リセット

            // カウンタ
            Add(SystemConstants.TOTAL_COUNTER_RESET1_BTN, valueSetAction, 1, null, null, autoOffAction, SystemConstants.ADDR_TOTAL_COUNTER_RESET1, SystemConstants.BTN_BANK_UNSAVED);   // トータルカウントクリア
            Add(SystemConstants.QTY_COUNTER_RESET1_BTN, valueSetAction, 1, null, null, autoOffAction, SystemConstants.ADDR_QTY_COUNTER_RESET1, SystemConstants.BTN_BANK_UNSAVED);       // QTYカウントリセット
            Add(SystemConstants.LOT_COUNTER_RESET1_BTN, valueSetAction, 1, null, null, autoOffAction, SystemConstants.ADDR_LOT_COUNTER_RESET1, SystemConstants.BTN_BANK_UNSAVED);		// LOTカウントリセット
            Add(SystemConstants.COUNT_UP_BTN, bitControlAction, 0, null, null, autoOffAction, SystemConstants.ADDR_COUNT_UP_DOWN, SystemConstants.BTN_BANK_UNSAVED);				    // カウントアップ
            Add(SystemConstants.COUNT_DOWN_BTN, bitControlAction, 1, null, null, autoOffAction, SystemConstants.ADDR_COUNT_UP_DOWN, SystemConstants.BTN_BANK_UNSAVED);				    // カウントダウン

            // センサ（未使用）
            #endregion
#endif

        }


    }
}