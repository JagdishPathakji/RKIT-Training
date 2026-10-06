using System;

class Program
{
    static void Main()
    {
        // 1. Math.PI
        Console.WriteLine("1. Math.PI");
        Console.WriteLine("PI = " + Math.PI);


        // 2. Math.E
        Console.WriteLine("\n2. Math.E");
        Console.WriteLine("E = " + Math.E);


        // 3. Math.Abs()
        Console.WriteLine("\n3. Math.Abs()");
        Console.WriteLine("Abs(-25) = " + Math.Abs(-25));
        Console.WriteLine("Abs(25)  = " + Math.Abs(25));


        // 4. Math.Max()
        Console.WriteLine("\n4. Math.Max()");
        Console.WriteLine("Max(10, 20) = " + Math.Max(10, 20));


        // 5. Math.Min()
        Console.WriteLine("\n5. Math.Min()");
        Console.WriteLine("Min(10, 20) = " + Math.Min(10, 20));


        // 6. Math.Pow()
        Console.WriteLine("\n6. Math.Pow()");
        Console.WriteLine("2^3 = " + Math.Pow(2, 3));
        Console.WriteLine("5^2 = " + Math.Pow(5, 2));


        // 7. Math.Sqrt()
        Console.WriteLine("\n7. Math.Sqrt()");
        Console.WriteLine("Sqrt(25) = " + Math.Sqrt(25));
        Console.WriteLine("Sqrt(2)  = " + Math.Sqrt(2));


        // 8. Math.Cbrt()
        Console.WriteLine("\n8. Math.Cbrt()");
        Console.WriteLine("Cbrt(27) = " + Math.Cbrt(27));


        // 9. Math.Round()
        Console.WriteLine("\n9. Math.Round()");
        Console.WriteLine("Round(4.4) = " + Math.Round(4.4));
        Console.WriteLine("Round(4.6) = " + Math.Round(4.6));
        Console.WriteLine("Round(4.567, 2) = " + Math.Round(4.567, 2));


        // 10. Math.Floor()
        Console.WriteLine("\n10. Math.Floor()");
        Console.WriteLine("Floor(4.9) = " + Math.Floor(4.9));
        Console.WriteLine("Floor(-4.9) = " + Math.Floor(-4.9));


        // 11. Math.Ceiling()
        Console.WriteLine("\n11. Math.Ceiling()");
        Console.WriteLine("Ceiling(4.1) = " + Math.Ceiling(4.1));
        Console.WriteLine("Ceiling(-4.1) = " + Math.Ceiling(-4.1));


        // 12. Math.Truncate()
        Console.WriteLine("\n12. Math.Truncate()");
        Console.WriteLine("Truncate(4.9) = " + Math.Truncate(4.9));
        Console.WriteLine("Truncate(-4.9) = " + Math.Truncate(-4.9));


        // 13. Math.Sign()
        Console.WriteLine("\n13. Math.Sign()");
        Console.WriteLine("Sign(10)  = " + Math.Sign(10));
        Console.WriteLine("Sign(0)   = " + Math.Sign(0));
        Console.WriteLine("Sign(-10) = " + Math.Sign(-10));


        // 14. Math.Sin()
        Console.WriteLine("\n14. Math.Sin()");
        Console.WriteLine("Sin(0) = " + Math.Sin(0));


        // 15. Math.Cos()
        Console.WriteLine("\n15. Math.Cos()");
        Console.WriteLine("Cos(0) = " + Math.Cos(0));


        // 16. Math.Tan()
        Console.WriteLine("\n16. Math.Tan()");
        Console.WriteLine("Tan(0) = " + Math.Tan(0));


        // 17. Math.Log() → base e
        Console.WriteLine("\n17. Math.Log()");
        Console.WriteLine("Log(10) = " + Math.Log(10));


        // 18. Math.Log10() → base 10
        Console.WriteLine("\n18. Math.Log10()");
        Console.WriteLine("Log10(100) = " + Math.Log10(100));


        // 19. Math.Exp()
        Console.WriteLine("\n19. Math.Exp()");
        Console.WriteLine("Exp(1) = " + Math.Exp(1));
    }
}