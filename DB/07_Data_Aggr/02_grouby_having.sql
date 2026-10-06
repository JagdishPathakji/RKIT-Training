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