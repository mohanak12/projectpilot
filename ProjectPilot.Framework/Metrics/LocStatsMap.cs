using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            // TODO: handle situation when the extension is not in the map
            return map[fileExtension.ToUpperInvariant()];
        }

        private Dictionary<string, ILocStats> map = new Dictionary<string, ILocStats>();
    }
}
