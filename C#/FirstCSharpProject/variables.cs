using System;

class Test {

    public static void Run() {

        // int (32 bit signed)
        int age = 21;
        // long (64 bit signed)
        long population = 80000000;
        // short (16 bit signed)
        short temperature = 100;
        // byte (8 bit unsigned)
        byte val = 21;
        // double (64 bit floating)
        double pi = 3.14159;
        // float (32 bit floating)
        float temp = 36.5f;
        // decimal (high precision)
        decimal price = 999.99m;
        // bool
        bool isloggedin = true;
        // char
        char grade = 'A';
        // string
        string name = "jagdish";

        // Type aliases
        // int     → System.Int32
        // long    → System.Int64
        // short   → System.Int16
        // byte    → System.Byte
        // bool    → System.Boolean
        // char    → System.Char
        // double  → System.Double
        // float   → System.Single
        // decimal → System.Decimal
        // string  → System.String


        // value types vs reference types
        // common value types: int, long, short, byte, float, double, decimal, bool, char, struct, enum
        // common reference types: string, class, array, interface, delegate

        int a = 10;
        int b = a;
        b = 30; // does not change a

        Console.WriteLine(a);
        Console.WriteLine(b);
        
        // string is ref type + immutable
        // array is ref type + mutable
        // class is ref type + mutable / immutable both possible based on design
        string s = "jagdish";
        // s[0] = 'k';

        string s1 = s;

        s1 = "mihir";
        Console.WriteLine(s);
        Console.WriteLine(s1);
        

    }
}