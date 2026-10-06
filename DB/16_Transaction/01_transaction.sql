-- COMMIT :- Everything is correct. Save permanently.
-- ROLLBACK :- Something went wrong. Undo everything.
-- Transaction Means: A complete logical unit of work.
-- A transaction groups SQL statements into one unit.
-- It ensures the database never ends up in a partially updated state.
-- All statements either succeed together or fail together.



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



-- Normal ROLLBACK, Means: Undo the entire transaction.
-- ROLLBACK TO savepoint_name, Means: Undo only changes after that savepoint.



-- ROLLBACK TO SAVEPOINT does not end the transaction, we have to COMMIT to complete the transaction.
-- SAVEPOINT only works inside transactions.
-- COMMIT destroys all savepoints.
-- ROLLBACK destroys all savepoints.
-- Creating same savepoint name replaces old one.


-- Query:- The "Approve & Publish" Action (COMMIT / ROLLBACK)
-- Scenario: An editor clicks "Approve and Publish" on an article version. This requires 3 distinct database operations.
-- 1. INSERT the editor's approval into EDITORIAL_REVIEW.
-- 2. UPDATE the ARTICLE_VERSION status to 'Published'.
-- 3. UPDATE the main ARTICLE table to point to current_published_version_id to this new version.


-- 1. Create a brand new Editor
SET @editor_id = UUID_TO_BIN(UUID(), 1);
INSERT INTO USER 
    (user_id, username, email, password_hash) 
VALUES 
    (@editor_id, 'editor_tirth', 'tirth@example.com', 'hashed_pw_123');

-- Asssign the 'Editor' role to Jane
INSERT INTO USER_ROLE 
    (user_id, role_id) 
VALUES 
    (@editor_id, (SELECT role_id FROM ROLE WHERE role_name = 'Editor'));

-- 2. Create a brand new Author
SET @author_id = UUID_TO_BIN(UUID(), 1);
INSERT INTO USER (user_id, username, email, password_hash) 
VALUES (@author_id, 'author_tirth', 'tirth_auth@example.com', 'hashed_pw_456');

-- Assign the 'Author' role to Mark
INSERT INTO USER_ROLE (user_id, role_id) 
VALUES (@author_id, (SELECT role_id FROM ROLE WHERE role_name = 'Author'));

-- 3. author_tirth creates a new Article (It is NOT published yet, so published_version is NULL)
SET @article_id = UUID_TO_BIN(UUID(), 1);
INSERT INTO ARTICLE 
    (article_id, author_id, current_published_version_id) 
VALUES 
    (@article_id, @author_id, NULL); 

-- 4. author_tirth submits Version 1 of his article for review
SET @version_id = UUID_TO_BIN(UUID(), 1);
INSERT INTO ARTICLE_VERSION 
    (version_id, article_id, version_number, title, status_id, created_by)
VALUES 
    (@version_id, @article_id,  1, 
    'How to use Transactions in SQL', 
    (SELECT status_id FROM STATUS WHERE status_name = 'Pending Editor Review'), 
    @author_id
);

-- 5. Adding a 'Text' block as the first paragraph
INSERT INTO CONTENT_BLOCK (version_id, block_type_id, content_data, sequence_order)
VALUES (
    @version_id, 
    (SELECT block_type_id FROM BLOCK_TYPE WHERE type_name = 'Text'),
    'A transaction in SQL ensures the All-or-Nothing rule applies to database operations.',
    1
);

-- 6. Adding a 'Code' block right below it
INSERT INTO CONTENT_BLOCK (version_id, block_type_id, content_data, sequence_order)
VALUES (
    @version_id, 
    (SELECT block_type_id FROM BLOCK_TYPE WHERE type_name = 'Code'),
    'START TRANSACTION;\nUPDATE ARTICLE_VERSION SET status_id = 5;\nCOMMIT;',
    2
);


-- -------------------------------
-- Transaction starts (approve and publish)
-- -------------------------------
START TRANSACTION;

    -- Approve the article (done by editor)
    INSERT INTO EDITORIAL_REVIEW 
        (version_id, editor_id, decision, feedback)
    VALUES
        (@version_id, @editor_id, 'Approve', 'Excellent article, perfectly explained!');
    
    -- data store (redo and undo logs and bein log)
    -- single transaction dump
    
    -- The version status is upgraded from 'Pending' to 'Published'
    UPDATE ARTICLE_VERSION
    SET status_id = (
        SELECT 
            status_id 
        FROM STATUS 
        WHERE status_name = 'Published'
    ) 
    WHERE version_id = @version_id;

    -- The main ARTICLE is updated to point to this new Live Version 
    UPDATE ARTICLE
    SET current_published_version_id = @version_id
    WHERE article_id = @article_id;

COMMIT;


SHOW GRANTS FOR CURRENT_USER();
USE knowledge_base;

SHOW tables;
SELECT * FROM USER;
SELECT * FROM ARTICLE;
SELECT * FROM ARTICLE_VERSION;
SELECT * FROM CONTENT_BLOCK;
SELECT * FROM EDITORIAL_REVIEW;

SELECT DATABASE();
SHOW DATABASES;
SHOW tables;