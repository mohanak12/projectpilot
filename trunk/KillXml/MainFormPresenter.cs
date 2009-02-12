using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Xsl;

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
            try
            {
                xmlDocument.LoadXml(view.XmlSource);
            }
            catch (Exception ex)
            {
                view.ShowXPathError(ex);
                return;
            }

            XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);

            IDictionary<string, string> namespacesUsed = ListNamespacesUsed(xmlDocument);
            if (namespacesUsed.ContainsKey(String.Empty))
                xmlNamespaceManager.AddNamespace("j", namespacesUsed[string.Empty]);

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

                XPathResultItem item = new XPathResultItem(node.Name, nodePath, node.InnerXml);
                results.Add(item);
            }

            view.ShowXPathResults(results);
        }

        public void OnNamespacesTabShown()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(view.XmlSource);

            IDictionary<string, string> namespacesUsed = ListNamespacesUsed(xmlDocument);
            view.ListNamespaces (namespacesUsed);
        }

        public void OnTransformButtonClicked()
        {
            XsltSettings xsltSettings = new XsltSettings(true, true);
            XmlDocument xsltDoc = new XmlDocument();
            xsltDoc.LoadXml(view.XsltSource);

            XmlUrlResolver resolver = new XmlUrlResolver();
            XslCompiledTransform transform = new XslCompiledTransform(true);

            try
            {
                transform.Load(xsltDoc, xsltSettings, resolver);
            }
            catch (XsltException ex)
            {
                ShowXsltError("Error in XSLT document: {0}", ex.Message);
                return;
            }

            const string TransformedXmlFileName = "transformed.xml";

            using (StringReader reader = new StringReader(view.XmlSource))
            {
                XmlReader xmlReader = XmlReader.Create(reader);
                using (XmlWriter writer = XmlWriter.Create(TransformedXmlFileName))
                    transform.Transform(xmlReader, writer);
            }

            ShowFileInTransformBrowser(TransformedXmlFileName);
        }

        private void ShowXsltError(string messageFormat, params object[] args)
        {
            File.WriteAllText("error.txt", String.Format(CultureInfo.InvariantCulture, messageFormat, args));
            ShowFileInTransformBrowser("error.txt");
        }

        private void ShowFileInTransformBrowser(string fileName)
        {
            Uri uri = new Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            string localPath = uri.LocalPath;
            string dir = Path.GetDirectoryName(localPath);

            Uri pageUrl = new Uri(Path.Combine(dir, fileName));

            view.ShowDocumentInTransformBrowser(pageUrl);
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