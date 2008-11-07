using System;

namespace ProjectPilot.Framework.Modules
{
    public class NullTrigger : ITrigger
    {
        public bool IsTriggered()
        {
            return false;
        }

        public void MarkEventAsHandled()
        {
            throw new NotSupportedException();
        }
    }
}