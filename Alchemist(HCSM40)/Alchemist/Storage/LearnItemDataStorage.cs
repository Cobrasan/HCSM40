using System;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

namespace Alchemist
{
    public struct LearnItemDataStruct
    {
        public string name;
        public string code;
    }

    public class LearnItemDataStorage
    {
        private static XMLAccessor xmlAccessor = new LearnItemDataXMLAccessor();
#if DEBUG && HCSM40
        private string xmlFileName = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\JAM\\Alchemist\\HCSM40\\learnitemdata.xml";
#else
        private string xmlFileName = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\JAM\\Alchemist\\learnitemdata.xml";
#endif
        private bool initFlag = false;

        /// <summary>
        /// system.xmlからシステムデータを読み込み、クラス内に値を保持します。
        /// ファイルにアクセスする際は、世代管理を行います。
        /// ファイルが見つからない場合、読み込みができない場合は、例外を発生させます。
        /// </summary>
        /// <returns></returns>
        public void Load()
        {
            try
            {
                // XMLファイルを開く
                xmlAccessor.LoadXmlFile(xmlFileName);

            }
            catch{
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
        /// タイプから要素名を返す
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        private string TypeToElementName(int Type)
        {
            string type = "";
            switch(Type)
            {
                case SystemConstants.LEARN_ITEM_WIRETYPE:
                    type = "wiretype";
                    break;
                case SystemConstants.LEARN_ITEM_CORESIZE:
                    type = "coresize";
                    break;
                case SystemConstants.LEARN_ITEM_COLOR1:
                    type = "color1";
                    break;
                case SystemConstants.LEARN_ITEM_COLOR2:
                    type = "color2";
                    break;
            }
            return type;
        }

        /// <summary>
        /// 指定されたタイプの名前のリストを返す
        /// </summary>
        /// <returns></returns>
        public string[] GetNames(int Type)
        {
            IEnumerable<string> keys;
            string elem = TypeToElementName(Type);
            keys = (from n in xmlAccessor.document.Elements("learnitemdata").Elements(elem).Elements("item")
                    select n.Attribute("name").Value);                    

            string[] result = keys.ToArray();

            return result;
        }

        /// <summary>
        /// 指定されたタイプの名前からコードを返す
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Code"></param>
        /// <returns>
        /// データがあれば、LIPF_SUCCESSとCodeを返す
        /// データがなければ、ERR_LIPF_NO_DATAを返す
        /// </returns>
        public int GetCode(string Name, ref string Code, int Type)
        {
            string elem = TypeToElementName(Type);

            var nodes = from n in xmlAccessor.document.Elements("learnitemdata").Elements(elem).Elements("item")
                        where n.Attribute("name").Value == Name
                        select n;
            XElement item;
            if (nodes.Count() >= 1)
            {
                item = nodes.First();
                Code = item.Attribute("code").Value;
                return SystemConstants.LIPF_SUCCESS;
            }
            else
            {
                return SystemConstants.ERR_LIPF_NO_DATA;
            }
        }

        /// <summary>
        /// 項目の登録
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Code"></param>
        /// <returns></returns>
        public int WriteItem(string Name, string Code, int Type)
        {
            //ロードチェック
            if (initFlag != true)
            {
                return SystemConstants.ERR_UNINITIALIZED;
            }

            string elem = TypeToElementName(Type);

            // Nameからノードを見つける
            var nodes = from n in xmlAccessor.document.Elements("learnitemdata").Elements(elem).Elements("item")
                        where n.Attribute("name").Value == Name
                        select n;
            XElement item;
            // 登録がある場合
            if (nodes.Count() >= 1)
            {
                // エントリを消して再登録
                nodes.Remove();
            }
           
            // 作る
            item = new XElement("item");
            item.SetAttributeValue("name", Name);
            item.SetAttributeValue("code", Code);
            xmlAccessor.document.Element("learnitemdata").Element(elem).Add(item);
            
            // XMLファイルにセーブする
            try
            {
                // XMLファイルを開く
                xmlAccessor.SaveXmlFile(xmlFileName);
            }
            catch (Exception)
            {
                // ファイルに保存されないけどメモリのノードに追加されちゃうから消す
                item.Remove();
                return SystemConstants.ERR_LIPF_FILE_ERROR;
            }
            finally
            {
                Utility.DeleteBackupFile(xmlFileName);
            }

            return SystemConstants.LIPF_SUCCESS;
        }

        /// <summary>
        /// 項目の削除
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Code"></param>
        /// <returns></returns>
        public int DeleteItem(string Name, int Type)
        {
            //ロードチェック
            if (initFlag != true)
            {
                return SystemConstants.ERR_UNINITIALIZED;
            }

            string elem = TypeToElementName(Type);

            // Nameからノードを見つける
            var nodes = from n in xmlAccessor.document.Elements("learnitemdata").Elements(elem).Elements("item")
                        where n.Attribute("name").Value == Name
                        select n;
            
            // 登録がある場合
            if (nodes.Count() >= 1)
            {
                // エントリを消して再登録
                nodes.Remove();
            }

            // XMLファイルにセーブする
            try
            {
                // XMLファイルを開く
                xmlAccessor.SaveXmlFile(xmlFileName);
            }
            catch (Exception)
            {
                return SystemConstants.ERR_LIPF_FILE_ERROR;
            }
            finally
            {
                Utility.DeleteBackupFile(xmlFileName);
            }

            return SystemConstants.LIPF_SUCCESS;
        }

    }
}
