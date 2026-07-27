CREATE DATABASE joins;
USE joins;

CREATE TABLE departments (
    deptid INT PRIMARY KEY,
    deptname VARCHAR(30),
    location VARCHAR(30)
);

INSERT INTO departments VALUES
(101, 'HR', 'Mumbai'),
(102, 'IT', 'Bangalore'),
(103, 'Finance', 'Delhi'),
(104, 'Marketing', 'Pune'),
(105, 'Sales', 'Ahmedabad');

CREATE TABLE employees (
    emp_id INT PRIMARY KEY,
    emp_name VARCHAR(50),
    salary DECIMAL(10,2),
    department_id INT
);

INSERT INTO employees VALUES
(1, 'Amit', 65000, 102),
(2, 'Priya', 55000, 101),
(3, 'Rahul', 70000, 102),
(4, 'Sneha', 60000, 103),
(5, 'Karan', 50000, NULL),
(6, 'Neha', 62000, 104),
(7, 'Rohan', 58000, 106);

-- Return only matching result
SELECT * FROM employees AS e INNER JOIN departments AS d on e.department_id = d.deptid;

-- Return all rows from left table
SELECT * FROM employees AS e LEFT JOIN departments AS d on e.department_id = d.deptid;

-- Return all rows from right table
SELECT * FROM employees AS e RIGHT JOIN departments AS d on e.department_id = d.deptid;

-- Return all rows from both table
SELECT * FROM employees AS e LEFT JOIN departments AS d on e.department_id = d.deptid
UNION
SELECT * FROM employees AS e RIGHT JOIN departments AS d on e.department_id = d.deptid;



-- Adding 3rd table for JOIN
CREATE TABLE projects (
    project_id INT PRIMARY KEY,
    project_name VARCHAR(50),
    department_id INT,
    budget INT
);

INSERT INTO projects VALUES
(201, 'Payroll System', 101, 500000),
(202, 'E-Commerce Website', 102, 1500000),
(203, 'Mobile App', 102, 1200000),
(204, 'Tax Management', 103, 800000),
(205, 'Digital Marketing', 104, 600000),
(206, 'CRM System', 106, 900000);

-- Query : Show employee, department and project
SELECT * FROM 
	employees AS e 
		INNER JOIN 
	departments AS d 
		on 
	e.department_id = d.deptid 
		INNER JOIN 
	projects AS p 
		on 
	d.deptid = p.department_id;
    

-- Show departments having no projects
SELECT * FROM
	departments AS d
		LEFT JOIN
	projects AS p
        ON
	d.deptid = p.department_id
		WHERE
	p.project_id IS NULL;

USE joins;
SELECT * FROM employees;    
SELECT * FROM departments;

-- JOINS with GROUP BY

-- 1. Number of employees in each department
SELECT deptname, COUNT(*) AS Members FROM departments AS d LEFT JOIN employees AS e on d.deptid = e.department_id GROUP BY deptname;
-- 2. Average salary of each department
SELECT deptname, AVG(salary) as "AVERAGE SALARY" FROM employees AS e INNER JOIN departments AS d ON d.deptid = e.department_id GROUP BY deptname;

-- JOINS with GROUP BY and HAVING

-- 3. Department having less than 3 employees
SELECT deptname, COUNT(*) AS Members FROM departments AS d LEFT JOIN employees AS e on d.deptid = e.department_id GROUP BY deptname HAVING COUNT(*) <= 2;