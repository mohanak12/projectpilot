namespace ProjectPilot.Extras.LogParser
{
    public abstract class ParsedElementBase
    {
//        public object Element
//        {
//            get { return element; }
//            set { element = value; }
//        }

        public abstract object Parse(string line);

//        private object element;
    }
}
