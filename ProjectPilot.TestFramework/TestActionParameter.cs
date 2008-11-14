namespace ProjectPilot.TestFramework
{
    public class TestActionParameter
    {
        public TestActionParameter(string parameterKey, string parameterValue)
        {
            this.parameterKey = parameterKey;
            this.parameterValue = parameterValue;
        }

        public string ParameterValue
        {
            get { return parameterValue; }
        }

        public string ParameterKey
        {
            get { return parameterKey; }
        }

        private readonly string parameterKey;
        private readonly string parameterValue;
    }
}