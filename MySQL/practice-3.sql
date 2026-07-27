-- Creating tables for practicing queries
CREATE DATABASE DB;
USE DB;

CREATE TABLE employees (
    emp_id INT PRIMARY KEY,
    name VARCHAR(30),
    department_id INT,
    salary INT,
    manager_id INT,
    city VARCHAR(20)
);

INSERT INTO employees VALUES
(101,'Alice',1,90000,NULL,'Ahmedabad'),
(102,'Bob',1,75000,101,'Ahmedabad'),
(103,'Charlie',2,60000,104,'Surat'),
(104,'David',2,95000,NULL,'Vadodara'),
(105,'Eva',3,50000,106,'Rajkot'),
(106,'Frank',3,85000,NULL,'Rajkot'),
(107,'Grace',1,70000,101,'Surat'),
(108,'Henry',2,55000,104,'Ahmedabad'),
(109,'Ivy',3,45000,106,'Vadodara'),
(110,'Jack',1,80000,101,'Rajkot');

CREATE TABLE departments (
    department_id INT PRIMARY KEY,
    department_name VARCHAR(30)
);

INSERT INTO departments VALUES
(1,'Engineering'),
(2,'HR'),
(3,'Finance'),
(4,'Marketing');

CREATE TABLE projects (
    project_id INT PRIMARY KEY,
    project_name VARCHAR(40),
    department_id INT,
    budget INT
);

INSERT INTO projects VALUES
(1,'Payroll System',3,500000),
(2,'Recruitment Portal',2,300000),
(3,'AI Chatbot',1,900000),
(4,'Website Redesign',4,200000),
(5,'Cloud Migration',1,700000);

-- Why do subqueries even exist? Can't JOIN solve everything?
-- A JOIN answers: "How do I combine information from multiple tables?"
-- A Subquery answers: "I need the answer to one question before I can answer another."

-- Types of Subqueries
-- 1. Single row
-- 2. Multiple row
-- 3. Correlated


-- -------------------------------------------------------------------------------------------------
-- Topic 1 :- Single Row Subquery
-- A single-row subquery returns exactly one value.
-- Example : SELECT AVG(salary) FROM employees; always returns one-value.
-- Operators Used with Single-Row Subqueries are : =, !=, <, >, <=, >=

-- ex 1 : Find employees earning more than the average salary.
SELECT * FROM employees WHERE salary > (SELECT AVG(salary) FROM employees);
-- ex 2 : Find employees earning the maximum salary.
SELECT * FROM employees WHERE salary = (SELECT MAX(salary) FROM employees);
-- ex 3 : Find employees earning the minimum salary.
SELECT * FROM employees WHERE salary = (SELECT MIN(salary) FROM employees);
-- ex 4 : Find employees earning below the average salary.
SELECT * FROM employees WHERE salary < (SELECT AVG(salary) FROM employees);
-- ex 5 : Find employees earning more than the minimum salary.
SELECT * FROM employees WHERE salary > (SELECT MIN(salary) FROM employees);
-- ex 6 : Find employees earning less than the maximum salary.
SELECT * FROM employees WHERE salary < (SELECT MAX(salary) FROM employees);
-- ex 7 : Find employees whose salary equals the average salary (if any).
SELECT * FROM employees WHERE salary = (SELECT AVG(salary) FROM employees);
-- ex 8 : Find employees earning more than the average salary of all employees.
SELECT * FROM employees WHERE salary > (SELECT AVG(salary) FROM employees);
-- ex 9 : Find employees earning at least the average salary.
SELECT * FROM employees WHERE salary >= (SELECT AVG(salary) FROM employees);
-- ex 10 : Find employees earning at most the average salary.
SELECT * FROM employees WHERE salary <= (SELECT AVG(salary) FROM employees);
-- ex 11 : Find employees earning exactly the highest salary.
SELECT * FROM employees WHERE salary = (SELECT MAX(salary) FROM employees);

-- -------------------------------------------------------------------------------------------------
-- Topic 2 :- Multiple Row Subquery
-- A multiple-row subquery returns multiple value.
-- Example : SELECT dept_id FROM projects;
-- Operators Used with Multiple-Row Subqueries are : IN , NOT IN, ANY, ALL.

-- ex 1 : Find employees who work in departments that have projects.
SELECT name FROM employees WHERE department_id IN (SELECT department_id FROM projects);
-- ex 2 : Find employees who work in Engineering or Finance.
SELECT name FROM employees WHERE department_id IN (SELECT department_id FROM departments WHERE department_name IN ('Engineering','Finance'));
-- ex 3 : Find departments that currently have no projects.
SELECT department_name FROM departments WHERE department_id NOT IN (SELECT department_id FROM projects);
-- ex 4 : Find employees who do not work in Marketing.
SELECT name FROM employees WHERE department_id NOT IN (SELECT department_id FROM departments WHERE department_name = 'Marketing');
-- ex 5 : Find all employees whose department appears in the projects table.
SELECT * FROM employees WHERE department_id IN (SELECT department_id FROM projects);
-- ex 6 : Find departments that are used by employees but have no projects.
SELECT department_name
	FROM departments
WHERE department_id 
	IN
(
    SELECT department_id
    FROM employees
)
	AND 
department_id NOT IN
(
    SELECT department_id
    FROM projects
);
-- ex 7 : Find employees who belong to departments whose project budget is greater than ₹500,000.
SELECT name FROM employees WHERE department_id IN (SELECT department_id FROM projects WHERE budget > 500000);
-- ex 8 : Find departments that have projects with a budget less than ₹400,000.
SELECT department_name FROM departments WHERE department_id IN (SELECT department_id FROM projects WHERE budget < 400000);
-- ex 9 : Find employees whose department is not present in the projects table.
SELECT name FROM employees WHERE department_id NOT IN (SELECT department_id FROM projects);
-- ex 10 : Find department names where at least one employee works.
SELECT department_name FROM departments WHERE department_id IN (SELECT department_id FROM employees GROUP BY department_id HAVING COUNT(*) > 0);
-- ex 11 : Find employees earning more than any HR employee.
SELECT name FROM employees WHERE salary > ANY (SELECT salary FROM employees WHERE department_id = (SELECT department_id FROM departments WHERE department_name='HR'));
-- ex 12 : Find employees earning less than any Engineering employee.
SELECT name FROM employees WHERE salary < ANY (SELECT salary FROM employees WHERE department_id = (SELECT department_id FROM departments WHERE department_name='Engineering'));
-- ex 13 : Find projects with a budget greater than any Finance project.
SELECT project_name FROM projects WHERE budget > ANY (SELECT budget FROM projects WHERE department_id = (SELECT department_id FROM departments WHERE department_name = 'Finance'));
-- ex 14 : Find employees earning more than all HR employees.
SELECT name FROM employees WHERE salary > ALL (SELECT salary FROM employees WHERE department_id = (SELECT department_id FROM departments WHERE department_name='HR'));
-- ex 15 : Find projects with a budget higher than all Marketing projects.
SELECT project_name FROM projects WHERE budget > ALL (SELECT budget FROM projects WHERE department_id = (SELECT department_id FROM departments WHERE department_name = 'Marketing'));
-- ex 16 : Find employees earning less than all Engineering employees
SELECT name FROM employees WHERE salary < ALL (SELECT salary FROM employees WHERE department_id = (SELECT department_id FROM departments WHERE department_name = 'Engineering'));

-- -------------------------------------------------------------------------------------------------
-- Topic 3 :- Using EXISTS and NOT EXISTS
-- Unlike IN, ANY, and ALL, these do not compare values. Instead, they answer a yes/no question.
-- EXISTS : Does the subquery return at least one row?
-- NOT EXISTS : Does the subquery return no rows?

-- ex 1 : Find departments that have at least one project.
SELECT department_name FROM departments d WHERE EXISTS (
	SELECT 1 FROM projects p WHERE
		p.department_id = d.department_id
);

-- ex 2 : Find departments that have no projects.
SELECT department_name FROM departments d WHERE NOT EXISTS (
	SELECT 1 FROM projects p WHERE
		p.department_id = d.department_id
);


-- ex 3 : Find employees whose department has at least one project.
SELECT name FROM employees e WHERE EXISTS (
	SELECT 1 FROM projects p WHERE 
		e.department_id = p.department_id
);

-- ex 4 : Find employees whose department has no projects.
SELECT name FROM employees e WHERE NOT EXISTS (
	SELECT 1 FROM projects p WHERE
		e.department_id = p.department_id
);

-- ex 5 : Find departments that have at least one employee.
SELECT department_name FROM departments d WHERE EXISTS
(
    SELECT 1 FROM employees e WHERE e.department_id = d.department_id
);

-- ex 6 : Find departments with no employees.
SELECT department_name FROM departments d WHERE NOT EXISTS
(
    SELECT 1 FROM employees e WHERE e.department_id = d.department_id
);

-- -------------------------------------------------------------------------------------------------
-- Topic 4 :- Correlated Subqueries
-- A correlated subquery is dependent on the outer query. 
-- So the inner query needs information from the outer query.
-- Conceptually, the inner query is evaluated for each row of the outer query.

-- 1. Find employees earning above their department average.
SELECT name FROM employees e1 WHERE salary > (SELECT AVG(salary) FROM employees e2 WHERE e2.department_id = e1.department_id);

-- 2. Find employees earning below their department average.
SELECT name FROM employees e1 WHERE salary < (SELECT AVG(salary) FROM employees e2 WHERE e2.department_id = e1.department_id);

-- 3. Find employees earning the highest salary in their department.
SELECT name FROM employees e1 WHERE salary = (SELECT MAX(salary) FROM employees e2 WHERE e2.department_id = e1.department_id);

-- 4. Find employees earning the lowest salary in their department.
SELECT name FROM employees e1 WHERE salary = (SELECT MIN(salary) FROM employees e2 WHERE e2.department_id = e1.department_id);

-- 5. Find employees whose salary is equal to the average salary of their department.
SELECT name FROM employees e1 WHERE salary = (SELECT AVG(salary) FROM employees e2 WHERE e2.department_id = e1.department_id);

-- 6. Find employees earning at least the average salary of their department.
SELECT name FROM employees e1 WHERE salary >= (SELECT AVG(salary) FROM employees e2 WHERE e2.department_id = e1.department_id);

-- 7. Find employees earning at most the average salary of their department.
SELECT name FROM employees e1 WHERE salary <= (SELECT AVG(salary) FROM employees e2 WHERE e2.department_id = e1.department_id);

-- 8. Find employees whose salary is not the maximum salary in their department.
SELECT name FROM employees e1 WHERE salary != (SELECT MAX(salary) FROM employees e2 WHERE e2.department_id = e1.department_id);

-- 9. Find the second highest salary in each department.
SELECT e1.name, e1.salary,e1.department_id FROM employees e1 WHERE salary = (
	SELECT MAX(e2.salary) FROM employees e2 WHERE
		e1.department_id = e2.department_id
			AND
        e2.salary < (SELECT MAX(e3.salary) FROM employees e3
				WHERE e2.department_id = e3.department_id
		)
);

-- After GROUP BY, every column in the SELECT list must be either:
-- - In the GROUP BY clause, or
-- - Inside an aggregate function (AVG, COUNT, MAX, MIN, SUM, etc.).
-- However, the GROUP BY column itself does not have to appear in the SELECT list. It can be used purely to define the groups, as in your AVG(salary) example.

-- 10. Find departments with the highest average salary.
SELECT MAX(average) as "dept-wise-average" FROM (SELECT AVG(salary) as "average" FROM employees GROUP BY department_id) as t;

-- 11. Find employees whose salary is above the company average but below their department maximum.
SELECT name FROM employees e1 WHERE salary > (SELECT AVG(salary) FROM employees) AND salary < (SELECT MAX(salary) FROM employees e2 WHERE e1.department_id = e2.department_id);

-- 12. Find employees who have at least one colleague earning more than them.
SELECT name FROM employees e1 WHERE salary < ANY (SELECT salary FROM employees e2 WHERE e1.department_id = e2.department_id);

-- 13. Find employees who are the only employee in their department.
SELECT name FROM employees e1 WHERE NOT EXISTS (SELECT 1 FROM employees e2 WHERE e1.department_id = e2.department_id AND e1.emp_id != e2.emp_id);