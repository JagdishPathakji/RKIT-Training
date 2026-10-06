using System;
namespace CSharpDemo;

public static class ConsoleHelper
{
    /// <summary>
    /// Clears both the active console viewport and the terminal's scrollback buffer,
    /// ensuring old outputs do not persist or bleed into subsequent views.
    /// </summary>
    public static void Clear()
    {
        try
        {
            Console.Clear();
        }
        catch
        {
            // Ignored if standard output is redirected
        }

        // ANSI / VT escape sequences:
        // \x1b[2J = Erase Entire Display (viewport)
        // \x1b[3J = Erase Saved Lines (scrollback history buffer)
        // \x1b[H  = Move Cursor to Home (1, 1)
        Console.Write("\x1b[2J\x1b[3J\x1b[H");
    }
}
