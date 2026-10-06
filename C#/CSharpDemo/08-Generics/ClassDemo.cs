using System;
using System.Collections.Generic;
using System.Linq;
namespace CSharpDemo.Generics;

/*
Generic Classes
*/

/*
Without generics, we might separate classes for different types:
class IntBox {
    int value;
}
class StringBox { 
    string value;
}
class DoubleBox { 
    double value;
}

The classes are doing exactly the same thing. The only difference is the type of value. Generics solves this problem.
*/



/*
Instead of creating three classes, we create one generic class.
class Box<T> {
    T value;
}

Here, T is a type parameter. It means: "I dont know actual type yet. The person using this class will specify it."

Example:
Box<int> b1 = new Box<int>();
b1.value = 10;

Box<string> b2 = new Box<string>();
b2.value = "hello";
*/



/*
Generic constraints restrict what types can be used for T. "T must satisfy some condition."

1. where T : class
This means T must be a reference type.
(string allowed, int not allowed)

class Box<T> where T : class {
    public T value;
}


2. where T : struct
This means T must be a value type.
(int allowed, string not allowed)

class Box<T> where T : struct {
    public T value;
}


3. where T : SomeInterface
T is guaranteed to implement some interface. 

interface IEntity {
    int Id { get; }
}

class Article : IEntity {
    public int Id { get; set; }
}

class User : IEntity { 
    public int Id { get; set; }
}

class Repository<T> where T : IEntity {

    public void PrintId(T entity) {
        Console.WriteLine(entity.Id);
    }
}

Both Repository<Article> and Repository<User> are valid because they implement IEntity.
*/

