using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Diagnostics;
using System.IO;

namespace Alchemist
{
    public struct LearnDataStruct
    {
        public int Type;
        public int SearchType;
        public int ID;
        public string value;
    }

    public class LearnDataStorage
    {
#if HCSM40
        // 検索キーコード一覧
        private static readonly int[] learnKeyCodes = new int[]{
                SystemConstants.DB_GROUP_SEARCH_TYPE_WIRE1,
                SystemConstants.DB_GROUP_SEARCH_TYPE_WIRELENGTH1,
                SystemConstants.DB_GROUP_SEARCH_TYPE_STRIPLENGTH1,
                SystemConstants.DB_GROUP_SEARCH_TYPE_STRIPLENGTH2
        };
#else
        // 検索キーコード一覧
        private static readonly int[] learnKeyCodes = new int[]{
                SystemConstants.DB_GROUP_SEARCH_TYPE_WIRE1,
//                SystemConstants.DB_GROUP_SEARCH_TYPE_WIRELENGTH1,
                SystemConstants.DB_GROUP_SEARCH_TYPE_CRIMP1,
                SystemConstants.DB_GROUP_SEARCH_TYPE_CRIMP2,
                SystemConstants.DB_GROUP_SEARCH_TYPE_SEAL1,
                SystemConstants.DB_GROUP_SEARCH_TYPE_SEAL2
        };
#endif

#if HCSM40
        // 加工動作ボタン
        private static readonly int[] learnedModeChangeBtn = new int[]{
            SystemConstants.STRIP1_BTN,
            SystemConstants.STRIP2_BTN
        };
#else
        // 加工動作ボタン
        private static readonly int[] learnedModeChangeBtn = new int[]{
            SystemConstants.STRIP1_BTN,
            SystemConstants.STRIP2_BTN,
            SystemConstants.CRIMP1_BTN,
            SystemConstants.CRIMP2_BTN,
            SystemConstants.SEAL1_BTN,
            SystemConstants.SEAL2_BTN
        };
#endif

#if HCSM40
        // 学習データのキーによる、動作モードのON, OFFを保存しておく
        private struct BtnStatusStruct
        {
            public bool Strip;
        }
#else
        // 学習データのキーによる、動作モードのON, OFFを保存しておく
        private struct BtnStatusStruct
        {
            public bool Strip;
            public bool Crimp;
            public bool Seal;
        }
#endif
        private Dictionary<int, BtnStatusStruct> btnMap = new Dictionary<int, BtnStatusStruct>();

        public LearnDataStorage()
        {
#if HCSM40
            /* ハッシュ値計算
             * +-----------------+------------+
             * | ストリップ長    | ハッシュ値 |
             * +-----------------+------------+
             * |     ×          |     0      |
             * |     ○          |     1      |
             * +-----------------+------------+
             * 
             */

            // マップを作成する
            BtnStatusStruct[] btnStatus = new BtnStatusStruct[2];

            // ハッシュ値0
            btnStatus[0].Strip = false;

            // ハッシュ値1
            btnStatus[1].Strip = true;

            // 各データを登録する
            btnMap.Add(0, btnStatus[0]);
            btnMap.Add(1, btnStatus[1]);
#else
            /* ハッシュ値計算
             * +-----------+-----------+-----------------+------------+
             * | 端子名    | 防水栓名  | ストリップ長    | ハッシュ値 |
             * +-----------+-----------+-----------------+------------+
             * |   ×      |     ×    |     ×          |     0      |
             * |   ○      |     ×    |     ×          |     1      |
             * |   ×      |     ○    |     ×          |     2      |
             * |   ○      |     ○    |     ×          |     3      |
             * |   ×      |     ×    |     ○          |     4      |
             * |   ×      |     ○    |     ○          |     6      |
             * +-----------+-----------+-----------------+------------+
             * 
             */

            // マップを作成する
            BtnStatusStruct[] btnStatus = new BtnStatusStruct[6];

            // ハッシュ値0
            btnStatus[0].Strip = false;
            btnStatus[0].Crimp = false;
            btnStatus[0].Seal = false;

            // ハッシュ値1
            btnStatus[1].Strip = true;
            btnStatus[1].Crimp = true;
            btnStatus[1].Seal = false;

            // ハッシュ値2
            btnStatus[2].Strip = false;
            btnStatus[2].Crimp = false;
            btnStatus[2].Seal = true;

            // ハッシュ値3
            btnStatus[3].Strip = true;
            btnStatus[3].Crimp = true;
            btnStatus[3].Seal = true;

            // ハッシュ値4
            btnStatus[4].Strip = true;
            btnStatus[4].Crimp = false;
            btnStatus[4].Seal = false;

            // ハッシュ値6
            btnStatus[5].Strip = true;
            btnStatus[5].Crimp = false;
            btnStatus[5].Seal = true;

            // 各データを登録する
            btnMap.Add(0, btnStatus[0]);
            btnMap.Add(1, btnStatus[1]);
            btnMap.Add(2, btnStatus[2]);
            btnMap.Add(3, btnStatus[3]);
            btnMap.Add(4, btnStatus[4]);
            btnMap.Add(6, btnStatus[5]);
#endif
        }

#if HCSM40
        private int getWorkModeHashData(string StripLength)
        {
            int workModeHash = 0;

            if (StripLength.Length > 0) workModeHash += (1 << 0);

            return workModeHash;
        }
#else
        private int getWorkModeHashData(string TerminalName, string SealName, string StripLength)
        {
            int workModeHash = 0;

            // 名称の有無でハッシュコードを作成する
            if (TerminalName.Length > 0) workModeHash += (1 << 0);
            if (SealName.Length > 0) workModeHash += (1 << 1);
            if (StripLength.Length > 0) workModeHash += (1 << 2);

            return workModeHash;
        }
#endif

        // データキー保存用
        private string[] itemKeys = new string[0];

        private XMLAccessor xmlAccessor = new LearnDataXMLAccessor();
#if DEBUG && HCSM40
        private string xmlFileName = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\JAM\\Alchemist\\HCSM40\\learndata.xml";
#else
        private string xmlFileName = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\JAM\\Alchemist\\learndata.xml";
#endif
        private bool initFlag = false;

        // ファイルからロードする
        public void Load()
        {
            try
            {
                // XMLファイルを開く
                xmlAccessor.LoadXmlFile(xmlFileName);
            }
            catch (FileNotFoundException)
            {
                // ファイルがなければ作る
                xmlAccessor.NewDocument();
                xmlAccessor.SaveXmlFile(xmlFileName);
            }
            finally
            {
                initFlag = true;
            }
        }

        /// <summary>
        /// 検索キーの一覧を返す
        /// </summary>
        /// <returns></returns>
        public string[] GetSearchKeys()
        {
            string[] ItemKeys = new string[itemKeys.Count()];

            // キーの配列を作る
            ItemKeys = new string[itemKeys.Count()];
            // 一覧を変数に入れる
            for (int i = 0; i < itemKeys.Count(); i++)
                ItemKeys[i] = itemKeys[i];

            return ItemKeys;
        }

        /// <summary>
        /// 検索キーを取得用に保存する
        /// </summary>
        /// <param name="ItemKeys"></param>
        /// <returns></returns>
        public int storeSearchKey(string[] ItemKeys)
        {
            // 格納用の配列を作成する
            itemKeys = new string[ItemKeys.Count()];

            // 配列にデータを入れる
            for (int i = 0; i < ItemKeys.Count(); i++)
                itemKeys[i] = ItemKeys[i];

            return SystemConstants.LSPF_SUCCESS;
        }

        /// <summary>
        /// 学習済みアイテム一覧を取得する
        /// </summary>
        /// <param name="ItemKeyCode"></param>
        /// <returns></returns>
        public string[] GetLearnedItemKey(int ItemKeyCode)
        {
            // 検索項目を作成する
            string itemKey = "key" + ItemKeyCode.ToString();
            IEnumerable<string> keys;

            switch (ItemKeyCode)
            {
#if HCSM40
                case SystemConstants.DB_GROUP_KEY_WIRENAME:
                    keys = (from n in xmlAccessor.document.Elements("learndata").Elements("searchkey")
                            where n.Attribute("key0").Value != ""
                            orderby n.Attribute("key0").Value
                            select n.Attribute("key0").Value.ToString()).Distinct();
                    break;
                case SystemConstants.DB_GROUP_KEY_WIRESIZE:
                    keys = (from n in xmlAccessor.document.Elements("learndata").Elements("searchkey")
                            where n.Attribute("key1").Value != ""
                            orderby n.Attribute("key1").Value
                            select n.Attribute("key1").Value.ToString()).Distinct();
                    break;
#else
                case SystemConstants.DB_GROUP_KEY_WIRENAME:
                    keys = (from n in xmlAccessor.document.Elements("learndata").Elements("searchkey")
                            where n.Attribute("key0").Value != ""
                            orderby n.Attribute("key0").Value
                            select n.Attribute("key0").Value.ToString()).Distinct();
                    break;
                case SystemConstants.DB_GROUP_KEY_WIRESIZE:
                    keys = (from n in xmlAccessor.document.Elements("learndata").Elements("searchkey")
                            where n.Attribute("key1").Value != ""
                            orderby n.Attribute("key1").Value
                            select n.Attribute("key1").Value.ToString()).Distinct();
                    break;
                case SystemConstants.DB_GROUP_KEY_TERMINALNAME1:
                    keys = (from n in xmlAccessor.document.Elements("learndata").Elements("searchkey")
                            where n.Attribute("key2").Value != ""
                            && n.Attribute("type").Value == SystemConstants.DB_GROUP_SEARCH_TYPE_CRIMP1.ToString()
                            orderby n.Attribute("key2").Value
                            select n.Attribute("key2").Value.ToString()).Distinct();
                    break;
                case SystemConstants.DB_GROUP_KEY_TERMINALNAME2:
                    keys = (from n in xmlAccessor.document.Elements("learndata").Elements("searchkey")
                            where n.Attribute("key2").Value != ""
                            && n.Attribute("type").Value == SystemConstants.DB_GROUP_SEARCH_TYPE_CRIMP2.ToString()
                            orderby n.Attribute("key2").Value
                            select n.Attribute("key2").Value.ToString()).Distinct();
                    break;
                case SystemConstants.DB_GROUP_KEY_SEALNAME1:
                    keys = (from n in xmlAccessor.document.Elements("learndata").Elements("searchkey")
                            where n.Attribute("key3").Value != ""
                            && (n.Attribute("type").Value == SystemConstants.DB_GROUP_SEARCH_TYPE_CRIMP1.ToString() || n.Attribute("type").Value == SystemConstants.DB_GROUP_SEARCH_TYPE_SEAL1.ToString())
                            orderby n.Attribute("key3").Value
                            select n.Attribute("key3").Value.ToString()).Distinct();
                    break;
                case SystemConstants.DB_GROUP_KEY_SEALNAME2:
                    keys = (from n in xmlAccessor.document.Elements("learndata").Elements("searchkey")
                            where n.Attribute("key3").Value != ""
                            && (n.Attribute("type").Value == SystemConstants.DB_GROUP_SEARCH_TYPE_CRIMP2.ToString() || n.Attribute("type").Value == SystemConstants.DB_GROUP_SEARCH_TYPE_SEAL2.ToString())
                            orderby n.Attribute("key3").Value
                            select n.Attribute("key3").Value.ToString()).Distinct();
                    break;
                case SystemConstants.DB_GROUP_KEY_APLICATOR1:
                    keys = (from n in xmlAccessor.document.Elements("learndata").Elements("searchkey")
                            where n.Attribute("type").Value != ""
                            && n.Attribute("type").Value == SystemConstants.DB_GROUP_SEARCH_TYPE_CRIMP1.ToString()
                            orderby n.Attribute("key5").Value
                            select n.Attribute("key5").Value.ToString()).Distinct();
                    break;
                case SystemConstants.DB_GROUP_KEY_APLICATOR2:
                    keys = (from n in xmlAccessor.document.Elements("learndata").Elements("searchkey")
                            where n.Attribute("type").Value != ""
                            && n.Attribute("type").Value == SystemConstants.DB_GROUP_SEARCH_TYPE_CRIMP2.ToString()
                            orderby n.Attribute("key5").Value
                            select n.Attribute("key5").Value.ToString()).Distinct();
                    break;
#endif
                default:
                    throw new ArgumentException();
            }

            string[] result = keys.ToArray();

            return result;
        }

        /// <summary>
        /// キー項目に対して、学習データからデータを取得して、LearnDataStruct構造体の配列に入れて返す
        /// </summary>
        /// <param name="WireName"></param>
        /// <param name="WireSize"></param>
        /// <param name="TerminalName1"></param>
        /// <param name="TerminalName2"></param>
        /// <param name="SealName1"></param>
        /// <param name="SealName2"></param>
        /// <param name="StripLength1"></param>
        /// <param name="StripLength2"></param>
        /// <param name="LearnData"></param>
        /// <returns></returns>
        public int GetLearnData(string[] ItemKeys, ref LearnDataStruct[] LearnData)
        {
            int result = SystemConstants.LSPF_SUCCESS;
            string[] keys = null;
            LearnDataStruct[] partsLearnData = null;
            LearnData = new LearnDataStruct[0];
            int n = 0;

            // データキータイプ毎に処理を行う
            foreach (int keytype in learnKeyCodes)
            {
                // 検索キーを取得する
                keys = CreateLearnSearchKey(keytype, ItemKeys);

                // 各データキータイプ毎に学習データを取得する
                result = GetLearnData(keytype, keys, ref partsLearnData);

                // 取得したデータがnullでない場合、処理を続行する
                if (partsLearnData.Count() > 0)
                {
                    // 配列を広げる
                    Array.Resize(ref LearnData, LearnData.Count() + partsLearnData.Count());

                    // 結果を結果変数に追加する
                    partsLearnData.CopyTo(LearnData, n);
                    n += partsLearnData.Count();
                }
            }

            // ボタン配列を追加する
            LearnDataStruct[] btnLearnData = getBtnModeLearnData(ItemKeys);
            // 配列を広げて、コピーする
            Array.Resize(ref LearnData, LearnData.Count() + btnLearnData.Count());
            btnLearnData.CopyTo(LearnData, n);

            return result;
        }

        /// <summary>
        /// 学習データから、検索キーに該当するデータをLearnDataに構造体にして返します。
        /// ここで取得するデータは、Typeで指定されている加工値の一部のデータです。
        /// </summary>
        /// <param name="type"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public int GetLearnData(int type, string[] keys, ref LearnDataStruct[] LearnData)
        {
            // ロードチェック
            if (!initFlag) return SystemConstants.ERR_UNINITIALIZED;

            // 検索キータイプが適切でない場合、例外を発生させる
            if (!IsLearnSearchKeyCode(type) || !IsSearchKey(keys)) throw new ArgumentException();

            // XMLファイルを読む
            var nodes = from n in xmlAccessor.document.Elements("learndata").Elements("searchkey")
                        where n.Attribute("type").Value == type.ToString()
                        && n.Attribute("key0").Value == keys[0]
                        && n.Attribute("key1").Value == keys[1]
                        && n.Attribute("key2").Value == keys[2]
                        && n.Attribute("key3").Value == keys[3]
                        && n.Attribute("key4").Value == keys[4]
                        && n.Attribute("key5").Value == keys[5]
                        && n.Attribute("key6").Value == keys[6]
                        && n.Attribute("key7").Value == keys[7]
                        && n.Attribute("key8").Value == keys[8]
                        && n.Attribute("key9").Value == keys[9]
                        select n;

            // 見付かったノードが1でなければ、エラーを返す
            if (nodes.Count() != 1)
            {
                LearnData = new LearnDataStruct[0];
                return SystemConstants.ERR_LSPF_NO_LEARN_DATA;
            }

            // entryを取出す ついでに、昇順に並び替える
            nodes = from n in nodes.Elements("entry")
                    orderby n.Attribute("workid").Value
                    select n;

            // 構造体を割当てる
            int count = nodes.Count();
            LearnData = new LearnDataStruct[count];

            // 構造体に読込む
            int i = 0;
            foreach (var entry in nodes)
            {
                LearnData[i].Type = SystemConstants.WORKMEM_TYPE_WORKDATA;
                LearnData[i].SearchType = type;
                LearnData[i].ID = Int32.Parse(entry.Attribute("workid").Value.Remove(0, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                LearnData[i].value = entry.Attribute("value").Value;
                i++;
            }

            return SystemConstants.LSPF_SUCCESS;
        }

        /// <summary>
        /// キー項目に対して、LearnDataの値の構造体一覧を保存する。
        /// </summary>
        /// <param name="ItemKeys"></param>
        /// <param name="LearnData"></param>
        /// <returns></returns>
        public int WriteLearnData(string[] ItemKeys, LearnDataStruct[] LearnData)
        {
            int result = SystemConstants.LSPF_SUCCESS;
            string[] keys = null;
            LearnDataStruct[] selectedLearnData = null;

            // データキータイプ毎に処理を行う
            foreach (int seachkeytype in learnKeyCodes)
            {
                // 検索キーを生成する
                keys = CreateLearnSearchKey(seachkeytype, ItemKeys);

                // 学習データ配列から、データを取得する
                selectedLearnData = extractLearnData(seachkeytype, LearnData);

                // 取得したデータをが0でなければキー毎に書込む
                if (selectedLearnData != null)
                    result = WriteLearnData(seachkeytype, keys, selectedLearnData);
            }

            return result;
        }

        /// <summary>
        /// 引数で渡されるキーの項目に対して、LearnDataの値の構造体一覧を保存する。
        /// 当該検索キーが存在しない場合は、新規に追加し、存在している場合は、上書きを行います。
        /// 正常に終了した場合は、LSPF_SCCESS を返します。
        /// </summary>
        /// <param name="type"></param>
        /// <param name="keys"></param>
        /// <param name="LearnData"></param>
        /// <returns></returns>
        public int WriteLearnData(int type, string[] keys, LearnDataStruct[] LearnData)
        {
            // ロードチェック
            if (!initFlag) return SystemConstants.ERR_LSPF_UNINITIALIZED;

            // 検索キーのタイプが正しくない場合、例外を発生させる
            if (!IsLearnSearchKeyCode(type) || !IsSearchKey(keys)) throw new ArgumentException();

            // 学習データから、ノードを見つける
            var nodes = from n in xmlAccessor.document.Elements("learndata").Elements("searchkey")
                        where n.Attribute("type").Value == type.ToString()
                        && n.Attribute("key0").Value == keys[0]
                        && n.Attribute("key1").Value == keys[1]
                        && n.Attribute("key2").Value == keys[2]
                        && n.Attribute("key3").Value == keys[3]
                        && n.Attribute("key4").Value == keys[4]
                        && n.Attribute("key5").Value == keys[5]
                        && n.Attribute("key6").Value == keys[6]
                        && n.Attribute("key7").Value == keys[7]
                        && n.Attribute("key8").Value == keys[8]
                        && n.Attribute("key9").Value == keys[9]
                        select n;

            XElement learn;
            if (nodes.Count() >= 1)
            {
                // ノードがある場合、エントリーを消す
                learn = nodes.First();
                learn.RemoveNodes();
            }
            else
            {
                // ノードが無い場合、エントリー項目を作る
                learn = new XElement("searchkey");
                learn.SetAttributeValue("type", type);
                learn.SetAttributeValue("key0", keys[0]);
                learn.SetAttributeValue("key1", keys[1]);
                learn.SetAttributeValue("key2", keys[2]);
                learn.SetAttributeValue("key3", keys[3]);
                learn.SetAttributeValue("key4", keys[4]);
                learn.SetAttributeValue("key5", keys[5]);
                learn.SetAttributeValue("key6", keys[6]);
                learn.SetAttributeValue("key7", keys[7]);
                learn.SetAttributeValue("key8", keys[8]);
                learn.SetAttributeValue("key9", keys[9]);
                xmlAccessor.document.Element("learndata").Add(learn);
            }

            // エントリーを作る
            for (int i = 0; i < LearnData.Length; i++)
            {
                var elem = new XElement("entry");
                elem.SetAttributeValue("workid", string.Format("0x{0:X4}", LearnData[i].ID));
                elem.SetAttributeValue("value", LearnData[i].value);
                learn.Add(elem);
            }

            // XMLファイルにセーブする
            try
            {
                // XMLファイルを開く
                xmlAccessor.SaveXmlFile(xmlFileName);
            }
            catch (Exception)
            {
                return SystemConstants.ERR_LSPF_FILE_ERROR;
            }
            finally
            {
                Utility.DeleteBackupFile(xmlFileName);
            }

            return SystemConstants.LSPF_SUCCESS;
        }

        /// <summary>
        /// キータイプと値で保存対象のデータかを見分ける
        /// 圧着: 端子名があり、ストリップ長が無い場合保存対象
        /// 防水: 防水栓名がある場合、対象
        /// ストリップ: 端子名が無く、ストリップ長がある場合保存対象
        /// </summary>
        /// <param name="type"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        private bool isLearnedData(int type, string[] keys)
        {
            bool result = true;
            switch (type)
            {
#if HCSM40
                case SystemConstants.DB_GROUP_SEARCH_TYPE_STRIPLENGTH1:
                    result = (keys[SystemConstants.DB_GROUP_KEY_STRIPLENGTH1].Length > 0);
                    break;
                case SystemConstants.DB_GROUP_SEARCH_TYPE_STRIPLENGTH2:
                    result = (keys[SystemConstants.DB_GROUP_KEY_STRIPLENGTH2].Length > 0);
                    break;
#else
                case SystemConstants.DB_GROUP_SEARCH_TYPE_CRIMP1:
                    result = (keys[SystemConstants.DB_GROUP_KEY_TERMINALNAME1].Length > 0 || keys[SystemConstants.DB_GROUP_KEY_STRIPLENGTH1].Length > 0);
                    break;
                case SystemConstants.DB_GROUP_SEARCH_TYPE_CRIMP2:
                    result = (keys[SystemConstants.DB_GROUP_KEY_TERMINALNAME2].Length > 0 || keys[SystemConstants.DB_GROUP_KEY_STRIPLENGTH2].Length > 0);
                    break;
                case SystemConstants.DB_GROUP_SEARCH_TYPE_SEAL1:
                    result = (keys[SystemConstants.DB_GROUP_KEY_SEALNAME1].Length > 0);
                    break;
                case SystemConstants.DB_GROUP_SEARCH_TYPE_SEAL2:
                    result = (keys[SystemConstants.DB_GROUP_KEY_SEALNAME2].Length > 0);
                    break;
#endif
            }
            return result;
        }

        /// <summary>
        /// 学習データ検索用のタイプ別のキーを作成する
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="WireName">電線種類</param>
        /// <param name="WireSize">コアサイズ</param>
        /// <param name="Color1">色1</param>
        /// <param name="Color2">色2</param>
        /// <param name="WireLength">切断長</param>
        /// <param name="StripLength1">ストリップ長1</param>
        /// <param name="StripLength2">ストリップ長2</param>
        /// <returns></returns>
        public static string[] CreateLearnSearchKey(int Type, string[] ItemKeys)
        {
            const int keyArrayCount = 10;
            // キー用の配列を作成・初期化する
            string[] searchkey = new string[keyArrayCount];
            for (int i = 0; i < keyArrayCount; i++) searchkey[i] = "";

            // タイプ別に作成を行う
            // 初期化で配列はすべて空文字になっているので、変更点だけ入れる
            switch (Type)
            {
                case SystemConstants.DB_GROUP_SEARCH_TYPE_NONE:
                    break;
                case SystemConstants.DB_GROUP_SEARCH_TYPE_WIRE1:
                    searchkey[0] = ItemKeys[SystemConstants.DB_GROUP_KEY_WIRENAME];
                    searchkey[1] = ItemKeys[SystemConstants.DB_GROUP_KEY_WIRESIZE];
                    searchkey[2] = ItemKeys[SystemConstants.DB_GROUP_KEY_WIRECOLOR1];
                    searchkey[3] = ItemKeys[SystemConstants.DB_GROUP_KEY_WIRECOLOR2];
                    break;
                case SystemConstants.DB_GROUP_SEARCH_TYPE_WIRELENGTH1:
                    searchkey[0] = ItemKeys[SystemConstants.DB_GROUP_KEY_WIRENAME];
                    searchkey[1] = ItemKeys[SystemConstants.DB_GROUP_KEY_WIRESIZE];
                    searchkey[2] = ItemKeys[SystemConstants.DB_GROUP_KEY_WIRECOLOR1];
                    searchkey[3] = ItemKeys[SystemConstants.DB_GROUP_KEY_WIRECOLOR2];
                    searchkey[4] = ItemKeys[SystemConstants.DB_GROUP_KEY_WIRELENGTH1];
                    break;
                case SystemConstants.DB_GROUP_SEARCH_TYPE_STRIPLENGTH1:
                    searchkey[0] = ItemKeys[SystemConstants.DB_GROUP_KEY_WIRENAME];
                    searchkey[1] = ItemKeys[SystemConstants.DB_GROUP_KEY_WIRESIZE];
                    searchkey[2] = ItemKeys[SystemConstants.DB_GROUP_KEY_WIRECOLOR1];
                    searchkey[3] = ItemKeys[SystemConstants.DB_GROUP_KEY_WIRECOLOR2];
                    searchkey[4] = ItemKeys[SystemConstants.DB_GROUP_KEY_STRIPLENGTH1];
                    break;
                case SystemConstants.DB_GROUP_SEARCH_TYPE_STRIPLENGTH2:
                    searchkey[0] = ItemKeys[SystemConstants.DB_GROUP_KEY_WIRENAME];
                    searchkey[1] = ItemKeys[SystemConstants.DB_GROUP_KEY_WIRESIZE];
                    searchkey[2] = ItemKeys[SystemConstants.DB_GROUP_KEY_WIRECOLOR1];
                    searchkey[3] = ItemKeys[SystemConstants.DB_GROUP_KEY_WIRECOLOR2];
                    searchkey[4] = ItemKeys[SystemConstants.DB_GROUP_KEY_STRIPLENGTH2];
                    break;
                default:
                    // タイプが範囲外の場合、例外を発生させる
                    throw new ArgumentException();
            }

            return searchkey;
        }

        /// <summary>
        /// LearnData から、検索キータイプのデータを抽出する
        /// </summary>
        /// <param name="searchType"></param>
        /// <param name="LearnData"></param>
        /// <returns></returns>
        private LearnDataStruct[] extractLearnData(int searchType, LearnDataStruct[] LearnData)
        {
            LearnDataStruct[] resultlearnData = null;

            // 学習データ配列から、データを取得する
            var learnquery = from learndata in LearnData
                             where learndata.SearchType == searchType
                             orderby learndata.ID
                             select learndata;
            resultlearnData = learnquery.ToArray();

            return resultlearnData;
        }

        private LearnDataStruct[] getBtnModeLearnData(string[] ItemKeys)
        {
            LearnDataStruct[] btnLearnData = new LearnDataStruct[6];

            // ハッシュを取得する
            int hash1 = getWorkModeHashData(ItemKeys[SystemConstants.DB_GROUP_KEY_STRIPLENGTH1]);
            int hash2 = getWorkModeHashData(ItemKeys[SystemConstants.DB_GROUP_KEY_STRIPLENGTH2]);
            // ハッシュリストからデータを取得する
            BtnStatusStruct btnStatus1, btnStatus2;
            btnMap.TryGetValue(hash1, out btnStatus1);
            btnMap.TryGetValue(hash2, out btnStatus2);

            // 取得したモードに従い、返値を作成する
            btnLearnData[0] = makeBtnLearnDataStruct(SystemConstants.STRIP1_BTN, btnStatus1.Strip);
            btnLearnData[1] = makeBtnLearnDataStruct(SystemConstants.STRIP2_BTN, btnStatus2.Strip);
#if !HCSM40
            btnLearnData[2] = makeBtnLearnDataStruct(SystemConstants.CRIMP1_BTN, btnStatus1.Crimp);
            btnLearnData[3] = makeBtnLearnDataStruct(SystemConstants.CRIMP2_BTN, btnStatus2.Crimp);
            btnLearnData[4] = makeBtnLearnDataStruct(SystemConstants.SEAL1_BTN, btnStatus1.Seal);
            btnLearnData[5] = makeBtnLearnDataStruct(SystemConstants.SEAL2_BTN, btnStatus2.Seal);
#endif
            return btnLearnData;
        }

        private LearnDataStruct makeBtnLearnDataStruct(int Btn, bool Status)
        {
            LearnDataStruct learnData;
            learnData.ID = Btn;
            if (Status)
                learnData.value = SystemConstants.BTN_ON.ToString();
            else
                learnData.value = SystemConstants.BTN_OFF.ToString();
            learnData.SearchType = SystemConstants.DB_GROUP_SEARCH_TYPE_BTN;
            learnData.Type = SystemConstants.WORKMEM_TYPE_WORKBTN;

            return learnData;
        }

        /// <summary>
        /// 検索キータイプが正しいかチェックする。正常の場合、true を返す
        /// </summary>
        /// <param name="LearnSearchKeyCode"></param>
        /// <returns></returns>
        public static bool IsLearnSearchKeyCode(int LearnSearchKeyCode)
        {
            if (Array.IndexOf(learnKeyCodes, LearnSearchKeyCode) < 0)
                return false;
            else
                return true;
        }

        /// <summary>
        /// 検索キーが正しいかをチェックする。正常の場合、trueを返す
        /// </summary>
        /// <param name="SearchKeys"></param>
        /// <returns></returns>
        public static bool IsSearchKey(string[] SearchKeys)
        {
            if (SearchKeys.Length >= 10)
                return true;
            else
                return false;
        }

        /// <summary>
        /// ボタンが学習モードで使用するボタンかを返します
        /// </summary>
        /// <param name="BtnID"></param>
        /// <returns></returns>
        public static bool IsLearnedModeBtn(int BtnID)
        {
            if (Array.IndexOf(learnedModeChangeBtn, BtnID) < 0)
                return false;
            else
                return true;
        }

        /// <summary>
        /// 当該キーがストリップ動作かを返す
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public bool IsStripWork(int Side, string[] ItemKeys)
        {
            LearnDataStruct[] learnData = getBtnModeLearnData(ItemKeys);
            int stripBtnworkID;
#if !HCSM40
            int crimpBtnworkID;
#endif
            bool stripBtn = false, crimpBtn = false;

            switch (Side)
            {
                case SystemConstants.DB_GROUP_WORKSIDE1:
                    stripBtnworkID = SystemConstants.STRIP1_BTN;
#if !HCSM40
                    crimpBtnworkID = SystemConstants.CRIMP1_BTN;
#endif
                    break;
                case SystemConstants.DB_GROUP_WORKSIDE2:
                    stripBtnworkID = SystemConstants.STRIP2_BTN;
#if !HCSM40
                    crimpBtnworkID = SystemConstants.CRIMP2_BTN;
#endif
                    break;
                default:
                    return false;
            }

            foreach (LearnDataStruct learn in learnData)
            {
                if (learn.ID == stripBtnworkID && learn.value == SystemConstants.BTN_ON.ToString()) stripBtn = true;
#if !HCSM40
                if (learn.ID == crimpBtnworkID && learn.value == SystemConstants.BTN_ON.ToString()) crimpBtn = true;
#endif
            }

            if (stripBtn && !crimpBtn)
                return true;
            else
                return false;
        }

        // キーに記載されているストリップ長を返す
        public double ConvKeytoStrip(int Side, string[] ItemKeys)
        {
            string stripLength = "";
            double stripLen = 0.0;

            switch (Side)
            {
                case SystemConstants.DB_GROUP_WORKSIDE1:
                    stripLength = ItemKeys[SystemConstants.DB_GROUP_KEY_STRIPLENGTH1];
                    break;
                case SystemConstants.DB_GROUP_WORKSIDE2:
                    stripLength = ItemKeys[SystemConstants.DB_GROUP_KEY_STRIPLENGTH2];
                    break;
                default:
                    return 0.0;
            }

            // double型に変換する。変換が不適切の場合、0.0を返す
            try
            {
                stripLen = Convert.ToDouble(stripLength);
            }
            catch
            {
                stripLen = 0.0;
            }

            return stripLen;
        }

        /// <summary>
        /// ストリップがLearnDataに含まれているかを確認する
        /// </summary>
        /// <param name="Side"></param>
        /// <param name="LearnData"></param>
        /// <returns></returns>
        public bool IsIncludeStripLength(int Side, LearnDataStruct[] LearnData)
        {
            int workID;
            switch (Side)
            {
                case SystemConstants.DB_GROUP_WORKSIDE1:
                    workID = SystemConstants.STRIP_LENGTH1;
                    break;
                case SystemConstants.DB_GROUP_WORKSIDE2:
                    workID = SystemConstants.STRIP_LENGTH2;
                    break;
                default:
                    return false;
            }

            foreach (LearnDataStruct learn in LearnData)
            {
                if (learn.ID == workID && learn.Type == SystemConstants.WORKMEM_TYPE_WORKDATA)
                    return true;
            }

            return false;
        }

    }
}
