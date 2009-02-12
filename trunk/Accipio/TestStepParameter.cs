namespace Accipio
{
    public class TestStepParameter
    {
        public TestStepParameter(string parameterName, object parameterValue)
        {
            this.parameterName = parameterName;
            this.parameterValue = parameterValue;
        }

        public string ParameterName
        {
            get { return parameterName; }
        }

        public object ParameterValue
        {
            get { return parameterValue; }
        }

        private readonly string parameterName;
        private readonly object parameterValue;
    }
}