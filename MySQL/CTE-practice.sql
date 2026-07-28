CREATE DATABASE ctedb;
USE ctedb;

CREATE TABLE employees (
    id INT PRIMARY KEY,
    name VARCHAR(50),
    department VARCHAR(20),
    salary INT,
    city VARCHAR(30)
);

INSERT INTO employees VALUES
(1,'Alice','IT',90000,'Mumbai'),
(2,'Bob','IT',70000,'Delhi'),
(3,'Charlie','HR',60000,'Mumbai'),
(4,'David','HR',50000,'Pune'),
(5,'Eva','Sales',80000,'Delhi'),
(6,'Frank','Sales',75000,'Ahmedabad'),
(7,'Grace','IT',95000,'Pune'),
(8,'Henry','Finance',65000,'Mumbai');

-- 1. Create a CTE named ITEmployees that contains all IT employees.
WITH ITEmployees AS (
	SELECT * FROM employees WHERE department = 'IT'
)
SELECT * FROM ITEmployees;

-- Create a CTE named HighSalary. Include employees with salary greater than 75000.
WITH HighSalary AS (
	SELECT * FROM employees WHERE salary > 75000
)
SELECT * FROM HighSalary;

-- Create a CTE named MumbaiEmployees. Display only:name and salary.
WITH MumbaiEmployees AS (
	SELECT name, salary FROM employees WHERE city='Mumbai'
)
SELECT * FROM MumbaiEmployees;

-- Create a CTE containing employees from the Sales department. From that CTE, display only employees whose salary is greater than 76000.
WITH SalesEmployees AS
(
	SELECT * FROM employees WHERE department = 'Sales'
)
SELECT * FROM SalesEmployees WHERE salary > 76000;

-- Create a CTE containing employees with salary greater than 60000.
-- Using only the CTE: Count employees, Find maximum salary, Find minimum salary, Find average salary
WITH SalaryMore AS
(
	SELECT * FROM employees WHERE salary > 60000
)
SELECT COUNT(*), MIN(salary), MAX(salary), AVG(salary) FROM SalaryMore;