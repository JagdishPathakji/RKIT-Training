namespace CSharpDemo.DatabaseWithCSharp;

internal static class TagInput {

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