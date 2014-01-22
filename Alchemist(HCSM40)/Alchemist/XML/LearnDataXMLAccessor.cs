using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Linq;

namespace Alchemist
{
    public class LearnDataXMLAccessor : XMLAccessor
    {
        private const string schema = @"
<xsd:schema xmlns:xsd='http://www.w3.org/2001/XMLSchema'>
  <!-- address_string型 -->
  <xsd:simpleType name='address_string'>
    <xsd:restriction base='xsd:string'>
      <xsd:pattern value='^0x[0-9A-F]{4}' />
    </xsd:restriction>
  </xsd:simpleType>

  <!-- value_number型 -->
  <xsd:simpleType name='value_number'>
    <xsd:restriction base='xsd:double' />
  </xsd:simpleType>

  <!-- workid_type -->
  <xsd:simpleType name='type_number'>
    <xsd:restriction base='xsd:string'>
      <xsd:enumeration value='3' />
      <xsd:enumeration value='4' />
      <xsd:enumeration value='5' />
    </xsd:restriction>
  </xsd:simpleType>

  <!-- key_string 型 -->
  <xsd:simpleType name='key_string'>
    <xsd:restriction base='xsd:string' />
  </xsd:simpleType>

  <!-- keytype_number型 -->
  <xsd:simpleType name='keytype_value'>
    <xsd:restriction base='xsd:string'>
      <xsd:enumeration value='0' />
      <xsd:enumeration value='1' />
      <xsd:enumeration value='2' />
      <xsd:enumeration value='3' />
      <xsd:enumeration value='4' />
      <xsd:enumeration value='5' />
      <xsd:enumeration value='6' />
      <xsd:enumeration value='7' />
    </xsd:restriction>
  </xsd:simpleType>

  <!-- learndata要素定義 -->
  <xsd:element name='learndata' type='learndata_type'>
    <!-- 主キー設定 -->
    <xsd:unique name='pkSearchKey'>
      <xsd:selector xpath='searchkey' />
      <xsd:field xpath='@type' />
      <xsd:field xpath='@key0' />
      <xsd:field xpath='@key1' />
      <xsd:field xpath='@key2' />
      <xsd:field xpath='@key3' />
      <xsd:field xpath='@key4' />
      <xsd:field xpath='@key5' />
      <xsd:field xpath='@key6' />
      <xsd:field xpath='@key7' />
      <xsd:field xpath='@key8' />
      <xsd:field xpath='@key9' />
    </xsd:unique>
  </xsd:element>

  <xsd:complexType name='learndata_type'>
    <xsd:sequence>
      <xsd:element minOccurs='1' maxOccurs='unbounded' name='searchkey' type='searchkey_type' />
    </xsd:sequence>
    <!-- 選択内容属性定義 -->
    <xsd:attribute name='key0' use='required' type='key_string' />
    <xsd:attribute name='key1' use='required' type='key_string' />
    <xsd:attribute name='key2' use='required' type='key_string' />
    <xsd:attribute name='key3' use='required' type='key_string' />
    <xsd:attribute name='key4' use='required' type='key_string' />
    <xsd:attribute name='key5' use='required' type='key_string' />
    <xsd:attribute name='key6' use='required' type='key_string' />
    <xsd:attribute name='key7' use='required' type='key_string' />
    <xsd:attribute name='key8' use='required' type='key_string' />
    <xsd:attribute name='key9' use='required' type='key_string' />
  </xsd:complexType>

  <!-- searchkey要素定義 -->
  <xsd:complexType name='searchkey_type'>
    <xsd:sequence>
      <xsd:element minOccurs='0' maxOccurs='unbounded' name='entry' type='entry_type' />
    </xsd:sequence>

    <xsd:attribute name='type' use='required' type='keytype_value' />
    <xsd:attribute name='key0' use='required' type='key_string' />
    <xsd:attribute name='key1' use='required' type='key_string' />
    <xsd:attribute name='key2' use='required' type='key_string' />
    <xsd:attribute name='key3' use='required' type='key_string' />
    <xsd:attribute name='key4' use='required' type='key_string' />
    <xsd:attribute name='key5' use='required' type='key_string' />
    <xsd:attribute name='key6' use='required' type='key_string' />
    <xsd:attribute name='key7' use='required' type='key_string' />
    <xsd:attribute name='key8' use='required' type='key_string' />
    <xsd:attribute name='key9' use='required' type='key_string' />
  </xsd:complexType>

  <!-- entry 要素定義 -->
  <xsd:complexType name='entry_type'>
    <!-- 空要素 -->
    <xsd:sequence />

    <xsd:attribute name='workid' use ='required' type='address_string' />
    <xsd:attribute name='value' use='required' type='value_number' />
    <xsd:attribute name='type' use='required' type='type_number' />

  </xsd:complexType>
</xsd:schema>
";

        protected override string GetSchema()
        {
            return schema;
        }

        public override void NewDocument()
        {
            doc = new XDocument(new XElement("learndata", new XElement("searchkey")));
            doc.Element("learndata").SetAttributeValue("key0", "");
            doc.Element("learndata").SetAttributeValue("key1", "");
            doc.Element("learndata").SetAttributeValue("key2", "");
            doc.Element("learndata").SetAttributeValue("key3", "");
            doc.Element("learndata").SetAttributeValue("key4", "");
            doc.Element("learndata").SetAttributeValue("key5", "");
            doc.Element("learndata").SetAttributeValue("key6", "");
            doc.Element("learndata").SetAttributeValue("key7", "");
            doc.Element("learndata").SetAttributeValue("key8", "");
            doc.Element("learndata").SetAttributeValue("key9", "");
            
            doc.Element("learndata").Element("searchkey").SetAttributeValue("type", 0);
            doc.Element("learndata").Element("searchkey").SetAttributeValue("key0", "");
            doc.Element("learndata").Element("searchkey").SetAttributeValue("key1", "");
            doc.Element("learndata").Element("searchkey").SetAttributeValue("key2", "");
            doc.Element("learndata").Element("searchkey").SetAttributeValue("key3", "");
            doc.Element("learndata").Element("searchkey").SetAttributeValue("key4", "");
            doc.Element("learndata").Element("searchkey").SetAttributeValue("key5", "");
            doc.Element("learndata").Element("searchkey").SetAttributeValue("key6", "");
            doc.Element("learndata").Element("searchkey").SetAttributeValue("key7", "");
            doc.Element("learndata").Element("searchkey").SetAttributeValue("key8", "");
            doc.Element("learndata").Element("searchkey").SetAttributeValue("key9", "");
        }

    }
}
