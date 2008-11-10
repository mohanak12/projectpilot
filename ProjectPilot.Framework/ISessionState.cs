using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace ProjectPilot.Framework
{
    /// <summary>
    /// Represents an object which can hold session information. 
    /// </summary>
    /// <remarks>
    /// The information is stored as key value pairs.
    /// </remarks>
    public interface ISessionState : IDisposable
    {
        /// <summary>
        /// Gets the unique ID of the session holder.
        /// </summary>
        /// <value>The session holder ID.</value>
        string SessionHolderId { get; }

        /// <summary>
        /// Gets the session value specified by its key name.
        /// </summary>
        /// <typeparam name="TValue">The type of the session value.</typeparam>
        /// <param name="valueKey">The session value key.</param>
        /// <returns>The session value or <c>null</c> if the session does not contain the value.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        TValue GetValue<TValue>(string valueKey) where TValue : class;

        /// <summary>
        /// Gets the session value specified by its key name.
        /// </summary>
        /// <typeparam name="TValue">The type of the session value.</typeparam>
        /// <param name="valueKey">The session value key.</param>
        /// <param name="defaultValue">The default value that the method should return if the value
        /// is not stored in the session.</param>
        /// <returns>
        /// The session value or <c>defaultValue</c> if the session does not contain the value.
        /// </returns>
        TValue GetValue<TValue>(string valueKey, TValue defaultValue);

        /// <summary>
        /// Determines whether the session contains the specified value.
        /// </summary>
        /// <param name="valueKey">The session value key.</param>
        /// <returns>
        /// 	<c>true</c> if the session contains the specified value; otherwise, <c>false</c>.
        /// </returns>
        bool HasValue(string valueKey);

        /// <summary>
        /// Sets the session value.
        /// </summary>
        /// <param name="valueKey">The session value key.</param>
        /// <param name="value">The session value.</param>
        void SetValue(string valueKey, object value);
    }
}