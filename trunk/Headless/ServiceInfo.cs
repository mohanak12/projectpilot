namespace Headless
{
    public class ServiceInfo
    {
        public string ComputerName
        {
            get { return computerName; }
            set { computerName = value; }
        }

        public int PortNumber
        {
            get { return portNumber; }
            set { portNumber = value; }
        }

        private string computerName;
        private int portNumber;
    }
}