namespace DockerDotnetTest.Scenario.Exceptions;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

public static class RestrictiveGetterClassBuilder
{
    private static readonly Dictionary<string, Type> DynamicClassCache = new();

    public static object Build(object input)
    {
        var className = $"Proxy_{input.GetType().Name}";
        var inputType = input.GetType();
        var properties = inputType.GetProperties()
            .Select(x => new { x.Name, x.PropertyType, x }).ToArray();

        if (!DynamicClassCache.TryGetValue(className, out var outputType))
        {
            TypeBuilder dynamicClass = CreateClass(inputType, className);
            CreateConstructor(dynamicClass);

            foreach (var prop in properties)
            {
                CreateProperty(dynamicClass, prop.Name, prop.PropertyType);
            }

            outputType = dynamicClass.CreateType();
            DynamicClassCache[className] = outputType;
        }

        return Activator.CreateInstance(outputType);
    }

    private static TypeBuilder CreateClass(Type baseType, string className)
    {
        var assemblyName = new AssemblyName(className);
        AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
        ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
        TypeBuilder typeBuilder = moduleBuilder.DefineType(
            assemblyName.FullName,
            TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.AutoClass | TypeAttributes.AnsiClass | TypeAttributes.BeforeFieldInit | TypeAttributes.AutoLayout,
            baseType);
        return typeBuilder;
    }

    private static void CreateConstructor(TypeBuilder typeBuilder)
    {
        typeBuilder.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);
    }

    private static void CreateProperty(TypeBuilder typeBuilder, string propertyName, Type propertyType)
    {
        FieldBuilder backingFieldBuilder = typeBuilder.DefineField($"_{propertyName}", propertyType, FieldAttributes.Private);
        FieldBuilder trackingFieldBuilder =
            typeBuilder.DefineField($"was_{propertyName}_set", typeof(bool), FieldAttributes.Private);

        MethodBuilder getPropMthdBldr = typeBuilder.DefineMethod(
            $"get_{propertyName}",
            MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig | MethodAttributes.Virtual, propertyType, Type.EmptyTypes);
        ILGenerator getIl = getPropMthdBldr.GetILGenerator();

        Label labelReturnValue = getIl.DefineLabel();

        getIl.Emit(OpCodes.Ldarg_0);
        getIl.Emit(OpCodes.Ldfld, trackingFieldBuilder);
        getIl.Emit(OpCodes.Brtrue_S, labelReturnValue);

        var exceptionConstructor = typeof(SetterNotCalledException).GetConstructor(new[] { typeof(string) });
        //getIl.Emit(OpCodes.Ldstr, propertyName);
        getIl.Emit(OpCodes.Newobj, exceptionConstructor);
        getIl.Emit(OpCodes.Throw);

        getIl.MarkLabel(labelReturnValue);
        getIl.Emit(OpCodes.Ldarg_0);
        getIl.Emit(OpCodes.Ldfld, backingFieldBuilder);
        getIl.Emit(OpCodes.Ret);

        MethodBuilder setPropMthdBldr = typeBuilder.DefineMethod(
            $"set_{propertyName}",
            MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig | MethodAttributes.Virtual,
            null, new[] { propertyType });

        ILGenerator setIl = setPropMthdBldr.GetILGenerator();

        setIl.Emit(OpCodes.Ldarg_0);
        setIl.Emit(OpCodes.Ldc_I4_1);
        setIl.Emit(OpCodes.Stfld, trackingFieldBuilder);

        setIl.Emit(OpCodes.Ldarg_0);
        setIl.Emit(OpCodes.Ldarg_1);
        setIl.Emit(OpCodes.Stfld, backingFieldBuilder);
        setIl.Emit(OpCodes.Ret);
    }
}
