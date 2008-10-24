using System.Collections.Generic;

namespace ProjectPilot.Framework.CCNet
{
    public class CCNetProjectStatisticsBuildEntry
    {
        private string buildLabel;
        private string buildStatus;
        private Dictionary<string,string> parameters = new Dictionary<string, string>();
    }
}