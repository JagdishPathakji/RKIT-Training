USE db;

SELECT * FROM employees;
CREATE VIEW EmployeeInfo AS SELECT * FROM employees;
DROP VIEW EmployeeInfo;
SELECT * FROM EmployeeInfo;
SELECT name FROM EmployeeInfo;


-- Updatable View
UPDATE EmployeeInfo SET city = 'Anand' WHERE emp_id = 101;

-- CREATE OR REPLACE
CREATE OR REPLACE VIEW EmployeeInfo AS SELECT emp_id,name,salary FROM employees;
SELECT * FROM EmployeeInfo;
SHOW CREATE VIEW EmployeeInfo;



-- Why use WITH CHECK OPTION?
-- To ensure that inserted or updated rows continue to satisfy the view's WHERE condition.
CREATE VIEW ExpensiveProducts AS
SELECT *
FROM Products
WHERE Price > 5000 -- inserts or updates only if this condition is satisfied.
WITH CHECK OPTION;