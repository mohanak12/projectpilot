using System;

namespace Stump.Views
{
    public class LogTabData
    {
        public LogTabData(string tabText, string toolTipText)
        {
            this.tabText = tabText;
            this.toolTipText = toolTipText;
        }

        /// <summary>
        /// Compares the current <see cref="LogTabData"/> object to the specified object for equivalence.
        /// </summary>
        /// <param name="obj">The <see cref="LogTabData"/> object to test for equivalence with the current object.</param>
        /// <returns>
        /// <c>true</c> if the two <see cref="LogTabData"/> objects are equal; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            LogTabData that = obj as LogTabData;

            if (that == null)
                return false;

            return String.Equals(tabText, that.tabText) && String.Equals(toolTipText, that.toolTipText);
        }

        /// <summary>
        /// Returns the hash code for this <see cref="LogTabData"/> object.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override int GetHashCode()
        {
            if (toolTipText != null)
                return toolTipText.GetHashCode();

            return base.GetHashCode();
        }

        private readonly string tabText;
        private readonly string toolTipText;
    }
}