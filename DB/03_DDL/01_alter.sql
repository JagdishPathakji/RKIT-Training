USE knowledge_base;

-- Adding a column to USER table
ALTER TABLE USER ADD COLUMN bio TEXT NULL;

-- Allow Floating point ratings in USER_RATING table
ALTER TABLE USER_RATING MODIFY COLUMN rating_value DECIMAL(2,1) NOT NULL;

-- Adding Sample Indexing (temporary purpose just to show ALTER query demo [delete later])
ALTER TABLE ARTICLE_VERSION ADD INDEX idx_article_title (title);