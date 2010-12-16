using Flubu;
using Flubu.Tasks.Iis;
using MbUnit.Framework;

namespace ProjectPilot.Tests.FlubuTests
{
    public class IisTests
    {
        [Test, Explicit]
        public void Test()
        {
            ConsoleExecutionEnvironment environment = new ConsoleExecutionEnvironment("test", "test", 0);
            IIisMaster iisMaster = new IisMaster(environment);

            IIisTasksFactory iisTasksFactory = iisMaster.LocalIisTasksFactory;

            Assert.IsInstanceOfType(typeof(Iis7TasksFactory), iisTasksFactory);

            const string AppPoolName = "something";

            IDeleteAppPoolTask deleteAppPoolTask = iisTasksFactory.DeleteAppPoolTask;
            deleteAppPoolTask.ApplicationPoolName = AppPoolName;
            deleteAppPoolTask.FailIfNotExist = false;
            deleteAppPoolTask.Execute(environment);

            IControlAppPoolTask controlAppPoolTask = null;
            try
            {
                controlAppPoolTask = iisTasksFactory.ControlAppPoolTask;
                controlAppPoolTask.ApplicationPoolName = AppPoolName;
                controlAppPoolTask.Action = ControlApplicationPoolAction.Start;
                controlAppPoolTask.FailIfNotExist = true;
                controlAppPoolTask.Execute(environment);
                Assert.Fail();
            }
            catch (RunnerFailedException ex)
            {
                Assert.AreEqual("Application pool 'something' does not exist.", ex.Message);
            }

            ICreateAppPoolTask createAppPoolTask = iisTasksFactory.CreateAppPoolTask;
            createAppPoolTask.ApplicationPoolName = AppPoolName;
            createAppPoolTask.Mode = CreateApplicationPoolMode.UpdateIfExists;
            createAppPoolTask.Execute(environment);

            createAppPoolTask.Mode = CreateApplicationPoolMode.DoNothingIfExists;
            createAppPoolTask.Execute(environment);

            controlAppPoolTask.Execute(environment);
            controlAppPoolTask.Action = ControlApplicationPoolAction.Recycle;
            controlAppPoolTask.Execute(environment);
            controlAppPoolTask.Action = ControlApplicationPoolAction.Stop;
            controlAppPoolTask.Execute(environment);

            createAppPoolTask.Mode = CreateApplicationPoolMode.FailIfAlreadyExists;
            try
            {
                createAppPoolTask.Execute(environment);
            }
            catch (RunnerFailedException ex)
            {
                Assert.AreEqual("Application 'something' already exists.", ex.Message);
            }

            deleteAppPoolTask.FailIfNotExist = true;
            deleteAppPoolTask.Execute(environment);

            deleteAppPoolTask.FailIfNotExist = true;
            try
            {
                deleteAppPoolTask.Execute(environment);
            }
            catch (RunnerFailedException ex)
            {
                Assert.AreEqual("Application 'something' does not exist.", ex.Message);
            }
        }
    }
}