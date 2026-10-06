## 1. The Problem
Imagine we have a List of Articles in our `Knowledge Base`. We want to write a method that filters this list. Sometimes we want Drafts, sometimes we want Published articles and sometimes we want articles by a specific author. 

Instead of writing 10 different methods (GetDrafts, GetPublished, etc), what if we could write ONE method, and pass the filtering rule (a method) into it as a variable ?

To pass a method as a variable, C# requires a specific type. That type is called a `Delegate`.


## 2. What is a Delegate ?
- WHAT IT IS: A delegate is a C# type that defines what kind of method can be stored in a variable. (e.g., "This variable can hold ANY method that takes an Article and returns a bool.")

- WHY IT EXISTS: C# is strongly typed. The compiler needs a strict contract (the delegate) to ensure the method you are passing is safe.


### Example 
```csharp
using System;

namespace KnowledgeBase 
{
    public class Article 
    {
        public string Title { get; set; }
        public bool IsPublished { get; set; }
    }

    class Program 
    {
        // 1. DEFINE THE DELEGATE (The Contract)
        // This says: "Any method that takes an (Article) and returns a (bool) can be stored here."
        public delegate bool ArticleFilterDelegate(Article article);

        // 2. CREATE A MATCHING METHOD
        // This method perfectly matches the contract above!
        static bool CheckIfPublished(Article article) 
        {
            return article.IsPublished == true;
        }

        static void Main() 
        {
            Article myArticle = new Article { Title = "C# Basics", IsPublished = true };

            // 3. ASSIGN THE METHOD TO A VARIABLE
            // We create a variable named 'myRule' of type 'ArticleFilterDelegate'
            // and we store our method inside it!
            ArticleFilterDelegate myRule = CheckIfPublished;

            // 4. EXECUTE THE VARIABLE
            // We call the variable just like a method
            bool result = myRule(myArticle); 
            
            Console.WriteLine($"Is it published? {result}");
        }
    }
}
```


## 3. The Evolution to Lambdas
In previous concept, we had to write a full named method `CheckIfPublished` just so we could assign it to a delegate variable. If you have 50 different filters, writing 50 separate methods is exhausting. C# introduced shorter ways to assign logic to a delegate. This is how we arrived at `Lambda Expressions`.

### Example
```csharp
using System;

namespace KnowledgeBase {

    public class Article {
        public bool IsPublished {get; set;}
    }

    class Program {
        public delegate bool ArticleFilter(Article article);

        static void Main() {

            Article article = new Article {
                IsPublished = true
            };

            ArticleFilter filter = delegate(Article article) {
                return article.IsPublished == true;
            };

            Console.WriteLine($"Is published ? {filter(article)}");
        }
    }
}
```

### Lambda Expressions
Microsoft introduced more shorter version using `=>` operator (The lambda operator). The compiler is smart enough to firgure out the types for you.

### Example
```csharp
using System;

namespace KnowledgeBase {

    public class Article {
        public bool IsPublished {get; set;}
    }

    class Program {

        public delegate bool ArticleFilter(Article article);

        static void Main() {

            Article article = new Article {
                IsPublished = true
            };

            ArticleFilter filter = (article) => article.IsPublished == true;
            Console.WriteLine($"Is published ? {filter(article)}");
        }
    }
}
```


## 4. The Built-in Delegates
In previous all examples, we had to declare `public delegate bool ArticleFilter(Article article)` at the top of our class.

Declaring a custom delegate type everytime you want to use lambda is exteremely annoying. To solve this, Microsoft added three generic delegates directly into C# so you never have to declare your own again: `Action, Func and Predicate`.

### 1. Action<T> (Always returns void)
Use `Action` when your lambda performs a task but does not need to return an answer. 
```csharp
Action<Article> printArticle = article => Console.WriteLine(article.Title);
```

### 2. Func<TInput, TOutput> (Always returns a value)
Use Func when your lambda needs to calculate or return a value. The LAST generic parameter is ALWAYS the return type.
```csharp
// takes an article, returns a string
Func<Article, string> getTitle = article => article.Title.ToUpper();

// Takes user and article, returns a bool
Func<User, Article, bool> canEdit = (user, article) => user.Id = article.AuthorId; 
```

### 3. Predicate<T> (Always returns a bool)
`Predicate<T>` and `Func<T, bool>` have the exact same shape (they both take an input and returns a bool). However, `Predicate<T>` is a special delegate specifically designed for True/False filtering.
```csharp
Predicate<Article> isPublished = article => article.IsPublished == true;
```


## 5. Built-in List<T> Methods
Because passing logic as a parameter is so powerful, Microsoft built methods directly into `List<T>` that accept `Predicate<T>` and `Action<T>`.

## 6. Variable Capture and Closures

1. What is Variable Capture ?
Variable Capture occurs when a lambda expression uses a variable that was declared outside the lambda but is available in its surrounding scope.
Example:-
```csharp
int targetAuthorId = 101;
Predicate<Article> filter = article => article.AuthorId == targetAuthorId; 
```
Here:
- article is a lambda parameter
- targetAuthorId is a captured variable
- The lambda "captures" `targetAuthorId` so it can use it when executed

2. Lambda Parameter vs Captured Variable
2.1. Lambda parameter -- article
This value is supplied when the lambda is called: filter(myArticle);

2.2. Captured variable -- targetAuthorId
This variable comes from the surrounding scope.

3. Practical Example
A lambda captures the variable, not simply a one-time copy of its current value. (lambda has addreess of the variable).
Change in the variable value, lambda gets the latest value everytime.
```csharp
using System;

namespace KnowledgeBase {
    public class Article {
        public int AuthorId { get; set; }
    }

    class Program {
        static void Main() {
            Article article = new Article {
                AuthorId = 102
            };

            int targetAuthorId = 101;

            Predicate<Article> filter =
                a => a.AuthorId == targetAuthorId;

            Console.WriteLine(filter(article));

            targetAuthorId = 102;

            Console.WriteLine(filter(article));
        }
    }
}
```