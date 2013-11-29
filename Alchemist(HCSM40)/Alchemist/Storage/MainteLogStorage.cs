using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Diagnostics;
using System.IO;

namespace Alchemist
{
    public struct MainteLogStruct
    {
        public int Unit;
        public int LogType;
        public string DateStr;
        public string TimeStr;
        public int WorkIDType;
        public int WorkID;
        public string OldValue;
        public string NewValue;
        public string Comment;
    }

    public partial class MainteLogStorage
    {
        public MainteLogStorage()
        {

        }

        public void Initialize()
        {
            // グループコードとユニットコードの対比マップを作成する
            groupCodemap.Clear();
#if MAINTELOG
            int offset;
#if HCSM40
            // 補正値
            offset = 0x10000;
            groupCodemap.Add(SystemConstants.CORR_GROUP_BASEMACHINE1 + offset, SystemConstants.MAINTELOG_CORR_BASEMACHINE);
            groupCodemap.Add(SystemConstants.CORR_GROUP_TRANS1 + offset, SystemConstants.MAINTELOG_CORR_TRANSFER1);
            groupCodemap.Add(SystemConstants.CORR_GROUP_TRANS2 + offset, SystemConstants.MAINTELOG_CORR_TRANSFER2);
            groupCodemap.Add(SystemConstants.CORR_GROUP_CRIMP1 + offset, SystemConstants.MAINTELOG_CORR_CRIMP1);
            groupCodemap.Add(SystemConstants.CORR_GROUP_EJECT1 + offset, SystemConstants.MAINTELOG_CORR_EJECT1);
            groupCodemap.Add(SystemConstants.CORR_GROUP_FEED1 + offset, SystemConstants.MAINTELOG_CORR_FEED1);
            groupCodemap.Add(SystemConstants.CORR_GROUP_SEAL1 + offset, SystemConstants.MAINTELOG_CORR_SEAL1);
            groupCodemap.Add(SystemConstants.CORR_GROUP_SLIDER1 + offset, SystemConstants.MAINTELOG_CORR_SLIDER1);
            groupCodemap.Add(SystemConstants.CORR_GROUP_SLIDER2 + offset, SystemConstants.MAINTELOG_CORR_SLIDER2);
            groupCodemap.Add(SystemConstants.CORR_GROUP_STRIP1 + offset, SystemConstants.MAINTELOG_CORR_STRIP1);

            // タイミング
            offset = 0x20000;
            groupCodemap.Add(SystemConstants.TIMM_GROUP_SIDE1 + offset, SystemConstants.MAINTELOG_TIMM_SIDE1);
            groupCodemap.Add(SystemConstants.TIMM_GROUP_SIDE2 + offset, SystemConstants.MAINTELOG_TIMM_SIDE2);
            groupCodemap.Add(SystemConstants.TIMM_GROUP_SIDE3 + offset, SystemConstants.MAINTELOG_TIMM_EJECT1);
            groupCodemap.Add(SystemConstants.TIMM_GROUP_SEAL1 + offset, SystemConstants.MAINTELOG_TIMM_SEAL1);
            groupCodemap.Add(SystemConstants.TIMM_GROUP_SEAL2 + offset, SystemConstants.MAINTELOG_TIMM_SEAL2);
#endif
#endif
        }

        private XMLAccessor xmlAccessor = new MainteLogXMLAccessor();
#if DEBUG && HCSM40
        private string xmlFileName = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\JAM\\Alchemist\\HCSM40\\maintelog.xml";
#else
        private string xmlFileName = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\JAM\\Alchemist\\maintelog.xml";
#endif
        private bool initFlag = false;
        Dictionary<int, int> groupCodemap = new Dictionary<int, int>();


        /// <summary>
        /// 既存のログをロードする
        /// </summary>
        public void Load()
        {
            try
            {
                // XMLファイルを開く
                xmlAccessor.LoadXmlFile(xmlFileName);
            }
            catch (FileNotFoundException)
            {
                // ファイルが無ければ作る
                xmlAccessor.NewDocument();
                xmlAccessor.SaveXmlFile(xmlFileName);
            }
            finally
            {
                initFlag = true;
            }
        }

        /// <summary>
        /// 保全履歴を追加する
        /// </summary>
        /// <param name="UnitCode"></param>
        /// <param name="WorkID"></param>
        /// <param name="OldValue"></param>
        /// <param name="NewValue"></param>
        /// <returns></returns>
        public int AddRecord(int UnitCode, int WorkIDType, int WorkID, string OldValue, string NewValue)
        {
#if MAINTELOG
            // ロードチェック
            if (!initFlag) return SystemConstants.ERR_RLPF_UNINITIALIZED;

            // 日付、時間を作成する
            string dateStr, timeStr;
            dateStr = DateTime.Now.ToString("yyyy-MM-dd");
            timeStr = DateTime.Now.ToString("HH:mm:ss.fffzzz");

            // 記録ノードをを作成する
            XElement mainteRecord = new XElement("record");
            mainteRecord.SetAttributeValue("type", SystemConstants.MAINTELOG_RECORD_TYPE_WORKID);
            mainteRecord.SetAttributeValue("date", dateStr);
            mainteRecord.SetAttributeValue("time", timeStr);
            xmlAccessor.document.Element("maintelog").Add(mainteRecord);

            // ユニットコードを作成する
            var elem = new XElement("unit");
            elem.SetAttributeValue("code", UnitCode);
            mainteRecord.Add(elem);

            // データ更新記録を作成する
            var elem2 = new XElement("value_change");
            elem2.SetAttributeValue("workidtype", WorkIDType.ToString());
            elem2.SetAttributeValue("workid", string.Format("0x{0:X4}", WorkID));
            elem2.SetAttributeValue("old", OldValue);
            elem2.SetAttributeValue("new", NewValue);
            mainteRecord.Add(elem2);

            // XMLファイルにセーブする
            try
            {
                xmlAccessor.SaveXmlFile(xmlFileName);
            }
            catch (Exception)
            {
                return SystemConstants.ERR_RLPF_FILE_ERROR;
            }
            finally
            {
                Utility.DeleteBackupFile(xmlFileName);
            }
#endif
            return SystemConstants.RLPF_SUCCESS;
        }

        /// <summary>
        /// 保全履歴を追加する
        /// </summary>
        /// <param name="UnitCode"></param>
        /// <param name="WorkID"></param>
        /// <param name="OldValue"></param>
        /// <param name="NewValue"></param>
        /// <returns></returns>
        public int AddRecord(int UnitCode, string Comment)
        {
            // ロードチェック
            if (!initFlag) return SystemConstants.ERR_RLPF_UNINITIALIZED;

            // 日付、時間を作成する
            string dateStr, timeStr;
            dateStr = DateTime.Now.ToString("yyyy-MM-dd");
            timeStr = DateTime.Now.ToString("HH:mm:ss.fffzzz");

            // 記録ノードをを作成する
            XElement mainteRecord = new XElement("record");
            mainteRecord.SetAttributeValue("type", SystemConstants.MAINTELOG_RECORD_TYPE_COMMENT);
            mainteRecord.SetAttributeValue("date", dateStr);
            mainteRecord.SetAttributeValue("time", timeStr);
            xmlAccessor.document.Element("maintelog").Add(mainteRecord);

            // ユニットコードを作成する
            var elem = new XElement("unit");
            elem.SetAttributeValue("code", UnitCode);
            mainteRecord.Add(elem);

            // データ更新記録を作成する
            var elem2 = new XElement("maintenance");
            elem2.SetAttributeValue("comment", Comment);
            mainteRecord.Add(elem2);

            // XMLファイルにセーブする
            try
            {
                xmlAccessor.SaveXmlFile(xmlFileName);
            }
            catch (Exception)
            {
                return SystemConstants.ERR_RLPF_FILE_ERROR;
            }
            finally
            {
                Utility.DeleteBackupFile(xmlFileName);
            }

            return SystemConstants.RLPF_SUCCESS;
        }

        /// <summary>
        /// ログの一覧を取得する
        /// </summary>
        /// <returns></returns>
        public MainteLogStruct[] GetRecords()
        {
            MainteLogStruct[] recordarr = new MainteLogStruct[0];

            // ロードチェック
            if (!initFlag) return recordarr;

            // XMLファイルを読み出す
            var nodes = from n in xmlAccessor.document.Elements("maintelog").Elements("record")
                        orderby n.Attribute("date").Value, n.Attribute("time").Value descending
                        select n;

            // 配列を確保する
            int count = nodes.Count();
            Array.Resize(ref recordarr, count);

            // 配列を構造体にいれる
            int i = 0;
            foreach (var record in nodes)
            {
                structInitialize(ref recordarr[i]);
                recordarr[i].LogType = Int32.Parse(record.Attribute("type").Value);
                recordarr[i].DateStr = record.Attribute("date").Value;
                recordarr[i].TimeStr = record.Attribute("time").Value;
                recordarr[i].Unit = Int32.Parse(record.Element("unit").Attribute("code").Value);

                switch (recordarr[i].LogType)
                {
                    case SystemConstants.MAINTELOG_RECORD_TYPE_WORKID:
                        recordarr[i].WorkIDType = Int32.Parse(record.Element("value_change").Attribute("workidtype").Value);
                        recordarr[i].WorkID = Int32.Parse(record.Element("value_change").Attribute("workid").Value.Remove(0, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                        recordarr[i].OldValue = record.Element("value_change").Attribute("old").Value;
                        recordarr[i].NewValue = record.Element("value_change").Attribute("new").Value;
                        break;
                    case SystemConstants.MAINTELOG_RECORD_TYPE_COMMENT:
                        recordarr[i].Comment = record.Element("maintenance").Attribute("comment").Value;
                        break;
                }

                i++;
            }
            return recordarr;
        }

        // ログ構造体を初期化する
        private void structInitialize(ref MainteLogStruct LogStruct)
        {
            LogStruct.LogType = 0;
            LogStruct.TimeStr = "";
            LogStruct.DateStr = "";
            LogStruct.Unit = 0;
            LogStruct.WorkIDType = 0;
            LogStruct.WorkID = 0;
            LogStruct.OldValue = "";
            LogStruct.NewValue = "";
            LogStruct.Comment = "";
        }

        /// <summary>
        /// WorkIDに対応するユニットコードを返す
        /// </summary>
        /// <param name="WorkIDType"></param>
        /// <param name="WorkIDGroupCode"></param>
        /// <returns></returns>
        public int GetUnitCode(int WorkIDType, int WorkIDGroupCode)
        {

            int unitCode = 0, key = 0;
            int offset = 0;

            switch (WorkIDType)
            {
                case SystemConstants.WORKID_TYPE_CORRECTDATA:
                    offset = 0x10000;
                    break;
                case SystemConstants.WORKID_TYPE_TIMINGDATA:
                    offset = 0x20000;
                    break;
            }

            key = offset + WorkIDGroupCode;

            // mapからデータを検索する
            try
            {
                groupCodemap.TryGetValue(key, out unitCode);
            }
            catch { }

            // データを返す
            return unitCode;
        }


    }
}
