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
    public partial class learnDataItemEditfrm : Form
    {
        public learnDataItemEditfrm()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            Program.MainForm.AddOwnedForm(this);
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
            }
        }

        /// <summary>
        /// 各テキストボックスをクリアする
        /// </summary>
        private void textBoxClear()
        {
            textWireType.Clear();
            textCoreSize.Clear();
            textColor1.Clear();
            textColor2.Clear();
        }

        /// <summary>
        /// フォームを閉じるときのイベント
        /// フォームを閉じないで非表示にする
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void learnDataItemEditfrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Visible = false;
        }

        /// <summary>
        /// フォーム表示するときのイベント
        /// これだと一回だけしか更新されない
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void learnDataItemEditfrm_Shown(object sender, EventArgs e)
        {
            //comboBoxRegItem();
        }

        /// <summary>
        /// フォームを表示するときのイベント
        /// 都度更新される
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void learnDataItemEditfrm_VisibleChanged(object sender, EventArgs e)
        {
            // 非表示の時は処理を行わない
            if (!this.Visible) return;

            // コンボボックスを更新する
            updateComboBox();
        }

        /// <summary>
        /// コンボボックスの選択されたインデックスが変更されたときのイベント
        /// 選択された電線種類の名前からコードをテキストボックスに表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboWireType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cbx = (ComboBox)sender;
            string name = cbx.Text;
            string code = "";
            
            if (Program.DataController.GetWireTypeCode(name, ref code) == SystemConstants.LIPF_SUCCESS)
            {
                textWireType.Text = code;
            }
        }

        /// <summary>
        /// コンボボックスの選択されたインデックスが変更されたときのイベント
        /// 選択されたコアサイズの名前からコードをテキストボックスに表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboCoreSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cbx = (ComboBox)sender;
            string name = cbx.Text;
            string code = "";

            if (Program.DataController.GetCoreSizeCode(name, ref code) == SystemConstants.LIPF_SUCCESS)
            {
                textCoreSize.Text = code;
            }
        }

        /// <summary>
        /// コンボボックスの選択されたインデックスが変更されたときのイベント
        /// 選択された色１の名前からコードをテキストボックスに表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboColor1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cbx = (ComboBox)sender;
            string name = cbx.Text;
            string code = "";

            if (Program.DataController.GetColo1Code(name, ref code) == SystemConstants.LIPF_SUCCESS)
            {
                textColor1.Text = code;
            }
        }

        /// <summary>
        /// コンボボックスの選択されたインデックスが変更されたときのイベント
        /// 選択された色２の名前からコードをテキストボックスに表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboColor2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cbx = (ComboBox)sender;
            string name = cbx.Text;
            string code = "";

            if (Program.DataController.GetColo2Code(name, ref code) == SystemConstants.LIPF_SUCCESS)
            {
                textColor2.Text = code;
            }
        }

        /// <summary>
        /// 電線種類の登録ボタンのイベント
        /// 入力した名前とコードを登録、あれば更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWireTypeAdd_Click(object sender, EventArgs e)
        {
            string name = comboWireType.Text;
            string code = textWireType.Text;

            if (name == "" || code == "") return;
            if (code.Length != 3)
            {
                textBoxClear();
                return;
            }

            if (Program.DataController.WriteWireType(name.PadRight(5), code) == SystemConstants.LIPF_SUCCESS)
            {
                MessageBox.Show("Success");
            }
            else
            {
                MessageBox.Show("Faleture");
            }
            updateComboBox();
            comboBoxClear(false);
            textBoxClear();
        }

        /// <summary>
        /// 電線種類の削除ボタンのイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWireTypeDel_Click(object sender, EventArgs e)
        {
            string name = comboWireType.Text;
            
            if (name == "") return;
            
            if (Program.DataController.DeleteWireType(name.PadRight(5)) == SystemConstants.LIPF_SUCCESS)
            {
                MessageBox.Show("Success");
            }
            else
            {
                MessageBox.Show("Faleture");
            }
            updateComboBox();
            comboBoxClear(false);
            textBoxClear();
        }

        /// <summary>
        /// コアサイズの登録ボタンのイベント
        /// 入力した名前とコードを登録、あれば更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCoreSizeAdd_Click(object sender, EventArgs e)
        {
            string name = comboCoreSize.Text;
            string code = textCoreSize.Text;

            if (name == "" || code == "") return;
            if (code.Length != 3)
            {
                textBoxClear();
                return;
            }

            if (Program.DataController.WriteCoreSize(name.PadLeft(6), code) == SystemConstants.LIPF_SUCCESS)
            {
                MessageBox.Show("Success");
            }
            else
            {
                MessageBox.Show("Faleture");
            }
            updateComboBox();
            comboBoxClear(false);
            textBoxClear();
        }

        /// <summary>
        /// コアサイズの削除ボタンのイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCoreSizeDel_Click(object sender, EventArgs e)
        {
            string name = comboCoreSize.Text;

            if (name == "") return;

            if (Program.DataController.DeleteCoreSize(name.PadLeft(6)) == SystemConstants.LIPF_SUCCESS)
            {
                MessageBox.Show("Success");
            }
            else
            {
                MessageBox.Show("Faleture");
            }
            updateComboBox();
            comboBoxClear(false);
            textBoxClear();
        }

        /// <summary>
        /// 色１の登録ボタンのイベント
        /// 入力した名前とコードを登録、あれば更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnColor1Add_Click(object sender, EventArgs e)
        {
            string name = comboColor1.Text;
            string code = textColor1.Text;

            if (name == "" || code == "") return;
            if (code.Length != 2)
            {
                textBoxClear();
                return;
            }

            if (Program.DataController.WriteColor1(name.PadRight(2), code) == SystemConstants.LIPF_SUCCESS)
            {
                MessageBox.Show("Success");
            }
            else
            {
                MessageBox.Show("Faleture");
            }
            updateComboBox();
            comboBoxClear(false);
            textBoxClear();
        }

        /// <summary>
        /// 色１の削除ボタンのイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnColor1Del_Click(object sender, EventArgs e)
        {
            string name = comboColor1.Text;

            if (name == "") return;

            if (Program.DataController.DeleteColor1(name.PadRight(2)) == SystemConstants.LIPF_SUCCESS)
            {
                MessageBox.Show("Success");
            }
            else
            {
                MessageBox.Show("Faleture");
            }
            updateComboBox();
            comboBoxClear(false);
            textBoxClear();
        }

        /// <summary>
        /// 色２の登録ボタンのイベント
        /// 入力した名前とコードを登録、あれば更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnColor2Add_Click(object sender, EventArgs e)
        {
            string name = comboColor2.Text;
            string code = textColor2.Text;

            if (name == "" || code == "") return;
            if (code.Length != 2)
            {
                textBoxClear();
                return;
            }

            if (Program.DataController.WriteColor2(name.PadRight(3), code) == SystemConstants.LIPF_SUCCESS)
            {
                MessageBox.Show("Success");
            }
            else
            {
                MessageBox.Show("Faleture");
            }
            updateComboBox();
            comboBoxClear(false);
            textBoxClear();
        }

        /// <summary>
        /// 電線種類の削除ボタンのイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnColor2Del_Click(object sender, EventArgs e)
        {
            string name = comboColor2.Text;

            if (name == "") return;

            if (Program.DataController.DeleteColor2(name.PadRight(3)) == SystemConstants.LIPF_SUCCESS)
            {
                MessageBox.Show("Success");
            }
            else
            {
                MessageBox.Show("Faleture");
            }
            updateComboBox();
            comboBoxClear(false);
            textBoxClear();
        }

    }
}
