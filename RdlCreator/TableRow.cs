﻿using System.Collections.Generic;
using System.Xml.Serialization;


namespace fyiReporting.RdlCreator
{
    public class TableRow
    {
        [XmlElement(ElementName = "Height")]
        public string Height { get; set; }

        [XmlElement(ElementName = "TableCells")]
        public TableCells TableCells { get; set; }

    }
}