-- 1. Calculate the total "rating points" an article has received
SELECT article_id, SUM(rating_value) as total_rating
FROM USER_RATING GROUP BY article_id ORDER BY total_rating DESC;