using System;
using System.Windows.Forms;

namespace Alchemist
{
    public partial class workDetailItemfrm : Form
    {
        // ���H�f�[�^�O���[�v��`�F�����l�͓d��
        // �p�������e�O���[�v���w�肵�\�����鍀�ڂ�ύX
		protected int Group = SystemConstants.WORK_GROUP_WIRE1;

        /// <summary>
        /// �t�H�[���̃R���X�g���N�^
        /// </summary>
        public workDetailItemfrm()
        {
            InitializeComponent();
        }

		/// <summary>
        /// �������ݒ�
		/// </summary>
		public void Initialize()
		{
			Program.MainForm.AddOwnedForm(this);

            // �Z���̃N���b�N�C�x���g
            Program.MainForm.ClickDataGridViewEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.TENKEY_INPUT_DATA, workDetailItemView);
		}

        /// <summary>
        /// �O���b�h�̍��ڂ�\�����鏈��
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
				// ���̂��擾����(�ǂ���邩�H�j
				name = Utility.GetMessageString(SystemConstants.WORK_MSG, workid);

				// �͈͂��擾����
				dataController.GetWorkDataRangeStr(workid, ref min, ref max);

				// �l���擾����
				dataController.ReadWorkDataStr(workid, ref value);

                //�P�ʂ��擾����B
                Program.DataController.GetWorkDataUnit(workid, ref unit);

				// �l��ݒ肷��
				workDetailItemView.Rows.Add(new Object[] { workid, name, string.Format("{0} <-> {1}", new object[] { min, max }), value, unit });
			}
		}

        /// <summary>
        /// �Z���̒l���X�V���鏈��
        /// ���C���t�H�[���̃X���b�h����Ăяo��
        /// </summary>
		public void refresh() {
			int rowCount = workDetailItemView.Rows.Count;
			string value = "";

			for (int y = 0; y < rowCount; y++) {
				int workid = Int32.Parse(workDetailItemView.Rows[y].Cells[0].Value.ToString());

				// �l���擾����
				Program.DataController.ReadWorkDataStr(workid, ref value);

				var cell = workDetailItemView.Rows[y].Cells[3];

				// �l���ҏW���łȂ���΁A�l��ύX����
				if (!cell.IsInEditMode)
				{
					workDetailItemView.Rows[y].Cells[3].Value = value;
				}				
			}
		}

        /// <summary>
        /// �t�H�[�������[�h���ꂽ�Ƃ��̃C�x���g
        /// �O���b�h�̐ݒ�Ɠ��e�̓ǂݍ���
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
        /// �t�H�[�����\�����ꂽ�Ƃ��̃C�x���g
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void workDetailItemfrm_Shown(object sender, EventArgs e)
        {
			workDetailItemView.CurrentCell = null;	
        }

		/// <summary>
        /// ����{�^���������������̃C�x���g
        /// �t�H�[���̕\��������
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnClose_Click(object sender, EventArgs e)
		{
			Visible = false;
		}

        /// <summary>
        /// �t�H�[�������Ƃ��̃C�x���g
        /// �\�������������ŏI���͂��Ȃ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void workDetailItemfrm_FormClosing(object sender, FormClosingEventArgs e)
		{
			Visible = false;
			e.Cancel = true;
		}

        /// <summary>
        /// �Z�����ҏW���ɂ���Ƃ��̃C�x���g
        /// �ݒ�l�̃Z���̂Ƃ������L�[���͂�L���ɂ���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void workDetailItemView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
		{
			//�\������Ă���R���g���[����DataGridViewTextBoxEditingControl�����ׂ�
			if (e.Control is DataGridViewTextBoxEditingControl)
			{
				DataGridView dgv = (DataGridView)sender;

				//�ҏW�̂��߂ɕ\������Ă���R���g���[�����擾
				DataGridViewTextBoxEditingControl tb =
					(DataGridViewTextBoxEditingControl)e.Control;

				//�C�x���g�n���h�����폜
				tb.KeyPress -=
					new KeyPressEventHandler(dataGridViewTextBox_KeyPress);


				//�Y������񂩒��ׂ�
				if (dgv.CurrentCell.OwningColumn.Name == "Value")
				{
					//KeyPress�C�x���g�n���h����ǉ�
					tb.KeyPress +=
						new KeyPressEventHandler(dataGridViewTextBox_KeyPress);

				}
			}
		}

		/// <summary>
        /// �O���b�h�̃L�[���̓C�x���g
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void dataGridViewTextBox_KeyPress(object sender,
			KeyPressEventArgs e)
		{
			//�����������͂ł��Ȃ��悤�ɂ���
			if ((e.KeyChar < '0' || e.KeyChar > '9') && (e.KeyChar != '.') && (e.KeyChar != '\b')  )
			{
				e.Handled = true;
			}
		}

		/// <summary>
        /// �Z���̓��̓t�H�[�J�X���������Ƃ��̃C�x���g
        /// �Z���̓��̓`�F�b�N�ƒl�̐ݒ������
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

    #region �O���[�v���Ƃ̃N���X��`�F���C���t�H�[������q�t�H�[���Ƃ��ăC���X�^���X�𐶐�

    /// <summary>
    /// ���H�f�[�^�O���[�v���d���̃t�H�[���N���X
    /// WORK_GROUP_WIRE1
	/// </summary>
	public class workDetailItemfrmWIRE1 : workDetailItemfrm
	{
		public workDetailItemfrmWIRE1() : base() {
			Group = SystemConstants.WORK_GROUP_WIRE1;
		}
	}

	/// <summary>
    /// ���H�f�[�^�O���[�v���X�g���b�v�P�̃t�H�[���N���X
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
    /// ���H�f�[�^�O���[�v���X�g���b�v�Q�̃t�H�[���N���X
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