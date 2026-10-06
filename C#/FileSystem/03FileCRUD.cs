using System;
using System.IO;
using System.Text;

class FileCRUDDemo
{
    public static void Main()
    {
        CreateDemo();
        Console.WriteLine("\n----------------------------------------\n");

        WriteDemo();
        Console.WriteLine("\n----------------------------------------\n");

        ReadDemo();
        Console.WriteLine("\n----------------------------------------\n");

        // EncodingDemo();
        // Console.WriteLine("\n-----   -----------------------------------\n");

        DeleteDemo();
        Console.WriteLine("\n----------------------------------------\n");

        CopyMoveDemo();
    }

    // =====================================================
    // 1. CREATING FILES (no FileStream — using File.WriteAllText/AllBytes only)
    // =====================================================
    static void CreateDemo()
    {
        Console.WriteLine("=== CREATE DEMO ===");

        string folder = "FileDemo";
        Directory.CreateDirectory(folder); // safe even if it already exists — no exception

        // ---- Method 1: File.WriteAllBytes — creates the file with raw byte content ----
        string path1 = Path.Combine(folder, "created1.txt");
        byte[] data = Encoding.UTF8.GetBytes("Created via WriteAllBytes\n");
        File.WriteAllBytes(path1, data);
        Console.WriteLine($"Created: {path1}");

        // ---- Method 2: File.WriteAllText — simplest way, creates AND writes in one call ----
        string path2 = Path.Combine(folder, "created2.txt");
        File.WriteAllText(path2, "Created via WriteAllText");
        Console.WriteLine($"Created: {path2}");

        // WriteAllText — you hand it a string, and .NET converts it to bytes internally for you.
        // WriteAllBytes — you convert the string to bytes yourself (Encoding.UTF8.GetBytes(...)), then hand those raw bytes over.
        // files aren't always text. Images, zip files, PDFs — these are pure bytes, not strings. WriteAllBytes is the general tool for "write raw bytes," and it works for text too, just with an extra manual step.

        // ---- Method 3: File.WriteAllLines — creates a file from a list of lines ----
        string path3 = Path.Combine(folder, "created3.txt");
        File.WriteAllLines(path3, new[] { "Created via WriteAllLines", "Second line" });
        Console.WriteLine($"Created: {path3}");

        // ⚠️ GOTCHA: File.WriteAllText / WriteAllBytes / WriteAllLines all OVERWRITE
        // an existing file silently — no warning, no exception.
        File.WriteAllText(path1, "This OVERWROTE the previous content!\n");
        Console.WriteLine($"\nAfter overwrite, {path1} contains:");
        Console.WriteLine(File.ReadAllText(path1));

        // Cleanup so other demos start fresh
        Directory.Delete(folder, recursive: true);
    }

    // =====================================================
    // 2. WRITING FILES
    // =====================================================
    static void WriteDemo()
    {
        Console.WriteLine("=== WRITE DEMO ===");

        string path = "output.txt";

        // File.WriteAllText — OVERWRITES the entire file every time you call it.
        File.WriteAllText(path, "First write\n");
        Console.WriteLine("After 1st WriteAllText:");
        Console.WriteLine(File.ReadAllText(path));

        // Calling it again completely replaces the content — "First write" is GONE.
        File.WriteAllText(path, "Second write — first write is GONE\n");
        Console.WriteLine("After 2nd WriteAllText (overwritten):");
        Console.WriteLine(File.ReadAllText(path));

        // File.AppendAllText — adds to the END of the file, does NOT erase existing content.
        File.AppendAllText(path, "Appended line 1\n");
        File.AppendAllText(path, "Appended line 2\n");
        Console.WriteLine("After AppendAllText x2:");
        Console.WriteLine(File.ReadAllText(path));

        // File.WriteAllLines — takes a string[] (or IEnumerable<string>), writes each as its own line.
        // NOTE: this still OVERWRITES the whole file, just like WriteAllText.
        string[] lines = { "Alpha", "Beta", "Gamma" };
        File.WriteAllLines(path, lines);
        Console.WriteLine("After WriteAllLines (overwritten):");
        Console.WriteLine(File.ReadAllText(path));

        // File.AppendAllLines — appends multiple lines at once without overwriting.
        File.AppendAllLines(path, new[] { "Delta", "Epsilon" });
        Console.WriteLine("After AppendAllLines:");
        Console.WriteLine(File.ReadAllText(path));

        // File.WriteAllBytes — for raw binary data (images, custom formats, etc.), not text.
        byte[] binaryData = { 0x48, 0x65, 0x6C, 0x6C, 0x6F }; // these bytes spell "Hello" in ASCII
        string binPath = "binary_output.bin";
        File.WriteAllBytes(binPath, binaryData);
        Console.WriteLine($"\nWrote {binaryData.Length} raw bytes to {binPath}");
        // Converts bytes → actual readable text
        Console.WriteLine($"Read back as text: {Encoding.ASCII.GetString(File.ReadAllBytes(binPath))}");

        // Cleanup
        File.Delete(path);
        File.Delete(binPath);
    }

    // =====================================================
    // 3. READING FILES
    // =====================================================
    static void ReadDemo()
    {
        Console.WriteLine("=== READ DEMO ===");

        string path = "sample.txt";
        File.WriteAllLines(path, new[]
        {
            "Line 1: Hello",
            "Line 2: World",
            "Line 3: File System in C#"
        });

        // File.ReadAllText — reads the ENTIRE file into a single string (with newlines embedded).
        Console.WriteLine("--- File.ReadAllText (one big string) ---");
        string allText = File.ReadAllText(path);
        Console.WriteLine(allText);

        // File.ReadAllLines — reads the ENTIRE file, splits into a string[].
        // Loads everything into memory BEFORE returning — fine for small files.
        Console.WriteLine("--- File.ReadAllLines (string[], all loaded at once) ---");
        string[] allLines = File.ReadAllLines(path);
        for (int i = 0; i < allLines.Length; i++)
        {
            Console.WriteLine($"[{i}] {allLines[i]}");
        }

        // File.ReadLines — returns IEnumerable<string>, reads LAZILY line-by-line as you iterate.
        // Use this instead of ReadAllLines for huge files — avoids loading the whole thing into memory.
        Console.WriteLine("--- File.ReadLines (lazy, memory-efficient) ---");
        foreach (string line in File.ReadLines(path))
        {
            Console.WriteLine($"(lazy) {line}");
        }

        // File.ReadAllBytes — reads raw bytes, used for binary files (images, PDFs, etc.)
        Console.WriteLine("--- File.ReadAllBytes (raw binary) ---");
        byte[] bytes = File.ReadAllBytes(path);
        Console.WriteLine($"Byte count: {bytes.Length}");
        // Converts bytes → hex string
        Console.WriteLine($"First 10 bytes (hex): {BitConverter.ToString(bytes, 0, 10)}");

        // Cleanup
        File.Delete(path);
    }

    // =====================================================
    // 4. ENCODING — how text becomes bytes and back
    // =====================================================
    // static void EncodingDemo()
    // {
    //     Console.WriteLine("=== ENCODING DEMO ===");

    //     string path = "encoding_test.txt";
    //     string text = "Héllo Wörld — special chars: café, naïve, 日本語";

    //     // Default encoding: File.WriteAllText uses UTF-8 WITHOUT a BOM (Byte Order Mark) by default.
    //     Console.WriteLine("--- Default encoding (UTF-8, no BOM) ---");
    //     File.WriteAllText(path, text);
    //     byte[] defaultBytes = File.ReadAllBytes(path);
    //     Console.WriteLine($"Byte count: {defaultBytes.Length}");
    //     PrintFirstBytes(defaultBytes);

    //     // Explicit UTF-8 WITH BOM — adds 3 special bytes (EF BB BF) at the very start of the file.
    //     // Some programs (like older Excel CSV import) expect this; others choke on it. Know your target.
    //     Console.WriteLine("\n--- UTF-8 WITH BOM ---");
    //     File.WriteAllText(path, text, new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));
    //     byte[] bomBytes = File.ReadAllBytes(path);
    //     Console.WriteLine($"Byte count: {bomBytes.Length} (3 more than no-BOM version)");
    //     PrintFirstBytes(bomBytes); // you'll see EF-BB-BF as the first 3 bytes

    //     // UTF-16 (Unicode) — uses 2 bytes per character (roughly), so file size roughly doubles.
    //     Console.WriteLine("\n--- UTF-16 (Unicode) ---");
    //     File.WriteAllText(path, text, Encoding.Unicode);
    //     byte[] utf16Bytes = File.ReadAllBytes(path);
    //     Console.WriteLine($"Byte count: {utf16Bytes.Length}");
    //     PrintFirstBytes(utf16Bytes);

    //     // ASCII — DANGEROUS for non-English text. Any character outside basic ASCII gets replaced with '?'.
    //     // This is silent, permanent data loss — no exception is thrown.
    //     Console.WriteLine("\n--- ASCII (LOSSY!) ---");
    //     File.WriteAllText(path, text, Encoding.ASCII);
    //     string readBackAscii = File.ReadAllText(path, Encoding.ASCII);
    //     Console.WriteLine($"Read back: {readBackAscii}"); // special characters turned into '?'

    //     // Reading WITHOUT specifying an encoding: File.ReadAllText auto-detects via BOM if one is present,
    //     // otherwise it assumes UTF-8.
    //     Console.WriteLine("\n--- Auto-detection when reading ---");
    //     File.WriteAllText(path, text, new UTF8Encoding(true)); // write with BOM
    //     string autoDetected = File.ReadAllText(path); // no encoding passed — auto-detects from BOM
    //     Console.WriteLine($"Auto-detected read: {autoDetected}");

    //     // Cleanup
    //     File.Delete(path);
    // }

    // Helper used only by EncodingDemo, to print raw byte values in hex
    // static void PrintFirstBytes(byte[] bytes, int count = 10)
    // {
    //     int n = Math.Min(count, bytes.Length);
    //     Console.WriteLine($"First {n} bytes: {BitConverter.ToString(bytes, 0, n)}");
    // }

    // =====================================================
    // 5. DELETING FILES (no FileStream — using File.Exists + a second process concept explained instead)
    // =====================================================
    static void DeleteDemo()
    {
        Console.WriteLine("=== DELETE DEMO ===");

        string path = "to_delete.txt";
        File.WriteAllText(path, "temporary content");

        Console.WriteLine($"Exists before delete: {File.Exists(path)}");
        File.Delete(path);
        Console.WriteLine($"Exists after delete: {File.Exists(path)}");

        // GOTCHA #1: Deleting a file that doesn't exist does NOT throw an exception.
        // It just silently does nothing. This is intentional .NET behavior.
        File.Delete("this_file_never_existed.txt");
        Console.WriteLine("Deleting a non-existent file: no exception thrown (by design)");

        // GOTCHA #2: Deleting a READ-ONLY file throws UnauthorizedAccessException.
        string readOnlyPath = "readonly_file.txt";
        File.WriteAllText(readOnlyPath, "protected content");
        File.SetAttributes(readOnlyPath, FileAttributes.ReadOnly); // mark it read-only

        try
        {
            File.Delete(readOnlyPath); // this will throw
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"\nCaught expected exception: {ex.Message}");
            // Fix: remove the ReadOnly attribute first, then delete
            File.SetAttributes(readOnlyPath, FileAttributes.Normal);
            File.Delete(readOnlyPath);
            Console.WriteLine("Deleted successfully after clearing ReadOnly attribute.");
        }
    }

    // =====================================================
    // 6. COPY & MOVE
    // =====================================================
    static void CopyMoveDemo()
    {
        Console.WriteLine("=== COPY / MOVE DEMO ===");

        // -------------------------------------------------
        // 6.1 Basic Copy & Move (recap)
        // -------------------------------------------------
        string source = "source.txt";
        File.WriteAllText(source, "original content");

        string copyDest = "copy.txt";
        File.Copy(source, copyDest, overwrite: true);
        Console.WriteLine($"Copied to {copyDest}: {File.Exists(copyDest)}");
        Console.WriteLine($"Original still exists after copy: {File.Exists(source)}"); // True

        string moveDest = "moved.txt";
        File.Move(source, moveDest);
        Console.WriteLine($"Source exists after move: {File.Exists(source)}");   // False
        Console.WriteLine($"Moved file exists: {File.Exists(moveDest)}");         // True

        // -------------------------------------------------
        // 6.2 What happens WITHOUT overwrite: true
        // -------------------------------------------------
        Console.WriteLine("\n--- Copy without overwrite, when destination already exists ---");

        File.WriteAllText("a.txt", "content A");
        File.WriteAllText("b.txt", "content B (already exists)");

        try
        {
            // Default overload — no overwrite flag — throws if destination exists
            File.Copy("a.txt", "b.txt");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Caught expected exception: {ex.Message}");
        }

        // Same story for Move — it NEVER overwrites, no overload even allows it
        try
        {
            File.Move("a.txt", "b.txt"); // throws because b.txt exists
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Caught expected exception on Move: {ex.Message}");
        }

        // .NET Core 3.0+ added an overwrite overload for Move too:
        File.Move("a.txt", "b.txt", overwrite: true); // now succeeds, b.txt's old content is gone
        Console.WriteLine($"After overwrite Move, b.txt contains: {File.ReadAllText("b.txt")}");

        File.Delete("b.txt");

        // -------------------------------------------------
        // 6.3 File.Replace — the 3-way operation
        // -------------------------------------------------
        Console.WriteLine("\n--- File.Replace (source, destination, backup) ---");

        // File.Replace is different from Copy/Move: it's built for a very specific scenario —
        // "swap this file in, but keep a backup of what was there before" — atomically.
        //
        // Signature: File.Replace(sourceFileName, destinationFileName, destinationBackupFileName)
        //
        // What it does, in ONE atomic step:
        //   1. Deletes destinationBackupFileName if it exists
        //   2. Renames/moves destinationFileName -> destinationBackupFileName (the "old" file is preserved as backup)
        //   3. Renames/moves sourceFileName -> destinationFileName (the "new" file takes its place)
        //
        // IMPORTANT: destinationFileName MUST already exist, or this throws.
        // This is NOT a general-purpose copy — it's specifically for "replace an existing file, keep a backup".

        string newVersion = "config_new.txt";
        string liveFile = "config_live.txt";
        string backupFile = "config_backup.txt";

        File.WriteAllText(liveFile, "old config v1");     // the "current" file already in use
        File.WriteAllText(newVersion, "new config v2");   // the "new" file we want to swap in

        File.Replace(newVersion, liveFile, backupFile);

        Console.WriteLine($"newVersion exists after Replace: {File.Exists(newVersion)}"); // False — it was consumed/moved
        Console.WriteLine($"liveFile now contains: {File.ReadAllText(liveFile)}");         // "new config v2"
        Console.WriteLine($"backupFile now contains: {File.ReadAllText(backupFile)}");     // "old config v1"

        // This pattern is commonly used for: config file updates, safe "atomic" file swaps,
        // where you never want to be left in a state with NO valid file if something crashes mid-write.

        File.Delete(liveFile);
        File.Delete(backupFile);

        // -------------------------------------------------
        // 6.4 Cross-drive / cross-volume behavior
        // -------------------------------------------------
        Console.WriteLine("\n--- Cross-drive move behavior (conceptual) ---");

        // File.Move behaves differently depending on whether source & destination
        // are on the SAME drive/volume or DIFFERENT ones:
        //
        //   SAME drive/volume  → Move is just a metadata rename. Near-instant, regardless of file size,
        //                        because the actual data blocks never move — only the directory entry changes.
        //
        //   DIFFERENT drive/volume (e.g., C:\ -> D:\, or two different mounted disks on Linux)
        //                      → .NET internally does a full COPY to the new location,
        //                        then DELETES the original. This means:
        //                          - it takes time proportional to file size (not instant)
        //                          - it temporarily uses double the disk space (both copies briefly exist)
        //                          - if the app crashes mid-move, you could end up with a partial file
        //                            on the destination AND the original still on the source (or vice versa)
        //
        // You can't easily "detect" this in code beforehand without comparing drive roots yourself:

        string pathA = @"C:\Temp\file.txt";
        string pathB = @"D:\Backup\file.txt";
        bool sameDrive = string.Equals(
            Path.GetPathRoot(pathA),
            Path.GetPathRoot(pathB),
            StringComparison.OrdinalIgnoreCase
        );
        Console.WriteLine($"Are '{pathA}' and '{pathB}' on the same drive? {sameDrive}");
        // False here — so a real move between these two paths would be a copy+delete under the hood

        // Practical implication: for LARGE files being moved cross-drive, don't assume it's instant.
        // If you need progress reporting or cancellation for such a move, you'd want to do it manually
        // via streaming copy (Streams topic) rather than relying on File.Move's black-box behavior.

        // -------------------------------------------------
        // 6.5 Retry pattern — handling transient lock failures
        // -------------------------------------------------
        Console.WriteLine("\n--- Retry pattern for Copy/Move ---");

        // In real-world apps, a copy/move can fail TRANSIENTLY — not because of a real problem,
        // but because something briefly has the file open: antivirus scanning it, a backup tool,
        // search indexing, another thread in your own app, etc. These failures often resolve
        // themselves within milliseconds if you just try again.
        //
        // A naive single-attempt Copy/Move will crash the whole operation on these blips.
        // The fix: wrap it in a retry loop with a short delay and a maximum attempt count.

        string retrySource = "retry_source.txt";
        string retryDest = "retry_dest.txt";
        File.WriteAllText(retrySource, "important data");

        bool success = CopyWithRetry(retrySource, retryDest, maxAttempts: 3, delayMs: 200);
        Console.WriteLine($"Copy with retry succeeded: {success}");
        Console.WriteLine($"Destination content: {File.ReadAllText(retryDest)}");

        File.Delete(retrySource);
        File.Delete(retryDest);
    }

    // Helper: attempts File.Copy up to 'maxAttempts' times, waiting 'delayMs' between failures.
    // Only retries on IOException (locked file, transient access issue) —
    // does NOT retry on things like UnauthorizedAccessException or FileNotFoundException,
    // since those won't magically resolve themselves by waiting.
    static bool CopyWithRetry(string source, string destination, int maxAttempts, int delayMs)
    {
        for (int attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                File.Copy(source, destination, overwrite: true);
                return true; // success — exit immediately
            }
            catch (IOException ex) when (attempt < maxAttempts)
            {
                // "when (attempt < maxAttempts)" is an exception filter — this catch block
                // only runs if we still have attempts left. On the FINAL attempt, this filter
                // is false, so the exception is NOT caught here and propagates up normally
                // (or you could remove the filter and just check manually — both work).
                Console.WriteLine($"Attempt {attempt} failed ({ex.GetType().Name}: {ex.Message}). Retrying in {delayMs}ms...");
                Thread.Sleep(delayMs);
            }
        }

        // Final attempt, outside the loop's retry logic — let any exception here bubble up for real
        try
        {
            File.Copy(source, destination, overwrite: true);
            return true;
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Final attempt failed: {ex.Message}");
            return false;
        }
    }
}