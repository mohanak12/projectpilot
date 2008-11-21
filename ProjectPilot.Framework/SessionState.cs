using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ProjectPilot.Framework
{
    /// <summary>
    /// The default implementation of the <see cref="ISessionState"/> interface.
    /// </summary>
    [Serializable]
    [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    [SuppressMessage("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces")]
    public class SessionState : ISessionState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SessionState"/> class
        /// for a specified session holder and using the specified <see cref="ISessionStorage"/>
        /// object,
        /// </summary>
        /// <param name="sessionHolderId">The unique ID of the session holder.</param>
        /// <param name="sessionStorage">The session storage object.</param>
        public SessionState(string sessionHolderId, ISessionStorage sessionStorage)
        {
            this.sessionHolderId = sessionHolderId;
            this.sessionStorage = sessionStorage;
        }

        /// <summary>
        /// Gets or sets the unique ID of the session holder.
        /// </summary>
        /// <value>The session holder ID.</value>
        public string SessionHolderId
        {
            get { return sessionHolderId; }
            set { sessionHolderId = value; }
        }

        /// <summary>
        /// Gets or sets the session storage object.
        /// </summary>
        /// <value>The session storage object.</value>
        public ISessionStorage SessionStorage
        {
            get { return sessionStorage; }
            set { sessionStorage = value; }
        }

        /// <summary>
        /// Saves the session state object to the session storage and disposes of the object.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA1816:CallGCSuppressFinalizeCorrectly")]
        [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        public void Dispose()
        {
            if (isDirty)
            {
                sessionStorage.SaveSession(this);
            }
        }

        /// <summary>
        /// Gets the session value specified by its key name.
        /// </summary>
        /// <typeparam name="TValue">The type of the session value.</typeparam>
        /// <param name="valueKey">The session value key.</param>
        /// <returns>
        /// The session value or <c>null</c> if the session does not contain the value.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public TValue GetValue<TValue>(string valueKey) where TValue : class
        {
            if (false == HasValue(valueKey))
                return null;

            return (TValue)sessionValues[valueKey];
        }

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
        public TValue GetValue<TValue>(string valueKey, TValue defaultValue)
        {
            if (false == HasValue(valueKey))
                return defaultValue;

            return (TValue)sessionValues[valueKey];
        }

        /// <summary>
        /// Determines whether the session contains the specified value.
        /// </summary>
        /// <param name="valueKey">The session value key.</param>
        /// <returns>
        ///     <c>true</c> if the session contains the specified value; otherwise, <c>false</c>.
        /// </returns>
        public bool HasValue(string valueKey)
        {
            return sessionValues.ContainsKey(valueKey);
        }

        /// <summary>
        /// Sets the session value.
        /// </summary>
        /// <param name="valueKey">The session value key.</param>
        /// <param name="value">The session value.</param>
        public void SetValue(string valueKey, object value)
        {
            sessionValues[valueKey] = value;
            isDirty = true;
        }

        [NonSerialized]
        private bool isDirty;
        private string sessionHolderId;
        [NonSerialized]
        private ISessionStorage sessionStorage;
        private Dictionary<string,object> sessionValues = new Dictionary<string, object>();
    }
}