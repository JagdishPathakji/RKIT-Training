/*
What is Namespace ?
A namespace is a named container for types.
Types include: classes, structs, interfaces, enums, delegates, etc

For example:
namespace MyApplication {

    class Student {
    
    }

    class Teacher {
    
    }
}



Why do we need namespaces ?
Mainly to organize types and avoid naming conflicts.



The System Namespace ?
Console is a class we use in `Console.WriteLine`.
That Console class is provided by System namespace.



using keyword ?
when you write `using System;`, you are telling the compiler that you want to refer to types inside `System` namespace.
*/

using College;
using System;
// using Alias
// using CollegeStudent = College.Student;
// using C = College;

class Program {

    public static void Main(string[] args) {

        Student s = new Student();
        s.Name = "Jagdish";
        Console.WriteLine(s.Name);
    }
}

// Library → reusable code
// Assembly → compiled unit that contains .NET code -> need to explore more
// Namespace → logical organization/name of types
// Type → class, struct, interface, enum, etc.
// using → makes namespace/type names convenient to reference
// NuGet package → distribution/package mechanism for .NET libraries