namespace ProjectPilot.Extras.LogParser
{
    public class LiteralPatternLayoutElement : ConversionPatternBase
    {
        public LiteralPatternLayoutElement(string literalText)
        {
            this.literalText = literalText;
        }

        public string LiteralText
        {
            get { return literalText; }
        }

        public override int Parse(string line, int startingIndex)
        {
            throw new System.NotImplementedException();
        }

        private string literalText;
    }
}