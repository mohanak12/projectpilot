using System;
using System.DirectoryServices;
using System.IO;

namespace Flubu.Tasks.Iis
{
    // IIS metabase properties
    // http://msdn.microsoft.com/library/default.asp?url=/library/en-us/iissdk/html/bb9c0d25-d003-4ddd-8adb-8662de0a24ee.asp
    // http://west-wind.com/weblog/posts/399.aspx

    public class CreateVirtualDirectoryTask : TaskBase
    {
        public bool AllowAnonymous
        {
            get { return allowAnonymous; }
            set { allowAnonymous = value; }
        }

        public bool AllowAuthNtlm
        {
            get { return allowAuthNtlm; }
            set { allowAuthNtlm = value; }
        }

        public string AnonymousUserName
        {
            get { return anonymousUserName; }
            set { anonymousUserName = value; }
        }

        public string AnonymousUserPass
        {
            get { return anonymousUserPass; }
            set { anonymousUserPass = value; }
        }

        public string AppFriendlyName
        {
            get { return appFriendlyName; }
            set { appFriendlyName = value; }
        }

        public bool AspEnableParentPaths
        {
            get { return aspEnableParentPaths; }
            set { aspEnableParentPaths = value; }
        }

        public bool AccessScript
        {
            get { return accessScript; }
            set { accessScript = value; }
        }

        public bool AccessExecute
        {
            get { return accessExecute; }
            set { accessExecute = value; }
        }

        public string DefaultDoc
        {
            get { return defaultDoc; }
            set { defaultDoc = value; }
        }

        public bool EnableDefaultDoc
        {
            get { return enableDefaultDoc; }
            set { enableDefaultDoc = value; }
        }

        public string ParentVirtualDirectoryName
        {
            get { return parentVirtualDirectoryName; }
            set { parentVirtualDirectoryName = value; }
        }

        public string ApplicationPoolName
        {
            get { return applicationPoolName; }
            set { applicationPoolName = value; }
        }

        public override string TaskDescription
        {
            get
            {
                return String.Format(
                    System.Globalization.CultureInfo.InvariantCulture,
                    "Create IIS virtual directory '{0}' on local path '{1}'",
                    virtualDirectoryName, 
                    localPath);
            }
        }

        public CreateVirtualDirectoryTask (
            string virtualDirectoryName,
            string localPath,
            CreateVirtualDirectoryMode mode)
        {
            this.virtualDirectoryName = virtualDirectoryName;
            this.localPath = localPath;
            this.mode = mode;
        }

        protected override void DoExecute (IScriptExecutionEnvironment environment)
        {
            string parentName = parentVirtualDirectoryName;

            using (DirectoryEntry parent = new DirectoryEntry (parentName))
            {
                DirectoryEntry virtualDirEntry = null;

                try
                {
                    // first check if the user already exists
                    try
                    {
                        virtualDirEntry = parent.Children.Find (virtualDirectoryName, "IIsWebVirtualDir");

                        // user already exists
                        if (mode == CreateVirtualDirectoryMode.DoNothingIfExists)
                        {
                            environment.Logger.Log(
                                String.Format (
                                    System.Globalization.CultureInfo.InvariantCulture,
                                    "Virtual directory '{0}' already exists, doing nothing.", 
                                    virtualDirectoryName));
                            return;
                        }
                        else if (mode == CreateVirtualDirectoryMode.FailIfAlreadyExists)
                        {
                            throw new RunnerFailedException (
                                String.Format (
                                    System.Globalization.CultureInfo.InvariantCulture,
                                    "Virtual directory '{0}' already exists.", 
                                    virtualDirectoryName));
                        }
                        // otherwise we should update the existing application pool
                    }
                    catch (DirectoryNotFoundException)
                    {
                        // application pool does not exist, go on and add it
                        virtualDirEntry = parent.Children.Add (virtualDirectoryName, "IIsWebVirtualDir");
                    }

                    virtualDirEntry.Properties["Path"][0] = localPath;
                    virtualDirEntry.CommitChanges ();

                    //foreach (PropertyValueCollection c in virtualDirEntry.Properties)
                    //{
                    //    environment.ReportMessage (String.Format ("'{0}', {1}", c.PropertyName, c.Count));
                    //}

                    virtualDirEntry.Invoke ("AppCreate3", new object[] { 2, applicationPoolName, false });

                    if (appFriendlyName != null)
                        virtualDirEntry.Properties["AppFriendlyName"][0] = appFriendlyName;
                    else
                        virtualDirEntry.Properties["AppFriendlyName"][0] = virtualDirectoryName;

                    //virtualDirEntry.Properties["AppIsolated"][0] = 2;

                    int authFlags = 0;
                    if (this.allowAnonymous)
                        authFlags |= 1;
                    if (this.allowAuthNtlm)
                        authFlags |= 4;
                    //if (this.AuthBasic)
                    //    Flags = Flags + 2;

                    virtualDirEntry.Properties["AuthFlags"][0] = authFlags;
                    if (anonymousUserName != null)
                        virtualDirEntry.Properties["AnonymousUserName"][0] = anonymousUserName;
                    if (anonymousUserPass != null)
                        virtualDirEntry.Properties["AnonymousUserPass"][0] = anonymousUserPass;
                    if (defaultDoc != null)
                        virtualDirEntry.Properties["DefaultDoc"][0] = defaultDoc;
                    virtualDirEntry.Properties["EnableDefaultDoc"][0] = enableDefaultDoc;

                    virtualDirEntry.Properties["AccessScript"][0] = true;
                    virtualDirEntry.Properties["AccessExecute"][0] = true;

                    virtualDirEntry.Properties["AspEnableParentPaths"][0] = aspEnableParentPaths;

                    virtualDirEntry.CommitChanges ();
                }
                finally
                {
                    if (virtualDirEntry != null)
                        virtualDirEntry.Dispose ();
                }

                parent.CommitChanges ();
            }
        }

        private CreateVirtualDirectoryMode mode = CreateVirtualDirectoryMode.FailIfAlreadyExists;

        private string virtualDirectoryName;
        private string parentVirtualDirectoryName = @"IIS://localhost/W3SVC/1/Root";
        private string localPath;
        private bool allowAnonymous = true;
        private bool allowAuthNtlm = true;
        private bool accessScript = true;
        private bool accessExecute;
        private string anonymousUserName;
        private string anonymousUserPass;
        private string appFriendlyName;
        private bool aspEnableParentPaths;
        private string defaultDoc;
        private bool enableDefaultDoc = true;
        private string applicationPoolName = "DefaultAppPool";

        //private PropertyCollection virtualDirectoryProperties;
    }
}
