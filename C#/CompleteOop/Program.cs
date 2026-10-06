using System;

// STATIC CONSTRUCTOR
// ──────────────────
// Belongs to type
// Runs automatically
// Runs at most once
// Initializes static state
// No parameters
// No access modifier -> runtime automatically invokes it, so having access modifiers makes no sense


// INSTANCE CONSTRUCTOR
// ────────────────────
// Belongs to object creation
// Runs when an instance is created
// Can run many times
// Initializes instance state
// Can have parameters
// Can have access modifiers

// public
// → EVERYWHERE

// private
// → SAME CLASS

// protected
// → SAME CLASS + CHILDREN

// internal
// → SAME ASSEMBLY

// protected internal
// → SAME ASSEMBLY OR CHILDREN

// private protected
// → SAME ASSEMBLY AND CHILDREN

// protected
//             Derived
//               ↓
//           same assembly ✅
//           other assembly ✅


// private protected
//             Derived
//               ↓
//           same assembly ✅
//           other assembly ❌


using System;

class Parent
{
    public int a = 1;
    private int b = 2;
    protected int c = 3;
    internal int d = 4;
    protected internal int e = 5;
    private protected int f = 6;

    public void InsideParent()
    {
        Console.WriteLine(a); // public
        Console.WriteLine(b); // private
        Console.WriteLine(c); // protected
        Console.WriteLine(d); // internal
        Console.WriteLine(e); // protected internal
        Console.WriteLine(f); // private protected
    }
}

class Child : Parent
{
    public void InsideChild()
    {
        Console.WriteLine(a); // ✅ public
        // Console.WriteLine(b); // ❌ private

        Console.WriteLine(c); // ✅ protected
        Console.WriteLine(d); // ✅ internal
        Console.WriteLine(e); // ✅ protected internal
        Console.WriteLine(f); // ✅ private protected
    }
}

class Unrelated
{
    public void InsideUnrelated()
    {
        Parent p = new Parent();

        Console.WriteLine(p.a); // ✅ public
        // Console.WriteLine(p.b); // ❌ private
        // Console.WriteLine(p.c); // ❌ protected

        Console.WriteLine(p.d); // ✅ internal
        Console.WriteLine(p.e); // ✅ protected internal

        // Console.WriteLine(p.f); // ❌ private protected
    }
}

class Program
{
    static void Main()
    {
        Parent p = new Parent();

        p.InsideParent();

        Child c = new Child();
        c.InsideChild();

        Unrelated u = new Unrelated();
        u.InsideUnrelated();
    }
}