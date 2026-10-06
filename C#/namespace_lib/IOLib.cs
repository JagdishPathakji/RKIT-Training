using System.IO;


string path = "test.txt";
File.WriteAllText(path, "Hello from a Jagdish !!");

string content = File.ReadAllText(path);
Console.WriteLine(content);