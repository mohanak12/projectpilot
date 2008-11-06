using System;

namespace ProjectPilot.Framework
{
    /// <summary>
    /// A standard implementation of the <see cref="ITimeProvider"/> interface which
    /// uses <see cref="DateTime.Now"/> property.
    /// </summary>
    public class StandardTimeProvider : ITimeProvider
    {
        /// <summary>
        /// Gets the current system time.
        /// </summary>
        /// <returns>The current system time.</returns>
        public DateTime GetCurrentTime()
        {
            return DateTime.Now;
        }
    }
}