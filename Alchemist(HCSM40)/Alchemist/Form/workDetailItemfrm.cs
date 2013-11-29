using System;
using System.Windows.Forms;

namespace Alchemist
{
    public partial class workDetailItemfrm : Form
    {
        // 加工データグループ定義：初期値は電線
        // 継承した各グループが指定し表示する項目を変更
		protected int Group = SystemConstants.WORK_GROUP_WIRE1;

        /// <summary>
        /// フォームのコンストラクタ
        /// </summary>
        public workDetailItemfrm()
        {
            InitializeComponent();
        }

		/// <summary>
        /// 初期化設定
		/// </summary>
		public void Initialize()
		{
			Program.MainForm.AddOwnedForm(this);

            // セルのクリックイベント
            Program.MainForm.ClickDataGridViewEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.TENKEY_INPUT_DATA, workDetailItemView);
		}

        /// <summary>
        /// グリッドの項目を表示する処理
        /// </summary>
        /// <param name="Group"></param>
		public void insertData(int Group)
		{
			int[] workIDs = null;

			workDetailItemView.Rows.Clear();

			DataController dataController = Program.DataController;
			dataController.GetMemryDataGroupList(SystemConstants.WORKID_TYPE_WORKDATA, Group, ref workIDs);

			string min = "";
			string max = "";
			string value = "";
			string name = "";
            string unit = "";

			foreach (var workid in workIDs)
			{
				// 名称を取得する(どうやるか？）
				name = Utility.GetMessageString(SystemConstants.WORK_MSG, workid);

				// 範囲を取得する
				dataController.GetWorkDataRangeStr(workid, ref min, ref max);

				// 値を取得する
				dataController.ReadWorkDataStr(workid, ref value);

                //単位を取得する。
                Program.DataController.GetWorkDataUnit(workid, ref unit);

				// 値を設定する
				workDetailItemView.Rows.Add(new Object[] { workid, name, string.Format("{0} <-> {1}", new object[] { min, max }), value, unit });
			}
		}

        /// <summary>
        /// セルの値を更新する処理
        /// メインフォームのスレッドから呼び出し
        /// </summary>
		public void refresh() {
			int rowCount = workDetailItemView.Rows.Count;
			string value = "";

			for (int y = 0; y < rowCount; y++) {
				int workid = Int32.Parse(workDetailItemView.Rows[y].Cells[0].Value.ToString());

				// 値を取得する
				Program.DataController.ReadWorkDataStr(workid, ref value);

				var cell = workDetailItemView.Rows[y].Cells[3];

				// 値が編集中でなければ、値を変更する
				if (!cell.IsInEditMode)
				{
					workDetailItemView.Rows[y].Cells[3].Value = value;
				}				
			}
		}

        /// <summary>
        /// フォームがロードされたときのイベント
        /// グリッドの設定と内容の読み込み
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void workDetailItemfrm_Load(object sender, EventArgs e)
        {
			for (int i = 0; i < workDetailItemView.Columns.Count; i++) {
				workDetailItemView.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
			}

			insertData(Group);
        }

        /// <summary>
        /// フォームが表示されたときのイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void workDetailItemfrm_Shown(object sender, EventArgs e)
        {
			workDetailItemView.CurrentCell = null;	
        }

		/// <summary>
        /// 閉じるボタンを押下した時のイベント
        /// フォームの表示を消す
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnClose_Click(object sender, EventArgs e)
		{
			Visible = false;
		}

        /// <summary>
        /// フォームを閉じるときのイベント
        /// 表示を消すだけで終了はしない
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void workDetailItemfrm_FormClosing(object sender, FormClosingEventArgs e)
		{
			Visible = false;
			e.Cancel = true;
		}

        /// <summary>
        /// セルが編集中にあるときのイベント
        /// 設定値のセルのときだけキー入力を有効にする
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void workDetailItemView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
		{
			//表示されているコントロールがDataGridViewTextBoxEditingControlか調べる
			if (e.Control is DataGridViewTextBoxEditingControl)
			{
				DataGridView dgv = (DataGridView)sender;

				//編集のために表示されているコントロールを取得
				DataGridViewTextBoxEditingControl tb =
					(DataGridViewTextBoxEditingControl)e.Control;

				//イベントハンドラを削除
				tb.KeyPress -=
					new KeyPressEventHandler(dataGridViewTextBox_KeyPress);


				//該当する列か調べる
				if (dgv.CurrentCell.OwningColumn.Name == "Value")
				{
					//KeyPressイベントハンドラを追加
					tb.KeyPress +=
						new KeyPressEventHandler(dataGridViewTextBox_KeyPress);

				}
			}
		}

		/// <summary>
        /// グリッドのキー入力イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void dataGridViewTextBox_KeyPress(object sender,
			KeyPressEventArgs e)
		{
			//数字しか入力できないようにする
			if ((e.KeyChar < '0' || e.KeyChar > '9') && (e.KeyChar != '.') && (e.KeyChar != '\b')  )
			{
				e.Handled = true;
			}
		}

		/// <summary>
        /// セルの入力フォーカスを失ったときのイベント
        /// セルの入力チェックと値の設定をする
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void workDetailItemView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
		{
            if (Program.SystemData.tachpanel == true) return;
            if (e.ColumnIndex != 3) return;

            DataGridView view = workDetailItemView;
            int workidtype = SystemConstants.WORKID_TYPE_WORKDATA;
            int workid = Int32.Parse(view.Rows[e.RowIndex].Cells[0].Value.ToString());
            object value = e.FormattedValue;

            Program.MainForm.EnterDataGridView(workidtype, workid, value);
		}
    }

    #region グループごとのクラス定義：メインフォームから子フォームとしてインスタンスを生成

    /// <summary>
    /// 加工データグループが電線のフォームクラス
    /// WORK_GROUP_WIRE1
	/// </summary>
	public class workDetailItemfrmWIRE1 : workDetailItemfrm
	{
		public workDetailItemfrmWIRE1() : base() {
			Group = SystemConstants.WORK_GROUP_WIRE1;
		}
	}

	/// <summary>
    /// 加工データグループがストリップ１のフォームクラス
    /// WORK_GROUP_STRIP1
	/// </summary>
	public class workDetailItemfrmSTRIP1 : workDetailItemfrm
	{
		public workDetailItemfrmSTRIP1()
			: base()
		{
			Group = SystemConstants.WORK_GROUP_STRIP1;
		}
	}

	/// <summary>
    /// 加工データグループがストリップ２のフォームクラス
    /// WORK_GROUP_STRIP2
	/// </summary>
	public class workDetailItemfrmSTRIP2 : workDetailItemfrm
	{
		public workDetailItemfrmSTRIP2()
			: base()
		{
			Group = SystemConstants.WORK_GROUP_STRIP2;
		}
	}

    #endregion

}