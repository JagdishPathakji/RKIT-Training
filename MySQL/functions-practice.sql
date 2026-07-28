CREATE DATABASE companydb;
USE companydb;
CREATE TABLE departments (
    dept_id INT PRIMARY KEY,
    dept_name VARCHAR(50),
    location VARCHAR(50)
);

INSERT INTO departments VALUES
(1, 'HR', 'Ahmedabad'),
(2, 'IT', 'Vadodara'),
(3, 'Finance', 'Surat'),
(4, 'Sales', 'Rajkot');

CREATE TABLE employees (
    emp_id INT PRIMARY KEY,
    first_name VARCHAR(30),
    last_name VARCHAR(30),
    gender CHAR(1),
    salary DECIMAL(10,2),
    hire_date DATE,
    email VARCHAR(100),
    city VARCHAR(50),
    dept_id INT,
    manager_id INT,
    FOREIGN KEY (dept_id) REFERENCES departments(dept_id)
);

INSERT INTO employees VALUES
(101,'Amit','Shah','M',65000,'2020-01-10','amit@gmail.com','Ahmedabad',2,NULL),
(102,'Priya','Patel','F',55000,'2021-03-12','priya@gmail.com','Vadodara',1,101),
(103,'Rahul','Mehta','M',72000,'2019-07-15','rahul@gmail.com','Surat',2,101),
(104,'Sneha','Joshi','F',48000,'2022-05-18','sneha@gmail.com','Rajkot',3,103),
(105,'Karan','Desai','M',85000,'2018-11-21','karan@gmail.com','Ahmedabad',2,NULL),
(106,'Neha','Patel','F',62000,'2020-08-30','neha@gmail.com','Vadodara',4,105),
(107,'Rohit','Singh','M',51000,'2023-02-01','rohit@gmail.com','Rajkot',4,105),
(108,'Anjali','Verma','F',76000,'2017-09-10','anjali@gmail.com','Surat',3,NULL);

CREATE TABLE projects (
    project_id INT PRIMARY KEY,
    project_name VARCHAR(100),
    budget DECIMAL(12,2),
    start_date DATE,
    end_date DATE,
    dept_id INT,
    FOREIGN KEY (dept_id) REFERENCES departments(dept_id)
);

INSERT INTO projects VALUES
(1,'ERP System',2500000,'2024-01-01','2025-01-30',2),
(2,'AI Chatbot',1800000,'2024-02-10','2024-12-20',2),
(3,'Payroll System',900000,'2024-03-15','2024-11-30',1),
(4,'Finance Dashboard',1200000,'2024-04-01','2025-02-15',3),
(5,'Sales Portal',1500000,'2024-05-10','2025-03-31',4);


-- (1) Built-in functions in SQL.

-- 1. LENGTH()
SELECT LENGTH('Jagdish');
SELECT first_name, LENGTH(first_name) FROM employees;
SELECT * FROM employees WHERE LENGTH(first_name) = 5;
SELECT * FROM employees ORDER BY LENGTH(first_name);

-- 2. UPPER()
SELECT UPPER(first_name) FROM employees;
SELECT UPPER(CONCAT(first_name,' ',last_name)) AS "FULL NAME" FROM employees;

-- 3. LOWER()
SELECT first_name, LOWER(first_name) FROM employees;

-- 4. CONCAT()
SELECT CONCAT(first_name,' ',last_name) FROM employees;

-- 5. LEFT()
SELECT LEFT(first_name,3) FROM employees;

-- 6. RIGHT()
SELECT RIGHT(first_name,3) FROM employees;

-- 7. SUBSTRING()
SELECT SUBSTRING(first_name,1,LENGTH(first_name)) FROM employees;

-- 8. REPLACE()
SELECT REPLACE(email,'gmail.com','yahoo.com') FROM employees;

-- 9. REVERSE()
SELECT REVERSE(email) FROM employees;

-- 10. ROUND()
SELECT ROUND(123.4567,2);

-- 11. CEIL()
SELECT CEIL(12.1);

-- 12. FLOOR()
SELECT FLOOR(13.8);

-- 13. ABS()
SELECT ABS(-100);

-- 14. MOD()
SELECT MOD(10,3);

-- 15. POWER()
SELECT POWER(10,2);

-- 16. SQRT()
SELECT SQRT(9);

-- 17. TRUNCATE()
SELECT TRUNCATE(16.89791,2);

-- 18. NOW() (returns current date and time)
SELECT NOW();

-- 19. CURDATE() (returns only todays date)
SELECT CURDATE();

-- 20. CURTIME() (returns current time)
SELECT CURTIME();

-- 21. YEAR()
SELECT YEAR(NOW());

-- 22. MONTH()
SELECT MONTH(NOW());

-- 23. DAY()
SELECT DAY(NOW());

-- 24. DATEDIFF()
SELECT DATEDIFF('2005-12-05','2005-12-09');

-- NULL Functions
SELECT IFNULL(NULL,10000); -- REPLACE NULL WITH VALUE 
SELECT COALESCE(NULL,NULL,100,NULL); -- RETURNS FIRST NOT NULL VALUE
SELECT NULLIF(10,10); -- RETURN NULL IF TWO VALUES ARE EQUAL



-- (1) User defined functions in SQL.
-- CREATE FUNCTION function_name(parameter datatype) 
-- RETURNS datatype
-- DETERMINISTIC
-- BEGIN
-- 	RETURN expression;
-- END;

-- Example 1
DELIMITER //
CREATE FUNCTION AnnualSalary(salary DECIMAL(10,2))
RETURNS DECIMAL(10,2)
DETERMINISTIC
BEGIN
	RETURN salary * 12;
END //
DELIMITER ;

SELECT AnnualSalary(10000);
SELECT AnnualSalary(1000);


-- Example 2
DELIMITER //
CREATE FUNCTION Area(length INT, width INT) 
RETURNS INT
DETERMINISTIC
BEGIN
	RETURN length * width;
END //
DELIMITER ;

SELECT Area(10,10) as "Area";



-- Example 3
DELIMITER //
CREATE FUNCTION AnnualSalaryWithBonus(salary DECIMAL(10,2))
RETURNS DECIMAL(10,2)
DETERMINISTIC
BEGIN
	DECLARE annual_salary DECIMAL(10,2);
    DECLARE bonus DECIMAL(10,2) DEFAULT 10000;
    SET annual_salary = salary * 12;
    SET bonus = annual_salary * 0.10;
    RETURN annual_salary + bonus;
END //
DELIMITER ;

SELECT AnnualSalaryWithBonus(10000);



-- Example 4
DELIMITER //
CREATE FUNCTION CalculateAnnualSalary(monthly_salary DECIMAL(10,2))
RETURNS DECIMAL(10,2)
DETERMINISTIC

BEGIN
    -- Variables
    DECLARE annual_salary DECIMAL(10,2);
    DECLARE bonus DECIMAL(10,2) DEFAULT 0;
    DECLARE final_salary DECIMAL(10,2);

    -- Calculate annual salary
    SET annual_salary = monthly_salary * 12;

    -- Decide bonus
    IF monthly_salary < 50000 THEN
        SET bonus = annual_salary * 0.20;
    ELSEIF monthly_salary <= 100000 THEN
        SET bonus = annual_salary * 0.10;
    ELSE
        SET bonus = annual_salary * 0.05;
    END IF;

    -- Final salary
    SET final_salary = annual_salary + bonus;
    
    RETURN final_salary;

END //
DELIMITER ;

SELECT CalculateAnnualSalary(100000);
SELECT CalculateAnnualSalary(45000);



-- DROP a function
DROP FUNCTION IF EXISTS function_name;

SHOW FUNCTION STATUS;