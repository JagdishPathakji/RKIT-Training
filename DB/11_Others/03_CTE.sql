-- Query:- Making complex logic readable
-- We want article list of users and their published articles. Instead of throwing everything into one giant JOIN with article WHERE clause, article CTE will help to make it more cleaner and readable.
USE knowledge_base;

WITH DraftVersion AS (
    SELECT 
        created_by, title, created_at
    FROM ARTICLE_VERSION
    WHERE status_id = (
        SELECT 
            status_id 
        FROM 
            STATUS 
        WHERE 
            status_name = 'Published'
    )
)
SELECT 
    usr.username, draftversion.title AS draft_title, draftversion.created_at 
FROM USER usr
JOIN 
DraftVersion draftversion 
ON
usr.user_id = draftversion.created_by;





-- Query:- Making complex logic readable 
WITH ArticleRatingStats AS (
    SELECT 
        article_id, AVG(rating_value) AS average_rating, COUNT(*) AS total_votes
    FROM USER_RATING
    GROUP BY article_id
)
SELECT 
    article.article_id, usr.username AS author, stats.average_rating
FROM ARTICLE article 
JOIN USER usr
ON
article.author_id = usr.user_id
JOIN
ArticleRatingStats stats 
ON
article.article_id = stats.article_id;