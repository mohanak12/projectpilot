using Headless.Threading;

namespace Headless
{
    public class ProjectRelatedJob : Job
    {
        public ProjectRelatedJob(string projectId)
            : base(projectId)
        {
        }
    }
}