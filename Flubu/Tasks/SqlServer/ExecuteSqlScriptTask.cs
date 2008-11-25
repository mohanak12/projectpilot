using System;
using System.Data.SqlClient;
using System.IO;
using Microsoft.SqlServer.Management.Common;

namespace Flubu.Tasks.SqlServer
{
    public class ExecuteSqlScriptTask : TaskBase
    {
        #region ITask Members

        public override string TaskDescription
        {
            get
            {
                return String.Format(
                    System.Globalization.CultureInfo.InvariantCulture,
                    "Execute SQL script '{0}'", 
                    scriptFilePath);
            }
        }

        public ExecuteSqlScriptTask (string connectionString, string scriptFilePath)
        {
            this.connectionString = connectionString;
            this.scriptFilePath = scriptFilePath;
        }

        protected override void DoExecute (IScriptExecutionEnvironment environment)
        {
            using (SqlConnection sqlConnection = new SqlConnection (connectionString))
            {
                ServerConnection serverConnection = new ServerConnection (sqlConnection);
                serverConnection.Connect ();
                Microsoft.SqlServer.Management.Smo.Server server = new Microsoft.SqlServer.Management.Smo.Server (serverConnection);

                using (Stream scriptFile = File.Open (scriptFilePath, FileMode.Open, FileAccess.Read))
                {
                    using (StreamReader reader = new StreamReader (scriptFile))
                    {
                        string sqlScript = reader.ReadToEnd ();

                        server.ConnectionContext.ExecuteNonQuery (sqlScript);
                    }
                }
            }
        }

        #endregion

        private string connectionString;
        private string scriptFilePath;
    }
}
