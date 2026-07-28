# Definition
A reusable block of of SQL code that accepts input, performs some work, and returns exactly `one value`.

<br>
<br>

### Where are functions stored ?
The functions are stored inside database itself.

<br>
<br>

### Characteristics of User Defined Function
- Permanent database object
- Has a name
- Accepts parameters
- Returns exactly one value
- Can be reused
- Can be called inside SELECT
- Can call built-in functions
- Can contain IF, CASE
- Can query tables (with some restrictions)

<br>
<br>

### Basic function template syntax
```sql
DELIMITER //
CREATE FUNCTION function_name(parameter datatype)
RETURNS datatype
DETERMINISTIC
BEGIN
    RETURN expression;
END //
DELIMITER ;
```

<br>
<br>

#### Understanding each keyword
1. DELIMITER :- Used to ignore ';' as end of SQL statement.
2. CREATE FUNCTION :- Create a new permanent database object.
3. function_name :- Name of the function.
4. Parameters :- (salary DECIMAL(10,2)).
5. RETURNS datatype :- This function will return what datatype value.
6. DETERMINISTIC :- Means that for same input always produce the same output. (NOW(),CURDATE(),CURTIME(),RAND() are non-deterministic function).
7. BEGIN :- Start of function body.
8. RETURN expression :- Value to return after some calculation.
8. END :- Marks end of the function body.