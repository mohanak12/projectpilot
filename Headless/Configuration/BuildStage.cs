using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Headless.Configuration
{
    public class BuildStage
    {
        public BuildStage(string stageId)
        {
            this.stageId = stageId;
        }

        public string BuildComputer
        {
            get { return buildComputer; }
            set { buildComputer = value; }
        }

        public IList<BuildStage> DependsOn
        {
            get { return dependsOn; }
        }

        public string StageId
        {
            get { return stageId; }
        }

        public IBuildTask Task
        {
            get { return task; }
            set { task = value; }
        }

        private string buildComputer;
        private List<BuildStage> dependsOn = new List<BuildStage>();
        private string stageId;
        private IBuildTask task;
    }
}
