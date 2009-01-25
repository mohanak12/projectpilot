namespace Headless
{
    public class BuildStageReport
    {
        public BuildOutcome StageOutcome
        {
            get { return stageOutcome; }
            set { stageOutcome = value; }
        }

        private BuildOutcome stageOutcome;
    }
}