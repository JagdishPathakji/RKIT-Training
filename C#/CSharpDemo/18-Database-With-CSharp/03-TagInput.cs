namespace CSharpDemo.DatabaseWithCSharp;

/// <summary>Represents the TagInput type.</summary>
internal static class TagInput {

    /// <summary>Reads and validates a positive tag ID from the console.</summary>
    /// <returns>The validated tag ID.</returns>
    public static int ReadId() {

        while(true) {

            Console.Write("Tag ID: ");
            string? input = Console.ReadLine();

            if(int.TryParse(input, out int id) && id > 0) {
                return id;
            }

            Console.WriteLine("Enter a positive whole number.");
        }
    }

    /// <summary>Reads a non-empty tag name of at most 100 characters.</summary>
    /// <param name="prompt">The message to display before reading input.</param>
    /// <returns>The validated tag name.</returns>
    public static string ReadName(string prompt) {

        while(true) {

            Console.Write(prompt);
            string name = Console.ReadLine()?.Trim() ?? "";

            if(name.Length is > 0 and <= 100) {
                return name;
            }

            Console.WriteLine("Name must contain 1 to 100 characters.");
        }
    }
}
