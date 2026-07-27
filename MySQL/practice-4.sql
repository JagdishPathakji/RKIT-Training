-- RULES FOR SET OPERATIONS
-- 1. Both queries must return the same number of columns.
-- 2. Corresponding columns should have compatible data type.
-- 3. Column names in the final result come from the first query.

CREATE DATABASE students;
USE students;

CREATE TABLE students_a (
	id INT PRIMARY KEY,
    name VARCHAR(30)
);

CREATE TABLE students_b (
	id INT PRIMARY KEY,
    name VARCHAR(30)
);

INSERT INTO students_a VALUES (1,'Jagdish');
INSERT INTO students_a VALUES (2,'Yug');
INSERT INTO students_a VALUES (3,'Mihir');


INSERT INTO students_b VALUES (2,'Yug');
INSERT INTO students_b VALUES (3,'Mihir');
INSERT INTO students_b VALUES (4,'Rudra');


-- UNION :- Returns all UNIQUE rows from both queries
SELECT * FROM students_a 
UNION
SELECT * FROM students_b;

-- UNION ALL :- Returns all rows including DUPLICATE rows
SELECT * FROM students_a
UNION ALL
SELECT *  FROM students_b;

-- INTERSECT :- Returns rows present in BOTH query output (supported in version 8.0.31+)
SELECT * FROM students_a
INTERSECT
SELECT * FROM students_b;

-- EXCEPT :- Returns rows in first query but not in second (supported in version 8.0.31+)
SELECT * FROM students_a
EXCEPT
SELECT * FROM students_b;