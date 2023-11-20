namespace DockerDotnetTest.Scenario.Memory;

public class MemoryConsumption : IMemoryConsumption
{
    private List<byte[]> Cache = new List<byte[]>();

    public MemoryConsumption()
    {
        
    }

    public void AddCache()
    {
        var bytes = new byte[1048576];
        Cache.Add(bytes);
    }
}