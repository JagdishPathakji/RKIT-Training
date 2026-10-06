using System;

class Program
{
    static void Main()
    {
        // 1. Current date and time
        Console.WriteLine("1. DateTime.Now");
        Console.WriteLine(DateTime.Now);


        // 2. Current UTC date and time
        Console.WriteLine("\n2. DateTime.UtcNow");
        Console.WriteLine(DateTime.UtcNow); // london based time


        // 3. Today
        Console.WriteLine("\n3. DateTime.Today");
        Console.WriteLine(DateTime.Today);


        // 4. Create a specific date
        Console.WriteLine("\n4. Creating a DateTime");
        DateTime date = new DateTime(2026, 8, 17);

        Console.WriteLine("Date : " + date);


        // 5. Create date + time
        Console.WriteLine("\n5. Creating DateTime with Time");

        DateTime dateTime = new DateTime(2026, 8, 17, 20, 30, 45);

        Console.WriteLine("Date and Time : " + dateTime);


        // 6. Access individual components
        Console.WriteLine("\n6. DateTime Properties");

        Console.WriteLine("Year        : " + dateTime.Year);
        Console.WriteLine("Month       : " + dateTime.Month);
        Console.WriteLine("Day         : " + dateTime.Day);
        Console.WriteLine("Hour        : " + dateTime.Hour);
        Console.WriteLine("Minute      : " + dateTime.Minute);
        Console.WriteLine("Second      : " + dateTime.Second);
        Console.WriteLine("Millisecond  : " + dateTime.Millisecond);


        // 7. DayOfWeek
        Console.WriteLine("\n7. DayOfWeek");
        Console.WriteLine(dateTime.DayOfWeek);


        // 8. DayOfYear
        Console.WriteLine("\n8. DayOfYear");
        Console.WriteLine(dateTime.DayOfYear);


        // 9. AddDays()
        Console.WriteLine("\n9. AddDays()");

        Console.WriteLine("Original : " + date);
        Console.WriteLine("After +10 days : " + date.AddDays(10));
        Console.WriteLine("After -5 days  : " + date.AddDays(-5));


        // 10. AddMonths()
        Console.WriteLine("\n10. AddMonths()");

        Console.WriteLine("Original : " + date);
        Console.WriteLine("After +2 months : " + date.AddMonths(2));
        Console.WriteLine("After -1 month  : " + date.AddMonths(-1));


        // 11. AddYears()
        Console.WriteLine("\n11. AddYears()");

        Console.WriteLine("Original : " + date);
        Console.WriteLine("After +2 years : " + date.AddYears(2));
        Console.WriteLine("After -1 year  : " + date.AddYears(-1));


        // 12. AddHours()
        Console.WriteLine("\n12. AddHours()");

        Console.WriteLine("Original : " + dateTime);
        Console.WriteLine("After +5 hours : " + dateTime.AddHours(5));


        // 13. AddMinutes()
        Console.WriteLine("\n13. AddMinutes()");

        Console.WriteLine("Original : " + dateTime);
        Console.WriteLine("After +30 minutes : " +
                          dateTime.AddMinutes(30));


        // 14. AddSeconds()
        Console.WriteLine("\n14. AddSeconds()");

        Console.WriteLine("Original : " + dateTime);
        Console.WriteLine("After +20 seconds : " +
                          dateTime.AddSeconds(20));


        // 15. Subtract DateTime values
        Console.WriteLine("\n15. Subtract DateTime");

        DateTime startDate = new DateTime(2026, 8, 10);
        DateTime endDate = new DateTime(2026, 8, 17);

        TimeSpan difference = endDate - startDate;

        Console.WriteLine("Start : " + startDate);
        Console.WriteLine("End   : " + endDate);
        Console.WriteLine("Difference : " + difference);


        // 16. Difference in days
        Console.WriteLine("\n16. Difference in Days");
        Console.WriteLine("Days : " + difference.Days);


        // 17. Compare dates
        Console.WriteLine("\n17. Comparing Dates");

        DateTime date1 = new DateTime(2026, 8, 10);
        DateTime date2 = new DateTime(2026, 8, 17);

        Console.WriteLine("date1 < date2 : " + (date1 < date2));
        Console.WriteLine("date1 > date2 : " + (date1 > date2));
        Console.WriteLine("date1 == date2 : " + (date1 == date2));


        // 18. DateTime.Compare()
        Console.WriteLine("\n18. DateTime.Compare()");

        Console.WriteLine(
            "Compare(date1, date2) : " +
            DateTime.Compare(date1, date2));


        // 19. Parse()
        Console.WriteLine("\n19. DateTime.Parse()");

        string dateString = "17/08/2026";

        // Parse() converts a valid string into a DateTime.
        // But if the input cannot be converted, It throws a FormatException and your program can stop if you don't handle it.

        DateTime parsedDate = DateTime.Parse(dateString);

        Console.WriteLine("String : " + dateString);
        Console.WriteLine("Date   : " + parsedDate);

 
        // 20. TryParse()
        // TryParse() attempts the conversion without throwing an exception for normal invalid

        Console.WriteLine("\n20. DateTime.TryParse()");

        string input = "25/12/2026";
        // out basically means: "The method will put a value into this variable."
        // Put the converted DateTime into `result`
        // puts default value (DateTime.MinValue) if parse fails
        bool success = DateTime.TryParse(input, out DateTime result);

        Console.WriteLine("Conversion successful : " + success);
        Console.WriteLine("Result : " + result);

        // 21. ToString()
        Console.WriteLine("\n21. ToString()");

        Console.WriteLine(dateTime.ToString());


        // 22. Custom formatting
        Console.WriteLine("\n22. Custom Date Formatting");

        Console.WriteLine("dd/MM/yyyy : " +
                          dateTime.ToString("dd/MM/yyyy"));

        Console.WriteLine("MM/dd/yyyy : " +
                          dateTime.ToString("MM/dd/yyyy"));

        Console.WriteLine("yyyy-MM-dd : " +
                          dateTime.ToString("yyyy-MM-dd"));

        Console.WriteLine("dd-MM-yyyy HH:mm:ss : " +
                          dateTime.ToString("dd-MM-yyyy HH:mm:ss"));


        // 23. Date property
        Console.WriteLine("\n23. Date Property");

        Console.WriteLine("Original : " + dateTime);
        Console.WriteLine("Date only : " + dateTime.Date);


        // 24. TimeOfDay
        Console.WriteLine("\n24. TimeOfDay");

        Console.WriteLine("DateTime : " + dateTime);
        Console.WriteLine("Time     : " + dateTime.TimeOfDay);
    }
}