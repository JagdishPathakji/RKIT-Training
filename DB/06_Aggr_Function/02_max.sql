-- 1. Find the latest version number for an article
SELECT MAX(version_number) AS latest_version_number 
FROM ARTICLE_VERSION 
WHERE article_id = UUID_TO_BIN('AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA',1);


-- 2. Find the sequence order for appending a new content block
SELECT MAX(sequence_order) AS last_block_order 
FROM CONTEN	T_BLOCK  WHERE version_id = UUID_TO_BIN('AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA',1);


-- 3. Find the most recent activity on the platform
SELECT MAX(created_at) AS most_recent_comment_time FROM USER_COMMENT;