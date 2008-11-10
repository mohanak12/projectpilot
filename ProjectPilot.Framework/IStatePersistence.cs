using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace ProjectPilot.Framework
{
    /// <summary>
    /// Defines methods for persisting the object's state.
    /// </summary>
    public interface IStatePersistence
    {
        /// <summary>
        /// Loads the state from persistent storage.
        /// </summary>
        /// <returns>State object.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        T LoadState<T>() where T : class;

        /// <summary>
        /// Saves the state to the persistent storage.
        /// </summary>
        /// <param name="state">The state object.</param>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        void SaveState<T>(T state) where T : class;
    }
}
