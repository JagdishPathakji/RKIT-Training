-- COMMIT :- Everything is correct. Save permanently.
-- ROLLBACK :- Something went wrong. Undo everything.
-- Transaction Means: A complete logical unit of work.
-- A transaction groups SQL statements into one unit.
-- It ensures the database never ends up in a partially updated state.
-- All statements either succeed together or fail together.



-- -----------------------------------------------------------------------------------------------
-- (*) WHAT IS AUTOCOMMIT ?
-- By default, MySQL automatically saves every successful statement.
-- Example :- 
-- UPDATE Accounts SET balanace = 6000 WHERE AccountID = 1;
-- The moment this query succeeds, MySQL immediately saves it permanently.
-- You don't need to write `COMMIT` because MySQL does it automatically.
-- This feature is called AUTOCOMMIT.


-- Check AUTOCOMMIT 
SELECT @@autocommit; -- 1 means ON , 0 means OFF
-- TURN OFF
SET autocommit = 0;
-- TURN ON
SET autocommit = 1;


-- What is START TRANSACTION ?
-- Instead of every query being saved immediately, we will group queries together.
-- `START TRANSACTION` tells MySQL the next SQL statements belong together. Dont make them permanent until i tell you.
-- AUTOCOMMIT is temporarily suspended for that transcation, even if @@autocommit = 1;

-- AUTOCOMMIT = ON → Every statement is automatically committed unless you're inside an explicit transaction.
-- START TRANSACTION → Temporarily disables automatic commits for the statements that follow.
-- COMMIT or ROLLBACK → Ends the explicit transaction.
-- After the transaction ends, AUTOCOMMIT behavior resumes automatically (if it was ON).


-- step 1 : create the table
USE db;
CREATE TABLE Accounts (
	AccountID INT PRIMARY KEY,
    AccountHolder VARCHAR(50),
    Balance DECIMAL(10,2)
);

INSERT INTO Accounts VALUES
(101,'Amit',10000),
(102,'Neha',5000),
(103,'Rahul',8000);


-- Scenario :-
-- Amit wants to transfer ₹2,000 to Neha.
-- To complete this operation, we need to:
-- Deduct ₹2,000 from Amit
-- Add ₹2,000 to Neha
-- These two queries must succeed together.



-- CASE 1 : AUTOCOMMIT = ON(Default)
SET AUTOCOMMIT = 1;

UPDATE Accounts
SET Balance = Balance - 2000
WHERE AccountID = 101;

UPDATE Accounts
SET Balance = Balance + 2000
WHERE AccountID = 102;



-- CASE 2 : Using COMMIT
START TRANSACTION;

UPDATE Accounts SET Balance = Balance - 2000 WHERE AccountID = 101;
UPDATE Accounts SET Balance = Balance + 2000 WHERE AccountID = 102;

SELECT * FROM Accounts; -- show visibility of commands result
COMMIT;
SELECT * FROM Accounts; -- show final result as per rollback or commit




-- CASE 3 : Using ROLLBACK
START TRANSACTION;

UPDATE Accounts SET Balance = Balance - 2000 WHERE AccountID = 101;
UPDATE Accounts SET Balance = Balance + 2000 WHERE AccountID = 102;

SELECT * FROM Accounts; -- show visibility of commands result
ROLLBACK;
SELECT * FROM Accounts; -- show final result as per rollback or commit



-- SAVEPOINT, ROLLBACK TO SAVEPOINT & RELEASE SAVEPOINT
-- A SAVEPOINT is a checkpoint inside a transaction.
-- It creates a temporary point where you can return later.

-- (*) Example 
-- START TRANSACTION
-- Query 1 (saved due to checkpoint till A)
-- Query 2 (saved due to checkpoint till A)
-- SAVEPOINT A   ← checkpoint
-- Query 3
-- Query 4
-- Query 5
-- ROLLBACK TO A
-- COMMIT

-- Normal ROLLBACK, Means: Undo the entire transaction.
-- ROLLBACK TO savepoint_name, Means: Undo only changes after that savepoint.



-- (*) Example
START TRANSACTION;

UPDATE Accounts SET Balance = Balance - 2000 WHERE AccountID = 101;
SAVEPOINT after_amit_deduction;
UPDATE Accounts SET Balance = Balance + 2000 WHERE AccountID = 102;
-- Suppose 102 id's account has a problem, instead of ROLLBACK, we do ROLLBACK TO after_amit_deduction;
-- So, only 102's update is cancelled.
ROLLBACK TO after_amit_deduction;
COMMIT; -- will save first update and undo second one.

-- ROLLBACK TO SAVEPOINT does not end the transaction, we have to COMMIT to complete the transaction.
-- SAVEPOINT only works inside transactions.
-- COMMIT destroys all savepoints.
-- ROLLBACK destroys all savepoints.
-- Creating same savepoint name replaces old one.