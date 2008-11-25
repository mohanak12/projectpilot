namespace Flubu.Tasks.Iis
{
    public enum CreateVirtualDirectoryMode
    {
        /// <summary>
        /// If the virtual directory already exists, the task should fail.
        /// </summary>
        FailIfAlreadyExists,

        /// <summary>
        /// If the virtual directory already exists, it should be updated.
        /// </summary>
        UpdateIfExists,

        /// <summary>
        /// If the virtual directory already exists, the task should do nothing.
        /// </summary>
        DoNothingIfExists,
    }
}