namespace Headless.Web
{
    public interface IWebRequestProcessor
    {
        void ProcessRequest(WebRequestData request, WebResponseData response);
    }
}