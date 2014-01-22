using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Alchemist
{
    partial class mainfrm : Form
    {
        
        #region コントロールのイベント設定関数

        /// <summary>
        /// WorkIDを持つボタンのイベントを設定する
        /// </summary>
        /// <param name="WorkID"></param>
        /// <param name="btnAction"></param>
        /// <param name="btn"></param>
        public void SetBtnEvent(int WorkID, int btnAction, Button btn)
        {
            btn.Click += new EventHandler(delegate(object sender, EventArgs args)
            {
                mainfrm.WritePushBtn(WorkID, btnAction);
            });
        }

        /// <summary>
        /// テキストボックスでEnterキー入力時のイベントを設定する。
        /// </summary>
        /// <param name="workIDType"></param>
        /// <param name="WorkID"></param>
        /// <param name="customtextBox"></param>
        public void SetTextBoxEvent(int workIDType, int WorkID, CustomTextBox customtextBox)
        {
            customtextBox.EnterKeyDown += delegate(EventArgs e)
            {
                // 入力チェックと値更新
                EnterTextBox(workIDType, WorkID, customtextBox);

                // フォーカスアウトする
                Form frm = customtextBox.FindForm();
                frm.ActiveControl = null;
            };
        }

        /// <summary>
        /// テキストボックスでEnterキー入力時のイベントを設定する。
        /// 入力された値が適正かだけを確認する。メモリ更新はしない。
        /// </summary>
        /// <param name="workIDType"></param>
        /// <param name="WorkID"></param>
        /// <param name="customtextBox"></param>
        public void SetCheckOnlyTextBoxEvent(int workIDType, int WorkID, CustomTextBox customtextBox)
        {
            customtextBox.EnterKeyDown += delegate(EventArgs e)
            {
                double value;
                string message = "";

                // テキストボックスの入力チェック
                if (checkTextBoxValue(workIDType, WorkID, customtextBox.Text, out value, out message) == false)
                {
                    customtextBox.Text = "";
                    Utility.ShowErrorMsg(message);
                    return;
                }

                // フォーカスアウトする
                Form frm = customtextBox.FindForm();
                frm.ActiveControl = null;
            };
        }

        /// <summary>
        /// テキストボックスでエンターキー押されたときの処理。
        /// 入力をチェックして適正ならメモリに書き込み。
        /// </summary>
        /// <param name="workIDType"></param>
        /// <param name="WorkID"></param>
        /// <param name="customtextBox"></param>
        public void EnterTextBox(int WorkIDType, int WorkID, CustomTextBox customtextBox)
        {
            double value;
            string message = "";

            // テキストボックスの入力チェック
            if (checkTextBoxValue(WorkIDType, WorkID, customtextBox.Text, out value, out message) == false)
            {
                Utility.ShowErrorMsg(message);
                return;
            }

            // データの書き込み
            switch (WorkIDType)
            {
                case SystemConstants.WORKID_TYPE_WORKDATA:
                    // ワークデータを書き込む
                    mainfrm.WriteWorkData(WorkID, value);
                    break;
                case SystemConstants.WORKID_TYPE_CORRECTDATA:
                    // 補正値クデータを書き込む
                    mainfrm.WriteCorrectData(WorkID, value);
                    break;
                case SystemConstants.WORKID_TYPE_TIMINGDATA:
                    // ワークデータを書き込む
                    mainfrm.WriteTimingData(WorkID, value);
                    break;
            }
        }

        /// <summary>
        /// テキストボックスでエンターキー押されたときの処理。
        /// 入力をチェックだけする。
        /// </summary>
        /// <param name="workIDType"></param>
        /// <param name="WorkID"></param>
        /// <param name="customtextBox"></param>
        public void EnterCheckTextBox(int WorkIDType, int WorkID, CustomTextBox customtextBox)
        {
            double value;
            string message = "";

            // テキストボックスの入力チェック
            if (checkTextBoxValue(WorkIDType, WorkID, customtextBox.Text, out value, out message) == false)
            {
                // エラーなら入力内容をクリア：更新がかかるやつなら元の値になる
                customtextBox.Text = ""; 
                Utility.ShowErrorMsg(message);
                return;
            }
        }

        /// <summary>
        /// テキストボックスでエンターキー押されたときの処理
        /// スピード画面の切断長閾値の入力は制限がほかのと違うので分ける。
        /// 閾値１は０～閾値２未満
        /// 閾値２は閾値１＋最小係数以上
        /// </summary>
        /// <param name="workIDType"></param>
        /// <param name="WorkID"></param>
        /// <param name="customtextBox"></param>
        public void EnterTextBoxWireLengthThres(int workIDType, int WorkID, CustomTextBox customtextBox)
        {
            double value;
            string message;
            double workdata = 0;
            string thres;

            // 形式チェック
            if (Program.MainForm.checkTextBoxValue(workIDType, WorkID, customtextBox.Text, out value, out message) == false)
            {
                Utility.ShowErrorMsg(message);
                return;
            }

            switch (WorkID)
            {
                case SystemConstants.FEED_SPEED_THRES1:

                    // 閾値２を読む
                    Program.DataController.ReadWorkData(SystemConstants.FEED_SPEED_THRES2, ref workdata);

                    thres = workdata.ToString();

                    // 適正範囲チェック
                    if (customtextBox.Text == "0" || workdata <= value)
                    {
                        string workname = Utility.GetMessageString(SystemConstants.WORK_MSG, SystemConstants.FEED_SPEED_THRES2);
                        Utility.ShowErrorMsg(SystemConstants.SYSTEM_MSG027, workname, "0", thres);
                        return;
                    }

                    break;

                case SystemConstants.FEED_SPEED_THRES2:
                    
                    // 閾値１を読む
                    Program.DataController.ReadWorkData(SystemConstants.FEED_SPEED_THRES1, ref workdata);

                    thres = GetSpeedThresString(SystemConstants.FEED_SPEED_THRES1);

                    // 範囲チェック
                    if (value <= workdata)
                    {
                        string workname = Utility.GetMessageString(SystemConstants.WORK_MSG, SystemConstants.FEED_SPEED_THRES2);
                        Utility.ShowErrorMsg(SystemConstants.SYSTEM_MSG022, workname, thres);
                        return;
                    }

                    break;
            }

            // ワークデータを書き込む
            mainfrm.WriteWorkData(WorkID, value);
        }

        /// <summary>
        /// データグリッドのセルでエンターキー押されたときの処理。
        /// 入力をチェックして適正ならメモリに書き込み。
        /// </summary>
        /// <param name="workIDType"></param>
        /// <param name="WorkID"></param>
        /// <param name="customtextBox"></param>
        public void EnterDataGridView(int WorkIDType, int WorkID, object GridValue)
        {
            double value;
            string message = "";

            // テキストボックスの入力チェック
            if (checkTextBoxValue(WorkIDType, WorkID, GridValue, out value, out message) == false)
            {
                Utility.ShowErrorMsg(message);
                return;
            }

            // データの書き込み
            switch (WorkIDType)
            {
                case SystemConstants.WORKID_TYPE_WORKDATA:
                    // ワークデータを書き込む
                    mainfrm.WriteWorkData(WorkID, value);
                    break;
                case SystemConstants.WORKID_TYPE_CORRECTDATA:
                    // 補正値クデータを書き込む
                    mainfrm.WriteCorrectData(WorkID, value);
                    break;
                case SystemConstants.WORKID_TYPE_TIMINGDATA:
                    // ワークデータを書き込む
                    mainfrm.WriteTimingData(WorkID, value);
                    break;
            }
        }

        /// <summary>
        /// テンキーから入力完了イベント
        /// </summary>
        /// <param name="td"></param>
        public void TenKeyEnterEvent(TenKeyDataStruct td)
        {
            switch (td.obj.GetType().Name)
            {
                case "CustomTextBox":
                    CustomTextBox ct = (CustomTextBox)td.obj;
                    ct.Text = td.value.ToString();
                    switch(td.actiontype)
                    {
                        case SystemConstants.TENKEY_INPUT_ONLY:
                            break;
                        case SystemConstants.TENKEY_INPUT_DATA:
                            EnterTextBox(td.workidtype, td.workid, ct);
                            break;
                        case SystemConstants.TENKEY_INPUT_CHECK: 
                            EnterCheckTextBox(td.workidtype, td.workid, ct);
                            break;        
                        case SystemConstants.TENKEY_INPUT_WIRETHRES:
                            EnterTextBoxWireLengthThres(td.workidtype, td.workid, ct);
                            break;
                    }
                    break;
                case "DataGridView":
                    DataGridView dg = (DataGridView)td.obj;
                    dg.Rows[td.rowindex].Cells[td.columindex].Value = td.value;
                    string a = dg.Name;
                    switch(td.actiontype)
                    {
                        case SystemConstants.TENKEY_INPUT_ONLY:
                            break;
                        case SystemConstants.TENKEY_INPUT_DATA:
                            EnterDataGridView(td.workidtype, td.workid, td.value);
                            break;
                    }
                    break;
            }
        }

        /// <summary>
        /// テキストボックスのクリックイベントを設定する。
        /// テンキー表示対応
        /// </summary>
        /// <param name="workIDType"></param>
        /// <param name="WorkID"></param>
        /// <param name="customtextBox"></param>
        public void ClickTextBoxEvent(int workIDType, int actionType, int WorkID, CustomTextBox customtextBox)
        {
            customtextBox.Click += delegate(Object sender, EventArgs e)
            {
                if (Program.SystemData.tachpanel == false) return;

                CustomTextBox ct = (CustomTextBox)sender;
                
                // データなしなら０に置き換え
                double preval;
                if (ct.Text == "") preval = 0;
                else preval = double.Parse(ct.Text);

                // テンキーを表示
                TenkeyControl tc = Program.TenkeyController;
                tc.tenKeyData.obj = sender;
                tc.tenKeyData.value = preval;
                tc.tenKeyData.workid = WorkID;
                tc.tenKeyData.workidtype = workIDType;
                tc.tenKeyData.actiontype = actionType;
                tc.tenkeyFormShow();

                // フォーカスアウトする
                Form frm = customtextBox.FindForm();
                frm.ActiveControl = null;
            };
        }

        /// <summary>
        /// データグリッドのクリックイベントを設定する。
        /// テンキー表示対応
        /// </summary>
        /// <param name="workIDType"></param>
        /// <param name="dataGridView"></param>
        public void ClickDataGridViewEvent(int workIDType, int actionType, DataGridView dataGridView)
        {
            dataGridView.CellClick += delegate(object sender, DataGridViewCellEventArgs e)
            {
                if (Program.SystemData.tachpanel == false) return;

                if (e.ColumnIndex != 3) return;
                if (e.RowIndex < 0) return;

                DataGridView dg = (DataGridView)sender;
                
                // データなしなら０に置き換え
                double preval;
                if (dg.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null) preval = 0;
                else preval = double.Parse(dg.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());

                // テンキーを表示
                TenkeyControl tc = Program.TenkeyController;
                tc.tenKeyData.obj = sender;
                tc.tenKeyData.value = preval;
                tc.tenKeyData.rowindex = e.RowIndex;
                tc.tenKeyData.columindex = e.ColumnIndex;
                tc.tenKeyData.workid = Int32.Parse(dg.Rows[e.RowIndex].Cells[0].Value.ToString());
                tc.tenKeyData.workidtype = workIDType;
                tc.tenKeyData.actiontype = actionType;
                tc.tenkeyFormShow();

                // フォーカスアウトする
                dg.CurrentCell = null;               
            };
        }

        #endregion

        #region コントロールの状態確認関数

        /// <summary>
        /// コントロール内のテキストボックスの長さを設定する。
        /// </summary>
        /// <param name="ctl"></param>
        /// <param name="Length"></param>
        public void SetTextBoxLength(Control ctl, int Length)
        {
            foreach (Control control in ctl.Controls)
            {
                // システムフォームの場合、実行しない
                if (control is systemConfigurationfrm) return;

                // テキストボックスならばテキスト長を設定する
                if (control is CustomTextBox)
                {
                    if ((control as CustomTextBox).TextLengthChange)
                    {
                        (control as CustomTextBox).MaxLength = Length;
                    }
                }
                else if (control is TextBox)
                {
                    (control as TextBox).MaxLength = Length;
                }

                // 子コントロールを含む場合は再帰的に処理を行う。
                else if (control.HasChildren)
                {
                    SetTextBoxLength(control, Length);
                }
            }
        }

        /// <summary>
        /// テキストボックスの値をチェックする。
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="workID"></param>
        /// <param name="inValue"></param>
        /// <param name="outValue"></param>
        /// <param name="errMessage"></param>
        /// <returns></returns>
        public bool checkTextBoxValue(int Type, int workID, object inValue, out double outValue, out string errMessage)
        {
            int ret = 0;
            string workName = "";
            string strValue = "";
            double min = 0;
            double max = 0;
            string strMin = "";
            string strMax = "";

            string format = "";

            // 範囲を取得
            switch (Type)
            {
                case SystemConstants.WORKID_TYPE_WORKDATA:
                    // 範囲を取得
                    ret = Program.DataController.GetWorkDataRange(workID, ref min, ref max);
                    Program.DataController.GetWorkDataRangeStr(workID, ref strMin, ref strMax);
                    workName = Utility.GetMessageString(SystemConstants.WORK_MSG, workID);
                    break;
                case SystemConstants.WORKID_TYPE_CORRECTDATA:
                    // 範囲を取得
                    ret = Program.DataController.GetCorrectDataRange(workID, ref min, ref max);
                    Program.DataController.GetCorrectDataRangeStr(workID, ref strMin, ref strMax);
                    workName = Utility.GetMessageString(SystemConstants.CORRECT_MSG, workID);
                    break;
                case SystemConstants.WORKID_TYPE_TIMINGDATA:
                    // 範囲を取得
                    ret = Program.DataController.GetTimingDataRange(workID, ref min, ref max);
                    Program.DataController.GetTimingDataRangeStr(workID, ref strMin, ref strMax);
                    workName = Utility.GetMessageString(SystemConstants.TIMMING_MSG, workID);
                    break;
            }

            // ワークIDが見つからない場合
            if (SystemConstants.ERR_NO_WORK_ID == ret)
            {
                errMessage = Utility.GetMessageString(SystemConstants.SYSTEM_MSG, SystemConstants.SYSTEM_MSG023);
                outValue = 0;
                return false;
            }

            // nullチェック
            if (inValue == null)
            {
                format = Utility.GetMessageString(
                    SystemConstants.SYSTEM_MSG,
                    SystemConstants.SYSTEM_MSG024
                );
                outValue = 0;
                errMessage = string.Format(format, workName);

                return false;
            }

            strValue = inValue.ToString();

            // 必須チェック
            if (strValue.Trim() == "")
            {
                format = Utility.GetMessageString(
                    SystemConstants.SYSTEM_MSG,
                    SystemConstants.SYSTEM_MSG024
                );
                outValue = 0;
                errMessage = string.Format(format, workName);

                return false;
            }

            // フォーマットチェック
            if (double.TryParse(strValue, out outValue) == false)
            {
                format = Utility.GetMessageString(
                    SystemConstants.SYSTEM_MSG,
                    SystemConstants.SYSTEM_MSG025
                );
                errMessage = string.Format(format, workName);
                return false;
            }


            // 範囲チェック
            if (Utility.CheckRange(outValue, min, max) == false)
            {
                format = Utility.GetMessageString(
                    SystemConstants.SYSTEM_MSG,
                    SystemConstants.SYSTEM_MSG026
                );
                errMessage = string.Format(format, workName, strMin, strMax);
                return false;
            }

            errMessage = "";
            return true;
        }

        #endregion

        #region コントロールの値や表示の更新関数

        /// <summary>
        /// WorkIDを持つコントロールを最新の値に更新する
        /// </summary>
        /// <param name="WorkID"></param>
        /// <param name="ctl"></param>
        /// <param name="addText"></param>
        public void refreshControl(int WorkID, Control ctl, string addText = "")
        {
            // コントロールが非表示の場合、何もしない
            if (!ctl.Visible) return;

            string value = "";

            if (!ctl.Focused)
            {
                Program.DataController.ReadWorkDataStr(WorkID, ref value);

                if (ctl.Text != value)
                {
                    ctl.Text = value + addText;
                }
            }
        }

        /// <summary>
        /// 任意の文字列に更新する
        /// </summary>
        /// <param name="DisplayString"></param>
        /// <param name="ctl"></param>
        public void refreshControl(string DisplayString, Control ctl)
        {
            // コントロールが非表示の場合、何もしない
            if (!ctl.Visible) return;

            if (!ctl.Focused)
            {
                if (ctl.Text != DisplayString)
                {
                    ctl.Text = DisplayString;
                }
            }
        }

        /// <summary>
        /// リソースファイル名に更新する
        /// </summary>
        /// <param name="type"></param>
        /// <param name="MsgID"></param>
        /// <param name="ctl"></param>
        /// <param name="addText"></param>
        public void refreshControl(int type, int MsgID, Control ctl, string addText = "")
        {
            if (!ctl.Visible) return;

            string str = Utility.GetMessageString(type, MsgID);
            str = str + addText;

            refreshControl(str, ctl);
        }

        /// <summary>
        /// WorkIDを持つコントロールを最新の値に更新する、切断長閾値専用
        /// 切断長閾値の２段目以降は、閾値に最小単位を足したものになるために値の計算を行う。
        ///ファイルがロードされていない状態で、この関数を呼び出した場合は、例外が発生します。
        /// </summary>
        /// <param name="WorkID">表示する切断長閾値のWorkID</param>
        /// <param name="ctl">値を表示するコントロール</param>
        /// <param name="addText">値に付加するテキスト</param>
        public void refreshControlSpeedThres(int WorkID, Control ctl, string addText = "")
        {
            // コントロールが非表示の場合、何もしない
            if (!ctl.Visible) return;

            // 閾値＋最小単位
            string str = GetSpeedThresString(WorkID, addText);

            if (!ctl.Focused)
            {
                if (ctl.Text != str)
                {
                    ctl.Text = str;
                }
            }
        }

        /// <summary>
        /// 閾値に最小単位を足したものになるために値の計算を行う。
        /// </summary>
        /// <param name="WorkID"></param>
        /// <param name="addText"></param>
        /// <returns></returns>
        public string GetSpeedThresString(int WorkID, string addText = "")
        {
            string valuestr = "";
            double value = 0;
            int valuefactor = 0;

            Program.DataController.ReadWorkData(WorkID, ref value);
            Program.DataController.GetWorkDataValueFactor(WorkID, ref valuefactor);
            value += 1 / valuefactor;

            valuestr = Utility.GetWorkDataString(value, valuefactor);

            return valuestr;
        }

        /// <summary>
        /// ボタンのステータスによってボタンの色を更新
        /// </summary>
        /// <param name="BtnStatus">ボタンステータス:true=ON、false=OFF</param>
        /// <param name="Ctl"></param>
        /// <param name="ReverseFlg"></param>
        public static void CheckBtnAnd_ChangeColor(bool BtnStatus, Control Ctl, bool ReverseFlg = false)
        {
            // ボタンが表示されていない場合、処理を行わない
            if (!Ctl.Visible) return;

            Color onColor, offColor;
            if (ReverseFlg)
            {
                onColor = Color.Gray;
                offColor = Color.Red;
            }
            else
            {
                onColor = Color.Red;
                offColor = Color.Gray;
            }

            //前回と変更がない場合は、処理なし
            if ((!BtnStatus && Ctl.BackColor == offColor) || (BtnStatus && Ctl.BackColor == onColor))
            {
                return;
            }
            else
            {
                if (!BtnStatus)
                    Ctl.BackColor = offColor;
                else
                    Ctl.BackColor = onColor;
            }

        }

        /// <summary>
        /// ボタンのWorkIDから状態を見てボタンの色を更新
        /// </summary>
        /// <param name="BtnID"></param>
        /// <param name="Ctl"></param>
        /// <param name="ReverseFlg"></param>
        public static void CheckBtnAnd_ChangeColor(int BtnID, Control Ctl, bool ReverseFlg = false)
        {
            bool btnStatus = false;
            int status = 0;
            Program.DataController.ReadPushBtn(BtnID, ref status);

            if (status == SystemConstants.BTN_ON)
                btnStatus = true;
            else
                btnStatus = false;

            CheckBtnAnd_ChangeColor(btnStatus, Ctl, ReverseFlg);
        }

        /// <summary>
        /// ボタンのWorkIDからボタンのイメージ画像を更新（ONとOFF画像）
        /// </summary>
        /// <param name="BtnID"></param>
        /// <param name="Btn"></param>
        /// <param name="PictureOn"></param>
        /// <param name="PictureOff"></param>
        static public void CheckBtnAnd_ChangePicture(int BtnID, Button Btn, Image PictureOn, Image PictureOff)
        {
            int status = 0;
            Program.DataController.ReadPushBtn(BtnID, ref status);

            if (status == SystemConstants.BTN_OFF)
            {
                Btn.Image = PictureOff;
            }
            else
            {
                Btn.Image = PictureOn;
            }
        }

        /// <summary>
        /// ボタンのステータスからボタンのイメージ画面を更新。
        /// </summary>
        /// <param name="Status"></param>
        /// <param name="Ctl"></param>
        /// <param name="PictureOn"></param>
        /// <param name="PictureOff"></param>
        static public void CheckAnd_ChangePicture(bool Status, PictureBox Ctl, Image PictureOn, Image PictureOff)
        {
            Image im = null;
            if (Status)
                im = PictureOn;
            else
                im = PictureOff;

            if (Ctl.Image != im)
                Ctl.Image = im;
        }

        #endregion

        #region データコントローラー書き込み用関数のエラー表示版

        /// <summary>
        /// 補正値データの更新
        /// </summary>
        /// <param name="WorkID"></param>
        /// <param name="WorkData"></param>
        /// <param name="WriteFileFlag"></param>
        /// <returns></returns>
        static public int WriteCorrectData(int WorkID, double WorkData, bool WriteFileFlag = true)
        {
            if (!Program.Initialized)
            {
                Utility.ShowErrorMsg(SystemConstants.SYSTEM_MSG028);
                return SystemConstants.ERR_SYNC_CANCELLED;
            }

            int ret = Program.DataController.WriteCorrectData(WorkID, WorkData, WriteFileFlag);

            Utility.ShowErrorCode(ret);

            return ret;
        }

        /// <summary>
        /// 加工データの更新
        /// </summary>
        /// <param name="WorkID"></param>
        /// <param name="WorkData"></param>
        /// <param name="initFlag"></param>
        /// <param name="feedFlag"></param>
        /// <returns></returns>
        static public int WriteWorkData(int WorkID, double WorkData, bool initFlag = true, bool feedFlag = false)
        {
            if (!Program.Initialized)
            {
                Utility.ShowErrorMsg(SystemConstants.SYSTEM_MSG028);
                return SystemConstants.ERR_SYNC_CANCELLED;
            }

            int ret = Program.DataController.WriteWorkData(WorkID, WorkData, initFlag, feedFlag);

            Utility.ShowErrorCode(ret);
            return ret;
        }

        /// <summary>
        /// タイミングデータの更新
        /// </summary>
        /// <param name="WorkID"></param>
        /// <param name="WorkData"></param>
        /// <param name="WriteFileFlag"></param>
        /// <returns></returns>
        static public int WriteTimingData(int WorkID, double WorkData, bool WriteFileFlag = true)
        {
            if (!Program.Initialized)
            {
                Utility.ShowErrorMsg(SystemConstants.SYSTEM_MSG028);
                return SystemConstants.ERR_SYNC_CANCELLED;
            }

            int ret = Program.DataController.WriteTimingData(WorkID, WorkData, WriteFileFlag);

            Utility.ShowErrorCode(ret);
            return ret;
        }

        /// <summary>
        /// ボタンの状態の更新
        /// </summary>
        /// <param name="BtnID"></param>
        /// <param name="BtnStatus"></param>
        /// <param name="execRelated"></param>
        /// <param name="initFlag"></param>
        /// <returns></returns>
        static public int WritePushBtn(int BtnID, int BtnStatus, bool execRelated = true, bool initFlag = true)
        {
            if (!Program.Initialized)
            {
                Utility.ShowErrorMsg(SystemConstants.SYSTEM_MSG028);
                return SystemConstants.ERR_SYNC_CANCELLED;
            }

            int ret = Program.DataController.WritePushBtn(BtnID, BtnStatus, execRelated, initFlag);

            Utility.ShowErrorCode(ret);
            return ret;
        }

        #endregion

        #region バンクデータ用関数

        /// <summary>
        /// バンクコメントの更新
        /// </summary>
        /// <param name="BankNo"></param>
        /// <param name="BankComment"></param>
        /// <returns></returns>
        static public int BankDataCommentWrite(int BankNo, string BankComment)
        {
            int ret = Program.DataController.BankDataCommentWrite(BankNo, BankComment);

            Utility.ShowErrorCode(ret);
            return ret;
        }

        /// <summary>
        /// 選択されたバンクNoの更新
        /// </summary>
        /// <param name="SelectedNo"></param>
        /// <returns></returns>
        static public int BankNoWrite(int SelectedNo)
        {
            int ret = Program.DataController.BankNoWrite(SelectedNo);

            Utility.ShowErrorCode(ret);
            return ret;
        }

        /// <summary>
        /// バンクデータの読み込み
        /// </summary>
        /// <param name="SelectedNo"></param>
        /// <returns></returns>
        static public int BankDataLoad(int SelectedNo)
        {
            int ret = Program.DataController.BankDataLoad(SelectedNo);

            Utility.ShowErrorCode(ret);
            return ret;
        }

        /// <summary>
        /// バンクデータの保存
        /// </summary>
        /// <param name="SelectedNo"></param>
        /// <returns></returns>
        static public int BankDataSave(int SelectedNo)
        {
            int ret = Program.DataController.BankDataSave(SelectedNo);

            Utility.ShowErrorCode(ret);
            return ret;
        }

        #endregion

        #region 学習データ用関数

        /// <summary>
        /// 学習データの読み込み
        /// </summary>
        /// <param name="ItemKeys"></param>
        /// <returns></returns>
        static public int LearnDataLoad(string[] ItemKeys)
        {
            int ret = Program.DataController.LearnDataLoad(ItemKeys);

            Utility.ShowErrorCode(ret);
            return ret;
        }

        /// <summary>
        /// 学習データの保存
        /// </summary>
        /// <param name="ItemKeys"></param>
        /// <returns></returns>
        static public int LearnDataSave(string[] ItemKeys)
        {
            int ret = Program.DataController.LearnDataSave(ItemKeys);

            Utility.ShowErrorCode(ret);
            return ret;
        }

        #endregion

        /// <summary>
        /// 学習データキーの保存
        /// </summary>
        /// <param name="ItemKeys"></param>
        /// <returns></returns>
        static public int LearnItemKeysSave(string[] ItemKeys)
        {
            int ret = Program.DataController.LearnItemKeysWrite(ItemKeys);

            Utility.ShowErrorCode(ret);
            return ret;
        }

        #region CFM用関数

        static public void CFMRecord(string[] ItemKeys)
        {
#if OMOIKANE
            // CFMの保存を停止する
            CFMRecordStop(omoikane.SystemConstants.CHANNEL_NO1);
            CFMRecordStop(omoikane.SystemConstants.CHANNEL_NO2);

            // 電線情報を取得
            string wireName = ItemKeys[SystemConstants.DB_GROUP_KEY_WIRENAME];
            string wireSize = ItemKeys[SystemConstants.DB_GROUP_KEY_WIRESIZE];
            string terminalName1 = ItemKeys[SystemConstants.DB_GROUP_KEY_TERMINALNAME1];
            string terminalName2 = ItemKeys[SystemConstants.DB_GROUP_KEY_TERMINALNAME2];
            string sealName1 = ItemKeys[SystemConstants.DB_GROUP_KEY_SEALNAME1];
            string sealName2 = ItemKeys[SystemConstants.DB_GROUP_KEY_SEALNAME2];

            // 1側のCFMの登録を行う
            if (IsRecordTarget(terminalName1))
            {
                // ファイル名を取得する
                string fileName1 = CreateCFMFileName(omoikane.SystemConstants.CHANNEL_NO1.ToString(), wireName, wireSize, terminalName1, sealName1);
                // ファイル保存を開始する
                CFMRecordStart(omoikane.SystemConstants.CHANNEL_NO1, fileName1);
            }

            // 2側のCFMの登録を行う
            if (IsRecordTarget(terminalName2))
            {
                // ファイル名を取得する
                string fileName2 = CreateCFMFileName(omoikane.SystemConstants.CHANNEL_NO2.ToString(), wireName, wireSize, terminalName2, sealName2);
                // ファイル保存を開始する
                CFMRecordStart(omoikane.SystemConstants.CHANNEL_NO2, fileName2);
            }
#endif

        }

        static public void CFMRecord(int BankNo)
        {
#if OMOIKANE
            // CFMの保存を停止する
            CFMRecordStop(omoikane.SystemConstants.CHANNEL_NO1);
            CFMRecordStop(omoikane.SystemConstants.CHANNEL_NO2);

            // 圧着動作のボタン状態を取得する
            int btn1 = SystemConstants.BTN_OFF, btn2 = SystemConstants.BTN_OFF;
            Program.DataController.ReadPushBtn(SystemConstants.CRIMP1_BTN, ref btn1);
            Program.DataController.ReadPushBtn(SystemConstants.CRIMP1_BTN, ref btn2);

            // 1側のCFM登録を行う
            if (btn1 == SystemConstants.BTN_ON)
            {
                string fileName1 = "BANK" + BankNo.ToString() + "_1.csv";
                CFMRecordStart(omoikane.SystemConstants.CHANNEL_NO1, fileName1, true);
            }

            // 2側のCFM登録を行う
            if (btn2 == SystemConstants.BTN_ON)
            {
                string fileName2 = "BANK" + BankNo.ToString() + "_2.csv";
                CFMRecordStart(omoikane.SystemConstants.CHANNEL_NO2, fileName2, true);
            }
#endif
        }

        /// <summary>
        /// CFMの保存動作を停止する
        /// </summary>
        /// <param name="CFMChannel"></param>
        public static void CFMRecordStop(int CFMChannel)
        {
#if OMOIKANE
            if (Program.OmoikaneConnector.IsRecord(CFMChannel))
                Program.OmoikaneConnector.StopRecord(CFMChannel);
#endif
        }

        public static void CFMRecordStart(int CFMChannel, string FileName, bool FileNameOnly = false)
        {
#if OMOIKANE
            // 念の為、ファイル名設定前に、CFMの保存動作中の場合、停止する処理を追加
            if (Program.OmoikaneConnector.IsRecord(CFMChannel))
                if (!Program.OmoikaneConnector.StopRecord(CFMChannel)) return;

            // ファイル名を変更する
            if (!Program.OmoikaneConnector.SetRecordPath(CFMChannel, FileName)) return;

            if (!FileNameOnly)
            {
                // ファイル保存を開始する
                Program.OmoikaneConnector.StartRecord(CFMChannel);
            }
#endif
        }

        private static bool IsRecordTarget(string TerminalName)
        {
            // 保存対象かをチェックする
            if (TerminalName.Length > 0)
                return true;
            else
                return false;
        }

        public static string CreateCFMFileName(string Side, string WireName, string WireSize, string TerminalName, string SealName)
        {
            // もし、ファイルパスに出来ない文字が含まれている場合、例外を発生させる
            if (!IsValidFileNameChars(WireName) || !IsValidFileNameChars(WireSize) || !IsValidFileNameChars(TerminalName) || !IsValidFileNameChars(SealName))
            {
                throw new ArgumentException();
            }

            // 防水栓が無い場合は、防水栓項目は"NOSEAL"にする
            if (SealName.Length == 0) SealName = "NOSEAL";

            // ファイルパスを作成 (Mydocument\JAM\CFMLog)
            string filePath = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\JAM\\CFMLog";

            // ファイル名を作成
            string fileName = "\\" + "Side" + Side + "_" + WireName + WireSize + "_" + TerminalName + "_" + SealName + ".csv";

            return filePath + fileName;
        }

        public static bool IsValidFileNameChars(string ItemKey)
        {
            // 使用出来ない文字列を取得する
            char[] invalidChar = Path.GetInvalidFileNameChars();

            // 使用出来ない文字が含まれるかをチェックする
            foreach (char c in invalidChar)
            {
                // 使用不可文字が含まれていた場合、falseを返す
                if (ItemKey.Contains(Convert.ToString(c)))
                    return false;
            }
            return true;
        }

        #endregion

    }
}
