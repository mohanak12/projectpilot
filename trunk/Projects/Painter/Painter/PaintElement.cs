using System.Collections.Generic;

namespace Painter
{
    public class PaintElement
    {
        public PaintElement(string elementName)
        {
            this.elementName = elementName;
        }

        public void AddParameter (int parameter)
        {
            parameters.Add(parameter);
        }

        private string elementName;
        private List<int> parameters = new List<int>();
    }
}