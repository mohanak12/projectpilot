using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Xml;

namespace KillXml
{
    public class MainFormPresenter
    {
        public MainFormPresenter(IMainView view)
        {
            this.view = view;
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public void ExecuteXPathQuery()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(view.XmlContent);

            XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);

            IDictionary<string, string> namespacesUsed = ListNamespacesUsed(xmlDocument);
            if (namespacesUsed.ContainsKey(String.Empty))
                xmlNamespaceManager.AddNamespace("def", namespacesUsed[string.Empty]);

            XmlNodeList nodes = null;
            try
            {
                nodes = xmlDocument.SelectNodes(view.XPathExpression, xmlNamespaceManager);
            }
            catch (Exception ex)
            {
                view.ShowXPathError(ex);
                return;
            }

            List<XPathResultItem> results = new List<XPathResultItem>();
            foreach (XmlNode node in nodes)
            {
                XmlNode cursorNode = node;
                string nodePath = String.Empty;

                while (cursorNode != null)
                {
                    nodePath = cursorNode.Name + "/" + nodePath;
                    cursorNode = cursorNode.ParentNode;
                }

                XPathResultItem item = new XPathResultItem(node.Name, nodePath, node.Value);
                results.Add(item);
            }

            view.ShowXPathResults(results);
        }

        public void OnNamespacesTabShown()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(view.XmlContent);

            IDictionary<string, string> namespacesUsed = ListNamespacesUsed(xmlDocument);
            view.ListNamespaces (namespacesUsed);
        }

        private void AnalyseNamespacesUsed(XmlElement element, IDictionary<string, string> namespacesUsed)
        {
            if (false == namespacesUsed.ContainsKey(element.Prefix))
                namespacesUsed.Add(element.Prefix, element.NamespaceURI);

            foreach (XmlNode node in element.ChildNodes)
                if (node is XmlElement)
                    AnalyseNamespacesUsed(node as XmlElement, namespacesUsed);
        }

        private Dictionary<string, string> ListNamespacesUsed(XmlDocument xmlDocument)
        {
            Dictionary<string, string> namespacesUsed = new Dictionary<string, string>();
            XmlElement element = xmlDocument.DocumentElement;

            AnalyseNamespacesUsed(element, namespacesUsed);
            return namespacesUsed;
        }

        private IMainView view;
    }
}