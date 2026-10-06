-- 1. Count how many articles each author has published
SELECT author_id, COUNT(article_id) AS total_articles
FROM ARTICLE GROUP BY author_id;


-- 2. Calculate the average rating for every single article
SELECT 
    article_id, 
    AVG(rating_value) AS average_rating
FROM USER_RATING
GROUP BY article_id;


-- 3. Count how many tags are attached to each article
SELECT 
    article_id, 
    COUNT(tag_id) AS total_tags
FROM ARTICLE_TAG
GROUP BY article_id;