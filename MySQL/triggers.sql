CREATE DATABASE triggerdb;
USE triggerdb;

CREATE TABLE employees (
    emp_id INT PRIMARY KEY,
    emp_name VARCHAR(50),
    salary DECIMAL(10,2),
    dept VARCHAR(30)
);

CREATE TABLE employee_audit (
    audit_id INT AUTO_INCREMENT PRIMARY KEY,
    emp_id INT,
    action_type VARCHAR(20),
    old_salary DECIMAL(10,2),
    new_salary DECIMAL(10,2),
    action_time DATETIME
);

-- Sample Data
INSERT INTO employees VALUES
(1,'Amit',50000,'IT'),
(2,'Rahul',70000,'HR'),
(3,'Priya',60000,'Sales');

-- Example 1 (BEFORE INSERT)
-- Company policy : salary should never be negative.
DELIMITER //
CREATE TRIGGER trg_before_insert_salary
BEFORE INSERT
ON employees
FOR EACH ROW
BEGIN
    IF NEW.salary < 0 THEN
        SET NEW.salary = ABS(NEW.salary);
    END IF;
END //
DELIMITER ;

INSERT INTO employees VALUES (4,'Jagdish',-10000,'IT');




-- Example 2 (AFTER INSERT)
-- Company wants : whenever an employee is added, store log.
DELIMITER //
CREATE TRIGGER trg_after_insert
AFTER INSERT
ON employees
FOR EACH ROW
BEGIN
    INSERT INTO employee_audit(emp_id,action_type,new_salary,action_time) VALUES
    (NEW.emp_id,'INSERT',NEW.salary,NOW());
END//
DELIMITER ; 

INSERT INTO employees VALUES (5,'Mihir',55000,'Finance');
SELECT * FROM employee_audit;




-- Example 3 (BEFORE UPDATE)
-- Company : Nobody can reduce salary below 30000.
DELIMITER //
CREATE TRIGGER trg_update_before_salary
BEFORE UPDATE
ON employees
FOR EACH ROW
BEGIN
    IF NEW.salary < 30000 THEN
        SET NEW.salary = 30000;
    END IF;
END //
DELIMITER ;

SELECT * FROM employees;
UPDATE employees SET salary = 10000 WHERE emp_id = 1;




-- Example 4 : AFTER UPDATE
-- Company : Whenever salary changes, store history
DELIMITER //
CREATE TRIGGER trg_after_update_salary
AFTER UPDATE
ON employees
FOR EACH ROW
BEGIN
    INSERT INTO employee_audit(emp_id,action_type,old_salary,new_salary,action_time) VALUES
    (NEW.emp_id,'SALARY UPDATE',OLD.salary,NEW.salary,NOW());
END //
DELIMITER ;

UPDATE employees SET salary=80000 WHERE emp_id=2;




-- Example 5 : BEFORE DELETE
-- Company : HR department Empployees cannot be deleted.
DELIMITER //
CREATE TRIGGER trg_before_delete_employee
BEFORE DELETE
ON employees
FOR EACH ROW
BEGIN
    IF OLD.dept = 'HR' THEN
        SIGNAL SQLSTATE '45000' -- user defined exception (executions stops)
        SET MESSAGE_TEXT = 'HR employees cannot be deleted';
    END IF;
END //
DELIMITER ;

DELETE FROM employees WHERE emp_id = 2;
SELECT * FROM employees;




-- Example 6 : AFTER DELETE
-- Company : As soon as employee is deleted, we need to log it.

DELIMITER //
CREATE TRIGGER trg_after_delete_employee
AFTER DELETE
ON employees
FOR EACH ROW
BEGIN
    INSERT INTO employee_audit(emp_id,action_type,action_time) VALUES
    (OLD.emp_id,'DELETE',NOW());
END //
DELIMITER ;

DELETE FROM employees WHERE emp_id = 3;
SELECT * FROM employees;
SELECT * FROM employee_audit;

-- DROP TRIGGER
DROP TRIGGER IF EXISTS trigger_name;

SHOW TRIGGERS;