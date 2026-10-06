-- Views are best used to hide massive, complex JOINs from your backend code, or to pre-calculate statistics so your C# code doesn't have to do the heavy lifting.
-- Clean
-- Reusable

-- Usage:- The "Public Feed" View 
CREATE VIEW vw_PublishedArticles AS
SELECT 
    article.article_id,
    article_version.version_id,
    article_version.title,
    usr.username AS author_name,
    article_version.created_at AS published_date
FROM ARTICLE AS article
JOIN ARTICLE_VERSION article_version 
ON
article.current_published_version_id = article_version.version_id
JOIN USER AS user 
ON
article.author_id = usr.user_id
JOIN STATUS AS status
ON 
article_version.status_id = status.status_id
WHERE status.status_name = 'Published';

SELECT * FROM vw_PublishedArticles;




-- Usage:- The "Analytics/Metrics" View 
CREATE VIEW vw_ArticleMetrics AS
SELECT  
    article_id,
    (SELECT COUNT(*) FROM USER_RATING WHERE article_id = article.article_id) AS total_ratings,
    (SELECT COALESCE(AVG(rating_value),0) FROM USER_RATING WHERE article_id = article.article_id) AS average_rating,
    (SELECT COUNT(*) FROM USER_COMMENT WHERE article_id = article.article_id) AS total_comments
FROM ARTICLE article;

SELECT * FROM vw_ArticleMetrics;