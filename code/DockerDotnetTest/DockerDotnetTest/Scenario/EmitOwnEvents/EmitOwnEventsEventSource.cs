using System.Diagnostics.Eventing.Reader;
using System.Diagnostics.Tracing;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using System.Globalization;
using EventLevel = System.Diagnostics.Tracing.EventLevel;

namespace DockerDotnetTest.Scenario.EmitOwnEvents;

public class EmitOwnEventsEventSource : EventSource
{
    public static EmitOwnEventsEventSource RandomEventSource = new EmitOwnEventsEventSource("Workshop.Random");
    public static EmitOwnEventsEventSource RequestContentEventSource = new EmitOwnEventsEventSource("Workshop.Request");

    public EmitOwnEventsEventSource(string name) : base(name) {}

    public void Info(EventSourceKeywords keywords, string eventName, string message)
    {
        if (IsEnabled())
        {
            var options = new EventSourceOptions { Keywords = (EventKeywords)Convert.ToInt64(keywords, CultureInfo.InvariantCulture), Level = EventLevel.Informational };

            Write(eventName, options, new { Payload = message });
        }
    }

    public void Error(EventSourceKeywords keywords, string eventName, string message)
    {
        if (IsEnabled())
        {
            var options = new EventSourceOptions { Keywords = (EventKeywords)Convert.ToInt64(keywords, CultureInfo.InvariantCulture), Level = EventLevel.Informational };

            Write(eventName, options, new { Payload = message });
        }
    }
}