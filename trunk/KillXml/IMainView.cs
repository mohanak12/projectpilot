using System;
using System.Collections.Generic;
using System.Xml;

namespace KillXml
{
    public interface IMainView
    {
        string XmlSource { get; set; }

        string XPathExpression { get; set; }

        string XsltSource { get; set; }

        void ListNamespaces(IDictionary<string, string> namespaces);
        
        void ShowXPathError(Exception ex);
        
        void ShowXPathResults(IList<XPathResultItem> results);

        void ShowDocumentInTransformBrowser(Uri transformedXmlUrl);
    }
}