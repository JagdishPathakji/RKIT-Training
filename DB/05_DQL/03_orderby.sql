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