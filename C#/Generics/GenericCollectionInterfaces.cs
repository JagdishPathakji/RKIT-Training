using System;
using System.Collections.Generic;


// IEnumerable<T>  (The Root: "I can loop through items")
//  │
//  ├── ICollection<T>  (The Modifier: "I have a .Count, and can Add/Remove")
//  │    │
//  │    ├── IList<T>  (The Indexer: "I maintain order, access by [index]")
//  │    │    └── List<T>
//  │    │
//  │    ├── IDictionary<TKey, TValue>  (The Lookup: "I map Keys to Values")
//  │    │    └── Dictionary<TKey, TValue>
//  │    │
//  │    └── ISet<T>  (The Unique Collection: "I only allow unique items")
//  │         └── HashSet<T>
//  │
//  └── (Direct Implementers of IEnumerable)
//       ├── Stack<T>  (LIFO: Last-In, First-Out)
//       └── Queue<T>  (FIFO: First-In, First-Out)










/*
IEnumerable<T> provides an enumerator that starts at the begining of a collection and moves forward one step at a time until it reaches the end. 

If a class implements `IEnumerable`, it makes a single simple promise: "You can loop through my items one by one".

Because `IEnumerable<T>` is at the very bottom of the capability ladder, it is highly restricted. 
-- You cannot add items
-- You cannot remove items
-- You cannot access items by an index number
-- It does not know how many items it has
*/


// ------------------------------------------------
// public class Program {

//     public static void Main() {

//         List<string> secretAgents = new List<string> {"james bond", "jason bourne", "ethan hunt"};

//         // we can hide the list behind the IEnumerable interface
//         IEnumerable<string> readOnlyAgents = secretAgents;

//         // we can iterate over them
//         foreach(string agent in readOnlyAgents) {
//             Console.WriteLine(agent);
//         }

//         // we cannot add or remove or access via index using [] or check how many elements it has using .Count().
//     }
// }
// ----------------------------------------------








/*
ICollection<T> inherits from IEnumerable<T>. That means ICollection<T> can do everything an IEnumerable<T> can do but it adds a whole new set of rules. 

If a class implements ICollection<T>, it promises : "You can add items, remove items and always know exactly how many items i am holding".

By using ICollection<T>, we gain access to essential methods and properties:
-- .Count: tells how many items are in the collection
-- .Add(item): puts a new item into the collection
-- .Remove(item): takes a specific item out
-- .Clear(): wipes the entire collection clean
-- .Contains(item): checks if a specific item exists inside the collection and returns true or false.

The Limitation: No Index based Access
ICollection<T> still does not understand the concept of position. It treats data like a bag of marbles. You know how many marbles are in the bag, and you can take a red marble out, but you cannot ask for "the 3rd marble."
*/


// -----------------------------------------
// class Program {

//     public static void Main() {

//         ICollection<string> shoppingCart = new List<string>();

//         shoppingCart.Add("Milk");
//         shoppingCart.Add("Eggs");
//         shoppingCart.Add("Bread");
        
//         Console.WriteLine($"Items in cart: {shoppingCart.Count}");

//         if (shoppingCart.Contains("Eggs")) {
//             shoppingCart.Remove("Eggs");
//         }

//         Console.WriteLine($"Items after removal: {shoppingCart.Count}");

//         foreach (string item in shoppingCart) {
//             Console.WriteLine(item);
//         }

//         // still can't do this
//         // string firstItem = shoppingCart[0];
//     }
// }
// -----------------------------------------







/*
IList<T> inherist from ICollection<T>. This means an IList<T> is an IEnumerable<T> and an ICollection<T>. It does everything what parent classes do. 

If a class implements IList<T>, it promises: "I keep all items in a specific order, and you can access or change any item instantly if you know its index number."

By using IList<T>, we have following additional methods:
-- [index]: The indexer. Can access items via index.
-- .Insert(index,item): shows a new item into a specific position pushing everything else down.
-- .RemoveAt(index): Deletes whatever is at that index.
-- .IndexOf(item): Searches for an item and tells you its exact index number or retuen -1 if it is not there.
*/

// ---------------------------------------------------
// class Program {

//     public static void Main() {

//         IList<string> topMovies = new List<string>();
//         topMovies.Add("The Matrix");
//         topMovies.Add("Inception");

//         topMovies.Insert(0, "Jurassic Park");
//         Console.WriteLine($"The number 1 movie is: {topMovies[0]}");

//         topMovies[1] = "The Matrix Reloaded";
//         topMovies.RemoveAt(2); // Removes "Inception"

//         int index = topMovies.IndexOf("Jurassic Park");
//         Console.WriteLine($"Jurassic Park is at index: {index}");        
//     }
// }
// ---------------------------------------------------









/*
IDictionary<Tkey,Tvalue> dictates how any key-value collection must act. 

If a class signs the `IDictionary<Tkey,Tvalue>` contract, it promises: "I map unique key to values and you can instantly look up a value if you give me its key."

It forces the class to implement:
-- [key]: The key-based indexer (e.g., myDict["user123"]).
-- .Add(key, value): Adds a new pair.
-- .ContainsKey(key): Fast check to see if a key exists.
-- .Keys and .Values: Lets you grab just the list of keys or just the list of values.
*/


// -------------------------------------------------
// class Program {

//     public static void Main() {

//         IDictionary<string,string> serverConfig = new Dictionary<string,string>();

//         serverConfigs.Add("Database", "192.168.1.10");
//         serverConfigs.Add("Web", "192.168.1.20");
//         serverConfigs["API"] = "192.168.1.30"; // Adds a new key
//         serverConfigs["Web"] = "192.168.1.25"; // Overwrites the existing "Web" key

//         if (serverConfigs.ContainsKey("Database")) {
//             Console.WriteLine($"Database IP is: {serverConfigs["Database"]}");
//         }

//         // IDictionary exposes .Keys and .Values as their own ICollections

//         // If you want to add or remove items, you must do it through the IDictionary itself (using dict.Add() or dict.Remove()). The collections returned by .Keys and .Values are strictly for looking at the data or looping through it.
//         ICollection<string> justTheKeys = serverConfigs.Keys;
//         Console.WriteLine("\nAll Server Roles:");
//         foreach (string role in justTheKeys) {
//             Console.WriteLine(role);
//         }

//         // Iterating through an IDictionary gives you KeyValuePair objects
//         Console.WriteLine("\nFull Configuration:");
//         foreach (KeyValuePair<string, string> config in serverConfigs) {
//             Console.WriteLine($"Role: {config.Key} -> IP: {config.Value}");
//         }
//     }
// }
// -------------------------------------------------








/*
ISet<T> completely ignores order and indexing.

If a class implements ISet<T>, it promises: "I absolutely will not allow duplicates, and I am heavily optimized for math-like Set operations."

Properties of ISet<T>:
-- Uniqueness: If you try to add "Apple" 100 times to a HashSet, it will only keep one "Apple". It silently ignores duplicates.

-- No Order, No Index: You cannot ask a HashSet for item [0]. It treats items like a chaotic bag.

-- Blazing Fast .Contains(): Checking if a List contains an item requires checking every single item one by one ($O(N)$ time). A HashSet uses a "hash code" to instantly know if the item is there, no matter how big the set is (O(1) time).
*/

// --------------------------------------
// public class Program
// {
//     public static void Main()
//     {
//         // 1. Instantiate a concrete HashSet, interact with it via the ISet interface
//         ISet<string> authorizedUsers = new HashSet<string>();

//         // 2. ISet's .Add() returns true if added, false if it's a duplicate!
//         bool firstAdd = authorizedUsers.Add("Alice");  // Returns true
//         bool secondAdd = authorizedUsers.Add("Alice"); // Returns false (Ignored!)
        
//         authorizedUsers.Add("Bob");
//         authorizedUsers.Add("Charlie");

//         Console.WriteLine($"First add success: {firstAdd}");
//         Console.WriteLine($"Second add success: {secondAdd}");
//         Console.WriteLine($"Total users: {authorizedUsers.Count}\n"); // Count is 3, not 4

//         // 3. Fast checking (Inherited from ICollection, optimized by HashSet)
//         if (authorizedUsers.Contains("Bob"))
//         {
//             Console.WriteLine("Bob is verified.\n");
//         }

//         // 4. Mathematical Set Operations (Unique to ISet)
//         ISet<string> currentlyLoggedOn = new HashSet<string> { "Alice", "David" };

//         // IntersectWith modifies the first set to ONLY keep items that exist in BOTH sets
//         authorizedUsers.IntersectWith(currentlyLoggedOn); 
        
//         Console.WriteLine("Users who are BOTH authorized AND logged on:");
//         foreach(string user in authorizedUsers)
//         {
//             // Only "Alice" will print, because she is the only overlap
//             Console.WriteLine(user); 
//         }
//     }
// }
// --------------------------------------







/*
You should almost never use the old non-generic collections. Always use Generics.


1. The IList<T> Family:-
-- List<T>: The standard, go-to dynamic array.
-- T[] (Standard Arrays): Regular arrays like int[] or string[] secretly implement IList<T> under the hood!


2. The IDictionary<TKey, TValue> Family:-
-- Dictionary<TKey, TValue>: The standard, lightning-fast hash table for lookups.
-- SortedDictionary<TKey, TValue>: A dictionary that automatically keeps your keys sorted in alphabetical/numerical order (uses a binary tree).
-- SortedList<TKey, TValue>: Very similar to SortedDictionary, but uses less memory. (Ironically, despite its name, it implements IDictionary, NOT IList!).


3. The ISet<T> Family:-
-- HashSet<T>: The standard, ultra-fast set. Order is completely random.
-- SortedSet<T>: A set that automatically keeps your unique items sorted in alphabetical/numerical order.


4. The ICollection<T> Family:-
-- LinkedList<T>: A chain of items where each item only knows about the item directly in front of it and behind it.


5. The IEnumerable<T> Family:-
-- Stack<T>: Last-In, First-Out (LIFO). You can only .Push() to the top and .Pop() from the top.
-- Queue<T>: First-In, First-Out (FIFO). You can only .Enqueue() to the back and .Dequeue() from the front.
*/