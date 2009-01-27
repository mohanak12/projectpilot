
using Accipio.Reporting;

namespace Accipio
{
    public interface IHtmlTestReportGenerator
    {
        void Generate(TestRunsDatabase testRunDatabase);
    }
}
