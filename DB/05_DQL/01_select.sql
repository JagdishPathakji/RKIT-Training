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