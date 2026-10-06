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