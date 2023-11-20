using System.Diagnostics.Tracing;

namespace DockerDotnetTest.Scenario.EmitOwnEvents;

[Flags]
public enum EventSourceKeywords : long
{
    Int = 1 << 0,
    Double = 1 << 1,
    AllNumbers = Int | Double,

    PostParameter = 1 << 3,
    GetParameter = 1 << 4,
    AllParameter = PostParameter | GetParameter
}