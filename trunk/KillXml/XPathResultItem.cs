namespace KillXml
{
    public class XPathResultItem
    {
        public XPathResultItem(string nodeName, string nodePath, string nodeValue)
        {
            this.nodeName = nodeName;
            this.nodePath = nodePath;
            this.nodeValue = nodeValue;
        }

        public string NodeName
        {
            get { return nodeName; }
        }

        public string NodePath
        {
            get { return nodePath; }
        }

        public string NodeValue
        {
            get { return nodeValue; }
        }

        private string nodeName;
        private string nodePath;
        private string nodeValue;
    }
}