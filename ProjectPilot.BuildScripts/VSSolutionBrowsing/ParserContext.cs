namespace ProjectPilot.BuildScripts.VSSolutionBrowsing
{
    internal class ParserContext
    {
        public int LineCount
        {
            get { return lineCount; }
        }

        public void IncrementLineCount () { lineCount++; }

        private int lineCount;
    }
}