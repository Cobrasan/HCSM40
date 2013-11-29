using System;
using System.Windows.Forms;

namespace Alchemist
{
    public partial class workDetailSpeedfrm : Form
    {
        /// <summary>
        /// 初期化設定
        /// </summary>
        public void Initialize()
        {
            Program.MainForm.AddOwnedForm(this);

            // 機種毎の設定を実行する
            formCustom();
        }

        /// <summary>
        /// フォームのコンストラクタ
        /// </summary>
        public workDetailSpeedfrm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// フォーム生成時のイベント登録等の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void workDetailSpeedfrm_Shown(object sender, EventArgs e)
        {            
            #region テキスト入力イベント
            Program.MainForm.SetTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.FEED_SPEED1, textFEED_SPEED1);
            Program.MainForm.SetTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.FEED_ACCEL1, textFEED_ACCEL1);
            Program.MainForm.SetTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.WIRE_LENGTH_CORRECT1, textWIRE_LENGTH_CORRECT11);
            Program.MainForm.SetTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.FEED_SPEED2, textFEED_SPEED2);
            Program.MainForm.SetTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.FEED_ACCEL2, textFEED_ACCEL2);
            Program.MainForm.SetTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.WIRE_LENGTH_CORRECT2, textWIRE_LENGTH_CORRECT12);
            Program.MainForm.SetTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.FEED_SPEED3, textFEED_SPEED3);
            Program.MainForm.SetTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.FEED_ACCEL3, textFEED_ACCEL3);
            Program.MainForm.SetTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.WIRE_LENGTH_CORRECT3, textWIRE_LENGTH_CORRECT13);
            Program.MainForm.SetTextBoxLength(this, 10);
            #endregion

            #region テンキー入力用のクリックイベント
            Program.MainForm.ClickTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.TENKEY_INPUT_DATA, SystemConstants.FEED_SPEED1, textFEED_SPEED1);
            Program.MainForm.ClickTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.TENKEY_INPUT_DATA, SystemConstants.FEED_ACCEL1, textFEED_ACCEL1);
            Program.MainForm.ClickTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.TENKEY_INPUT_DATA, SystemConstants.WIRE_LENGTH_CORRECT1, textWIRE_LENGTH_CORRECT11);
            Program.MainForm.ClickTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.TENKEY_INPUT_DATA, SystemConstants.FEED_SPEED2, textFEED_SPEED2);
            Program.MainForm.ClickTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.TENKEY_INPUT_DATA, SystemConstants.FEED_ACCEL2, textFEED_ACCEL2);
            Program.MainForm.ClickTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.TENKEY_INPUT_DATA, SystemConstants.WIRE_LENGTH_CORRECT2, textWIRE_LENGTH_CORRECT12);
            Program.MainForm.ClickTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.TENKEY_INPUT_DATA, SystemConstants.FEED_SPEED3, textFEED_SPEED3);
            Program.MainForm.ClickTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.TENKEY_INPUT_DATA, SystemConstants.FEED_ACCEL3, textFEED_ACCEL3);
            Program.MainForm.ClickTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.TENKEY_INPUT_DATA, SystemConstants.WIRE_LENGTH_CORRECT3, textWIRE_LENGTH_CORRECT13);
            Program.MainForm.ClickDataGridViewEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.TENKEY_INPUT_DATA, workDetailSpeedView);
            Program.MainForm.ClickTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.TENKEY_INPUT_WIRETHRES, SystemConstants.FEED_SPEED_THRES1, textFEED_SPEED_THRES1);
            Program.MainForm.ClickTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.TENKEY_INPUT_WIRETHRES, SystemConstants.FEED_SPEED_THRES2, textFEED_SPEED_THRES2);
            #endregion

            #region グリッドの内容表示
            int[] ID = null;
            workDetailSpeedView.CurrentCell = null;
            Program.DataController.GetMemryDataGroupList(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.WORK_GROUP_SPEED1, ref ID);

            if (ID != null)
            {
                string min = "";
                string max = "";
                string value = "";
                string name = "";
                string unit = "";

                foreach (var workid in ID)
                {
                    // 名称を取得する(どうやるか？）
                    name = Utility.GetMessageString(SystemConstants.WORK_MSG, workid);

                    // 範囲を取得する
                    Program.DataController.GetWorkDataRangeStr(workid, ref min, ref max);

                    // 値を取得する
                    Program.DataController.ReadWorkDataStr(workid, ref value);

                    //単位を取得する。
                    Program.DataController.GetWorkDataUnit(workid, ref unit);

                    // 値を設定する
                    workDetailSpeedView.Rows.Add(new Object[] { workid, name, string.Format("{0} <-> {1}", new object[] { min, max }), value, unit });
                }
            }
            #endregion
        }

        /// <summary>
        /// フォームを閉じるイベント
        /// 終了しないで非表示にする
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void workDetailSpeedfrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Visible = false;
        }

        /// <summary>
        /// 閉じるボタンのイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            Visible = false;
        }

        /// <summary>
        /// 画面の表示内容を更新する処理
        /// 画面表示中はメインフォームのスレッドから呼ばれる
        /// </summary>
        public void refresh()
        {
            #region 設定項目の表示更新
            Program.MainForm.refreshControl(SystemConstants.FEED_SPEED_THRES1, textFEED_SPEED_THRES1);
            Program.MainForm.refreshControl(SystemConstants.FEED_SPEED_THRES2, textFEED_SPEED_THRES2);
            Program.MainForm.refreshControl(SystemConstants.FEED_SPEED1, textFEED_SPEED1);
            Program.MainForm.refreshControl(SystemConstants.FEED_ACCEL1, textFEED_ACCEL1);
            Program.MainForm.refreshControl(SystemConstants.WIRE_LENGTH_CORRECT1, textWIRE_LENGTH_CORRECT11);
            Program.MainForm.refreshControl(SystemConstants.FEED_SPEED2, textFEED_SPEED2);
            Program.MainForm.refreshControl(SystemConstants.FEED_ACCEL2, textFEED_ACCEL2);
            Program.MainForm.refreshControl(SystemConstants.WIRE_LENGTH_CORRECT2, textWIRE_LENGTH_CORRECT12);
            Program.MainForm.refreshControl(SystemConstants.FEED_SPEED3, textFEED_SPEED3);
            Program.MainForm.refreshControl(SystemConstants.FEED_ACCEL3, textFEED_ACCEL3);
            Program.MainForm.refreshControl(SystemConstants.WIRE_LENGTH_CORRECT3, textWIRE_LENGTH_CORRECT13);

            //閾値ラベル表示
            Program.MainForm.refreshControlSpeedThres(SystemConstants.FEED_SPEED_THRES1, lblFEED_SPEED_THRES2, " -");
            Program.MainForm.refreshControlSpeedThres(SystemConstants.FEED_SPEED_THRES2, lblFEED_SPEED_THRES3, " -");
            #endregion

            #region グリッドの内容の更新
            int rowCount = workDetailSpeedView.Rows.Count;
            string value = "";

            //GridViewの更新ｓ
            for (int y = 0; y < rowCount; y++)
            {
                int workid = Int32.Parse(workDetailSpeedView.Rows[y].Cells[0].Value.ToString());

                // 値を取得する
                Program.DataController.ReadWorkDataStr(workid, ref value);

                var cell = workDetailSpeedView.Rows[y].Cells[3];

                // 値が編集中でなければ、値を変更する
                if (!cell.IsInEditMode)
                {
                    workDetailSpeedView.Rows[y].Cells[3].Value = value;
                }
            }
            #endregion
        }

        /// <summary>
        /// データグリッドのセルを変更されたら発生するイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void workDetailSpeedView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (Program.SystemData.tachpanel == true) return;
            if (e.ColumnIndex != 3) return;

            DataGridView view = workDetailSpeedView;
            int workidtype = SystemConstants.WORKID_TYPE_WORKDATA;
            int workid = Int32.Parse(view.Rows[e.RowIndex].Cells[0].Value.ToString());
            object value = e.FormattedValue;

            Program.MainForm.EnterDataGridView(workidtype, workid, value);
        }

        /// <summary>
        /// 閾値２の確定処理
        /// </summary>
        /// <param name="e"></param>
        private void textFEED_SPEED_THRES2_EnterKeyDown(EventArgs e)
        {
            /*
            double workdata = 0;
            string errMessage;
            double outValue;

            // 形式チェック
            if (Program.MainForm.checkTextBoxValue(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.FEED_SPEED_THRES2, textFEED_SPEED_THRES2.Text, out outValue, out errMessage) == false)
            {
                Utility.ShowErrorMsg(errMessage);
                return;
            }

            // 閾値１を読む
            Program.DataController.ReadWorkData(SystemConstants.FEED_SPEED_THRES1, ref workdata);

            // 範囲チェック
            if (outValue <= workdata)
            {
                string workname = Utility.GetMessageString(SystemConstants.WORK_MSG, SystemConstants.FEED_SPEED_THRES2);
                Utility.ShowErrorMsg(SystemConstants.SYSTEM_MSG022, workname, lblFEED_SPEED_THRES2.Text);
                return;
            }

            // ワークデータを書き込む
            mainfrm.WriteWorkData(SystemConstants.FEED_SPEED_THRES2, outValue);
            */

            Program.MainForm.EnterTextBoxWireLengthThres(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.FEED_SPEED_THRES2, textFEED_SPEED_THRES2);

            // フォーカスアウトする
            Form frm = textFEED_SPEED_THRES2.FindForm();
            frm.ActiveControl = null;
        }

        /// <summary>
        ///  閾値１の確定処理
        /// </summary>
        /// <param name="e"></param>
        private void textFEED_SPEED_THRES1_EnterKeyDown_1(EventArgs e)
        {
            /*
            double outValue;
            string errMessage;
            double workdata = 0;

            // 形式チェック
            if (Program.MainForm.checkTextBoxValue(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.FEED_SPEED_THRES1, textFEED_SPEED_THRES1.Text, out outValue, out errMessage) == false)
            {
                Utility.ShowErrorMsg(errMessage);
                return;
            }

            // 閾値２を読む
            Program.DataController.ReadWorkData(SystemConstants.FEED_SPEED_THRES2, ref workdata);

            // 適正範囲チェック
            if (textFEED_SPEED_THRES1.Text == "0" || workdata <= outValue)
            {
                string workname = Utility.GetMessageString(SystemConstants.WORK_MSG, SystemConstants.FEED_SPEED_THRES2);
                Utility.ShowErrorMsg(SystemConstants.SYSTEM_MSG027, workname, "0", textFEED_SPEED_THRES2.Text);
                return;
            }

            // ワークデータを書き込む
            mainfrm.WriteWorkData(SystemConstants.FEED_SPEED_THRES1, outValue);
            */

            Program.MainForm.EnterTextBoxWireLengthThres(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.FEED_SPEED_THRES1, textFEED_SPEED_THRES1);

            // フォーカスアウトする
            Form frm = textFEED_SPEED_THRES1.FindForm();
            frm.ActiveControl = null;
        }
    }
}