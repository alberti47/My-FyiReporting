﻿using System.Xml.Serialization;


namespace fyiReporting.RdlCreator
{
    public class Query
    {
        [XmlElement(ElementName = "DataSourceName")]
        public string DataSourceName { get; set; }

        [XmlElement(ElementName = "CommandText")]
        public string CommandText { get; set; }
    }
}