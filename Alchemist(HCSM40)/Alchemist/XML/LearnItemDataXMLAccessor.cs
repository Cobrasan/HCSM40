using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Linq;

namespace Alchemist
{
    public class LearnItemDataXMLAccessor : XMLAccessor
    {
        private const string schema = @"
<xsd:schema xmlns:xsd='http://www.w3.org/2001/XMLSchema'>

  <!-- learnitemdata要素定義 -->
  <xsd:element name='learnitemdata' type='learnitemdata_type'/>
  
  <xsd:complexType name='learnitemdata_type'>
    <xsd:sequence>
      <xsd:element name='wiretype' type='wiretype_type'>
        <xsd:unique name='wireTypeName'>
          <xsd:selector xpath='item' />
          <xsd:field xpath='@name' />
        </xsd:unique>
      </xsd:element>
      <xsd:element name='coresize' type='coresize_type'>
        <xsd:unique name='coreSizeName'>
          <xsd:selector xpath='item' />
          <xsd:field xpath='@name' />
        </xsd:unique>
      </xsd:element>
      <xsd:element name='color1' type='color1_type'>
        <xsd:unique name='color1Name'>
          <xsd:selector xpath='item' />
          <xsd:field xpath='@name' />
        </xsd:unique>
      </xsd:element>
      <xsd:element name='color2' type='color2_type'>
        <xsd:unique name='color2Name'>
          <xsd:selector xpath='item' />
          <xsd:field xpath='@name' />
        </xsd:unique>
      </xsd:element>
    </xsd:sequence>
  </xsd:complexType>

  <!-- 電線種要素型 -->
  <xsd:complexType name='wiretype_type'>
    <xsd:sequence>
      <xsd:element name='item' minOccurs='0' maxOccurs='unbounded' type='wiretype_item'/>
    </xsd:sequence>
  </xsd:complexType>

  <!-- コアサイズ要素型 -->
  <xsd:complexType name='coresize_type'>
    <xsd:sequence>
      <xsd:element name='item' minOccurs='0' maxOccurs='unbounded' type='coresize_item'/>
    </xsd:sequence>
  </xsd:complexType>

  <!-- 色1要素型 -->
  <xsd:complexType name='color1_type'>
    <xsd:sequence>
      <xsd:element name='item' minOccurs='0' maxOccurs='unbounded' type='color1_item'/>
    </xsd:sequence>
  </xsd:complexType>

  <!-- 色2要素型 -->
  <xsd:complexType name='color2_type'>
    <xsd:sequence>
      <xsd:element name='item' minOccurs='0' maxOccurs='unbounded' type='color2_item'/>
    </xsd:sequence>
  </xsd:complexType>


  <!-- 電線種アイテム型 -->
  <xsd:complexType name='wiretype_item'>
    <xsd:sequence/>
    <xsd:attribute name ='name' use='required' type='wiretype_name'/>
    <xsd:attribute name ='code' use='required' type='wire_code'/>
  </xsd:complexType>

  <!-- コアサイズアイテム型 -->
  <xsd:complexType name='coresize_item'>
    <xsd:sequence/>
    <xsd:attribute name ='name' use='required' type='coresize_name'/>
    <xsd:attribute name ='code' use='required' type='wire_code'/>
  </xsd:complexType>

  <!-- 色1アイテム型 -->
  <xsd:complexType name='color1_item'>
    <xsd:sequence/>
    <xsd:attribute name ='name' use='required' type='color1_name'/>
    <xsd:attribute name ='code' use='required' type='color_code'/>
  </xsd:complexType>

  <!-- 色2アイテム型 -->
  <xsd:complexType name='color2_item'>
    <xsd:sequence/>
    <xsd:attribute name ='name' use='required' type='color2_name'/>
    <xsd:attribute name ='code' use='required' type='color_code'/>
  </xsd:complexType>

  <!-- 電線種名前型 -->
  <xsd:simpleType name='wiretype_name'>
    <xsd:restriction base='xsd:string'>
      <xsd:length value='5'/>
      <xsd:whiteSpace value='preserve'/>      
    </xsd:restriction>
  </xsd:simpleType>

  <!-- コアサイズ名前型 -->
  <xsd:simpleType name='coresize_name'>
    <xsd:restriction base='xsd:string'>
      <xsd:length value='6'/>
      <xsd:whiteSpace value='preserve'/>
    </xsd:restriction>
  </xsd:simpleType>

  <!-- 色1名前型 -->
  <xsd:simpleType name='color1_name'>
    <xsd:restriction base='xsd:string'>
      <xsd:length value='2'/>
      <xsd:whiteSpace value='preserve'/>
    </xsd:restriction>
  </xsd:simpleType>

  <!-- 色2名前型 -->
  <xsd:simpleType name='color2_name'>
    <xsd:restriction base='xsd:string'>
      <xsd:length value='3'/>
      <xsd:whiteSpace value='preserve'/>
    </xsd:restriction>
  </xsd:simpleType>

  <!-- 電線コード型 -->
  <xsd:simpleType name='wire_code'>
    <xsd:restriction base='xsd:string'>
      <xsd:pattern value='[0-9]{3}'/>
    </xsd:restriction>
  </xsd:simpleType>

  <!-- 色コード型 -->
  <xsd:simpleType name='color_code'>
    <xsd:restriction base='xsd:string'>
      <xsd:pattern value='[A-Z0-9]{2}'/>
    </xsd:restriction>
  </xsd:simpleType>

</xsd:schema>
";

        protected override string GetSchema()
        {
            return schema;
        }

        public override void NewDocument()
        {
            doc = new XDocument(new XElement("learnitemdata"));
        }
    }
}
