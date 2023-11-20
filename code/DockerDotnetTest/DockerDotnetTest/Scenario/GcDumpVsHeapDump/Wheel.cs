namespace DockerDotnetTest.Scenario.GcDumpVsHeapDump;

public class Wheel
{
    private int index;
    private static int globalIndex;

    public Wheel()
    {
        index = globalIndex++;
    }
}