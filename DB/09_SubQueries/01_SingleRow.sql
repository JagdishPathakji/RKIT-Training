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