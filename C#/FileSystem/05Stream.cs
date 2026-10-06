/*
What is Stream ?
A Stream is just a sequence of bytes flowing between your program and some source (a file, a network, a memory, etc.). 

Instead of loading everything at once (like File.ReadAllText does), a stream lets you read/write in chunks, which is why it is used for large files or performance sensitive code.

FileStream class is the class specifically for reading/writing files as byte streams.
*/

using System;
using System.IO;
using System.Text;

class StreamBasicDemo {

    static void Main() {

        string path = "stream_demo.txt";

        // FileMode: what to do with the file (Create, Open, Append, etc)
        // FileAccess: what you are allowed to (Read, Write, ReadWrite)
        // FileShare: what OTHER processes/handles are allowed to do while you have it open


        using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None)) {
            byte[] data = Encoding.UTF8.GetBytes("Hello via FileStream");
            fs.Write(data, 0, data.Length);
        } // stream closed and flushed here automatically

        Console.WriteLine("Written using FileStream");
    
        // Reading it back
        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read)) {
            byte[] buffer = new byte[fs.Length]; // buffer to hold the read bytes
            fs.Read(buffer, 0, buffer.Length); // After this call, buffer is no longer empty — it now contains the actual byte data that was in the file.
            string text = Encoding.UTF8.GetString(buffer); // Since we wrote with Encoding.UTF8.GetBytes(...), we must read back with Encoding.UTF8.GetString(...)
            Console.WriteLine($"Read back: {text}");
        }

        File.Delete(path);
    }
}
/*
What is Encoding?
Computers only store bytes (numbers 0-255). Text like "Hello" isn't naturally a byte — it's a human concept. Encoding is the rulebook that converts characters ↔ bytes.
*/

/*
What is UTF-8?
UTF-8 is the most common encoding standard today. It's a specific rulebook for converting characters → bytes that:
- Uses 1 byte for standard English/ASCII characters (efficient)
- Uses more bytes (2-4) for special characters, emoji, etc.
- Is the default/standard almost everywhere on the web and in modern apps
*/


/*
fs.Write(data, 0, data.Length);
//        ↑    ↑   ↑
//        |    |   └── count: HOW MANY bytes to write
//        |    └────── offset: START POSITION in the array to begin reading from
//        └─────────── buffer: the byte array containing the data
*/


/*
fs.Read(buffer, 0, buffer.Length);
//       ↑        ↑   ↑
//       |        |   └── count: HOW MANY bytes to attempt to read
//       |        └────── offset: WHERE in the buffer array to start placing the read bytes
//       └─────────────── buffer: an EMPTY array that will get FILLED with the read data
*/



/*
FileMode — Common Values
FileMode.Create     → creates a new file, OVERWRITES if it already exists
FileMode.CreateNew   → creates a new file, THROWS if it already exists (safe, no accidental overwrite)
FileMode.Open        → opens an existing file, THROWS if it doesn't exist
FileMode.OpenOrCreate→ opens if exists, creates if it doesn't (no exception either way)
FileMode.Append      → opens for appending, creates if it doesn't exist
FileMode.Truncate    → opens existing file and wipes its content to zero length
*/

/*
FileAccess — Common Values
FileAccess.Read       → read-only
FileAccess.Write      → write-only
FileAccess.ReadWrite  → both
*/

/*
FileShare — Controls What Others Can Do While You Have It Open
FileShare.None       → nobody else can touch this file while you have it open (fully locked)
FileShare.Read       → others can READ it, but not write, while you have it open
FileShare.ReadWrite  → others can both read and write it too
*/