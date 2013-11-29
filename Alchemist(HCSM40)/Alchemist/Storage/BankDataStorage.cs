using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Diagnostics;
using System.IO;

namespace Alchemist
{
    public struct BankDataStruct
    {
        public int Type;
        public int ID;
        public string value;
    };

    public class BankDataStorage
    {
        private XMLAccessor xmlAccessor = new BankXmlAccessor();
#if DEBUG && HCSM40
        private string xmlFileName = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\JAM\\Alchemist\\HCSM40\\bankdata.xml";
#else
        private string xmlFileName = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\JAM\\Alchemist\\bankdata.xml";
#endif
        private bool initFlag = false;

        // 選択されているバンク番号
        public int SelectedNo
        {
            get;
            set;
        }

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
        /// 指定ファイルをロードしてスキーマが正しいかチェックする
        /// </summary>
        /// <param name="filename">チェックするファイル</param>
        public bool Load(string filename)
        {
            try
            {
                // XMLファイルを開く
                xmlAccessor.LoadXmlFile(filename);
                
                // XMLファイルを読む
                var nodes = from n in xmlAccessor.document.Elements("bankdata").Elements("bank")
                            select n;

                // 見つかったノードが０ならエラーを返す。※定義データだとエラーにならないので。
                if (nodes.Count() == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                return false;

            }
        }

        /// <summary>
        /// バンクデータからBankNoに該当するデータをBankDataに構造体にして返します。
        /// 正常に終了した場合、BSPF_SUCCESSを返します。
        /// 該当バンクデータが無い場合、ERR_NO_BANK_DATAを返します。
        /// </summary>
        /// <param name="BankNo">読出すバンクナンバー</param>
        /// <param name="BankData[]">読出したバンクデータを格納する構造体</param>
        /// <returns></returns>
        public int GetBankData(int BankNo, ref BankDataStruct[] BankData)
        {
            //ロードチェック
            if(initFlag !=true){
                return SystemConstants.ERR_UNINITIALIZED;   
            }

            // バンクナンバー範囲チェック
            bool BankNoCheck = Utility.CheckRange(BankNo, 0, SystemConstants.BANK_MAX - 1);

            // チェック結果がflaseだった場合
            if (BankNoCheck == false)
            {
                // バンクナンバー範囲外のエラーを返す
                return SystemConstants.ERR_BANKNO_RANGE;
            }

            // XMLファイルを読む
            var nodes = from n in xmlAccessor.document.Elements("bankdata").Elements("bank")
                        where n.Attribute("no").Value == BankNo.ToString()
                        select n;

            // 見つかったノードが１でなければエラーを返す。
            if (nodes.Count() != 1)
            {
                BankData = new BankDataStruct[0];
                return SystemConstants.ERR_NO_BANK_DATA;
            }

            //昇順に並び変え
            nodes = from n in nodes.Elements("entry")
                    orderby n.Attribute("type").Value, n.Attribute("workid").Value
                    select n;

            int count = nodes.Count();


            // 構造体を割り当てる
            BankData = new BankDataStruct[count];


            // 構造体に読み込む
            int i = 0;
            foreach (var entry in nodes)
            {
                BankData[i].Type = Int32.Parse(entry.Attribute("type").Value);
                BankData[i].ID = Int32.Parse(entry.Attribute("workid").Value.Remove(0, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                BankData[i].value = entry.Attribute("value").Value;
                i++;
            }
            return SystemConstants.BSPF_SUCCESS;
        }

        /// <summary>
        /// バンクデータからBankNoに該当するバンクのコメントを読み出し
        /// BankCommentに入れて返します。
        /// 正常に終了した場合、BSPF_SUCCESSを返します。
        /// 該当バンクデータ無い場合、ERR_NO_BANK_DATAを返します。
        /// </summary>
        /// <param name="BankNo">読出すバンクナンバー</param>
        /// <param name="BankComment">読出したバンクコメントを格納する</param>
        /// <returns></returns>
        public int GetBankDataComment(int BankNo, ref string BankComment)
        {
            //ロードチェック
            if (initFlag != true)
            {
                return SystemConstants.ERR_UNINITIALIZED;
            }

            // バンクナンバー範囲チェック
            bool BankNoCheck = Utility.CheckRange(BankNo, 0, SystemConstants.BANK_MAX - 1);
            // チェック結果がflaseだった場合
            if (BankNoCheck == false)
            {
                // バンクナンバー範囲外のエラーを返す
                return SystemConstants.ERR_BANKNO_RANGE;
            }

            // XMLファイルを読む
            var nodes = from n in xmlAccessor.document.Elements("bankdata").Elements("bank")
                        where n.Attribute("no").Value == BankNo.ToString()
                        select n;

            // 見つかったノードが１でなければエラーを返す。
            if (nodes.Count() != 1)
            {
                return SystemConstants.ERR_NO_BANK_DATA;
            }

            //コメント取得
            var bank = nodes.First();
            BankComment = bank.Attribute("comment").Value;

            return SystemConstants.BSPF_SUCCESS;
        }

        /// <summary>
        /// バンクデータのBandkNoにBankDataで渡される(Type,ID,値)の構造体一覧を保存する。
        /// BankNoが存在しない場合は、新規に追加し、存在している場合は、上書きを行います。
        /// 正常に終了した場合は、BSPF＿SUCCESSを返します。
        /// </summary>
        /// <param name="BankNo">書き込むバンクナンバー</param>
        /// <param name="BankData[]">書き込むバンクデータを格納する構造体</param>
        /// <returns></returns>
        public int WriteBankData(int BankNo, BankDataStruct[] BankData)
        {
            //ロードチェック
            if (initFlag != true)
            {
                return SystemConstants.ERR_UNINITIALIZED;
            }

            // バンクナンバー範囲チェック
            bool BankNoCheck = Utility.CheckRange(BankNo, 0, SystemConstants.BANK_MAX - 1);
            // チェック結果がflaseだった場合
            if (BankNoCheck == false)
            {
                // バンクナンバー範囲外のエラーを返す
                return SystemConstants.ERR_BANKNO_RANGE;
            }

            // バンクの番号
            xmlAccessor.document.Element("bankdata").Attribute("selectedno").Value = SelectedNo.ToString();

            // バンク番号からノードを見つける
            // XMLファイルを読む
            var nodes = from n in xmlAccessor.document.Elements("bankdata").Elements("bank")
                        where n.Attribute("no").Value == BankNo.ToString()
                        select n;
            XElement bank;
            // バンクがある場合
            if (nodes.Count() >= 1)
            {
                // エントリを消す
                bank = nodes.First();
                bank.RemoveNodes();
            }
            // バンクがない場合
            else
            {
                // バンクを作る
                bank = new XElement("bank");
                bank.SetAttributeValue("no", BankNo);
                bank.SetAttributeValue("comment", "");
                xmlAccessor.document.Element("bankdata").Add(bank);
            }
            // エントリを作る
            for (int i = 0; i < BankData.Length; i++)
            {
                var elem = new XElement("entry");

                elem.SetAttributeValue("type", BankData[i].Type);
                elem.SetAttributeValue("workid", string.Format("0x{0:X4}", BankData[i].ID));
                elem.SetAttributeValue("value", BankData[i].value);
                bank.Add(elem);
            }
            // XMLファイルにセーブする
            try
            {
                // XMLファイルを開く
                xmlAccessor.SaveXmlFile(xmlFileName);
            }
            catch (Exception)
            {
                return SystemConstants.ERR_BANK_FILE_ERROR;
            }
            finally
            {
                Utility.DeleteBackupFile(xmlFileName);
            }

            return SystemConstants.BSPF_SUCCESS;

        }

        /// <summary>
        /// バンクデータのBandkNoにBankCommentを保存する。
        /// BankNoが存在しない場合は、新規に追加し、存在している場合は、上書きを行います。
        /// 正常に終了した場合は、BSPF_SUCCESSを返します。
        /// </summary>
        /// <param name="BankNo">書き込むバンクナンバー</param>
        /// <param name="BankComment">書き込むバンクコメント</param>
        /// <returns></returns>
        public int WriteBankDataComment(int BankNo, string BankComment)
        {
            //ロードチェック
            if (initFlag != true)
            {
                return SystemConstants.ERR_UNINITIALIZED;
            }

            // バンクナンバー範囲チェック
            bool BankNoCheck = Utility.CheckRange(BankNo, 0, SystemConstants.BANK_MAX - 1);
            // チェック結果がflaseだった場合
            if (BankNoCheck == false)
            {
                // バンクナンバー範囲外のエラーを返す
                return SystemConstants.ERR_BANKNO_RANGE;
            }

            // バンク番号からノードを見つける
            // XMLファイルを読む
            var nodes = from n in xmlAccessor.document.Elements("bankdata").Elements("bank")
                        where n.Attribute("no").Value == BankNo.ToString()
                        select n;

            XElement bank;
            // バンクがある場合
            if (nodes.Count() >= 1)
            {

                bank = nodes.First();
                // コメントがある場合
                if (bank.Attribute("comment") != null)
                {
                    // コメントを消す
                    bank.Attribute("comment").Remove();
                    bank.SetAttributeValue("comment", BankComment);
                }
                // コメントがない場合
                else
                {
                    // コメントを作成
                    bank.SetAttributeValue("comment", BankComment);
                }
            }
            // バンクがない場合
            else
            {
                // バンクを作る
                bank = new XElement("bank");
                bank.SetAttributeValue("no", BankNo);
                bank.SetAttributeValue("comment", BankComment);
                xmlAccessor.document.Element("bankdata").Add(bank);
            }
            // XMLファイルにセーブする
            try
            {
                // XMLファイルを開く
                xmlAccessor.SaveXmlFile(xmlFileName);

            }
            catch (Exception)
            {
                return SystemConstants.ERR_BANK_FILE_ERROR;
            }
            finally
            {
                Utility.DeleteBackupFile(xmlFileName);
            }

            return SystemConstants.BSPF_SUCCESS;

        }

        /// <summary>
        /// バンクデータのBankNoのデータを削除します。
        /// 正常に終了した場合は、BSPF_SUCCESSを返します。
        /// </summary>
        /// <param name="BankNo">削除するバンクナンバー</param>
        /// <returns></returns>
        public int DeleteBankData(int BankNo)
        {
            //ロードチェック
            if (initFlag != true)
            {
                return SystemConstants.ERR_UNINITIALIZED;
            }

            // バンクナンバー範囲チェック
            bool BankNoCheck = Utility.CheckRange(BankNo, 0, SystemConstants.BANK_MAX - 1);
            // チェック結果がflaseだった場合
            if (BankNoCheck == false)
            {
                // バンクナンバー範囲外のエラーを返す
                return SystemConstants.ERR_BANKNO_RANGE;
            }

            // バンク番号からノードを見つける
            // XMLファイルを読む
            var nodes = from n in xmlAccessor.document.Elements("bankdata").Elements("bank")
                        where n.Attribute("no").Value == BankNo.ToString()
                        select n;

            // バンクがある場合
            if (nodes.Count() >= 1)
            {
                // バンクを削除する
                nodes.Remove();
            }
            // バンクがない場合
            else
            {
                //処理なし
            }

            // XMLファイルにセーブする
            try
            {
                // XMLファイルを開く
                xmlAccessor.SaveXmlFile(xmlFileName);
            }
            catch (Exception)
            {
                return SystemConstants.ERR_BANK_FILE_ERROR;
            }
            finally
            {
                Utility.DeleteBackupFile(xmlFileName);
            }

            return SystemConstants.BSPF_SUCCESS;

        }

        /// <summary>
        /// SourceBankNoのバンクデータ(含むコメント)をDestBankNoへコピーする
        /// 正常に終了した場合は、BSPF_SUCCESSを返します。
        /// </summary>
        /// <param name="SourceBankNo">コピー元のバンクナンバー</param>
        /// <param name="DestBankNo">コピー先のバンクナンバー</param>
        /// <returns></returns>
        public int CopyBankData(int SourceBankNo, int DestBankNo)
        {
            //ロードチェック
            if (initFlag != true)
            {
                return SystemConstants.ERR_UNINITIALIZED;
            }

            // コピー元バンクナンバー範囲チェック
            bool SourceBankNoCheck = Utility.CheckRange(SourceBankNo, 0, SystemConstants.BANK_MAX - 1);
            // コピー先バンクナンバー範囲チェック
            bool DestBankNoNoCheck = Utility.CheckRange(DestBankNo, 0, SystemConstants.BANK_MAX - 1);
            // チェック結果がflaseだった場合
            if (SourceBankNoCheck == false || DestBankNoNoCheck == false)
            {
                // バンクナンバー範囲外のエラーを返す
                return SystemConstants.ERR_BANKNO_RANGE;
            }

            string comment = "";

            int ret;

            BankDataStruct[] BankData = null;
            // バンクデータを取得
            ret = GetBankData(SourceBankNo, ref BankData);
            if (ret != SystemConstants.BSPF_SUCCESS)
            {
                return ret;
            }

            // バンクデータを書き込み
            ret = WriteBankData(DestBankNo, BankData);
            if (ret != SystemConstants.BSPF_SUCCESS)
            {
                return ret;
            }
            // コメントを読み込み
            ret = GetBankDataComment(SourceBankNo, ref comment);
            if (ret != SystemConstants.BSPF_SUCCESS)
            {
                return ret;
            }
            // コメントを書き込み
            ret = WriteBankDataComment(DestBankNo, comment);
            if (ret != SystemConstants.BSPF_SUCCESS)
            {
                return ret;
            }

            return SystemConstants.BSPF_SUCCESS;
        }

        /// <summary>
        /// bandata.xmlからselectednoを取得する
        /// </summary>
        /// <param name="SelectedNo"></param>
        /// <returns></returns>
        public int GetSelectedNo(ref int SelectedNo)
        {
            //ロードチェック
            if (initFlag != true)
            {
                return SystemConstants.ERR_UNINITIALIZED;
            }

            // XMLファイルを読む
            var nodes = from n in xmlAccessor.document.Elements("bankdata")
                        select n;


            //selectedno取得
            var bank = nodes.First();
            SelectedNo = Int32.Parse(bank.Attribute("selectedno").Value);

            return SystemConstants.BSPF_SUCCESS;
        }

        /// <summary>
        /// bankdata.xmlにSelectedNoを書き込む
        /// </summary>
        /// <param name="SelectedNo"></param>
        /// <returns></returns>
        public int WriteSelectedNo(int SelectedNo)
        {
            //ロードチェック
            if (initFlag != true)
            {
                return SystemConstants.ERR_UNINITIALIZED;
            }

            this.SelectedNo = SelectedNo;

            // バンク番号からノードを見つける
            // XMLファイルを読む
            var nodes = from n in xmlAccessor.document.Elements("bankdata")
                        select n;

            XElement bank = nodes.First();

            bank.SetAttributeValue("selectedno", SelectedNo);

            // XMLファイルにセーブする
            try
            {
                // XMLファイルを開く
                xmlAccessor.SaveXmlFile(xmlFileName);

            }
            catch (Exception)
            {
                return SystemConstants.ERR_BANK_FILE_ERROR;
            }
            finally
            {
                Utility.DeleteBackupFile(xmlFileName);
            }

            return SystemConstants.BSPF_SUCCESS;
        }
    }
}
