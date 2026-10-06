-- =======================================================================
-- FILE: 01_Database\01_Create_Database.sql
-- =======================================================================

CREATE DATABASE IF NOT EXISTS knowledge_base;
USE knowledge_base;




-- =======================================================================
-- FILE: 02_Tables\01_look_tables.sql
-- =======================================================================

-- refering to this database for next set of sql statements
USE knowledge_base;

-- this table is for the different roles provided like (author, editor, reviewer / normal user)
CREATE TABLE ROLE (
    role_id INT PRIMARY KEY,
    role_name VARCHAR(50) UNIQUE NOT NULL
);

-- this table is to keep the track of status (draft -> pending for review -> needs improvement / rejected / published)
CREATE TABLE STATUS (
    status_id INT PRIMARY KEY,
    status_name VARCHAR(50) UNIQUE NOT NULL
);

-- this table is for the type of content we will allow in articles (text, code, image)
CREATE TABLE BLOCK_TYPE (
    block_type_id INT PRIMARY KEY,
    type_name VARCHAR(50) UNIQUE NOT NULL
);

-- different categories 
CREATE TABLE CATEGORY (
    category_id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(100) UNIQUE NOT NULL
);




-- =======================================================================
-- FILE: 02_Tables\02_user_tables.sql
-- =======================================================================

-- refering to this database for next set of sql statements
USE knowledge_base;

-- store general user information 
CREATE TABLE USER (
    user_id BINARY(16) DEFAULT (UUID_TO_BIN(UUID(), 1)) PRIMARY KEY,
    username VARCHAR(100) UNIQUE NOT NULL,
    email VARCHAR(255) UNIQUE NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

USE knowledge_base;
DESCRIBE USER_ROLE;
-- store role of the each user
CREATE TABLE USER_ROLE (
    user_id BINARY(16),
    role_id INT,
    PRIMARY KEY (user_id, role_id),
    FOREIGN KEY (user_id) REFERENCES USER(user_id) ON DELETE CASCADE, -- delete this entry if user is deleted
    FOREIGN KEY (role_id) REFERENCES ROLE(role_id) ON DELETE CASCADE -- delete this entry if role is deleted
);




-- =======================================================================
-- FILE: 02_Tables\03_article_tables.sql
-- =======================================================================

-- refering to this database for next set of sql statements
USE knowledge_base;

-- different tags
CREATE TABLE TAG (
    tag_id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(100) UNIQUE NOT NULL
);

-- store author, latest version accepted by editor 
CREATE TABLE ARTICLE (
    article_id BINARY(16) DEFAULT (UUID_TO_BIN(UUID(), 1)) PRIMARY KEY,
    author_id BINARY(16) NOT NULL,
    current_published_version_id BINARY(16) NULL, -- Will be set via foreign key after ARTICLE_VERSION is created
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (author_id) REFERENCES USER(user_id)
);

-- store categories of an article (one article -> many categories)
CREATE TABLE ARTICLE_CATEGORY (
    article_id BINARY(16),
    category_id INT,
    PRIMARY KEY (article_id, category_id),
    FOREIGN KEY (article_id) REFERENCES ARTICLE(article_id) ON DELETE CASCADE, -- delete this entry when article is deleted
    FOREIGN KEY (category_id) REFERENCES CATEGORY(category_id) ON DELETE CASCADE -- delete this entry when category is deleted 
);

-- store tags of an article (one article -> many tags)
CREATE TABLE ARTICLE_TAG (
    article_id BINARY(16),
    tag_id INT,
    PRIMARY KEY (article_id, tag_id),
    FOREIGN KEY (article_id) REFERENCES ARTICLE(article_id) ON DELETE CASCADE, -- delete this entry when article is deleted
    FOREIGN KEY (tag_id) REFERENCES TAG(tag_id) ON DELETE CASCADE -- delete entry entry when tag is deleted 
);




-- =======================================================================
-- FILE: 02_Tables\04_versionining_tables.sql
-- =======================================================================

-- refering to this database for next set of sql statements
USE knowledge_base;

-- store each version of each article with its status
CREATE TABLE ARTICLE_VERSION (
    version_id BINARY(16) DEFAULT (UUID_TO_BIN(UUID(), 1)) PRIMARY KEY,
    article_id BINARY(16) NOT NULL,
    version_number INT NOT NULL UNIQUE,
    title VARCHAR(255) NOT NULL,
    status_id INT NOT NULL,
    created_by BINARY(16) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP, -- yyyy-mm-dd 
    FOREIGN KEY (article_id) REFERENCES ARTICLE(article_id) ON DELETE CASCADE, -- delete this entry when article is deleted
    FOREIGN KEY (status_id) REFERENCES STATUS(status_id),
    FOREIGN KEY (created_by) REFERENCES USER(user_id)
);

-- ARTICLE table always point to latest version
ALTER TABLE ARTICLE 
ADD CONSTRAINT fk_current_published_version 
FOREIGN KEY (current_published_version_id) REFERENCES ARTICLE_VERSION(version_id) ON DELETE SET NULL;

-- content block for each article's each version (track by version_id directly)
CREATE TABLE CONTENT_BLOCK (
    block_id BINARY(16) DEFAULT (UUID_TO_BIN(UUID(), 1)) PRIMARY KEY,
    version_id BINARY(16) NOT NULL,
    block_type_id INT NOT NULL,
    content_data TEXT,
    sequence_order INT NOT NULL,
    FOREIGN KEY (version_id) REFERENCES ARTICLE_VERSION(version_id) ON DELETE CASCADE, -- delete this entry if this version of article is deleted
    FOREIGN KEY (block_type_id) REFERENCES BLOCK_TYPE(block_type_id) -- available types for article must be from BLOCK_TYPE look_table
);




-- =======================================================================
-- FILE: 02_Tables\05_editor_tables.sql
-- =======================================================================

-- refering to this database for next set of sql statements
USE knowledge_base;


-- storing track of status of each article when reviewed by an editor
CREATE TABLE EDITORIAL_REVIEW (
    review_id BINARY(16) DEFAULT (UUID_TO_BIN(UUID(), 1)) PRIMARY KEY,
    version_id BINARY(16) NOT NULL, -- which version is being reviewed by editor ?
    editor_id BINARY(16) NOT NULL, -- who is the editor reviewing this article ?
    decision ENUM('Approve', 'Reject', 'Suggest Improvements') NOT NULL, -- possible decisions
    feedback TEXT NOT NULL, -- feedback by editor to author
    reviewed_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP, -- when it was reviewed for publishment
    FOREIGN KEY (version_id) REFERENCES ARTICLE_VERSION(version_id) ON DELETE CASCADE, -- delete this entry when that article version is deleted
    FOREIGN KEY (editor_id) REFERENCES USER(user_id) -- editor must be a user with role as editor
);




-- =======================================================================
-- FILE: 02_Tables\06_review_tables.sql
-- =======================================================================

-- refering to this database for next set of sql statements
USE knowledge_base;


-- stores user rating for particular article 
CREATE TABLE USER_RATING (
    article_id BINARY(16), -- for which article rating is for
    user_id BINARY(16), -- who has rated
    rating_value TINYINT NOT NULL CHECK (rating_value >= 1 AND rating_value <= 5), -- rated value
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP, -- when 
    PRIMARY KEY (article_id, user_id),
    FOREIGN KEY (article_id) REFERENCES ARTICLE(article_id) ON DELETE CASCADE, -- delete this entry if article is deleted
    FOREIGN KEY (user_id) REFERENCES USER(user_id) ON DELETE CASCADE -- delete this entry if the user (reviewer) is deleted
);

-- stores user comments for article
CREATE TABLE USER_COMMENT (
    comment_id BINARY(16) DEFAULT (UUID_TO_BIN(UUID(), 1)) PRIMARY KEY,
    article_id BINARY(16) NOT NULL, -- for which article comment is for
    user_id BINARY(16) NOT NULL, -- who has commented
    comment_text TEXT NOT NULL, -- what is comment
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP, -- when it was made 
    FOREIGN KEY (article_id) REFERENCES ARTICLE(article_id) ON DELETE CASCADE, -- delete this entry when article is deleted
    FOREIGN KEY (user_id) REFERENCES USER(user_id) ON DELETE CASCADE -- delete this entry when user (reviewer) is deleted
);




-- =======================================================================
-- FILE: 03_DDL\01_alter.sql
-- =======================================================================

USE knowledge_base;

-- Adding a column to USER table
ALTER TABLE USER ADD COLUMN bio TEXT NULL;

-- Allow Floating point ratings in USER_RATING table
ALTER TABLE USER_RATING MODIFY COLUMN rating_value DECIMAL(2,1) NOT NULL;

-- Adding Sample Indexing (temporary purpose just to show ALTER query demo [delete later])
ALTER TABLE ARTICLE_VERSION ADD INDEX idx_article_title (title);




-- =======================================================================
-- FILE: 03_DDL\02_Rename_Table.sql
-- =======================================================================

USE knowledge_base;

-- Example: Renaming a table (commented out to prevent accidental execution)
RENAME TABLE USER TO USERS;
RENAME TABLE USERS TO USER;




-- =======================================================================
-- FILE: 03_DDL\03_Truncate_Table.sql
-- =======================================================================

USE knowledge_base;

-- Example: Truncating a table to delete all rows quickly without logging individual deletes
-- TRUNCATE TABLE USER_COMMENT;




-- =======================================================================
-- FILE: 03_DDL\04_Drop_Table.sql
-- =======================================================================

USE knowledge_base;

-- Example: Dropping a table 
-- DROP TABLE IF EXISTS USER_COMMENT;

-- Example: Dropping an Index (Dropping that temporary index)
ALTER TABLE ARTICLE_VERSION DROP INDEX idx_article_title;




-- =======================================================================
-- FILE: 04_DML\01_lookup_table_insert.sql
-- =======================================================================

-- Insert Default values in all lookup tables
INSERT INTO ROLE (role_id, role_name) VALUES 
(1, 'Reviewer'), (2, 'Author'), (3, 'Editor');

INSERT INTO STATUS (status_id, status_name) VALUES 
(1, 'Draft'), (2, 'Pending Editor Review'), (3, 'Needs Improvement'), (4, 'Rejected'), (5, 'Published');

INSERT INTO BLOCK_TYPE (block_type_id, type_name) VALUES 
(1, 'Text'), (2, 'Code'), (3, 'Image');

INSERT INTO CATEGORY (name) VALUES 
('Engineering & Technology'),
('Product & Design'),
('Human Resources'),
('Sales & Marketing'),
('Customer Support'),
('Company Operations');




-- =======================================================================
-- FILE: 04_DML\02_insert_sample_data.sql
-- =======================================================================

-- 1. setting up UUID manually for now
-- 1 changes UUID in such a way to have timestamp based sorted order (helps in searching)
SET @author_id  = UUID_TO_BIN('11111111-1111-1111-1111-111111111111', 1);
SET @editor_id  = UUID_TO_BIN('22222222-2222-2222-2222-222222222222', 1);
SET @reviewer_id  = UUID_TO_BIN('33333333-3333-3333-3333-333333333333', 1);



-- 2. INSERT MANY ROWS AT A TIME
INSERT INTO USER (user_id, username, email, password_hash) VALUES 
(@author_id , 'jagdish', 'jagdish@example.com', 'hash1'),
(@editor_id , 'mihir', 'mihir@example.com', 'hash2'),
(@reviewer_id , 'rudra', 'rudra@example.com', 'hash3');



-- 3. INSERT BASED ON SELECT
INSERT INTO USER_ROLE (user_id,role_id) 
SELECT user_id, 2 FROM USER WHERE username = 'jagdish'; -- setting as author

INSERT INTO USER_ROLE (user_id,role_id)
SELECT user_id, 3 FROM USER WHERE username = 'mihir'; -- setting as editor

INSERT INTO USER_ROLE (user_id,role_id)
SELECT user_id, 1 FROM USER WHERE username = 'rudra'; -- setting as reviewer



-- 4. INSERT A ARTICLE
SET @article_id = UUID_TO_BIN('AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA', 1);
-- The article is born (but has no published version yet)
INSERT INTO ARTICLE (article_id, author_id) VALUES (@article_id, @author_id);



-- 5. ATTACH A VERSION FIRST
SET @version_id = UUID_TO_BIN('BBBBBBBB-BBBB-BBBB-BBBB-BBBBBBBBBBBB', 1);
-- The Author saves their first Draft (Status 1 = Draft)
INSERT INTO ARTICLE_VERSION (version_id, article_id, version_number, title, status_id, created_by)
VALUES (@version_id, @article_id, 1, 'My Awesome SQL Guide', 1, @author_id);



-- 6. ATTACH CONTENT TO THIS VERSION
INSERT INTO CONTENT_BLOCK (version_id, block_type_id, content_data, sequence_order) VALUES
(@version_id, 1, 'SQL stands for Structured Query Language.', 1),
(@version_id, 3, 'SELECT * FROM users;', 2);



-- 7. ATTACH TAGS AND CATEGORIES
INSERT INTO ARTICLE_CATEGORY (article_id, category_id) VALUES (@article_id, 1);
INSERT IGNORE INTO TAG (name) VALUES ('SQL');
INSERT IGNORE INTO ARTICLE_TAG (article_id, tag_id) SELECT @article_id, tag_id FROM TAG WHERE name = 'SQL';



-- 8. Author clicks "Submit for Review". Backend updates status.
UPDATE ARTICLE_VERSION 
SET status_id = 2 -- Status 2 = Pending Editor Review
WHERE version_id = @version_id;



-- 9. Editor (Mihir) reads the article and clicks "Approve".
-- Backend saves the review...
INSERT INTO EDITORIAL_REVIEW (version_id, editor_id, decision, feedback)
VALUES (@version_id, @editor_id, 'Approve', 'Excellent tutorial. Ready to go live!');
-- ...and Backend instantly upgrades the status to Published.
UPDATE ARTICLE_VERSION 
SET status_id = 5 -- Status 5 = Published
WHERE version_id = @version_id;
-- Backend officially maps this published version to the main article for fast reading.
UPDATE ARTICLE 
SET current_published_version_id = @version_id
WHERE article_id = @article_id;



-- 10. A reviewer (Rudra) reads the now-published article and leaves a 5-star rating and comment.
INSERT INTO USER_RATING (article_id, user_id, rating_value) 
VALUES (@article_id, @reviewer_id, 5);

INSERT INTO USER_COMMENT (article_id, user_id, comment_text) 
VALUES (@article_id, @reviewer_id, 'This guide saved my life today, thank you Jagdish!');





----------------------------------------------------------------------------------------------------------------
-- Adding two more articles for the same author to populate the database (using same flow as above)

/* ==========================================================
   ARTICLE 2
   ========================================================== */

-- 11. CREATE SECOND ARTICLE
SET @article2_id = UUID_TO_BIN('CCCCCCCC-CCCC-CCCC-CCCC-CCCCCCCCCCCC', 1);

INSERT INTO ARTICLE (article_id, author_id) VALUES (@article2_id, @author_id);


-- CREATE FIRST VERSION
SET @version2_id = UUID_TO_BIN('DDDDDDDD-DDDD-DDDD-DDDD-DDDDDDDDDDDD', 1);

INSERT INTO ARTICLE_VERSION (version_id, article_id, version_number, title, status_id, created_by)
VALUES (@version2_id, @article2_id, 1, 'Understanding Database Indexes', 1, @author_id);


-- CONTENT BLOCKS
INSERT INTO CONTENT_BLOCK (version_id, block_type_id, content_data, sequence_order)
VALUES (@version2_id, 1, 'Indexes improve database query performance by reducing the amount of data scanned.', 1),
(@version2_id, 1, 'However, indexes also increase storage usage and slightly slow INSERT, UPDATE, and DELETE operations.', 2),
(@version2_id, 3, 'CREATE INDEX idx_username ON users(username);', 3);


-- CATEGORY & TAG
INSERT INTO ARTICLE_CATEGORY (article_id, category_id) VALUES (@article2_id, 1);

INSERT IGNORE INTO TAG (name) VALUES ('Indexing');

INSERT IGNORE INTO ARTICLE_TAG (article_id, tag_id)
SELECT @article2_id, tag_id FROM TAG WHERE name = 'Indexing';


-- SUBMIT FOR REVIEW
UPDATE ARTICLE_VERSION
SET status_id = 2
WHERE version_id = @version2_id;


-- EDITOR APPROVES
INSERT INTO EDITORIAL_REVIEW (version_id, editor_id, decision, feedback)
VALUES (@version2_id, @editor_id, 'Approve', 'Well structured explanation with useful SQL example.');

UPDATE ARTICLE_VERSION
SET status_id = 5
WHERE version_id = @version2_id;

UPDATE ARTICLE
SET current_published_version_id = @version2_id
WHERE article_id = @article2_id;


-- USER RATING & COMMENT
INSERT INTO USER_RATING (article_id, user_id, rating_value)
VALUES (@article2_id, @reviewer_id, 4);

INSERT INTO USER_COMMENT (article_id, user_id, comment_text)
VALUES (@article2_id, @reviewer_id, 'Very helpful explanation of indexes and their trade-offs.');



/* ==========================================================
   ARTICLE 3
   ========================================================== */

-- 12. CREATE THIRD ARTICLE
SET @article3_id = UUID_TO_BIN('EEEEEEEE-EEEE-EEEE-EEEE-EEEEEEEEEEEE', 1);

INSERT INTO ARTICLE (article_id, author_id)
VALUES (@article3_id, @author_id);


-- CREATE FIRST VERSION
SET @version3_id = UUID_TO_BIN('FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF', 1);

INSERT INTO ARTICLE_VERSION (version_id, article_id, version_number, title, status_id, created_by)
VALUES (@version3_id, @article3_id, 1, 'Getting Started with SQL JOINs', 1, @author_id);


-- CONTENT BLOCKS
INSERT INTO CONTENT_BLOCK (version_id, block_type_id, content_data, sequence_order)
VALUES (@version3_id, 1, 'JOIN combines rows from two or more tables based on a related column.', 1),
(@version3_id, 1, 'Common JOIN types include INNER JOIN, LEFT JOIN, RIGHT JOIN, and FULL OUTER JOIN.', 2),
(@version3_id, 3, 'SELECT u.username, r.role_name FROM user u INNER JOIN user_role ur ON u.user_id = ur.user_id INNER JOIN role r ON ur.role_id = r.role_id;', 3);


-- CATEGORY & TAG
INSERT INTO ARTICLE_CATEGORY (article_id, category_id) VALUES (@article3_id, 1);

INSERT IGNORE INTO TAG (name) VALUES ('JOIN');

INSERT IGNORE INTO ARTICLE_TAG (article_id, tag_id)
SELECT @article3_id, tag_id
FROM TAG
WHERE name = 'JOIN';


-- SUBMIT FOR REVIEW
UPDATE ARTICLE_VERSION
SET status_id = 2
WHERE version_id = @version3_id;


-- EDITOR APPROVES
INSERT INTO EDITORIAL_REVIEW (version_id, editor_id, decision, feedback)
VALUES (@version3_id, @editor_id, 'Approve', 'Clear introduction with practical SQL example.');

UPDATE ARTICLE_VERSION
SET status_id = 5
WHERE version_id = @version3_id;

UPDATE ARTICLE
SET current_published_version_id = @version3_id
WHERE article_id = @article3_id;


-- USER RATING & COMMENT
INSERT INTO USER_RATING (article_id, user_id, rating_value)
VALUES (@article3_id, @reviewer_id, 5);

INSERT INTO USER_COMMENT (article_id, user_id, comment_text)
VALUES (@article3_id, @reviewer_id, 'Excellent beginner-friendly article on SQL JOINs.');




-- =======================================================================
-- FILE: 04_DML\03_misc_query.sql
-- =======================================================================

-- upsert 
-- The database tries to INSERT a new row. If that row already exists (based on a PRIMARY KEY or UNIQUE constraint), it doesn't throw an error; instead, it automatically switches to doing an UPDATE on the existing row.

-- sample query
-- INSERT INTO USER_RATING (article_id, user_id, rating_value) 
-- VALUES ('article-uuid', 'user-uuid', 5)
-- ON DUPLICATE KEY UPDATE 
--     rating_value = 5, 
--     updated_at = CURRENT_TIMESTAMP;



-- replace into -- check
-- REPLACE INTO looks similar to an Upsert, but it is much more aggressive under the hood. If it finds a duplicate key, it completely DELETES the old row and INSERTS a brand new one.
-- (dangerous if applied ON DELETE CASCADE)



-- decimal vs float
-- FLOAT (or DOUBLE): Stores an approximate value -- not 100% accurate
-- DECIMAL (or NUMERIC): Stores the exact value -- 100% accurate




-- =======================================================================
-- FILE: 05_DQL\01_select.sql
-- =======================================================================

-- select all roles 
SELECT role_name FROM ROLE;

-- select all categories
SELECT name FROM CATEGORY;

-- select all tags
SELECT name FROM TAG;

-- select all block type supported
SELECT type_name FROM BLOCK_TYPE;

-- select all users
SELECT username,email FROM USER;

-- select distinct rating values
SELECT DISTINCT rating_value FROM USER_RATING;

-- select all articles
SELECT article_id FROM ARTICLE;

SELECT * FROM USER_COMMENT;
SELECT * FROM USER_RATING;
SELECT * FROM TAG;
SELECT * FROM ARTICLE_TAG;




-- =======================================================================
-- FILE: 05_DQL\02_where.sql
-- =======================================================================

-- select users created after '01-01-2026'
SELECT username, email FROM USER WHERE created_at > '2026-01-01';

-- select all articles created after '01-01-2026'
SELECT article_id FROM ARTICLE WHERE created_at > '2026-01-01';

-- select all article versions reviewed after '01-01-2026'
SELECT review_id, version_id, editor_id FROM EDITORIAL_REVIEW WHERE reviewed_at > '2026-01-01';

-- select all ratings above 4 and created after '01-01-2026'
SELECT rating_value FROM USER_RATING WHERE rating_value > 4.0 AND updated_at > '2026-01-01';

-- select all comments after '01-01-2026'
SELECT comment_text FROM USER_COMMENT WHERE created_at > '2026-01-01';




-- =======================================================================
-- FILE: 05_DQL\03_orderby.sql
-- =======================================================================

-- 1. Retrieve users ordered by when they joined (Newest first)
SELECT username, email, created_at  
FROM USER 
ORDER BY created_at DESC;


-- 2. Retrieve tags in alphabetical order
SELECT name 
FROM TAG 
ORDER BY name ASC; 


-- 3. Retrieve article versions ordered by version number
SELECT title, version_number, created_at 
FROM ARTICLE_VERSION 
ORDER BY version_number ASC;


-- 4. Retrieve content blocks for an article version in the correct sequence
SELECT block_id, block_type_id, content_data, sequence_order 
FROM CONTENT_BLOCK 
ORDER BY sequence_order ASC;




-- =======================================================================
-- FILE: 05_DQL\04_offset_limit.sql
-- =======================================================================

-- Get the first 10 newest users
SELECT username, created_at 
FROM USER 
ORDER BY created_at DESC 
LIMIT 10;


-- Get the NEXT 10 users (Page 2)
SELECT username, created_at 
FROM USER 
ORDER BY created_at DESC 
LIMIT 10 OFFSET 10;




-- =======================================================================
-- FILE: 06_Aggr_Function\01_min.sql
-- =======================================================================

-- 1. first registered user
SELECT MIN(created_at) AS "FIRST USER" 
FROM USER;


-- 2. Find the lowest rating a specific article has ever received.
SELECT MIN(rating_value) AS lowest_rating 
FROM USER_RATING 
WHERE article_id = UUID_TO_BIN('AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA',1);




-- =======================================================================
-- FILE: 06_Aggr_Function\02_max.sql
-- =======================================================================

-- 1. Find the latest version number for an article
SELECT MAX(version_number) AS latest_version_number 
FROM ARTICLE_VERSION 
WHERE article_id = UUID_TO_BIN('AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA',1);


-- 2. Find the sequence order for appending a new content block
SELECT MAX(sequence_order) AS last_block_order 
FROM CONTEN	T_BLOCK  WHERE version_id = UUID_TO_BIN('AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA',1);


-- 3. Find the most recent activity on the platform
SELECT MAX(created_at) AS most_recent_comment_time FROM USER_COMMENT;




-- =======================================================================
-- FILE: 06_Aggr_Function\03_sum.sql
-- =======================================================================

-- 1. Calculate the total "rating points" an article has received
SELECT article_id, SUM(rating_value) as total_rating
FROM USER_RATING GROUP BY article_id ORDER BY total_rating DESC;




-- =======================================================================
-- FILE: 06_Aggr_Function\04_ifnull.sql
-- =======================================================================

-- 1. Displaying a default status for unpublished articles
SELECT 
    article_id, 
    IFNULL(current_published_version_id, 'Draft - Not Published') AS publish_status 
FROM ARTICLE;




-- =======================================================================
-- FILE: 07_Data_Aggr\01_groupby.sql
-- =======================================================================

-- 1. Count how many articles each author has published
SELECT author_id, COUNT(article_id) AS total_articles
FROM ARTICLE GROUP BY author_id;


-- 2. Calculate the average rating for every single article
SELECT 
    article_id, 
    AVG(rating_value) AS average_rating
FROM USER_RATING
GROUP BY article_id;


-- 3. Count how many tags are attached to each article
SELECT 
    article_id, 
    COUNT(tag_id) AS total_tags
FROM ARTICLE_TAG
GROUP BY article_id;




-- =======================================================================
-- FILE: 07_Data_Aggr\02_grouby_having.sql
-- =======================================================================

-- 1. Find low-rated articles that need improvement (Average rating strictly below 3.0)
SELECT 
    article_id, 
    AVG(rating_value) AS average_rating
FROM USER_RATING
GROUP BY article_id
HAVING AVG(rating_value) < 3.0;


-- 2. find users who have more than 10 comments
SELECT 
    user_id,
    COUNT(comment_id) as total_comments
FROM USER_COMMENT
GROUP BY user_id
HAVING COUNT(comment_id) > 10;


-- 3. Find Articles with more than 5 tags
SELECT 
    article_id, 
    COUNT(tag_id) AS tag_count
FROM ARTICLE_TAG
GROUP BY article_id
HAVING COUNT(tag_id) > 5;




-- =======================================================================
-- FILE: 07_Data_Aggr\03_combined_variations.sql
-- =======================================================================

-- 1. Find Top-Rated Articles based ONLY on Recent Ratings
SELECT  
    article_id,
    AVG(rating_value) AS average_rating,
    COUNT(user_id) AS total_ratings
FROM USER_RATING
WHERE updated_at >= '2026-01-01'
GROUP BY article_id
HAVING AVG(rating_value) > 4.0
ORDER BY average_rating DESC;





-- =======================================================================
-- FILE: 08_JOINS\01_inner.sql
-- =======================================================================

-- 1. Get Published Articles and their Author's Name
SELECT 
    article.article_id,
    article_version.title AS article_title,
    usr.username AS author_name,
    article.created_at AS original_creation_date
FROM ARTICLE article
INNER JOIN USER usr ON article.author_id = usr.user_id
INNER JOIN ARTICLE_VERSION article_version ON article.current_published_version_id = article_version.version_id;


-- 2. Show Tags for an Article
SELECT 
    tag.name AS tag_name
FROM ARTICLE article
INNER JOIN ARTICLE_TAG article_tag ON article.article_id = article_tag.article_id
INNER JOIN TAG tag ON article_tag.tag_id = tag.tag_id
WHERE article.article_id = UUID_TO_BIN('AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA', 1);



-- 3. Show Categories for an Article
SELECT
    category.name AS category
FROM ARTICLE article
INNER JOIN ARTICLE_CATEGORY article_category ON article.article_id = article_category.article_id
INNER JOIN CATEGORY category ON category.category_id = article_category.category_id
WHERE article.article_id = UUID_TO_BIN('AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA', 1);




-- =======================================================================
-- FILE: 08_JOINS\02_left.sql
-- =======================================================================

-- 1. List All Articles and their Assigned Categories (Including uncategorized articles)
SELECT 
    article.article_id,
    IFNULL(GROUP_CONCAT(category.name SEPARATOR ', '), 'Uncategorized') AS category_names
FROM ARTICLE article
LEFT JOIN ARTICLE_CATEGORY article_category ON article.article_id = article_category.article_id
LEFT JOIN CATEGORY category ON article_category.category_id = category.category_id
GROUP BY article.article_id;


-- 2. A list of every single user in the database, and the titles of all the officially published articles they have written.
SELECT 
    usr.username, 
    IFNULL(article_version.title, 'No Published Articles Yet') AS authored_article_title
FROM USER usr 
LEFT JOIN ARTICLE article ON usr.user_id = article.author_id
LEFT JOIN ARTICLE_VERSION article_version ON article.current_published_version_id = article_version.version_id;




-- =======================================================================
-- FILE: 08_JOINS\03_right.sql
-- =======================================================================

-- 1. find which user is allocated which role, and find roles which have no user
SELECT 
    user_role.user_id, 
    role.role_name AS role_name
FROM USER_ROLE user_role
RIGHT JOIN ROLE role ON user_role.role_id = role.role_id;

-- 2. Find Unused Categories
SELECT
    category.name AS unused_categories
FROM ARTICLE_CATEGORY article_category 
RIGHT JOIN
CATEGORY category ON category.category_id = article_category.category_id
WHERE article_category.article_id IS NULL;




-- =======================================================================
-- FILE: 09_SubQueries\01_SingleRow.sql
-- =======================================================================

USE knowledge_base;


-- Query:- find the details of most recently 'published' article version. (get latest published article)
SELECT 
    version_id, article_id, version_number, title, created_at 
FROM ARTICLE_VERSION 
WHERE status_id = (
    SELECT status_id 
    FROM STATUS 
    WHERE status_name = 'Published'
)
AND
created_at = (
    SELECT 
        MAX(created_at) 
    FROM ARTICLE_VERSION 
    WHERE status_id = (
        SELECT status_id 
        FROM STATUS 
        WHERE status_name = 'Published'
    )
);


-- Query:- Find all the articles with rating below average rating
SELECT 
    article_id, rating_value 
FROM USER_RATING 
WHERE rating_value < (SELECT AVG(rating_value) FROM USER_RATING);


-- Query:- Find all article versions that need improvement.
SELECT 
    version_id, title, created_at 
FROM ARTICLE_VERSION
WHERE status_id = (
    SELECT status_id 
    FROM STATUS 
    WHERE status_name = 'Needs Improvement'
);




-- =======================================================================
-- FILE: 09_SubQueries\02_MultipleRow.sql
-- =======================================================================

-- Query:- Find all the users who are editors 
SELECT 
    username, email 
FROM USER 
WHERE user_id IN (
    SELECT user_id 
    FROM USER_ROLE 
    WHERE role_id = (SELECT role_id FROM ROLE WHERE role_name = 'Editor')
);


-- Query:- Find all articles that belong to the 'Engineering & Technology' category
SELECT 
    article_id, created_at 
FROM ARTICLE 
WHERE article_id IN (
    SELECT article_id 
    FROM ARTICLE_CATEGORY 
    WHERE category_id = (SELECT category_id FROM CATEGORY WHERE name = 'Engineering & Technology')
);


-- Query:- Find article versions that have been rejected by ANY editor.
SELECT 
    version_id, title
FROM ARTICLE_VERSION 
WHERE version_id = ANY (
    SELECT version_id 
    FROM EDITORIAL_REVIEW 
    WHERE decision = 'Reject'
);




-- =======================================================================
-- FILE: 09_SubQueries\03_Correlated.sql
-- =======================================================================

-- Query:- Find the latest version of EACH article.
SELECT 
    article_id, version_id, title 
FROM ARTICLE_VERSION AS article_version
WHERE version_number = (
    SELECT MAX(version_number) 
    FROM ARTICLE_VERSION article_version
    WHERE article_version.article_id = article_version.article_id
);


-- Query:- Find users who have commented on their OWN articles.
SELECT 
    user_id,article_id
FROM USER_COMMENT AS user_comment
WHERE user_id = (
    SELECT author_id 
    FROM ARTICLE AS article
    WHERE article.article_id = user_comment.article_id
);


-- Query:- Get article list of authors along with their total published articles count.
SELECT 
    usr.username,
    (
        SELECT COUNT(*) 
        FROM ARTICLE AS article 
        WHERE article.author_id = usr.user_id 
          AND article.current_published_version_id IS NOT NULL
    ) AS total_published_articles
FROM USER AS user;




-- =======================================================================
-- FILE: 10_Unions\01_Union.sql
-- =======================================================================

-- Query:- Get a combined timeline of a user's comments and ratings
SELECT 
    'Left a Comment' AS activity_type, article_id, created_at AS activity_date
FROM USER_COMMENT 
WHERE user_id = UUID_TO_BIN('33333333-3333-3333-3333-333333333333', 1)

UNION 

SELECT 
    'Gave a Rating' AS activity_type, article_id, updated_at AS activity_date
FROM USER_RATING 
WHERE user_id = UUID_TO_BIN('33333333-3333-3333-3333-333333333333', 1)

ORDER BY activity_date DESC;


-- Query:- All metrics in one view
SELECT 
    'Total Users' AS metric_name, COUNT(*) AS metric_value 
FROM USER

UNION

SELECT 
    'Total Published Articles' AS metric_name, COUNT(*) AS metric_value 
FROM ARTICLE_VERSION 
WHERE status_id = (SELECT status_id FROM STATUS WHERE status_name = 'Published')

UNION

SELECT 
    'Total Comments' AS metric_name, COUNT(*) AS metric_value 
FROM USER_COMMENT;




-- =======================================================================
-- FILE: 10_Unions\02_Intersect.sql
-- =======================================================================

-- Query:- Find users who exist in BOTH the ARTICLE table and the USER_COMMENT table (highly active users)
SELECT 
    author_id AS user_id 
FROM ARTICLE

INTERSECT

SELECT 
    user_id 
FROM USER_COMMENT;


	-- Query:- Find articles that have Tag ID 5 AND Tag ID 2
SELECT 
    article_id FROM ARTICLE_TAG WHERE tag_id = (
    SELECT 
        tag_id 
    FROM TAG 
    WHERE name = 'SQL'
)

INTERSECT

SELECT 
    article_id 
FROM ARTICLE_TAG 
WHERE tag_id = (
    SELECT 
        tag_id 
    FROM TAG 
    WHERE name = 'JOIN'
);




-- =======================================================================
-- FILE: 10_Unions\03_Except.sql
-- =======================================================================

-- Query:- Find ghost authors (inactive check)
SELECT user_id 
FROM USER_ROLE 
WHERE role_id = (
    SELECT role_id 
    FROM ROLE 
    WHERE role_name = 'Author'
)

EXCEPT

-- Subtract anyone who actually has a record in the ARTICLE table
SELECT author_id 
FROM ARTICLE;



-- Query:- All article versions currently in 'Pending Editor Review' state
SELECT version_id 
FROM ARTICLE_VERSION 
WHERE status_id = (
    SELECT status_id 
    FROM STATUS 
    WHERE status_name = 'Pending Editor Review'
)

EXCEPT

SELECT version_id 
FROM EDITORIAL_REVIEW 
WHERE editor_id = UUID_TO_BIN('22222222-2222-2222-2222-222222222222', 1);




-- =======================================================================
-- FILE: 11_Others\01_built-in-function.sql
-- =======================================================================

-- 1. LENGTH()
SELECT title,LENGTH(title) FROM ARTICLE_VERSION;

-- 2. UPPER()
SELECT UPPER(username) FROM USER;

-- 3. LOWER()
SELECT LOWER(username) FROM USER;

-- 4. CONCAT()
SELECT CONCAT(username, ' ', email) FROM USER;

-- 5. REPLACE()
SELECT REPLACE(email,'gmail.com','yahoo.com') FROM USER;

-- 6. REVERSE()
SELECT REVERSE(email) FROM USER;

-- 7. ROUND()
SELECT ROUND(123.4567,2);

-- 8. CEIL()
SELECT CEILING(12.1);

-- 9. FLOOR()
SELECT FLOOR(13.8);

-- 10. ABS()
SELECT ABS(-100);

-- 11. MOD()
SELECT MOD(10,3);

-- 12. POWER()
SELECT POWER(10,2);

-- 13. SQRT()
SELECT SQRT(9);

-- 14. TRUNCATE()
SELECT TRUNCATE(16.89791,2);

-- 15. IFNULL()
SELECT IFNULL(NULL,10000); -- REPLACE NULL WITH VALUE 




-- =======================================================================
-- FILE: 11_Others\02_user-defined-function.sql
-- =======================================================================

-- Usage:- Function 1: Estimating "Reading Time" (fn_EstimateReadingTime)
USE knowledge_base;

DROP FUNCTION IF EXISTS fn_EstimateReadingTime;

DELIMITER //
CREATE FUNCTION fn_EstimateReadingTime(p_version_id BINARY(16))
RETURNS INT
DETERMINISTIC 
BEGIN
    DECLARE total_chars INT;
    DECLARE minutes_to_read INT;

    -- SUM of length of all 'TEXT' blocks for this specific article version
    SELECT 
        COALESCE(SUM(LENGTH(content_data)),0) INTO total_chars
    FROM CONTENT_BLOCK
    WHERE version_id = p_version_id
        AND
    block_type_id = (
        SELECT 
            block_type_id 
        FROM BLOCK_TYPE 
        WHERE type_name = 'Text'
    );

    -- assume average reading speed is 200 chars per minute
    SET minutes_to_read = CEILING(total_chars / 200);

    -- If less than 1 min, just return 1
    IF minutes_to_read = 0 THEN 
        SET minutes_to_read = 1;
    END IF;

    RETURN minutes_to_read;

END //
DELIMITER ;

SHOW FUNCTION STATUS
WHERE Db = 'knowledge_base'
  AND Name = 'fn_EstimateReadingTime';
  
SELECT DATABASE();

-- Using it:-
SELECT title, fn_EstimateReadingTime(version_id) AS read_time_mins 
FROM ARTICLE_VERSION;





-- Usage:- Fetching Average Rating (fn_GetAverageRating)
DELIMITER //
CREATE FUNCTION fn_GetAverageRating(p_article_id BINARY(16)) 
RETURNS DECIMAL(3,1)
READS SQL DATA
BEGIN
    DECLARE avg_score DECIMAL(3,1);
    
    SELECT COALESCE(AVG(rating_value), 0.0) INTO avg_score
    FROM USER_RATING
    WHERE article_id = p_article_id;
    
    RETURN avg_score;
END //
DELIMITER ;


SELECT article_id 
FROM ARTICLE 
WHERE fn_GetAverageRating(article_id) > 4.5;




-- =======================================================================
-- FILE: 11_Others\03_CTE.sql
-- =======================================================================

-- Query:- Making complex logic readable
-- We want article list of users and their published articles. Instead of throwing everything into one giant JOIN with article WHERE clause, article CTE will help to make it more cleaner and readable.
USE knowledge_base;

WITH DraftVersion AS (
    SELECT 
        created_by, title, created_at
    FROM ARTICLE_VERSION
    WHERE status_id = (
        SELECT 
            status_id 
        FROM 
            STATUS 
        WHERE 
            status_name = 'Published'
    )
)
SELECT 
    usr.username, draftversion.title AS draft_title, draftversion.created_at 
FROM USER usr
JOIN 
DraftVersion draftversion 
ON
usr.user_id = draftversion.created_by;





-- Query:- Making complex logic readable 
WITH ArticleRatingStats AS (
    SELECT 
        article_id, AVG(rating_value) AS average_rating, COUNT(*) AS total_votes
    FROM USER_RATING
    GROUP BY article_id
)
SELECT 
    article.article_id, usr.username AS author, stats.average_rating
FROM ARTICLE article 
JOIN USER usr
ON
article.author_id = usr.user_id
JOIN
ArticleRatingStats stats 
ON
article.article_id = stats.article_id;




-- =======================================================================
-- FILE: 11_Others\04_triggers.sql
-- =======================================================================

-- BEFORE INSERT (validate email to convert into lowercase)

DELIMITER //
CREATE TRIGGER trg_beforeinsert_user
BEFORE INSERT
ON USER
FOR EACH ROW
BEGIN
    -- 'NEW' holds the data the user is trying to insert
    -- We force the email to be in lowercase to avoid problems during auth
    SET NEW.email = LOWER(NEW.email)
END //
DELIMITER ;



-- AFTER INSERT (for editorial flow)

DELIMITER //
CREATE TRIGGER trg_afterinsert_editorialreview
AFTER INSERT
ON EDITORIAL_REVIEW
FOR EACH ROW
BEGIN

    IF NEW.decision = 'Approve' THEN
        UPDATE ARTICLE_VERSION
        SET status_id = (
            SELECT status_id
            FROM STATUS
            WHERE status_name = 'Published'
        )
        WHERE version_id = NEW.version_id;
    
    ELSEIF NEW.decision = 'Reject' THEN
        UPDATE ARTICLE_VERSION
        SET status_id = (
            SELECT status_id 
            FROM STATUS
            WHERE status_name = 'Rejected'
        )
        WHERE version_id = NEW.version_id;

    ELSEIF NEW.decision = 'Suggest Improvement' THEN
        UPDATE ARTICLE_VERSION
        SET status_id = (
            SELECT status_id 
            FROM STATUS
            WHERE status_name = 'Needs Improvement'
        )
        WHERE version_id = NEW.version_id;

    END IF;
END //
DELIMITER ;



-- BEFORE UPDATE (cannot downgrade a article status)

DELIMITER //
CREATE TRIGGER trg_beforeupdate_articleversion
BEFORE UPDATE 
ON ARTICLE_VERSION
FOR EACH ROW
BEGIN
    DECLARE v_published_id INT;
    DECLARE v_draft_id INT;
    
    -- Dynamically grab the IDs so we avoid hardcoding magic numbers
    SELECT status_id INTO v_published_id FROM STATUS WHERE status_name = 'Published';
    SELECT status_id INTO v_draft_id FROM STATUS WHERE status_name = 'Draft';

    -- Check if someone is trying to make an illegal state change
    IF OLD.status_id = v_published_id AND NEW.status_id = v_draft_id THEN
        -- SIGNAL SQLSTATE '45000' acts like 'throw new Exception()' in C#
        -- It immediately aborts the UPDATE query.
        SIGNAL SQLSTATE '45000' 
        SET MESSAGE_TEXT = 'Invalid Action: Cannot downgrade a Published version back to a Draft. Please create a new version instead.';
    END IF;
END //
DELIMITER ;



-- AFTER UPDATE (when article is published, update current_published_version_id to point the new version)

DELIMITER //
CREATE TRIGGER trg_afterupdate_articleversion
AFTER UPDATE
ON ARTICLE_VERSION
FOR EACH ROW
BEGIN

    DECLARE v_published_id INT;
    
    SELECT 
        status_id 
    INTO 
        v_published_id 
    FROM 
        STATUS 
    WHERE 
        status_name = 'Published';

    IF NEW.status_id = v_published_id AND OLD.status_id != v_published_id THEN
        UPDATE ARTICLE
        SET current_published_version_id = NEW.version_id
        WHERE article_id = NEW.article_id;
    END IF;

END //
DELIMITER ;



-- BEFORE DELETE (take backup in the cases where needed)

DELIMITER //
CREATE TRIGGER trg_beforedelete_article
BEFORE DELETE 
ON ARTICLE
FOR EACH ROW
BEGIN
    -- Just before the article is permanently destroyed, 
    -- we rescue the data and copy it into an Archive table!
    
    INSERT INTO ARTICLE_ARCHIVE (old_article_id, original_author_id, deleted_at)
    VALUES (OLD.article_id, OLD.author_id, CURRENT_TIMESTAMP); 
END //
DELIMITER ;



-- AFTER DELETE ()

DELIMITER //
CREATE TRIGGER trg_afterdelete_user
AFTER DELETE 
ON USER
FOR EACH ROW
BEGIN
    -- The user has been successfully deleted from the database.
    -- Now, we write a "receipt" to our Audit Log using their OLD data.
    
    INSERT INTO USER_AUDIT_LOG (deleted_user_id, deleted_username, deleted_timestamp)
    VALUES (OLD.user_id, OLD.username, CURRENT_TIMESTAMP);
    
END //
DELIMITER ;




-- =======================================================================
-- FILE: 11_Others\05_indexing.sql
-- =======================================================================

-- primary index (clustered)  [already present during creation]
-- ALTER TABLE USER ADD PRIMARY KEY (user_id);

-- unique index (already present during creation)
-- CREATE UNIQUE INDEX idx_unique_email ON USER (email);

-- single column index (secondary) [already present during creation]
-- CREATE INDEX idx_tag_name ON TAG (name);

-- composite index
CREATE INDEX idx_status_date ON ARTICLE_VERSION (status_id, created_at);

SELECT * FROM ARTICLE_VERSION WHERE status_id > 2;
SELECT * FROM ARTICLE_VERSION WHERE status_id > 2 AND created_at > '2026-08-25';
SELECT * FROM ARTICLE_VERSION WHERE created_at > '2026-08-25';

 
-- full-text index
--  Standard indexes cannot search inside long paragraphs. If you use LIKE '%database%', the database does a Full Table Scan. A Full-Text index builds a massive dictionary of every single word in your articles so you can search them like Google.
CREATE FULLTEXT INDEX idx_ft_content ON CONTENT_BLOCK (content_data);


-- Usage:
-- SELECT * FROM CONTENT_BLOCK WHERE MATCH(content_data) AGAINST('SQL architecture');

-- MATCH() AGAINST() is the special syntax you must use to tell the database: use the Full-Text Index to search this paragraph!.

-- MATCH(column_name) tells the database WHERE to look.
-- AGAINST('keyword') tells the database WHAT word to look for.



-- This works in Postgres/SQL Server
-- MySQL does not natively support Partial Indexes. 
CREATE FULLTEXT INDEX idx_ft_content 
ON CONTENT_BLOCK (content_data) 
WHERE block_type_id = 1 OR block_type_id = 2; -- (Only index Text and Code)




-- View existing indexes
SHOW INDEX FROM USER;
-- or
SHOW INDEXES FROM USER


-- Drop a secondary index
DROP INDEX idx_name ON table_name;
-- or
ALTER TABLE table_name DROP INDEX idx_name;


-- Removing primary-key index
ALTER TABLE table_name DROP PRIMARY KEY;;




-- =======================================================================
-- FILE: 12_StoredProcedure\01_prepare-stmt.sql
-- =======================================================================

-- Query:- get articles sorted based on a column and direction

SET @sort_column = 'created_at';
SET @sort_direction = 'DESC';

SET @query = CONCAT(
    'SELECT
        title, created_at
    FROM 
        ARTICLE_VERSION
    WHERE 
        status_id = ?
    ORDER BY
    ',
    @sort_column,
    ' ',
    @sort_direction
);

PREPARE stmt_DynamicSearch FROM @query;
SET @status_id = (SELECT status_id FROM STATUS WHERE status_name = 'Published');
EXECUTE stmt_DynamicSearch USING @status_id;
DEALLOCATE PREPARE stmt_DynamicSearch;




-- Query:- Dynamic Profile Updater
-- 1. The User selects "MONTH" from the frontend dropdown
SET @time_period = 'MONTH'; 

-- 2. Build the query string dynamically to inject the correct time function
SET @dynamic_sql = CONCAT(
    'SELECT ', 
    @time_period, '(created_at) AS time_group, ', -- Becomes: MONTH(created_at)
    'COUNT(*) AS total_articles_published ',
    'FROM ARTICLE_VERSION ',
    'WHERE status_id = ? ',
    'GROUP BY ', @time_period, '(created_at)'    -- Becomes: GROUP BY MONTH(created_at)
);

-- 3. PREPARE the chart query
PREPARE stmt_AnalyticsChart FROM @dynamic_sql;

-- 4. EXECUTE, securely passing the ID for 'Published'
SET @published_status = (SELECT status_id FROM STATUS WHERE status_name = 'Published');
EXECUTE stmt_AnalyticsChart USING @published_status;

-- 5. DEALLOCATE
DEALLOCATE PREPARE stmt_AnalyticsChart;




-- =======================================================================
-- FILE: 12_StoredProcedure\02_temp-table.sql
-- =======================================================================

-- 1. Run the heavy query ONCE and store the result in memory
CREATE TEMPORARY TABLE temp_top_authors AS
SELECT usr.user_id, usr.username, COUNT(article.article_id) AS total_published
    FROM USER usr
JOIN ARTICLE article ON usr.user_id = article.author_id
WHERE 
    article.current_published_version_id IS NOT NULL
GROUP BY 
    usr.user_id, usr.username
ORDER BY 
    total_published DESC
LIMIT 10;

-- 2. OPERATION A: Get the emails of these specific top authors for article newsletter
SELECT username, email 
FROM USER 
WHERE user_id IN (
    SELECT user_id 
    FROM temp_top_authors
);

-- 3. OPERATION B: Calculate the average rating these specific authors received
SELECT temp_top_authors.username, COALESCE(AVG(user_rating.rating_value), 0) AS avg_rating
FROM temp_top_authors temp_top_authors
LEFT JOIN ARTICLE article ON temp_top_authors.user_id = article.author_id
LEFT JOIN USER_RATING user_rating ON article.article_id = user_rating.article_id
GROUP BY temp_top_authors.username;

-- 4. Clean up the session memory
DROP TEMPORARY TABLE temp_top_authors;




-- =======================================================================
-- FILE: 12_StoredProcedure\03_cursor.sql
-- =======================================================================

DELIMITER //

CREATE PROCEDURE sp_CountArticlesByTag_UsingCursor(IN p_tag_name VARCHAR(50))
BEGIN
	-- parameters
    
    DECLARE v_total_count INT DEFAULT 0;
    DECLARE v_current_article_id BINARY(16);
    
    -- We need a flag to know when the loop hits the end of the list
    DECLARE done INT DEFAULT FALSE;
    
    -- 2. DECLARE THE CURSOR: We define the "list" we want to loop through
    DECLARE article_cursor CURSOR FOR 
        SELECT article_tag.article_id 
        FROM ARTICLE_TAG article_tag
        JOIN TAG tag ON article_tag.tag_id = tag.tag_id
        WHERE tag.name = p_tag_name;
        
    -- 3. Declare an error handler: When the cursor runs out of rows, set 'done' to TRUE
    DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;

    -- 4. OPEN the Cursor (loads the list into memory)
    OPEN article_cursor;

    -- 5. Start the foreach loop
    count_loop: LOOP
        
        -- Grab the next row's article_id and put it in our variable
        FETCH article_cursor INTO v_current_article_id;
        
        -- If the 'NOT FOUND' handler triggered, break out of the loop!
        IF done THEN 
            LEAVE count_loop; 
        END IF;

        -- We successfully fetched a row! Add 1 to our manual counter.
        SET v_total_count = v_total_count + 1;

    END LOOP;

    -- 6. CLOSE the Cursor to free up database RAM
    CLOSE article_cursor;

    -- Output the final result to the screen
    SELECT p_tag_name AS TagName, v_total_count AS TotalArticlesFound;

END //
DELIMITER ;


CALL sp_CountArticlesByTag_UsingCursor('SQL');




-- =======================================================================
-- FILE: 13_Views\01_views.sql
-- =======================================================================

-- Views are best used to hide massive, complex JOINs from your backend code, or to pre-calculate statistics so your C# code doesn't have to do the heavy lifting.
-- Clean
-- Reusable

-- Usage:- The "Public Feed" View 
CREATE VIEW vw_PublishedArticles AS
SELECT 
    article.article_id,
    article_version.version_id,
    article_version.title,
    usr.username AS author_name,
    article_version.created_at AS published_date
FROM ARTICLE AS article
JOIN ARTICLE_VERSION article_version 
ON
article.current_published_version_id = article_version.version_id
JOIN USER AS user 
ON
article.author_id = usr.user_id
JOIN STATUS AS status
ON 
article_version.status_id = status.status_id
WHERE status.status_name = 'Published';

SELECT * FROM vw_PublishedArticles;




-- Usage:- The "Analytics/Metrics" View 
CREATE VIEW vw_ArticleMetrics AS
SELECT  
    article_id,
    (SELECT COUNT(*) FROM USER_RATING WHERE article_id = article.article_id) AS total_ratings,
    (SELECT COALESCE(AVG(rating_value),0) FROM USER_RATING WHERE article_id = article.article_id) AS average_rating,
    (SELECT COUNT(*) FROM USER_COMMENT WHERE article_id = article.article_id) AS total_comments
FROM ARTICLE article;

SELECT * FROM vw_ArticleMetrics;




-- =======================================================================
-- FILE: 15_Explain\01_explain.sql
-- =======================================================================

-- When you type EXPLAIN in front of article query, the database does not return the data. Instead, it returns article Query Execution Plan.

-- It is the database's way of opening its brain and telling you: "Here is the exact strategy I am going to use to find the data you asked for."

-- Developers use EXPLAIN when a query is running slowly. By reading the output, you can instantly see if your database is working efficiently, or if it is doing unnecessary heavy lifting.


-- (*) When you run it, you look for three main columns in the output:
-- 1. type: How did it search? If it says ALL, it means a "Full Table Scan" (it read every single row in the table, which is terrible for performance). If it says ref or eq_ref, it used an Index (which is incredibly fast).
-- 2. key: Which Index did it use? If it says NULL, it means the database couldn't find an index and had to search manually.
-- 3. rows: How many rows does the database estimate it has to read to find your answer? If this number is in the millions, your query needs fixing!


EXPLAIN SELECT 
    version_id, content_data 
    FROM CONTENT_BLOCK
    WHERE content_data LIKE '%transaction%';

EXPLAIN SELECT 
    article_version.title, usr.username
    FROM ARTICLE article
    JOIN ARTICLE_VERSION article_version 
    ON 
    article.current_published_version_id = article_version.version_id
    JOIN USER usr 
    ON article.author_id = usr.user_id;

EXPLAIN ANALYZE  -- provides time taken to execute the query
    SELECT article_id, AVG(rating_value) 
    FROM USER_RATING 
    GROUP BY article_id;


-- If EXPLAIN tells you article query is slow (usually by showing type: ALL and key: NULL), you have to step in and fix it.

-- 1. Create an Index
-- 2. Rewrite the Query
-- 3. Add Pagination


-- (1) id -> this is execution step number.
-- (2) select_type = SIMPLE -> it means no join, no subquery, no union, no cte, just straight forward SELECT.
-- (3) table = Customers -> Tells which table MySQL is reading.
-- (4) partitions = NULL -> Your table is not partitioned. Partitioning is an advanced feature where article table is split into multiple physical partitions.
-- (5) type = ref -> type = How is MySQL going to access this table ?. more than one rows expected (normal index = ref) , primary key = const (const = 1 row expected)
-- (6) possible_keys = primaryindex -> index that could be used.
-- (7) key = primaryindex -> After evaluating all possibilities, I chose this index.
-- (8) key_len = 4 -> This tells how many bytes of the index MySQL used. (INT so 4)
-- (9) ref = const -> This tells what MySQL is comparing the indexed column against. (101 is constant value used in query)
-- (10) rows = 1 -> It is MySQL's estimate of how many rows it expects to examine.
-- (11) filtered = 100.00 -> I expect all examined rows to satisfy the WHERE condition.
-- (12) Extra = NULL -> No extra operations required.




-- =======================================================================
-- FILE: 16_Transaction\01_transaction.sql
-- =======================================================================

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