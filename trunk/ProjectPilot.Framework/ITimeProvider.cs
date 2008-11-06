using System;
using System.Diagnostics.CodeAnalysis;

namespace ProjectPilot.Framework
{
    /// <summary>
    /// Represents an entity which provides the current system time.
    /// </summary>
    public interface ITimeProvider
    {
        /// <summary>
        /// Gets the current system time.
        /// </summary>
        /// <returns>The current system time.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        DateTime GetCurrentTime();
    }
}