-- Query:- Get a combined timeline of a user's comments and ratings
SELECT 
    'Left a Comment' AS activity_type, article_id, created_at AS activity_date
FROM USER_COMMENT 
WHERE user_id = UUID_TO_BIN('33333333-3333-3333-3333-333333333333', 1)

UNION 

SELECT 
    'Gave a Rating' AS activity_type, article_id, updated_at AS activity_date
FROM USER_RATING 
WHERE user_id = UUID_TO_BIN('33333333-3333-3333-3333-333333333333', 1)

ORDER BY activity_date DESC;


-- Query:- All metrics in one view
SELECT 
    'Total Users' AS metric_name, COUNT(*) AS metric_value 
FROM USER

UNION

SELECT 
    'Total Published Articles' AS metric_name, COUNT(*) AS metric_value 
FROM ARTICLE_VERSION 
WHERE status_id = (SELECT status_id FROM STATUS WHERE status_name = 'Published')

UNION

SELECT 
    'Total Comments' AS metric_name, COUNT(*) AS metric_value 
FROM USER_COMMENT;