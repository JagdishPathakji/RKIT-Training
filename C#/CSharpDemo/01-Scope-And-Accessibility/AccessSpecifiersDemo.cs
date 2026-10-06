using CSharpDemo;
namespace CSharpDemo.ScopeAndAccessibility;

public class AccessSpecifiersDemo
{
    public string PublicValue = "public";
    private string PrivateValue = "private";
    protected string ProtectedValue = "protected";
    internal string InternalValue = "internal";
    protected internal string ProtectedInternalValue = "protected internal";
    private protected string PrivateProtectedValue = "private protected";

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

public class AccessSpecifiersDerivedDemo : AccessSpecifiersDemo
{
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

public class AccessSpecifiersOtherDemo
{
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