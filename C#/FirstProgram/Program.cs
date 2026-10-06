// ===================================================================================================================================================================
// Using Console.writeline()
// Console.WriteLine("Name: Jagdish");
// Console.WriteLine("College: BVM");
// Console.WriteLine("Course: B.Tech IT");
// Console.WriteLine("Learning: C#");


// Console is a built-in class provided by .NET.
// The Console class is specifically used for interacting with the terminal.
// It allows you to:
// - Display text.
// - Read input from the keyboard.
// - Change text color.
// - Clear the screen.


// WriteLine is an action provided by the Console class.
// Its job is: Display the given text and then move the cursor to the next line.
// ===================================================================================================================================================================
// Creating variables
// string name = "Jagdish";
// string college = "BVM";
// string course = "B.Tech IT";

// Console.WriteLine(name);
// Console.WriteLine(college);
// Console.WriteLine(course);
// ===================================================================================================================================================================
// Data types

// 1. int: Stores whole numbers.
// int age = 20;
// int marks = 95;
// int salary = 50000;

// 2. double: Stores decimal numbers.
// double price = 99.99;
// double pi = 3.14159;

// 3. char: Stores exactly one character.
// char grade = 'A';
// char symbol = '#';

// 4. string: Stores text.
// string name = "Jagdish";
// string college = "BVM";

// 5. bool: Stores only true or false
// bool isStudent = true;
// bool isLoggedIn = false;


// string name = "Jagdish";
// int age = 20;
// double cgpa = 8.75;
// char grade = 'A';
// bool isStudent = true;

// Console.WriteLine(name);
// Console.WriteLine(age);
// Console.WriteLine(cgpa);
// Console.WriteLine(grade);
// Console.WriteLine(isStudent);
// ===================================================================================================================================================================
// Understanding Operators
// Arithmetic Operators
int a = 10;
int b = 3;

Console.WriteLine("Arithmetic Operators");
Console.WriteLine($"a + b = {a + b}");
Console.WriteLine($"a - b = {a - b}");
Console.WriteLine($"a * b = {a * b}");
Console.WriteLine($"a / b = {a / b}");
Console.WriteLine($"a % b = {a % b}");

Console.WriteLine();

// Assignment Operators
int x = 10;

x += 5;
Console.WriteLine($"x += 5 : {x}");

x -= 3;
Console.WriteLine($"x -= 3 : {x}");

x *= 2;
Console.WriteLine($"x *= 2 : {x}");

x /= 4;
Console.WriteLine($"x /= 4 : {x}");

x %= 3;
Console.WriteLine($"x %= 3 : {x}");

Console.WriteLine();

// Comparison Operators
int p = 10;
int q = 20;

Console.WriteLine("Comparison Operators");
Console.WriteLine($"p == q : {p == q}");
Console.WriteLine($"p != q : {p != q}");
Console.WriteLine($"p > q  : {p > q}");
Console.WriteLine($"p < q  : {p < q}");
Console.WriteLine($"p >= q : {p >= q}");
Console.WriteLine($"p <= q : {p <= q}");

Console.WriteLine();

// Logical Operators
bool isStudent = true;
bool hasID = false;

Console.WriteLine("Logical Operators");
Console.WriteLine($"isStudent && hasID : {isStudent && hasID}");
Console.WriteLine($"isStudent || hasID : {isStudent || hasID}");
Console.WriteLine($"!isStudent : {!isStudent}");

Console.WriteLine();

// Increment & Decrement
int n = 5;

Console.WriteLine("Increment / Decrement");
Console.WriteLine($"n = {n}");

n++;
Console.WriteLine($"n++ : {n}");

n--;
Console.WriteLine($"n-- : {n}");

Console.WriteLine();

// Unary Operators
int num = 7;

Console.WriteLine("Unary Operators");
Console.WriteLine($"+num = {+num}");
Console.WriteLine($"-num = {-num}");

Console.WriteLine();

// Ternary Operator
int age = 20;

string result = age >= 18 ? "Adult" : "Minor";

Console.WriteLine("Ternary Operator");
Console.WriteLine(result);
// ===================================================================================================================================================================