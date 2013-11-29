using System;
using System.Windows.Forms;

namespace Alchemist
{
    public partial class bankOperationfrm : Form
    {
        // 初期化設定
        public void Initialize()
        {
            Program.MainForm.AddOwnedForm(this);

            // 機種毎の設定を実行する
            formCustom();

            // テンキー用入力イベント登録
            Program.MainForm.ClickTextBoxEvent(0, SystemConstants.TENKEY_INPUT_ONLY, 0, textCopy2);
        }

        public bankOperationfrm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// バンク選択処理
        /// </summary>
        private void selectBank()
        {
            int selectno = bankOperationView.CurrentRow.Index;

            // selectednoを設定する
            mainfrm.BankNoWrite(selectno);

            // バンクデータをロードする
            int result = mainfrm.BankDataLoad(selectno);

            // バンクデータをセーブする
            result = mainfrm.BankDataSave(selectno);

            // 表示を更新する
            lblNowBankNo2.Text = (selectno + 1).ToString();
            Utility.ShowErrorCode(result);

            // CFMの記録を停止する
            mainfrm.CFMRecordStop(omoikane.SystemConstants.CHANNEL_NO1);
            mainfrm.CFMRecordStop(omoikane.SystemConstants.CHANNEL_NO2);

            // フォームを閉じる
            Visible = false;
        }

        // 選択ボタン押下時の処理
        private void button1_Click(object sender, EventArgs e)
        {
            selectBank();
        }

        // コピーボタン押下時の処理
        private void btnCopy_Click(object sender, EventArgs e)
        {
            string bankComment = "";
            int sourceNo, destNo, currentNo = 0;
            sourceNo = bankOperationView.SelectedRows[0].Index;
            destNo = Int32.Parse(textCopy2.Text) - 1;

            // コピー先のバンクナンバーが0以下、BANK_MAX + 1以上の数字が入力された場合
            if (textCopy2.Text != "")
            {
                // 現在のバンクNoを取得する
                Program.DataController.BankNoRead(ref currentNo);

                // コピー元とコピー先が同じだった場合、又はコピー先が現在の番号の場合、何もしない
                if (sourceNo == destNo || destNo == currentNo)
                {
                    // 処理なし
                    return;
                }

                if (destNo < 0 || destNo >= SystemConstants.BANK_MAX + 1)
                {
                    // 範囲外である旨及び入力範囲のメッセージを表示する
                    Utility.ShowErrorMsg(SystemConstants.SYSTEM_MSG026, lblCopy1.Text, 1, SystemConstants.BANK_MAX);
                }
                else
                {
                    // 選択されているバンクNoを指定されたバンクNoにコピー
                    Program.DataController.CopyBankData(sourceNo, destNo);

                    // コピー先のバンクデータからバンクコメントを取得
                    Program.DataController.BankDataCommentRead(destNo, ref bankComment);

                    // コピー先のバンクコメントの表示を更新
                    bankOperationView.Rows[destNo].Cells[1].Value = bankComment;
                }
            }
            else
            {
                Utility.ShowErrorMsg(SystemConstants.SYSTEM_MSG026, lblCopy1.Text, 1, SystemConstants.BANK_MAX);
            }
        }

        // 閉じるボタン押下時の処理
        private void btnClose_Click(object sender, EventArgs e)
        {
            Visible = false;
        }

        // フォーム閉じる処理
        private void bankOperationfrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Visible = false;
        }


        // Visible状態変更時の処理
        private void bankOperationfrm_VisibleChanged(object sender, EventArgs e)
        {

            string bankComment = "";
            int selectedno = 0;

            if (Visible == true)
            {

                // 現在されているバンクナンバーを取得
                Program.DataController.BankNoRead(ref selectedno);

                // 現在選択されているバンクナンバーを現在バンクNoに設定
                lblNowBankNo2.Text = (selectedno + 1).ToString();

                this.SuspendLayout();

                // グリッドビューの中身を全て空にする
                bankOperationView.RowCount = 0;

                for (int i = 0; i < SystemConstants.BANK_MAX; i++)
                {
                    // i番目のバンクコメントを取得
                    int result = Program.DataController.BankDataCommentRead(i, ref bankComment);

                    // BankDataCommentReadからERR_NO_BANK_DATAが返ってきた場合
                    if (result == SystemConstants.ERR_NO_BANK_DATA)
                    {
                        bankOperationView.Rows.Add(new Object[] { (i + 1).ToString(), "[No Bank Data]" });
                    }
                    // BankDataCommentReadで読み込めた場合
                    else
                    {
                        bankOperationView.Rows.Add(new Object[] { (i + 1).ToString(), bankComment });
                    }
                }

                // 選択を解除する
                bankOperationView.ClearSelection();

                // 行を選択
                bankOperationView.Rows[selectedno].Selected = true;

                this.ResumeLayout();
            }
        }

        // セルをダブルクリックした時の処理（選択処理）
        private void bankOperationView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            selectBank();
        }

        private void lblNowBankNo2_Click(object sender, EventArgs e)
        {

        }
    }
}