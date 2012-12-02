using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace LCW.Framework.Common.Xml
{
    public class XmlPackage
    {
        public XmlDocument CreateXml(string filepath,string version,string encoding,Action<XmlDocument> action)
        {
            XmlDocument document = new XmlDocument();
            XmlDeclaration declaration = document.CreateXmlDeclaration(version,encoding, null);
            document.AppendChild(declaration);
            if (action != null)
            {
                action(document);
            }
            try
            {
                document.Save(filepath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return document;
        }

        public XmlDocument CreateXml(string filepath)
        {
            return CreateXml(filepath, "1.0", "UTF-8", null);
        }

        public XmlDocument CreateXml(string filepath, string encoding)
        {
            return CreateXml(filepath, "1.0",encoding,null);
        }

        public XmlNode CreateXmlNode(XmlDocument document,string name,string text)
        {
            XmlNode node = document.CreateNode(XmlNodeType.Element, name, "");
            node.InnerText = text;
            document.AppendChild(node);
            return node;
        }

        public XmlElement CreateXmlElement(XmlNode Node,string name,string data)
        {
            XmlElement element = Node.OwnerDocument.CreateElement(name);
            element.AppendChild(Node.OwnerDocument.CreateCDataSection(@data));
            Node.AppendChild(element);
            return element;
        }
    }
}
