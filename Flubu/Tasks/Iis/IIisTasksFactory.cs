namespace Flubu.Tasks.Iis
{
    public interface IIisTasksFactory
    {
        IControlAppPoolTask ControlAppPoolTask { get; }
        ICreateAppPoolTask CreateAppPoolTask { get; }
        IDeleteAppPoolTask DeleteAppPoolTask { get; }
    }
}