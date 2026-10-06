// string result = (20 < 18) ? "Good day." : "Good evening.";
// Console.WriteLine(result);

// string[] cars = {"Volvo", "BMW", "Ford", "Mazda"};
// foreach (string i in cars) 
// {
//   Console.WriteLine(i);
// }

// int[] myNum = {10, 20, 30, 40};
// Console.WriteLine(cars.Length);


// use new keyword when you need to specify size

// Create an array of four elements, and add values later
// string[] cars1 = new string[4];
// cars1[0] = "Hello";
// Console.WriteLine(cars1[0]);

// Create an array of four elements and add values right away 
// string[] cars2 = new string[4] {"Volvo", "BMW", "Ford", "Mazda"};

// Create an array of four elements without specifying the size 
// string[] cars3 = new string[] {"Volvo", "BMW", "Ford", "Mazda"};

// Create an array of four elements, omitting the new keyword, and without specifying the size
// string[] cars4 = {"Volvo", "BMW", "Ford", "Mazda"};


// Array.Sort(cars);
// foreach (string i in cars)
// {
//   Console.WriteLine(i);
// }

// int[] myNumbers = {5, 1, 8, 9};
// Array.Sort(myNumbers);
// foreach (int i in myNumbers)
// {
//   Console.WriteLine(i);
// }

// int[,] numbers = { {1, 4, 2}, {3, 6, 8} };
// numbers[0, 0] = 5;  // Change value to 5
// Console.WriteLine(numbers[0, 0]); 

// foreach (int i in numbers)
// {
//   Console.WriteLine(i);
// } 

// matrix.GetLength(0);  // 3 → rows
// matrix.GetLength(1);  // 4 → columns

// for (int i = 0; i < numbers.GetLength(0); i++) 
// { 
//   for (int j = 0; j < numbers.GetLength(1); j++) 
//   { 
//     Console.WriteLine(numbers[i, j]); 
//   } 
// }  

// void print() {
//     Console.WriteLine("hello");
// }
// print();

// named arguments
// void method(string name, string dept) {

//     Console.WriteLine(string.Concat(name,dept));
// }

// method(dept:"IT",name:"Jagdish");



// With top-level statements, your methods are treated as local functions. In that context, C# does not allow local functions to be overloaded by parameter types the way regular class methods can be.

// int PlusMethod(int x, int y)
// {
//   return x + y;
// }

// double PlusMethod(double x, double y)
// {
//   return x + y;
// }

// int myNum1 = PlusMethod(8, 5);
// double myNum2 = PlusMethod(4.3, 6.26);
// Console.WriteLine("Int: " + myNum1);
// Console.WriteLine("Double: " + myNum2);


// class Program {

//     static int PlusMethod(int x, int y)
//     {
//         return x + y;
//     }

//     static double PlusMethod(double x, double y)
//     {
//         return x + y;
//     }

//     static void Main(string[] args) {

//         Console.WriteLine(Program.PlusMethod(5,4));
//         Console.WriteLine(Program.PlusMethod(5.3,2.3));
//     }
// }
