/*
Generic classes :- 


** Need **
Without generics we might separate classes for different types:
class IntBox {
    int value;
}
class StringBox {
    string value;
}
class DoubleBox {
    double value;
}

Usage:-
IntBox a = new IntBox();
a.Value = 10;

StringBox b = new StringBox();
b.Value = "Hello";

DoubleBox c = new DoubleBox();
c.Value = 10.5;

The classes are doing exactly the same thing. The only difference is the type of value. Generics solve this problem. 
*/


/*
Instead of creating three classes, we create one generic class.
class Box<T> {
    T value;
}
Here, T is a type parameter. 
it means : "I dont know the actual type yet. The person using this class will specify it."

usage:-
Box<int> intBox = new Box<int>();
intBox.value = 10;

Box<string> stringBox = new Box<string>();
stringBox.value = "jagdish";
*/


/*
T = "TYPE PARAMETER"
T is not a special keyword. It is simply the conventional name used for a "type parameter". we can use any name instead of T like TKey, TValue, TResult, TEntity, etc...


Data type = "TYPE ARGUMENT"
When we actually use the class:
Box<int>, int is called a "type argument".


Each constructed generic type is strongly typed. This is the type safety provided by generics.

Generic class can have multiple type parameters. 


*/


/*
Generic constraints restrict what types can be used for T.
"T must satisfy some condition."


1. where T : class
This means T must be a reference type.
(string allowed, int not allowed)
class Box<T> where T : class {
    public T Value;
}

2. where T : struct
T must be a value type.
(int allowed, string now allowed)
class Box<T> where T : struct {
    public T Value;
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
    public void PrintId(T entity)
    {
        Console.WriteLine(entity.Id);
    }
}

1. Repository<Article>
2. Repository<User> 
are valid because both implement IEntity.
*/











/*
Generic Methods :-

The class doesn't need to be generic. Only method has a type parameter.
syntax:
returntype methodname<T>(T parameter) {
    // code
}

usage:
methodname<type>(argument)
or
methodname(argument) // c# automatically infers data type
*/




/*
Generic Collections :-

*/