﻿using System;
using System.Globalization;

namespace Flubu.Tasks.Iis
{
    public class IisMaster : IIisMaster
    {
        public IisMaster(IScriptExecutionEnvironment environment)
        {
            this.environment = environment;
        }

        public IIisTasksFactory Iis6TasksFactory
        {
            get
            {
                return new Iis6TasksFactory();
            }
        }

        public IIisTasksFactory Iis7TasksFactory
        {
            get
            {
                return new Iis7TasksFactory();
            }
        }

        public IIisTasksFactory LocalIisTasksFactory
        {
            get
            {
                Version version = new Version(GetLocalIisVersionTask.GetIisVersion(environment, true));
                if (version.Major >= 7)
                    return Iis7TasksFactory;
                if (version.Major >= 6)
                    return Iis6TasksFactory;

                string message = string.Format(
                    CultureInfo.InvariantCulture,
                    "IIS version {0} is not supported.",
                    version);
                throw new NotSupportedException(message);
            }
        }

        private readonly IScriptExecutionEnvironment environment;
    }
}