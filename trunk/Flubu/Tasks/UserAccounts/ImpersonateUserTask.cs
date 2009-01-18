using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace Flubu.Tasks.UserAccounts
{
    public class ImpersonateUserTask : ITask, IDisposable
    {
        public ImpersonateUserTask(string userName, string domain, string password)
        {
            this.domain = domain;
            this.password = password;
            this.userName = userName;
        }

        public WindowsImpersonationContext ImpersonationContext
        {
            get { return impersonationContext; }
        }

        public string TaskDescription
        {
            get { return String.Format(CultureInfo.InvariantCulture, @"Impersonate user '{0}\{1}'", domain, userName); }
        }

        public void Execute(IScriptExecutionEnvironment environment)
        {
            WindowsIdentity tempWindowsIdentity;
            IntPtr token = IntPtr.Zero;
            IntPtr tokenDuplicate = IntPtr.Zero;
            // request default security provider a logon token with LOGON32_LOGON_NEW_CREDENTIALS,
            // token returned is impersonation token, no need to duplicate
            if (LogonUser(userName, domain, password, 9, 0, ref token) != 0)
            {
                tempWindowsIdentity = new WindowsIdentity(token);
                impersonationContext = tempWindowsIdentity.Impersonate();
                // close impersonation token, no longer needed
                CloseHandle(token);
                if (impersonationContext != null)
                    return;
            }

            throw new InvalidOperationException(
                String.Format(
                    CultureInfo.InvariantCulture,
                    @"Failed to impersonate user '{0}\{1}'",
                    domain,
                    userName));
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or
        /// resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing">If <code>false</code>, cleans up native resources. 
        /// If <code>true</code> cleans up both managed and native resources</param>
        protected virtual void Dispose(bool disposing)
        {
            if (false == disposed)
            {
                if (disposing)
                {
                    // clean managed resources            
                    if (impersonationContext != null)
                        impersonationContext.Undo();
                }

                disposed = true;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [SuppressMessage("Microsoft.Globalization", "CA2101:SpecifyMarshalingForPInvokeStringArguments", MessageId = "0")]
        [SuppressMessage("Microsoft.Globalization", "CA2101:SpecifyMarshalingForPInvokeStringArguments", MessageId = "1")]
        [SuppressMessage("Microsoft.Globalization", "CA2101:SpecifyMarshalingForPInvokeStringArguments", MessageId = "2")]
        [DllImport("advapi32.dll")]
        private static extern int LogonUser(
            string lpszUsername, 
            string lpszDomain,
            string lpszPassword,
            int dwLogonType, 
            int dwLogonProvider, 
            ref IntPtr phToken);

        [SuppressMessage("Microsoft.Interoperability", "CA1414:MarkBooleanPInvokeArgumentsWithMarshalAs")]
        [SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(IntPtr hToken);

        private bool disposed;

        private readonly string domain;
        private WindowsImpersonationContext impersonationContext;
        private readonly string password;
        private readonly string userName;
    }
}