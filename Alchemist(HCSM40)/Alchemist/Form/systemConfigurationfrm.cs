using System;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;

namespace Alchemist
{
    public partial class systemConfigurationfrm : Form
    {
#if MAINTELOG
        private int[] showMainteUnitCodes = new int[]{
            SystemConstants.MAINTELOG_UNIT_FEED1,
            SystemConstants.MAINTELOG_UNIT_TRANSFER1,
            SystemConstants.MAINTELOG_UNIT_TRANSFER2,
            SystemConstants.MAINTELOG_UNIT_EJECT1,
            SystemConstants.MAINTELOG_UNIT_SLIDER1,
            SystemConstants.MAINTELOG_UNIT_SLIDER2,
            SystemConstants.MAINTELOG_UNIT_CUTSTRIP1,
            SystemConstants.MAINTELOG_UNIT_CRIMP1,
            SystemConstants.MAINTELOG_UNIT_CRIMP2,
            SystemConstants.MAINTELOG_UNIT_SEAL1,
            SystemConstants.MAINTELOG_UNIT_SEAL2,
            SystemConstants.MAINTELOG_UNIT_OTHER
        };
#endif

        /// <summary>
        /// 状態表示用データクラスを定義する
        /// index は、以下のルールで作成されます。
        /// 
        ///   X00
        /// 
        /// X: 表示タイミング 0: 初期表示、1: 1枚目
        /// 00: 表示位置
        /// 
        /// 
        /// ・表示位置
        /// +--+--+--+
        /// | 0|10|20|
        /// +--+--+--+
        /// | 1|11|21|
        /// +--+--+--+
        /// | 2|12|22|
        /// +--+--+--+
        /// | 3|13|23|
        /// +--+--+--+
        /// | 4|14|24|
        /// 
        /// </summary>
        private class sensorLockWriteBtn
        {
            public int BtnID;
            public int BtnMotion;
        }
        
        /// <summary>
        /// センサーロック状態読み込み用ボタンクラス
        /// </summary>
        private class sensorLockReadBtn
        {
            public int BtnID;
            public bool ReverseFlg;
        }
        
        /// <summary>
        /// センサーロック状態パネルの構造体
        /// </summary>
        private struct sensorLockDspStruct
        {
            public int MsgID;
            public sensorLockReadBtn ReadBtn;
            public sensorLockWriteBtn WriteBtn;
        }
        
        private Dictionary<int, sensorLockDspStruct> sensorDspMap = new Dictionary<int, sensorLockDspStruct>();     // センサーロックボタンのデータマップ
        private int statusDspSensorPgMax = 0;   // センサーロック状態パネルのページ数
        
        /// <summary>
        /// センサーロックボタンのデータマップに内容を登録する処理
        /// </summary>
        /// <param name="dspTimming"></param>
        /// <param name="dspPosition"></param>
        /// <param name="msgID"></param>
        /// <param name="readBtnCls"></param>
        /// <param name="writeBtnCls"></param>
        private void sensorLockDspMapAdd(int dspTimming, int dspPosition, int msgID, sensorLockReadBtn readBtnCls, sensorLockWriteBtn writeBtnCls)
        {
            // 引数が不適切な場合、例外を発生させる
            if (dspTimming < 0 || dspPosition < 0 || dspPosition > 99) new ArgumentException("SensorLock DspTimming or DspPosition is out of range");

            // ページの最大数を求める
            if (dspTimming > statusDspSensorPgMax) statusDspSensorPgMax = dspTimming;

            // インデックスを生成する
            int index = mainfrm.dspIndexCalc(dspTimming, dspPosition);

            // 設定データに追加する
            sensorLockDspStruct opeStruct = new sensorLockDspStruct();
            opeStruct.MsgID = msgID;
            opeStruct.ReadBtn = readBtnCls;
            opeStruct.WriteBtn = writeBtnCls;

            // マップに追加する
            sensorDspMap.Add(index, opeStruct);
        }

        /// <summary>
        /// センサーロックボタンに状態を書き込む処理
        /// </summary>
        /// <param name="btnID"></param>
        /// <param name="btnMotion"></param>
        /// <returns></returns>
        private sensorLockWriteBtn getWriteBtnClass(int btnID, int btnMotion)
        {
            // ボタンの動作モードが異なる場合、例外を返す
            if (btnMotion != SystemConstants.BTN_PUSH && btnMotion != SystemConstants.BTN_ON && btnMotion != SystemConstants.BTN_OFF) throw new ArgumentException("BtnMotion is out of range");

            sensorLockWriteBtn writeBtn = new sensorLockWriteBtn();
            writeBtn.BtnID = btnID;
            writeBtn.BtnMotion = btnMotion;
            return writeBtn;
        }

        /// <summary>
        /// センサーロックボタン状態を返す処理
        /// </summary>
        /// <param name="btnID"></param>
        /// <param name="reverseFlg"></param>
        /// <returns></returns>
        private sensorLockReadBtn getReadBtnClass(int btnID, bool reverseFlg = false)
        {
            sensorLockReadBtn readBtn = new sensorLockReadBtn();
            readBtn.BtnID = btnID;
            readBtn.ReverseFlg = reverseFlg;
            return readBtn;
        }

        /// <summary>
        /// メモリーモニター画面の定義
        /// </summary>
        private iocheckfrm iochecForm = new iocheckfrm();
        public ioMonitorfrm ioMonitorForm = null;
        private learnDataItemEditfrm learndataitemeditForm = new learnDataItemEditfrm();

        /// <summary>
        /// 初期化設定
        /// </summary>
        public void Initialize()
        {
            Program.MainForm.AddOwnedForm(this);

            // 機種毎の表示・非表示を変更
            formCustom();

            #region 検出ボタンイベントを割当
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX0, btnSensorLock00);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX1, btnSensorLock01);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX2, btnSensorLock02);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX3, btnSensorLock03);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX4, btnSensorLock04);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX5, btnSensorLock05);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX6, btnSensorLock06);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX7, btnSensorLock07);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX8, btnSensorLock08);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX9, btnSensorLock09);

            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX10, btnSensorLock10);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX11, btnSensorLock11);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX12, btnSensorLock12);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX13, btnSensorLock13);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX14, btnSensorLock14);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX15, btnSensorLock15);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX16, btnSensorLock16);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX17, btnSensorLock17);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX18, btnSensorLock18);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX19, btnSensorLock19);

            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX20, btnSensorLock20);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX21, btnSensorLock21);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX22, btnSensorLock22);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX23, btnSensorLock23);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX24, btnSensorLock24);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX25, btnSensorLock25);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX26, btnSensorLock26);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX27, btnSensorLock27);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX28, btnSensorLock28);
            setSensorLockBtn(SystemConstants.SENSOR_LOCK_BOX29, btnSensorLock29);
            #endregion

            #region 補正値・タイミングの表への入力イベント登録
            setDataGridEvent(SystemConstants.WORKID_TYPE_CORRECTDATA, systemConfiguration_CORR_GROUP_BASEMACHINE1View);
            setDataGridEvent(SystemConstants.WORKID_TYPE_CORRECTDATA, systemConfiguration_CORR_GROUP_FEED1View);
            setDataGridEvent(SystemConstants.WORKID_TYPE_CORRECTDATA, systemConfiguration_CORR_GROUP_SLIDER1View);
            setDataGridEvent(SystemConstants.WORKID_TYPE_CORRECTDATA, systemConfiguration_CORR_GROUP_STRIP1View);
            setDataGridEvent(SystemConstants.WORKID_TYPE_TIMINGDATA, systemConfiguration_TIMM_GROUP_SIDE1VIEW);
            setDataGridEvent(SystemConstants.WORKID_TYPE_TIMINGDATA, systemConfiguration_TIMM_GROUP_SIDE2VIEW);
            #endregion

            #region テンキー入力のためのグリッドクリックイベント
            Program.MainForm.ClickDataGridViewEvent(SystemConstants.WORKID_TYPE_CORRECTDATA, SystemConstants.TENKEY_INPUT_DATA, systemConfiguration_CORR_GROUP_BASEMACHINE1View);
            Program.MainForm.ClickDataGridViewEvent(SystemConstants.WORKID_TYPE_CORRECTDATA, SystemConstants.TENKEY_INPUT_DATA, systemConfiguration_CORR_GROUP_FEED1View);
            Program.MainForm.ClickDataGridViewEvent(SystemConstants.WORKID_TYPE_CORRECTDATA, SystemConstants.TENKEY_INPUT_DATA, systemConfiguration_CORR_GROUP_SLIDER1View);
            Program.MainForm.ClickDataGridViewEvent(SystemConstants.WORKID_TYPE_CORRECTDATA, SystemConstants.TENKEY_INPUT_DATA, systemConfiguration_CORR_GROUP_STRIP1View);
            Program.MainForm.ClickDataGridViewEvent(SystemConstants.WORKID_TYPE_TIMINGDATA, SystemConstants.TENKEY_INPUT_DATA, systemConfiguration_TIMM_GROUP_SIDE1VIEW);
            Program.MainForm.ClickDataGridViewEvent(SystemConstants.WORKID_TYPE_TIMINGDATA, SystemConstants.TENKEY_INPUT_DATA, systemConfiguration_TIMM_GROUP_SIDE2VIEW);            
            #endregion

            #region IOチェックの初期化
            iochecForm.Initialize();
            #endregion
        }

        /// <summary>
        /// フォームのコンストラクタ
        /// </summary>
        public systemConfigurationfrm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 通信ポートから数字を取得する処理
        /// </summary>
        /// <param name="name">COM1など通信ポート</param>
        /// <returns>COM1なら1を返す</returns>
        private int portNameToInt(String name)
        {
            int ret = 0;

            try
            {
                ret = Int32.Parse(name.Replace("COM", ""));
            }
            catch
            {
                /* 無視 */
            }

            return ret;
        }

        /// <summary>
        /// MemoryMonitorボタンの押下イベント
        /// メモリーモニター画面を表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMemoryMonitor_Click(object sender, EventArgs e)
        {
            iochecForm.Show();
        }

        /// <summary>
        /// 表示の更新処理
        /// メインフォームのスレッドから呼び出される
        /// 補正値・タイミング・センサーロックの状態を更新
        /// </summary>
        public void refresh()
        {
            #region 補正値・タイミング値の表示更新
            GridUpdate(systemConfiguration_CORR_GROUP_BASEMACHINE1View, SystemConstants.WORKID_TYPE_CORRECTDATA);
            GridUpdate(systemConfiguration_CORR_GROUP_FEED1View, SystemConstants.WORKID_TYPE_CORRECTDATA);
            GridUpdate(systemConfiguration_CORR_GROUP_SLIDER1View, SystemConstants.WORKID_TYPE_CORRECTDATA);
            GridUpdate(systemConfiguration_CORR_GROUP_STRIP1View, SystemConstants.WORKID_TYPE_CORRECTDATA);
            GridUpdate(systemConfiguration_TIMM_GROUP_SIDE1VIEW, SystemConstants.WORKID_TYPE_TIMINGDATA);
            GridUpdate(systemConfiguration_TIMM_GROUP_SIDE2VIEW, SystemConstants.WORKID_TYPE_TIMINGDATA);
            #endregion

            // 検出タブの表示更新
            sensorLockDspRefresh();
        }

        /// <summary>
        /// センサーロック表示を更新する処理
        /// ページ、ボタンから内容を更新
        /// </summary>
        private void sensorLockDspRefresh()
        {
            // ロックボタン一覧
            Button[] senBtns = new Button[]{
                btnSensorLock00, btnSensorLock01, btnSensorLock02, btnSensorLock03, btnSensorLock04, 
                btnSensorLock05, btnSensorLock06, btnSensorLock07, btnSensorLock08, btnSensorLock09,
                btnSensorLock10, btnSensorLock11, btnSensorLock12, btnSensorLock13, btnSensorLock14,
                btnSensorLock15, btnSensorLock16, btnSensorLock17, btnSensorLock18, btnSensorLock19,
                btnSensorLock20, btnSensorLock21, btnSensorLock22, btnSensorLock23, btnSensorLock24,
                btnSensorLock25, btnSensorLock26, btnSensorLock27, btnSensorLock28, btnSensorLock29
            };

            // 表示画面 (0ページ目固定)
            int dspPage = 0;

            for (int i = 0; i < senBtns.Length; i++)
            {
                int index = mainfrm.dspIndexCalc(dspPage, i);
                sensorLockDspUpate(index, senBtns[i]);
            }
        }

        /// <summary>
        /// 指示されたセンサーロックボタンの状態を返す処理
        /// </summary>
        /// <param name="index">ボタン配列の添え字</param>
        /// <param name="ctl">ボタン</param>
        private void sensorLockDspUpate(int index, Control ctl)
        {
            // 値を取得する
            sensorLockDspStruct senDsp;
            if (sensorDspMap.TryGetValue(index, out senDsp))
            {
                ctl.Visible = true;
                Program.MainForm.refreshControl(SystemConstants.STATUS_DISPLAY_MSG, senDsp.MsgID, ctl);

                if (senDsp.ReadBtn != null)
                {
                    mainfrm.CheckBtnAnd_ChangeColor(senDsp.ReadBtn.BtnID, ctl, senDsp.ReadBtn.ReverseFlg);
                }
            }
            else
            {
                ctl.Visible = false;
            }
        }

        /// <summary>
        /// センサーロックイベントを割り当てる処理
        /// </summary>
        /// <param name="btnSensorLockPosition"></param>
        /// <param name="btn"></param>
        private void setSensorLockBtn(int btnSensorLockPosition, Button btn)
        {
            btn.Click += new EventHandler(delegate(object sender, EventArgs args)
            {
                // 表示画面は0固定
                int dspPage = 0;

                // mapから割当データを取得する
                sensorLockDspStruct senDsp;
                int index = mainfrm.dspIndexCalc(dspPage, btnSensorLockPosition);
                if (sensorDspMap.TryGetValue(index, out senDsp))
                {
                    if (senDsp.WriteBtn != null)
                    {
                        mainfrm.WritePushBtn(senDsp.WriteBtn.BtnID, senDsp.WriteBtn.BtnMotion);
                    }
                }
            });
        }

        /// <summary>
        /// グリッドビューにデータを表示させる処理
        /// </summary>
        /// <param name="view"></param>
        /// <param name="group"></param>
        private void viewDisp(DataGridView view, int type, int group)
        {
            int[] ID = null;

            view.CurrentCell = null;
            Program.DataController.GetMemryDataGroupList(type, group, ref ID);

            int rowCount = 0;
            string min = "";
            string max = "";
            string value = "";
            string name = "";
            string unit = "";

            // IDがnullの場合、抜ける
            if (ID == null) return;

            foreach (var workid in ID)
            {
                // 補正値
                if (type == SystemConstants.WORKID_TYPE_CORRECTDATA)
                {

                    // 名称を取得する(どうやるか？）
                    name = Utility.GetMessageString(SystemConstants.CORRECT_MSG, workid);

                    // 範囲を取得する
                    Program.DataController.GetCorrectDataRangeStr(workid, ref min, ref max);

                    // 値を取得する
                    Program.DataController.ReadCorrectDataStr(workid, ref value);

                    //単位を取得する
                    Program.DataController.GetCorrectDataUnit(workid, ref unit);
                }
                // タイミング
                else
                {
                    // 名称を取得する(どうやるか？）
                    name = Utility.GetMessageString(SystemConstants.TIMMING_MSG, workid);

                    // 範囲を取得する
                    Program.DataController.GetTimingDataRangeStr(workid, ref min, ref max);

                    // 値を取得する
                    Program.DataController.ReadTimingDataStr(workid, ref value);

                    //単位を取得する
                    Program.DataController.GetTimingDataUnit(workid, ref unit);
                }
                // 値を設定する
                view.Rows.Add(new Object[] { workid, name, string.Format("{0} <-> {1}", new object[] { min, max }), value, unit });

                // 背景色を設定する
                if ((rowCount % 2) == 0)
                {
                    // 偶数の場合、背景白色
                    view[1, rowCount].Style.BackColor = Color.White;
                }
                else
                {
                    // 奇数の場合、背景薄青色
                    view[1, rowCount].Style.BackColor = Color.LightGreen;
                }

                rowCount++;
            }
        }

        /// <summary>
        /// グリッドビューのテキストボックス上でEnterが押された時の処理
        /// </summary>
        /// <param name="view"></param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CellValidating(DataGridView view, int type, object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (Program.SystemData.tachpanel == true) return;
            if (e.ColumnIndex < 3) return;

            int workid = Int32.Parse(view.Rows[e.RowIndex].Cells[0].Value.ToString()) + (e.ColumnIndex - 3);
            object value = e.FormattedValue;

            Program.MainForm.EnterDataGridView(type, workid, value);

#if MAINTELOG
            // メンテナンスログを更新する
            refreshMainteLog();
#endif            
        }

        /// <summary>
        /// 閉じるボタンを押下したときのイベント
        /// フォームの表示を消す
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            Visible = false;
        }

        /// <summary>
        /// フォームを閉じているときのイベント
        /// フォームの表示を消し、閉じない
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void systemConfigurationfrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Visible = false;
        }

        /// <summary>
        /// グリッドビュー入力イベントを設定する処理
        /// </summary>
        /// <param name="WorkIDType"></param>
        /// <param name="dataGridView"></param>
        private void setDataGridEvent(int WorkIDType, DataGridView dataGridView)
        {
            dataGridView.CellValidating += new DataGridViewCellValidatingEventHandler(delegate(object sender, DataGridViewCellValidatingEventArgs args)
            {
                CellValidating(dataGridView, WorkIDType, sender, args);
            });
        }

        /// <summary>
        /// ボタンの状態を取得し、状態に応じて背景色を変更する処理
        /// ON :Gray
        /// Off:Red
        /// </summary>
        /// <param name="BtnID"></param>
        /// <param name="Btn"></param>
        private void CheckBtnAnd_ChangeColor(int BtnID, Button Btn)
        {
            int status = 0;
            Program.DataController.ReadPushBtn(BtnID, ref status);

            if (status == SystemConstants.BTN_ON)
            {
                Btn.BackColor = Color.Red;
            }
            else
            {
                Btn.BackColor = Color.Gray;
            }

        }

        /// <summary>
        /// 設定ボタンを押下したときのイベント
        /// システムの設定を反映させる
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetting_Click(object sender, EventArgs e)
        {
            #region パスワード設定
            // パスワードと確認が異なる場合はエラー
            if (maskedTextPASSWORD.Text != maskedTextCHECK.Text)
            {
                Utility.ShowErrorMsg(SystemConstants.SYSTEM_MSG018);
                return;
            }

            //パスワードの保存
            Program.SystemData.password = maskedTextPASSWORD.Text;
            #endregion

            #region 自動機操作設定
            // 機械のみの場合
            if (radioButtonMain.Checked)
            {
                Program.SystemData.machineoperation = "machine";
            }
            // 自動機・PCの場合
            else if (radioButtonMain_PC.Checked)
            {
                Program.SystemData.machineoperation = "both";
            }
            #endregion

            #region 通信設定
            Program.SystemData.comport = comboCOMPORT.Text;
            Program.SystemData.borate = comboBORATE.SelectedIndex + 1;
            Program.SystemData.dataBits = comboDATABIT.SelectedIndex + 1;
            Program.SystemData.stopBits = comboSTOPBIT.SelectedIndex + 1; ;
            Program.SystemData.parity = comboPARITY.SelectedIndex + 1;
            Program.SystemData.handshake = comboflow_control.SelectedIndex + 1;
            #endregion

            #region Language設定
            if (radioJAPANESE.Checked == true)
            {
                Program.SystemData.culture = "ja-JP";
            }
            else if (radioENGLISH.Checked == true)
            {
                Program.SystemData.culture = "en-US";
            }
            else
            {
                Program.SystemData.culture = "zh-CN";
            }
            #endregion

            #region テンキー使用設定
            Program.SystemData.tachpanel = checkBoxTACHPANEL.Checked;
            #endregion

            #region 設定をXMLファイルに保存
            try
            {
                Program.SystemData.Save();
            }
            catch (Exception)
            {
                Utility.ShowErrorMsg(SystemConstants.SYSTEM_MSG013);
                return;
            }

            // システム設定を行いました。
            Utility.ShowInfoMsg(SystemConstants.SYSTEM_MSG019);
            #endregion
        }

        /// <summary>
        /// グリッドビューの内容を更新する処理
        /// </summary>
        /// <param name="view"></param>
        /// <param name="type"></param>
        private void GridUpdate(DataGridView view, int type)
        {
            int rowCount = view.Rows.Count;
            string value = "";

            //GridViewの更新ｓ
            for (int y = 0; y < rowCount; y++)
            {
                int workid = Int32.Parse(view.Rows[y].Cells[0].Value.ToString());

                if (type == SystemConstants.WORKID_TYPE_CORRECTDATA)
                {
                    // 値を取得する
                    Program.DataController.ReadCorrectDataStr(workid, ref value);
                }
                else
                {
                    // 値を取得する
                    Program.DataController.ReadTimingDataStr(workid, ref value);
                }
                var cell = view.Rows[y].Cells[3];

                // 値が編集中でなければ、値を変更する
                if (!cell.IsInEditMode)
                {
                    view.Rows[y].Cells[3].Value = value;
                }
            }

        }

        /// <summary>
        /// フォームが表示されたときに設定を取得表示内容を更新
        /// システム設定の内容の更新
        /// メンテナンス履歴内容の更新
        /// </summary>
        private void systemConfigurationfrm_VisibleChanged(object sender, EventArgs e)
        {
            // true->falseになったときは、何もしない
            if (Visible == false)
            {
                return;
            }

            string[] com = null;
            
            #region パスワード設定
            maskedTextPASSWORD.Text = Program.SystemData.password;
            maskedTextCHECK.Text = Program.SystemData.password;
            #endregion

            #region 自動機操作設定
            switch (Program.SystemData.machineoperation)
            {
                case "machine":
                    radioButtonMain.Checked = true;
                    break;

                case "both":
                    radioButtonMain_PC.Checked = true;
                    break;
            }
            #endregion

            #region 言語設定
            switch (Program.SystemData.culture)
            {

                case "ja-JP":
                    radioJAPANESE.Checked = true;
                    break;
                case "en-US":
                    radioENGLISH.Checked = true;
                    break;
                case "zh-CN":
                    radioCHINESE.Checked = true;
                    break;
            }
            #endregion

            #region 通信設定

            // 使用しているPCのCOMポート設定を取得
            try
            {
                com = SerialPort.GetPortNames();
            }
            catch (Exception)
            {

            }
            comboCOMPORT.Items.Clear();

            foreach (var tmp in from c in com orderby portNameToInt(c) select c)
            {
                comboCOMPORT.Items.Add(tmp);
            }

            // COMポートの値を設定する
            comboCOMPORT.SelectedIndex = comboCOMPORT.Items.IndexOf(Program.SystemData.comport);
            
            // ボーレート
            switch (Program.SystemData.borate)
            {
                case 1:
                    comboBORATE.SelectedIndex = 0;
                    break;

                case 2:
                    comboBORATE.SelectedIndex = 1;
                    break;

                case 3:
                    comboBORATE.SelectedIndex = 2;
                    break;

                case 4:
                    comboBORATE.SelectedIndex = 3;
                    break;

                case 5:
                    comboBORATE.SelectedIndex = 4;
                    break;

                case 6:
                    comboBORATE.SelectedIndex = 5;
                    break;

                case 7:
                    comboBORATE.SelectedIndex = 6;
                    break;
            }
            
            // データビット
            switch (Program.SystemData.dataBits)
            {
                case 1:
                    comboDATABIT.SelectedIndex = 0;
                    break;
                case 2:
                    comboDATABIT.SelectedIndex = 1;
                    break;
            }
            
            // ストップビット
            switch (Program.SystemData.stopBits)
            {
                case 1:
                    comboSTOPBIT.SelectedIndex = 0;
                    break;
                case 2:
                    comboSTOPBIT.SelectedIndex = 1;
                    break;
            }
            
            // パリティビット
            switch (Program.SystemData.parity)
            {
                case 1:
                    comboPARITY.SelectedIndex = 0;
                    break;
                case 2:
                    comboPARITY.SelectedIndex = 1;
                    break;
                case 3:
                    comboPARITY.SelectedIndex = 2;
                    break;
            }
            
            // フロー制御
            switch (Program.SystemData.handshake)
            {
                case 1:
                    comboflow_control.SelectedIndex = 0;
                    break;
                case 2:
                    comboflow_control.SelectedIndex = 1;
                    break;
                case 3:
                    comboflow_control.SelectedIndex = 2;
                    break;
            }
            #endregion

            #region テンキー使用設定
            checkBoxTACHPANEL.Checked = Program.SystemData.tachpanel;
            #endregion

#if MAINTELOG
            // メンテナンスログを更新する
            refreshMainteLog();
#endif
        }

        /// <summary>
        /// フォーム表示されたときのイベント
        /// タブ表示を初期設定
        /// メンテナンス履歴の項目リストの設定
        /// </summary>
        private void systemConfigurationfrm_Shown(object sender, EventArgs e)
        {
            #region グリッドビュー表示
            // 補正値タブ
            viewDisp(systemConfiguration_CORR_GROUP_BASEMACHINE1View, SystemConstants.WORKID_TYPE_CORRECTDATA, SystemConstants.CORR_GROUP_BASEMACHINE1);    // ベースマシン
            viewDisp(systemConfiguration_CORR_GROUP_FEED1View, SystemConstants.WORKID_TYPE_CORRECTDATA, SystemConstants.CORR_GROUP_FEED1);                  // ワイヤーフィード
            viewDisp(systemConfiguration_CORR_GROUP_SLIDER1View, SystemConstants.WORKID_TYPE_CORRECTDATA, SystemConstants.CORR_GROUP_POSITION1);              // No1 前後
            viewDisp(systemConfiguration_CORR_GROUP_STRIP1View, SystemConstants.WORKID_TYPE_CORRECTDATA, SystemConstants.CORR_GROUP_CUTSTRIP1);                // ストリップ
            
            // タイミングタブ
            viewDisp(systemConfiguration_TIMM_GROUP_SIDE1VIEW, SystemConstants.WORKID_TYPE_TIMINGDATA, SystemConstants.TIMM_GROUP_SIDE1);                   // 1側
            viewDisp(systemConfiguration_TIMM_GROUP_SIDE2VIEW, SystemConstants.WORKID_TYPE_TIMINGDATA, SystemConstants.TIMM_GROUP_SIDE2);                   // 2側
            #endregion

#if MAINTELOG
            // ユニットコンボボックスを追加する
            addUnitComboList();
            comboUnitName.SelectedIndex = 0;
#endif
        }

        /// <summary>
        /// メンテナンス履歴の内容を更新する処理
        /// </summary>
        private void refreshMainteLog()
        {
#if MAINTELOG
            string msgStr = "";
            int msgCode = 0;

            // 処理を早くする為に、再描画を停止する
            listMainteHistory.BeginUpdate();

            // 値を消去する
            listMainteHistory.Items.Clear();

            // 値をクラスから取得する
            MainteLogStruct[] mainteLogs;
            mainteLogs = Program.MainteLog.GetRecords();

            foreach (MainteLogStruct maintelog in mainteLogs)
            {
                // 日付部分を作成する
                msgStr = maintelog.DateStr + " " + maintelog.TimeStr + " ";

                // メンテナンスログのタイプ毎に処理を分ける
                switch (maintelog.LogType)
                {
                    case SystemConstants.MAINTELOG_RECORD_TYPE_WORKID:
                        switch (maintelog.WorkIDType)
                        {
                            case SystemConstants.WORKID_TYPE_CORRECTDATA:
                                msgCode = SystemConstants.CORRECT_MSG;
                                break;
                            case SystemConstants.WORKID_TYPE_TIMINGDATA:
                                msgCode = SystemConstants.TIMMING_MSG;
                                break;
                            case SystemConstants.WORKID_TYPE_WORKDATA:
                                msgCode = SystemConstants.WORK_MSG;
                                break;
                        }

                        msgStr += "[" + Utility.GetMessageString(msgCode, maintelog.WorkID) + "] "
                            + maintelog.OldValue + " -> " + maintelog.NewValue;
                        break;
                    case SystemConstants.MAINTELOG_RECORD_TYPE_COMMENT:
                        msgStr += Utility.GetMessageString(SystemConstants.MAINTELOG_MSG, maintelog.Unit) + " ";
                        msgStr += maintelog.Comment;
                        break;
                    default:
                        msgStr = "";
                        break;
                }

                if (msgStr.Length > 0)
                    listMainteHistory.Items.Add(msgStr);
            }

            // 描画を再開する
            listMainteHistory.EndUpdate();
#endif
        }

        /// <summary>
        /// 追加ボタンの押下したときのイベント
        /// メンテナンス履歴へ手動でレコードを追加
        /// </summary>
        private void btnAddMaintenanceRecord_Click(object sender, EventArgs e)
        {
#if MAINTELOG
            string commentStr = textAddMaintenanceRecord.Text.Trim();
            Program.MainteLog.AddRecord(selectedUnitMsgCode(), commentStr);

            // レコードを更新する
            refreshMainteLog();

            // 入力欄を空欄にする
            textAddMaintenanceRecord.Clear();
#endif
        }

        /// <summary>
        /// メンテナンス履歴の選ばれたインデックスのメッセージコードを返す処理
        /// </summary>
        /// <returns>msgCode</returns>
        private int selectedUnitMsgCode()
        {
            int msgCode = 0;
#if MAINTELOG
            int indexNo;
            indexNo = comboUnitName.SelectedIndex;

            if (indexNo >= 0 && indexNo < showMainteUnitCodes.Count())
            {
                msgCode = showMainteUnitCodes[indexNo];
            }
#endif
            return msgCode;
        }

        /// <summary>
        /// メンテナンス履歴の項目をコンボボックスに登録する処理
        /// </summary>
        private void addUnitComboList()
        {
#if MAINTELOG
            string unitStr;

            // リストをクリアする
            comboUnitName.Items.Clear();

            // コンボボックスにデータを入れる
            for (int i = 0; i < showMainteUnitCodes.Count(); i++)
            {
                unitStr = Utility.GetMessageString(SystemConstants.MAINTELOG_MSG, showMainteUnitCodes[i]);
                comboUnitName.Items.Add(unitStr);
            }
            comboUnitName.SelectedIndex = 0;
#endif
        }

        /// <summary>
        /// エクスポートボタンを押下したときのイベント
        /// 補正値・タイミングデータをCSVファイルとして出力する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExport_Click(object sender, EventArgs e)
        {
            // 初期ファイル名を設定
            string initialFileName = "exportData.csv";
            int groupType;
            if (sender == btnCorrectionOutput)
            {
                initialFileName = "correctData.csv";
                groupType = SystemConstants.WORKID_TYPE_CORRECTDATA;
            }
            else if (sender == btnTimmingOutput)
            {
                initialFileName = "timmingData.csv";
                groupType = SystemConstants.WORKID_TYPE_TIMINGDATA;
            }
            else
            {
                return;
            }

            // ファイルダイアログを表示する
            SaveFileDialog sfd1 = new SaveFileDialog();
            sfd1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            sfd1.FileName = initialFileName;
            sfd1.DefaultExt = "csv";
            sfd1.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
            sfd1.FilterIndex = 1;
            sfd1.AddExtension = true;
            sfd1.OverwritePrompt = true;
            sfd1.RestoreDirectory = false;
            sfd1.CreatePrompt = false;

            int ret;
            if (sfd1.ShowDialog() == DialogResult.OK)
            {
                ret = Program.DataController.ExportCurrentData(groupType, sfd1.FileName);
            }

            sfd1.Dispose();
        }

        /// <summary>
        /// バンクデータエクスポートボタンの押下したときのイベント
        /// バンクデータを指定フォルダへ保存する
        /// </summary>
        private void btnBankExport_Click(object sender, EventArgs e)
        {
            string xmlFileName = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\JAM\\Alchemist\\bankdata.xml";

            //ファイルダイアログを表示する
            SaveFileDialog sfd1 = new SaveFileDialog();
            sfd1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            sfd1.FileName = "bankdata.xml";
            sfd1.DefaultExt = "xml";
            sfd1.Filter = "xml files (*.xml)|*.xml";
            sfd1.FilterIndex = 1;
            sfd1.AddExtension = true;
            sfd1.OverwritePrompt = true;
            sfd1.RestoreDirectory = false;
            sfd1.CreatePrompt = false;

            if (sfd1.ShowDialog() == DialogResult.OK)
            {
                System.IO.File.Copy(xmlFileName, sfd1.FileName, true);
            }

            sfd1.Dispose();
        }

        /// <summary>
        /// バンクデータインポートボタンの押下したときのイベント
        /// 読み込んだバンクデータでファイルを置き換える
        /// </summary>
        private void btnBankImport_Click(object sender, EventArgs e)
        {
            string xmlFileName = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\JAM\\Alchemist\\bankdata.xml";
            BankDataStorage bankDataStorage = new BankDataStorage();

            //ファイルダイアログを表示する
            OpenFileDialog ofd1 = new OpenFileDialog();
            ofd1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            ofd1.FileName = "bankdata.xml";
            ofd1.DefaultExt = "xml";
            //ofd1.Filter = "xml files (*.xml)|*.xml";
            ofd1.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            ofd1.FilterIndex = 1;
            ofd1.AddExtension = true;

            // ファイルを選択された場合
            if (ofd1.ShowDialog() == DialogResult.OK)
            {
                // ファイルのスキーマチェック                
                if (bankDataStorage.Load(ofd1.FileName))
                {
                    // 現在のデータを保存バックアップファイルを合わせて作成。
                    Program.DataController.BankDataSave();

                    // 選択したファイルをオリジナルにコピー
                    System.IO.File.Copy(ofd1.FileName,　xmlFileName, true);

                    // バンクデータファイル再読込み
                    Program.DataController.BankDataReLoad();

                    // selectednoを設定する
                    int selectno = 0;
                    mainfrm.BankNoWrite(selectno);
                    // バンクデータをロードする
                    int result = mainfrm.BankDataLoad(selectno);
                    // バンクデータをセーブする
                    result = mainfrm.BankDataSave(selectno);

                    // データをインポートしました。
                    Utility.ShowInfoMsg(SystemConstants.SYSTEM_MSG036);
                }
                else
                {
                    // データをインポートできませんでした。
                    Utility.ShowErrorMsg(SystemConstants.SYSTEM_MSG037);               
                }
            }
        }

        /// <summary>
        /// IOモニターボタンを押下したときのイベント
        /// IOモニター画面を表示する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIoMonitorDsp_Click(object sender, EventArgs e)
        {
            if (ioMonitorForm != null)
            {
                if (!ioMonitorForm.Visible)
                    ioMonitorForm.Show();
            }
        }

        private void btnLearnDataItemEdit_Click(object sender, EventArgs e)
        {
            if (learndataitemeditForm != null)
            {
                if (!learndataitemeditForm.Visible)
                {
                    learndataitemeditForm.Show();
                }
            }
        }

    }
}
