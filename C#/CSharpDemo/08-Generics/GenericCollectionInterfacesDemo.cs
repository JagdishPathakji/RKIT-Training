using System;
using System.Collections.Generic;
namespace CSharpDemo.Generics;

/*
IEnumerable<T> provides an enumerator that starts at the begining of a collection and moves forward one step at a time until it reaches the end.

If a class implements `IEnumerable`, it makes a single simple promise: "You can loop through my items one by one".

Because `IEnumerable<T>` is at the very bottom of the capability ladder, it is highly restricted.
-- You cannot add items
-- You cannot remove items
-- You cannot access items by an index number
-- It does not know how many items it has
*/



/*
ICollection<T> inherits from IEnumerable<T>. That means ICollection<T> can do everything an IEnumerable<T> can do but it adds a whole new set of rules.

If a class implements ICollection<T>, it promises: "You can add items, remove items, and know how many items i am holding".

By using ICollection<T>, we gain access to essential methods and properties:
-- .Count: tells how many items are in the collection
-- .Add(item): puts a new item into the collection
-- Remove(item): takes a specific item out
-- Clear(): wipes the entire collection clean
-- .Contains(item): checks if a specific item exists inside the collection and returns true or false

The Limitation: No Index Based Access
ICollection<T> still does not understand the concept of position. It treats data like a bag of marbles. You know how many marbles are in the bag, and you can take a red marble out, but you cannot ask for "the 3rd marble".
*/



/*
IList<T> inherits from ICollection<T>. This means an IList<T> is an IEnumerable<T> and an ICollection<T>. It does everything what parent classes do. 

If a class implements IList<T>, it promises: "I keep all items in a specific order, and you can access or change any item instantly if you know its index number."

By using IList<T>, we have following additional methods:
-- [index]: can access items via index
-- .Insert(index,item): inserts a new item into a specific position and pushing 
everything else down
-- .RemoveAt(index): deletes whatever is at that index
-- .IndexOf(item): searches for an item and tells you its exact index number or return -1 if it is not present in the IList<T>.
*/



/*
IDictionary<TKey,TValue> dictates how any key-value collection must act. It also implements ICollection and indirectly IEnumerable.

If a class signs the IDictionary<TKey,TValue> contract, it promises: "I map unique key to values and you can instantly look up a value if you give me its key."

It forces the class to implement:
-- [key]: the key-based access like index
-- .Add(key,value): adds a new pair
-- .Contains(key): check if key is present
-- .Keys and .Values: lets you grab just the list of key or just list of values in form of ICollections. 
-- .Remove(key): removes key and value

If you want to add or remove items, you must do it through the IDictionary itself (using dict.Add() or dict.Remove()). The collections returned by .Keys and .Values are strictly for looking at the data or looping through it.
*/



/*
ISet<T> completely ignores order and indexing, If a class implements ISet<T>, it promises: "I absolutely will now allow duplicates, and I am optimized for math-like set operations".

Properties of ISet<T>:
-- Uniqueness: If you try to add "Apple" 100 times, it will only keep one "Apple". It silently ignores duplicates.

-- No Order, No Index: You cannot ask for item [0]. It treats items like a chaotic bag.

-- Blazing Fast .Contains(): Checking if a List contains an item requires checking every single item one by one (O(N) time). A ISet uses a "hash code" to instantly know if the item is there, no matter how big the set is (O(1) time).
*/



/*
Stack<T>: Last In First Out. It inherits IEnumerable<T> only. It has methods .Push() and .Pop() which works at top of stack.

Queue<T>: First In First Out. It inherits IEnumerable<T> only. It has methods .Enqueue() which works at back and .Dequeue() which works at front of Queue. 
*/



/*
IEnumerable<T>  (The Root: "I can loop through items")
│
├── ICollection<T>  (The Modifier: "I have a .Count, and can Add/Remove")
│    │
│    ├── IList<T>  (The Indexer: "I maintain order, access by [index]")
│    │    └── List<T>
│    │
│    ├── IDictionary<TKey, TValue>  (The Lookup: "I map Keys to Values")
│    │    └── Dictionary<TKey, TValue>
│    │
│    └── ISet<T>  (The Unique Collection: "I only allow unique items")
│         └── HashSet<T>
│
└── (Direct Implementers of IEnumerable)
    ├── Stack<T>  (LIFO: Last-In, First-Out)
    └── Queue<T>  (FIFO: First-In, First-Out)
*/
