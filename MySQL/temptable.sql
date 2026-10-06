-- =========================================================
-- MYSQL TEMPORARY TABLE - COMPLETE EXAMPLE
-- =========================================================
CREATE DATABASE temp;
USE temp;
-- 1. Create a REAL / PERMANENT table
DROP TABLE IF EXISTS employees;

CREATE TABLE employees (
    employee_id INT PRIMARY KEY,
    employee_name VARCHAR(100),
    department VARCHAR(50),
    salary DECIMAL(10,2),
    city VARCHAR(50)
);


-- 2. Insert data into the REAL table
INSERT INTO employees
(employee_id, employee_name, department, salary, city)
VALUES
(1, 'Rahul', 'IT', 60000, 'Ahmedabad'),
(2, 'Amit', 'HR', 45000, 'Vadodara'),
(3, 'Priya', 'IT', 75000, 'Ahmedabad'),
(4, 'Neha', 'Finance', 55000, 'Surat'),
(5, 'Karan', 'IT', 80000, 'Rajkot'),
(6, 'Riya', 'HR', 50000, 'Ahmedabad'),
(7, 'Jay', 'Finance', 65000, 'Vadodara'),
(8, 'Pooja', 'IT', 70000, 'Surat');


-- =========================================================
-- METHOD 1: NORMAL CREATE TEMPORARY TABLE + INSERT
-- =========================================================

DROP TEMPORARY TABLE IF EXISTS temp_employees;

-- Create the temporary table manually
CREATE TEMPORARY TABLE temp_employees (
    employee_id INT,
    employee_name VARCHAR(100),
    department VARCHAR(50),
    salary DECIMAL(10,2),
    city VARCHAR(50)
);

-- Insert data manually
INSERT INTO temp_employees
VALUES
(101, 'Temporary Employee 1', 'IT', 50000, 'Ahmedabad');

-- Insert data from the REAL table
INSERT INTO temp_employees
SELECT
    employee_id,
    employee_name,
    department,
    salary,
    city
FROM employees
WHERE department = 'IT';

-- View temporary table
SELECT * FROM temp_employees;


-- =========================================================
-- OPERATIONS WE CAN PERFORM ON TEMPORARY TABLE
-- =========================================================

-- SELECT
SELECT *
FROM temp_employees;

-- WHERE
SELECT *
FROM temp_employees
WHERE salary > 70000;

-- ORDER BY
SELECT *
FROM temp_employees
ORDER BY salary DESC;

-- GROUP BY
SELECT
    department,
    COUNT(*) AS employee_count,
    AVG(salary) AS average_salary
FROM temp_employees
GROUP BY department;

-- UPDATE
UPDATE temp_employees
SET salary = salary + 5000
WHERE employee_id = 101;

-- INSERT
INSERT INTO temp_employees
VALUES
(102, 'Temporary Employee 2', 'HR', 45000, 'Rajkot');

-- DELETE
DELETE FROM temp_employees
WHERE employee_id = 102;

-- ALTER TABLE
ALTER TABLE temp_employees
ADD COLUMN annual_salary DECIMAL(12,2);

-- Update newly added column
UPDATE temp_employees
SET annual_salary = salary * 12;

-- Create INDEX
CREATE INDEX idx_temp_salary
ON temp_employees(salary);

-- JOIN temporary table with REAL table
SELECT
    t.employee_name,
    t.salary,
    e.city
FROM temp_employees t
JOIN employees e
ON t.employee_id = e.employee_id;


-- =========================================================
-- DROP METHOD 1 TABLE
-- =========================================================

DROP TEMPORARY TABLE IF EXISTS temp_employees;


-- =========================================================
-- METHOD 2: CREATE TEMPORARY TABLE AS SELECT
-- =========================================================

DROP TEMPORARY TABLE IF EXISTS temp_high_salary;

-- Create temporary table directly from SELECT
CREATE TEMPORARY TABLE temp_high_salary AS
SELECT
    employee_id,
    employee_name,
    department,
    salary,
    city
FROM employees
WHERE salary >= 70000;

-- View result
SELECT *
FROM temp_high_salary;


-- =========================================================
-- USE THE TEMPORARY TABLE IN MULTIPLE QUERIES
-- =========================================================

-- Count employees
SELECT COUNT(*) AS total_employees
FROM temp_high_salary;

-- Average salary
SELECT AVG(salary) AS average_salary
FROM temp_high_salary;

-- Maximum salary
SELECT MAX(salary) AS maximum_salary
FROM temp_high_salary;

-- Filter
SELECT *
FROM temp_high_salary
WHERE city = 'Ahmedabad';

-- Group
SELECT
    department,
    COUNT(*) AS employee_count,
    AVG(salary) AS average_salary
FROM temp_high_salary
GROUP BY department;


-- =========================================================
-- TEMPORARY TABLE WITH CALCULATED COLUMN
-- =========================================================

DROP TEMPORARY TABLE IF EXISTS temp_salary;

CREATE TEMPORARY TABLE temp_salary AS
SELECT
    employee_id,
    employee_name,
    salary,
    salary * 12 AS annual_salary
FROM employees;

SELECT *
FROM temp_salary;


-- =========================================================
-- FINAL CLEANUP
-- =========================================================

DROP TEMPORARY TABLE IF EXISTS temp_high_salary;
DROP TEMPORARY TABLE IF EXISTS temp_salary;

-- NOTE:
-- Temporary tables are automatically removed when
-- the current database connection/session ends.