-- 1. Find Top-Rated Articles based ONLY on Recent Ratings
SELECT  
    article_id,
    AVG(rating_value) AS average_rating,
    COUNT(user_id) AS total_ratings
FROM USER_RATING
WHERE updated_at >= '2026-01-01'
GROUP BY article_id
HAVING AVG(rating_value) > 4.0
ORDER BY average_rating DESC;

