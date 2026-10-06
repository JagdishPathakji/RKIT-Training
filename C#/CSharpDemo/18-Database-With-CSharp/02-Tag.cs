namespace CSharpDemo.DatabaseWithCSharp;

internal sealed class Tag {

    public int Id { get; init; }
    public string Name { get; init; } = "";

    public override string ToString() => $"{Id} | {Name}";
}