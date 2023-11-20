namespace DockerDotnetTest.Scenario.Exceptions;

public class SetterNotCalledException : Exception
{
    public SetterNotCalledException(){}
    public SetterNotCalledException(string message) : base(message){}
}