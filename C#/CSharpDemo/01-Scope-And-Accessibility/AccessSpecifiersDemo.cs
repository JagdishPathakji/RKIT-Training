using CSharpDemo;
namespace CSharpDemo.ScopeAndAccessibility;

/// <summary>Represents the AccessSpecifiersDemo type.</summary>
public class AccessSpecifiersDemo
{
    /// <summary>Stores the value of the public field.</summary>
    public string PublicValue = "public";
    private string PrivateValue = "private";
    /// <summary>Stores the value of the protected field.</summary>
    protected string ProtectedValue = "protected";
    /// <summary>Stores the value of the internal field.</summary>
    internal string InternalValue = "internal";
    /// <summary>Stores the value of the protected internal field.</summary>
    protected internal string ProtectedInternalValue = "protected internal";
    /// <summary>Stores the value of the private protected field.</summary>
    private protected string PrivateProtectedValue = "private protected";

    /// <summary>Runs the demonstration.</summary>
    public void Run()
    {
        ConsoleHelper.Clear();

        Console.WriteLine("=== SCOPE AND ACCESSIBILITY DEMO ===");

        Console.WriteLine("\nInside the declaring class:");
        Console.WriteLine(PublicValue);
        Console.WriteLine(PrivateValue);
        Console.WriteLine(ProtectedValue);
        Console.WriteLine(InternalValue);
        Console.WriteLine(ProtectedInternalValue);
        Console.WriteLine(PrivateProtectedValue);

        AccessSpecifiersDerivedDemo derivedDemo = new AccessSpecifiersDerivedDemo();
        derivedDemo.PrintAccessibleValues();

        AccessSpecifiersOtherDemo otherDemo = new AccessSpecifiersOtherDemo();
        otherDemo.PrintAccessibleValues();

        // In another assembly, only public is accessible here.
        // A derived class in another assembly can also access protected and protected internal.
        // private, internal, and private protected are not accessible from another assembly.

        Console.WriteLine("\nPress any key to return...");
        Console.ReadKey(true);
    }
}

/// <summary>Represents the AccessSpecifiersDerivedDemo type.</summary>
public class AccessSpecifiersDerivedDemo : AccessSpecifiersDemo
{
    /// <summary>Prints the members available to a derived class.</summary>
    public void PrintAccessibleValues()
    {
        Console.WriteLine("\nInside a derived class in the same assembly:");
        Console.WriteLine(PublicValue);
        // PrivateValue is not accessible here.
        Console.WriteLine(ProtectedValue);
        Console.WriteLine(InternalValue);
        Console.WriteLine(ProtectedInternalValue);
        Console.WriteLine(PrivateProtectedValue);
    }
}

/// <summary>Represents the AccessSpecifiersOtherDemo type.</summary>
public class AccessSpecifiersOtherDemo
{
    /// <summary>Prints the members available to an unrelated class in this assembly.</summary>
    public void PrintAccessibleValues()
    {
        AccessSpecifiersDemo demo = new AccessSpecifiersDemo();

        Console.WriteLine("\nInside an unrelated class in the same assembly:");
        Console.WriteLine(demo.PublicValue);
        // PrivateValue is not accessible here.
        // ProtectedValue is not accessible here.
        Console.WriteLine(demo.InternalValue);
        Console.WriteLine(demo.ProtectedInternalValue);
        // PrivateProtectedValue is not accessible here.
    }
}
