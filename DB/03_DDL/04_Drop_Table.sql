USE knowledge_base;

-- Example: Dropping a table 
-- DROP TABLE IF EXISTS USER_COMMENT;

-- Example: Dropping an Index (Dropping that temporary index)
ALTER TABLE ARTICLE_VERSION DROP INDEX idx_article_title;