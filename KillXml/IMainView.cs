using System;
using System.Collections.Generic;
using System.Xml;

namespace KillXml
{
    public interface IMainView
    {
        string XmlContent { get; set; }

        string XPathExpression { get; set; }

        void ListNamespaces(IDictionary<string, string> namespaces);
        
        void ShowXPathError(Exception ex);
        
        void ShowXPathResults(IList<XPathResultItem> results);
    }
}