CREATE DATABASE cursor_demo;
USE cursor_demo;


CREATE TABLE employees (

    employee_id INT PRIMARY KEY,
    employee_name VARCHAR(100),
    department VARCHAR(50),
    salary DECIMAL(2,1)
);

    
INSERT INTO employees
(employee_id, employee_name, department, salary)
VALUES
(1, 'Rahul', 'IT', 60000),
(2, 'Amit', 'HR', 45000),
(3, 'Priya', 'IT', 75000),
(4, 'Neha', 'IT', 80000),
(5, 'Jay', 'Finance', 55000);


SELECT * FROM employees;


-- create function
DELIMITER //

CREATE FUNCTION get_total_it_salary()
RETURNS DECIMAL(10,2)
DETERMINISTIC
BEGIN

    -- variable to store current employee salary
    DECLARE v_salary DECIMAL(10,2) DEFAULT 0;
    -- variable to store final total
    DECLARE v_total DECIMAL(10,2) DEFAULT 0;
    -- flag to detect end of cursor
    DECLARE done INT DEFAULT 0;

    -- declare cursor
    DECLARE emp_cursor CURSOR FOR
        SELECT salary 
        FROM employees
        WHERE department = 'IT';

    -- handler for no more rows
    DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = 1; -- done automatically by MySQL when NOT FOUND occurs.
    -- CONTINUE :- Handle the condition and continue execution after the statement that caused the condition.

    -- OPEN cursor
    OPEN emp_cursor;

    -- start processing rows
    read_loop: LOOP

        -- Get one salary
        FETCH emp_cursor INTO v_salary;

        -- Stop when there are no more rows
        IF done = 1 THEN
            LEAVE read_loop;
        END IF;

        SET v_total = v_total + v_salary;

    END LOOP;

    -- CLOSE cursor
    CLOSE emp_cursor;

    RETURN v_total;

END //
DELIMITER ;


SELECT get_total_it_salary();

-- A MySQL cursor is local to the stored program execution where it is declared. It is not a permanent database object like a table or view.

-- MySQL automatically closes cursors when the stored program block in which they are declared ends.

-- Yes. After CLOSE, you can OPEN the same cursor again, as long as you are still inside the function/procedure where the cursor was declared.
-- when you reopen the cursor, it starts from the beginning of its result set again.

