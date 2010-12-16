﻿namespace Flubu.Tasks.Iis
{
    public class Iis6TasksFactory : IIisTasksFactory
    {
        public IControlAppPoolTask ControlAppPoolTask
        {
            get { return new Iis6ControlAppPoolTask(); }
        }

        public ICreateAppPoolTask CreateAppPoolTask
        {
            get { return new Iis6CreateAppPoolTask(); }
        }

        public IDeleteAppPoolTask DeleteAppPoolTask
        {
            get { return new Iis6DeleteAppPoolTask(); }
        }
    }
}