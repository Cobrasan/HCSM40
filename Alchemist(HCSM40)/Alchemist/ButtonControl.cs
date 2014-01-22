using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Alchemist
{
    // チェック関数デリゲート
    public delegate bool BoolDelegate(ButtonActionStruct actionStruct, int WorkID, int Status);

    // WritePushBtn用の例外クラス
    public class WritePushBtnException : Exception
    {
        public int ErrorCode
        {
            get;
            set;
        }

        public WritePushBtnException(int ErrorCode)
        {
            this.ErrorCode = ErrorCode;
        }

        public static void ThrowException(int ErrorCode)
        {
            throw new WritePushBtnException(ErrorCode);
        }
    }

    // ボタン動作構造体
    public struct ButtonActionStruct
    {
        public int WorkID;
        public BoolDelegate ActionDelegate;
        public BoolDelegate OnOffCheck;
        public BoolDelegate OffOnCheck;
        public int Digit;
        public BoolDelegate RelatedDelegate;
        public int Address;
        public int BankStore;
        public int SearchType;
        public int DefaultMode;
    };

    // ボタンの挙動をコントロールするクラス
    public partial class ButtonControl
    {
        private DataController dataController;
        private MachineConnector machineConnector;
        private WorkDataMemory workDataMemory;

        public ButtonControl(DataController dataController, MachineConnector machineConnector, WorkDataMemory workDataMemory)
        {
            this.dataController = dataController;
            this.machineConnector = machineConnector;
            this.workDataMemory = workDataMemory;

            // ボタンコントロールを追加する
            addButtonControl();

        }

        // ボタン動作設定table
        private Dictionary<int, ButtonActionStruct> map = new Dictionary<int, ButtonActionStruct>();

        // ボタン動作設定登録関数
        private void Add(int WorkID, BoolDelegate Action, int Digit, BoolDelegate OnOffCheck,
            BoolDelegate OffOnCheck, BoolDelegate RelatedDelegate, int Address, int BankStore, 
            int SearchType = SystemConstants.DB_GROUP_SEARCH_TYPE_BTN, int DefaultMode = SystemConstants.BTN_OFF)
        {
            ButtonActionStruct actionStruct = new ButtonActionStruct();
            actionStruct.WorkID = WorkID;
            actionStruct.ActionDelegate = Action;
            actionStruct.OnOffCheck = OnOffCheck;
            actionStruct.OffOnCheck = OffOnCheck;
            actionStruct.RelatedDelegate = RelatedDelegate;
            actionStruct.Digit = Digit;
            actionStruct.Address = Address;
            actionStruct.BankStore = BankStore;
            actionStruct.SearchType = SearchType;
            actionStruct.DefaultMode = DefaultMode;

            map.Add(
                WorkID,
                actionStruct
            );
        }

        /// <summary>
        /// ボタン動作の構造体を取得します。
        /// </summary>
        /// <param name="WorkID"></param>
        /// <returns></returns>
        public ButtonActionStruct Get(int WorkID)
        {
            return map[WorkID];
        }

        /// <summary>
        /// ボタン動作のWorkIDをすべて取得します。
        /// </summary>
        /// <param name="WorkID"></param>
        public void GetAllBtnWorkID(ref int[] BtnWorkID)
        {
            int i = 0;

            foreach (int btnworkID in map.Keys)
            {
                // 配列の要素を1つ増やす
                Array.Resize(ref BtnWorkID, BtnWorkID.Length + 1);

                // ボタンのworkIDを追加していく
                BtnWorkID[i] = btnworkID;
                i++;
            }
        }

        /// <summary>
        /// WorkIDのアドレスのBitPosで指定されたビット位置を取得し、
        /// BitMemValに入れて返します。
        /// BitMemValは0か0以外が返されます。
        /// 0以外が返された場合、BitPosビット目が1であることを示しています。
        /// </summary>
        /// <param name="WorkID"></param>
        /// <param name="BitPos"></param>
        /// <param name="BitMemVal"></param>
        public void bitRead(int WorkID, int BitPos, ref int BitMemVal)
        {
            // WorkIDのアドレスを取得
            ButtonActionStruct action = Get(WorkID);

            // WorkIDのアドレスの値を取得
            int result = machineConnector.MemRead(action.Address, ref BitMemVal);

            // BitPosビット目を取得
            BitMemVal = BitMemVal & (1 << (BitPos));
        }

        /// <summary>
        /// ボタンの設定値を取得する
        /// </summary>
        /// <param name="WorkID"></param>
        /// <returns></returns>
        private int GetBtnStatus(int WorkID)
        {
            int BitMemVal = 0;

            // WorkIDのアドレスを取得
            ButtonActionStruct action = Get(WorkID);

            // ビット操作の場合
            if (action.ActionDelegate == bitControlAction)
            {
                // WorkIDのアドレスの値を取得
                int result = machineConnector.MemReadPC(action.Address, ref BitMemVal);

                // BitPosビット目を取得
                BitMemVal = BitMemVal & (1 << (action.Digit));

                if (BitMemVal == 0)
                {
                    return SystemConstants.BTN_OFF;
                }
                else
                {
                    return SystemConstants.BTN_ON;
                }
            }
            // 値設定の場合
            else if (action.ActionDelegate == valueSetAction)
            {
                // データを読む
                machineConnector.MemReadPC(action.Address, ref BitMemVal);

                // 読んだ値＝設定値の場合ON
                if (BitMemVal == action.Digit)
                {
                    return SystemConstants.BTN_ON;
                }
                // 読んだ値！＝設定値の場合OFF
                else
                {
                    return SystemConstants.BTN_OFF;
                }
            }

            return SystemConstants.BTN_OFF;
        }

        /// <summary>
        /// 操作ボタンIDの状態を、BtnStatusに入れて返します。
        /// </summary>
        /// <param name="BtnID"></param>
        /// <param name="BtnStatus"></param>
        /// <returns></returns>
        public int ReadPushBtn(int BtnID, ref int BtnStatus)
        {
            ButtonActionStruct actionStruct = Get(BtnID);
            int BitMem = 0;

            // ビット操作
            if (actionStruct.ActionDelegate == bitControlAction)
            {

                // ビットを読む
                bitRead(BtnID, actionStruct.Digit, ref BitMem);

                // 値を変換する
                if (BitMem == 0)
                {
                    BtnStatus = SystemConstants.BTN_OFF;
                }
                else
                {
                    BtnStatus = SystemConstants.BTN_ON;
                }
            }
            // 値セットの場合
            else if (actionStruct.ActionDelegate == valueSetAction)
            {
                // データを読む
                machineConnector.MemRead(actionStruct.Address, ref BitMem);

                // 読んだ値＝設定値の場合ON
                if (BitMem == actionStruct.Digit)
                {
                    BtnStatus = SystemConstants.BTN_ON;
                }
                // 読んだ値！＝設定値の場合OFF
                else
                {
                    BtnStatus = SystemConstants.BTN_OFF;
                }
            }

            return SystemConstants.DCPF_SUCCESS;
        }

        /// <summary>
        /// 操作ボタンID(BtnID)がボタンStatusに変化した時に対応する
        /// 処理を行います。
        /// </summary>
        /// <param name="BtnID">BTNのID</param>
        /// <param name="BtnStatus">BTN_PUSHの時、ボタンを押された事を意味します。
        /// BTN_OFFの時、OFFの状態にする事を意味します。
        /// BTN_ONの時、ONの状態にする事を意味します。</param>
        /// <param name="execRelated">関連動作を実行する場合は、TRUEにします。</param>
        /// <returns></returns>
        public int WritePushBtn(int BtnID, int BtnStatus, bool execRelated = true, bool initFlag = true)
        {
            int ret = 0;

            ButtonActionStruct actionStruct;
            try
            {
                actionStruct = Get(BtnID);
            }
            catch
            {
                return SystemConstants.ERR_NO_WORK_ID;
            }

            //ボタンの状態を取得する
            int currentBtnStatus = 0;

            // 加工値メモリからボタンの状態を取得する
            currentBtnStatus = GetBtnStatus(BtnID);

            // BTN_PUSHの場合、BtnStatusを逆にする
            if (BtnStatus == SystemConstants.BTN_PUSH)
            {
                if (currentBtnStatus == SystemConstants.BTN_ON)
                {
                    BtnStatus = SystemConstants.BTN_OFF;
                }
                else
                {
                    BtnStatus = SystemConstants.BTN_ON;
                }
            }

            // 状態の変化がない場合は、何もしない
            if (initFlag == true)
            {
                if (((currentBtnStatus == SystemConstants.BTN_OFF) && (BtnStatus == SystemConstants.BTN_OFF)) ||
                    ((currentBtnStatus == SystemConstants.BTN_ON) && (BtnStatus == SystemConstants.BTN_ON)))
                {
                    return SystemConstants.DCPF_SUCCESS;
                }
            }

            try
            {
                // 関連動作を実行が指定されていれば関連動作を実行する
                if (execRelated)
                {
                    // OFF->ONの場合
                    if ((currentBtnStatus == SystemConstants.BTN_OFF) &&
                        (BtnStatus == SystemConstants.BTN_ON))
                    {

                        // チェックNGの場合は、終了
                        if (actionStruct.OffOnCheck != null)
                        {
                            if (actionStruct.OffOnCheck(actionStruct, BtnID, BtnStatus) == false)
                            {
                                return SystemConstants.DCPF_SUCCESS;
                            }
                        }
                    }
                    // ON->OFFの場合
                    else
                    {
                        if (actionStruct.OnOffCheck != null)
                        {
                            // チェックNGの場合は、終了
                            if (actionStruct.OnOffCheck(actionStruct, BtnID, BtnStatus) == false)
                            {
                                return SystemConstants.DCPF_SUCCESS;
                            }
                        }
                    }
                    if (actionStruct.RelatedDelegate != null)
                    {
                        if (actionStruct.RelatedDelegate(actionStruct, BtnID, BtnStatus) == false)
                        {
                            return SystemConstants.DCPF_SUCCESS;
                        }
                    }

                    // 自分の動作を実行
                    actionStruct.ActionDelegate(actionStruct, BtnID, BtnStatus);


                }
                else
                {
                    // 自分の動作を実行
                    actionStruct.ActionDelegate(actionStruct, BtnID, BtnStatus);
                }
            }
            // WritePushBtnで例外が発生した場合、エラーコードに変換する
            catch (WritePushBtnException ex)
            {
                // 関連動作の場合
                if (execRelated == false)
                {
                    throw;
                }
                // 関連動作ではない場合
                else
                {
                    ret = ex.ErrorCode;
                    return ret;
                }
            }

            // initFlagがtrueの場合は、強制的にメモリを送る
            machineConnector.MemSend(actionStruct.Address, 1, initFlag);

            // 初期化フラグが立っている場合のみファイルにセーブする。
            if (initFlag)
            {
                // バンク保存対象の場合保存を行う
                if (actionStruct.BankStore == SystemConstants.BTN_BANK_SAVED && Program.DataController.GetWorkMode() == SystemConstants.WORKMODE_BANK)
                {
                    ret = dataController.BankDataSave();
                    if (ret != SystemConstants.DCPF_SUCCESS)
                    {
                        // 関連動作の場合
                        if (!execRelated)
                        {
                            WritePushBtnException.ThrowException(ret);
                        }
                        else
                        {
                            return ret;
                        }
                    }
                }

                // 学習データに保存する
                if (actionStruct.SearchType != SystemConstants.DB_GROUP_SEARCH_TYPE_BTN && (Program.DataController.GetWorkMode() == SystemConstants.WORKMODE_LEARN))
                {
                    ret = dataController.LearnDataSave();
                    if (ret != SystemConstants.DCPF_SUCCESS)
                    {
                        // 関連動作の場合
                        if (!execRelated)
                        {
                            WritePushBtnException.ThrowException(ret);
                        }
                        else
                        {
                            return ret;
                        }
                    }
                }

            }

            return SystemConstants.DCPF_SUCCESS;
        }

        /// <summary>
        /// ビット操作をします。
        /// On_offが0だった場合、WorkIDのアドレスのBitPosで指定されたビット位置を反転します。
        /// On_offが1だった場合、WorkIDのアドレスのBitPosで指定されたビット位置を1にします。
        /// On_offが2だった場合、WorkIDのアドレスのBitPosで指定されたビット位置を0にします。
        /// </summary>
        /// <param name="WorkID"></param>
        /// <param name="BitPos"></param>
        /// <param name="On_off"></param>
        private void bitControl(int WorkID, int BitPos, int On_off)
        {
            int memVal = 0;
            // WorkIDのアドレスを取得
            ButtonActionStruct action = Get(WorkID);

            // WorkIDのvalueを取得する。
            machineConnector.MemReadPC(action.Address, ref memVal);

            // On_offがPushだった場合
            if (On_off == SystemConstants.BTN_PUSH)
            {
                // ON->OFF、OFF->ONにする
                memVal = memVal ^ (1 << BitPos);
            }
            // On_offがOnだった場合
            else if (On_off == SystemConstants.BTN_ON)
            {
                // BitPosビット目を1にします
                memVal = memVal | (1 << BitPos);
            }
            // On_offがOffだった場合
            else if (On_off == SystemConstants.BTN_OFF)
            {
                // BitPosビット目を0にします
                memVal = memVal & ~(1 << BitPos);
            }

            //加工値メモリに保存する。
            workDataMemory.Set(SystemConstants.WORKMEM_TYPE_WORKBTN, WorkID, On_off.ToString());

            // ビット演算結果をMemWriteする
            machineConnector.MemWrite(action.Address, memVal);
        }

        // ボタンの状態（PC側）を取得する
        private void ReadPushBtnPC(int BtnID, ref int BtnStatus)
        {
            BtnStatus = GetBtnStatus(BtnID);
        }

        // ストリップのOFF->ON, ON->OFFチェック
        public bool checkStripBtn(ButtonActionStruct actionStruct, int WorkID, int BtnStatus)
        {
            // 加工モードを取得
            int workMode = Program.DataController.GetWorkMode();

            if (workMode == SystemConstants.WORKMODE_BANK)
                return true;
            else
                return false;
        }

        // 圧着のOFF->ONチェック
        public bool checkCrimpOffOnBtn(ButtonActionStruct actionStruct, int WorkID, int BtnStatus)
        {
            // 加工モードを取得
            int workMode = Program.DataController.GetWorkMode();

            if (workMode == SystemConstants.WORKMODE_BANK)
                return true;
            else
                return false;

        }


        // 圧着1のOFF-> ONチェック
        public bool checkCrimp1_Btn(ButtonActionStruct actionStruct, int WorkID, int BtnStatus)
        {
            // 加工が学習モードの時は動作不可
            int workMode = Program.DataController.GetWorkMode();
            if (workMode == SystemConstants.WORKMODE_LEARN) return false;

            int statusStrip1 = 0;
            int statusSemiStrip1 = 0;

            // ストリップ1の値を取得
            ReadPushBtnPC(SystemConstants.STRIP1_BTN, ref statusStrip1);

            // ハーフストリップ1の値を取得
            ReadPushBtnPC(SystemConstants.SEMISTRIP1_BTN, ref statusSemiStrip1);

            //ストリップ1がONかつ、ハーフストリップ1がOFFだった場合
            if (statusStrip1 == SystemConstants.BTN_ON && statusSemiStrip1 == SystemConstants.BTN_OFF)
            {
                // trueを返す。
                return true;
            }
            else
            {
                // falseを返す。
                return false;
            }
        }

        // 防水1のOFF-> ONチェック
        public bool checkSeal1_Btn(ButtonActionStruct actionStruct, int WorkID, int BtnStatus)
        {
            int statusSemiStrip1 = 0;

            // ハーフストリップ1の値を取得
            ReadPushBtnPC(SystemConstants.SEMISTRIP1_BTN, ref statusSemiStrip1);

            // ハーフストリップ1がOFFだった場合
            if (statusSemiStrip1 == SystemConstants.BTN_OFF)
            {
                // trueを返す
                return true;
            }
            else
            {
                // falseを返す
                return false;
            }
        }

        // ハーフストリップ1のOFF-> ONチェック
        public bool checkSemiStrip1_Btn(ButtonActionStruct actionStruct, int WorkID, int BtnStatus)
        {
            int statusStrip1 = 0;
            //int statusCrimp1 = 0;
            //int statusSeal1 = 0;

            // ストリップ1の値を取得
            ReadPushBtnPC(SystemConstants.STRIP1_BTN, ref statusStrip1);

            // 圧着1の値を取得
            //ReadPushBtnPC(SystemConstants.CRIMP1_BTN, ref statusCrimp1);

            // 防水1の値を取得
            //ReadPushBtnPC(SystemConstants.SEAL1_BTN, ref statusSeal1);

            //ストリップ1がONかつ、圧着1がOFFだった場合
            if (statusStrip1 == SystemConstants.BTN_ON) //&& statusCrimp1 == SystemConstants.BTN_OFF && statusSeal1 == SystemConstants.BTN_OFF)
            {
                // trueを返す。
                return true;
            }
            else
            {
                // falseを返す。
                return false;
            }
        }

        // 圧着2のOFF-> ONチェック
        public bool checkCrimp2_Btn(ButtonActionStruct actionStruct, int WorkID, int BtnStatus)
        {
            // 加工が学習モードの時は動作不可
            int workMode = Program.DataController.GetWorkMode();
            if (workMode == SystemConstants.WORKMODE_LEARN) return false;

            int statusStrip2 = 0;
            int statusSemiStrip2 = 0;

            // ストリップ2の値を取得
            ReadPushBtnPC(SystemConstants.STRIP2_BTN, ref statusStrip2);

            // ハーフストリップ2の値を取得
            ReadPushBtnPC(SystemConstants.SEMISTRIP2_BTN, ref statusSemiStrip2);

            //ストリップ2がONかつ、ハーフストリップ2がOFFだった場合
            if (statusStrip2 == SystemConstants.BTN_ON && statusSemiStrip2 == SystemConstants.BTN_OFF)
            {
                // trueを返す。
                return true;
            }
            else
            {
                // falseを返す。
                return false;
            }
        }

        // 防水2のOFF-> ONチェック
        public bool checkSeal2_Btn(ButtonActionStruct actionStruct, int WorkID, int BtnStatus)
        {
            int statusSemiStrip2 = 0;

            // ハーフストリップ2の値を取得
            ReadPushBtnPC(SystemConstants.SEMISTRIP2_BTN, ref statusSemiStrip2);

            //ハーフストリップ2がOFFだった場合
            if (statusSemiStrip2 == SystemConstants.BTN_OFF)
            {
                // trueを返す
                return true;
            }
            else
            {
                // falseを返す
                return false;
            }
        }

        // ハーフストリップ2のOFF-> ONチェック
        public bool checkSemiStrip2_Btn(ButtonActionStruct actionStruct, int WorkID, int BtnStatus)
        {
            int statusStrip2 = 0;
            //int statusCrimp2 = 0;
            //int statusSeal2 = 0;

            // ストリップ2の値を取得
            ReadPushBtnPC(SystemConstants.STRIP2_BTN, ref statusStrip2);

            // 圧着2の値を取得
            //ReadPushBtnPC(SystemConstants.CRIMP2_BTN, ref statusCrimp2);

            // 防水2の値を取得
            //ReadPushBtnPC(SystemConstants.SEAL2_BTN, ref statusSeal2);

            //ストリップ1がONかつ、圧着2がOFFだった場合
            if (statusStrip2 == SystemConstants.BTN_ON) //&& statusCrimp2 == SystemConstants.BTN_OFF && statusSeal2 == SystemConstants.BTN_OFF)
            {
                // trueを返す。
                return true;
            }
            else
            {
                // falseを返す。
                return false;
            }
        }

        // 圧着機1スイッチのOFF-> ONチェック
        public bool checkCrimp1_Sw_Btn(ButtonActionStruct actionStruct, int WorkID, int BtnStatus)
        {
#if HCSM40
            return false;
#else
            int statusCrimp1 = 0;

            // 圧着1の値を取得
            ReadPushBtnPC(SystemConstants.CRIMP1_BTN, ref statusCrimp1);

            // 圧着1がONだった場合
            if (statusCrimp1 == SystemConstants.BTN_ON)
            {
                // trueを返す。
                return true;
            }
            else
            {
                // falseを返す。
                return false;
            }
#endif
        }

        // 圧着機2スイッチのOFF-> ONチェック
        public bool checkCrimp2_Sw_Btn(ButtonActionStruct actionStruct, int WorkID, int BtnStatus)
        {
#if HCSM40
            return false;
#else
            int statusCrimp2 = 0;

            // 圧着2の値を取得
            ReadPushBtnPC(SystemConstants.CRIMP2_BTN, ref statusCrimp2);

            // 圧着2がONだった場合
            if (statusCrimp2 == SystemConstants.BTN_ON)
            {
                // trueを返す。
                return true;
            }
            else
            {
                // falseを返す。
                return false;
            }
#endif
        }

        // インチング1ボタン及びインチング2ボタンのOFF-> ONチェック
        public bool checkInching1and2_Btn(ButtonActionStruct actionStruct, int WorkID, int BtnStatus)
        {
#if HCSM40
            return false;
#else
            int statusJog = 0;
            int statusPressSw = 0;

            // JOGの値を取得
            ReadPushBtnPC(SystemConstants.JOG_BTN, ref statusJog);

            // PRESS SWの値取得
            if (WorkID == SystemConstants.INCHING1_BTN)
                ReadPushBtnPC(SystemConstants.CRIMP1_SW_BTN, ref statusPressSw);
            else
                ReadPushBtnPC(SystemConstants.CRIMP2_SW_BTN, ref statusPressSw);

            // JOGがONだった場合
            /*if (statusJog == SystemConstants.BTN_ON)
            {
                // trueを返す。
                return true;
            }
            else
            {
                // falseを返す。
                return false;
            }*/

            if (statusJog != SystemConstants.BTN_ON)
                return false;

            if (statusPressSw != SystemConstants.BTN_ON)
                return false;

            return true;
#endif
        }

        // シーンセット1のOFF-> ONチェック
        // チャック開閉1のOFF-> ONチェック及びON-> OFF条件
        public bool checkSeal_Set1_Btn_and_Seal_Chack1_Btn(ButtonActionStruct actionStruct, int WorkID, int BtnStatus)
        {
#if HCSM40
            return false;
#else
            int statusSeal1_Btn = 0;

            // 防水1の値を取得
            ReadPushBtnPC(SystemConstants.SEAL1_BTN, ref statusSeal1_Btn);

            // 防水1がONだった場合
            if (statusSeal1_Btn == SystemConstants.BTN_ON)
            {
                // trueを返す。
                return true;
            }
            else
            {
                // falseを返す。
                return false;
            }
#endif
        }

        // シーンセット2のOFF-> ONチェック
        // チャック開閉2のOFF-> ONチェック及びON-> OFF条件
        public bool checkSeal_Set2_Btn_and_Seal_Chack2_Btn(ButtonActionStruct actionStruct, int WorkID, int BtnStatus)
        {
#if HCSM40
            return false;
#else
            int statusSeal2_Btn = 0;

            // 防水2の値を取得
            ReadPushBtnPC(SystemConstants.SEAL2_BTN, ref statusSeal2_Btn);

            // 防水2がONだった場合
            if (statusSeal2_Btn == SystemConstants.BTN_ON)
            {
                // trueを返す。
                return true;
            }
            else
            {
                // falseを返す。
                return false;
            }
#endif            
        }

        // サイクルモード OFF -> ON チェック
        public bool checkCycleMode(ButtonActionStruct actionstruct, int WorkID, int BtnStatus)
        {
            // サイクルモードがJOGの場合は、常にOK
            if (WorkID == SystemConstants.JOG_BTN) return true;

            int statusInching1 = SystemConstants.BTN_OFF, statusInching2 = SystemConstants.BTN_OFF;

#if !HCSM40
            // インチング1, インチング2の値を取得
            ReadPushBtnPC(SystemConstants.INCHING1_BTN, ref statusInching1);
            ReadPushBtnPC(SystemConstants.INCHING2_BTN, ref statusInching2);
#endif

            // インチングボタンが両方ともOFFの場合、サイクルモードの変更を許可
            if (statusInching1 == SystemConstants.BTN_OFF && statusInching2 == SystemConstants.BTN_OFF)
                return true;
            else
                return false;
        }

        // インチングスイッチの OFF -> ON チェック
        public bool checkInchingMode(ButtonActionStruct actionstruct, int WorkID, int BtnStatus)
        {
            // JOGモードかを判断する
            int statusCycleJogMode = SystemConstants.BTN_OFF;
            ReadPushBtnPC(SystemConstants.JOG_BTN, ref statusCycleJogMode);

            if (statusCycleJogMode == SystemConstants.BTN_ON)
                return true;
            else
                return false;
        }


        // ビットを操作するアクション
        public bool bitControlAction(ButtonActionStruct actionStruct, int WorkID, int BtnStatus)
        {
            bitControl(WorkID, actionStruct.Digit, BtnStatus);

            return true;
        }

        // 値を直接セットするアクション
        public bool valueSetAction(ButtonActionStruct actionStruct, int WorkID, int BtnStatus)
        {
            // ONの場合
            if (BtnStatus == SystemConstants.BTN_ON)
            {
                //加工値メモリに保存する。
                workDataMemory.Set(SystemConstants.WORKMEM_TYPE_WORKBTN, WorkID, actionStruct.Digit.ToString());

                // ビット演算結果をMemWriteする
                machineConnector.MemWrite(actionStruct.Address, actionStruct.Digit);

            }
            // OFFの場合
            else if (BtnStatus == SystemConstants.BTN_OFF)
            {
                //加工値メモリに保存する。
                workDataMemory.Set(SystemConstants.WORKMEM_TYPE_WORKBTN, WorkID, "0");

                // ビット演算結果をMemWriteする
                machineConnector.MemWrite(actionStruct.Address, 0);
            }

            return true;
        }

        // 動作モードグループの関連アクション
        public bool modeGroupAction(ButtonActionStruct actionStruct, int WorkID, int BtnStatus)
        {
            // 切断対象ボタン
            int[] OffBtnIDs = new int[] {
				SystemConstants.NORMAL_BTN,
				SystemConstants.EJECT_BTN,
				SystemConstants.TEST_BTN, 
				SystemConstants.SAMPLE_BTN,
				SystemConstants.FREE_BTN
			};

            // 自分のボタン以外はOffにする
            foreach (int btnID in OffBtnIDs)
            {
                if (WorkID != btnID)
                {
                    WritePushBtn(btnID, SystemConstants.BTN_OFF, false);
                }
            }
#if !HCSM40
            // フリーモードの時は圧着機スイッチをOffにする
            if (WorkID == SystemConstants.FREE_BTN)
            {
                WritePushBtn(SystemConstants.CRIMP1_SW_BTN, SystemConstants.BTN_OFF, false);
                WritePushBtn(SystemConstants.CRIMP2_SW_BTN, SystemConstants.BTN_OFF, false);
            }
#endif
            return true;
        }

        // サイクルモードの関連アクション
        public bool cycleModeAction(ButtonActionStruct actionStruct, int WorkID, int BtnStatus)
        {
            // 切断対象ボタン
            int[] OffBtnIDs = new int[] {
				SystemConstants.CYCLE_BTN,
				SystemConstants.JOG_BTN,
				SystemConstants.AUTO_BTN
			};

            // 自分のボタン以外はOffにする
            foreach (int btnID in OffBtnIDs)
            {
                if (WorkID != btnID)
                {
                    WritePushBtn(btnID, SystemConstants.BTN_OFF, false);
                }
            }

            return true;
        }

        // OFF動作禁止
        public bool forbidOffAction(ButtonActionStruct actionStruct, int WorkID, int BtnStatus)
        {
            // OFFの場合は、falseを返す
            if (SystemConstants.BTN_OFF == BtnStatus)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        // 読み取り専用
        public bool readonlyAction(ButtonActionStruct actionStruct, int WorkID, int BtnStatus)
        {
            return false;
        }

        // 1秒したらOffにする。
        public bool autoOffAction(ButtonActionStruct actionStruct, int WorkID, int BtnStatus)
        {
            // ONの場合
            if (SystemConstants.BTN_ON == BtnStatus)
            {
                // 1秒後にOFFにする処理を予約する
                ThreadPool.QueueUserWorkItem(
                    new WaitCallback(delegate(object o)
                    {
                        Thread.Sleep(1000);
                        WritePushBtn(WorkID, SystemConstants.BTN_OFF, true);
                    })
                );
            }

            return true;
        }

        // 0.5秒したらOffにする。
        public bool autoOffAction2(ButtonActionStruct actionStruct, int WorkID, int BtnStatus)
        {
            // ONの場合
            if (SystemConstants.BTN_ON == BtnStatus)
            {
                // 0.5秒後にOFFにする処理を予約する
                ThreadPool.QueueUserWorkItem(
                    new WaitCallback(delegate(object o)
                    {
                        Thread.Sleep(500);
                        WritePushBtn(WorkID, SystemConstants.BTN_OFF, true);
                    })
                );
            }

            return true;
        }

        // 0.2秒したらOffにする。
        public bool autoOffAction3(ButtonActionStruct actionStruct, int WorkID, int BtnStatus)
        {
            // ONの場合
            if (SystemConstants.BTN_ON == BtnStatus)
            {
                // 0.5秒後にOFFにする処理を予約する
                ThreadPool.QueueUserWorkItem(
                    new WaitCallback(delegate(object o)
                    {
                        Thread.Sleep(200);
                        WritePushBtn(WorkID, SystemConstants.BTN_OFF, true);
                    })
                );
            }

            return true;
        }

        // ストリップ１をOFFにする場合、圧着１防水１、ハーフストリップでONになっているボタンをOFFにする。
        public bool strip1Action(ButtonActionStruct actionStruct, int WorkID, int BtnStatus)
        {
            if (SystemConstants.BTN_OFF == BtnStatus)
            {
#if !HCSM40
                WritePushBtn(SystemConstants.CRIMP1_BTN, SystemConstants.BTN_OFF, false);
                WritePushBtn(SystemConstants.SEAL1_BTN, SystemConstants.BTN_OFF, false);
                WritePushBtn(SystemConstants.CRIMP1_SW_BTN, SystemConstants.BTN_OFF, false);
#endif
                WritePushBtn(SystemConstants.SEMISTRIP1_BTN, SystemConstants.BTN_OFF, false);
                
            }

            return true;
        }

        // ストリップ2をOFFにする場合、圧着2, 防水2, ハーフストリップ2で、
        // ONになっているボタンをOFFにして、ストリップ2をOFFにする。
        public bool strip2Action(ButtonActionStruct actionStruct, int WorkID, int BtnStatus)
        {
            if (SystemConstants.BTN_OFF == BtnStatus)
            {
#if !HCSM40
                WritePushBtn(SystemConstants.CRIMP2_BTN, SystemConstants.BTN_OFF, false);
                WritePushBtn(SystemConstants.SEAL2_BTN, SystemConstants.BTN_OFF, false);
                WritePushBtn(SystemConstants.CRIMP2_SW_BTN, SystemConstants.BTN_OFF, false);
#endif
                WritePushBtn(SystemConstants.SEMISTRIP2_BTN, SystemConstants.BTN_OFF, false);                
            }

            return true;
        }

        // 圧着1をOFFにする場合、圧着機1スイッチをOFFにして、圧着1をOFFにする。
        public bool crimp1Action(ButtonActionStruct actionStruct, int WorkID, int BtnStatus)
        {
#if !HCSM40
            if (SystemConstants.BTN_OFF == BtnStatus)
            {
                WritePushBtn(SystemConstants.CRIMP1_SW_BTN, SystemConstants.BTN_OFF, false);
            }
#endif
            return true;
        }

        // 圧着2をOFFにする場合、圧着機2スイッチをOFFにして、圧着2をOFFにする。
        public bool crimp2Action(ButtonActionStruct actionStruct, int WorkID, int BtnStatus)
        {
#if !HCSM40
            if (SystemConstants.BTN_OFF == BtnStatus)
            {
                WritePushBtn(SystemConstants.CRIMP2_SW_BTN, SystemConstants.BTN_OFF, false);
            }
#endif
            return true;
        }

        // 圧着機1のON-> OFFチェック
        public bool checkCrimpSw1and2Off(ButtonActionStruct actionStruct, int WorkID, int BtnStatus)
        {
#if HCSM40
            return false;
#else
            int statusInching = 0;

            // インチング1の値を取得
            if (WorkID == SystemConstants.CRIMP1_SW_BTN)
            {
                ReadPushBtnPC(SystemConstants.INCHING1_BTN, ref statusInching);
            }
            else
            {
                ReadPushBtnPC(SystemConstants.INCHING2_BTN, ref statusInching);
            }

            //ストリップ1がONかつ、ハーフストリップ1がOFFだった場合
            if ( statusInching == SystemConstants.BTN_OFF)
            {
                // trueを返す。
                return true;
            }
            else
            {
                // falseを返す。
                return false;
            }
#endif
        }

#if FOR_JN03SDGP || (FOR_JN03SDC && FOR_Y)
        // セレクター切り替えアクション
        public bool selectorChangeAction(ButtonActionStruct actionStruct, int WorkID, int BtnStatus)
        {
            int statusLoad = 0;
            int statusSelector = 0;

            // ロードの値を取得
            ReadPushBtnPC(SystemConstants.LOAD1_BTN, ref statusLoad);

            // ONだった場合
            if (statusLoad == SystemConstants.BTN_ON)
            {
                // falseを返す。
                return false;
            }

            // 押されたセレクターの値を取得
            ReadPushBtnPC(WorkID, ref statusSelector);

            // ONだった場合
            if (statusSelector == SystemConstants.BTN_ON)
            {
                // falseを返す。
                return false;
            }

            // 切断対象ボタン
            int[] OffBtnIDs = new int[] {
				SystemConstants.SELECTOR1_BTN,
				SystemConstants.SELECTOR2_BTN
			};

            // 自分のボタン以外はOffにする
            foreach (int btnID in OffBtnIDs)
            {
                if (WorkID != btnID)
                {
                    WritePushBtn(btnID, SystemConstants.BTN_OFF, false);
                }
            }
            return true;
        }
#endif

#if FOR_JN03SDGP
        // インチング1ボタン及びインチング2ボタンのON-> OFFチェック
        public bool checkInching1and2_BtnOff(ButtonActionStruct actionStruct, int WorkID, int BtnStatus)
        {          
            double statusInching = 0;           

            // PRESS SWの値取得
            if (WorkID == SystemConstants.INCHING1_BTN)
                Program.DataController.ReadWorkData(SystemConstants.STATUS_INCHING1, ref statusInching);                
            else
                Program.DataController.ReadWorkData(SystemConstants.STATUS_INCHING2, ref statusInching);

            if (statusInching == SystemConstants.BTN_ON)
                return false;

            return true;
        }
#endif
        // 防水1がOFFになった際、チャック開閉1もOFFにする
        public bool seal1Action(ButtonActionStruct actionStruct, int WorkID, int BtnStatus)
        {
#if !HCSM40
            if (SystemConstants.BTN_OFF == BtnStatus)
            {
                WritePushBtn(SystemConstants.SEAL_CHUCK1_BTN, SystemConstants.BTN_OFF, false);
            }
#endif
            return true;
        }

        // 防水2がOFFになった際、チャック開閉2もOFFにする
        public bool seal2Action(ButtonActionStruct actionStruct, int WorkID, int BtnStatus)
        {
#if !HCSM40
            if (SystemConstants.BTN_OFF == BtnStatus)
            {
                WritePushBtn(SystemConstants.SEAL_CHUCK2_BTN, SystemConstants.BTN_OFF, false);
            }
#endif
            return true;
        }

#if FOR_JN03SDGP || (FOR_JN03SDC && FOR_Y)
        // フィード開閉OFFチェック（フィード、ロードOFF->ONチェック） 
        public bool checkFeedOpen_BtnOff(ButtonActionStruct actionStruct, int WorkID, int BtnStatus)
        {
            int statusFeedOpen = 0;

            ReadPushBtnPC(SystemConstants.FEED_OPEN1_BTN, ref statusFeedOpen);
            
            if (statusFeedOpen == SystemConstants.BTN_ON)
                return false;

            return true;
        }
#endif

        // １側端子サイド/エンド切り替えアクション
        public bool termType1ChangeAction(ButtonActionStruct actionStruct, int WorkID, int BtnStatus)
        {
            int statusType = 0;

            // 押された値を取得
            ReadPushBtnPC(WorkID, ref statusType);

            // ONだった場合
            if (statusType == SystemConstants.BTN_ON)
            {
                // falseを返す。
                return false;
            }

            // 切断対象ボタン
            int[] OffBtnIDs = new int[] {
#if !HCSM40
                SystemConstants.END_TERM1_BTN,
				SystemConstants.SIDE_TERM1_BTN
#endif
			};

            // 自分のボタン以外はOffにする
            foreach (int btnID in OffBtnIDs)
            {
                if (WorkID != btnID)
                {
                    WritePushBtn(btnID, SystemConstants.BTN_OFF, false);
                }
            }
            return true;
        }

        // ２側端子サイド/エンド切り替えアクション
        public bool termType2ChangeAction(ButtonActionStruct actionStruct, int WorkID, int BtnStatus)
        {
            int statusType = 0;

            // 押された値を取得
            ReadPushBtnPC(WorkID, ref statusType);

            // ONだった場合
            if (statusType == SystemConstants.BTN_ON)
            {
                // falseを返す。
                return false;
            }

            // 切断対象ボタン
            int[] OffBtnIDs = new int[] {
#if !HCSM40
                SystemConstants.END_TERM2_BTN,
				SystemConstants.SIDE_TERM2_BTN
#endif
			};

            // 自分のボタン以外はOffにする
            foreach (int btnID in OffBtnIDs)
            {
                if (WorkID != btnID)
                {
                    WritePushBtn(btnID, SystemConstants.BTN_OFF, false);
                }
            }
            return true;
        }

#if FOR_JN03SDC && FOR_Y
        // 端子送りONチェック（端子送りOFF->ONチェック） 
        public bool checkATermAirFeed_BtnOn(ButtonActionStruct actionStruct, int WorkID, int BtnStatus)
        {
            int statusTermAirFeed = 0;

            if(WorkID == SystemConstants.TERM_FEED1_BTN)
                ReadPushBtnPC(SystemConstants.TERM_AIRFEED1_BTN, ref statusTermAirFeed);
            else
                ReadPushBtnPC(SystemConstants.TERM_AIRFEED2_BTN, ref statusTermAirFeed);

            if (statusTermAirFeed == SystemConstants.BTN_OFF)
                return false;

            return true;
        }
#endif
        /// <summary>
        /// 空運転モードON時、電線がロードしていればメッセージを出し空運転モードにしない。
        /// </summary>
        /// <param name="actionStruct"></param>
        /// <param name="WorkID"></param>
        /// <param name="BtnStatus"></param>
        /// <returns></returns>
        public bool checkWireLoad_FreeOn(ButtonActionStruct actionStruct, int WorkID, int BtnStatus)
        {
            int statusLoad = 0;
            
            ReadPushBtn(SystemConstants.LOAD1_STATUS, ref statusLoad);

            if (statusLoad == SystemConstants.BTN_ON)
            {
                Utility.ShowErrorMsg(SystemConstants.SYSTEM_MSG038);
                return false;
            }

            return true;
        }

        /// <summary>
        /// ロードボタンON時、空運転中ならメッセージをロードさせない。
        /// </summary>
        /// <param name="actionStruct"></param>
        /// <param name="WorkID"></param>
        /// <param name="BtnStatus"></param>
        /// <returns></returns>
        public bool checkFreeMode_LoadOn(ButtonActionStruct actionStruct, int WorkID, int BtnStatus)
        {
            int statusFree = 0;
            
            ReadPushBtn(SystemConstants.FREE_BTN, ref statusFree);

            if (statusFree == SystemConstants.BTN_ON)
            {
                Utility.ShowErrorMsg(SystemConstants.SYSTEM_MSG038);
                return false;
            }

            return true;
        }

    }
}
