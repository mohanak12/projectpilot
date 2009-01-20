using Headless.Configuration;

namespace Headless
{
    public class StageStatus    
    {
        public StageStatus(BuildStage stage)
        {
            this.stage = stage;
        }

        public StageOutcome Outcome
        {
            get { return outcome; }
        }

        public BuildStage Stage
        {
            get { return stage; }
        }

        public IStageRunner StageRunner
        {
            get { return stageRunner; }
        }

        public void MarkAsNotExecuted()
        {
            outcome = StageOutcome.NotExecuted;
        }

        public void PrepareToStart(IStageRunner stageRunner)
        {
            this.stageRunner = stageRunner;
        }

        private StageOutcome outcome = StageOutcome.Initial;
        private BuildStage stage;
        private IStageRunner stageRunner;
    }
}