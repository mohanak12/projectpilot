namespace Flubu
{
    public interface IScriptExecutionEnvironment : IFlubuLogger
    {
        /// <summary>
        /// Gets or sets the name of the script.
        /// </summary>
        /// <value>The name of the script.</value>
        string ScriptName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the execution environment runs in an interactive mode.
        /// </summary>
        /// <remarks>When the execution environment is running in the interactive mode, the user has
        /// the ability to influence certain aspects of the environment. One example is entering values
        /// for missing configuration settings.</remarks>
        /// <value><c>true</c> if the execution environment runs in an interactive mode; otherwise, <c>false</c>.</value>
        bool InteractiveMode { get; set; }

        /// <summary>
        /// Gets a value indicating whether the script is running on Windows Server 2003.
        /// </summary>
        /// <value>
        ///     <c>true</c> if the script is running on Windows Server 2003; otherwise, <c>false</c>.
        /// </value>
        bool IsWinServer2003 { get; }

        //IFlubuLogger Logger { get; set; }

        /// <summary>
        /// Gets the .NET version number for .NET 1.0.
        /// </summary>
        /// <value>.NET version number for .NET 1.0.</value>
        string Net10VersionNumber { get; }

        /// <summary>
        /// Gets the .NET version number for .NET 1.1.
        /// </summary>
        /// <value>.NET version number for .NET 1.1.</value>
        string Net11VersionNumber { get; }

        /// <summary>
        /// Gets the .NET version number for .NET 2.0.
        /// </summary>
        /// <value>.NET version number for .NET 2.0.</value>
        string Net20VersionNumber { get; }

        /// <summary>
        /// Gets the .NET version number for .NET 3.0.
        /// </summary>
        /// <value>.NET version number for .NET 3.0.</value>
        string Net30VersionNumber { get; }

        /// <summary>
        /// Gets the .NET version number for .NET 3.5.
        /// </summary>
        /// <value>.NET version number for .NET 3.5.</value>
        string Net35VersionNumber { get; }

        /// <summary>
        /// Gets the .NET version number for .NET 4.0.
        /// </summary>
        /// <value>.NET version number for .NET 4.0.</value>
        string Net40VersionNumber { get; }

        /// <summary>
        /// Gets the Windows system root directory path.
        /// </summary>
        /// <value>The Windows system root directory path.</value>
        string SystemRootDir { get; }

        /// <summary>
        /// Gets the path to the .NET Framework directory.
        /// </summary>
        /// <param name="dotNetVersion">The version of the .NET (example: "v2.0.50727").</param>
        /// <returns>The path to the .NET Framework directory.</returns>
        string GetDotNetFWDir (string dotNetVersion);

        /// <summary>
        /// Gets or sets a value indicating whether to dry-run the script. 
        /// When set to <c>true</c>, the tasks are not really executed,
        /// instead just a log of activities is created.
        /// </summary>
        bool DryRun { get; set; }

        void AddLogger(IFlubuLogger logger);

        string GetConfigurationSettingValue (string settingName);

        string ReceiveInput (string prompt);

        void SetConfigurationSettingValue(string settingName, string settingValue);
    }
}
