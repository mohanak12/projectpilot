namespace ProjectPilot.Extras.LogParser
{
    public abstract class ConversionPatternBase : ILog4NetPatternLayoutElement
    {
        public int? MaxLength
        {
            get { return maxLength; }
            set { maxLength = value; }
        }

        public int? MinLength
        {
            get { return minLength; }
            set { minLength = value; }
        }

        public string PatternName
        {
            get { return patternName; }
            set { patternName = value; }
        }

        public abstract int Parse(string line, int startingIndex);

        private string patternName;
        private int? minLength;
        private int? maxLength;
    }
}