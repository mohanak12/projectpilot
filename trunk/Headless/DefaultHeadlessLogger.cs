using System;
using log4net;
using log4net.Core;

namespace Headless
{
    public class DefaultHeadlessLogger : IHeadlessLogger
    {
        public void Log(LogEvent logEvent)
        {
            LoggingEventData loggingEventData = new LoggingEventData();
            loggingEventData.Level = logEvent.Level;
            loggingEventData.LoggerName = logEvent.SourceId;
            loggingEventData.Message = logEvent.Message;
            loggingEventData.TimeStamp = DateTime.Now;
            LoggingEvent loggingEvent = new LoggingEvent(loggingEventData);
            log.Logger.Log(loggingEvent);
        }

        private static readonly ILog log = LogManager.GetLogger(typeof(DefaultHeadlessLogger));                
    }
}