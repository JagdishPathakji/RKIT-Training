# What is a Temporary Table ?
A temporary table is a table that exists only temporarily during database sessions. It behaves almost like a normal table, but MySQL automatically removes it when the session / connection ends.

<!-- create -->
```sql
CREATE TEMPORARY TABLE temp_empoloyees (
    id INT,
    name VARCHAR(50),
    salary DECIMAL(10,2)
);
```


<!-- insert -->
```sql
INSERT INTO temp_employees 
VALUES
(1, 'Rahul', 50000),
(2, 'Jagdish', 10000),
(3, 'Mihir', 20000);
```

<!-- select -->
SELECT * FROM temp_employees;



# Why do we need Temporary Tables ?
Suppose you have a complicated query whose result you need to use multiple times. Instead of repeatedly executing the complicated query, you can store its result temporarily. 

<!-- example -->
```sql
CREATE TEMPORARY TABLE high_salary_employees AS
SELECT * FROM employees WHERE salary > 50000;

SELECT * FROM high_salary_employees;
```


# Temporary Table lifetime
A temporary table normally exists for the current database session / connection.

<!-- manually dropping a temporary table -->
DROP TEMPORARY TABLE temp_test;
DROP TEMPORARY TABLE IF EXISTS temp_table;



# Can Two users have temporary tables with the same name ?
Yes.
They are seperate temporary table because they belong to different sessions.



# CTE vs Temporary Table
> CTE = Temporary query result -> primarily used within one statement only.
> Temporary Table = Actual table -> Can be used across multiple statements and is deleted only after connection is closed.




# View vs Temporary Table
> View = A saved SQL query. -> permanent until dropped -> available in other sessions as well
> Temporary Table = Actual temporary table. -> only for current session -> not available in other sessions



# Use Case of Temporary Table
1. store intermediate query result so you can perform multiple operations on them.
2. when we dont want to calculate expensive intermediate result