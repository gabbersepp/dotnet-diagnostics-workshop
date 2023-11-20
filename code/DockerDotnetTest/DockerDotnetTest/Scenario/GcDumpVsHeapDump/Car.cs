namespace DockerDotnetTest.Scenario.GcDumpVsHeapDump;

public class Car
{
    public List<Wheel> Wheels;

    public Car()
    {
        Wheels = new List<Wheel> { new Wheel(), new Wheel(), new Wheel(), new Wheel() };
    }
}