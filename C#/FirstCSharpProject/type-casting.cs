// Implicit casting is done automatically when passing a smaller size type to a larger size type:
int myInt = 9;
double myDouble = myInt;       // Automatic casting: int to double

Console.WriteLine(myInt);      // Outputs 9
Console.WriteLine(myDouble);   // Outputs 9



// Explicit casting must be done manually by placing the type in parentheses in front of the value:
double myDouble1 = 9.78;
int myInt1 = (int) myDouble1;    // Manual casting: double to int

Console.WriteLine(myDouble1);   // Outputs 9.78
Console.WriteLine(myInt1);      // Outputs 9


// It is also possible to convert data types explicitly by using built-in methods, such as Convert.ToBoolean, Convert.ToDouble, Convert.ToString, Convert.ToInt32 (int) and Convert.ToInt64 (long):
int myInt2 = 10;
double myDouble2 = 5.25;
bool myBool2 = true;

Console.WriteLine(Convert.ToString(myInt2));    // convert int to string
Console.WriteLine(Convert.ToDouble(myInt2));    // convert int to double
Console.WriteLine(Convert.ToInt32(myDouble2));  // convert double to int
Console.WriteLine(Convert.ToString(myBool2));   // convert bool to string
