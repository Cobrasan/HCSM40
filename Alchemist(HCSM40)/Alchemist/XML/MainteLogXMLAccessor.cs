﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Alchemist
{
    public class MainteLogXMLAccessor : XMLAccessor
    {
        private const string schema = @"
<xsd:schema xmlns:xsd='http://www.w3.org/2001/XMLSchema'>

  <!-- WorkID型 -->
  <xsd:simpleType name='workid_type'>
    <xsd:restriction base='xsd:string'>
      <xsd:pattern value='^0x[0-9A-F]{4}' />
    </xsd:restriction>
  </xsd:simpleType>

  
  <xsd:element name='maintelog' type='maintelog_type' />

  <!-- maintelog型 -->
  <xsd:complexType name='maintelog_type'>
    <xsd:sequence>
      <xsd:element name='record' minOccurs='0' maxOccurs='unbounded' type='record_type' />
    </xsd:sequence>
  </xsd:complexType>
  
  <!-- Record型 -->
  <xsd:complexType name='record_type'>
    <xsd:sequence>
      <xsd:element name='unit' minOccurs='1' maxOccurs='1' type='unit_type' />
      <xsd:element name='value_change' minOccurs='0' maxOccurs='1' type='value_change_type' />
      <xsd:element name='maintenance' minOccurs='0' maxOccurs='1' type='maintenance_type'  />
    </xsd:sequence>
    <xsd:attribute name='type' use='required' type='xsd:integer' />
    <xsd:attribute name='date' use='required' type='xsd:date' />
    <xsd:attribute name='time' use='required' type='xsd:time' />
  </xsd:complexType>

  <!-- unit型 -->
  <xsd:complexType name='unit_type'>
    <xsd:sequence />
    <xsd:attribute name ='code' use ='required' type ='xsd:integer' />
  </xsd:complexType>

  <!-- value_change型 -->
  <xsd:complexType name='value_change_type'>
    <xsd:sequence />
    <xsd:attribute name='workidtype' use='required' type='xsd:integer' />
    <xsd:attribute name ='workid' use ='required' type='workid_type' />
    <xsd:attribute name='old' use='required' type='xsd:double' />
    <xsd:attribute name='new' use='required' type='xsd:double' />
  </xsd:complexType>

  <!-- maintenance型 -->
  <xsd:complexType name='maintenance_type'>
    <xsd:sequence />
    <xsd:attribute name='comment' use='required' type='xsd:string' />
  </xsd:complexType>

</xsd:schema>
";

        protected override string GetSchema()
        {
            return schema;
        }

        public override void NewDocument()
        {
            doc = new XDocument(new XElement("maintelog"));
        }

    }
}
