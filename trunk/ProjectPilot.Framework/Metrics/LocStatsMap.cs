using System.Collections.Generic;

namespace ProjectPilot.Framework.Metrics
{
    public class LocStatsMap
    {
        public void AddToMap(string fileExtension, ILocStats locStats)
        {
            map.Add(fileExtension.ToUpperInvariant(), locStats);
        }

        public ILocStats GetLocStatsForExtension(string fileExtension)
        {
            if (map.ContainsKey(fileExtension.ToUpperInvariant()))
                return map[fileExtension.ToUpperInvariant()];
            else
            {
                return null;
            }
        }

        private Dictionary<string, ILocStats> map = new Dictionary<string, ILocStats>();
    }
}
