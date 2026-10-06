// method overloading means havving multiple methods with the same name but different parameters lists.
// To override:-
// number of params, type of params
// its compile time polymorpishm because which method to call is decided at comiple time.

// runtime polymorphism :- method overriding


// virtual vs override vs abstract (main thing in OOP)
/*
virtual :- parent provides a default implementation.
child may override it.
class Animal {
    public virtual void MakeSound() {
        Console.WriteLine("Some Sound");
    }
}
*/


/*
abstract :- parent provides no implementation. (class is abstract class)
A concrete child must override it.
abstract class Animal {
    public abstract void MakeSound();
}
*/


/*
override :- child replaces / extends the inherited virtual / abstract implementation.
class Dog : Animal {
    public override void MakeSound() {
        Console.WriteLine("Dog Sound");
    }
}
*/


abstract class Employee {

    public string Name {get; set;}

    protected Employee(string name) {
        Name = name;
    }

    public abstract void Work();

    public virtual void Report() {
        Console.WriteLine($"{Name} submitted a report");
    }

    public void Print() {
        Console.WriteLine($"Employee : {Name}");
    }

    public void Print(string department) {
        Console.WriteLine($"Employee: {Name}, Department: {department}");
    }

    public void Print(string department, int experience) {
        Console.WriteLine($"Employee: {Name}, Department: {department}, Experience: {experience} years");
    }
}

class Developer : Employee {

    public Developer(string name) : base(name) {
    
    }

    public override void Work() {
        Console.WriteLine($"{Name} is writing code");
    }

    public override void Report() {
        base.Report();
        Console.WriteLine($"{Name} submitted a development report");
    }
}

class Designer : Employee {
    public Designer(string name) : base(name) {
    }

    public override void Work() {
        Console.WriteLine($"{Name} is designing UI");
    }
}

class Program {
    static void Main() {
        Employee developer = new Developer("Jagdish");
        Employee designer = new Designer("Rahul");

        developer.Work();
        developer.Report();

        designer.Work();
        designer.Report();

        developer.Print();
        developer.Print("IT");
        developer.Print("IT", 2);
    }
}