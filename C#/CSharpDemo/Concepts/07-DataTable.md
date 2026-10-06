# 07 - DataTable: In-Memory Tabular Data

## What problem does `DataTable` solve?

Sometimes code receives or produces data naturally shaped like a table: named columns, typed values, and rows. `System.Data.DataTable` represents that shape in memory. It lets code define a schema, insert/find/update/delete rows, enforce some constraints, and expose sorted or filtered views.

A `DataTable` is not a database, an ORM, or a C# class hierarchy. It does not connect to a database or save itself to disk. It is a mutable in-memory data structure. This guide uses a table of the demo application's menu modules so every example relates to the current project.

## First understand table, schema, column, and row

- A **table** is a collection of rows governed by a schema.
- A **schema** describes the table: column names, data types, nullability, keys, defaults, and constraints.
- A **column** (`DataColumn`) describes one value in each row.
- A **row** (`DataRow`) contains one value for each column, plus row-state/version information.
- A **constraint** enforces a rule such as unique values or a valid relationship.

The general structure looks like this:

```text
DemoModules
+---- Id      : Int32       (key)
+---- Name    : String
+---- Enabled : Boolean

1 | Namespace and libraries | True
2 | Enumerations            | True
3 | File operations         | False
```

The values inside a `DataRow` are accessed by column name or ordinal. A DataTable has a schema at runtime, so the compiler cannot check every column name or value type as strongly as it can for a fixed C# class.

## Create a table and define its schema

```csharp
using System.Data;

var modules = new DataTable("DemoModules");

DataColumn idColumn = modules.Columns.Add("Id", typeof(int));
idColumn.AllowDBNull = false;

DataColumn nameColumn = modules.Columns.Add("Name", typeof(string));
nameColumn.AllowDBNull = false;

DataColumn enabledColumn = modules.Columns.Add("Enabled", typeof(bool));
enabledColumn.DefaultValue = true;

modules.PrimaryKey = new[] { idColumn };
```

Walk through the setup:

1. `new DataTable("DemoModules")` creates an empty in-memory table and gives it a name.
2. `Columns.Add("Id", typeof(int))` creates an integer column. The `typeof` expression supplies a runtime `Type` object describing the kind of value.
3. `AllowDBNull = false` says a value is required in that column.
4. `DefaultValue = true` supplies a value when a new row does not explicitly set `Enabled`.
5. `PrimaryKey` identifies the column or columns that uniquely identify rows and enables key-based lookup.

A table starts without columns. Adding rows before defining a suitable schema is possible in some workflows but usually makes the data less predictable. Define column names and CLR types deliberately. Common types include `int`, `decimal`, `DateTime`, `DateOnly` where supported by APIs, `bool`, and `string`.

## Add rows

Values can be added positionally in column order:

```csharp
modules.Rows.Add(1, "Namespace and libraries", true);
modules.Rows.Add(2, "Enumerations", true);
modules.Rows.Add(3, "File operations", false);
```

Positional input is compact but depends on column order. For code that needs to remain readable as the schema changes, create a row and assign named columns:

```csharp
DataRow newModule = modules.NewRow();
newModule["Id"] = 4;
newModule["Name"] = "DataTable";
// Enabled receives its DefaultValue of true.
modules.Rows.Add(newModule);
```

`NewRow()` creates a row with the table's schema but does not add it to `Rows` until `Rows.Add`. A value assigned to a column must be compatible with that column's declared type. Constraints can cause an exception when a row is added or changed.

## Read rows and values

Use `foreach` to visit rows. `DataRow` has an indexer that accepts a column name or ordinal:

```csharp
foreach (DataRow row in modules.Rows)
{
    int id = (int)row["Id"];
    string name = (string)row["Name"];
    bool enabled = (bool)row["Enabled"];

    Console.WriteLine($"{id}. {name} (enabled: {enabled})");
}
```

The indexer returns `object`, so direct casts are needed and can fail if the schema/value does not match. `Field<T>` is a typed helper that can make intent clearer:

```csharp
int id = row.Field<int>("Id");
string name = row.Field<string>("Name")!;
```

The null-forgiving `!` only tells nullable analysis to suppress a warning; it does not validate at runtime. If the column permits null, represent that in the C# type and handle the value.

## `DBNull.Value`, C# `null`, and nullable columns

`DBNull.Value` is the `System.Data` marker used for a database-style missing value in a DataRow. It is not the same object/value as C# `null`. To store a missing value in a nullable column, use `DBNull.Value`:

```csharp
DataColumn description = modules.Columns.Add("Description", typeof(string));
description.AllowDBNull = true;

DataRow module = modules.Rows.Find(1)!;
module["Description"] = DBNull.Value;

string? text = module.Field<string?>("Description");
Console.WriteLine(text ?? "No description");
```

`Field<T?>` maps `DBNull.Value` to a nullable CLR value for supported nullable types. Directly casting `row["Description"]` to `string` when it contains `DBNull.Value` is not the right null-handling pattern. Use `row.IsNull("Description")` when you want to check explicitly.

The `!` after `Rows.Find(1)` is only a nullability suppression. `Find` returns null when no matching key exists. A production example should check the result:

```csharp
DataRow? module = modules.Rows.Find(999);
if (module is not null)
{
    Console.WriteLine(module.Field<string>("Name"));
}
```

## Primary keys, uniqueness, and finding a row

A primary key identifies a row and must be unique. With a primary key configured, use `Rows.Find` rather than scanning every row yourself:

```csharp
DataRow? enumsModule = modules.Rows.Find(2);
if (enumsModule is not null)
{
    Console.WriteLine(enumsModule.Field<string>("Name"));
}
```

Trying to add a duplicate primary key violates the table's constraint and raises a `ConstraintException`. Validate inputs and handle constraint errors at the boundary where the application can give a useful response. Other useful column rules include `Unique`, `AllowDBNull`, `DefaultValue`, `AutoIncrement`, `MaxLength` for string columns, and `ReadOnly`.

Composite keys use more than one column. Their values must be supplied in key-column order when calling `Rows.Find` with a key array. Choose keys that match the identity rule of the data rather than using a key only because it is convenient.

## Select/filter rows and use a `DataView`

`DataTable.Select` can select rows using a DataColumn expression:

```csharp
DataRow[] enabledRows = modules.Select("Enabled = true", "Name ASC");
```

A `DataView` represents a configurable view over a table. It can sort and filter without physically reordering the table's `Rows` collection:

```csharp
var enabledView = new DataView(modules)
{
    RowFilter = "Enabled = true",
    Sort = "Name ASC"
};

foreach (DataRowView viewRow in enabledView)
{
    Console.WriteLine(viewRow["Name"]);
}
```

The filter/sort text is a DataColumn expression language, not SQL. It uses column names and expression syntax; do not paste arbitrary user input into it without carefully escaping/validating values. A view does not provide database query execution.

## Update and delete rows

Rows can be changed through the indexer. A row's state records whether it was added, changed, deleted, or has no current change:

```csharp
DataRow? row = modules.Rows.Find(2);
if (row is not null)
{
    row["Name"] = "Enum concepts";
    Console.WriteLine(row.RowState); // Modified after the table has accepted initial rows.
}
```

Deletion is marked with `Delete()`:

```csharp
DataRow? row = modules.Rows.Find(3);
row?.Delete();
```

The row may remain in the collection with state `Deleted` until changes are accepted. Calling `AcceptChanges` removes deleted rows; before then, use `DataRowVersion.Original` if you need to inspect their original values. Do not assume `Rows.Find` or ordinary current-value access behaves the same for a deleted row.

## Row states and `AcceptChanges` / `RejectChanges`

The `DataRowState` property can report:

- `Detached`: the row is not currently part of a table.
- `Added`: inserted since the last accepted state.
- `Unchanged`: no tracked edits since the last accept.
- `Modified`: changed since the last accepted state.
- `Deleted`: marked for deletion since the last accepted state.

`AcceptChanges()` accepts the current values: added/modified rows become `Unchanged`, and deleted rows are removed. `RejectChanges()` reverts tracked changes: added rows are removed, modified rows return to their original values, and deleted rows are restored.

This tracking is useful when a component needs to inspect changes. Calling `AcceptChanges()` too early discards the distinction between original and changed values. A DataTable does not automatically save accepted or modified rows anywhere; it only tracks in-memory state.

## `DataSet`, `DataRelation`, and related concepts

A `DataSet` is an in-memory container that can hold multiple DataTables and relations between them. A `DataRelation` connects parent and child columns and can express navigation/constraint relationships between tables. A `DataView` is a view of a table, often for sorting/filtering or binding to UI controls.

These types are useful in some data-binding and legacy integration workflows. They do not mean a live database connection exists, and they do not automatically load or persist data. An application must explicitly obtain data and explicitly choose how to save or serialize it.

## Complete runnable example

The following program defines the schema, adds demo module rows, queries by primary key, filters with a view, updates a row, and shows the row state. Save it in a console project that references the framework-provided `System.Data` APIs:

```csharp
using System;
using System.Data;

internal class Program
{
    private static void Main()
    {
        DataTable modules = CreateModulesTable();
        AddInitialModules(modules);
        modules.AcceptChanges();

        Console.WriteLine("Enabled modules:");
        var enabledView = new DataView(modules)
        {
            RowFilter = "Enabled = true",
            Sort = "Id ASC"
        };

        foreach (DataRowView viewRow in enabledView)
        {
            Console.WriteLine($"{viewRow["Id"]}: {viewRow["Name"]}");
        }

        DataRow? enumModule = modules.Rows.Find(3);
        if (enumModule is not null)
        {
            enumModule["Name"] = "Enumerations and flags";
            Console.WriteLine($"Updated row state: {enumModule.RowState}");
        }

        DataRow? missingModule = modules.Rows.Find(99);
        Console.WriteLine(missingModule is null ? "No module with Id 99." : "Found it.");
    }

    private static DataTable CreateModulesTable()
    {
        var table = new DataTable("DemoModules");

        DataColumn id = table.Columns.Add("Id", typeof(int));
        id.AllowDBNull = false;

        DataColumn name = table.Columns.Add("Name", typeof(string));
        name.AllowDBNull = false;

        DataColumn enabled = table.Columns.Add("Enabled", typeof(bool));
        enabled.DefaultValue = true;

        table.PrimaryKey = new[] { id };
        return table;
    }

    private static void AddInitialModules(DataTable table)
    {
        table.Rows.Add(1, "Namespace and libraries", true);
        table.Rows.Add(2, "Scope and accessibility", true);
        table.Rows.Add(3, "Enumerations", true);
        table.Rows.Add(4, "File operations", false);
    }
}
```

Expected output:

```text
Enabled modules:
1: Namespace and libraries
2: Scope and accessibility
3: Enumerations
Updated row state: Modified
No module with Id 99.
```

`AcceptChanges()` is called after initial insertion so the later name edit becomes `Modified`, not `Added`. Notice that this is only in-memory state; the program does not connect to or write to a database.

## Performance, concurrency, and limitations

- DataTable has more schema/change-tracking machinery than a simple list of typed objects; that flexibility has memory and CPU costs.
- Avoid using it for unbounded data that should be streamed or queried incrementally.
- Column name lookups and conversions can fail at runtime; define schema once and reuse typed `Field<T>` access where appropriate.
- A DataTable is not inherently thread-safe for concurrent mutation. Coordinate access or use a different structure.
- It does not automatically persist data, provide database transactions, or replace a database access library.
- It does not have the same compile-time guarantees as typed domain models.

## Interview questions with answers

### Is a DataTable a database?

No. It is a mutable table in memory. It does not connect to or persist into a database by itself.

### What is a DataColumn versus a DataRow?

A DataColumn describes one typed/schema field. A DataRow contains the values for all columns in one record and tracks its change state.

### What is the difference between `null` and `DBNull.Value`?

`null` is the C# null value for a nullable reference/nullable value. `DBNull.Value` is the `System.Data` marker stored in a DataRow for a database-style missing value. `Field<T?>` helps map it to a nullable CLR value.

### What does setting `PrimaryKey` provide?

It declares key columns, enforces uniqueness/required key values, and enables lookup with `Rows.Find`.

### Why call `AcceptChanges`?

It marks current rows as the accepted baseline: added/modified rows become unchanged, while deleted rows are removed. It also discards the pending-change distinction, so call it only when appropriate.

### What is the difference between `DataTable.Select` and `DataView`?

`Select` returns an array of matching DataRows for a filter/sort expression. `DataView` maintains a configurable sorted/filtered view over the table, commonly useful for binding and repeated viewing.

### When is a typed collection preferable?

When the data shape is stable and application logic benefits from compile-time checking, explicit behavior, and simpler memory/performance characteristics.

## Practice

1. Add a `Category` string column with a default value and explain what happens when you omit it from `Rows.Add`.
2. Try adding a duplicate primary key and handle the resulting constraint error.
3. Add a nullable `Description` column and read it with `Field<string?>` when its value is `DBNull.Value`.
4. Call `AcceptChanges`, modify a row, inspect `RowState`, then call `RejectChanges` and inspect the value again.
5. Add a filter and sort to a `DataView`, then explain why the table's physical row order did not change.
6. Explain whether your current use case is better represented by a DataTable or a list of typed objects.

## Mental model

Think of a DataTable as a schema plus a mutable set of rows and change history, all held in memory. Columns define permitted shape; constraints define rules; rows hold values; views present filtered/sorted interpretations. Persistence and database access are separate responsibilities.
