using System;

namespace ProjectPilot.Framework.Modules
{
    public class PeriodicTrigger : ITrigger
    {
        public PeriodicTrigger(TimeSpan period, ITimeProvider timeProvider)
        {
            this.period = period;
            this.timeProvider = timeProvider;
        }

        public bool IsTriggered()
        {
            return (timeProvider.GetCurrentTime() - lastTriggerTime) >= period;
        }

        public void MarkEventAsHandled()
        {
            lastTriggerTime = timeProvider.GetCurrentTime();
        }

        private DateTime lastTriggerTime = DateTime.MinValue;
        private readonly TimeSpan period;
        private readonly ITimeProvider timeProvider;
    }
}