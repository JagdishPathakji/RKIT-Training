using System;
using System.Data;
/*
A DataTable is a C#/.NET object that represents a table data in memory.
Normally, this kind of data might exists inside a database table. But sometimes C# program needs to temporarily hold tabular data inside RAM. This is where DataTable comes in.
*/




/*
Why do we need it ?
Suppose your application gets data from a databse:
Id    Name     Salary
1     Rahul    50000
2     Priya    60000
3     Amit     45000

You might want to:
- read it
- modify it
- filter it
- sort it
- add rows
- remove rows
- pass it to another part of application
Instead of immediately writing everything back to the database, you can keep a copy in memory using a DataTable.

Database -> DataTable -> C# Program works with the data

Benifits:-
1. Close your database connection right away instead of keeping it open.
2. Run queries once, then work with the data locally in your app memory.
3. Search and filter rows without writing a new database query.
4. Connect easily to grids and forms in desktop or web applications.

Avoid:-
1. Huge amounts of data use too much memory and slow down your app.
2. f you only need to loop through data once, a fast data reader is better.
*/




/*
DataTable belongs to System.Data
The class is: System.Data.DataTable

A DataTable consists of Rows and Columns
*/




class Program {


    static void print(DataTable table) {

        foreach(DataColumn col in table.Columns) {
            Console.Write($"{col.ColumnName} \t");
        }
        Console.WriteLine();

        foreach(DataRow row in table.Rows) {
            foreach(DataColumn column in table.Columns) {
                if (row.RowState == DataRowState.Deleted) {
                    Console.Write(
                        $"{row[column, DataRowVersion.Original]}\t"
                    );
                }
                else {
                    Console.Write($"{row[column]}\t");
                }
            }
            Console.WriteLine();
        }

    }

    static void Main(string[] args) {

        // creating empty datatable with name "Employees"
        DataTable table = new DataTable("Employees");
        Console.WriteLine("DataTable Created");
        Console.WriteLine("Name : "+table.TableName);



        // creating columns
        // every column inside a DataTable is actually represented by a DataColumn object.
        table.Columns.Add("Id", typeof(int));
        table.Columns.Add("Name", typeof(string));
        table.Columns.Add("Age", typeof(int));
        DataColumn salaryColumn = new DataColumn("Salary", typeof(int));
        table.Columns.Add(salaryColumn);



        // creating and inserting DataRow
        // A DataRow represents one record / row inside a DataTable.
        DataRow row1 = table.NewRow();
        row1["Id"] = 1;
        row1["Name"] = "Jagdish";
        row1["Age"] = 21;
        row1["Salary"] = 500000;
        table.Rows.Add(row1);

        DataRow row2 = table.NewRow();
        row2["Id"] = 2;
        row2["Name"] = "Mihir";
        row2["Age"] = 19;
        row2["Salary"] = 1000000;
        table.Rows.Add(row2);

        // shorter way
        table.Rows.Add(3,"Rudra",21,50000);
        table.AcceptChanges();



        // reading row data from a DataTable
        foreach(DataRow row in table.Rows) {
            Console.WriteLine($"Id: {row["Id"]} \t Name: {row["Name"]} \t Age: {row["Age"]} \t Salary: {row["Salary"]}");
        }
        Console.WriteLine();

        // reading col data from a DataTable
        foreach(DataColumn col in table.Columns) {
            Console.WriteLine($"Column: {col.ColumnName}");
        }
        Console.WriteLine();

        // print proper tabular format
        foreach(DataColumn col in table.Columns) {
            Console.Write($"{col.ColumnName} \t");
        }
        Console.WriteLine();

        foreach(DataRow row in table.Rows) {
            foreach(DataColumn column in table.Columns) {
                Console.Write($"{row[column]} \t");
            }
            Console.WriteLine();
        }




        // access data in DataTable using indexing
        // access a row
        // DataRow row = table.Rows[0];
        // acess col for that row
        // row[0], row[1]

        // access directly
        // table.Rows[0][0], table.Rows[0][1],...
        // table.Rows[0]["Id"], table.Rows[0]["Name"],...



        // Select() <- filtering & sorting 

        // 1. Select all rows
        DataRow[] result1 = table.Select();

        foreach (DataRow row in result1) {
            Console.WriteLine($"{row["Id"]} {row["Name"]} {row["Age"]} {row["Salary"]}");
        }
        Console.WriteLine();


        // 2. Select rows using equality
        DataRow[] result2 = table.Select("Age = 21");

        foreach (DataRow row in result2) {
            Console.WriteLine($"{row["Name"]} - {row["Age"]}");
        }
        Console.WriteLine();


        // 3. Select rows using not equal
        DataRow[] result3 = table.Select("Age <> 21");

        foreach (DataRow row in result3) {
            Console.WriteLine($"{row["Name"]} - {row["Age"]}");
        }
        Console.WriteLine();


        // 4. Select rows using greater than
        DataRow[] result4 = table.Select("Salary > 100000");

        foreach (DataRow row in result4) {
            Console.WriteLine($"{row["Name"]} - {row["Salary"]}");
        }
        Console.WriteLine();


        // 5. Select rows using less than
        DataRow[] result5 = table.Select("Salary < 500000");

        foreach (DataRow row in result5) {
            Console.WriteLine($"{row["Name"]} - {row["Salary"]}");
        }
        Console.WriteLine();


        // 6. Greater than or equal to
        DataRow[] result6 = table.Select("Age >= 21");

        foreach (DataRow row in result6) {
            Console.WriteLine($"{row["Name"]} - {row["Age"]}");
        }
        Console.WriteLine();


        // 7. Less than or equal to
        DataRow[] result7 = table.Select("Age <= 21");

        foreach (DataRow row in result7) {
            Console.WriteLine($"{row["Name"]} - {row["Age"]}");
        }
        Console.WriteLine();


        // 8. AND condition
        DataRow[] result8 = table.Select("Age >= 20 AND Salary > 100000");

        foreach (DataRow row in result8) {
            Console.WriteLine($"{row["Name"]} - {row["Age"]} - {row["Salary"]}");
        }
        Console.WriteLine();


        // 9. OR condition
        DataRow[] result9 = table.Select("Age = 19 OR Age = 21");

        foreach (DataRow row in result9) {
            Console.WriteLine($"{row["Name"]} - {row["Age"]}");
        }
        Console.WriteLine();


        // 10. String equality
        DataRow[] result10 = table.Select("Name = 'Jagdish'");

        foreach (DataRow row in result10) {
            Console.WriteLine($"{row["Name"]}");
        }
        Console.WriteLine();


        // 11. String LIKE - starts with
        DataRow[] result11 = table.Select("Name LIKE 'J%'");

        foreach (DataRow row in result11) {
            Console.WriteLine($"{row["Name"]}");
        }
        Console.WriteLine();


        // 12. String LIKE - ends with
        DataRow[] result12 = table.Select("Name LIKE '%r'");

        foreach (DataRow row in result12) {
            Console.WriteLine($"{row["Name"]}");
        }
        Console.WriteLine();


        // 13. String LIKE - contains
        DataRow[] result13 = table.Select("Name LIKE '%i%'");

        foreach (DataRow row in result13) {
            Console.WriteLine($"{row["Name"]}");
        }
        Console.WriteLine();


        // 14. Filter + sorting ascending
        DataRow[] result14 = table.Select(
            "Age >= 20",
            "Salary ASC"
        );

        foreach (DataRow row in result14) {
            Console.WriteLine($"{row["Name"]} - {row["Salary"]}");
        }
        Console.WriteLine();


        // 15. Filter + sorting descending
        DataRow[] result15 = table.Select(
            "Age >= 20",
            "Salary DESC"
        );

        foreach (DataRow row in result15) {
            Console.WriteLine($"{row["Name"]} - {row["Salary"]}");
        }
        Console.WriteLine();


        // 16. Select all rows + sorting
        DataRow[] result16 = table.Select(
            "",
            "Age ASC"
        );

        foreach (DataRow row in result16) {
            Console.WriteLine($"{row["Name"]} - {row["Age"]}");
        }
        Console.WriteLine();


        // 17. Multiple sorting columns
        // First sort by Age, then Salary
        DataRow[] result17 = table.Select(
            "",
            "Age ASC, Salary DESC"
        );

        foreach (DataRow row in result17) {
            Console.WriteLine($"{row["Name"]} - Age: {row["Age"]} - Salary: {row["Salary"]}");
        }
        Console.WriteLine();


        // 18. Filter + multiple sorting columns
        DataRow[] result18 = table.Select(
            "Age >= 20",
            "Age ASC, Salary DESC"
        );

        foreach (DataRow row in result18) {
            Console.WriteLine($"{row["Name"]} - Age: {row["Age"]} - Salary: {row["Salary"]}");
        }
        Console.WriteLine();



        // modifying DataRow
        // 1. directly changing a value
        DataRow rowx = table.Rows[0];
        rowx["Name"] = "Jagdish Pathakji";
        rowx["Age"] = 23;
        rowx["Salary"] = 600000;
        print(table);
        Console.WriteLine();



        // 2. using BeginEdit() and EndEdit() method.
        // "I'm about to make a set of related changes to this row; treat them as one editing operation."
        rowx.BeginEdit();
        rowx["Name"] = "Jay Kishan";
        // rowx.CancelEdit(); // does not make changes whatever made
        rowx.EndEdit(); 
        print(table);
        Console.WriteLine();

        table.AcceptChanges();



        // deleting rows

        // 1. DataRow.Delete() 
        DataRow rowy = table.Rows[0];
        Console.WriteLine(rowy.RowState); // Unchanged
        rowy.Delete();
        Console.WriteLine(rowy.RowState); // Deleted
        print(table);
        Console.WriteLine();

        // Delete() changes the state of the row to Deleted.
        // it does not behave like removing an object.
        // So, Delete() is designed around the idea of tracking changes
        // the row's current version is no longer available
        // however we can ask explicitly for original values
        // to delete permanently do:
        table.AcceptChanges();
        Console.WriteLine(rowy.RowState); // Detached



        // 2. Remove()
        DataRow rowz = table.Rows[1];
        // row is removed actually from datatable
        table.Rows.Remove(rowz);
        Console.WriteLine(table.Rows.Count);
        Console.WriteLine(rowz.RowState); // Detached
        print(table);
        Console.WriteLine();



        // 3. RemoveAt()
        table.Rows.RemoveAt(0);
        print(table);
        Console.WriteLine();


        // Detached : Row is not currently attached to a DataTable
        // Deleted : Row is still part of the DataTable, but marked for deletion


        // when to do AcceptChanges ?
        // purpose :- “Treat the current state of all rows as the new baseline/original state.”

        // Load database data -> DataTable -> Unchanged -> Make several changes -> Added / Modified / Deleted -> Send changes tos to database -> AcceptChanges() -> Unchanged        


        // AcceptChanges() : Current state becomes the new accepted/baseline state.
        // RejectChanges() : Discard changes made since the last accepted state and restore the accepted state. 
    }
}