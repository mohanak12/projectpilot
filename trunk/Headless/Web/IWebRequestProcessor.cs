using System;

namespace Headless.Web
{
    public interface IWebRequestProcessor
    {
        string ProcessRequest(Uri requestUrl);
    }
}