using System;
using System.Collections.Generic;

namespace EventMuncher
{
    public class EventSourcesConfiguration
    {
        public int EventSourcesCount
        {
            get { return eventSources.Count; }
        }

        public void AddEventSource (EventSource eventSource)
        {
            eventSources.Add(eventSource);
        }

        private List<EventSource> eventSources = new List<EventSource>();
    }
}