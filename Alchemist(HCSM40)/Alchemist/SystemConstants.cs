using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alchemist
{
    public class SystemConstants
    {
        #region  設定定数
        public const string MUTEX_NAME = "Global\\ALCHEMIST";   /* MUTEXの名前 */

        public const int BORATE_4800 = 1;	            /* ComConfigureで使用する定数。ポーレート4800bpsを意味する。 */
        public const int BORATE_9600 = 2;	            /* ComConfigureで使用する定数。ポーレート9600bpsを意味する。 */
        public const int BORATE_14400 = 3;	            /* ComConfigureで使用する定数。ポーレート14400bpsを意味する。 */
        public const int BORATE_19200 = 4;	            /* ComConfigureで使用する定数。ポーレート19200bpsを意味する。 */
        public const int BORATE_38400 = 5;	            /* ComConfigureで使用する定数。ポーレート38400bpsを意味する。 */
        public const int BORATE_57600 = 6;	            /* ComConfigureで使用する定数。ポーレート57600bpsを意味する。 */
        public const int BORATE_115200 = 7;	            /* ComConfigureで使用する定数。ポーレート115200bpsを意味する。 */
        public const int DATABIT_7 = 1;	                /* ComConfigureで使用する定数。データビット 7を意味する */
        public const int DATABIT_8 = 2;	                /* ComConfigureで使用する定数。データビット 8を意味する */
        public const int EVENT_CONNECT = 1;	            /* ConnectStatusChangeのEventCodeの定数。通信開始を意味する */
        public const int EVENT_DISCONNECT = 2;	        /* ConnectStatusChangeのEventCodeの定数。通信切断を意味する */
        public const int FLOW_NONE = 1;	                /* ComConfigureで使用する定数。フロー制御 無手順を意味する */
        public const int FLOW_HARD = 2;	                /* ComConfigureで使用する定数。フロー制御 ハードウェアを意味する */
        public const int FLOW_XONXOFF = 3;	            /* ComConfigureで使用する定数。フロー制御 XON/XOFFを意味する */
        public const int PARITY_NONE = 1;	            /* ComConfigureで使用する定数。パリティビット無しを意味する */
        public const int PARITY_ODD = 2;	            /* ComConfigureで使用する定数。パリティビット奇数を意味する */
        public const int PARITY_EVEN = 3;	            /* ComConfigureで使用する定数。パリティビット偶数を意味する */
        public const int STOPBIT_1 = 1;	                /* ComConfigureで使用する定数。ストップビット1を意味する */
        public const int STOPBIT_2 = 2;	                /* ComConfigureで使用する定数。ストップビット1を意味する */
        public const int BTN_PUSH = 2;	                /* 操作ボタンが押された事を意味する */
        public const int BTN_ON = 1;	                /* 操作ボタンがONを意味する */
        public const int BTN_OFF = 0;	                /* 操作ボタンがOFFを意味する */
        public const int BTN_BANK_SAVED = 1;            /* バンク保存対象ボタンを意味する */
        public const int BTN_BANK_UNSAVED = 0;          /* バンク保存対象外ボタンを意味する    */
        public const int ERR_MSG_INFORMATION = 5;	    /* エラーメッセージのうち、情報を意味する */
        public const int ERR_MSG_ERROR = 6;     	    /* エラーメッセージのうち、エラーを意味する */
        public const int FORMAT_DONE = 1;	            /* 機械が設定済を意味する値 */
        public const int FORMAT_NEED = 2;	            /* 機械が未設定を意味する値 */
        public const int MACHINE_CONNECT = 1;	        /* 機械と通信状態を意味する */
        public const int MACHINE_DISCONNECT = 2;        /* 機械と切断状態を意味する */
        public const int WORKMEM_TYPE_WORKBTN = 3;	    /* BankData Typeの三次元配列のボタンを意味する値 */
        public const int WORKMEM_TYPE_WORKDATA = 4;     /* BankData Typeの三次元配列の加工値を意味する値 */
        public const int WORKMEM_TYPE_WORKCOMMENT = 5;	/* BankData Typeの三次元配列のコメントを意味する値 */
        public const int WORKGROUP_ROOT = 0;	        /* GroupIndexのルートを意味する */
        public const int WORKMODE_BANK = 0;	            /* バンクモードを意味する値 */
        public const int WORKMODE_LEARN = 1;            /* データモードが学習データ */
        public const int ERROR_MSG = 1;         	    /* エラーメッセージを意味する */
        public const int SYSTEM_MSG = 2;	            /* システムメッセージを意味する */
        public const int WORK_MSG = 3;					/* 加工値メッセージを意味する */
        public const int CORRECT_MSG = 4;	            /* 補正値メッセージを意味する */
        public const int TIMMING_MSG = 5;	            /* タイミングメッセージを意味する */
        public const int MAINTELOG_MSG = 6;             /* 保全履歴メッセージを意味する   */
        public const int IO_ASSIGN_MSG = 7;             /* IOのアサインメッセージである事を意味する  */
        public const int STATUS_DISPLAY_MSG = 8;        /* センサ、動作状態表示メッセージである事を意味する    */
        public const int WORKID_TYPE_WORKDATA = 1;      /* typeが加工値であること意味する */
        public const int WORKID_TYPE_CORRECTDATA = 2;   /* typeが補正値であること意味する */
        public const int WORKID_TYPE_TIMINGDATA = 3;    /* typeがタイミングであること意味する */
        public const int INFOMATION_REDISPLAY_INTERVAL = 30000;     /* 情報ウインドウの再表示間隔 */
        public const int ERR_PICTURE_REFRESH_INTERVAL = 3000;       /* エラー画像の更新間隔 */
        public const int BANK_MAX = 500;	            /* バンク登録件数 */
        public const bool TENKEY_OFF = false;           /* テンキー使用無を意味する */
        public const bool TENKEY_ON = true;             /* テンキー使用有を意味する */
        public const int TENKEY_INPUT_ONLY = 0;         /* テンキーからの入力値をメモリーに反映しない */
        public const int TENKEY_INPUT_DATA = 1;         /* テンキーからの入力値をメモリーに反映する */
        public const int TENKEY_INPUT_CHECK = 2;        /* テンキーからの入力値を範囲チェックだけに使う */
        public const int TENKEY_INPUT_WIRETHRES = 3;    /* テンキーからの入力値で切断長閾値の設定に使う */
        public const int LEARN_ITEM_WIRETYPE = 1;       /* 学習データ項目が電線種類を意味する */
        public const int LEARN_ITEM_CORESIZE = 2;       /* 学習データ項目がコアサイズを意味する */
        public const int LEARN_ITEM_COLOR1 = 3;         /* 学習データ項目が色１を意味する */
        public const int LEARN_ITEM_COLOR2 = 4;         /* 学習データ項目が色２を意味する */
        #endregion

        #region 加工値 WORKID
#if HCSM40
        // 切断長
        public const int WIRE_LENGTH1 = 0x0101;
        public const int WIRE_ROLLER_PRS_LV = 0x0102;
        public const int WIRE_STRAIGHTNER_PRS_LV = 0x0103;
		
        // ストリップ1
        public const int STRIP_LENGTH1 = 0x0201;
		public const int STRIP_SEMI_LENGTH1 = 0x0202;
		public const int STRIP_DEPTH1 = 0x0203;
		public const int STRIP_PULLBACK1 = 0x0204;
        public const int STRIP_BACK_SPEED1 = 0x0205;
		
        // ストリップ2
        public const int STRIP_LENGTH2 = 0x0301;
		public const int STRIP_SEMI_LENGTH2 = 0x0302;
        public const int STRIP_DEPTH2 = 0x0203;
        public const int STRIP_PULLBACK2 = 0x0304;        
		public const int STRIP_BACK_SPEED2 = 0x0305;
		
        // カウンタ
        public const int TOTAL_COUNTER1 = 0x0401;
        public const int QTY_COUNTER1 = 0x0402;
        public const int LOT_COUNTER1 = 0x0403;
        public const int QTY_SET_COUNTER1 = 0x0404;
        public const int LOT_SET_COUNTER1 = 0x0405;
		public const int MACHINE_TACT1 = 0x0406;
		
        // 速度
        public const int LOT_INTERVAL1 = 0x0501;
		
        // 速度（切断長別）
        public const int WIRE_LENGTH_CORRECT1 = 0x0601;
        public const int WIRE_LENGTH_CORRECT2 = 0x0602;
        public const int WIRE_LENGTH_CORRECT3 = 0x0603;
        public const int FEED_SPEED1 = 0x0604;
		public const int FEED_SPEED2 = 0x0605;
        public const int FEED_SPEED3 = 0x0606;
        public const int FEED_ACCEL1 = 0x0607;
        public const int FEED_ACCEL2 = 0x0608;
		public const int FEED_ACCEL3 = 0x0609;
		public const int FEED_SPEED_THRES1 = 0x060A;
		public const int FEED_SPEED_THRES2 = 0x060B;        
#endif
        #endregion

        #region ボタンWORKID
#if HCSM40
        // 動作モード
        public const int NORMAL_BTN = 0x0101;                   /* 標準 */
        public const int EJECT_BTN = 0x0102;                    /* 排出 */
        public const int TEST_BTN = 0x0103;                     /* 試行 */
        public const int FREE_BTN = 0x0104;                     /* 空運転 */
        public const int SAMPLE_BTN = 0x0105;                   /* サンプル */
        
        // サイクルモード
        public const int JOG_BTN = 0x0201;                      /* ジョグ */
        public const int CYCLE_BTN = 0x0202;                    /* 単動 */
        public const int AUTO_BTN = 0x0203;                     /* 連動 */

        // 加工モード1
        public const int STRIP1_BTN = 0x0301;                   /* ストリップ1 */
        public const int SEMISTRIP1_BTN = 0x0302;               /* ハーフストリップ1 */

        // 加工モード2
        public const int STRIP2_BTN = 0x0401;                   /* ストリップ2 */
        public const int SEMISTRIP2_BTN = 0x0402;               /* ハーフストリップ2 */

        // 加工モード3

        // 加工モード4            
        public const int LOT_INTERVAL1_BTN = 0x0601;            /* 自動復帰 */
        public const int DOUBLE_MOTION_BTN = 0x0602;            /* ２段動作 */
        public const int OUTPUT_BTN = 0x0603;                   /* 外部出力 */

        // 段取り            
        public const int LOAD1_BTN = 0x0701;                    /* 電線ロード */
        public const int LOAD1_STATUS = 0x0702;                 /* 電線ロード状態 */

        // 操作            
        public const int MACHINE_START1_BTN = 0x0801;           /* スタート1 */
        public const int MACHINE_STOP1_BTN = 0x0803;            /* ストップ */
        public const int MACHINE_RESET1_BTN = 0x0804;           /* リセット */

        // カウンタ
        public const int TOTAL_COUNTER_RESET1_BTN = 0x0901;     /* トータルカウンタリセット */
        public const int QTY_COUNTER_RESET1_BTN = 0x0902;       /* QTYカウンタリセット */
        public const int LOT_COUNTER_RESET1_BTN = 0x0903;       /* LOTカウンタリセット */
        public const int COUNT_UP_BTN = 0x0904;                 /* カウントアップ */
        public const int COUNT_DOWN_BTN = 0x0905;               /* カウントダウン */

        // センサ
        public const int STRIP1_SENSOR_LOCK = 0x0A01;           /* ストリップミス1ロック */
#endif
        #endregion

        #region グループ定数
#if HCSM40
        // 加工値
        public const int WORK_GROUP_WIRE1 = 1;
		public const int WORK_GROUP_STRIP1 = 2;
		public const int WORK_GROUP_STRIP2 = 3;
		public const int WORK_GROUP_COUNTER1 = 4;
		public const int WORK_GROUP_SPEED1 = 5;
		public const int WORK_GROUP_SPEED1_HIDE = 6;

        // ボタン
		public const int BTN_GROUP_ACTION1 = 1;
		public const int BTN_GROUP_CYCLEMODE1 = 2;
		public const int BTN_GROUP_WORKMODE1 = 3;
		public const int BTN_GROUP_WORKMODE2 = 4;
		public const int BTN_GROUP_WORKMODE3 = 5;
		public const int BTN_GROUP_WORKMODE4 = 6;
		public const int BTN_GROUP_SETUPOPERATION1 = 7;
		public const int BTN_GROUP_OPERATE1 = 8;
		public const int BTN_GROUP_COUNTER1 = 9;
		public const int BTN_GROUP_SENSOR = 10;
        public const int BTN_GROUP_ALARM = 11;

        // 補正値
		public const int CORR_GROUP_BASEMACHINE1 = 1;
		public const int CORR_GROUP_FEED1 = 2;
		public const int CORR_GROUP_POSITION1 = 3;
		public const int CORR_GROUP_CUTSTRIP1 = 4;
		
        // タイミング
		public const int TIMM_GROUP_SIDE1 = 1;
		public const int TIMM_GROUP_SIDE2 = 2;		
#endif
        #endregion

        #region 返値定数
        // 処理
        public const int MCPF_SUCCESS = 0x0100;	                    /* 処理が正常に行われた場合の返り値 (MachineConnector Public Function) */
        public const int ERR_ADDRESS_RANGE = 0x0101;	            /* アドレス範囲外 */
        public const int ERR_MEMORY_RANGE = 0x0102; 	            /* メモリ値範囲外 */
        public const int ERR_TRANSFAR_RANGE = 0x0103;	            /* 送信数範囲外 */
        public const int ERR_PORTOPEN = 0x0104;	                    /* COMポートオープンエラー */
        public const int ERR_OPENTEST = 0x0105;	                    /* テスト通信エラー */
        public const int ERR_TIMEOUTRANGE = 0x0106;	                /* タイムアウト範囲外 */
        public const int ERR_POLLINGRANGE = 0x0107;	                /* ポーリング間隔範囲外 */
        public const int ERR_ILLEGALSTATUS = 0x0108;	            /* 不正状態 */
        // 総合データ管理
        public const int DCPF_SUCCESS = 0x0200;	                    /* 処理が正常に行われた場合の返り値 (DataConroller Public Function) */
        public const int ERR_NO_WORK_ID = 0x0201;	                /* 加工値ID無し */
        public const int ERR_WORK_RANGE = 0x0202;	                /* 加工値範囲外 */
        public const int ERR_EXCLUSION_ID = 0x0203;                 /* 除外項目 */
        //        public const int ERR_ILLEGALSTATUS = 0x0203;	    /* 不正状態 */
        public const int ERR_BANK_PARTS_BREAK = 0x0204;	            /* 欠損バンクデータ */
        public const int ERR_BANK_PARTS_RANGE = 0x0205;	            /* バンクデータ値範囲外 */
        public const int ERR_WORKID_RANGE = 0x0206;	                /* WorkType, WorkID範囲外 */
        public const int ERR_LEARN_PARTS_BREAK = 0x0207;            /* 欠損学習データ */
        public const int ERR_LEARN_PARTS_RANGE = 0x0208;            /* 学習データ値範囲外 */
        // バンクデータ管理
        public const int BSPF_SUCCESS = 0x0300;	                    /* 処理が正常に行われた場合の返り値 (BankDataStorage Public Function) */
        public const int ERR_NO_BANK_DATA = 0x0301;	                /* バンクデータが無い場合の返り値 */
        public const int ERR_BANKNO_RANGE = 0x0302;                 /* バンクナンバー範囲外 */
        public const int ERR_UNINITIALIZED = 0x0303;                /* BankDataStorageが初期化されていない */
        public const int ERR_BANK_FILE_ERROR = 0x304;               /* BANKファイルが書き込めない */
        public const int ERR_CORRECT_FILE_ERROR = 0x305;            /* BANKファイルが書き込めない */
        public const int ERR_MEMALLOC_FILE_ERROR = 0x306;           /* BANKファイルが書き込めない */
        // 同期管理
        public const int ERR_SYNC_CANCELLED = 0x401;                /* 同期中にキャンセルされた */
        public const int ERR_SYNC_TIMEOUT = 0x402;                  /* 同期中にタイムアウト */
        // 学習データ管理
        public const int LSPF_SUCCESS = 0x0500;                     /* 処理が正常に行われた場合の返値 (LearnDataStorage Public Function) */
        public const int ERR_LSPF_UNINITIALIZED = 0x0501;           /* LearnDataStorageが初期化されていない */
        public const int ERR_LSPF_NO_LEARN_DATA = 0x0502;           /* 学習データが無い場合の返値 */
        public const int ERR_LSPF_FILE_ERROR = 0x0503;              /* 学習データのファイルアクセスの失敗 */
        // メンテ管理
        public const int RLPF_SUCCESS = 0x0600;                     /* 保全履歴機能が正常に処理された場合の返り値 */
        public const int ERR_RLPF_UNINITIALIZED = 0x0601;           /* 保全履歴機能が初期化されていない場合の返り値  */
        public const int ERR_RLPF_FILE_ERROR = 0x0602;              /* 保全履歴機能でファイルアクセス失敗   */
        // 学習データ項目管理
        public const int LIPF_SUCCESS = 0x0700;                     /* 正常に処理された */
        public const int ERR_LIPF_UNINITIALIZED = 0x0701;           /* 初期化されていない */
        public const int ERR_LIPF_FILE_ERROR = 0x0702;              /* ファイルアクセスの失敗 */
        public const int ERR_LIPF_NO_DATA = 0x0703;                 /* データが登録されていない */
        #endregion        

        #region 学習データ定数

        #region 学習データ検索グループ定数
        public const int DB_GROUP_SEARCH_TYPE_NONE = 0;             //  検索対象外
        public const int DB_GROUP_SEARCH_TYPE_WIRE1 = 1;            //  電線種類, コアサイズ, 色1, 色2
        public const int DB_GROUP_SEARCH_TYPE_WIRELENGTH1 = 2;      //  電線種類, コアサイズ, 色1, 色2, 切断長
        public const int DB_GROUP_SEARCH_TYPE_STRIPLENGTH1 = 3;     //  電線名, 電線サイズ, 端子名, 防水栓名, ストリップ長
        public const int DB_GROUP_SEARCH_TYPE_STRIPLENGTH2 = 4;     //  電線名, 電線サイズ, 端子名, 防水栓名, ストリップ長
        public const int DB_GROUP_SEARCH_TYPE_BTN = 99;             //  ボタンを示す定数。検索には使用しないが、ボタンの加工値適用の為に使用
        #endregion

        #region 検索キーコード
        public const int DB_GROUP_KEY_WIRENAME = 0;         //  電線種類
        public const int DB_GROUP_KEY_WIRESIZE = 1;         //  コアサイズ
        public const int DB_GROUP_KEY_WIRECOLOR1 = 2;       //  色1
        public const int DB_GROUP_KEY_WIRECOLOR2 = 3;       //  色2
        public const int DB_GROUP_KEY_WIRELENGTH1 = 4;      //  切断長
        public const int DB_GROUP_KEY_STRIPLENGTH1 = 5;     //  ストリップ長1
        public const int DB_GROUP_KEY_STRIPLENGTH2 = 6;     //  ストリップ長2
        #endregion

        #region その他
        public const int DB_GROUP_WORKSIDE1 = 1;
        public const int DB_GROUP_WORKSIDE2 = 2;
        #endregion

        #endregion

        #region 保全履歴機能定数
        public const int MAINTELOG_RECORD_TYPE_WORKID = 1;
        public const int MAINTELOG_RECORD_TYPE_COMMENT = 2;
        #endregion

        #region メッセージコード定数

        #region エラーメッセージ
        public const int ERR_MSG000 = 0x0000;
        public const int ERR_MSG001 = 0x0001;
        public const int ERR_MSG002 = 0x0002;
        public const int ERR_MSG003 = 0x0003;
        public const int ERR_MSG004 = 0x0004;
        public const int ERR_MSG005 = 0x0005;
        public const int ERR_MSG006 = 0x0006;
        public const int ERR_MSG007 = 0x0007;
        public const int ERR_MSG008 = 0x0008;
        public const int ERR_MSG009 = 0x0009;
        public const int ERR_MSG010 = 0x000A;
        public const int ERR_MSG011 = 0x000B;
        public const int ERR_MSG012 = 0x000C;
        public const int ERR_MSG013 = 0x000D;
        public const int ERR_MSG014 = 0x000E;
        public const int ERR_MSG015 = 0x000F;
        public const int ERR_MSG016 = 0x0010;
        public const int ERR_MSG017 = 0x0011;
        public const int ERR_MSG018 = 0x0012;
        public const int ERR_MSG019 = 0x0013;
        public const int ERR_MSG020 = 0x0014;
        public const int ERR_MSG021 = 0x0015;
        public const int ERR_MSG022 = 0x0016;
        public const int ERR_MSG023 = 0x0017;
        public const int ERR_MSG024 = 0x0018;
        public const int ERR_MSG025 = 0x0019;
        public const int ERR_MSG026 = 0x001A;
        public const int ERR_MSG027 = 0x001B;
        public const int ERR_MSG028 = 0x001C;
        public const int ERR_MSG029 = 0x001D;
        public const int ERR_MSG030 = 0x001E;
        public const int ERR_MSG031 = 0x001F;
        public const int ERR_MSG032 = 0x0020;
        public const int ERR_MSG033 = 0x0021;
        public const int ERR_MSG034 = 0x0022;
        public const int ERR_MSG035 = 0x0023;
        public const int ERR_MSG036 = 0x0024;
        public const int ERR_MSG037 = 0x0025;
        public const int ERR_MSG038 = 0x0026;
        public const int ERR_MSG039 = 0x0027;
        public const int ERR_MSG040 = 0x0028;
        public const int ERR_MSG041 = 0x0029;
        public const int ERR_MSG042 = 0x002A;
        public const int ERR_MSG043 = 0x002B;
        public const int ERR_MSG044 = 0x002C;
        public const int ERR_MSG045 = 0x002D;
        public const int ERR_MSG046 = 0x002E;
        public const int ERR_MSG047 = 0x002F;
        public const int ERR_MSG048 = 0x0030;
        public const int ERR_MSG049 = 0x0031;
        public const int ERR_MSG050 = 0x0032;
        public const int ERR_MSG051 = 0x0033;
        public const int ERR_MSG052 = 0x0034;
        public const int ERR_MSG053 = 0x0035;
        public const int ERR_MSG054 = 0x0036;
        public const int ERR_MSG055 = 0x0037;
        public const int ERR_MSG056 = 0x0038;
        public const int ERR_MSG057 = 0x0039;
        public const int ERR_MSG058 = 0x003A;
        public const int ERR_MSG059 = 0x003B;
        public const int ERR_MSG060 = 0x003C;
        public const int ERR_MSG061 = 0x003D;
        public const int ERR_MSG062 = 0x003E;
        public const int ERR_MSG063 = 0x003F;
        public const int ERR_MSG064 = 0x0040;
        public const int ERR_MSG065 = 0x0041;
        public const int ERR_MSG066 = 0x0042;
        public const int ERR_MSG067 = 0x0043;
        public const int ERR_MSG068 = 0x0044;
        public const int ERR_MSG069 = 0x0045;
        public const int ERR_MSG070 = 0x0046;
        public const int ERR_MSG071 = 0x0047;
        public const int ERR_MSG072 = 0x0048;
        public const int ERR_MSG073 = 0x0049;
        public const int ERR_MSG074 = 0x004A;
        public const int ERR_MSG075 = 0x004B;
        public const int ERR_MSG076 = 0x004C;
        public const int ERR_MSG077 = 0x004D;
        public const int ERR_MSG078 = 0x004E;
        public const int ERR_MSG079 = 0x004F;
        public const int ERR_MSG080 = 0x0050;
        public const int ERR_MSG081 = 0x0051;
        public const int ERR_MSG082 = 0x0052;
        public const int ERR_MSG083 = 0x0053;
        public const int ERR_MSG084 = 0x0054;
        public const int ERR_MSG085 = 0x0055;
        public const int ERR_MSG086 = 0x0056;
        public const int ERR_MSG087 = 0x0057;
        public const int ERR_MSG088 = 0x0058;
        public const int ERR_MSG089 = 0x0059;
        public const int ERR_MSG090 = 0x005A;
        public const int ERR_MSG091 = 0x005B;
        public const int ERR_MSG092 = 0x005C;
        public const int ERR_MSG093 = 0x005D;
        public const int ERR_MSG094 = 0x005E;
        public const int ERR_MSG095 = 0x005F;
        public const int ERR_MSG096 = 0x0060;
        public const int ERR_MSG097 = 0x0061;
        public const int ERR_MSG098 = 0x0062;
        public const int ERR_MSG099 = 0x0063;
        public const int ERR_MSG100 = 0x0064;
        public const int ERR_MSG101 = 0x0065;
        public const int ERR_MSG102 = 0x0066;
        public const int ERR_MSG103 = 0x0067;
        public const int ERR_MSG104 = 0x0068;
        public const int ERR_MSG105 = 0x0069;
        public const int ERR_MSG106 = 0x006A;
        public const int ERR_MSG107 = 0x006B;
        public const int ERR_MSG108 = 0x006C;
        public const int ERR_MSG109 = 0x006D;
        public const int ERR_MSG110 = 0x006E;
        public const int ERR_MSG111 = 0x006F;
        public const int ERR_MSG112 = 0x0070;
        public const int ERR_MSG113 = 0x0071;
        public const int ERR_MSG114 = 0x0072;
        public const int ERR_MSG115 = 0x0073;
        public const int ERR_MSG116 = 0x0074;
        public const int ERR_MSG117 = 0x0075;
        public const int ERR_MSG118 = 0x0076;
        public const int ERR_MSG119 = 0x0077;
        public const int ERR_MSG120 = 0x0078;
        public const int ERR_MSG121 = 0x0079;
        public const int ERR_MSG122 = 0x007A;
        public const int ERR_MSG123 = 0x007B;
        public const int ERR_MSG124 = 0x007C;
        public const int ERR_MSG125 = 0x007D;
        public const int ERR_MSG126 = 0x007E;
        public const int ERR_MSG127 = 0x007F;
        public const int ERR_MSG500 = 0x0100;
        public const int ERR_MSG501 = 0x0101;
        public const int ERR_MSG502 = 0x0102;
        public const int ERR_MSG503 = 0x0103;
        public const int ERR_MSG504 = 0x0104;
        public const int ERR_MSG505 = 0x0105;
        public const int ERR_MSG506 = 0x0106;
        public const int ERR_MSG507 = 0x0107;
        public const int ERR_MSG508 = 0x0108;
        public const int ERR_MSG509 = 0x0109;
        public const int ERR_MSG510 = 0x010A;
        public const int ERR_MSG511 = 0x010B;
        public const int ERR_MSG512 = 0x010C;
        public const int ERR_MSG513 = 0x010D;
        public const int ERR_MSG514 = 0x010E;
        public const int ERR_MSG515 = 0x010F;
        public const int ERR_MSG516 = 0x0110;
        public const int ERR_MSG517 = 0x0111;
        public const int ERR_MSG518 = 0x0112;
        public const int ERR_MSG519 = 0x0113;
        #endregion

        #region システムメッセージ
        public const int SYSTEM_MSG001 = 1;
        public const int SYSTEM_MSG002 = 2;
        public const int SYSTEM_MSG003 = 3;
        public const int SYSTEM_MSG004 = 4;
        public const int SYSTEM_MSG005 = 5;
        public const int SYSTEM_MSG006 = 6;
        public const int SYSTEM_MSG007 = 7;
        public const int SYSTEM_MSG008 = 8;
        public const int SYSTEM_MSG009 = 9;
        public const int SYSTEM_MSG010 = 10;
        public const int SYSTEM_MSG011 = 11;
        public const int SYSTEM_MSG012 = 12;
        public const int SYSTEM_MSG013 = 13;
        public const int SYSTEM_MSG014 = 14;
        public const int SYSTEM_MSG015 = 15;
        public const int SYSTEM_MSG016 = 16;
        public const int SYSTEM_MSG017 = 17;
        public const int SYSTEM_MSG018 = 18;
        public const int SYSTEM_MSG019 = 19;
        public const int SYSTEM_MSG020 = 20;
        public const int SYSTEM_MSG021 = 21;
        public const int SYSTEM_MSG022 = 22;
        public const int SYSTEM_MSG023 = 23;
        public const int SYSTEM_MSG024 = 24;
        public const int SYSTEM_MSG025 = 25;
        public const int SYSTEM_MSG026 = 26;
        public const int SYSTEM_MSG027 = 27;
        public const int SYSTEM_MSG028 = 28;
        public const int SYSTEM_MSG029 = 29;
        public const int SYSTEM_MSG030 = 30;
        public const int SYSTEM_MSG031 = 31;
        public const int SYSTEM_MSG032 = 32;
        public const int SYSTEM_MSG033 = 33;
        public const int SYSTEM_MSG034 = 34;
        public const int SYSTEM_MSG035 = 35;
        public const int SYSTEM_MSG036 = 36;
        public const int SYSTEM_MSG037 = 37;
        public const int SYSTEM_MSG038 = 38;        
        #endregion

        #region メンテ履歴ユニットメッセージ

#if  MAINTELOG && HCSM40
        public const int MAINTELOG_CORR_BASEMACHINE = 0x1001;   /*  補正値: ベースマシン */
        public const int MAINTELOG_CORR_FEED1 = 0x1002;         /*  補正値: フィード   */
        public const int MAINTELOG_CORR_SETPOS1 = 0x1003;       /*  補正値: 位置決め   */
        public const int MAINTELOG_CORR_CUTSTRIP = 0x1004;      /*  補正値: カットストリップ   */
        public const int MAINTELOG_TIMM_SIDE1 = 0x1101;         /*  タイミング: 1側          */
        public const int MAINTELOG_TIMM_SIDE2 = 0x1102;         /*  タイミング: 2側          */
        public const int MAINTELOG_UNIT_TRANSFER1 = 0x0201;     /*  1側旋回                  */
        public const int MAINTELOG_UNIT_SLIDER1 = 0x0202;       /*  1側前後                  */
        public const int MAINTELOG_UNIT_SEAL1 = 0x0203;         /*  1側防水挿入              */
        public const int MAINTELOG_UNIT_CRIMP1 = 0x0204;        /*  1側圧着                  */
        public const int MAINTELOG_UNIT_CUTSTRIP1 = 0x0205;     /*  カット・ストリップ       */
        public const int MAINTELOG_UNIT_TRANSFER2 = 0x0206;     /*  2側搬送                  */
        public const int MAINTELOG_UNIT_SLIDER2 = 0x0207;       /*  2側前後                  */
        public const int MAINTELOG_UNIT_SEAL2 = 0x0208;         /*  2側防水挿入              */
        public const int MAINTELOG_UNIT_CRIMP2 = 0x0209;        /*  2側圧着                  */
        public const int MAINTELOG_UNIT_EJECT1 = 0x020A;        /*  排出                     */
        public const int MAINTELOG_UNIT_FEED1 = 0x020B;         /*  電線供給                 */
        public const int MAINTELOG_UNIT_OTHER = 0x0999;         /*  その他 */
#endif

        #endregion

        #endregion

        #region アドレス定数
        // 通信
        public const int ADDR_FORMAT_CHECK = 0x0005;                /* フォーマットチェック開始アドレス */
        public const int FORMAT_CHECK_COUNT = 5;                    /* フォーマットチェックのアドレス数 */

        public const int MACHINE_STATUS = 0x8000;                   /* マシンステータスの開始アドレス  */
        public const int MACHINE_STATUS_COUNT = 0x3F;               /* マシンステータスのアドレス数    */

        public const int ADDR_MACHINE_STATUS = 0x8000;              /* マシン状態アドレス */
        public const int ADDR_ERROR_STATUS = 0x8010;                /* エラー状態アドレス */

        public const int ADDR_MACHINE_REVISION = 0x800B;            /* マシンリビジョンアドレス */
        public const int MACHINE_REVISION_COUNT = 5;                /* マシンリビジョンアドレス数 */

        // ボタン
        public const int ADDR_CYCLE_MODE = 0x0000;                  /* サイクルモード */
        public const int ADDR_MOVE_MODE = 0x0001;                   /* 動作モード */
        public const int ADDR_WORK_PROCESS1 = 0x0002;               /* 1側加工モード */
        public const int ADDR_WORK_PROCESS2 = 0x0003;               /* 2側加工モード */
        public const int ADDR_LOT_INTERVAL1 = 0x000B;               /* 自動復帰 */
        public const int ADDR_DOUBLE_MOTION = 0x0023;               /* 2段ストリップ動作 */
        public const int ADDR_OUTPUT = 0x000C;                      /* 外部出力 */
        public const int ADDR_LOAD1 = 0x000E;                       /* 電線ロード */
        public const int ADDR_MACHINE_START1 = 0x0010;              /* スタート */
        public const int ADDR_MACHINE_STOP1 = 0x0011;               /* ストップ */
        public const int ADDR_MACHINE_RESET1 = 0x0012;              /* リセット */
        public const int ADDR_TOTAL_COUNTER_RESET1 = 0x001A;        /* トータルカウンターリセット */
        public const int ADDR_QTY_COUNTER_RESET1 = 0x001B;          /* QTYカウンターリセット */
        public const int ADDR_LOT_COUNTER_RESET1 = 0x001C;          /* LOTカウンターリセット */
        public const int ADDR_COUNT_UP_DOWN = 0x001F;               /* カウントアップダウン */
        #endregion

        #region 通信設定定数
        //通信設定
        public const int MAX_RETRY_COUNT = 3;                       /* リトライ回数 */
        public const int SERIAL_READ_TIMEOUT = 3000;                /* シリアルポート読み込みタイムアウト（ミリ秒） */
        public const int SERIAL_WRITE_TIMEOUT = 3000;               /* シリアルポート書き込みタイムアウト（ミリ秒） */
        public const int SERIAL_ACK_TIMEOUT = 3000;                 /* ACK待ちタイムアウト（ミリ秒） */
        public const int SERIAL_POLLING_INTERVAL = 10000;           /* ポーリング間隔 */

        // 通信の切断理由
        public const int COM_ERROR_NORMAL = 0;                      /* エラーなし */
        public const int COM_ERROR_RETRYERROR = 1;                  /* リトライエラー */
        #endregion

        #region 状態表示処理定数
        // 更新時間
        public const int STATUS_DISPLAY_REFRESH_TIME = 20;

        #region 表示位置定数
        #region メインフォーム
        public const int SENSOR_STATUS_BOX0 = 0;
        public const int SENSOR_STATUS_BOX1 = 1;
        public const int SENSOR_STATUS_BOX2 = 2;
        public const int SENSOR_STATUS_BOX3 = 3;
        public const int SENSOR_STATUS_BOX4 = 4;
        public const int SENSOR_STATUS_BOX5 = 5;
        public const int SENSOR_STATUS_BOX6 = 6;
        public const int SENSOR_STATUS_BOX7 = 7;
        public const int SENSOR_STATUS_BOX8 = 8;
        public const int SENSOR_STATUS_BOX9 = 9;
        public const int SENSOR_STATUS_BOX10 = 10;
        public const int SENSOR_STATUS_BOX11 = 11;
        public const int SENSOR_STATUS_BOX12 = 12;
        public const int SENSOR_STATUS_BOX13 = 13;
        public const int SENSOR_STATUS_BOX14 = 14;
        public const int SENSOR_STATUS_BOX15 = 15;

        public const int MOTION_STATUS_BOX_START = 30;

        public const int MOTION_STATUS_BOX0 = 30;
        public const int MOTION_STATUS_BOX1 = 31;
        public const int MOTION_STATUS_BOX2 = 32;
        public const int MOTION_STATUS_BOX3 = 33;
        public const int MOTION_STATUS_BOX4 = 34;
        public const int MOTION_STATUS_BOX5 = 35;
        public const int MOTION_STATUS_BOX6 = 36;
        public const int MOTION_STATUS_BOX7 = 37;
        public const int MOTION_STATUS_BOX8 = 38;
        public const int MOTION_STATUS_BOX9 = 39;
        public const int MOTION_STATUS_BOX10 = 40;
        public const int MOTION_STATUS_BOX11 = 41;
        #endregion

        #region システムフォーム
        // 1列目
        public const int SENSOR_LOCK_BOX0 = 0;
        public const int SENSOR_LOCK_BOX1 = 1;
        public const int SENSOR_LOCK_BOX2 = 2;
        public const int SENSOR_LOCK_BOX3 = 3;
        public const int SENSOR_LOCK_BOX4 = 4;
        public const int SENSOR_LOCK_BOX5 = 5;
        public const int SENSOR_LOCK_BOX6 = 6;
        public const int SENSOR_LOCK_BOX7 = 7;
        public const int SENSOR_LOCK_BOX8 = 8;
        public const int SENSOR_LOCK_BOX9 = 9;
        // 2列目
        public const int SENSOR_LOCK_BOX10 = 10;
        public const int SENSOR_LOCK_BOX11 = 11;
        public const int SENSOR_LOCK_BOX12 = 12;
        public const int SENSOR_LOCK_BOX13 = 13;
        public const int SENSOR_LOCK_BOX14 = 14;
        public const int SENSOR_LOCK_BOX15 = 15;
        public const int SENSOR_LOCK_BOX16 = 16;
        public const int SENSOR_LOCK_BOX17 = 17;
        public const int SENSOR_LOCK_BOX18 = 18;
        public const int SENSOR_LOCK_BOX19 = 19;
        // 3列目
        public const int SENSOR_LOCK_BOX20 = 20;
        public const int SENSOR_LOCK_BOX21 = 21;
        public const int SENSOR_LOCK_BOX22 = 22;
        public const int SENSOR_LOCK_BOX23 = 23;
        public const int SENSOR_LOCK_BOX24 = 24;
        public const int SENSOR_LOCK_BOX25 = 25;
        public const int SENSOR_LOCK_BOX26 = 26;
        public const int SENSOR_LOCK_BOX27 = 27;
        public const int SENSOR_LOCK_BOX28 = 28;
        public const int SENSOR_LOCK_BOX29 = 29;
        #endregion
        #endregion

        #region 表示メッセージ定数
        public const int STAT_DSP_MSG_DOUBLEMODE1 = 0x001E;
        public const int STAT_DSP_MSG_OUTPUT1 = 0x001F;
        #endregion

        #endregion

        #region IOモニタ
        public const int IO_MONITOR_BOARD_NONE = 0;
        public const int IO_MONITOR_BOARD_1_IN1 = 1;
        public const int IO_MONITOR_BOARD_1_IN2 = 2;
        public const int IO_MONITOR_BOARD_1_OUT1 = 3;
        public const int IO_MONITOR_BOARD_1_OUT2 = 4;
        public const int IO_MONITOR_BOARD_2_IN1 = 5;
        public const int IO_MONITOR_BOARD_2_IN2 = 6;
        public const int IO_MONITOR_BOARD_2_OUT1 = 7;
        public const int IO_MONITOR_BOARD_2_OUT2 = 8;
        #endregion

    }
}
