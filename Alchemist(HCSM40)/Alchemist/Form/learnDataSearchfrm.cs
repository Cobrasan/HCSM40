using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Alchemist
{
    public partial class learnDataSearchfrm : Form
    {
        private const int MODE_SIDE1 = 1;
        private const int MODE_SIDE2 = 2;
        private const int MODE_CRIMP = 1;
        private const int MODE_STRIP = 2;

        /// <summary>
        /// フォームのコンストラクタ
        /// </summary>
        public learnDataSearchfrm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 初期化処理
        /// イベント登録
        /// </summary>
        public void Initialize()
        {
            Program.MainForm.AddOwnedForm(this);

            #region 切断長、ストリップ長のテキスト入力イベントを設定する
            Program.MainForm.SetCheckOnlyTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.WIRE_LENGTH1, textWireLength);
            Program.MainForm.SetCheckOnlyTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.STRIP_LENGTH1, textStripLength1);
            Program.MainForm.SetCheckOnlyTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.STRIP_LENGTH2, textStripLength2);
            #endregion

            #region テンキークリックイベント
            Program.MainForm.ClickTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.TENKEY_INPUT_CHECK, SystemConstants.WIRE_LENGTH1, textWireLength);
            Program.MainForm.ClickTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.TENKEY_INPUT_CHECK, SystemConstants.STRIP_LENGTH1, textStripLength1);
            Program.MainForm.ClickTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.TENKEY_INPUT_CHECK, SystemConstants.STRIP_LENGTH2, textStripLength2);
            #endregion

        }

        /// <summary>
        /// 表示内容を更新
        /// </summary>
        public void refresh()
        {
            #region 入力値を更新する
            textFormatCorrect(textWireLength, SystemConstants.WIRE_LENGTH1);
            textFormatCorrect(textStripLength1, SystemConstants.STRIP_LENGTH1);
            textFormatCorrect(textStripLength2, SystemConstants.STRIP_LENGTH2);
            #endregion
        }

        /// <summary>
        /// 閉じるボタンのクリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        /// <summary>
        /// フォームを閉じるイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void databasefrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Visible = false;
        }

        /// <summary>
        /// フォーム表示イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void learnDataSearchfrm_VisibleChanged(object sender, EventArgs e)
        {
            // 非表示の時は処理を行わない
            if (!this.Visible) return;

            // コンボボックスを更新する
            updateComboBox();

        }

        /// <summary>
        /// コンボボックスのデータを更新する処理
        /// </summary>
        private void updateComboBox()
        {
            #region データを取得する
            string[] wireNames = Program.DataController.GetWireTypeNames();
            string[] wireSizes = Program.DataController.GetCoreSizeNames();
            string[] wireColor1 = Program.DataController.GetColor1Names();
            string[] wireColor2 = Program.DataController.GetColor2Names();
            #endregion

            // 各コンボボックスをクリアする
            comboBoxClear(true);

            #region 各コンボボックスに項目を登録する
            comboWireType.Items.AddRange(wireNames);
            comboCoreSize.Items.AddRange(wireSizes);
            comboColor1.Items.AddRange(wireColor1);
            comboColor2.Items.AddRange(wireColor2);
            #endregion

            // 選択済みの情報を表示する
            string[] itemkeys = new string[10];
            if (Program.DataController.LearnItemKeysRead(ref itemkeys) == SystemConstants.DCPF_SUCCESS)
            {
                comboWireType.Text = itemkeys[0];
                comboCoreSize.Text = itemkeys[1];
                comboColor1.Text = itemkeys[2];
                comboColor2.Text = itemkeys[3];
                textWireLength.Text = itemkeys[4];
                textStripLength1.Text = itemkeys[5];
                textStripLength2.Text = itemkeys[6];
            }
        }

        /// <summary>
        /// 設定したキーから学習データの値を適用する処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnApply_Click(object sender, EventArgs e)
        {
            string[] itemKeys = new string[10];

            #region 検索キーを登録する
            itemKeys[SystemConstants.DB_GROUP_KEY_WIRENAME] = comboWireType.Text;
            itemKeys[SystemConstants.DB_GROUP_KEY_WIRESIZE] = comboCoreSize.Text;
            itemKeys[SystemConstants.DB_GROUP_KEY_WIRECOLOR1] = comboColor1.Text;
            itemKeys[SystemConstants.DB_GROUP_KEY_WIRECOLOR2] = comboColor2.Text;
            itemKeys[SystemConstants.DB_GROUP_KEY_WIRELENGTH1] = textWireLength.Text;
            itemKeys[SystemConstants.DB_GROUP_KEY_STRIPLENGTH1] = textStripLength1.Text;
            itemKeys[SystemConstants.DB_GROUP_KEY_STRIPLENGTH2] = textStripLength2.Text;

            // 必須項目が入力されていない場合、エラーを表示させて抜ける
            if (itemKeys[SystemConstants.DB_GROUP_KEY_WIRENAME].Length <= 0 || 
                itemKeys[SystemConstants.DB_GROUP_KEY_WIRESIZE].Length <= 0 ||
                itemKeys[SystemConstants.DB_GROUP_KEY_WIRECOLOR1].Length <= 0 ||
                itemKeys[SystemConstants.DB_GROUP_KEY_WIRECOLOR2].Length <= 0 ||
                itemKeys[SystemConstants.DB_GROUP_KEY_WIRELENGTH1].Length <= 0 ||
                itemKeys[SystemConstants.DB_GROUP_KEY_STRIPLENGTH1].Length <= 0 ||
                itemKeys[SystemConstants.DB_GROUP_KEY_STRIPLENGTH2].Length <= 0)
            {
                Utility.ShowErrorMsg(SystemConstants.SYSTEM_MSG034);
                return;
            }
            #endregion

            // 学習データを適用する
            int result = mainfrm.LearnDataLoad(itemKeys);

            // 追加データ処理
            addOptionalWorkData();

            // 学習データをセーブする
            result = mainfrm.LearnDataSave(itemKeys);

            // 学習データのキーを保存する
            result = mainfrm.LearnItemKeysSave(itemKeys);

#if OMOIKANE
            // CFM保存を開始する
            mainfrm.CFMRecord(itemKeys);
#endif
            // フォームを閉じる
            Visible = false;
        }

        /// <summary>
        /// ハーフストリップ、防水の加工モード追加処理
        /// </summary>
        private void addOptionalWorkData()
        {
#if !HCSM40
            Double val = 0.0;

            #region ハーフストリップ1 追加データ処理
            // 圧着モード以外の時に実行
            if (!isCrimpMode(MODE_SIDE1))
            {
                // ストリップ長が入力されており、防水栓が入力されていない時に処理する
                if (textStripLength1.Text.Trim().Length > 0 && comboSealName1.Text.Trim().Length <= 0)
                {
                    // 有効な値が入力されている場合に実行
                    if (availInputData(textSemiStripLength1.Text.Trim(), out val))
                    {
                        Program.DataController.WritePushBtn(SystemConstants.SEMISTRIP1_BTN, SystemConstants.BTN_ON);
                        Program.DataController.WriteWorkData(SystemConstants.STRIP_SEMI_LENGTH1, val);
                    }
                }
            }
            #endregion

            #region ハーフストリップ2 追加データ処理
            // 圧着モード以外の時に実行
            if (!isCrimpMode(MODE_SIDE2))
            {
                // ストリップ長が入力されており、防水栓が入力されていない時に処理する
                if (textStripLength2.Text.Trim().Length > 0 && comboSealName2.Text.Trim().Length <= 0)
                {
                    // 有効な値が入力されている場合に実行
                    if (availInputData(textSemiStripLength2.Text.Trim(), out val))
                    {
                        Program.DataController.WritePushBtn(SystemConstants.SEMISTRIP2_BTN, SystemConstants.BTN_ON);
                        Program.DataController.WriteWorkData(SystemConstants.STRIP_SEMI_LENGTH2, val);
                    }
                }
            }
            #endregion

            #region 防水栓挿入モード1追加データ処理
            // 圧着モード以外の時に実行
            if (!isCrimpMode(MODE_SIDE1))
            {
                // 防水栓名が入力されている時に処理する
                if (comboSealName1.Text.Trim().Length > 0)
                {
                    if (availInputData(textSealInsertLength1.Text.Trim(), out val))
                        Program.DataController.WriteWorkData(SystemConstants.SEAL_INSERT_LENGTH1, val);
                    if (availInputData(textSealInsertBackLength1.Text.Trim(), out val))
                        Program.DataController.WriteWorkData(SystemConstants.SEAL_INSERT_BACK1, val);
                }
            }
            #endregion

            #region 防水栓挿入モード2追加データ処理
            // 圧着モード以外の時に実行
            if (!isCrimpMode(MODE_SIDE2))
            {
                // 防水栓名が入力されている時に処理する
                if (comboSealName2.Text.Trim().Length > 0)
                {
                    if (availInputData(textSealInsertLength2.Text.Trim(), out val))
                        Program.DataController.WriteWorkData(SystemConstants.SEAL_INSERT_LENGTH2, val);
                    if (availInputData(textSealInsertBackLength2.Text.Trim(), out val))
                        Program.DataController.WriteWorkData(SystemConstants.SEAL_INSERT_BACK2, val);
                }
            }
            #endregion
#endif
        }

        /// <summary>
        /// コンボボックスのtextをクリアする
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            comboBoxClear(false);
        }

        /// <summary>
        /// 各コンボボックスをクリアする
        /// </summary>
        /// <param name="WithListItem"></param>
        private void comboBoxClear(bool WithListItem)
        {
            if (WithListItem)
            {
                comboWireType.Items.Clear();
                comboCoreSize.Items.Clear();
                comboColor1.Items.Clear();
                comboColor2.Items.Clear();
            }
            else
            {
                comboWireType.Text = "";
                comboCoreSize.Text = "";
                comboColor1.Text = "";
                comboColor2.Text = "";
                textWireLength.Text = "";
                textStripLength1.Text = "";
                textStripLength2.Text = "";
            }
        }

        /// <summary>
        /// 防水の挿入量入力表示処理
        /// </summary>
        private void changeSealLengthVisible()
        {
#if !HCSM40
            // 防水栓の画面表示を更新する
            bool visibleStatus1 = !(tabctlCrimpMode1.SelectedTab == tabpgCrimp1);
            bool visibleStatus2 = !(tabctlCrimpMode2.SelectedTab == tabpgCrimp2);

            if (pnlSealInsert1.Visible != visibleStatus1) pnlSealInsert1.Visible = visibleStatus1;
            if (pnlSealInsert2.Visible != visibleStatus2) pnlSealInsert2.Visible = visibleStatus2;
#endif
        }

        /// <summary>
        /// 数値入力の欄の数値を数値フォーマットに直す
        /// </summary>
        /// <param name="ctl"></param>
        /// <param name="WorkID"></param>
        private void textFormatCorrect(CustomTextBox ctl, int WorkID)
        {
            string showStr = "";
            double inputtedVal = 0.0;

            // 非表示中、入力中または空欄の時は行わない
            if (!ctl.Visible || ctl.Focused || ctl.Text.Length == 0) return;

            // 数値を変換してみる
            try
            {
                inputtedVal = Convert.ToDouble(ctl.Text);
            }
            catch
            {
                // 空欄にする
                ctl.Text = "";
                return;
            }

            // メモリ割当データの数値フォーマットに変更する
            int result = Program.DataController.ValueChangeToWorkIDFormatStr(SystemConstants.WORKID_TYPE_WORKDATA, WorkID, inputtedVal, ref showStr);
            if (result == SystemConstants.DCPF_SUCCESS)
            {
                if (ctl.Text != showStr) ctl.Text = showStr;
            }
        }

        /// <summary>
        /// 圧着タブを選択されているか確認する処理
        /// </summary>
        /// <param name="Side"></param>
        /// <returns></returns>
        private bool isCrimpMode(int Side)
        {
#if !HCSM40
            switch (Side)
            {
                case MODE_SIDE1:
                    return (tabctlCrimpMode1.SelectedTab == tabpgCrimp1);
                case MODE_SIDE2:
                    return (tabctlCrimpMode2.SelectedTab == tabpgCrimp2);
                default:
                    return false;
            }
#else
            return false;
#endif
        }

        /// <summary>
        /// 入力フォーカスを外す処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void forcusOut_Click(object sender, EventArgs e)
        {
            ActiveControl = null;
        }

        /// <summary>
        /// 入力した値が有効かどうか確認する処理
        /// </summary>
        /// <param name="StrData"></param>
        /// <param name="DoubleData"></param>
        /// <returns>有効な場合は、true</returns>
        private bool availInputData(string StrData, out double DoubleData)
        {
            DoubleData = 0.0;
            try
            {
                DoubleData = Convert.ToDouble(StrData);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// コンボボックスに入力した文字を大文字にする処理
        /// 角コンボボックスでイベント登録
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void upCase_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = char.ToUpper(e.KeyChar);
        }
    }
}
