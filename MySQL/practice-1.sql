CREATE DATABASE dbname;

-- USE DATABASE dbname; 

CREATE TABLE dbname.tbname (
	id INT PRIMARY KEY auto_increment,
    name VARCHAR(45) NOT NULL,
    email VARCHAR(45) NOT NULL,
    age INT NOT NULL
);

DESCRIBE dbname.tbname;

-- DROP TABLE dbname.tbname;

ALTER TABLE dbname.tbname ADD COLUMN department VARCHAR(30);

DESCRIBE dbname.tbname;

ALTER TABLE dbname.tbname DROP COLUMN department;

SELECT * FROM dbname.tbname LIMIT 10;

INSERT INTO dbname.tbname (name, email,age) VALUES ('jagdish','pathakjijagdish1@gmail.com',21);
INSERT INTO dbname.tbname (name,email,age) VALUES ('mihir','pathakjimihir1@gmail.com',19),('yug
','dholakiyayug@gmail.com',21);

SELECT * FROM dbname.tbname;

UPDATE dbname.tbname SET email='pathakjijagdish111@gmail.com' WHERE id = 1;

DELETE FROM dbname.tbname WHERE id = 1;

SELECT * FROM dbname.tbname WHERE email LIKE '%@gmail.com%' ORDER BY age LIMIT 10;

-- Aggregate functions cannot be used directly with the WHERE clause.
-- Aggregate functions can be used with HAVING and ORDER BY clause.

-- Creating table to practice Aggr functions, GROUP BY and HAVING.
CREATE TABLE dbname.employees (
    empid INT PRIMARY KEY,
    empname VARCHAR(50),
    department VARCHAR(30),
    city VARCHAR(30),
    salary DECIMAL(10,2),
    experience INT,
    gender CHAR(1)
);

INSERT INTO dbname.employees VALUES
(101, 'Amit',    'IT',      'Ahmedabad', 60000, 2, 'M'),
(102, 'Priya',   'HR',      'Surat',     45000, 4, 'F'),
(103, 'Rahul',   'IT',      'Ahmedabad', 75000, 5, 'M'),
(104, 'Sneha',   'Finance', 'Vadodara',  55000, 3, 'F'),
(105, 'Karan',   'IT',      'Rajkot',    90000, 8, 'M'),
(106, 'Neha',    'HR',      'Surat',     50000, 2, 'F'),
(107, 'Arjun',   'Finance', 'Ahmedabad', 70000, 6, 'M'),
(108, 'Riya',    'IT',      'Surat',     65000, 4, 'F'),
(109, 'Vikas',   'Sales',   'Rajkot',    40000, 1, 'M'),
(110, 'Pooja',   'Sales',   'Ahmedabad', 48000, 3, 'F'),
(111, 'Manish',  'HR',      'Vadodara',  62000, 7, 'M'),
(112, 'Anjali',  'Finance', 'Surat',     80000, 9, 'F');


SELECT * FROM dbname.employees;
USE dbname;

-- Practicing questions
SELECT COUNT(*) FROM employees;
SELECT department,COUNT(*) FROM employees GROUP BY department;
SELECT city,COUNT(*) FROM employees GROUP BY city;
SELECT department,COUNT(*) FROM employees WHERE salary > 50000 GROUP BY department;

SELECT SUM(salary) FROM employees;
SELECT department,SUM(salary) FROM employees GROUP BY department;
SELECT city,SUM(salary) FROM employees GROUP BY city;

SELECT department,AVG(salary) FROM employees GROUP BY department; 
SELECT city,AVG(experience) FROM employees GROUP BY city;
SELECT department,AVG(salary) FROM employees GROUP BY department HAVING AVG(salary) > 65000;

SELECT MAX(salary) FROM employees;
SELECT MIN(salary) FROM employees;

DESCRIBE employees;

SELECT department FROM employees GROUP BY DEPARTMENT HAVING COUNT(*) > 2;
SELECT city FROM employees GROUP BY city HAVING COUNT(*) >= 3;

SELECT * FROM employees WHERE CITY IN (SELECT city FROM employees WHERE gender = 'F' GROUP BY city);
SELECT * FROM employees;

-- department with highest average salary
SELECT department, AVG(salary) as avgsalary FROM employees GROUP BY department;
SELECT department, AVG(salary) as avgsalary FROM employees GROUP BY department ORDER BY avgsalary DESC LIMIT 1;

-- IFNULL() does not change the data in the table. It only changes the value in the query result (your view/output).
ALTER TABLE employees ADD COLUMN bonus INT;
SELECT * FROM employees;
SELECT empname, IFNULL(bonus,0) AS bonus FROM employees;