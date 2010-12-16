using System;

namespace Flubu.Tasks.Iis
{
    public class Iis7TasksFactory : IIisTasksFactory
    {
        public IControlAppPoolTask ControlAppPoolTask
        {
            get { return new Iis7ControlAppPoolTask(); }
        }

        public ICreateAppPoolTask CreateAppPoolTask
        {
            get { return new Iis7CreateAppPoolTask(); }
        }

        public IDeleteAppPoolTask DeleteAppPoolTask
        {
            get { return new Iis7DeleteAppPoolTask(); }
        }
    }
}