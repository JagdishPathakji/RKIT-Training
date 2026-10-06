-- Query:- Find all the users who are editors 
SELECT 
    username, email 
FROM USER 
WHERE user_id IN (
    SELECT user_id 
    FROM USER_ROLE 
    WHERE role_id = (SELECT role_id FROM ROLE WHERE role_name = 'Editor')
);


-- Query:- Find all articles that belong to the 'Engineering & Technology' category
SELECT 
    article_id, created_at 
FROM ARTICLE 
WHERE article_id IN (
    SELECT article_id 
    FROM ARTICLE_CATEGORY 
    WHERE category_id = (SELECT category_id FROM CATEGORY WHERE name = 'Engineering & Technology')
);


-- Query:- Find article versions that have been rejected by ANY editor.
SELECT 
    version_id, title
FROM ARTICLE_VERSION 
WHERE version_id = ANY (
    SELECT version_id 
    FROM EDITORIAL_REVIEW 
    WHERE decision = 'Reject'
);