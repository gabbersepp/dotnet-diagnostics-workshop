using System.Reflection.Emit;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace DockerDotnetTest.Scenario.DynamicClasses;

public class DynamicClassHelper
{
    public static Type CreateAnonymousType(List<string> members)
    {
        AssemblyName dynamicAssemblyName = new AssemblyName("TempAssembly" + DateTime.UtcNow.Ticks.ToString());
        AssemblyBuilder dynamicAssembly =
            AssemblyBuilder.DefineDynamicAssembly(dynamicAssemblyName, AssemblyBuilderAccess.Run);
        var dynamicModule = dynamicAssembly.DefineDynamicModule("TempAssembly");

        // unique type name necessary
        var typename = "DynType" + DateTime.UtcNow.Ticks.ToString();

        var builder = dynamicModule.DefineType(typename, TypeAttributes.Public);
        members.ForEach(f =>
        {
            builder.DefineField(f, typeof(string), FieldAttributes.Public);
        });

        var dynamicAnonymousType = builder.CreateTypeInfo().AsType();
        Activator.CreateInstance(dynamicAnonymousType);
        return dynamicAnonymousType;
    }
}