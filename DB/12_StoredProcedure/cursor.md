> A cursor allows you to process rows returned by a query one row at a time. 
> Normally SQL works with a set of rows at once. A cursor is used when you specifically need row-by-row processing.


# Why do we need cursors ?
Suppose we have
```sql
SELECT * FROM employees WHERE department = 'IT';
```
Result:
1 Rahul  IT       60000
3 Priya  IT       75000
Normally, SQL processes this as a set (collection of rows that SQL is operating on)

But suppose our requirement is:
Get employee 1 -> do something
Get employee 2 -> do something
That's where a cursor can be useful.



# What is a Cursor ?
A cursor is basically a mechanism that lets you maintain a position within a query result and fetch rows one by one. Think of it like a pointer.

Query Result
┌─────────────────────────┐
│ Rahul                   │ ← cursor
├─────────────────────────┤
│ Amit                    │
├─────────────────────────┤
│ Priya                   │
├─────────────────────────┤
│ Neha                    │
└─────────────────────────┘

After fetching Rahul
┌─────────────────────────┐
│ Rahul                   │ ✓ processed
├─────────────────────────┤
│ Amit                    │ ← cursor
├─────────────────────────┤
│ Priya                   │
├─────────────────────────┤
│ Neha                    │
└─────────────────────────┘

Then
┌─────────────────────────┐
│ Rahul                   │ ✓
├─────────────────────────┤
│ Amit                    │ ✓
├─────────────────────────┤
│ Priya                   │ ← cursor
├─────────────────────────┤
│ Neha                    │
└─────────────────────────┘



# Cursors are mainly used in `Stored Programs`
In MySQL, cursors are used inside:
- Stored procedures
- Stored functions
- Triggers

> Cursor declaration belongs inside a stored program block.



# Cursor Lifecycle
DECLARE -> OPEN -> FETCH -> CLOSE



# DECLARE CURSOR
```sql
DECLARE emp_cursor CURSOR FOR SELECT employee_id, employee_name FROM employees; 
```
> DECLARE CURSOR does not fetch the rows.
> It defines which query the cursor will use.



# OPEN
OPEN emp_cursor;
> This opens the cursor and makes the result available for fetching.



# FETCH
FETCH cursor_name INTO v_id, v_name; -- fills the value then moves to next row



# CLOSE
CLOSE emp_cursor;
when you are finished. This closes the cursor.



# NOT FOUND Handler
To detect there are no more rows to fetch, we use `NOT FOUND`.
Example:
```sql
DECLARE finished INT DEFAULT 0;

DECLARE CONTINUE HANDLER FOR NOT FOUND SET finished = 1;
```