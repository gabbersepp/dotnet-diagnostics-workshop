using System.Diagnostics;
using DockerDotnetTest.Scenario.EmitOwnEvents;
using System.Diagnostics.Tracing;
using System.Globalization;

namespace DockerDotnetTest.Scenario.EventCounter;

public class EventCounterEventSource : EventSource
{
    public static System.Diagnostics.Tracing.EventCounter RequestTime;
    public static System.Diagnostics.Tracing.IncrementingEventCounter NumberOfRequests;
    public static PollingCounter AllocatedMemory;
    public static IncrementingPollingCounter IncrementingAllocatedMemory;
    public static EventCounterEventSource EventCounters = new EventCounterEventSource("Workshop.Counters");

    public EventCounterEventSource(string name) : base(name)
    {
        RequestTime = new System.Diagnostics.Tracing.EventCounter("RequestTime", this);
        NumberOfRequests = new IncrementingEventCounter("NumberOfRequests", this);
        AllocatedMemory = new PollingCounter("AllocatedMemory", this,
            () => Process.GetCurrentProcess().WorkingSet64 / 1024);
        IncrementingAllocatedMemory = new IncrementingPollingCounter("IncrementingAllocatedMemory", this,
            () => Process.GetCurrentProcess().WorkingSet64 / 1024);
    }
}