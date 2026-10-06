-- =====================================================
-- PREPARED STATEMENT COMPLETE EXAMPLE
-- =====================================================
USE temp;
-- 1. Prepare the SQL
PREPARE stmt
FROM '
    SELECT
        employee_id,
        employee_name,
        department,
        salary,
        city
    FROM employees
    WHERE salary > ?
';

-- 2. First value
SET @salary = 50000;

-- 3. Execute
EXECUTE stmt USING @salary;


-- 4. Change the value
SET @salary = 70000;

-- 5. Execute again
EXECUTE stmt USING @salary;


-- 6. Change the value again
SET @salary = 75000;

-- 7. Execute again
EXECUTE stmt USING @salary;


-- 8. Remove prepared statement
DEALLOCATE PREPARE stmt;




-- creating prepared statement for dynamic table name
SET @table_name = 'employees';
SET @query = CONCAT('SELECT * FROM ', @table_name);
PREPARE stmt FROM @query;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;