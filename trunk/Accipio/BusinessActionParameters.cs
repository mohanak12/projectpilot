
namespace Accipio
{
    public class BusinessActionParameters
    {
        public BusinessActionParameters(string parameterName, string parameterType)
        {
            this.parameterName = parameterName;
            this.parameterType = parameterType;
        }

        public string ParameterName
        {
            get { return parameterName; }
        }

        public string ParameterType
        {
            get { return parameterType; }
        }

        private readonly string parameterName;
        private readonly string parameterType;
    }
}
