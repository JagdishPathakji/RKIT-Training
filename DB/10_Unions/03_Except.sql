-- Query:- Find ghost authors (inactive check)
SELECT user_id 
FROM USER_ROLE 
WHERE role_id = (
    SELECT role_id 
    FROM ROLE 
    WHERE role_name = 'Author'
)

EXCEPT

-- Subtract anyone who actually has a record in the ARTICLE table
SELECT author_id 
FROM ARTICLE;



-- Query:- All article versions currently in 'Pending Editor Review' state
SELECT version_id 
FROM ARTICLE_VERSION 
WHERE status_id = (
    SELECT status_id 
    FROM STATUS 
    WHERE status_name = 'Pending Editor Review'
)

EXCEPT

SELECT version_id 
FROM EDITORIAL_REVIEW 
WHERE editor_id = UUID_TO_BIN('22222222-2222-2222-2222-222222222222', 1);