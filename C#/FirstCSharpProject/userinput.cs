Console.WriteLine("Enter username:");
string userName = Console.ReadLine();
Console.WriteLine("Username is: " + userName);

Console.WriteLine("Enter your age:");
int age = Convert.ToInt32(Console.ReadLine());
Console.WriteLine("Your age is: " + age);

Console.WriteLine(Math.Max(5, 10));
Console.WriteLine(Math.Min(5, 10));
Console.WriteLine(Math.Sqrt(64));

// Math.Abs(-4.7);
// Math.Round(9.99); [nearest whole number]

string txt = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
Console.WriteLine("The length of the txt string is: " + txt.Length);

string txt1 = "Hello World";
Console.WriteLine(txt1.ToUpper());   // Outputs "HELLO WORLD"
Console.WriteLine(txt1.ToLower());   // Outputs "hello world"

string firstName = "John ";
string lastName = "Doe";

string name = firstName + lastName;
Console.WriteLine(name);

string name1 = string.Concat(firstName, lastName);
Console.WriteLine(name1);

string name2 = $"My full name is: {firstName} {lastName}";
Console.WriteLine(name2);

