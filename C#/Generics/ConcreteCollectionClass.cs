using System;
using System.Collections.Generic;

public class Program
{
    public static void Main()
    {
        DemoList();
        DemoDictionary();
        DemoSortedDictionary();
        DemoHashSet();
        DemoSortedSet();
        DemoStack();
        DemoQueue();
    }

    // ==========================================
    // 1. LIST<T> - The standard numbered array
    // ==========================================
    static void DemoList()
    {
        Console.WriteLine("--- 1. LIST DEMO ---");
        List<string> fruits = new List<string>();

        // MUST-KNOW METHODS:
        fruits.Add("Apple");                          // Add single item
        fruits.AddRange(new string[] { "Banana", "Cherry" }); // Add multiple items
        fruits.Insert(0, "Mango");                    // Insert at specific index
        
        fruits.Remove("Banana");                      // Remove by value
        fruits.RemoveAt(0);                           // Remove by index
        
        fruits.Sort();                                // Alphabetical sort

        Console.WriteLine($"Contains Apple? {fruits.Contains("Apple")}");
        Console.WriteLine($"Item at index 0: {fruits[0]}\n");
    }

    // ==========================================
    // 2. DICTIONARY<TKey, TValue> - Fast Key/Value lookups
    // ==========================================
    static void DemoDictionary()
    {
        Console.WriteLine("--- 2. DICTIONARY DEMO ---");
        Dictionary<int, string> employees = new Dictionary<int, string>();

        // MUST-KNOW METHODS:
        employees.Add(101, "Alice");                  // Standard add (crashes if 101 already exists)
        employees.TryAdd(101, "Bob");                 // Safe add (ignores if 101 exists)
        employees[102] = "Charlie";                   // Indexer add/update
        
        employees.Remove(101);                        // Remove by key

        // Safe reading (The most important Dictionary method)
        // sets name to default value if fails ("", 0, etc)
        if (employees.TryGetValue(102, out string name))
        {
            Console.WriteLine($"Found employee 102: {name}");
        }

        Console.WriteLine($"Does ID 999 exist? {employees.ContainsKey(999)}\n");
    }

    // ==========================================
    // 3. SORTED DICTIONARY<TKey, TValue> - Auto-sorts by Key
    // ==========================================
    static void DemoSortedDictionary()
    {
        Console.WriteLine("--- 3. SORTED DICTIONARY DEMO ---");
        SortedDictionary<string, int> gameScores = new SortedDictionary<string, int>();

        // MUST-KNOW METHODS: (Same as Dictionary, but watch the loop output!)
        gameScores.Add("Charlie", 85);
        gameScores.Add("Alice", 95);
        gameScores.Add("Bob", 70);

        // It automatically prints Alice, Bob, Charlie (Alphabetical by Key!)
        foreach (KeyValuePair<string, int> entry in gameScores)
        {
            Console.WriteLine($"{entry.Key} scored {entry.Value}");
        }
        Console.WriteLine();
    }

    // ==========================================
    // 4. HASHSET<T> - Unique items only, very fast
    // ==========================================
    static void DemoHashSet()
    {
        Console.WriteLine("--- 4. HASHSET DEMO ---");
        HashSet<int> activeUsers = new HashSet<int>();

        // MUST-KNOW METHODS:
        activeUsers.Add(1);
        activeUsers.Add(2);
        bool addedAgain = activeUsers.Add(1);         // Returns false, duplicate ignored!

        activeUsers.Remove(2);

        // Mathematical Set operations
        HashSet<int> premiumUsers = new HashSet<int> { 1, 3, 5 };
        
        // Modifies activeUsers to ONLY keep numbers that are in both sets
        activeUsers.IntersectWith(premiumUsers); 

        Console.WriteLine($"Was duplicate added? {addedAgain}");
        Console.WriteLine($"Is User 1 still active? {activeUsers.Contains(1)}\n");
    }

    // ==========================================
    // 5. SORTED SET<T> - Unique items, auto-sorted
    // ==========================================
    static void DemoSortedSet()
    {
        Console.WriteLine("--- 5. SORTED SET DEMO ---");
        SortedSet<int> ages = new SortedSet<int>();

        // MUST-KNOW METHODS:
        ages.Add(50);
        ages.Add(10);
        ages.Add(99);
        ages.Add(10); // Duplicate ignored

        // Instant access to lowest and highest (Because it is sorted)
        Console.WriteLine($"Youngest: {ages.Min}");
        Console.WriteLine($"Oldest: {ages.Max}");

        Console.Write("All ages in order: ");
        foreach (int age in ages) Console.Write(age + " "); // Prints: 10 50 99
        Console.WriteLine("\n");
    }

    // ==========================================
    // 6. STACK<T> - Last-In, First-Out (LIFO)
    // ==========================================
    static void DemoStack()
    {
        Console.WriteLine("--- 6. STACK DEMO ---");
        Stack<string> undoHistory = new Stack<string>();

        // MUST-KNOW METHODS:
        undoHistory.Push("Typed 'Hello'");            // Add to top
        undoHistory.Push("Typed 'World'");

        string topItem = undoHistory.Peek();          // Look at top without removing
        string removedItem = undoHistory.Pop();       // Remove and get top item

        Console.WriteLine($"Peeked at: {topItem}");
        Console.WriteLine($"Popped (Undid): {removedItem}");
        Console.WriteLine($"Now on top: {undoHistory.Peek()}\n");
    }

    // ==========================================
    // 7. QUEUE<T> - First-In, First-Out (FIFO)
    // ==========================================
    static void DemoQueue()
    {
        Console.WriteLine("--- 7. QUEUE DEMO ---");
        Queue<string> printerLine = new Queue<string>();

        // MUST-KNOW METHODS:
        printerLine.Enqueue("Document1.pdf");         // Add to back of line
        printerLine.Enqueue("Image.png");

        string nextInLine = printerLine.Peek();       // Look at front without removing
        string finishedItem = printerLine.Dequeue();  // Remove and get front item

        Console.WriteLine($"Peeked at front: {nextInLine}");
        Console.WriteLine($"Dequeued (Printed): {finishedItem}");
        Console.WriteLine($"Now at front: {printerLine.Peek()}\n");
    }
}




/*
HashSet<T> (Hash Table Engine)

(*) When you INSERT an item:
----------------------------
Step 1: C# calculates the mathematical Hash Code of the item (e.g., 837291).

Step 2: It divides that hash code by the number of available memory buckets to get a specific bucket index (e.g., Bucket 4).

Step 3: It jumps straight to Bucket 4.

Step 4: If the bucket is empty, it drops the item in.

Step 5: If the bucket already has items (a collision), it checks if the exact item is already there. If yes, it aborts (no duplicates). If no, it links the new item to the end of that bucket's short list.

When you SEARCH for an item:
----------------------------
Step 1: C# calculates the Hash Code of the item you are looking for.

Step 2: It calculates the bucket index using the same math.

Step 3: It jumps directly to that specific bucket, skipping the rest of the collection entirely.

Step 4: It searches only the items residing inside that one bucket.
*/



/*
SortedSet<T> (Red-Black Tree Engine)
When you INSERT an item:
------------------------
Step 1: C# starts at the very top of the tree (the Root node).

Step 2: It compares your new item to the current node.

Step 3: If your item is smaller, it moves down to the Left branch. If it is larger, it moves down to the Right branch. (If it is exactly the same, it aborts to prevent duplicates).

Step 4: It repeats this Left/Right comparison until it hits an empty spot at the bottom of the tree, and attaches the new item there.

Step 5: C# instantly checks the tree's balance. If one side is getting too long, it automatically "rotates" the nodes to keep the tree perfectly balanced.

When you SEARCH for an item:
-----------------------------
Step 1: C# starts at the Root node.

Step 2: It compares the item you are searching for with the current node.

Step 3: If it matches, the search is over.

Step 4: If it doesn't match, it goes Left (if smaller) or Right (if larger).

Step 5: It repeats this until it either finds the item or hits a dead end (meaning the item doesn't exist).
*/