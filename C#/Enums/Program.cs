/*
Enums :- An enum is a value type that defines a set of named constant values.

An enum lets you create a type whose valid values come froma predefined set.

when you want something like:-
0 = Pending
1 = Shipped
2 = Delivered
3 = Cancelled
enum solves this problem.
*/

// it has underlying numeric values (0,1,2,3) by default

/*
Best Practices:-
1. Use enums for fixed sets of values.
2. Give the enum a meaningful singular name.
3. Give members meaningful names.
4. Consider defining 0 explicitly as None/Unknown.
5. Validate integers coming from external sources.
6. Don't use enums for dynamic data.
*/

using System;

enum OrderStatus {
    None = 0, // default
    Pending = 1,
    Shipped = 2,
    Delivered = 3,
    Cancelled = 4
}

class Order {
    public int Id { get; set; }
    public OrderStatus Status { get; set; }

    public void PrintStatus() {
        switch (Status) {
            case OrderStatus.None:
                Console.WriteLine("No status assigned.");
                break;

            case OrderStatus.Pending:
                Console.WriteLine("Order is pending.");
                break;

            case OrderStatus.Shipped:
                Console.WriteLine("Order is shipped.");
                break;

            case OrderStatus.Delivered:
                Console.WriteLine("Order is delivered.");
                break;

            case OrderStatus.Cancelled:
                Console.WriteLine("Order is cancelled.");
                break;

            default:
                Console.WriteLine("Unknown status.");
                break;
        }
    }
}

class Program {
    static void Main() {

        OrderStatus status = OrderStatus.Pending;
        Console.WriteLine(status);


        status = OrderStatus.Shipped;
        Console.WriteLine(status);


        if (status == OrderStatus.Shipped) {
            Console.WriteLine("Order has been shipped.");
        }


        int numericValue = (int)status;
        Console.WriteLine("Numeric value: " + numericValue);


        int value = 3;
        OrderStatus convertedStatus = (OrderStatus)value;
        Console.WriteLine("Converted status: " + convertedStatus);


        int externalValue = 99;
        if (Enum.IsDefined(typeof(OrderStatus), externalValue)) {
            OrderStatus validStatus = (OrderStatus)externalValue;
            Console.WriteLine("Valid status: " + validStatus);
        }
        else {
            Console.WriteLine("Invalid status value.");
        }


        bool exists = Enum.IsDefined(typeof(OrderStatus), OrderStatus.Delivered);
        Console.WriteLine("Delivered exists: " + exists);



        status = OrderStatus.Delivered;
        string text = status.ToString();
        Console.WriteLine("String: " + text);



        // 12. Switch with enum
        status = OrderStatus.Shipped;

        switch (status)
        {
            case OrderStatus.Pending:
                Console.WriteLine("Waiting for processing.");
                break;

            case OrderStatus.Shipped:
                Console.WriteLine("Order is on the way.");
                break;

            case OrderStatus.Delivered:
                Console.WriteLine("Order completed.");
                break;

            case OrderStatus.Cancelled:
                Console.WriteLine("Order cancelled.");
                break;

            default:
                Console.WriteLine("Unknown status.");
                break;
        }



        // 14. Default enum value

        OrderStatus defaultStatus = default;
        Console.WriteLine("Default status: " + defaultStatus);


        // 15. Enum inside a class

        Order order = new Order();
        order.Id = 101;
        order.Status = OrderStatus.Shipped;

        Console.WriteLine($"Order {order.Id}: {order.Status}");
        order.PrintStatus();


        // 16. Get all enum values

        Console.WriteLine("\nAll enum values:");

        foreach (OrderStatus item in
                 Enum.GetValues<OrderStatus>())
        {
            Console.WriteLine(
                $"{item} = {(int)item}");
        }


        // 17. Get all enum names

        Console.WriteLine("\nAll enum names:");
        foreach (string name in
                 Enum.GetNames<OrderStatus>())
        {
            Console.WriteLine(name);
        }
    }
}