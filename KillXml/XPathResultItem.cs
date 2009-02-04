namespace KillXml
{
    public class XPathResultItem
    {
        public XPathResultItem(string nodeName, string nodePath, string nodeValue)
        {
            this.nodeName = nodeName;
            this.nodePath = nodePath;
            this.nodeInnerXml = nodeValue;
        }

        public string NodeName
        {
            get { return nodeName; }
        }

        public string NodePath
        {
            get { return nodePath; }
        }

        public string NodeInnerXml
        {
            get { return nodeInnerXml; }
        }

        private string nodeName;
        private string nodePath;
        private string nodeInnerXml;
    }
}