using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Alchemist
{
    public partial class mainfrm : Form
    {
        // スレッド
        private Thread thread = null;
        delegate void RefreshDelegate();

        // 表示更新スレッド
        private void monitorRefreshThread()
        {
            while (true)
            {
                // UIスレッドでrefresh関数を実行させる
                Invoke(new RefreshDelegate(refresh));

                // カウンタフォームの表示更新
                if (counterForm.Visible == true)
                {
                    Invoke(new RefreshDelegate(counterForm.refresh));
                }

                // 段取り操作の表示更新
                if (setupOperationForm.Visible == true)
                {
                    Invoke(new RefreshDelegate(setupOperationForm.refresh));
                }

                // 通信画面の表示更新
                if (connectOperationForm.Visible == true)
                {
                    Invoke(new RefreshDelegate(connectOperationForm.refresh));
                }

                // 情報画面の表示更新
                Invoke(new RefreshDelegate(errInfoMsgForm.refresh));

                // 加工詳細設定画面(電線切断)の表示更新
                if (workDetailItemFormwire1.Visible == true)
                {
                    Invoke(new RefreshDelegate(workDetailItemFormwire1.refresh));
                }

                // 加工詳細設定画面(ストリップ1)の表示更新
                if (workDetailItemFormstrip1.Visible == true)
                {
                    Invoke(new RefreshDelegate(workDetailItemFormstrip1.refresh));
                }

                // 加工詳細設定画面(ストリップ2)の表示更新
                if (workDetailItemFormstrip2.Visible == true)
                {
                    Invoke(new RefreshDelegate(workDetailItemFormstrip2.refresh));
                }
                
                // 速度設定画面の表示更新
                if (workDetailSpeedForm.Visible == true)
                {
                    Invoke(new RefreshDelegate(workDetailSpeedForm.refresh));
                }

                // 加工動作詳細設定画面の表示更新
                if (workDetailMotionForm.Visible == true)
                {
                    Invoke(new RefreshDelegate(workDetailMotionForm.refresh));
                }

                // 段取り操作画面の表示更新
                if (setupOperationForm.Visible == true)
                {
                    Invoke(new RefreshDelegate(setupOperationForm.refresh));
                }

                // システム操作画面の表示更新
                if (systemConfigrationForm.Visible == true)
                {
                    Invoke(new RefreshDelegate(systemConfigrationForm.refresh));
                }

                // 学習データ操作画面の表示更新
                if (databaseForm.Visible)
                {
                    Invoke(new RefreshDelegate(databaseForm.refresh));
                }

                // IOモニタの更新
                if (ioMonitorForm.Visible)
                {
                    Invoke(new RefreshDelegate(ioMonitorForm.refresh));
                }

                Thread.Sleep(100);
            }
        }


        #region 子フォーム群の生成
        private counterfrm counterForm = new counterfrm();			                	            /* カウンタ詳細画面 */
        private setupOperationfrm setupOperationForm = new setupOperationfrm();	    	            /* 段取り操作画面 */
        private workDetailItemfrmWIRE1 workDetailItemFormwire1 = new workDetailItemfrmWIRE1();      /* 加工値詳細設定画面(電線切断) */
        private workDetailItemfrmSTRIP1 workDetailItemFormstrip1 = new workDetailItemfrmSTRIP1();   /* 加工値詳細設定画面(1側ストリップ) */
        private workDetailItemfrmSTRIP2 workDetailItemFormstrip2 = new workDetailItemfrmSTRIP2();   /* 加工値詳細設定画面(2側ストリップ) */
        private workDetailSpeedfrm workDetailSpeedForm = new workDetailSpeedfrm();                  /* 速度設定画面 */
        private workDetailMotionfrm workDetailMotionForm = new workDetailMotionfrm();               /* 加工動作詳細画面 */
        private machineOperationfrm machineOperationForm = new machineOperationfrm();               /* 機械操作画面 */
        private bankOperationfrm bankOperationForm = new bankOperationfrm();                        /* バンク操作画面 */
        private systemConfigurationfrm systemConfigrationForm = new systemConfigurationfrm();       /* システム設定画面 */
        private connectOperationfrm connectOperationForm = new connectOperationfrm();			    /* 接続・切断画面 */
        private errInfoMsgfrm errInfoMsgForm = new errInfoMsgfrm();                                 /* 情報・エラーメッセージ画面 */
        private passwordCollationfrm passwordcollationForm = new passwordCollationfrm();            /* パスワード照合画面 */
        private AboutBox1 aboutboxForm = new AboutBox1();                                           /* 情報ボックス画面 */
        private iocheckfrm iocheckForm = new iocheckfrm();                                          /* メモリモニタ画面 */
        private ioMonitorfrm ioMonitorForm = new ioMonitorfrm();                                    /* ＩＯモニター画面 */
        private learnDataSearchfrm databaseForm = new learnDataSearchfrm();                         /* データベース画面 */
        private learnDataItemEditfrm learnDataItemEditForm = new learnDataItemEditfrm();            /* 学習データ項目編集画面 */
#if OMOIKANE
        private omoikane.mainfrm omoikaneForm = new omoikane.mainfrm();                             /* 思兼メインフォーム */
#endif
        #endregion

        // 加工動作画面設定table
        private Dictionary<int, workMotionStruct> map = new Dictionary<int, workMotionStruct>();

        // 加工動作Visible設定構造体
        private struct workMotionStruct
        {
            public Image image;
            public bool wire_Length;
            public bool strip_Length;
            public bool semi_Strip;
            public bool strip_Depth;
            public bool strip_Pullback;
            public bool crimp_Height;
            public bool crimp_Position;
            public bool seal_Insert_Length;
            public bool seal_Insert_Back;
        };

        // 1側のボタンIDの配列
        private int[] btnIdArray1 = new int[]{
                SystemConstants.STRIP1_BTN,
                //SystemConstants.CRIMP1_BTN,
                //SystemConstants.SEAL1_BTN,
                SystemConstants.SEMISTRIP1_BTN,
            };

        // 2側のボタンIDの配列
        private int[] btnIdArray2 = new int[]{
                SystemConstants.STRIP2_BTN,
                //SystemConstants.CRIMP2_BTN,
                //SystemConstants.SEAL2_BTN,
                SystemConstants.SEMISTRIP2_BTN,
            };

        // コンストラクタ
        public mainfrm()
        {
            InitializeComponent();
        }

        // 初期化処理
        private void Initialize()
        {
            workMotionStruct[] workMtnStruct = new workMotionStruct[14];

#if OMOIKANE
            // 思兼のクラス等を設定する
            omoikaneForm.MX20Connector = Program.OmoikaneConnector;
            omoikaneForm.SystemData = Program.OmoikaneSystemData;
            // 画面更新スレッド動作用に非表示起動
            omoikaneForm.Show();            
#endif

            // 機械毎のフォームの表示・非表示設定する
            formCustom();

            #region フォームの初期化
            aboutboxForm.Initialize();
            bankOperationForm.Initialize();
            connectOperationForm.Initialize();
            counterForm.Initialize();
            errInfoMsgForm.Initialize();
            iocheckForm.Initialize();
            ioMonitorForm.Initialize();
            machineOperationForm.Initialize();
            passwordcollationForm.Initialize();
            setupOperationForm.Initialize();
            systemConfigrationForm.Initialize();
            workDetailItemFormwire1.Initialize();
            workDetailItemFormstrip1.Initialize();
            workDetailItemFormstrip2.Initialize();
            workDetailMotionForm.Initialize();
            workDetailSpeedForm.Initialize();
            databaseForm.Initialize();
            learnDataItemEditForm.Initialize();
            #endregion

            // Ioモニタをシステム設定フォームに入れる
            systemConfigrationForm.ioMonitorForm = ioMonitorForm;

#if HCSM40    
            #region テキストボックスにイベントを設定する
            SetTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.QTY_SET_COUNTER1, textQTY);  // QTY本数設定
            SetTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.LOT_SET_COUNTER1, textLOT);  // LOT本数設定
            SetTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.WIRE_LENGTH1, textWIRE_LENGTH_VALUE);    // 切断長
            SetTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.STRIP_LENGTH1, textSTRIP1_VALUE);    // ストリップ1
            SetTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.STRIP_LENGTH2, textSTRIP2_VALUE);    // ストリップ2
            SetTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.STRIP_SEMI_LENGTH1, textSEMI_STRIP1_VALUE); // ハーフストリップ1
            SetTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.STRIP_SEMI_LENGTH2, textSEMI_STRIP2_VALUE); // ハーフストリップ2
            SetTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.STRIP_DEPTH1, textSTRIP_DEPTH_VALUE);    // 切込深さ
            SetTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.STRIP_PULLBACK1, textSTRIP_PULLBACK_VALUE);   // ストリッププルバック
            #endregion

            #region ボタンにイベントを設定する
            SetBtnEvent(SystemConstants.STRIP1_BTN, SystemConstants.BTN_PUSH, btnSTRIP1);
            SetBtnEvent(SystemConstants.STRIP2_BTN, SystemConstants.BTN_PUSH, btnSTRIP2);
            SetBtnEvent(SystemConstants.SEMISTRIP1_BTN, SystemConstants.BTN_PUSH, btnSEMISTRIP1);
            SetBtnEvent(SystemConstants.SEMISTRIP2_BTN, SystemConstants.BTN_PUSH, btnSEMISTRIP2);
            SetBtnEvent(SystemConstants.NORMAL_BTN, SystemConstants.BTN_ON, btnNORMAL);
            SetBtnEvent(SystemConstants.EJECT_BTN, SystemConstants.BTN_ON, btnEJECT);
            SetBtnEvent(SystemConstants.SAMPLE_BTN, SystemConstants.BTN_ON, btnSAMPLE);
            SetBtnEvent(SystemConstants.TEST_BTN, SystemConstants.BTN_ON, btnTEST);
            SetBtnEvent(SystemConstants.FREE_BTN, SystemConstants.BTN_ON, btnFREE);
            SetBtnEvent(SystemConstants.JOG_BTN, SystemConstants.BTN_ON, btnJOG);
            SetBtnEvent(SystemConstants.CYCLE_BTN, SystemConstants.BTN_ON, btnCYCLE);
            SetBtnEvent(SystemConstants.AUTO_BTN, SystemConstants.BTN_ON, btnAUTO);
            SetBtnEvent(SystemConstants.LOT_INTERVAL1_BTN, SystemConstants.BTN_PUSH, btnAUTOEXIT);
            SetBtnEvent(SystemConstants.QTY_COUNTER_RESET1_BTN, SystemConstants.BTN_ON, btnQTYReset);
            SetBtnEvent(SystemConstants.LOT_COUNTER_RESET1_BTN, SystemConstants.BTN_ON, btnLOTReset);
            #endregion

            #region テンキー入力用のクリックイベント
            ClickTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.TENKEY_INPUT_CHECK, SystemConstants.QTY_SET_COUNTER1, textQTY);
            ClickTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.TENKEY_INPUT_DATA, SystemConstants.LOT_SET_COUNTER1, textLOT);
            ClickTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.TENKEY_INPUT_DATA, SystemConstants.WIRE_LENGTH1, textWIRE_LENGTH_VALUE);
            ClickTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.TENKEY_INPUT_DATA, SystemConstants.STRIP_LENGTH1, textSTRIP1_VALUE);
            ClickTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.TENKEY_INPUT_DATA, SystemConstants.STRIP_LENGTH2, textSTRIP2_VALUE);
            ClickTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.TENKEY_INPUT_DATA, SystemConstants.STRIP_PULLBACK1, textSTRIP_PULLBACK_VALUE);
            ClickTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.TENKEY_INPUT_DATA, SystemConstants.STRIP_DEPTH1, textSTRIP_DEPTH_VALUE);
            ClickTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA, SystemConstants.TENKEY_INPUT_DATA, SystemConstants.STRIP_SEMI_LENGTH1, textSEMI_STRIP1_VALUE);
            ClickTextBoxEvent(SystemConstants.WORKID_TYPE_WORKDATA,  SystemConstants.TENKEY_INPUT_DATA, SystemConstants.STRIP_SEMI_LENGTH2, textSEMI_STRIP2_VALUE);
            #endregion
#endif

#if HCSM40
            #region 加工動作ごとの設定

            // 加工動作が1側のぶつ切りの場合の設定
            workMtnStruct[0].image = Alchemist.Properties.Resources.Fig1_0;
            workMtnStruct[0].wire_Length = true;
            workMtnStruct[0].strip_Length = false;
            workMtnStruct[0].semi_Strip = false;
            workMtnStruct[0].strip_Depth = false;
            workMtnStruct[0].strip_Pullback = false;
            workMtnStruct[0].crimp_Height = false;
            workMtnStruct[0].crimp_Position = false;
            workMtnStruct[0].seal_Insert_Length = false;
            workMtnStruct[0].seal_Insert_Back = false;

            // 加工動作が2側のぶつ切りの場合の設定
            workMtnStruct[1].image = Alchemist.Properties.Resources.Fig2_0;
            workMtnStruct[1].wire_Length = true;
            workMtnStruct[1].strip_Length = false;
            workMtnStruct[1].semi_Strip = false;
            workMtnStruct[1].strip_Depth = false;
            workMtnStruct[1].strip_Pullback = false;
            workMtnStruct[1].crimp_Height = false;
            workMtnStruct[1].crimp_Position = false;
            workMtnStruct[1].seal_Insert_Length = false;
            workMtnStruct[1].seal_Insert_Back = false;

            // 加工動作が1側のストリップの場合の設定
            workMtnStruct[2].image = Alchemist.Properties.Resources.Fig1_1;
            workMtnStruct[2].wire_Length = true;
            workMtnStruct[2].strip_Length = true;
            workMtnStruct[2].semi_Strip = false;
            workMtnStruct[2].strip_Depth = true;
            workMtnStruct[2].strip_Pullback = true;
            workMtnStruct[2].crimp_Height = false;
            workMtnStruct[2].crimp_Position = false;
            workMtnStruct[2].seal_Insert_Length = false;
            workMtnStruct[2].seal_Insert_Back = false;

            // 加工動作が2側のストリップの場合の設定
            workMtnStruct[3].image = Alchemist.Properties.Resources.Fig2_1;
            workMtnStruct[3].wire_Length = true;
            workMtnStruct[3].strip_Length = true;
            workMtnStruct[3].semi_Strip = false;
            workMtnStruct[3].strip_Depth = true;
            workMtnStruct[3].strip_Pullback = true;
            workMtnStruct[3].crimp_Height = false;
            workMtnStruct[3].crimp_Position = false;
            workMtnStruct[3].seal_Insert_Length = false;
            workMtnStruct[3].seal_Insert_Back = false;

            // 加工動作が1側のハーフストリップの場合の設定
            workMtnStruct[4].image = Alchemist.Properties.Resources.Fig1_9;
            workMtnStruct[4].wire_Length = true;
            workMtnStruct[4].strip_Length = true;
            workMtnStruct[4].semi_Strip = true;
            workMtnStruct[4].strip_Depth = true;
            workMtnStruct[4].strip_Pullback = true;
            workMtnStruct[4].crimp_Height = false;
            workMtnStruct[4].crimp_Position = false;
            workMtnStruct[4].seal_Insert_Length = false;
            workMtnStruct[4].seal_Insert_Back = false;

            // 加工動作が2側のハーフストリップの場合の設定
            workMtnStruct[5].image = Alchemist.Properties.Resources.Fig2_9;
            workMtnStruct[5].wire_Length = true;
            workMtnStruct[5].strip_Length = true;
            workMtnStruct[5].semi_Strip = true;
            workMtnStruct[5].strip_Depth = true;
            workMtnStruct[5].strip_Pullback = true;
            workMtnStruct[5].crimp_Height = false;
            workMtnStruct[5].crimp_Position = false;
            workMtnStruct[5].seal_Insert_Length = false;
            workMtnStruct[5].seal_Insert_Back = false;
            #endregion
#endif

            #region 加工動作ごとのテーブル設定          
            map.Add(0, workMtnStruct[0]);       // 1側のぶつ切りのテーブルを設定
            map.Add(1, workMtnStruct[1]);       // 2側のぶつ切りのテーブルを設定
            map.Add(2, workMtnStruct[2]);       // 1側のストリップのテーブルを設定
            map.Add(3, workMtnStruct[3]);       // 2側のストリップのテーブルを設定
            map.Add(6, workMtnStruct[4]);       // 1側のストリップ、圧着のテーブルを設定
            map.Add(7, workMtnStruct[5]);       // 2側のストリップ、圧着のテーブルを設定
            map.Add(8, workMtnStruct[6]);       // 1側の防水のテーブルを設定
            map.Add(9, workMtnStruct[7]);       // 2側の防水のテーブルを設定
            map.Add(10, workMtnStruct[8]);      // 1側のストリップ、防水のテーブルを設定
            map.Add(11, workMtnStruct[9]);      // 2側のストリップ、防水のテーブルを設定
            map.Add(14, workMtnStruct[10]);     // 1側のストリップ、防水、圧着のテーブルを設定
            map.Add(15, workMtnStruct[11]);     // 2側のストリップ、防水、圧着のテーブルを設定
            map.Add(18, workMtnStruct[12]);     // 1側のストリップ、ハーフストリップのテーブルを設定
            map.Add(19, workMtnStruct[13]);     // 2側のストリップ、ハーフストリップのテーブルを設定
            #endregion

            // フォーム内のテキストボックスの長さを10にする。
            SetTextBoxLength(this, 10);

            // 描画スレッドを開始する
            thread = new Thread(new ThreadStart(monitorRefreshThread));
            thread.Start();
        }

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
        /// センサー行
        /// +--+--+--+--+--+
        /// | 0| 2| 4| 6| 8|
        /// +--+--+--+--+--+
        /// | 1| 3| 5| 7| 9|
        /// +--+--+--+--+--+
        /// 
        /// 動作行 (開始番号は、MOTION_STATUS_BOX_START で定義)
        /// +--+--+--+--+--+
        /// |30|32|34|36|38|
        /// +--+--+--+--+--+
        /// |31|33|35|37|39|
        /// +--+--+--+--+--+
        /// 
        /// </summar>y
        /// 
        private struct statusDspStruct
        {
            public int BtnID;
            public int MsgID;
            public bool ReverseFlg;
        }
        private Dictionary<int, statusDspStruct> statusDspMap = new Dictionary<int, statusDspStruct>();

        /// <summary>
        /// 状態表示項目データを追加する
        /// </summary>
        /// <param name="DspTimming"></param>
        /// <param name="DspPosition"></param>
        /// <param name="BtnID"></param>
        private void statusDspMapAdd(int DspTimming, int DspPosition, int BtnID, int MsgID, bool ReverseFlg = false)
        {
            // 引数が不適切な場合、例外を発生させる
            if (DspTimming < 0 || DspPosition < 0 || DspPosition > 99) new ArgumentException("DspTiming or DspPosition is under");

            // ページの最大数を求める
            if (DspPosition < SystemConstants.MOTION_STATUS_BOX_START)
            {
                if (DspTimming > statusDspSensorPgMax) statusDspSensorPgMax = DspTimming;
            }
            else
            {
                if (DspTimming > statusDspMotionPgMax) statusDspMotionPgMax = DspTimming;
            }

            // インデックスを生成する
            int index = dspIndexCalc(DspTimming, DspPosition);

            // 表示用データに追加する
            statusDspStruct dsp = new statusDspStruct();
            dsp.BtnID = BtnID;
            dsp.MsgID = MsgID;
            dsp.ReverseFlg = ReverseFlg;

            statusDspMap.Add(index, dsp);
        }

        public static int dspIndexCalc(int DspTimming, int DspPosition)
        {
            return (DspTimming * 100) + DspPosition;
        }


        /// <summary>
        /// メインフォーム初期化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mainfrm_Load(object sender, EventArgs e)
        {
            Initialize();
        }

        /// <summary>
        /// フォームが閉じられた時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mainfrm_FormClosed(object sender, FormClosedEventArgs e)
        {
#if OMOIKANE
            // 思兼フォームを終了する
            omoikaneForm.FormCloseProcess();
#endif

            // 表示処理を先に止める
            if (thread != null)
            {
                // スレッドを停止する
                thread.Abort();
                thread.Join();
            }

            // 機種名を保存する
            Program.SystemData.machineid = Program.DataController.GetMachineName();

            try
            {
                Program.SystemData.Save();
            }
            catch
            {
                /* 例外を無視 */
            }

            #region フォームを破棄する
            counterForm.Dispose();
            setupOperationForm.Dispose();
            workDetailItemFormwire1.Dispose();
            workDetailItemFormstrip1.Dispose();
            workDetailItemFormstrip2.Dispose();
            workDetailSpeedForm.Dispose();
            workDetailMotionForm.Dispose();
            machineOperationForm.Dispose();
            bankOperationForm.Dispose();
            systemConfigrationForm.Dispose();
            connectOperationForm.Dispose();
            errInfoMsgForm.Dispose();
            passwordcollationForm.Dispose();
            databaseForm.Dispose();
            learnDataItemEditForm.Dispose();
            #endregion

            // データコントローラを解放する
            Program.DataController.Dispose();
            Dispose();
        }

        // 描画処理
        private void refresh()
        {
            int selectedNo = 0;
            string bankComment = "";


            #region 接続状態の取得
            if (Program.DataController.IsConnect() == true)
            {
                // true: 背景Green 文字色 White 文字 ONLINE
                lblOFFLINE.Text = Utility.GetMessageString(SystemConstants.SYSTEM_MSG, SystemConstants.SYSTEM_MSG029);
                lblOFFLINE.BackColor = System.Drawing.Color.Green;
                lblOFFLINE.ForeColor = System.Drawing.Color.White;
                panelOFFLINE.BackColor = System.Drawing.Color.Green;
            }
            else
            {
                // false: 背景 Red 文字色 Black 文字 OFFLINE
                lblOFFLINE.Text = Utility.GetMessageString(SystemConstants.SYSTEM_MSG, SystemConstants.SYSTEM_MSG030);
                lblOFFLINE.BackColor = System.Drawing.Color.Red;
                lblOFFLINE.ForeColor = System.Drawing.Color.Black;
                panelOFFLINE.BackColor = System.Drawing.Color.Red;
            }
            #endregion

            #region ステータスによってボタンの画像を変更
            CheckBtnAnd_ChangePicture(SystemConstants.STRIP1_BTN, btnSTRIP1, Alchemist.Properties.Resources.StripAON, Alchemist.Properties.Resources.StripAOFF);
            CheckBtnAnd_ChangePicture(SystemConstants.SEMISTRIP1_BTN, btnSEMISTRIP1, Alchemist.Properties.Resources.HalfAON, Alchemist.Properties.Resources.HalfAOFF);
            CheckBtnAnd_ChangePicture(SystemConstants.STRIP2_BTN, btnSTRIP2, Alchemist.Properties.Resources.StripBON, Alchemist.Properties.Resources.StripBOFF);
            CheckBtnAnd_ChangePicture(SystemConstants.SEMISTRIP2_BTN, btnSEMISTRIP2, Alchemist.Properties.Resources.HalfBON, Alchemist.Properties.Resources.HalfBOFF);
            #endregion

            // 加工動作の画面設定を行う
            workMotionDisplay();

            // センサー状態表示の更新を行う
            statusDisplayRefresh();

#if HCSM40
            #region ボタン状態表示
            // 動作モード
            CheckBtnAnd_ChangeColor(SystemConstants.NORMAL_BTN, btnNORMAL);
            CheckBtnAnd_ChangeColor(SystemConstants.EJECT_BTN, btnEJECT);
            CheckBtnAnd_ChangeColor(SystemConstants.SAMPLE_BTN, btnSAMPLE);
            CheckBtnAnd_ChangeColor(SystemConstants.TEST_BTN, btnTEST);
            CheckBtnAnd_ChangeColor(SystemConstants.FREE_BTN, btnFREE);
            
            // サイクルモード
            CheckBtnAnd_ChangeColor(SystemConstants.JOG_BTN, btnJOG);
            CheckBtnAnd_ChangeColor(SystemConstants.CYCLE_BTN, btnCYCLE);
            CheckBtnAnd_ChangeColor(SystemConstants.AUTO_BTN, btnAUTO);
            
            // 加工モード4
            CheckBtnAnd_ChangeColor(SystemConstants.LOT_INTERVAL1_BTN, btnAUTOEXIT);
            #endregion

            #region 数値表示更新
            // カウンター
            refreshControl(SystemConstants.TOTAL_COUNTER1, lblTOTAL2);
            refreshControl(SystemConstants.QTY_COUNTER1, lblQTY2);
            refreshControl(SystemConstants.LOT_COUNTER1, lblLOT2);
            refreshControl(SystemConstants.QTY_SET_COUNTER1, textQTY);
            refreshControl(SystemConstants.LOT_SET_COUNTER1, textLOT);
            
            // タクト
            refreshControl(SystemConstants.MACHINE_TACT1, lblTact4);
            
            // 加工値
            refreshControl(SystemConstants.WIRE_LENGTH1, textWIRE_LENGTH_VALUE);
            refreshControl(SystemConstants.STRIP_LENGTH1, textSTRIP1_VALUE);
            refreshControl(SystemConstants.STRIP_LENGTH2, textSTRIP2_VALUE);
            refreshControl(SystemConstants.STRIP_SEMI_LENGTH1, textSEMI_STRIP1_VALUE);
            refreshControl(SystemConstants.STRIP_SEMI_LENGTH2, textSEMI_STRIP2_VALUE);
            refreshControl(SystemConstants.STRIP_DEPTH1, textSTRIP_DEPTH_VALUE);
            refreshControl(SystemConstants.STRIP_PULLBACK1, textSTRIP_PULLBACK_VALUE);
            #endregion
#endif

#if OMOIKANE
            // CFMのティーチ状態を反映させる

            int omoikaneStatus = omoikane.SystemConstants.CFM_STATUS_NO_SHOW;
            if (Program.OmoikaneConnector.IsConnect())
                omoikaneStatus = Program.OmoikaneConnector.GetCFMStatusData();
            CheckBtnAnd_ChangeColor((omoikaneStatus == omoikane.SystemConstants.CFM_STATUS_TEACH), btnCFMTeach);
#endif

            int workMode = Program.DataController.GetWorkMode();

            #region 加工データ保存切り替え
            switch (workMode)
            {
                // バンク時の処理
                case SystemConstants.WORKMODE_BANK:
                    // 現在選択されているBankNoを取得
                    Program.DataController.BankNoRead(ref selectedNo);
                    // バンクコメントを取得
                    Program.DataController.BankDataCommentRead(selectedNo, ref bankComment);
                    // メイン画面のbankcommentを設定
                    if (!textBankComment.Focused) textBankComment.Text = bankComment;
                    // ボタンの色を変更する
                    CheckBtnAnd_ChangeColor(true, btnBANK);
                    CheckBtnAnd_ChangeColor(false, btnLEARN);
                    break;
                // 学習モード時の処理
                case SystemConstants.WORKMODE_LEARN:
                    // メイン画面のbankcommentを設定
                    if (!textBankComment.Focused) textBankComment.Text = "";
                    // ボタンの色を変更する
                    CheckBtnAnd_ChangeColor(false, btnBANK);
                    CheckBtnAnd_ChangeColor(true, btnLEARN);
                    break;
            }
            #endregion

            #region マシン操作設定が本体の場合は、操作ボタンは非表示にする。
            if (Program.SystemData.machineoperation == "machine")
            {
                // 操作画面を消す
                if (machineOperationForm.Visible != false)
                {
                    machineOperationForm.Visible = false;
                }

                // パネルも消す
                if (pnlMachineOperate.Visible != false)
                {
                    pnlMachineOperate.Visible = false;
                }
            }
            else
            {
                if (pnlMachineOperate.Visible != true)
                {
                    pnlMachineOperate.Visible = true;
                }
            }
            #endregion
        }

        private void tsmiVersion_Click(object sender, EventArgs e)
        {
            aboutboxForm.Show();
        }

        private void btnDetail_Click(object sender, EventArgs e)
        {
            counterForm.Show();
        }

        private void mainfrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = false;
        }

        private void btnINCHING1_Click(object sender, EventArgs e)
        {
            setupOperationForm.Show();
        }

        private void btnWIRE1_Detail_Click(object sender, EventArgs e)
        {
            workDetailItemFormwire1.Show();
        }

        private void btnSTRIP1_Detail_Click(object sender, EventArgs e)
        {
            workDetailItemFormstrip1.Show();
        }

        private void btnCRIMP1_Detail_Click(object sender, EventArgs e)
        {
            //workDetailItemFormcrimp1.Show();
        }

        private void btnSEAL1_Detail_Click(object sender, EventArgs e)
        {
            //workDetailItemFormseal1.Show();
        }

        private void btnSTRIP2_Detail_Click(object sender, EventArgs e)
        {
            workDetailItemFormstrip2.Show();
        }

        private void btnCRIMP2_Detail_Click(object sender, EventArgs e)
        {
            //workDetailItemFormcrimp2.Show();
        }

        private void btnSEAL2_Detail_Click(object sender, EventArgs e)
        {
            //workDetailItemFormseal2.Show();
        }

        private void btnSpeedsetting_Click(object sender, EventArgs e)
        {
            workDetailSpeedForm.Show();
        }

        private void btnWorkMotion_Click(object sender, EventArgs e)
        {
            workDetailMotionForm.Show();
        }

        private void btnOperation_Click(object sender, EventArgs e)
        {
            machineOperationForm.Show();
        }

        private void btnBANK_Click(object sender, EventArgs e)
        {
            bankOperationForm.Show();
        }

        private void btnDataBase_Click(object sender, EventArgs e)
        {
            databaseForm.Show();
        }

        private void btnManagement_setting_Click(object sender, EventArgs e)
        {
            // パスワードが一致したら、システム設定を開く
            passwordCollationfrm pass = new passwordCollationfrm();

            // パスワードが空の場合表示
            if (string.IsNullOrEmpty(Program.SystemData.password))
            {
                systemConfigrationForm.Show();
            }
            // パスワードが設定されている場合
            else
            {
                // パスワード入力画面で照合を選んだ場合
                if (pass.ShowDialog() == DialogResult.OK)
                {
                    // パスワードが一致
                    if (pass.CheckPassword() == true)
                    {
                        systemConfigrationForm.Show();
                    }
                    // パスワードが不一致
                    else
                    {
                        Utility.ShowErrorMsg(SystemConstants.SYSTEM_MSG017);
                    }
                }
            }
        }

        private void lblOFFLINE_Click(object sender, EventArgs e)
        {
            connectOperationForm.Show();
        }

        private void workMotionDisplay()
        {
            int key1 = 0;
            int key2 = 0;
            int btnStatus = 0;

            //ハッシュテーブルのkey値算出方法
            //下記の表に対応した値を取得
            //
            //  100              10              1
            //　ハーフ　 ｜ 　　　　　 ｜ 1側or　　　　  ｜　　　｜
            //ストリップ ｜ ストリップ ｜ 2側(2側だと○) ｜2進数 ｜10進数
            // ―――――｜――――――｜――――――――｜―――｜―――
            //　　　　　 ｜ 　　　　　 ｜　　　　　　　　｜00000 ｜  0
            //　　　　　 ｜ 　　　　　 ｜  　　 ○ 　　　｜00001 ｜  1
            //　　　　　 ｜ 　　○　　 ｜  　　　　　　　｜00010 ｜  2
            //　　　　　 ｜ 　　○　　 ｜  　　 ○ 　　　｜00011 ｜  3
            //　　○　　 ｜ 　　○　　 ｜  　　　　　　　｜00110 ｜  6
            //　　○　　 ｜ 　　○　　 ｜  　　 ○ 　　　｜00111 ｜  7
            
            // 1側のkey値算出
            for (int i = 0; i < btnIdArray1.Length; i++)
            {
                Program.DataController.ReadPushBtn(btnIdArray1[i], ref btnStatus);
                if (btnStatus == SystemConstants.BTN_ON)
                {
                    key1 = key1 + (2 << i);
                }
            }

            // 2側のkey値算出
            for (int i = 0; i < btnIdArray2.Length; i++)
            {
                Program.DataController.ReadPushBtn(btnIdArray2[i], ref btnStatus);
                if (btnStatus == SystemConstants.BTN_ON)
                {
                    key2 = key2 + (2 << i);
                }
            }
            // key2に+1する(2側のkey値は1側の+1)
            key2 = key2 + 1;

            try
            {
                pictureBoxSIDE1.Image = map[key1].image;                                    // 1側の画像を設定
                lblWIRE_LENGTH_VALUE.Visible = map[key1].wire_Length;                       // 切断長ラベルのVisibleを設定
                textWIRE_LENGTH_VALUE.Visible = map[key1].wire_Length;                      // 切断長テキストのVisibleを設定
                lblSTRIP1_VALUE.Visible = map[key1].strip_Length;                           // 1側のストリップラベルのVisibleを設定
                textSTRIP1_VALUE.Visible = map[key1].strip_Length;                          // 1側のストリップテキストのVisibleを設定
                lblSEMI_STRIP1_VALUE.Visible = map[key1].semi_Strip;                        // 1側のハーフストリップラベルのVisibleを設定
                textSEMI_STRIP1_VALUE.Visible = map[key1].semi_Strip;                       // 1側のハーフストリップテキストのVisibleを設定
                lblCRIMP_HEIGHT1_VALUE.Visible = map[key1].crimp_Height;                    // 1側のCHラベルのVisibleを設定
                textCRIMP_HEIGHT1_VALUE.Visible = map[key1].crimp_Height;                   // 1側のCHテキストのVisibleを設定
                lblCRIMP_POSITION1_VALUE.Visible = map[key1].crimp_Position;                // 1側の圧着位置ラベルのVisibleを設定
                textCRIMP_POSITION1_VALUE.Visible = map[key1].crimp_Position;               // 1側の圧着位置テキストのVisibleを設定
                lblSEAL_INSERT_LENGTH1_VALUE.Visible = map[key1].seal_Insert_Length;        // 1側の挿入量ラベルのVisibleを設定
                textSEAL_INSERT_LENGTH1_VALUE.Visible = map[key1].seal_Insert_Length;       // 1側の挿入量テキストのVisibleを設定
                lblSEAL_INSERT_BACK1_VALUE.Visible = map[key1].seal_Insert_Back;            // 1側の戻し量ラベルのVisibleを設定
                textSEAL_INSERT_BACK1_VALUE.Visible = map[key1].seal_Insert_Back;           // 1側の戻し量テキストのVisibleを設定

                pictureBoxSIDE2.Image = map[key2].image;                                    // 2側の画像を設定
                lblWIRE_LENGTH_VALUE.Visible = map[key2].wire_Length;                       // 切断長ラベルのVisibleを設定
                textWIRE_LENGTH_VALUE.Visible = map[key2].wire_Length;                      // 切断長テキストのVisibleを設定
                lblSTRIP2_VALUE.Visible = map[key2].strip_Length;                           // 2側のストリップラベルのVisibleを設定
                textSTRIP2_VALUE.Visible = map[key2].strip_Length;                          // 2側のストリップテキストのVisibleを設定
                lblSEMI_STRIP2_VALUE.Visible = map[key2].semi_Strip;                        // 2側のハーフストリップラベルのVisibleを設定
                textSEMI_STRIP2_VALUE.Visible = map[key2].semi_Strip;                       // 2側のハーフストリップテキストのVisibleを設定
                lblCRIMP_HEIGHT2_VALUE.Visible = map[key2].crimp_Height;                    // 2側のCHラベルのVisibleを設定
                textCRIMP_HEIGHT2_VALUE.Visible = map[key2].crimp_Height;                   // 2側のCHテキストのVisibleを設定
                lblCRIMP_POSITION2_VALUE.Visible = map[key2].crimp_Position;                // 2側の圧着位置ラベルのVisibleを設定
                textCRIMP_POSITION2_VALUE.Visible = map[key2].crimp_Position;               // 2側の圧着位置テキストのVisibleを設定
                lblSEAL_INSERT_LENGTH2_VALUE.Visible = map[key2].seal_Insert_Length;        // 2側の挿入量ラベルのVisibleを設定
                textSEAL_INSERT_LENGTH2_VALUE.Visible = map[key2].seal_Insert_Length;       // 2側の挿入量テキストのVisibleを設定
                lblSEAL_INSERT_BACK2_VALUE.Visible = map[key2].seal_Insert_Back;            // 2側の戻し量ラベルのVisibleを設定
                textSEAL_INSERT_BACK2_VALUE.Visible = map[key2].seal_Insert_Back;           // 2側の戻し量テキストのVisibleを設定

                // 1側、2側でストリップボタンの状態で、どちらかが表示になっていた場合、表示にする
                lblSTRIP_DEPTH_VALUE.Visible = map[key1].strip_Depth || map[key2].strip_Depth;  // 切込深さラベルのVisibleを設定
                textSTRIP_DEPTH_VALUE.Visible = map[key1].strip_Depth || map[key2].strip_Depth; // 切込深さテキストのVisibleを設定
                lblSTRIP_PULLBACK_VALUE.Visible = map[key1].strip_Pullback || map[key2].strip_Pullback; // プルバックラベルのVisibleを設定
                textSTRIP_PULLBACK_VALUE.Visible = map[key1].strip_Pullback || map[key2].strip_Pullback;    // プルバックテキストのVisibleを設定
            }
            catch
            {
                /* 例外を無視（データの取得タイミングで本来とりうるはずのない組み合わせがきた場合の対策） */
            }

            /*
            if ((key1 & 0x0002) == 0 && (key2 & 0x0002) == 0)
            {
                lblSTRIP_DEPTH_VALUE.Visible = false;                                   // 切込深さラベルのVisibleを設定
                textSTRIP_DEPTH_VALUE.Visible = false;                                  // 切込深さテキストのVisibleを設定
                lblSTRIP_PULLBACK_VALUE.Visible = false;                                // プルバックラベルのVisibleを設定
                textSTRIP_PULLBACK_VALUE.Visible = false;                               // プルバックテキストのVisibleを設定
            }
            // 1側、2側のどちらかがストリップボタンがOFFだった場合
            else
            {
                lblSTRIP_DEPTH_VALUE.Visible = true;                                    // 切込深さラベルのVisibleを設定
                textSTRIP_DEPTH_VALUE.Visible = true;                                   // 切込深さテキストのVisibleを設定
                lblSTRIP_PULLBACK_VALUE.Visible = true;                                 // プルバックラベルのVisibleを設定
                textSTRIP_PULLBACK_VALUE.Visible = true;                                // プルバックテキストのVisibleを設定
            }
            */
        }

        /// <summary>
        /// センサ状態、動作表示更新用変数
        /// </summary>
        private int statusDspSensorPage = -1;
        private int statusDspMotionPage = -1;
        private int statusDspSensorPgMax = 0;
        private int statusDspMotionPgMax = 0;
        private DateTime statusDisplayedTime = DateTime.Now;
        /// <summary>
        /// センサ状態、動作状態表示を更新する
        /// </summary>
        private void statusDisplayRefresh()
        {
            // 一定時間経過後に、ページを切替える
            TimeSpan ts = DateTime.Now - statusDisplayedTime;
            if (ts.TotalSeconds > SystemConstants.STATUS_DISPLAY_REFRESH_TIME)
            {
                statusDspSensorPage++;
                statusDspMotionPage++;
                statusDisplayedTime = DateTime.Now;
            }

            int dspPage = 0;

            // センサー項目を表示する
            Label[] senLbls = new Label[]{
                lblSensorCheck00,lblSensorCheck01,
                lblSensorCheck02,lblSensorCheck03,
                lblSensorCheck04,lblSensorCheck05,
                lblSensorCheck06,lblSensorCheck07,
                lblSensorCheck08,lblSensorCheck09,
                lblSensorCheck10,lblSensorCheck11,
                lblSensorCheck12,lblSensorCheck13,
                lblSensorCheck14,lblSensorCheck15};


            /* 表示ページがオーバしている場合、ページを0に戻す
             * 変数を追加しているのは、Max値チェック後 - 表示までの間に手動でページ値が更新された場合、
             * 例外エラーを発生させてしまう為、その対策。
             */
            dspPage = statusDspSensorPage;

            while (dspPage <= statusDspSensorPgMax)
            {
                if (existsStatusDataInMap(dspPage, SystemConstants.SENSOR_STATUS_BOX0, senLbls.Length))
                {
                    statusDspSensorPage = dspPage;
                    break;
                }
                else
                    dspPage++;
            }

            if (dspPage > statusDspSensorPgMax)
            {
                dspPage = 0;
                statusDspSensorPage = 0;
            }

            for (int i = 0; i < senLbls.Length; i++)
            {
                int index = dspIndexCalc(dspPage, i);
                statusDspUpdate(index, senLbls[i]);
            }

            // 動作状態表示項目を表示する
            Label[] motionLbls = new Label[]{
                lblWorkMotion00, lblWorkMotion01,
                lblWorkMotion02, lblWorkMotion03,
                lblWorkMotion04, lblWorkMotion05,
                lblWorkMotion06, lblWorkMotion07,
                lblWorkMotion08, lblWorkMotion09,
                lblWorkMotion10, lblWorkMotion11};

            /* 表示ページがオーバしている場合、ページを0に戻す
             * 変数を追加しているのは、Max値チェック後 - 表示までの間に手動でページ値が更新された場合、
             * 例外エラーを発生させてしまう為、その対策。
             */
            dspPage = statusDspMotionPage;

            while (dspPage <= statusDspMotionPgMax)
            {
                if (existsStatusDataInMap(dspPage, SystemConstants.MOTION_STATUS_BOX0, motionLbls.Length))
                {
                    statusDspMotionPage = dspPage;
                    break;
                }
                else
                    dspPage++;
            }

            if (dspPage > statusDspMotionPgMax)
            {
                dspPage = 0;
                statusDspMotionPage = 0;
            }

            for (int i = 0; i < motionLbls.Length; i++)
            {
                int index = dspIndexCalc(statusDspMotionPage, SystemConstants.MOTION_STATUS_BOX_START + i);
#if FOR_Y
                statusDspUpdate(index, motionLbls[i], dspPage);
#else
                statusDspUpdate(index, motionLbls[i]);
#endif
            }
        }

        private bool existsStatusDataInMap(int pg, int startPosIndex, int count)
        {
            bool exists = false;

            int index = 0;
            for (int i = 0; i < count; i++)
            {
                index = dspIndexCalc(pg, startPosIndex);
                if (statusDspMap.ContainsKey(index))
                {
                    exists = true;
                    break;
                }
            }

            return exists;
        }

        private void statusDspUpdate(int index, Control Ctl, bool ReverseFlg = false)
        {
            // 値を取得する
            statusDspStruct dspData = new statusDspStruct();
            if (statusDspMap.TryGetValue(index, out dspData))
            {
                // senLbls[i].Visible = true;
                refreshControl(SystemConstants.STATUS_DISPLAY_MSG, dspData.MsgID, Ctl);
                CheckBtnAnd_ChangeColor(dspData.BtnID, Ctl, ReverseFlg);
            }
            else
            {
                refreshControl("", Ctl);
                CheckBtnAnd_ChangeColor(false, Ctl, ReverseFlg);
                // senLbls[i].Visible = false;
            }

        }

        /// <summary>
        /// Y仕様エンド端子選択時のアプリ選択ボタン状態非表示
        /// </summary>
        /// <param name="index"></param>
        /// <param name="Ctl"></param>
        /// <param name="dspPage"></param>
        /// <param name="ReverseFlg"></param>
#if !HCSM40        
        private void statusDspUpdate(int index, Control Ctl, int dspPage, bool ReverseFlg = false)
        {
            if (dspPage == 0)
            {
                if((CheckBtnOnReturnTrue(SystemConstants.END_TERM1_BTN) && (index == SystemConstants.MOTION_STATUS_BOX4 || index == SystemConstants.MOTION_STATUS_BOX6)) ||
                   (CheckBtnOnReturnTrue(SystemConstants.END_TERM2_BTN) && (index == SystemConstants.MOTION_STATUS_BOX5 || index == SystemConstants.MOTION_STATUS_BOX7)))
                {
                    refreshControl("", Ctl);
                    CheckBtnAnd_ChangeColor(false, Ctl, ReverseFlg);
                    return;
                }                
            }

            // 値を取得する
            statusDspStruct dspData = new statusDspStruct();
            if (statusDspMap.TryGetValue(index, out dspData))
            {
                // senLbls[i].Visible = true;
                refreshControl(SystemConstants.STATUS_DISPLAY_MSG, dspData.MsgID, Ctl);
                CheckBtnAnd_ChangeColor(dspData.BtnID, Ctl, ReverseFlg);
            }
            else
            {
                refreshControl("", Ctl);
                CheckBtnAnd_ChangeColor(false, Ctl, ReverseFlg);
                // senLbls[i].Visible = false;
            }

        }
#endif

        private void panel8_Click(object sender, EventArgs e)
        {
            ActiveControl = null;
        }

        private void pictureBoxSIDE1_Click(object sender, EventArgs e)
        {
            ActiveControl = null;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ActiveControl = null;
        }

        private void pictureBoxSIDE2_Click(object sender, EventArgs e)
        {
            ActiveControl = null;
        }

        /// <summary>
        /// バンクコメント入力
        /// </summary>
        /// <param name="e"></param>
        private void textBankComment_EnterKeyDown(EventArgs e)
        {
            int selectedNo = 0;

            // 現在選択されているバンクNoを取得
            Program.DataController.BankNoRead(ref selectedNo);

            // メインフォームのバンクコメントをbankdata.xmlに書き込む
            mainfrm.BankDataCommentWrite(selectedNo, textBankComment.Text);

            // フォーカスを外す
            ActiveControl = null;
        }

        /// <summary>
        /// CFM画面からメイン画面へ戻る
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mainfrm_Shown(object sender, EventArgs e)
        {
#if OMOIKANE
            if (Program.OmoikaneConnector.IsConnect())
            {
                omoikane.Cmd004Struct cmd004Struct;
                Program.OmoikaneConnector.GetCFMStatus(out cmd004Struct);
            }
#endif
        }

        /// <summary>
        /// CFM画面表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCFMShow_Click(object sender, EventArgs e)
        {
#if OMOIKANE
            omoikaneForm.Show();
#endif
        }

        /// <summary>
        /// ティーチングを設定する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCFMTeach_Click(object sender, EventArgs e)
        {
#if OMOIKANE
            Program.OmoikaneConnector.SetCFMStatus(omoikane.SystemConstants.CFM_STATUS_TEACH);
#endif
        }

        /// <summary>
        /// センサー状態のページ更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSensorNext_Click(object sender, EventArgs e)
        {
            statusDspSensorPage++;
            statusDisplayedTime = DateTime.Now;
        }

        /// <summary>
        /// 作業ボタン状態のページ更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWorkMotionNext_Click(object sender, EventArgs e)
        {
            statusDspMotionPage++;
            statusDisplayedTime = DateTime.Now;
        }

        /// <summary>
        /// メニュー→操作終了でプログラムを終了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiShutDown_Click(object sender, EventArgs e)
        {
            Program.MainForm.Close();
        }

    }
}