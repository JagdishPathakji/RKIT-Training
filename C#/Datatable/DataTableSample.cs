using System;
using System.Data;

class Program
{
    static void PrintTable(DataTable table)
    {
        Console.WriteLine("\nId\tName\tAge\tSalary\tState");

        foreach (DataRow row in table.Rows)
        {
            if (row.RowState == DataRowState.Deleted)
            {
                Console.WriteLine(
                    $"{row["Id", DataRowVersion.Original]}\t" +
                    $"{row["Name", DataRowVersion.Original]}\t" +
                    $"{row["Age", DataRowVersion.Original]}\t" +
                    $"{row["Salary", DataRowVersion.Original]}\t" +
                    $"{row.RowState}"
                );
            }
            else
            {
                Console.WriteLine(
                    $"{row["Id"]}\t" +
                    $"{row["Name"]}\t" +
                    $"{row["Age"]}\t" +
                    $"{row["Salary"]}\t" +
                    $"{row.RowState}"
                );
            }
        }
    }

    static void Main()
    {
        // =========================================================
        // 1. CREATE DATATABLE + COLUMNS + INSERT ROWS
        // =========================================================

        DataTable table = new DataTable("Employees");

        table.Columns.Add("Id", typeof(int));
        table.Columns.Add("Name", typeof(string));
        table.Columns.Add("Age", typeof(int));
        table.Columns.Add("Salary", typeof(int));

        table.Rows.Add(1, "Jagdish", 21, 500000);
        table.Rows.Add(2, "Mihir", 19, 1000000);
        table.Rows.Add(3, "Rudra", 25, 300000);

        Console.WriteLine("=== After Creating + Inserting ===");
        PrintTable(table);


        // =========================================================
        // 2. SELECT() - FILTER
        // =========================================================

        Console.WriteLine("\n=== Select: Salary > 400000 ===");

        DataRow[] filtered = table.Select("Salary > 400000");

        foreach (DataRow row in filtered)
        {
            Console.WriteLine(
                $"{row["Id"]}\t{row["Name"]}\t{row["Salary"]}"
            );
        }


        // =========================================================
        // 3. SELECT() - FILTER + SORT
        // =========================================================

        Console.WriteLine("\n=== Select: Age >= 20, Salary DESC ===");

        DataRow[] sorted = table.Select(
            "Age >= 20",
            "Salary DESC"
        );

        foreach (DataRow row in sorted)
        {
            Console.WriteLine(
                $"{row["Name"]}\t{row["Age"]}\t{row["Salary"]}"
            );
        }


        // =========================================================
        // 4. ACCEPT INITIAL STATE
        // =========================================================

        // Rows were Added. Make them Unchanged.
        table.AcceptChanges();

        Console.WriteLine("\n=== After AcceptChanges() ===");
        PrintTable(table);


        // =========================================================
        // 5. UPDATE - BeginEdit() / EndEdit()
        // =========================================================

        DataRow employee = table.Rows[0];

        employee.BeginEdit();

        employee["Name"] = "Jagdish Patel";
        employee["Age"] = 22;
        employee["Salary"] = 600000;

        employee.EndEdit();

        Console.WriteLine("\n=== After Update + EndEdit() ===");
        PrintTable(table);


        // =========================================================
        // 6. REJECT CHANGES
        // =========================================================

        employee.RejectChanges();

        Console.WriteLine("\n=== After RejectChanges() ===");
        PrintTable(table);


        // =========================================================
        // 7. UPDATE AGAIN + ACCEPT CHANGES
        // =========================================================

        employee.BeginEdit();

        employee["Age"] = 23;
        employee["Salary"] = 650000;

        employee.EndEdit();

        Console.WriteLine("\n=== Modified Again ===");
        PrintTable(table);

        table.AcceptChanges();

        Console.WriteLine("\n=== After AcceptChanges() ===");
        PrintTable(table);


        // =========================================================
        // 8. DELETE() 
        // =========================================================

        DataRow deletedRow = table.Rows[0];

        deletedRow.Delete();

        Console.WriteLine("\n=== After Delete() ===");
        Console.WriteLine($"RowState: {deletedRow.RowState}");

        // Deleted rows cannot be accessed using current values.
        // We therefore print only the remaining attached rows.
        PrintTable(table);


        // =========================================================
        // 9. REJECT DELETE
        // =========================================================

        deletedRow.RejectChanges();

        Console.WriteLine("\n=== After RejectChanges() - Delete Reverted ===");
        PrintTable(table);


        // =========================================================
        // 10. DELETE() + ACCEPTCHANGES()
        // =========================================================

        deletedRow.Delete();

        Console.WriteLine("\n=== Delete() Again ===");
        Console.WriteLine($"RowState: {deletedRow.RowState}");

        table.AcceptChanges();

        Console.WriteLine("\n=== After AcceptChanges() ===");
        Console.WriteLine($"Deleted RowState: {deletedRow.RowState}");
        PrintTable(table);


        // =========================================================
        // 11. REMOVE()
        // =========================================================

        DataRow removedRow = table.Rows[0];

        table.Rows.Remove(removedRow);

        Console.WriteLine("\n=== After Remove() ===");
        Console.WriteLine($"Removed RowState: {removedRow.RowState}");
        PrintTable(table);


        // =========================================================
        // 12. REMOVEAT()
        // =========================================================

        table.Rows.RemoveAt(0);

        Console.WriteLine("\n=== After RemoveAt(0) ===");
        PrintTable(table);
    }
}