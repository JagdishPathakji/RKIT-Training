-- 1. first registered user
SELECT MIN(created_at) AS "FIRST USER" 
FROM USER;


-- 2. Find the lowest rating a specific article has ever received.
SELECT MIN(rating_value) AS lowest_rating 
FROM USER_RATING 
WHERE article_id = UUID_TO_BIN('AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA',1);