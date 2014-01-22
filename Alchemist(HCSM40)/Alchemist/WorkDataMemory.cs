using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Diagnostics;

namespace Alchemist
{
    public class WorkDataMemory
    {
        public struct KeyStruct
        {
            public int Type;
            public int WorkID;

            public override bool Equals(object obj)
            {
                KeyStruct o = (KeyStruct)obj;

                return (Type == o.Type) && (WorkID == o.WorkID);
            }

            public override int GetHashCode()
            {
                return (Type * 0x10000) + WorkID;
            }

            public KeyStruct(int Type, int WorkID)
            {
                this.Type = Type;
                this.WorkID = WorkID;
            }
        }

        // 加工値メモリ構造体
        public struct WorkDataStruct
        {
            public int Type;
            public int WorkID;
            public string Value;

            public WorkDataStruct(int Type, int WorkID, string Value)
            {
                this.Value = Value;
                this.Type = Type;
                this.WorkID = WorkID;
            }
        };

        private MemoryAllocationData memAllocData;
        private ButtonControl buttonControl;

        // 加工値メモリテーブル
        private Dictionary<KeyStruct, WorkDataStruct> map = new Dictionary<KeyStruct, WorkDataStruct>();

        // 初期化
        public void Initialize(MemoryAllocationData memAllocData, ButtonControl buttonControl)
        {
            this.memAllocData = memAllocData;
            this.buttonControl = buttonControl;
        }

        /// <summary>
        /// 指定されたType,WorkIDにValueを設定する
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="WorkID"></param>
        public void Set(int Type, int WorkID, string Value, int SearchType = SystemConstants.DB_GROUP_SEARCH_TYPE_NONE)
        {
            KeyStruct key = new KeyStruct(Type, WorkID);
            WorkDataStruct w = new WorkDataStruct(Type, WorkID, Value);

            map[key] = w;
        }

        /// <summary>
        /// 指定されたType,WorkIDのValueを返す
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="WorkID"></param>
        /// <param name="Value"></param>
        public void Get(int Type, int WorkID, ref string Value)
        {
            KeyStruct key = new KeyStruct(Type, WorkID);

            // キーがある場合は格納されている値を返す。
            if (map.ContainsKey(key))
            {
                Value = map[key].Value;
            }
            // キーがない場合は、0を返す
            else
            {
                Value = "0";
            }
        }

        /// <summary>
        /// 指定されたType,WorkIDのValueを返す
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="WorkID"></param>
        /// <param name="Value"></param>
        public void Get(int Type, int WorkID, ref double Value)
        {
            string tmp = "";

            Get(Type, WorkID, ref tmp);

            Value = Convert.ToDouble(tmp);
        }

        /// <summary>
        /// 指定されたType,WorkIDのValueを返す(int版）
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="WorkID"></param>
        /// <param name="Value"></param>
        public int Get(int Type, int WorkID)
        {
            KeyStruct key = new KeyStruct(Type, WorkID);
            string Value = map[key].Value;

            return Int32.Parse(Value);
        }

        /// <summary>
        /// バンクデータをまるごと加工値メモリに設定する
        /// </summary>
        /// <param name="BankData"></param>
        public int SetArray(BankDataStruct[] BankData)
        {
            int[] groupID = null;
            int[] temp_workID = null;
            List<int> workID = new List<int>();
            MemAllocStruct memAllocStruct = null;
            int[] btnWorkID = new int[0];
            int result = SystemConstants.DCPF_SUCCESS;

            // 加工メモリをクリアする
            map.Clear();

            // バンクデータ配列がnullの場合、長さ０の配列を作る
            if (BankData == null)
            {
                BankData = new BankDataStruct[0];
            }

            // 受け取ったBankDataを全て加工値メモリに書く
            for (int i = 0; i < BankData.Length; i++)
            {
                Set(BankData[i].Type, BankData[i].ID, BankData[i].value);
            }

            // *メモリ割当データに存在するが、BankDataにないものは初期値で補填する
            // 加工値のgroup codeの一覧を取得
            try
            {
                memAllocData.GetMemoryDataGroupList(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.WORKGROUP_ROOT, ref groupID);
            }
            catch (MemoryAllocationDataException)
            {
                return SystemConstants.ERR_WORKID_RANGE;
            }

            // 一覧取得したgroup codeのwork idの一覧を取得
            for (int i = 0; i < groupID.Length; i++)
            {
                try
                {
                    memAllocData.GetMemoryDataGroupList(SystemConstants.WORKID_TYPE_WORKDATA, groupID[i], ref temp_workID);
                }
                catch (MemoryAllocationDataException)
                {
                    return SystemConstants.ERR_WORKID_RANGE;
                }

                for (int j = 0; j < temp_workID.Length; j++)
                {
                    // ArrayListにtemp_workID追加
                    workID.Add(temp_workID[j]);
                }
            }

            // メモリ割当にあって加工値メモリにない値を補う
            for (int i = 0; i < workID.Count; i++)
            {
                // work idのメモリ割当データを取得する
                try
                {
                    // メモリ割当データを使用し、WorkIDに対応する共有メモリのアドレスを取得
                    memAllocStruct = memAllocData.GetEntry(SystemConstants.WORKID_TYPE_WORKDATA, workID[i]);
                }
                // WorkIDがメモリ割当データに存在していない場合
                catch (MemoryAllocationDataException)
                {
                    continue;
                }

                // 加工値メモリに存在しない場合は、デフォルト値で補う
                KeyStruct key = new KeyStruct(SystemConstants.WORKMEM_TYPE_WORKDATA, workID[i]);
                if (!map.ContainsKey(key))
                {
                    // 加工値メモリへ当該不足IDを、メモリ割当データの初期値で追加
                    Set(SystemConstants.WORKMEM_TYPE_WORKDATA, workID[i], memAllocStruct.DefaultValue.ToString());

                    // バンク保存ありの値が欠損していた場合は、ERR_BANK_PARTS_BREKEを返す。
                    if (memAllocStruct.BankStore)
                    {
                        result = SystemConstants.ERR_BANK_PARTS_BREAK;
                    }
                }
            }

            // 全てのボタンIDを取得
            buttonControl.GetAllBtnWorkID(ref btnWorkID);

            // ボタンIDの数だけループする
            for (int i = 0; i < btnWorkID.Length; i++)
            {
                // ボタンIDのバンク保存対象を取得
                ButtonActionStruct btnBankStore = buttonControl.Get(btnWorkID[i]);

                KeyStruct key = new KeyStruct(SystemConstants.WORKMEM_TYPE_WORKBTN, btnWorkID[i]);

                // 加工値メモリに存在しない場合は、デフォルト値で補う
                if (!map.ContainsKey(key))
                {
                    Set(SystemConstants.WORKMEM_TYPE_WORKBTN, btnWorkID[i], btnBankStore.DefaultMode.ToString());

                    // バンク保存ありの値が欠損していた場合は、ERR_BANK_PARTS_BREKEを返す。
                    if (btnBankStore.BankStore == SystemConstants.BTN_BANK_SAVED)
                    {
                        result = SystemConstants.ERR_BANK_PARTS_BREAK;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 加工値メモリから取得したデータをBankDataに入れて返す
        /// </summary>
        /// <param name="BankData"></param>
        public int GetArray(ref BankDataStruct[] BankData)
        {

            int i = 0;
            MemAllocStruct memAllocStruct = null;
            int[] btnWorkID = new int[0];

            // BankDataがnullだった場合構造体を割り当てる
            if (BankData == null)
            {
                BankData = new BankDataStruct[0];
            }

            foreach (WorkDataStruct bankdata in map.Values)
            {
                if (bankdata.Type == SystemConstants.WORKMEM_TYPE_WORKDATA)
                {
                    // work idのメモリ割当データを取得する
                    try
                    {
                        // メモリ割当データを使用し、WorkIDに対応する共有メモリのアドレスを取得
                        memAllocStruct = memAllocData.GetEntry(SystemConstants.WORKID_TYPE_WORKDATA, bankdata.WorkID);
                    }
                    // WorkIDがメモリ割当データに存在していない場合
                    catch (MemoryAllocationDataException)
                    {
                        // データがあるものとして処理する
                        memAllocStruct = new MemAllocStruct();
                        memAllocStruct.BankStore = true;
                    }

                    // bank_storeが保存対象だった場合
                    if (memAllocStruct.BankStore == true)
                    {
                        // 配列の要素を1つ増やす
                        Array.Resize(ref BankData, BankData.Length + 1);

                        // BankDataに加工値メモリの値を設定
                        BankData[i].Type = bankdata.Type;
                        BankData[i].ID = bankdata.WorkID;
                        BankData[i].value = bankdata.Value;

                        i++;
                    }
                }
                else
                {
                    // ボタンIDのバンク保存対象を取得
                    ButtonActionStruct btnBankStore = new ButtonActionStruct();

                    try
                    {
                        btnBankStore = buttonControl.Get(bankdata.WorkID);
                    }
                    catch
                    {
                        btnBankStore.BankStore = SystemConstants.BTN_BANK_UNSAVED;
                    }

                    if (btnBankStore.BankStore == SystemConstants.BTN_BANK_SAVED)
                    {
                        // 配列の要素を1つ増やす
                        Array.Resize(ref BankData, BankData.Length + 1);

                        // BankDataに加工値メモリの値を設定
                        BankData[i].Type = bankdata.Type;
                        BankData[i].ID = bankdata.WorkID;
                        BankData[i].value = bankdata.Value;
                        i++;
                    }

                }
            }

            return SystemConstants.DCPF_SUCCESS;
        }

        /// <summary>
        /// 学習データをまるごと加工値メモリに設定する
        /// </summary>
        /// <param name="LearnData"></param>
        /// <returns></returns>
        public int SetArray(LearnDataStruct[] LearnData)
        {
            List<int> workID = new List<int>();
            int[] groupID = null;
            int[] temp_workID = null;
            MemAllocStruct memAllocStruct = null;
            int result = SystemConstants.MCPF_SUCCESS;

            // 加工メモリをクリアする
            map.Clear();

            // 学習データ配列がnullの場合、長さ0の配列を作る
            if (LearnData == null) LearnData = new LearnDataStruct[0];

            // 受け取った学習データをすべて加工値メモリに書く
            for (int i = 0; i < LearnData.Length; i++)
                Set(LearnData[i].Type, LearnData[i].ID, LearnData[i].value);

            /* メモリ割当データの一覧を取得する */
            // 加工値のgroup code の一覧を取得
            try
            {
                memAllocData.GetMemoryDataGroupList(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.WORKGROUP_ROOT, ref groupID);
            }
            catch (MemoryAllocationDataException)
            {
                return SystemConstants.ERR_WORKID_RANGE;
            }

            // 一覧取得したgroup codeからwork IDの一覧を取得
            for (int i = 0; i < groupID.Length; i++)
            {
                try
                {
                    memAllocData.GetMemoryDataGroupList(SystemConstants.WORKID_TYPE_WORKDATA, groupID[i], ref temp_workID);
                }
                catch (MemoryAllocationDataException)
                {
                    return SystemConstants.ERR_WORKID_RANGE;
                }

                // WorkIDに取得したworkID一覧を追加する
                for (int j = 0; j < temp_workID.Length; j++) workID.Add(temp_workID[j]);
            }

            // メモリ割当データにあって、加工メモリに不足しているデータを補う
            for (int i = 0; i < workID.Count; i++)
            {
                // workID のメモリ割当データを取得する
                try
                {
                    memAllocStruct = memAllocData.GetEntry(SystemConstants.WORKID_TYPE_WORKDATA, workID[i]);
                }
                catch (MemberAccessException)
                {
                    continue;
                }

                // 加工値メモリに存在しない場合は、デフォルト値で補う
                KeyStruct key = new KeyStruct(SystemConstants.WORKMEM_TYPE_WORKDATA, workID[i]);
                if (!map.ContainsKey(key))
                {
                    // 加工値メモリに不足IDを、メモリ割当データの初期値で追加
                    Set(SystemConstants.WORKMEM_TYPE_WORKDATA, workID[i], memAllocStruct.DefaultValue.ToString());

                    // 学習データ割当の値が欠損していた場合は、ERROR_LEARN_PARTS_BREAKを返す。
                    if (LearnDataStorage.IsLearnSearchKeyCode(memAllocStruct.learn_key_code))
                        result = SystemConstants.ERR_LEARN_PARTS_BREAK;
                }
            }

            // 全てのボタンIDを取得
            int[] btnWorkID = new int[0];
            buttonControl.GetAllBtnWorkID(ref btnWorkID);

            // ボタンworkIDの数だけループする
            for (int i = 0; i < btnWorkID.Length; i++)
            {
                // ボタンIDを取得
                ButtonActionStruct btnStore = buttonControl.Get(btnWorkID[i]);
                KeyStruct key = new KeyStruct(SystemConstants.WORKMEM_TYPE_WORKBTN, btnWorkID[i]);

                // 加工値メモリが存在しない場合は、デフォルト値で補う
                if (!map.ContainsKey(key))
                {
                    Set(SystemConstants.WORKMEM_TYPE_WORKBTN, btnWorkID[i], btnStore.DefaultMode.ToString());
                }
            }

            return result;
        }

        /// <summary>
        /// 加工値メモリから取得したデータをLearnDataに入れて返す
        /// </summary>
        /// <param name="LearnData"></param>
        /// <returns></returns>
        public int GetArray(ref LearnDataStruct[] LearnData)
        {
            int i = 0;
            MemAllocStruct memAllocStruct = null;

            // LearnDataがnullの場合、構造体を割当てる
            if (LearnData == null) LearnData = new LearnDataStruct[0];

            foreach (WorkDataStruct learndata in map.Values)
            {
                if (learndata.Type == SystemConstants.WORKMEM_TYPE_WORKDATA)
                {
                    // WorkIDのメモリ割当データを取得する
                    try
                    {
                        memAllocStruct = memAllocData.GetEntry(SystemConstants.WORKID_TYPE_WORKDATA, learndata.WorkID);
                    }
                    catch (MemoryAllocationDataException)
                    {
                        memAllocStruct = new MemAllocStruct();
                        memAllocStruct.learn_key_code = SystemConstants.DB_GROUP_SEARCH_TYPE_NONE;
                    }

                    // 学習データ対象の場合
                    if (LearnDataStorage.IsLearnSearchKeyCode(memAllocStruct.learn_key_code))
                    {
                        // 配列を一つ増やす
                        Array.Resize(ref LearnData, LearnData.Length + 1);

                        // LearnDataに加工値メモリの値を設定
                        LearnData[i].Type = learndata.Type;
                        LearnData[i].SearchType = memAllocStruct.learn_key_code;
                        LearnData[i].ID = learndata.WorkID;
                        LearnData[i].value = learndata.Value;
                        i++;
                    }
                }
                else if (learndata.Type == SystemConstants.WORKMEM_TYPE_WORKBTN)
                {
                    // ボタンIDの学習データ保存対象を取得
                    ButtonActionStruct btnLearnStore = new ButtonActionStruct();

                    try
                    {
                        btnLearnStore = buttonControl.Get(learndata.WorkID);
                    }
                    catch
                    {
                        btnLearnStore.SearchType = SystemConstants.DB_GROUP_SEARCH_TYPE_BTN;
                    }

                    if(btnLearnStore.SearchType != SystemConstants.DB_GROUP_SEARCH_TYPE_BTN)
                    {
                        // 配列の要素を１つ増やす
                        Array.Resize(ref LearnData, LearnData.Length + 1);

                        // LearnDataにデータを追加
                        LearnData[i].Type = learndata.Type;
                        LearnData[i].SearchType = btnLearnStore.SearchType;
                        LearnData[i].ID = learndata.WorkID;
                        LearnData[i].value = learndata.Value;
                        i++;
                    }

/*
                    // 動作処理ボタンの場合
                    if (LearnDataStorage.IsLearnedModeBtn(learndata.WorkID))
                    {
                        // 配列の要素を１つ増やす
                        Array.Resize(ref LearnData, LearnData.Length + 1);

                        // LearnDataにデータを追加
                        LearnData[i].Type = learndata.Type;
                        LearnData[i].SearchType = SystemConstants.DB_GROUP_SEARCH_TYPE_BTN;
                        LearnData[i].ID = learndata.WorkID;
                        LearnData[i].value = learndata.Value;
                        i++;
                    }
 */
                }
            }

            return SystemConstants.DCPF_SUCCESS;
        }
    }
}
