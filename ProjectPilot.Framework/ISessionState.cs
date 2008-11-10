using System;
using System.Diagnostics.CodeAnalysis;

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
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        TValue GetValue<TValue>(string valueKey);
        void SetValue(string valueKey, object value);
    }
}