# Common Table Expression
A common table expression (CTE) is a temporary named result set that exists only for the duration of a single SQL statament.

### Important Points
- Temporary
- Has a name
- Exists only for one query
- Can be referenced like a table
- Improves readability
- Organize complex query easily

### Basic Syntax
Think of the entire thing as one SQL statement, not two separate queries.
<br>
```sql
WITH cte_name AS 
(
    SELECT ...
)
SELECT * FROM cte_name
```
### Sample Example
Think of the entire thing as one SQL statement, not two separate queries.
<br>
```sql
WITH ITEmployees AS
(
    SELECT * FROM employee WEHRE department = 'IT';
)
SELECT * FROM ITEmployees;
```

CTE does not create a Table. 

### Rules
- Must start with `WITH`.
- Must have a name.
- Must contain a SELECT query.
- Can be used only in the immediately following statement.
- Can be referenced like a table within that statement.
- CTE is never stored unlike table or views.