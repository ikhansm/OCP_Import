using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ImportService.Wrapper
{
    [XmlRoot(ElementName = "ColorMapping")]
    public class ColorMapping
    {
        [XmlElement(ElementName = "COLOR_NAME")]
        public string COLOR_NAME { get; set; }
        [XmlElement(ElementName = "WEB_FRIENDLY_COLOR_NAME")]
        public string WEB_FRIENDLY_COLOR_NAME { get; set; }


    }

    [XmlRoot(ElementName = "ColorList")]
    public class ColorList
    {
        [XmlElement(ElementName = "ColorMapping")]
        public List<ColorMapping> ColorMapping { get; set; }
        [XmlAttribute(AttributeName = "od", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Od { get; set; }
    }
 

}
