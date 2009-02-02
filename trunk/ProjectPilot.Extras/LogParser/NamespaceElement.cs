using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectPilot.Extras.LogParser
{
    public class NamespaceElement : ParsedElementBase
    {
        public override void Parse(string line)
        {
            Element = line.Trim();
        }
    }
}
