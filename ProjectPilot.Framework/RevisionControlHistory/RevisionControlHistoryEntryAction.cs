namespace ProjectPilot.Framework.RevisionControlHistory
{
    public enum RevisionControlHistoryEntryAction
    {
        /// <summary>
        /// The item was added to the revision control system.
        /// </summary>
        Add,

        /// <summary>
        /// The item was deleted from the revision control system.
        /// </summary>
        Delete,

        /// <summary>
        /// The item was modified in the revision control system.
        /// </summary>
        Modify,

        /// <summary>
        /// The item was replaced in the revision control system.
        /// </summary>
        Replace
    }
}