# Definition
A Trigger is a database object that automatically executes when a specific event occurs on a table.

## General Flow
Some SQL operation happens -> Trigger detects it -> Trigger executes automatically -> Original operation completes.

## What events can trigger it ?
1. INSERT
2. UPDATE
3. DELETE


## BEFORE vs AFTER
Every event has two possible timings.
<br>
INSERT -> BEFORE INSERT -> INSERT ROW -> AFTER INSERT
<br>
UPDATE -> BEFORE UPDATE -> UPDATE ROW -> AFTER UPDATE
<br>
DELETE -> BEFORE DELETE -> DELETE ROW -> AFTER DELETE
<br>

## TRIGGER vs FUNCTION
FUNCTIONS are called.
TRIGGERS runs automatically.

> One Table can have Multiple Triggers.

## COMMON REAL-WORLD USES
1. Logging (Most common)
2. Automatic timestamp
3. Validation

## Trigger Syntax
```sql
DELIMITER //
CREATE TRIGGER before_employee_insert
BEFORE INSERT
ON employees
FOR EACH ROW
BEGIN
    SET NEW.salary = ABS(NEW.salary);
END //
DELIMITER ;
```

#### DELIMITER
Denotes end of SQL statement. 

#### CREATE TRIGGER
Create a permanent trigger inside the current database.

#### Trigger Name
before_employee_insert.

#### BEFORE/AFTER
This tells MySQL when to execute the trigger.

#### INSERT/UPDATE/DELETE
This specifies which event causes the trigger.

#### ON employees
Watches this table for event trigger.

#### FOR EACH ROW
Do something for each row.

#### BEGIN
Starts the trigger body.

#### NEW
Imaginary row contains current row. (incoming data).
NEW.salary
NEW.name
NEW.department

#### OLD
It is used with UPDATE/DELETE.
Helps to compare new and old values.
OLD.salary
OLD.name
OLD.department

#### END
Marks end of trigger.

> INSERT -> Only NEW exists.
> DELETE -> Only OLD exists.
> UPDATE -> Both NEW and OLD exists.