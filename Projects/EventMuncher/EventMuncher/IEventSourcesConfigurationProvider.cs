namespace EventMuncher
{
    public interface IEventSourcesConfigurationProvider
    {
        EventSourcesConfiguration ProvideConfiguration();
    }
}