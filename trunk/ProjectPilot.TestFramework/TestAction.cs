#region

using System.Collections.Generic;
using System.Collections.ObjectModel;

#endregion

namespace ProjectPilot.TestFramework
{
	public class TestAction
	{
		public TestAction(string actionName)
		{
			this.actionName = actionName;
		}



		public TestAction
			(string actionName,
			 ActionParameters actionParameter)
		{
			this.actionName = actionName;
			AddActionParameter(actionParameter);
		}



		/// <summary>
		/// Name of the action.
		/// </summary>
		/// <example><c>selectProject</c> in <selectProject>Mobi-Info</selectProject></example>
		public string ActionName
		{
			get { return actionName; }
		}



		/// <summary>
		/// Add action parameter.
		/// </summary>
		/// <param name="actionParameter"><see cref="TestFramework.ActionParameters"/></param>
		/// <example>url=http://asd.html</example>
		/// <example>name=parameter1</example>
		/// 
		public void AddActionParameter(ActionParameters actionParameter)
		{
			actionParameters.Add(actionParameter);
		}



		public int ActionParametersCount
		{
			get { return actionParameters.Count; }
		}

		public IList<ActionParameters> ActionParameters
		{
			get { return actionParameters; }
		}



		public string GetParameterKeyValue(string parameterKey)
		{
			foreach (ActionParameters actionParameter in actionParameters)
			{
				if (actionParameter.ParameterKey.Equals(parameterKey))
				{
					return actionParameter.ParameterValue;
				}
			}
			return null;
		}



		public bool HasParameters
		{
			get { return actionParameters.Count > 0 ? true : false; }
		}

		private readonly string actionName;

		private readonly List<ActionParameters> actionParameters = new List<ActionParameters>();
	}
}