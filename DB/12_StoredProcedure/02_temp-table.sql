-- 1. Run the heavy query ONCE and store the result in memory
CREATE TEMPORARY TABLE temp_top_authors AS
SELECT usr.user_id, usr.username, COUNT(article.article_id) AS total_published
    FROM USER usr
JOIN ARTICLE article ON usr.user_id = article.author_id
WHERE 
    article.current_published_version_id IS NOT NULL
GROUP BY 
    usr.user_id, usr.username
ORDER BY 
    total_published DESC
LIMIT 10;

-- 2. OPERATION A: Get the emails of these specific top authors for article newsletter
SELECT username, email 
FROM USER 
WHERE user_id IN (
    SELECT user_id 
    FROM temp_top_authors
);

-- 3. OPERATION B: Calculate the average rating these specific authors received
SELECT temp_top_authors.username, COALESCE(AVG(user_rating.rating_value), 0) AS avg_rating
FROM temp_top_authors temp_top_authors
LEFT JOIN ARTICLE article ON temp_top_authors.user_id = article.author_id
LEFT JOIN USER_RATING user_rating ON article.article_id = user_rating.article_id
GROUP BY temp_top_authors.username;

-- 4. Clean up the session memory
DROP TEMPORARY TABLE temp_top_authors;