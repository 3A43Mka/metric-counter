using System;
using System.Reflection;

var allTypes = GetAllTypes();
bool isRunning = true;

while (isRunning)
{
    Console.WriteLine("\nInput metric to show");
    Console.WriteLine("dit | noc | mif | mhf | ahf | aif | pof ");
    Console.WriteLine("...or write \"exit\" to close the app.\n");
    string? input = Console.ReadLine();

    switch (input)
    {
        case "dit":
            {
                handleDIT();
                break;
            }
        case "noc":
            {
                handleNOC();
                break;
            }
        case "mif":
            {
                handleMIF();
                break;
            }
        case "mhf":
            {
                handleMHF();
                break;
            }
        case "ahf":
            {
                handleAHF();
                break;
            }
        case "aif":
            {
                handleAIF();
                break;
            }
        case "pof":
            {
                handlePOF();
                break;
            }
        case "exit":
            {
                isRunning = false;
                break;
            }
        default:
            {
                Console.WriteLine("Unknown command.");
                break;
            }
    }
}

Console.WriteLine("Closing the app...");

void handleDIT()
{
    Type? targetClass = askClass();
    if (targetClass == null)
    {
        return;
    }
    Type? pc = targetClass.BaseType;
    int i = 0;
    while (pc != null && pc.Name != "Object")
    {
        pc = pc.BaseType;
        i += 1;
    }
    Console.WriteLine("DIT (Depth of Inheritance Tree) = " + i);
}

void handleNOC()
{
    Type? targetClass = askClass();
    if (targetClass == null)
    {
        return;
    }
    int count = countClassChildren(targetClass, allTypes);
    Console.WriteLine("NOC (Number of Children) = " + count);
}

void handleMIF()
{
    float totalMethods = 0;
    float totalInherited = 0;
    foreach (var type in allTypes)
    {
        var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        totalMethods += methods.Length;
        foreach (var method in methods)
        {
            if (method.DeclaringType != type)
            {
                totalInherited += 1;
            }
        }
    }
    Console.WriteLine("MIF (Method Inheritance Factor) = " + totalInherited / totalMethods);
}

void handleMHF()
{
    Type? targetClass = askClass();
    if (targetClass == null)
    {
        return;
    }
    var methods = targetClass.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
    float totalHidden = 0;
    float totalMethods = methods.Length;
    foreach (var method in methods)
    {
        if (!method.IsPublic)
        {
            totalHidden += 1;
        }
    }
    Console.WriteLine("MHF (Method Hiding Factor) = " + totalHidden / totalMethods);
}

void handleAHF()
{
    float totalAttributes = 0;
    float totalHidden = 0;
    foreach (var type in allTypes)
    {
        var attributes = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        totalAttributes += attributes.Length;
        foreach (var attribute in attributes)
        {
            if ((attribute.GetMethod == null || attribute.GetMethod.IsPrivate) && (attribute.SetMethod == null || attribute.SetMethod.IsPrivate))
            {
                totalHidden += 1;
            }
        }
    }
    Console.WriteLine("AHF (Attribute Hiding Factor) = " + totalHidden / totalAttributes);
}

void handleAIF()
{
    float totalAttributes = 0;
    float totalInherited = 0;
    foreach (var type in allTypes)
    {
        var attributes = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        totalAttributes += attributes.Length;
        foreach (var attribute in attributes)
        {
            if (attribute.DeclaringType != type)
            {
                totalInherited += 1;
            }
        }
    }
    Console.WriteLine("AHF (Attribute Inheritance Factor) = " + totalInherited / totalAttributes);
}

void handlePOF()
{
    float overriddenCount = 0;
    float denominator = 0;
    foreach (var type in allTypes)
    {
        float ownMethodsCount = 0;
        float noc = countClassChildren(type, allTypes);
        var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        foreach (var method in methods)
        {
            var baseDefinition = method.GetBaseDefinition();
            if (baseDefinition != method && method.DeclaringType == type)
            {
                overriddenCount += 1;
            }
            else if (method.DeclaringType == type)
            {
                ownMethodsCount += 1;
            }
        }
        denominator += ownMethodsCount * noc;
    }
    Console.WriteLine("POF (Polymorphism Object Factor) = " + overriddenCount / denominator);
}

Type?[] GetAllTypes()
{
    string pathToLib = "C:\\Users\\3A43Mka\\Downloads\\metric-counter\\Library\\bin\\Debug\\Library.dll";
    var lib = Assembly.LoadFile(pathToLib);
    return lib.GetTypes();
}

Type? askClass()
{
    Console.WriteLine("All classes:");
    foreach (var type in allTypes)
    {
        Console.Write(type.Name + ", ");
    }
    Console.WriteLine("\nPick the class:");
    string? className = Console.ReadLine();
    foreach (var type in allTypes)
    {
        if (className == type.Name)
        {
            return type;
        }
    }
    Console.WriteLine("No such class found.");
    return null;
}

int countClassChildren(Type targetClass, Type[] types)
{
    int count = 0;
    foreach (var type in types)
    {
        if (type.BaseType != null && type.BaseType.Name == targetClass.Name)
        {
            count += 1;
        }
    }
    return count;
}
