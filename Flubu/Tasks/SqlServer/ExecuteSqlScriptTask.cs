using System;
using System.Data.SqlClient;
using System.IO;
using Microsoft.SqlServer.Management.Common;

namespace Flubu.Tasks.SqlServer
{
    public class ExecuteSqlScriptTask : TaskBase
    {
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

        public static void ExecuteSqlScriptFile(
            IScriptExecutionEnvironment environment,
            string connectionString, 
            string scriptFilePath)
        {
            ExecuteSqlScriptTask task = new ExecuteSqlScriptTask(connectionString, scriptFilePath, null);
            task.Execute(environment);
        }

        public static void ExecuteSqlCommand(
            IScriptExecutionEnvironment environment,
            string connectionString, 
            string commandText)
        {
            ExecuteSqlScriptTask task = new ExecuteSqlScriptTask(connectionString, null, commandText);
            task.Execute(environment);
        }

        protected ExecuteSqlScriptTask(string connectionString, string scriptFilePath, string commandText)
        {
            this.connectionString = connectionString;
            this.scriptFilePath = scriptFilePath;
            this.commandText = commandText;
        }

        protected override void DoExecute(IScriptExecutionEnvironment environment)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                ServerConnection serverConnection = new ServerConnection(sqlConnection);
                serverConnection.Connect();
                Microsoft.SqlServer.Management.Smo.Server server = new Microsoft.SqlServer.Management.Smo.Server(serverConnection);

                string sqlScriptText = commandText;

                if (sqlScriptText == null)
                    sqlScriptText = File.ReadAllText(scriptFilePath);

                server.ConnectionContext.ExecuteNonQuery(sqlScriptText);
            }
        }

        private string connectionString;
        private string scriptFilePath;
        private string commandText;
    }
}