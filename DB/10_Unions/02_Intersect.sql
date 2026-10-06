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