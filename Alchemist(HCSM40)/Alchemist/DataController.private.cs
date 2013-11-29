using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace Alchemist
{
    public partial class DataController : IDisposable
    {
        // XMLファイルアクセス用クラス
        private MemoryAllocationData memalloc = new MemoryAllocationData();
        private CorrectDataStorage correctdata = new CorrectDataStorage();
        private BankDataStorage bankDataStorage = new BankDataStorage();
        private LearnDataStorage learnDataStorage = new LearnDataStorage();
        private LearnItemDataStorage learnItemDataStorage = new LearnItemDataStorage();

        // 通信クラス
        protected MachineConnector machineConnector = null; // 本番用

        // 加工値モード 
        private int workMode;

        // ボタン制御クラス
        private ButtonControl buttonControl;

        //加工値メモリ制御クラス
        private WorkDataMemory workDataMemory;

        // スレッド
        private Thread thread;

        // リスト
        private Dictionary<int, List<int>>[] dupList = new Dictionary<int, List<int>>[4];

        /// <summary>
        /// メモリ割当ファイルから単位を取得する
        /// </summary>
        /// <param name="WorkID"></param>
        /// <param name="type"></param>
        /// <param name="UnitStr"></param>
        /// <returns></returns>
        private int GetUnit(int WorkID, int type, ref string UnitStr)
        {
            MemAllocStruct memAllocStruct = null;

            try
            {
                // メモリ割当データを使用し、WorkIDに対応する共有メモリのアドレスを取得
                memAllocStruct = memalloc.GetEntry(type, WorkID);
            }
            // WorkIDがメモリ割当データに存在していない場合
            catch (MemoryAllocationDataException)
            {
                // エラーとしてERR_NO_WORK_IDを返します
                return SystemConstants.ERR_NO_WORK_ID;
            }

            //単位を取得
            UnitStr = memAllocStruct.Unit;

            return SystemConstants.DCPF_SUCCESS;
        }

        /// <summary>
        /// 共有メモリを読み出す
        /// </summary>
        /// <param name="memAllocStruct"></param>
        /// <param name="WorkData"></param>
        /// <returns></returns>
        private int ReadMemory(MemAllocStruct memAllocStruct, out double WorkData)
        {
            int result = 0;
            int iWorkData = 0;
            int[] num = null;

            // メモリ割当データを使用し、WorkIDに対応する共有メモリのアドレスからMemReadを使用してデータを取得
            //16ビットの場合
            if (memAllocStruct.DoubleWord == 0)
            {
                num = new int[1];
                result = MemRead(memAllocStruct.Address, ref iWorkData);
                num[0] = iWorkData;

            }
            //32ビットの場合
            else
            {
                num = new int[2];
                for (int i = 0; i < 2; i++)
                {
                    result = MemRead(memAllocStruct.Address + i, ref iWorkData);
                    if (result != SystemConstants.MCPF_SUCCESS)
                    {
                        break;
                    }
                    num[i] = iWorkData;
                }
            }

            // 正常に処理が行われた場合
            if (result == SystemConstants.MCPF_SUCCESS)
            {
                // 適切な形に変換し、WorkDataに入れて返す
                WorkData = Utility.ConvertWorkDataToDouble(memAllocStruct, num);

                // DCPF_SUCCESSを返す
                return SystemConstants.DCPF_SUCCESS;
            }
            else
            {
                WorkData = 0;
                return result;
            }
        }

        /// <summary>
        /// 加工値メモリ（設定値）を読み出す
        /// </summary>
        /// <param name="WorkID"></param>
        /// <param name="WorkData"></param>
        /// <returns></returns>
        private int ReadWorkMemory(int WorkID, out double WorkData)
        {
            string value = "";

            //加工値メモリから取得する。
            workDataMemory.Get(SystemConstants.WORKMEM_TYPE_WORKDATA, WorkID, ref value);

            //double型に変換
            WorkData = Convert.ToDouble(value);


            //DCPF_SCUUESSを返す。
            return SystemConstants.DCPF_SUCCESS;
        }

        /// <summary>
        /// 共有メモリへ書き込む
        /// </summary>
        /// <param name="memAllocStruct"></param>
        /// <param name="WorkData"></param>
        /// <returns></returns>
        private int WriteMemory(MemAllocStruct memAllocStruct, double WorkData)
        {
            int result = 0;
            int[] num = null;

            // WorkDataを適切な形(整数値等)に変換
            Utility.ConvertWokdDataToInt(memAllocStruct, WorkData, out num);

            // 通信クラスのMemWriteを使用して書込みます。
            //16ビットの場合
            if (memAllocStruct.DoubleWord == 0)
            {
                result = machineConnector.MemWrite(memAllocStruct.Address, num[0]);
            }
            //32ビットの場合
            else
            {
                for (int i = 0; i < 2; i++)
                {
                    result = machineConnector.MemWrite(memAllocStruct.Address + i, num[i]);

                    if (result != SystemConstants.MCPF_SUCCESS)
                    {
                        break;
                    }
                }
            }
            // 異常の場合
            if (result != SystemConstants.MCPF_SUCCESS)
            {
                return result;
            }

            //機械側に送信
            result = machineConnector.MemSend(memAllocStruct.Address, memAllocStruct.DoubleWord + 1, false);
            if (result == SystemConstants.MCPF_SUCCESS)
            {
                // DCPF_SUCCESSを返します。
                return SystemConstants.DCPF_SUCCESS;
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// 切断長もしくは切断長閾値が変更されたことにより区分の変更があるかをチェックし、変更があればメモリ上の
        /// 電線送り速度・加速時間・切断長補正の値を更新する。
        /// </summary>
        private void FeedAndWireLenCheck(int WorkID, double WorkData, MemAllocStruct memAllocStruct)
        {
            int WireFlagNow = 0;
            int WireFlagOld = 0;

            double dWireLength = 0.0;
            double dTHRESA = 0.0;
            double dTHRESB = 0.0;

            double FeedSpeed = 0.0;
            double FeedAccel = 0.0;
            double WireLengthCorrect = 0.0;

            //前回の切断長を取得する。
            workDataMemory.Get(SystemConstants.WORKMEM_TYPE_WORKDATA, SystemConstants.WIRE_LENGTH1, ref dWireLength);

            //閾値Aを取得する
            workDataMemory.Get(SystemConstants.WORKMEM_TYPE_WORKDATA, SystemConstants.FEED_SPEED_THRES1, ref dTHRESA);

            //閾値Bを取得する
            workDataMemory.Get(SystemConstants.WORKMEM_TYPE_WORKDATA, SystemConstants.FEED_SPEED_THRES2, ref dTHRESB);

            //前回の区分を割り出す
            WireFlagOld = CheckDivision(dWireLength, dTHRESA, dTHRESB);

            //現在の区分を割り出す
            //切断長が変更された場合
            if (WorkID == SystemConstants.WIRE_LENGTH1)
            {
                WireFlagNow = CheckDivision(WorkData, dTHRESA, dTHRESB);
            }
            //閾値Aが変更された場合
            else if (WorkID == SystemConstants.FEED_SPEED_THRES1)
            {
                WireFlagNow = CheckDivision(dWireLength, WorkData, dTHRESB);
            }
            //閾値Bが変更された場合
            else
            {
                WireFlagNow = CheckDivision(dWireLength, dTHRESA, WorkData);
            }

            //前回と状態が異なる場合
            if (WireFlagNow != WireFlagOld)
            {
                switch (WireFlagNow)
                {
                    case 1:
                        //区分A
                        workDataMemory.Get(SystemConstants.WORKMEM_TYPE_WORKDATA, SystemConstants.FEED_SPEED1, ref FeedSpeed);
                        workDataMemory.Get(SystemConstants.WORKMEM_TYPE_WORKDATA, SystemConstants.FEED_ACCEL1, ref FeedAccel);
                        workDataMemory.Get(SystemConstants.WORKMEM_TYPE_WORKDATA, SystemConstants.WIRE_LENGTH_CORRECT1, ref WireLengthCorrect);

                        WriteWorkData(SystemConstants.FEED_SPEED1, FeedSpeed, true, true);
                        WriteWorkData(SystemConstants.FEED_ACCEL1, FeedAccel, true, true);
                        WriteWorkData(SystemConstants.WIRE_LENGTH_CORRECT1, WireLengthCorrect, true, true);
                        break;
                    case 2:
                        //区分B
                        workDataMemory.Get(SystemConstants.WORKMEM_TYPE_WORKDATA, SystemConstants.FEED_SPEED2, ref FeedSpeed);
                        workDataMemory.Get(SystemConstants.WORKMEM_TYPE_WORKDATA, SystemConstants.FEED_ACCEL2, ref FeedAccel);
                        workDataMemory.Get(SystemConstants.WORKMEM_TYPE_WORKDATA, SystemConstants.WIRE_LENGTH_CORRECT2, ref WireLengthCorrect);

                        WriteWorkData(SystemConstants.FEED_SPEED2, FeedSpeed, true, true);
                        WriteWorkData(SystemConstants.FEED_ACCEL2, FeedAccel, true, true);
                        WriteWorkData(SystemConstants.WIRE_LENGTH_CORRECT2, WireLengthCorrect, true, true);
                        break;
                    case 3:
                        //区分C
                        workDataMemory.Get(SystemConstants.WORKMEM_TYPE_WORKDATA, SystemConstants.FEED_SPEED3, ref FeedSpeed);
                        workDataMemory.Get(SystemConstants.WORKMEM_TYPE_WORKDATA, SystemConstants.FEED_ACCEL3, ref FeedAccel);
                        workDataMemory.Get(SystemConstants.WORKMEM_TYPE_WORKDATA, SystemConstants.WIRE_LENGTH_CORRECT3, ref WireLengthCorrect);

                        WriteWorkData(SystemConstants.FEED_SPEED3, FeedSpeed, true, true);
                        WriteWorkData(SystemConstants.FEED_ACCEL3, FeedAccel, true, true);
                        WriteWorkData(SystemConstants.WIRE_LENGTH_CORRECT3, WireLengthCorrect, true, true);
                        break;
                }
            }
        }

        /// <summary>
        /// 切断長から適用するフィード速度を判定する
        /// </summary>
        /// <returns></returns>
        private int CheckDivision(double WireLength, double THRESA, double THRESB)
        {
            //切断長をチェックする
            if (0 < WireLength && WireLength < THRESA)
            {
                return 1;
            }
            else if (THRESA <= WireLength && WireLength < THRESB)
            {
                return 2;
            }
            else
            {
                return 3;
            }
        }

        /// <summary>
        /// WorkIDが選択されているかどうかを判定する
        /// </summary>
        /// <param name="WorkID"></param>
        /// <returns></returns>
        private bool isThresholdSelected(int WorkID)
        {
            double dWireLength = 0;
            double dTHRESA = 0;
            double dTHRESB = 0;

            ReadWorkData(SystemConstants.WIRE_LENGTH1, ref dWireLength);
            ReadWorkData(SystemConstants.FEED_SPEED_THRES1, ref dTHRESA);
            ReadWorkData(SystemConstants.FEED_SPEED_THRES2, ref dTHRESB);

            // 選択されている場合
            int div = CheckDivision(dWireLength, dTHRESA, dTHRESB);
            if (((WorkID == SystemConstants.FEED_SPEED1 || WorkID == SystemConstants.FEED_ACCEL1 || WorkID == SystemConstants.WIRE_LENGTH_CORRECT1) && div == 1) ||
                ((WorkID == SystemConstants.FEED_SPEED2 || WorkID == SystemConstants.FEED_ACCEL2 || WorkID == SystemConstants.WIRE_LENGTH_CORRECT2) && div == 2) ||
                ((WorkID == SystemConstants.FEED_SPEED3 || WorkID == SystemConstants.FEED_ACCEL3 || WorkID == SystemConstants.WIRE_LENGTH_CORRECT3) && div == 3))
            {
                return true;
            }
            // 選択されていない
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 同期処理（データの読み込み）
        /// </summary>
        /// <param name="WorkID"></param>
        /// <param name="WorkData"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private int ReadData(int WorkID, ref double WorkData, int type)
        {
            MemAllocStruct memAllocStruct = null;

            try
            {
                // メモリ割当データを使用し、WorkIDに対応する共有メモリのアドレスを取得
                memAllocStruct = memalloc.GetEntry(type, WorkID);
            }
            // WorkIDがメモリ割当データに存在していない場合
            catch (MemoryAllocationDataException)
            {
                // エラーとしてERR_NO_WORK_IDを返します
                return SystemConstants.ERR_NO_WORK_ID;
            }

            // アドレスが存在する場合
            if (memAllocStruct.address_special == 0)
            {
                return ReadMemory(memAllocStruct, out WorkData);
            }
            // アドレス共有
            else if (memAllocStruct.address_special == 1)
            {
                // 選択されている領域の場合
                if (isThresholdSelected(WorkID))
                {
                    return ReadMemory(memAllocStruct, out WorkData);
                }
                // 選択されていない領域の場合
                else
                {
                    return ReadWorkMemory(WorkID, out WorkData);
                }
            }
            // アドレスが存在しない場合は、加工値メモリからデータを取得する
            else
            {
                return ReadWorkMemory(WorkID, out WorkData);
            }
        }

        /// <summary>
        /// 加工値、補正値を書き込む関数
        /// </summary>
        /// <param name="Type">WorkIDの種別を指定する。</param>
        /// <param name="WorkID">WorkIDを指定する</param>
        /// <param name="WorkData">加工値を指定する</param>
        /// <param name="WriteFileFlag"></param>
        /// <returns></returns>
        private int writeCorrectData(int Type, int WorkID, double WorkData, bool WriteFileFlag)
        {
            int result = 0;
            bool dataFound = false;

            CorrectDataStruct[] correctDataStruct = null;
            MemAllocStruct memAllocStruct = null;

            try
            {
                // メモリ割当データを使用し、WorkIDに対応する共有メモリのアドレスを取得
                memAllocStruct = memalloc.GetEntry(Type, WorkID);
            }
            // WorkIDがメモリ割当データに存在していない場合
            catch (MemoryAllocationDataException)
            {
                // エラーとしてERR_NO_WORK_IDを返します
                return SystemConstants.ERR_NO_WORK_ID;
            }

#if MAINTELOG
            // 現在の値を取得する
            double currentValue = 0;
            ReadMemory(memAllocStruct, out currentValue);
#endif

            // 共有メモリに書き込む
            result = WriteMemory(memAllocStruct, WorkData);
            if (result != SystemConstants.DCPF_SUCCESS)
            {
                return result;
            }

            // WriteFileFlagが立っていれば、ファイルへ書き込む
            if (WriteFileFlag)
            {
                //補正値・タイミング値の構造体を取得する。
                correctdata.GetCorrectData(ref correctDataStruct);

                // 同じアドレスのWORKIDの設定値をまとめて変更する
                foreach (var workid in dupList[Type][memAllocStruct.Address])
                {
                    //システムデータにも反映
                    for (int i = 0; i < correctDataStruct.Length; i++)
                    {
                        //システムデータから該当WorkID検索
                        if (correctDataStruct[i].Type == Type && correctDataStruct[i].ID == workid)
                        {
                            //データに反映
                            correctDataStruct[i].value = WorkData.ToString();
                            dataFound = true;
                            break;
                        }
                    }
                    //システムデータ内に該当WorkIDが存在しない場合はシステムデータに追加する
                    if (!dataFound)
                    {
                        Array.Resize(ref correctDataStruct, correctDataStruct.Length + 1);
                        correctDataStruct[correctDataStruct.Length - 1].Type = Type;
                        correctDataStruct[correctDataStruct.Length - 1].ID = workid;
                        correctDataStruct[correctDataStruct.Length - 1].value = WorkData.ToString(); ;
                    }
                }

                //xmlファイルに書き込み
                try
                {
                    correctdata.WriteCorrectData(correctDataStruct);
                }
                catch (Exception)
                {
                    return SystemConstants.ERR_CORRECT_FILE_ERROR;
                }


            }

#if MAINTELOG
            // 値が更新されていた場合、メンテログにデータを保存する
            if (currentValue != WorkData)
                mainteLogWrite(Type, WorkID, currentValue, WorkData);
#endif

            return result;
        }

        /// <summary>
        /// 共有メモリに書き込む
        /// </summary>
        /// <param name="WorkID"></param>
        /// <param name="WorkData"></param>
        /// <param name="type"></param>
        /// <param name="isCorrectData"></param>
        /// <returns></returns>
        private int writeWorkData(int WorkID, double WorkData, bool initFlag, bool feedFlag)
        {
            int result = SystemConstants.DCPF_SUCCESS;
            MemAllocStruct memAllocStruct = null;

            try
            {
                // メモリ割当データを使用し、WorkIDに対応する共有メモリのアドレスを取得
                memAllocStruct = memalloc.GetEntry(SystemConstants.WORKID_TYPE_WORKDATA, WorkID);
            }
            // WorkIDがメモリ割当データに存在していない場合
            catch (MemoryAllocationDataException)
            {
                // エラーとしてERR_NO_WORK_IDを返します
                return SystemConstants.ERR_NO_WORK_ID;
            }

            // 切断長もしくは、フィードの閾値が変わった場合の処理
            if (initFlag == true && (WorkID == SystemConstants.WIRE_LENGTH1 || WorkID == SystemConstants.FEED_SPEED_THRES1 || WorkID == SystemConstants.FEED_SPEED_THRES2))
            {
                //フィード速度の区分をチェックし、変更があればフィード項目の値を更新する。
                FeedAndWireLenCheck(WorkID, WorkData, memAllocStruct);
            }

            // 通常の場合
            if (memAllocStruct.address_special == 0)
            {
                foreach (var workid in dupList[SystemConstants.WORKID_TYPE_WORKDATA][memAllocStruct.Address])
                {
                    //加工値メモリに反映
                    workDataMemory.Set(SystemConstants.WORKMEM_TYPE_WORKDATA, workid, WorkData.ToString());
                }
            }
            // フィード関連の場合
            else
            {
                //加工値メモリに反映
                workDataMemory.Set(SystemConstants.WORKMEM_TYPE_WORKDATA, WorkID, WorkData.ToString());
            }


            //address_specialが0もしくは、feedFlagがtrueの場合
            //feedFlagはフィード速度の書き込み条件が満たしているかどうかを判別
            if (memAllocStruct.address_special == 0 || feedFlag == true)
            {
                result = WriteMemory(memAllocStruct, WorkData);

                // 正常に処理が行われた場合
                if (result != SystemConstants.DCPF_SUCCESS)
                {
                    return result;
                }
            }
            //アドレスが共有の場合
            else if (memAllocStruct.address_special == 1)
            {
                // フィードが選択されている場合のみメモリにも反映する
                if (isThresholdSelected(WorkID))
                {
                    result = WriteMemory(memAllocStruct, WorkData);

                    // 正常に処理が行われた場合
                    if (result != SystemConstants.DCPF_SUCCESS)
                    {
                        return result;
                    }
                }
            }
            //アドレスが存在しない場合
            else
            {
                /* 何もしない */
            }

            if (initFlag == true)
            {
                // 保存対象の場合バンクに保存する
                if (memAllocStruct.BankStore && (GetWorkMode() == SystemConstants.WORKMODE_BANK))
                {
                    result = BankDataSave();
                    if (result != SystemConstants.DCPF_SUCCESS)
                    {
                        return result;
                    }
                }

                // 学習データに保存する
                if (LearnDataStorage.IsLearnSearchKeyCode(memAllocStruct.learn_key_code) && (GetWorkMode() == SystemConstants.WORKMODE_LEARN))
                {
                    result = LearnDataSave();
                    if (result != SystemConstants.DCPF_SUCCESS)
                    {
                        return result;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 同期処理（データの取得）
        /// </summary>
        /// <param name="type"></param>
        /// <param name="sendFlag"></param>
        /// <returns></returns>
        private int DataGet(int type, bool sendFlag = true)
        {
            int[] groupIDList = null;
            int[] workid = null;
            int sendCount = 0;

            MemAllocStruct memAllocStruct = null;

            //group codeを全取得
            GetMemryDataGroupList(type, SystemConstants.WORKGROUP_ROOT, ref groupIDList);

            for (int i = 0; i < groupIDList.Length; i++)
            {
                //グループごとのworkidを全取得
                GetMemryDataGroupList(type, groupIDList[i], ref workid);

                for (int j = 0; j < workid.Length; j++)
                {
                    //該当workidのメモリ割り当てデータを取得
                    memAllocStruct = memalloc.GetEntry(type, workid[j]);

                    //機械へ送信
                    //DoubleWord == 0　⇒16ビット　⇒1アドレス分
                    //DoubleWord == 1　⇒32ビット　⇒2アドレス分
                    if (sendFlag)
                    {
                        machineConnector.MemGet(memAllocStruct.Address, memAllocStruct.DoubleWord + 1);
                    }

                    sendCount++;
                }
            }

            return sendCount;
        }



        /// <summary>
        /// 同期処理（データの送信）
        /// </summary>
        /// <param name="type"></param>
        private int DataSend(int type, bool sendFlag = true)
        {
            int[] groupIDList = null;
            int[] workid = null;
            int sendCount = 0;

            List<CorrectDataStruct> correctDataStructList = new List<CorrectDataStruct>();
            CorrectDataStruct[] correctDataStruct_old = null;
            CorrectDataStruct[] correctDataStruct_new = null;
            CorrectDataStruct correctDataStruct_tmp;
            MemAllocStruct memAllocStruct = null;

            //システムデータを参照
            correctdata.GetCorrectData(ref correctDataStruct_old);

            //ArrayListにコピー
            correctDataStructList.AddRange(correctDataStruct_old);

            //group codeを全取得
            GetMemryDataGroupList(type, SystemConstants.WORKGROUP_ROOT, ref groupIDList);

            for (int i = 0; i < groupIDList.Length; i++)
            {
                //グループごとのworkidを全取得
                GetMemryDataGroupList(type, groupIDList[i], ref workid);

                for (int j = 0; j < workid.Length; j++)
                {
                    //該当workidのメモリ割り当てデータを取得
                    memAllocStruct = memalloc.GetEntry(type, workid[j]);

                    //CorrectDataにtypeがタイミングでwork[j]が存在するかチェック
                    var nodes = from n in correctDataStruct_old
                                where n.Type == type && n.ID == workid[j]
                                select n;

                    //システムデータに該当するIDの補正値が存在しない場合
                    if (nodes.Count() == 0)
                    {
                        if (sendFlag)
                        {
                            //標準値を書き込み
                            writeCorrectData(type, workid[j], memAllocStruct.DefaultValue, false);
                        }

                        //システムデータに存在しなしworkidの構造体作成
                        correctDataStruct_tmp.Type = type;
                        correctDataStruct_tmp.ID = workid[j];
                        correctDataStruct_tmp.value = memAllocStruct.DefaultValue.ToString();

                        //ArrayListに追加
                        correctDataStructList.Add(correctDataStruct_tmp);
                        sendCount++;
                    }
                    //存在する場合
                    else
                    {
                        if (sendFlag)
                        {
                            // メモリへ書き込み
                            writeCorrectData(type, workid[j], Convert.ToDouble(nodes.First().value), false);
                        }
                        sendCount++;
                    }
                }
            }
            //システムデータに反映
            correctDataStruct_new = new CorrectDataStruct[correctDataStructList.Count];

            for (int k = 0; k < correctDataStructList.Count; k++)
            {
                correctDataStruct_new[k] = correctDataStructList[k];
            }
            //xmlに書き込み
            correctdata.WriteCorrectData(correctDataStruct_new);

            return sendCount;
        }

        // 学習データに、データを追加する
        private void addLearnData(int Type, int WorkID, double Val, ref LearnDataStruct[] LearnData)
        {
            if (LearnData == null) LearnData = new LearnDataStruct[0];

            MemAllocStruct memstruct = memalloc.GetEntry(SystemConstants.WORKID_TYPE_WORKDATA, WorkID);

            // 追加のデータを作成する
            LearnDataStruct newDataStruct = new LearnDataStruct();
            newDataStruct.Type = Type;
            newDataStruct.SearchType = memstruct.learn_key_code;
            newDataStruct.ID = WorkID;
            newDataStruct.value = Val.ToString();

            // 新しい配列を1つ追加する
            int n = LearnData.Length;
            Array.Resize(ref LearnData, n + 1);
            LearnData[n] = newDataStruct;
        }

        // ログデータに、データを追加する
        private void mainteLogWrite(int WorkIDType, int WorkID, double OldValue, double NewValue)
        {
#if MAINTELOG
            int groupCode = (WorkID >> 8);
            int unitCode = Program.MainteLog.GetUnitCode(WorkIDType, groupCode);
            string oldValue = "", newValue = "";

            // 値を文字列に変更する
            int result = ValueChangeToWorkIDFormatStr(WorkIDType, WorkID, OldValue, ref oldValue);
            int result2 = ValueChangeToWorkIDFormatStr(WorkIDType, WorkID, NewValue, ref newValue);

            // 変換が正常に行われた場合、ログに登録を行う
            if (result == SystemConstants.DCPF_SUCCESS && result2 == SystemConstants.DCPF_SUCCESS)
            {
                Program.MainteLog.AddRecord(unitCode, WorkIDType, WorkID, oldValue, newValue);
            }
#endif
        }

        private int exportcurrentData(int GroupType, string filename)
        {
            // 接続していない場合、異常状態を返す
            if (!IsConnect()) return SystemConstants.ERR_ILLEGALSTATUS;

            int ret,ret1;
            // グループ一覧を取得する
            int[] groups = new int[0];
            ret = GetMemryDataGroupList(GroupType, SystemConstants.WORKGROUP_ROOT, ref groups);
            if (ret != SystemConstants.DCPF_SUCCESS) return ret;

            // ファイルに書き出す
            StreamWriter write = new StreamWriter(filename, false);
            try
            {
                // 各グループ一覧毎にデータを取得する
                int[] workIDs;
                string groupName, itemStr, unitStr, valueStr;
                int msgType = Utility.ConvertGroupTypeToMsgType(GroupType);
                foreach (int groupID in groups)
                {
                    // データを取得する
                    workIDs = new int[0];
                    ret = GetMemryDataGroupList(GroupType, groupID, ref workIDs);
                    ret1 = getExclusionItem(GroupType, groupID);

                    // 取得成功かつ、データ数が1以上の場合、データを出力する  ＋　除外項目ではないもの
                    if (ret == SystemConstants.DCPF_SUCCESS && workIDs.Count() > 0 && ret1 == SystemConstants.DCPF_SUCCESS)
                    {
                        // グループ名称を書込む
                        groupName = Utility.GetMessageString(msgType, (groupID << 8));
                        write.WriteLine("\"" + groupName + "\"");

                        // インデックス書込む
                        write.WriteLine("\"Item\",\"Value\",\"Unit\"");

                        // グループの各項目の値を書込む
                        for (int i = 0; i < workIDs.Count(); i++)
                        {
                            itemStr = Utility.GetMessageString(msgType, workIDs[i]);
                            ret = getExportValue(GroupType, workIDs[i], out valueStr, out  unitStr);
                            if (ret == SystemConstants.DCPF_SUCCESS)
                            {
                                write.WriteLine("\"" + itemStr + "\"" + "," + valueStr + "," + "\"" + unitStr + "\"");
                            }
                        }
                    }
                }
            }
            catch
            {
                return SystemConstants.ERR_ILLEGALSTATUS;
            }
            finally
            {
                write.Close();
            }

            return SystemConstants.DCPF_SUCCESS;
        }

        /// <summary>
        /// 除外項目に該当するか返します。
        /// </summary>
        /// <param name="GroupType"></param>
        /// <param name="GroupID"></param>
        /// <returns>除外項目該当しない場合、DCPF_SUCCESSを返す。</returns>
        private int getExclusionItem(int GroupType, int GroupID)
        {
            int ret = SystemConstants.DCPF_SUCCESS;

#if FOR_JN03SDC
 #if FOR_Y
            switch (GroupType)
            {
                case SystemConstants.WORKID_TYPE_CORRECTDATA:
                    switch(GroupID){
                        case SystemConstants.CORR_GROUP_CRIMP1:
                            ret = SystemConstants.ERR_EXCLUSION_ID;
                            break;
                    }
                    break;
                case SystemConstants.WORKID_TYPE_TIMINGDATA:
                    switch(GroupID){
                        case SystemConstants.TIMM_GROUP_SIDE3:
                            ret = SystemConstants.ERR_EXCLUSION_ID;
                            break;
                    }
                    break;
                default:
                    throw new ArgumentException();
            }            
#else
            switch (GroupType)
            {
                case SystemConstants.WORKID_TYPE_CORRECTDATA:
                    switch(GroupID){
                        case SystemConstants.CORR_GROUP_CRIMP1_Y:
                            ret = SystemConstants.ERR_EXCLUSION_ID;
                            break;
                    }
                    break;
                case SystemConstants.WORKID_TYPE_TIMINGDATA:
                    switch(GroupID){
                        case SystemConstants.TIMM_GROUP_SIDE3_Y:
                            ret = SystemConstants.ERR_EXCLUSION_ID;
                            break;
                    }
                    break;
                default:
                    throw new ArgumentException();
            }
 #endif
#else
            ret = SystemConstants.DCPF_SUCCESS;
#endif
            return ret;
        }

        private int getExportValue(int GroupType, int WorkID, out string ValueStr, out string UnitStr)
        {
            ValueStr = "";
            UnitStr = "";
            string valueStr = "", unitStr = "";
            int ret1, ret2;
            switch (GroupType)
            {
                case SystemConstants.WORKID_TYPE_WORKDATA:
                    ret1 = ReadWorkDataStr(WorkID, ref valueStr);
                    ret2 = GetWorkDataUnit(WorkID, ref unitStr);
                    break;
                case SystemConstants.WORKID_TYPE_CORRECTDATA:
                    ret1 = ReadCorrectDataStr(WorkID, ref valueStr);
                    ret2 = GetCorrectDataUnit(WorkID, ref unitStr);
                    break;
                case SystemConstants.WORKID_TYPE_TIMINGDATA:
                    ret1 = ReadTimingDataStr(WorkID, ref valueStr);
                    ret2 = GetTimingDataUnit(WorkID, ref unitStr);
                    break;
                default:
                    throw new ArgumentException();
            }

            // 受け取りに失敗した場合、エラーを返す
            if (ret1 != SystemConstants.DCPF_SUCCESS || ret2 != SystemConstants.DCPF_SUCCESS)
                return SystemConstants.ERR_ILLEGALSTATUS;

            ValueStr = valueStr;
            UnitStr = unitStr;
            return SystemConstants.DCPF_SUCCESS;
        }

    }
}
