> A prepared statement lets you define an SQL statement once and execute it later with different values. They are especially useful when values are dynamic, when the same query runs repeatedly, and when you want to avoid building SQL strings manually.

> It is session-scoped, and within that session it remains until you explicitly deallocate it or the session ends.

# The problem prepared statements solves
Suppose you want to find employees based on salary.

> Without Prepared Statement
SELECT * FROM employees WHERE salary > 10000;
SELECT * FROM employees WHERE salary > 20000;
SELECT * FROM employees WHERE salary > 30000;
The SQL structure is the same. Only the value changes. 
Prepared statement allows us to separate SQL structure and Dynamic values.



# Basic Concept
Instead of:
SELECT * FROM employees WHERE salary > 50000;
We do:
SELECT * FROM employees WHERE salary > ?;

> The '?' is a parameter placeholder. 
You can provide the actual value when executing it.



# MySQL Prepared Statement Syntax
PREPARE
EXECUTE
DEALLOCATE PREPARE

> Basic Structure
```sql
PREPARE statement_name
FROM 'SELECT * FROM employees WHERE salary > ?';

SET @salary = 60000;
EXECUTE statement_name USING @salary;
DEALLOCATE PREPARE statement_name; -- remove the prepared statement from the session
```


# Dynamic Table Name
Suppose we want table name to be selected dynamically.
we cannot do something like:
```sql
PREPARE stmt
FROM 'SELECT * FROM ?';
```

Instead we have to do:
```sql
SET @table_name = 'employees';

SET @query = CONCAT('SELECT * FROM ', @table_name);

PREPARE stmt FROM @query;

EXECUTE stmt;

DEALLOCATE PREPARE stmt;
```



# Prepared Statement and SQL Injection
Major reasons prepared statements are used is to prevent SQL Injection when handling dynamic values.

String concatenation:
> SQL + user input → SQL parser

Prepared statement:
> SQL structure → SQL parser
> user input → parameter/value (input is not part of SQL source code)