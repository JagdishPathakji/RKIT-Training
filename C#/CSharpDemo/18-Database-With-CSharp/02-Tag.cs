namespace CSharpDemo.DatabaseWithCSharp;

/// <summary>Represents the Tag type.</summary>
internal sealed class Tag {

    /// <summary>Gets or sets the id value.</summary>
    public int Id { get; init; }
    /// <summary>Gets or sets the name value.</summary>
    public string Name { get; init; } = "";

    /// <summary>Formats the tag ID and name for display.</summary>
    /// <returns>The tag ID and name separated by a vertical bar.</returns>
    public override string ToString() => $"{Id} | {Name}";
}
