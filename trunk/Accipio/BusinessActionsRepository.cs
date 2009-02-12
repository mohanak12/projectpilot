using System;
using System.Collections.Generic;
using System.Globalization;

namespace Accipio
{
    /// <summary>
    /// Contains list of business actions and business functions
    /// </summary>
    public class BusinessActionsRepository
    {
        public void AddAction (BusinessAction action)
        {
            if (actions.ContainsKey(action.ActionName))
                throw new ArgumentException(string.Format(
                                                CultureInfo.InvariantCulture,
                                                "Business action '{0}' has already been added to the repository",
                                                action.ActionName));

            actions.Add(action.ActionName, action);
        }

        public IList<BusinessAction> EnumerateActions()
        {
            List<BusinessAction> actionsSorted = new List<BusinessAction>(this.actions.Values);
            actionsSorted.Sort(
                delegate (BusinessAction action1, BusinessAction action2)
                    {
                        return action1.ActionName.CompareTo(action2.ActionName);
                    });

            return actionsSorted;
        }

        /// <summary>
        /// Gets the specified action.
        /// </summary>
        /// <param name="actionId">The action id.</param>
        /// <returns>Requested action or <c>null</c> See <see cref="BusinessAction"/></returns>
        public BusinessAction GetAction(string actionId)
        {
            if (false == actions.ContainsKey(actionId))
                throw new KeyNotFoundException(String.Format(
                                                   CultureInfo.InvariantCulture,
                                                   "Action '{0}' could not be found",
                                                   actionId));

            return actions[actionId];
        }

        private readonly Dictionary<string, BusinessAction> actions = new Dictionary<string, BusinessAction>();
    }
}
