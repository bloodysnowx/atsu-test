using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace OpenNashCalculator
{
    class InputXMLCreator
    {
        public XmlDocument create(TableData tableData)
        {
            var document = new XmlDocument();
            var input = document.CreateElement("input");
            document.AppendChild(input);
            setConfig(document, input);
            setHand(document, input, tableData);
            return document;
        }

        private void setHand(XmlDocument document, XmlElement input, TableData tableData)
        {
            var hand = document.CreateElement("hand");
            hand.SetAttribute("id", "1");
            input.AppendChild(hand);

            addElement(document, hand, "structure", tableData.Structure);
            addElement(document, hand, "blinds", 
                tableData.BB.ToString() + "," + tableData.SB.ToString() + "," + tableData.Ante.ToString());
            addElement(document, hand, "stacks", tableData.stacks);
        }

        private void setConfig(XmlDocument document, XmlElement input)
        {
            var config = document.CreateElement("config");
            input.AppendChild(config);

            addElement(document, config, "threads", "4");
            addElement(document, config, "iterations.linear", "300");
            addElement(document, config, "iterations.unrestricted", "300");
            addElement(document, config, "output.handev", "false");
            addElement(document, config, "output.played", "false");
            addElement(document, config, "output.optimizedlinear", "true");
            addElement(document, config, "compression", "false");
        }

        private void addElement(XmlDocument document, XmlElement config, String elementName, String value)
        {
            var element = document.CreateElement(elementName);
            element.InnerText = value;
            config.AppendChild(element);
        }
    }
}
