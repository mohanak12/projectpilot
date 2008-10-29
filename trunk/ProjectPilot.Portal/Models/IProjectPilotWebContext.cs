namespace ProjectPilot.Portal.Models
{
    public interface IProjectPilotWebContext
    {
        string CurrentModuleName { get; set; }
        string CurrentProjectName { get; set; }        
    }
}